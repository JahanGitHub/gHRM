using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Payroll
{
    public class SalaryDateConfigViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Day Of Monthly Salary")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DayOfMonthlySalary { get; set; }

        [Display(Name = "Is Current")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsCurrentlyUsing { get; set; }
    }
}