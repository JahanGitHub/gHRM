using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.ManualOvertimeConfiguration")]
    public partial class ManualOvertimeConfiguration
    {
        [Key]
        public long Id { get; set; }
        public int? EmployeeDesignationId { get; set; }
        public long? EmployeeId { get; set; }
        public int WorkingDayMax { get; set; }
        public int HolidayMax { get; set; }
        public int MonthlyMax { get; set; }
        public bool ManualOvertimeOnly { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
