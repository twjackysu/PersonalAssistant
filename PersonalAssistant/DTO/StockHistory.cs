using StockLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.DTO
{
    public class StockHistory
    {
        public uint StockHistoryID { get; set; }
        /// <summary>代號/summary>
        public string No { get; set; }
        /// <summary>類型是TSE或OTC/summary>
        public StockType Type { get; set; }
        public string Name { get; set; }
        /// <summary>日期</summary>
        public DateTime Date { get; set; }
        /// <summary>成交股數</summary>
        public uint TradeVolume { get; set; }
        /// <summary>成交金額</summary>
        public decimal TurnOverInValue { get; set; }
        /// <summary>開盤價</summary>
        public decimal OpeningPrice { get; set; }
        /// <summary>最高價</summary>
        public decimal HighestPrice { get; set; }
        /// <summary>最低價</summary>
        public decimal LowestPrice { get; set; }
        /// <summary>收盤價</summary>
        public decimal ClosingPrice { get; set; }
        /// <summary>漲跌價差</summary>
        public string DailyPricing { get; set; }
        /// <summary>成交筆數</summary>
        public uint NumberOfDeals { get; set; }
    }
}
