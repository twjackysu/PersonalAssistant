using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Services;
using PersonalAssistant.Extension;
using PersonalAssistant.Models.AccountManager;
using StockLib;
using System.Runtime.CompilerServices;
using PersonalAssistant.DTO.Dashboard;
using AngleSharp.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalAssistant.Controllers
{
    [Authorize]
    [Route("api/[action]")]
    [ApiController]
    public class NotRestfulAPIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotRestfulAPIController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IStockDB stockDB;
        private readonly IStockInfoBuilder stockInfoBuilder;
        private readonly IDashboardHelper dashboardHelper;
        public NotRestfulAPIController(ILogger<NotRestfulAPIController> logger, ApplicationDbContext context, IConfiguration configuration, IStockDB stockDB
            , IStockInfoBuilder stockInfoBuilder, IDashboardHelper dashboardHelper)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            this.stockDB = stockDB;
            this.stockInfoBuilder = stockInfoBuilder;
            this.dashboardHelper = dashboardHelper;
        }
        //1. 與上月相比資產增減幾%
        //2. 目前資產狀況
        //3. 歷史資產狀況
        [HttpGet]
        public async Task<IActionResult> GetHistoryBalance()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            //月份，<帳戶, 總值>
            //dictionary<string, decimal>()
            var oneYearDict = new Dictionary<string, List<Asset>>();
            var utcNow = DateTime.UtcNow;

            var start = utcNow.AddYears(-1).AddMonths(-1);
            start = new DateTime(start.Year, start.Month, 1, 8, 0, 0);
            var end = start.AddMonths(1);
            var accounts = new Dictionary<int, AccountInitialization>();
            var accountsBalance = new Dictionary<int, decimal>();

            var stocksAmount = new Dictionary<(Models.AccountManager.StockCategory type, string stockNo), int>();
            bool isFirst = true;
            while (start < utcNow)
            {
                var _start = isFirst ? (DateTime?)null : start;
                #region 帳戶
                var thisMonthAccountChanges = dashboardHelper.GetDateRangeAccountChanges(userID, end, _start);
                var thisMonthAccounts = dashboardHelper.GetDateRangeAccountsInit(userID, end, _start);
                //增加那個月份初始化的account
                foreach(var account in thisMonthAccounts)
                {
                    if (account.ID.HasValue)
                    {
                        accountsBalance.Add(account.ID.Value, account.Balance);
                        accounts.Add(account.ID.Value, account);
                    }
                }
                foreach (var change in thisMonthAccountChanges)
                {
                    if (accountsBalance.ContainsKey(change.Key))
                    {
                        accountsBalance[change.Key] += change.Value;
                    }
                }
                var list = new List<Asset>();
                list = list.Concat(accountsBalance.Select(x => new Asset() { Name = accounts[x.Key].Name, Balance = x.Value })).ToList();
                #endregion
                #region 股票
                var thisMonthStockChanges = dashboardHelper.GetDateRangeStockChanges(userID, end, _start);
                var thisMonthStocks = dashboardHelper.GetDateRangeStocksInit(userID, end, _start);
                foreach (var stock in thisMonthStocks)
                {
                    if (!stocksAmount.ContainsKey(stock.Key))
                    {
                        stocksAmount.Add(stock.Key, stock.Value);
                    }
                    else
                    {
                        //考慮拿掉，因為股票初始化不應該含有兩個相同股票代號(可再新增時前後端擋)
                        stocksAmount[stock.Key] += stock.Value;
                    }
                }
                foreach (var change in thisMonthStockChanges)
                {
                    if (stocksAmount.ContainsKey(change.Key))
                    {
                        stocksAmount[change.Key] += change.Value;
                    }
                    else
                    {
                        stocksAmount.Add(change.Key, change.Value);
                    }
                }
                var stockPrice = await dashboardHelper.GetStockPrice(stocksAmount, end.AddDays(-1));
                list = list.Concat(stockPrice.Select(x => new Asset() { Name = x.Name, Balance = x.Balance, Date = x.Date })).ToList();
                oneYearDict.Add(start.ToString("yyyy-MM"), list);
                #endregion
                start = new DateTime(start.Year, start.Month, 1, 8, 0, 0).AddMonths(1);
                end = start.AddMonths(1);
                isFirst = false;
            }
            return Ok(oneYearDict);
        }
        [HttpGet]
        public IActionResult GetLatestAccountBalance()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();

            var utcNow = DateTime.UtcNow;
            var result = dashboardHelper.GetCumulativeAccountBalance(userID, utcNow);
            return Ok(result.Select(x => new Asset() { Name = x.Name, Balance = x.Balance }));
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestStockValue()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var result = await dashboardHelper.GetCumulativeStockValue(userID);
            return Ok(result.Select(x => new Asset() { Name = x.Name, Balance = x.Balance, Date = x.Date }));
        }

        [HttpGet]
        public IActionResult Get1MonthCost()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var utcNow = DateTime.UtcNow;
            var before1Month = utcNow.AddMonths(-1);
            var result = dashboardHelper.GetDateRangeCostByExpenditureType(userID, utcNow, before1Month);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAvgEveryDayCost()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var utcNow = DateTime.UtcNow;
            var result = dashboardHelper.GetDateRangeAvgEveryDayCost(userID, utcNow);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Get1MonthIncome()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var utcNow = DateTime.UtcNow;
            var before1Month = utcNow.AddMonths(-1);
            var sum1MonthIncome = _context.Income.Where(x => x.OwnerID == userID
            && x.EffectiveDate <= utcNow
            && x.EffectiveDate >= before1Month)
                .Sum(x => x.Amount);
            return Ok(sum1MonthIncome);
        }

        [HttpGet]
        public IActionResult GetAvgEveryDayIncome()
        {
            var userID = User.GetSID();
            if (userID == null)
                return Forbid();
            var utcNow = DateTime.UtcNow;
            var startDate = _context.Income
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= utcNow)
                .Select(x => x.EffectiveDate)
                .OrderBy(x => x)
                .FirstOrDefault();
            if (startDate == default)
                return Ok(0);
            var sumIncome = _context.Income.Where(x => x.OwnerID == userID
            && x.EffectiveDate <= utcNow)
                .Sum(x => x.Amount);
            var everyDayIncome = Math.Round(sumIncome / (decimal)(utcNow - startDate).TotalDays, 2);
            return Ok(everyDayIncome);
        }

    }
}
