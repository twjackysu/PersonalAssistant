using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class InternalTransfer
    {
        [Key]
        public int? ID { get; set; }

        [Required]
        public string OwnerID { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public int AccountID { get; set; }

        public AccountInitialization Account { get; set; }

        [Required]
        public decimal OutAmount { get; set; }

        public int? Fees { get; set; }

        [Required]
        public int TransferIntoAccountID { get; set; }

        public AccountInitialization TransferIntoAccount { get; set; }

        [Required]
        public decimal InAmount { get; set; }

        public string Remarks { get; set; }
    }
}
