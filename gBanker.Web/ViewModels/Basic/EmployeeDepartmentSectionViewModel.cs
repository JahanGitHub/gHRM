using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeDepartmentSectionViewModel
    {
        public int SectionId { get; set; }
        public int DepartmentId { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string OfficeType { get; set; }
        public List<SelectListItem> OfficeTypeList { get; set; }
        public List<SelectListItem> DepartmentList { get; set; } 
    }
}