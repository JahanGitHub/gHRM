using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class LoanTypeViewModel: PFBaseModel
    {
        public string Id { get; set; }

        [Display(Name = "Loan Type Id")]
        public string LoanTypeId { get; set; }
        
        [Display(Name = "Loan Type")]
        public string LoanTypeName { get; set; }
        
        [Display(Name = "Interest Rate")]
        public string InterestRate { get; set; }

        [Display(Name = "Loan Percentage")]
        public string LoanPercentage { get; set; }

        public int InterestRateTypeId { get; set; }
        public string InterestRateType { get; set; }

        [Display(Name = "Interest Rate Type")]
        public IEnumerable<SelectListItem> InterestRateTypeList { get; set; }
    }
}