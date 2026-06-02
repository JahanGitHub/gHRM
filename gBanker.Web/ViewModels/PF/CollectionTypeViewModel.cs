using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class CollectionTypeViewModel : PFBaseModel
    {
        [Display(Name = "ID")]
        public int CollectionTypeId { get; set; }
        [Display(Name = "Name")]
        public string CollectionTypeName { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
    }
}