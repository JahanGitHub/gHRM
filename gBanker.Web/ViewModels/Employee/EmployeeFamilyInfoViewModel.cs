using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeFamilyInfoViewModel : BaseModel
    {
        public long FamilyInfoId { get; set; }

        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Member name is required")]
        [Display(Name = "Member Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Relation is required")]
        [Display(Name = "Relation")]
        public string Relation { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public string EducationalQualification { get; set; }
        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

    }
}