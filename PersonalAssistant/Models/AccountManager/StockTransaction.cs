using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class StockTransaction
    {
        [Key]
        public int? ID { get; set; }

        [StringLength(50)]
        public string OwnerID { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public int AccountID { get; set; }

        public AccountInitialization Account { get; set; }

        [StringLength(10)]
        [Required]
        public string StockCode { get; set; }

        [Required]
        public StockCategory Type { get; set; }

        [Required]
        public StockTransactionType TransactionType { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Amount { get; set; }

        public decimal? Fees { get; set; }
    }
    public enum StockTransactionType
    {
        Buy,
        Sell
    }
}
