using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.API.Model
{
    public class LeaveListViewModel
    {
        // LEAVE
        public string LeaveTypeName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public int TotalDays { get; set; }
        public string LeaveReason { get; set; }
        // END Leave

    }// END Class
}// ENd Namespace