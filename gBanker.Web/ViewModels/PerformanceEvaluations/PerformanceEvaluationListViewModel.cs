using gHRM.Core.Filters.PerformanceEvaluations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PerformanceEvaluations
{
    public class PerformanceEvaluationListViewModel
    {
        public PerformanceEvaluationSearchFilter SearchFilter { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Month { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Year { get; set; }

        [Display(Name = "Employee Code")]
        public string SearchTerm { get; set; }

        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Months { get; set; }
    }
}