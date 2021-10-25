using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAssistant.Models.AccountManager
{
    public class Expenditure
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

        [Required]
        public decimal Amount { get; set; }

        public decimal? Fees { get; set; }

        public string Remarks { get; set; }
        public int? ExpenditureWayID { get; set; }
        public ExpenditureWay ExpenditureWay { get; set; }

        public int? ExpenditureTypeID { get; set; }

        public ExpenditureType ExpenditureType { get; set; }
    }
}
