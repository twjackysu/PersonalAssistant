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

        public async Task<IEnumerable<(string Name, decimal Balance, string Type, string Date)>> GetCumulativeStockValue(string userID, DateTime? end = null)
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
                if (stockTransaction.TransactionType == StockTransactionType.Buy)
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

        public Dictionary<(Models.AccountManager.StockCategory type, string stockNo), int> GetDateRangeStockChanges(string userID, DateTime end, DateTime? start = null)
        {
            var sumBuy = context.StockTransaction.Where(x => x.OwnerID == userID && x.TransactionType == StockTransactionType.Buy &&
                                                             x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                                                            .AsEnumerable()
                                                            .GroupBy(x => new { x.StockCode, x.Type })
                                                            .ToDictionary(g => (g.Key.Type, g.Key.StockCode), g => g.Sum(x => x.Amount));

            var sumSell = context.StockTransaction.Where(x => x.OwnerID == userID && x.TransactionType == StockTransactionType.Sell &&
                                                              x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start))
                                                              .AsEnumerable()
                                                              .GroupBy(x => new { x.StockCode, x.Type })
                                                              .ToDictionary(g => (g.Key.Type, g.Key.StockCode), g => g.Sum(x => x.Amount));
            var nowStockAmount = new Dictionary<(Models.AccountManager.StockCategory type, string stockNo), int>();
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

        public List<AccountInitialization> GetDateRangeAccountsInit(string userID, DateTime end, DateTime? start = null)
        {
            return context.AccountInitialization.Where(x => x.OwnerID == userID && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToList();
        }

        public Dictionary<(Models.AccountManager.StockCategory type, string stockNo), int> GetDateRangeStocksInit(string userID, DateTime end, DateTime? start = null)
        {
            return context.StockInitialization.Where(x => x.OwnerID == userID && x.EffectiveDate < end && (!start.HasValue || x.EffectiveDate >= start)).ToDictionary(x => (x.Category, x.StockCode), x => x.Amount);
        }

        public async Task<IEnumerable<(string Name, decimal Balance, string Type, string Date)>> GetStockPrice(Dictionary<(Models.AccountManager.StockCategory type, string stockNo), int> stockAmount, DateTime? end)
        {
            stockAmount = stockAmount.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
            if (end.HasValue)
            {
                var latestStocksPrice = await stockDB.GetDateStocksPrice(end.Value, stockAmount.Select(x => x.Key.stockNo).Distinct().ToArray());
                return stockAmount.Select(x =>
                {
                    var stock = latestStocksPrice[x.Key.stockNo];
                    return (
                        Name: $"{x.Key.stockNo} {stock.Name}",
                        Balance: stock.ClosingPrice * stockAmount[x.Key],
                        Type: x.Key.type.ToString(),
                        Date: stock.Date.ToString("yyyy-MM-dd")
                    );
                });
            }
            else
            {
                var latestStocksPrice = await stockDB.GetLatestStocksPrice(stockAmount.Select(x => x.Key.stockNo).Distinct().ToArray());

                var stocksInfo = await stockInfoBuilder.GetStocksInfo(stockAmount.Select(x => (latestStocksPrice[x.Key.stockNo].Type, x.Key.stockNo)).ToArray());
                if (stocksInfo == null)
                {
                    return stockAmount.Select(x =>
                    {
                        var stock = latestStocksPrice[x.Key.stockNo];
                        return (
                            Name: $"{x.Key.stockNo} {stock.Name}",
                            Balance: stock.ClosingPrice * stockAmount[x.Key],
                            Type: x.Key.type.ToString(),
                            Date: stock.Date.ToString("yyyy-MM-dd")
                        );
                    });
                }
                else
                {
                    return stockAmount.Select(x =>
                    {
                        var stock = stocksInfo[x.Key.stockNo];
                        return (
                            Name: $"{x.Key.stockNo} {stock.Name}",
                            Balance: (decimal)((stock.LastTradedPrice ?? stock.YesterdayClosingPrice) * x.Value),
                            Type: x.Key.type.ToString(),
                            Data: default(string)
                        );
                    });
                }
            }
        }
    }
}
