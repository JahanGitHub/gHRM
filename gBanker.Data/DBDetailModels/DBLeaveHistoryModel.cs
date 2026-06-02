using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class DBLeaveHistoryModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public long LeaveId { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string LeaveStartDateMsg { get; set; }
        public string LeaveEndDateMsg { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal? TotalDays { get; set; }
        public decimal? TotalAvailableDays { get; set; }
        public string LeaveReason { get; set; }
        public string AddressDuringLeave { get; set; }
        public int? OfficeType { get; set; }
        public int OfficeId { get; set; }
        public string LeaveDayDuration { get; set; }
    }
}
