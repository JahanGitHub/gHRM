using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.OverTimes
{
    public class TimeKeepingReportModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int DesignationId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDesignationName { get; set; }
        public string EmployeeOfficeDesignationName { get; set; }
        public string LastLogInTime { get; set; }
        public int AttendanceTypeId { get; set; }
        public int LeaveTypeId { get; set; }
        public string DepartmentName { get; set; }
        public string TimeDifferenceLate { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string AttendaceDateFrom { get; set; }
        public string AttendanceDateTo { get; set; }
        public string AttendanceDate { get; set; }
        public string LogInTime { get; set; }
        public string LogOutTime { get; set; }
        public string LateTime { get; set; }
        public string WorkingHour { get; set; }
        public string RegularHour { get; set; }
        public string EmployeeAttendaceStatus { get; set; }
        public string PreparedBy { get; set; }
        public string EmployeeAttendaceStatusShort { get; set; }
        public string RemarksAttendance { get; set; }
        public string WorkingHourSUM { get; set; }
        public string OverTime { get; set; }
        public string OverTimeHourSUM { get; set; }
        public string OverTimeHourSUMInText { get; set; }

        public string TotalOvertimeAmount { get; set; }
        public string TotalOvertimeActualAmount { get; set; }
        public string OrganizationLogo { get; set; }

        public bool IsOvertimeException { get; set; }
        public bool IsOvertime { get; set; }
    }
}
