using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAssistant.Models
{
    public class Transaction
    {
        [Key]
        public int? ID { get; set; }

        public string OwnerID { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        public int AccountID { get; set; }

        public AccountInitialization Account { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        public int? TransferIntoAccountID { get; set; }

        public AccountInitialization TransferIntoAccount { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue)]
        [Required]
        public decimal Money { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal? Fees { get; set; }

        public string Remarks { get; set; }

        public int? ExpenditureTypeID { get; set; }

        public ExpenditureType ExpenditureType { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expenditure,
        InternalTransfer
    }
}
