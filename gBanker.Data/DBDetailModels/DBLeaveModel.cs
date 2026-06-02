using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gHRM.Data.DBDetailModels
{
    public class DBLeaveModel
    {
        public long EmployeeId { get; set; }
        public long LeaveId { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public string LeaveStartDateMsg { get; set; }
        public string LeaveEndDateMsg { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal? TotalDays { get; set; }
        public string LeaveReason { get; set; }
        public string AddressDuringLeave { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LeaveDayDuration { get; set; }
    }

}
