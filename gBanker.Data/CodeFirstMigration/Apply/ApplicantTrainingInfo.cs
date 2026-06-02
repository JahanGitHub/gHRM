using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Apply
{
    [Table("apply.ApplicantTrainingInfo")]
    public partial class ApplicantTrainingInfo
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 Id { get; set; }

        [Display(Name = "Applicant Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ApplicantId { get; set; }

        [Display(Name = "Training Title")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string TrainingTitle { get; set; }

        [Display(Name = "Topics Covered")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string TopicsCovered { get; set; }

        [Display(Name = "[ Training Year ]")]
        public DateTime TrainingYear { get; set; }

        [Display(Name = "[ Institute ]")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Institute { get; set; }

        [Display(Name = "Duration")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Duration { get; set; }

        public bool? IsActive { get; set; }

    }
}
