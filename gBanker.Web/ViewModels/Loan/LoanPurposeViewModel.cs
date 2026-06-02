using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanPurposeViewModel 
    {
        public int PurposeId { get; set; }
        [Display(Name = "Purpose Name")]
        public string PurposeName { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Method Type")]
        public string MethodType { get; set; }

        public int GracePeriod { get; set; }
        //[Display(Name = "Component Name")]
        //public int PRComponentID { get; set; }
        //[Display(Name = "Interest Rate")]
        //public int InterestRate { get; set; }
        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> GracePeriodLst { get; set; }
        public List<SelectListItem> MethodTypeLst { get; set; }
        public List<SelectListItem> PRComponentLst { get; set; }
    } 
} 