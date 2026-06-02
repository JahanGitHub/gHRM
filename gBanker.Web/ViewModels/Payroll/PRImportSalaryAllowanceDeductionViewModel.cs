using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRImportSalaryAllowanceDeductionViewModel
    {
        [Display(Name ="Start Date")]
        [Required(ErrorMessage ="{0} is Required")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime? EndDate { get; set; }

        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryDay { get; set; }

        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Months { get; set; }
       
    }

    public class PRImportSalaryAllowanceDeductionViewModel2
    {
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime? EndDate { get; set; }



        [Display(Name = "Component Category")]
        public string ComponentCategory { get; set; }

        [Display(Name = "Component Name")]
        public string PRComponentId { get; set; }

        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryDay { get; set; }

        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Months { get; set; }

        public IEnumerable<SelectListItem> ComponentList { get; set; }

        public IEnumerable<SelectListItem> ComponentCategoryList { get; set; }



    }
}