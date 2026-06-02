using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class ProfitDeclarationViewModel: PFBaseModel
    {

        [Display(Name="Declararion Id")]
        public string DeclararionId { get; set; }

        [Display(Name = "Organization Id")]
        public string OrgId { get; set; }

        [Display(Name = "Organization Name")]
        public string OrgName { get; set; }


         [Display(Name = "Year Start Date")]
        public string YearStartDate { get; set; }
         [Display(Name = "Year End Date")]
        public string YearEndDate { get; set; }
        [Display(Name = "Declaration Year")]
        public string DeclarationYear { get; set; }

        [Display(Name = "Profit")]
        public string Profit { get; set; }
        [Display(Name = "Calc. With Profit")]
        public bool CalculationWithProfit { get; set; }

        [Display(Name = "Profit Rate")]
        public string ProfitRate { get; set; }

        [Display(Name = "Prifit Distribution Rate")]
        public string InduceRate { get; set; }

        [Display(Name = "Prifit Distribution")]
        public string DistribursAmount { get; set; }

        [Display(Name = "Is Declared")]
        public bool IsDeclared { get; set; }
        public bool IsInduceRateReadonly { get; set; }
        public string DeclarationStatus { get; set; }
        //public IEnumerable<SelectListItem> DeclarationYearList { get; set; }
    }
}