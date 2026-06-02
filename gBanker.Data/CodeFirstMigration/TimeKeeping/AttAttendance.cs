using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttAttendance")]
    public partial class AttAttendance
    {
        [Key]
        public long AttAttendanceId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<long> AttCardIssueId { get; set; }
        public System.DateTime AttenDate { get; set; }
        public string LogInType { get; set; }
        public System.DateTime ? LoginTime { get; set; }
        public Nullable<System.DateTime> LogoutTime { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string LateTime { get; set; }
        public string InOutType { get; set; }
        public Nullable<System.DateTime> InOutTime { get; set; }
        public Nullable<int> AttOfficeMachineId { get; set; }
        public int AttAttendanceTypeId { get; set; }
        public string TimeKeepingType { get; set; }
        public Nullable<int> AttOfficeDayTypeId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public Nullable<long> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
