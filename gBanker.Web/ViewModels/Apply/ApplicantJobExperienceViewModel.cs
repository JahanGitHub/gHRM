using System;
using System.ComponentModel.DataAnnotations;

namespace gHRM.Web.ViewModels
{
    public class ApplicantJobExperienceViewModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 Id { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        [Display(Name = "Company Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Business")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CompanyBusiness { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Designation { get; set; }

        [Display(Name = "Areaof Experiences")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AreaofExperiences { get; set; }

        [Display(Name = "Responsibilities")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Responsibilities { get; set; }

        [Display(Name = "Company Location")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string CompanyLocation { get; set; }

        [Display(Name = "Job Start Date")]
        public DateTime JobStartDate { get; set; }

        public String JobStartDateMsg { get; set; }

    [Display(Name = "Job End Date")]
        public DateTime? JobEndDate { get; set; }

        public String JobEndDateMsg { get; set; }

        public string rowSl { get; set; }

        public string Continuing { get; set; }

        public string WorkingPeriod { get; set; }
    }
}