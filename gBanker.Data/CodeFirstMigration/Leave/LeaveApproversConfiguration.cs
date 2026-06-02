using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveApproversConfiguration")]
    public class LeaveApproversConfiguration
    {
        [Key]
        public int ID { get; set; }
        public int ApplicantDesignation { get; set; }
        public int ApprovalDesignation { get; set; }
        public int ApprovalLevel { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }       
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
