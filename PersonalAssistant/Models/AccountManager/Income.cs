using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class Income
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

        public int? IncomeCategoryID { get; set; }
        public IncomeCategory IncomeCategory { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Remarks { get; set; }
    }
}
