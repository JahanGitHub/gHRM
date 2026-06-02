using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EducationDegreeViewModel
    {
        public int DegreeId { get; set; }
        [Display(Name = "DegreeLevel Id")]
        public int DegreeLevelId { get; set; }
        [Display(Name = "Degree Level")]
        public string DegreeLevel { get; set; }

        [Display(Name = "New Degree Level")]
        public string NewDegreeLevel { get; set; }
        [Display(Name = "Degree Code")]
        public string DegreeCode { get; set; }
        [Display(Name = "Degree Name")]
        public string DegreeName { get; set; }
        [Display(Name = "Company Name")]
        public int CompanyId { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public int rowSl { get; set; }
        public string CompanyName { get; set; }
        public string DegreeTitle { get; set; }

        public int ConcentrationId { get; set; }
        public string ConcentrationCode { get; set; }
        public string ConcentrationName { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public IEnumerable<SelectListItem> DegreeLevelList { get; set; }
    }
}