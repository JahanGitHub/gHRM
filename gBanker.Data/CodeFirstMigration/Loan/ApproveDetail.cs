using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.ApproveDetail")]
    public class ApproveDetail
    {
        [Key]
        public int ApprovalDetailId { get; set; }
        public int ApprovalMasterId { get; set; }
        public long EmployeeId { get; set; }
        public int PriorityLevel { get; set; }
        public int? ConditionalAmount { get; set; }
        public string ConditionType { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
    }
}
