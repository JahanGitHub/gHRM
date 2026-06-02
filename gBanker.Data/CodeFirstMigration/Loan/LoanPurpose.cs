using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.LoanPurpose")]
    public class LoanPurpose
    {
        [Key]
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        public string LoanType { get; set; }
        public string MethodType { get; set; }// D=Decline Method,F=Flat Method
        //public int ComponentId { get; set; }
        //public int InterestRate { get; set; } 
        public int GracePeriod { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
