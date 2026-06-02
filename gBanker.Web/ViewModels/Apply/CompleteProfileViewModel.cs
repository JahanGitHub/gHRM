using System;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Apply;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gHRM.Web.ViewModels.Apply
{
    public class CompleteProfileViewModel : BaseModel
    {

        public List<ApplicantMaster> ApplicantMaster { get; set; }
        public List<ApplicantTrainingInfoViewModel> ApplicantTrainingInfoViewModel { get; set; }

        public List<ApplicantReferenceInfoViewModel> ApplicantReferenceInfoViewModel { get; set; }

        public List<ApplicantAccademicViewModel> ApplicantAccademicViewModel { get; set; }


        public List<ApplicantJobExperienceViewModel> ApplicantJobExperienceViewModel { get; set; }




    }

}