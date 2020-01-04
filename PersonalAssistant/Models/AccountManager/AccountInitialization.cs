using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class AccountInitialization
    {
        [Key]
        public int? ID { get; set; }

        [StringLength(50)]
        [Required]
        public string OwnerID { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        public ISO4217_CurrencyCode Currency { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
