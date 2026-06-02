using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttendancePenaltyConfiguration")]
    public class AttendancePenaltyConfiguration
    {
        [Key]
        public int Id { get; set; }
        public int TotalLateDays { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int LeaveDeduction { get; set; }
        public int LeaveOrder { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string EmployeeStatus { get; set; }
    }
}
