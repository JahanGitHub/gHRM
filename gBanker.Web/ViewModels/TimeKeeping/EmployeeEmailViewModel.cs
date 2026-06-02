using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.TimeKeeping
{
    public class EmployeeEmailViewModel
    {
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string OfficialEmail { get; set; }
        public string Email { get; set; }
        public string CCOfficialEmail { get; set; }
        public string CCEmail { get; set; }
    }
}