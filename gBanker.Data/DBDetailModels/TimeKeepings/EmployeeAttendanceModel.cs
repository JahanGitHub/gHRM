using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.OverTimes
{
    public class EmployeeAttendanceModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime InOutDateTime { get; set; }
        public string AttendanceTime { get; set; }

        /// <summary>
        /// EventType=In or  EventType=Out
        /// </summary>
        public string EventType { get; set; }
    }
}
