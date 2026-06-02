using System;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Apply;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Apply
{
    public class AppliedPostViewModel : BaseModel
    {
        [Display(Name = "Applied Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 AppliedId { get; set; }

        [Display(Name = "Job Id")]
        public Int64? JobId { get; set; }

        [Display(Name = "Applicant Id")]
        public Int64? ApplicantId { get; set; }

        [Display(Name = "Already Applied")]
        public int? AlreadyApplied { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }


        public string PostName { get; set; }

        public string PostDescription { get; set; }

        public string AppliedOrNot { get; set; }

        public string CheckButton { get; set; }

        public List<AppliedPostViewModel> AppliedPostList { get; set;}


    }
}