using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class OvertimeConfigurationViewModel
    {

        /// <summary>
        /// Item
        /// </summary>
        /// 
        public int OvertimeConfigId { get; set; }

        [Display(Name = "Hour From")]
        public int HourFrom { get; set; }

        [Display(Name = "Hour To")]
        public int HourTo { get; set; }


        [Display(Name = "Prev Hour To")]
        public int PrevHourTo { get; set; }

        [Display(Name = "Later Hour From")]
        public int LaterHourFrom { get; set; }


        [Display(Name = "Fixed Amount")]
        public double Amount { get; set; }

        [Display(Name = "Rule")]
        public string Rule { get; set; }

        [Display(Name = "Divided By")]
        public double DividedBy { get; set; }

        [Display(Name = "Rank")]
        public int Rank { get; set; }

        //public IEnumerable<SelectListItem> FromHourList { get; set; }
        //public IEnumerable<SelectListItem> ToHourList { get; set; }
        public IEnumerable<SelectListItem> RuleList { get; set; }        
    }
}