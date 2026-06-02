using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeInformationApprovalViewModel
    {
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeName { get; set; }
    }
}