using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class OvertimeExceptionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Employee Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Exception Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ExceptionType { get; set; }

        [Display(Name = "Effective Start Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EffectiveStartDate { get; set; }

        [Display(Name = "Effective End Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EffectiveEndDate { get; set; }

        public int EmployeeId { get; set; }
    }
}