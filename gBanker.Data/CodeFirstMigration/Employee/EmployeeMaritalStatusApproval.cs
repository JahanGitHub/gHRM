using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeMaritalStatusApproval")]
    public class EmployeeMaritalStatusApproval
    {
        [Key]
        public int MaritalId { get; set; }
        public string EmployeeCode { get; set; }
        public string MaritalStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int ApprovedOrRejectedBy { get; set; }
        public DateTime ApprovalOrRejectionDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
