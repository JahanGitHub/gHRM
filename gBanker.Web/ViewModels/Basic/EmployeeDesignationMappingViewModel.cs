using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeDesignationMappingViewModel
    {
        public int DesignationMapId { get; set; }
        public int EquivalentDesignationId { get; set; }
        public int OrnamentalDesginationid { get; set; }
        public int OfficeDesignationId { get; set; }
        public string EquivalentDesignationName { get; set; }
        public string OfficeDesginationName { get; set; }
        public string EmployeeDesignationName { get; set; }
        public IEnumerable<SelectListItem> EquivalenDesignationList { get; set; }
        public IEnumerable<SelectListItem> OfficeDesignationList { get; set; }
        public IEnumerable<SelectListItem> OrnamentalDesignationList { get; set; }
    }
}