using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundSetting
{
    public class HealthWelfareFundSettingViewModel
    {
        public int HealthWelfareFundSettingId { get; set; }

        [Display(Name = "Deduction Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DeductionAmount { get; set; }
        public bool IsPercentage { get; set; }
    }
}