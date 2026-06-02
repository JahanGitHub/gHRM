using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeStatusViewModel : BaseModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusValue { get; set; }
        public int ViewOrder { get; set; }
        public bool IsValid { get; set; }
        public bool? IsSalaryApplicable { get; set; }

        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
    }
}