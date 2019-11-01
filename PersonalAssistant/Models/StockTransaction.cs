using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Models
{
    public class StockTransaction
    {
        [Key]
        public int StockTransactionID { get; set; }

        public string OwnerID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int AccountID { get; set; }

        public AccountInitialization Account { get; set; }

        [Required]
        public string StockCode { get; set; }

        [Required]
        public StockTransactionType Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal? Fees { get; set; }
    }
    public enum StockTransactionType
    {
        Buy,
        Sell
    }
}
