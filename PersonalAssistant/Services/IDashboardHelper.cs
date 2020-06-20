using PersonalAssistant.Models.AccountManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Services
{
    public interface IDashboardHelper
    {
        IEnumerable<(string Name, decimal Balance)> GetCumulativeAccountBalance(string userID, DateTime end);
        Task<IEnumerable<(string Name, decimal Balance, string Date)>> GetCumulativeStockValue(string userID, DateTime? end = null);
        Dictionary<string, decimal> GetDateRangeCostByExpenditureType(string userID, DateTime end, DateTime? start = null);
        Dictionary<string, decimal> GetDateRangeAvgEveryDayCost(string userID, DateTime end, DateTime? start = null);
        Dictionary<int, decimal> GetDateRangeAccountChanges(string userID, DateTime end, DateTime? start = null);
        List<AccountInitialization> GetDateRangeAccountsInit(string userID, DateTime end, DateTime? start = null);
        Dictionary<string, int> GetDateRangeStocksInit(string userID, DateTime end, DateTime? start = null);
        Dictionary<string, int> GetDateRangeStockChanges(string userID, DateTime end, DateTime? start = null);
        Task<IEnumerable<(string Name, decimal Balance, string Date)>> GetStockPrice(Dictionary<string, int> stockAmount, DateTime? end);
    }
}
