using PersonalAssistant.Models.AccountManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalAssistant.Services
{
    public interface IDashboardHelper
    {
        IEnumerable<(string Name, decimal Balance)> GetCumulativeAccountBalance(string userID, DateTime end);
        /// <summary>
        /// 結算股價價值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">結算時間，給當天的話，如果當天開盤要等13:30後才會得到最新價錢，如果想要即時股價請設null</param>
        /// <returns></returns>
        Task<IEnumerable<(string Name, decimal Balance, string Type, string Date)>> GetCumulativeStockValue(string userID, DateTime? end = null);
        Dictionary<string, decimal> GetDateRangeCostByExpenditureType(string userID, DateTime end, DateTime? start = null);
        Dictionary<string, decimal> GetDateRangeAvgEveryDayCost(string userID, DateTime end, DateTime? start = null);
        /// <summary>
        /// 獲得指定時間範圍的帳戶總收入和總支出的總和
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        Dictionary<int, decimal> GetDateRangeAccountChanges(string userID, DateTime end, DateTime? start = null);
        /// <summary>
        /// 獲得指定時間範圍的新帳戶
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        List<AccountInitialization> GetDateRangeAccountsInit(string userID, DateTime end, DateTime? start = null);
        /// <summary>
        /// 獲得指定時間範圍的初始化股票
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        Dictionary<(StockCategory type, string stockNo), int> GetDateRangeStocksInit(string userID, DateTime end, DateTime? start = null);
        /// <summary>
        /// 獲得指定時間範圍的股票總買進和總賣出的數量總和
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="end">小於End</param>
        /// <param name="start">大於等於Start</param>
        /// <returns></returns>
        Dictionary<(StockCategory type, string stockNo), int> GetDateRangeStockChanges(string userID, DateTime end, DateTime? start = null);

        /// <summary>
        /// 獲得指定日期的股票價格
        /// </summary>
        /// <param name="stockAmount"></param>
        /// <param name="end">不給end會獲得最即時的價格</param>
        /// <returns></returns>
        Task<IEnumerable<(string Name, decimal Balance, string Type, string Date)>> GetStockPrice(Dictionary<(StockCategory type, string stockNo), int> stockAmount, DateTime? end);
    }
}
