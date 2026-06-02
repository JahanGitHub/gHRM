using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public partial class EmployeeTVPROViewModel
    {
        public int EmpOfficeVisitId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeName { get; set; }
        public string VisitType { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public string CurrentOfficeProvided { get; set; }
        public int CurrentOfficeProvidedVal { get; set; }
        public List<SelectListItem> VisitTypeList { get; set; }
        public List<SelectListItem> OfficeProvidedList { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
    }
}