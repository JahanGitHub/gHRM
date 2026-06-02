using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings
{
    public class FundSetupViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fund Type")]
        public string FundType { get; set; }

        [Display(Name = "Component Type")]
        public string ComponentType { get; set; }

        [Display(Name = "Component Amount")]
        public decimal? ComponentAmount { get; set; }

        [Display(Name = "Ratio Based On")]
        [Required(ErrorMessage = "{0} is Required")]
        public string RatioBasedOn { get; set; }

        [Display(Name = "Component Name")]
        [Required(ErrorMessage = "{0} is Required")]
        public int PRComponentId { get; set; }     

        public string ComponentName { get; set; }

        public int CreateUser { get; set; }

        public string CreateUserName { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public string CreateDateString { get; set; }

        public IEnumerable<SelectListItem> StaffWelfareFundSettingList { get; set; }

        public IEnumerable<SelectListItem> RatioBasedList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> ComponentTypeList { get; set; }

        public IEnumerable<SelectListItem> ComponentCategoryList { get; set; }
        public bool IsActive { get; internal set; }
    }
}