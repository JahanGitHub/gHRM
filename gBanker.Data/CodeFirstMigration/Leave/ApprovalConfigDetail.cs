using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.ApprovalConfigDetail")]
    public class ApprovalConfigDetail
    {
        [Key]
        public int ConfigDetailsId { get; set; }
        public int ConfigMasterId { get; set; }
        public int ApprovalLevel { get; set; }
        public int? ApproveOfficeId { get; set; }
        public int? ApproveDepartmentId { get; set; }
        public int ApproveDesignationId { get; set; }
        public int? ApprovalEmployeeId { get; set; }
        public int IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsApproverInSelfOffice { get; set; }

    }
}
