using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels
{
    public class EmployeeDesignationViewModel : BaseModel
    {
        public int DesignationId { get; set; }
        
        [Required(ErrorMessage = "Designation Code is required")]
        [Display(Name = "Designation Code(পদবীর কোড)")]
        public string DesignationCode { get; set; }
       
        [Required(ErrorMessage = "Designation Name is required")]
        [Display(Name = "Designation Name(পদবীর নাম)")]
        public string DesignationName { get; set; }
       
        [Display(Name="Short Name(সংক্ষিপ্ত নাম)")]
        public string DesignationShortName { get; set; }
         [Display(Name = "Salary Scale(বেতন স্কেল)")]

        public int SalaryScaleId { get; set; }
         [Display(Name = "Designation Type")]
         public string DesignationType { get; set; }
        public int? CompanyId { get; set; }
        public string Rank { get; set; }
        [Display(Name = "Insurance Amount")]
        public int? InsuranceAmount { get; set; }

        public int rowSl { get; set; }
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }
        public IEnumerable<SelectListItem> DesignationTypeList { get; set; }
        public IEnumerable<SelectListItem> RankList { get; set; }
        
    }
}