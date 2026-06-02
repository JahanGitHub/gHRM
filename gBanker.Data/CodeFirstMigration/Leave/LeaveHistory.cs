using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveHistory")]
    public partial class LeaveHistory
    {
        [Key]
        public long LeaveId { get; set; }

        public string LeaveNo { get; set; }

        public long EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeaveRequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime LeaveStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime LeaveEndDate { get; set; }

        public decimal? TotalDays { get; set; }

        [StringLength(500)]
        public string LeaveReason { get; set; }

        [StringLength(200)]
        public string AddressDuringLeave { get; set; }

        public byte[] LeaveAttachment { get; set; }

        public bool IsApproved { get; set; }

        public long? ApprovedBy { get; set; }

        public long? AdjustmentBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ApprovedDate { get; set; }

        public DateTime? AdjustmentDate { get; set; }

        public bool? IsEvidence { get; set; }

        public bool? IsRecommendation { get; set; }

        public string LeaveRecommendation { get; set; }

        public string LeaveNote { get; set; }

        public string LeaveHeader { get; set; }

        public string LeaveFooter { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdjustment { get; set; }

        public long? DispatchLeaveId { get; set; }

        public decimal? LWPSalaryDeduction { get; set; }

        public bool? IsSalaryDeducted { get; set; }

        public string leaveDispatchRemarks { get; set; }

        public long? ReplacementEmployee { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public DateTime? LeaveUpToDate { get; set; }
        public string LeaveDayDuration { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual LeaveType LeaveType { get; set; }
    }
}
