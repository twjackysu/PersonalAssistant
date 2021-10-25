using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAssistant.Models.AccountManager
{
    public class InternalTransfer
    {
        [Key]
        public int? ID { get; set; }

        [StringLength(50)]
        public string OwnerID { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public int AccountID { get; set; }

        [ForeignKey("AccountID")]
        public AccountInitialization Account { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int? Fees { get; set; }

        [Required]
        public int TransferIntoAccountID { get; set; }

        [ForeignKey("TransferIntoAccountID")]
        public AccountInitialization TransferIntoAccount { get; set; }

        public string Remarks { get; set; }
    }
}
