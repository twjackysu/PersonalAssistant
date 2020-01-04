using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class Expenditure
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
        public decimal Amount { get; set; }

        public decimal? Fees { get; set; }

        public string Remarks { get; set; }

        public int? ExpenditureTypeID { get; set; }

        public ExpenditureType ExpenditureType { get; set; }
    }
}
