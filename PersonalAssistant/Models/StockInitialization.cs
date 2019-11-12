using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models
{
    public class StockInitialization
    {

        [Key]
        public int? ID { get; set; }

        public string OwnerID { get; set; }

        [StringLength(10)]
        [Required]
        public string StockCode { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
