using gHRM.Core.Filters.PerformanceEvaluations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class PerformanceEvaluationViewModel
    {
        public IEnumerable<SelectListItem> HOList { get; set; }

        public IEnumerable<SelectListItem> ZoneList { get; set; }

        public IEnumerable<SelectListItem> AreaList { get; set; }

        public IEnumerable<SelectListItem> UnitList { get; set; }

        public IEnumerable<SelectListItem> BranchList { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }

        public PerformanceEvaluationSearchFilter SearchFilter { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Month { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Year { get; set; }

        [Display(Name = "Employee Code")]
        public string SearchTerm { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        [Display(Name = "Date To")]
        public string DateTo { get; set; }

        [Display(Name = "Ledger (Individual performance)")]
        public bool Ledger { get; set; }
        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Months { get; set; }
    }
}