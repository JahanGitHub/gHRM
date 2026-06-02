using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class EmployeeConfigurationViewModel: PFBaseModel
    {
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Is PF Withdrawn")]
        public bool IsPFWithdrawn { get; set; }

        [MaxLength(100)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Office Id")]
        public int OfficeId { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        
        [Display(Name = "Office")]
        public IEnumerable<SelectListItem> OfficeList { get; set; }

        [Display(Name = "Addition Self Rate")]
        public string AdditionalSelfRate { get; set; }  
    }
}