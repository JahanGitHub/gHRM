using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveApproversMetadata")]
    public class LeaveApproversMetadata
    {

        [Key]
        public int Id { get; set; }
        public int? ApproveOfficeId { get; set; }
        public int? ApprovalLevel { get; set; }
        public int? ApproveDepartmentId { get; set; }
        public int? ApproveDesignationId { get; set; }
        public bool? IsApproverInSelfOffice { get; set; }
        public int? ConfigDesignation { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int? FromDay { get; set; }

        public int? ToDay { get; set; }
    }
}
