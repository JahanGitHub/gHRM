using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.API.Model
{
    public class EMPLOYEEAPIModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string PresentAddress { get; set; }
        public string PhoneNo { get; set; }
        public string EmployeePassword { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ImageLink { get; set; }
        public string CurrentOfficeTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string Responsibility { get; set; }



    }// END Class
}// END Namespace