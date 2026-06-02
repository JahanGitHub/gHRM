using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LinkWithEmployeeViewModel
    {
        public int LinkId { get; set; }
        public string OrganizationCode { get; set; }
        public List<SelectListItem> OrganizationList { get; set; }
        public string RelativeEmployeeCode { get; set; }
        public string RelativeEmployeeName { get; set; }
        public string RelativeDepartmentName { get; set; }
        public string RelativeDesignationName { get; set; }
    }
}