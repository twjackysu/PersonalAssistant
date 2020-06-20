using AngleSharp.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalAssistant.Data;
using PersonalAssistant.Models.AccountManager;
using StockLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Services
{
    public class DashboardHelper : IDashboardHelper
    {
        private readonly ILogger<DashboardHelper> logger;
        private readonly ApplicationDbContext context;
        private readonly IStockDB stockDB;
        private readonly IStockInfoBuilder stockInfoBuilder;
        public DashboardHelper(ILogger<DashboardHelper> logger, ApplicationDbContext context, IStockDB stockDB, IStockInfoBuilder stockInfoBuilder)
        {
            this.logger = logger;
            this.context = context;
            this.stockDB = stockDB;
            this.stockInfoBuilder = stockInfoBuilder;
        }
        public IEnumerable<(string Name, decimal Balance)> GetCumulativeAccountBalance(string userID, DateTime end)
        {
            var changes = GetDateRangeAccountChanges(userID, end);
            var accounts = GetDateRangeAccountsInit(userID, end);
            return accounts.Select(x => {
                var balance = x.Balance;
                if (changes.ContainsKey(x.ID.Value))
                {
                    balance += changes[x.ID.Value];
                }
                return ( x.Name, balance );
            });
        }

        /// <summary>
        /// 結算股價價值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">結算時間，給當天的話，如果當天開盤要等13:30後才會得到最新價錢，如果想要即時股價請設null</param>
        /// <returns></returns>
        public async Task<IEnumerable<(string Name, decimal Balance, string Date)>> GetCumulativeStockValue(string userID, DateTime? end = null)
        {
            DateTime _end = DateTime.UtcNow;
            if (end.HasValue)
            {
                _end = end.Value;
            }
            var nowStockAmount = GetDateRangeStocksInit(userID, _end);
            var changes = GetDateRangeStockChanges(userID, _end);
            foreach (var change in changes)
            {
                if (nowStockAmount.ContainsKey(change.Key))
                {
                    nowStockAmount[change.Key] += change.Value;
                }
                else
                {
                    nowStockAmount.Add(change.Key, change.Value);
                }
            }
            return await GetStockPrice(nowStockAmount, end);
        }
        
        public Dictionary<string, decimal> GetDateRangeCostByExpenditureType(string userID, DateTime end, DateTime? start = null)
        {
            var types = context.ExpenditureType.Where(x => x.OwnerID == userID).ToDictionary(x => x.ID, x => x.TypeName);
            var totalExpenditure = context.Expenditure
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end && (!start.HasValue || x.EffectiveDate >= start.Value))
                .AsEnumerable()
                .GroupBy(x => x.ExpenditureTypeID)
                .ToDictionary(g => types[g.Key], g => g.Sum(x => x.Amount) + g.Sum(x => x.Fees ?? 0));
            var sumStockFee = context.StockTransaction
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end && (!start.HasValue || x.EffectiveDate >= start.Value))
                .Sum(x => x.Fees ?? 0);
            var sumInternalTransferFee = context.InternalTransfer
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end && (!start.HasValue || x.EffectiveDate >= start.Value))
                .Sum(x => x.Fees ?? 0);
            totalExpenditure.Add("Fee", sumStockFee + sumInternalTransferFee);
            return totalExpenditure;
        }
        //日期範圍有點錯誤，要改
        public Dictionary<string, decimal> GetDateRangeAvgEveryDayCost(string userID, DateTime end, DateTime? start = null)
        {
            if (!start.HasValue)
            {
                var expenditureStartDate = context.Expenditure.Where(x => x.OwnerID == userID).Select(x => x.EffectiveDate).OrderBy(x => x).FirstOrDefault();
                var stockTransactionStartDate = context.StockTransaction.Where(x => x.OwnerID == userID).Select(x => x.EffectiveDate).OrderBy(x => x).FirstOrDefault();
                if (expenditureStartDate == default && stockTransactionStartDate == default)
                    return new Dictionary<string, decimal>();
                if (expenditureStartDate == default)
                    start = stockTransactionStartDate;
                else if (stockTransactionStartDate == default)
                    start = expenditureStartDate;
                else
                    start = expenditureStartDate < stockTransactionStartDate ? expenditureStartDate : stockTransactionStartDate;
            }
            var daysCount = (end - start.Value).TotalDays;

            var types = context.ExpenditureType.Where(x => x.OwnerID == userID).ToDictionary(x => x.ID, x => x.TypeName);
            var totalExpenditure = context.Expenditure
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end)
                .AsEnumerable()
                .GroupBy(x => x.ExpenditureTypeID)
                .ToDictionary(g => types[g.Key], g => g.Sum(x => x.Amount) + g.Sum(x => x.Fees ?? 0));
            var sumStockFee = context.StockTransaction
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end)
                .Sum(x => x.Fees ?? 0);
            var sumInternalTransferFee = context.InternalTransfer
                .Where(x => x.OwnerID == userID && x.EffectiveDate <= end)
                .Sum(x => x.Fees ?? 0);
            totalExpenditure.Add("Fee", sumStockFee + sumInternalTransferFee);
            return totalExpenditure.ToDictionary(x => x.Key, x => Math.Round(totalExpenditure[x.Key] / (decimal)daysCount, 2));
        }

        /// <summary>
        /// 獲得指定時間範圍的帳戶總收入和總支出的總和
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        public Dictionary<int, decimal> GetDateRangeAccountChanges(string userID, DateTime end, DateTime? start = null)
        {
            var totalIncome = context.Income.Where(x => x.OwnerID == userID && x.EffectiveDate >= x.Account.EffectiveDate && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                .AsEnumerable()
                .GroupBy(x => x.AccountID)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            var totalExpenditure = context.Expenditure.Where(x => x.OwnerID == userID && x.EffectiveDate >= x.Account.EffectiveDate && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                .AsEnumerable()
                .GroupBy(x => x.AccountID)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount) + g.Sum(x => x.Fees ?? 0));

            void addFunc(Dictionary<int, decimal> totalDict, int id, decimal number)
            {
                if (totalDict.ContainsKey(id))
                {
                    totalDict[id] += number;
                }
                else
                {
                    totalDict.Add(id, number);
                }
            }

            var transfers = context.InternalTransfer.Where(x => x.OwnerID == userID && x.EffectiveDate >= x.Account.EffectiveDate && x.EffectiveDate >= x.TransferIntoAccount.EffectiveDate && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToList();
            foreach (var transfer in transfers)
            {
                var expenditure = transfer.Amount + (transfer.Fees ?? 0);
                addFunc(totalExpenditure, transfer.AccountID, expenditure);
                addFunc(totalIncome, transfer.TransferIntoAccountID, transfer.Amount);
            }
            var stockTransactions = context.StockTransaction.Where(x => x.OwnerID == userID && x.EffectiveDate >= x.Account.EffectiveDate && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToList();
            foreach (var stockTransaction in stockTransactions)
            {
                if (stockTransaction.Type == StockTransactionType.Buy)
                {
                    var expenditure = stockTransaction.Price * stockTransaction.Amount + (stockTransaction.Fees ?? 0);

                    addFunc(totalExpenditure, stockTransaction.AccountID, expenditure);
                }
                else
                {
                    addFunc(totalIncome, stockTransaction.AccountID, stockTransaction.Price * stockTransaction.Amount);
                    var fees = stockTransaction.Fees ?? 0;
                    addFunc(totalExpenditure, stockTransaction.AccountID, fees);
                }
            }
            var clone = totalIncome.ToDictionary(x => x.Key, x => x.Value);
            foreach(var totalExpenditurePair in totalExpenditure)
            {
                if (clone.ContainsKey(totalExpenditurePair.Key))
                {
                    clone[totalExpenditurePair.Key] -= totalExpenditurePair.Value;
                }
                else
                {
                    clone.Add(totalExpenditurePair.Key, -totalExpenditurePair.Value);
                }
            }
            return clone;
        }

        /// <summary>
        /// 獲得指定時間範圍的股票總買進和總賣出的數量總和
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        public Dictionary<string, int> GetDateRangeStockChanges(string userID, DateTime end, DateTime? start = null)
        {
            var sumBuy = context.StockTransaction.Where(x => x.OwnerID == userID && x.Type == StockTransactionType.Buy && 
                                                             x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                                                            .AsEnumerable()
                                                            .GroupBy(x => x.StockCode)
                                                            .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            var sumSell = context.StockTransaction.Where(x => x.OwnerID == userID && x.Type == StockTransactionType.Sell && 
                                                              x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                                                              .AsEnumerable()
                                                              .GroupBy(x => x.StockCode)
                                                              .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
            var nowStockAmount = new Dictionary<string, int>();
            foreach (var pair in sumBuy)
            {
                if (nowStockAmount.ContainsKey(pair.Key))
                {
                    nowStockAmount[pair.Key] += pair.Value;
                }
                else
                {
                    nowStockAmount.Add(pair.Key, pair.Value);
                }
            }
            foreach (var pair in sumSell)
            {
                if (nowStockAmount.ContainsKey(pair.Key))
                {
                    nowStockAmount[pair.Key] -= pair.Value;
                }
                else
                {
                    nowStockAmount.Add(pair.Key, -pair.Value);
                }
            }
            return nowStockAmount;
        }

        /// <summary>
        /// 獲得指定時間範圍的新帳戶
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        public List<AccountInitialization> GetDateRangeAccountsInit(string userID, DateTime end, DateTime? start = null)
        {
            return context.AccountInitialization.Where(x => x.OwnerID == userID && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToList();
        }
        /// <summary>
        /// 獲得指定時間範圍的初始化股票
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        public Dictionary<string, int> GetDateRangeStocksInit(string userID, DateTime end, DateTime? start = null)
        {
            return context.StockInitialization.Where(x => x.OwnerID == userID && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToDictionary(x => x.StockCode, x => x.Amount);
        }

        /// <summary>
        /// 獲得指定日期的股票價格
        /// </summary>
        /// <param name="stockAmount"></param>
        /// <param name="end">不給end會獲得最即時的價格</param>
        /// <returns></returns>
        public async Task<IEnumerable<(string Name, decimal Balance, string Date)>> GetStockPrice(Dictionary<string, int> stockAmount, DateTime? end)
        {
            stockAmount = stockAmount.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);

            if (end.HasValue)
            {
                var latestStocksPrice = await stockDB.GetDateStocksPrice(end.Value, stockAmount.Select(x => x.Key).ToArray());
                return latestStocksPrice.Select(x => (
                    Name: $"{x.Key} {x.Value.Name}",
                    Balance: x.Value.ClosingPrice * stockAmount[x.Key],
                    Date: x.Value.Date.ToString("yyyy-MM-dd")
                ));
            }
            else
            {
                var latestStocksPrice = await stockDB.GetLatestStocksPrice(stockAmount.Select(x => x.Key).ToArray());

                var stocksInfo = await stockInfoBuilder.GetStocksInfo(stockAmount.Select(x => (latestStocksPrice[x.Key].Type, x.Key)).ToArray());
                if (stocksInfo == null)
                {
                    return latestStocksPrice.Select(x => (
                        Name: $"{x.Key} {x.Value.Name}",
                        Balance: x.Value.ClosingPrice * stockAmount[x.Key],
                        Date: x.Value.Date.ToString("yyyy-MM-dd")
                    ));
                }
                else
                {
                    return stocksInfo.Select(x => (
                        Name: $"{x.No} {x.Name}",
                        Balance: (decimal)((x.LastTradedPrice ?? x.YesterdayClosingPrice) * stockAmount[x.No]),
                        Data: default(string)
                    ));
                }
            }
        }
    }
}
