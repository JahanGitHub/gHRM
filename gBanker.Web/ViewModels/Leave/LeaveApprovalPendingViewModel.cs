using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class LeaveApprovalPendingViewModel
    {
        public string LeaveNo { get; set; }
        public string LeaveTypeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string OffcDesignName { get; set; }
        public string PendingApproverName { get; set; }
        public string LeaveRequestDate { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
    }
}