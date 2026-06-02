using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Apply
{
    public class ApplicantAccademicViewModel : BaseModel
    {

        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ID { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        [Display(Name = "Levelof Education")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string LevelofEducation { get; set; }

        [Display(Name = "Exam Title")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ExamTitle { get; set; }

        [Display(Name = "Group")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Group { get; set; }

        [Display(Name = "Institute Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string InstituteName { get; set; }

        [Display(Name = "Result Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ResultType { get; set; }

        [Display(Name = "CGPA")]
        public decimal? CGPA { get; set; }

        [Display(Name = "Scale")]
        public decimal? Scale { get; set; }

        [Display(Name = "Year of Passing")]
        public DateTime? YearsofPassing { get; set; }

        [Display(Name = "Duration of Years")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Duration_Years { get; set; }

        public List<SelectListItem> ExamTitleList { get; set; }
        public List<SelectListItem> LevelofEducationList { get; set; }

        public int LevelofEducationId { get; set; }

        public int ExamTitleId { get; set; }

        public string YearsofPassingMsg { get; set; }

        public string rowSl { get; set; }
    }
}

