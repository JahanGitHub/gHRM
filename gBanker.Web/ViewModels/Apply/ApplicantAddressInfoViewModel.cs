using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Apply
{
    public class ApplicantAddressInfoViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 Id { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        [Display(Name = "Village_ Road")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Village_Road { get; set; }

        [Display(Name = "Police Station")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string PoliceStation { get; set; }

        [Display(Name = "District")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string District { get; set; }

        [Display(Name = "Care Of")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CareOf { get; set; }

        [Display(Name = "Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Type { get; set; }
    }
}