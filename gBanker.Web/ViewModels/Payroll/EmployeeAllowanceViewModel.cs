using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class EmployeeAllowanceViewModel
    {
        public int Id { get; set; }

        [Display(Name ="Employee Grade")]
        public int EmpGradeId { get; set; }

        //[Display(Name = "Employee Type")]
        //public int EmpTypeId { get; set; }

        [Display(Name = "Employee Staus")]
        public int EmpStatusId { get; set; }
        public bool IsActive { get; set; }
        public string RatioOn { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        [Display(Name = "Allowance Amt.")]
        public decimal? Allowance { get; set; }

        [Display(Name = "Component Name")]
        public int? ComponentId { get; set; }

        public string GradeName { get; set; }
        public string StatusName { get; set; }
        public string ComponentName { get; set; }


        public IEnumerable<SelectListItem> GradeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }


    }
}