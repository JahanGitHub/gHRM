using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class CollectionTypeConfigurationViewModel
    {
        [Display(Name = "Collection Type")]
        public string CollectionType { get; set; }
        [Display(Name = "Principal (%)")]
        public int PrincipalInPer { get; set; }
        [Display(Name = "Interest (%)")]
        public int InterestInPer { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<SelectListItem> CollectionTypeLst { get; set; }
    }
}