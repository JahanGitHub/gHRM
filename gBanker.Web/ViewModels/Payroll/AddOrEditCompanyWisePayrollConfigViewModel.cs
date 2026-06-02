using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class AddOrEditCompanyWisePayrollConfigViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Company Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string CompanyCode { get; set; }

        [Display(Name = "Payroll Type")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        [Required(ErrorMessage = "{0} is Required")]
        public string PayrollType { get; set; }

        [AllowHtml]
        [Display(Name = "Description")]       
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }
    }
}