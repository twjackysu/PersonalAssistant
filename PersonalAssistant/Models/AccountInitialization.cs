using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAssistant.Models
{
    public class AccountInitialization
    {
        [Key]
        public int AccountInitializationID { get; set; }

        public string OwnerID { get; set; }

        public DateTime EffectiveDate { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Balance { get; set; }
    }
}
