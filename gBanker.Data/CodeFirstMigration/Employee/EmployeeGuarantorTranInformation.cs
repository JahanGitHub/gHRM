using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeGuarantorTranInformation")]
    public class EmployeeGuarantorTranInformation
    {
       [Key]
        public long ID { get; set; }

        [Required]
        public long EmployeeID { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [Required]
        public DateTime? TransactionDate { get; set; }

        [Required]
        public decimal? TransactionAmount { get; set; }
        
        public string PaymentType { get; set; }
       
        public string BankName { get; set; }

        public string BranchName { get; set; }

             
        public string AccountNo { get; set; }
        
        public string ChequeNo { get; set; }
         
        public bool? IsRemoved { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreatedBy { get; set; }

        public long? UpdateBy { get; set; }

    }
}
