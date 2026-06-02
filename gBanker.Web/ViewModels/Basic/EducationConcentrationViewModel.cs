using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EducationConcentrationViewModel
    {
        public int ConcentrationId { get; set; }
        [Display(Name = "Degree Code")]
        public string DegreeCode { get; set; }
        [Display(Name = "Concentration Code")]
        public string ConcentrationCode { get; set; }
        [Display(Name = "Concentration Name")]
        public string ConcentrationName { get; set; }
        [Display(Name = "Company Name")]
        public int CompanyId { get; set; }
        [Display(Name = "Degree Name")]
        public string DegreeName { get; set; }
        [Display(Name = "Degree Level")]
        public string DegreeLevel { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> DegreeCodeList { get; set; }
        public int rowSl { get; set; }
        public string CompanyName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int DegreeId { get; set; }
    }
}