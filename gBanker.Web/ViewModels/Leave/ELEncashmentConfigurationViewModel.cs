using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class ELEncashmentConfigurationViewModel
    {
        public int ConfigurationId { get; set; }
        public string EligibleFrom { get; set; }
        public string EncashmentStage { get; set; }
        public int EligibilityDuration { get; set; }
        public int MinimumBalance { get; set; }
        public int EncashmentEligibleQuantity { get; set; }
        public string Formula { get; set; }

        public IEnumerable<SelectListItem> EncashmentStageList { get; set; }
        public IEnumerable<SelectListItem> EligibleFromList { get; set; }
        public IEnumerable<SelectListItem> EligibilityDurationList { get; set; }
        public IEnumerable<SelectListItem> FormulaList { get; set; }


    }
}