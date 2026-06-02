using System;
using System.ComponentModel.DataAnnotations;

namespace gHRM.Web.ViewModels.Apply
{
    public class ApplicantReferenceInfoViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 Id { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        [Display(Name = "Name")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Name { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Designation { get; set; }

        [Display(Name = "Organization")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Organization { get; set; }

        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Email { get; set; }

        [Display(Name = "Relation")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Relation { get; set; }

        public string rowSl { get; set; }


    }
}