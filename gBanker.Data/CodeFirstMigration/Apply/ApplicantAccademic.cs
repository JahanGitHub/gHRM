using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gHRM.Data.CodeFirstMigration.Apply
{
    [Table("apply.ApplicantAcademicInfo")]
    public partial class ApplicantAccademic
    {
        public ApplicantAccademic()
        {
            
        }
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ID { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        public string Group { get; set; }

        [Display(Name = "Institute Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string InstituteName { get; set; }

        [Display(Name = "Result Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ResultType { get; set; }

        [Display(Name = "C G P A")]
        public decimal? CGPA { get; set; }

        [Display(Name = "Scale")]
        public decimal? Scale { get; set; }

        [Display(Name = "Yearsof Passing")]
        public DateTime YearsofPassing { get; set; }

        [Display(Name = "Duration_ Years")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Duration_Years { get; set; }

        public int? LevelofEducationId { get; set; }

        public int? ExamTitleId { get; set; }

        public bool? IsActive { get; set; }
    }
}
