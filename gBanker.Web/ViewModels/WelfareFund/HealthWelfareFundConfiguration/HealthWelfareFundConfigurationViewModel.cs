using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundConfiguration
{
    public class HealthWelfareFundConfigurationViewModel
    {

        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CollectionYear { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CollectionMonth { get; set; }
        public int HealthWelfareFundConfigurationId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        [Display(Name = "Health Welfare Fund Setting")]
        public int HealthWelfareFundSettingId { get; set; }

        [Display(Name = "Collection Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal CollectionAmount { get; set; }

        [Display(Name = "Collection Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CollectionDate { get; set; }



        public IEnumerable<SelectListItem> HealthWelfareFundSettingList { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
    }
}