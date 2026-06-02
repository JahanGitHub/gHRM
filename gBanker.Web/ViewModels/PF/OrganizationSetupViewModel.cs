using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class OrganizationSetupViewModel
    {
        public int Id { get; set; }
        //[Display(Name = "PF Type")]
        //public string PFType { get; set; }
        [Display(Name = "Self Payroll Component")]
        public int? SelfContribution_ComponentPayrollId { get; set; }
        [Display(Name = "Office Payroll Component")]
        public int? OfficeContribution_ComponentPayrollId { get; set; }
        public bool IsActive { get; set; }
       // public IEnumerable<SelectListItem> PFTypeList { get; set; }
        public IEnumerable<SelectListItem> ComponentLst { get; set; }
    }
}