using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class CollectionMethodViewModel : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Method Type")]
        public string MethodType { get; set; }
        [Display(Name = "Collection Format")]
        public string CollectionFormat { get; set; }
        public int Principal { get; set; }
        public int Interest { get; set; }
        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> MethodTypeLst { get; set; }
        public List<SelectListItem> CollectionFormatLst { get; set; }

    } 
} 