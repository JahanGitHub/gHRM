using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeReferencesViewModel : BaseModel
    {
        public long ReferenceId { get; set; }

        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Reference name is required")]
        [Display(Name = "Reference Name")]
        public string ReferenceName { get; set; }

        [Required(ErrorMessage = "Occupation is required")]
        [Display(Name = "Occupation")]
        public string ReferenceOccupation { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation")]
        public string ReferenceDesignation { get; set; }

        [Display(Name = "Relation")]
        public string ConnectionWithEmployee { get; set; }

        [Display(Name = "Address")]
        public string ContactAddress { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Email")]
        public string RefEmail { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        
    }
}