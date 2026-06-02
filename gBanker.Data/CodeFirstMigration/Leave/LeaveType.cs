
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveType")]
    public partial class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        [StringLength(150)]
        public string LeaveTypeName { get; set; }

        [StringLength(2)]
        public string EligibleFrom { get; set; }
        public int? MaxLeaveDays { get; set; }
        public int? MaxAvailDays { get; set; }

        [StringLength(2)]
        public string LeaveStatus { get; set; }

        [StringLength(2)]
        public string LeaveGender { get; set; }

        public int LeaveTypeRank { get; set; }
        public decimal? DaysPerEL { get; set; }

        [StringLength(2)]
        public string ELAdd { get; set; }
        public int? LeaveQty { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public int EmployeeStatusId { get; set; }

        public string LeaveCategory { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsCountWeeklyHolidaysInBetween { get; set; }
        public bool IsCountOtherHolidaysInBetween { get; set; }
        public int? TotalMaxLeaveDays { get; set; }
    }
}
