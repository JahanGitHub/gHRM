using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Apply
{
    [Table("apply.QuestionAnsweredByApplicant")]
    public partial class QuestionAnsweredByApplicant
    {
       public QuestionAnsweredByApplicant()
        {

        }

        [Key]
        public Int64 AnsId { get; set; }

        [Display(Name = "Q Id")]
        public Int64? QId { get; set; }

        [Display(Name = "Applicant Id")]
        public Int64? ApplicantId { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Q Answer")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string QAnswer { get; set; }

    }
}
