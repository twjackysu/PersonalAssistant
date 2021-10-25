using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class StockInitialization
    {

        [Key]
        public int? ID { get; set; }

        [StringLength(50)]
        public string OwnerID { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public StockCategory Category { get; set; }

        [StringLength(10)]
        [Required]
        public string StockCode { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Amount { get; set; }
    }
}
