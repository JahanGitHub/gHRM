using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanEligibilityViewModel : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Purpose name")]
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        [Display(Name = "Min. Job Age")]
        public decimal MinmumJobAge { get; set; }
        [Display(Name = "Max. Job Age")]
        public decimal MaximumJobAge { get; set; }
        [Display(Name = "PF Contribution")]
        public string PFContribution { get; set; }
        [Display(Name = "Loan Eligible in Percent")]
        public decimal LoanEligibleInPercent { get; set; }
        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> PFContributionLst { get; set; }
        public List<SelectListItem> PurposeLst { get; set; }

    } // End of Class 
} // End of Namespace