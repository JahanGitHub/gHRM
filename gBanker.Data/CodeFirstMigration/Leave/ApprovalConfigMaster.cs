using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.ApprovalConfigMaster")]
    public class ApprovalConfigMaster
    {
        [Key]
        public int ConfigMasterId { get; set; }
        public string ModuleName { get; set; }
        public int? ConfigOfficeTypeId { get; set; }
        public int? ConfigOfficeId { get; set; }
        public int? ConfigDepartmentId { get; set; }
        public int ConfigDesignation { get; set; }
        public int CompanyId { get; set; }
        public int IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? LeaveTypeId { get; set; }
    }
}
