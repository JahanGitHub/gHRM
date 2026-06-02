using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.ApprovalMaster")]
    public class ApprovalMaster
    {
        [Key]
        public int ApprovalMasterId { get; set; }
        public string FormName { get; set; }
        public string LoanType { get; set; }
        public int? TotalLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
    }
}
