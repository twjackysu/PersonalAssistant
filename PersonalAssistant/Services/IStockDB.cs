using PersonalAssistant.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Services
{
    public interface IStockDB
    {
        /// <summary>
        /// 獲得最新的股票價格
        /// </summary>
        /// <param name="stockNo">股票編號</param>
        Task<Dictionary<string, StockHistory>> GetLatestStocksPrice(params string[] stockNo);

        /// <summary>
        /// 獲得指定日期的股票價格
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="stockNo">股票編號</param>
        Task<Dictionary<string, StockHistory>> GetDateStocksPrice(DateTime date, params string[] stockNo);
    }
}
