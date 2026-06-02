using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class PFTypeViewModel
    {
        [Display(Name = "ID")]
        public string PFTypeId { get; set; }
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Allow Self Contribution")]
        public bool HasSelfContribution { get; set; }
        [Required]
        [Display(Name = "Allow Org Contribution")]
        public bool HasOrgContribution { get; set; }
        [Required]
        [Display(Name = "Allow Additional Self Contribution")]
        public bool HasAddSelfContribution { get; set; }
        [Required]
        [Display(Name = "Self Contribution Rate")]
        public string SelfContributionRate { get; set; }
        [Required]
        [Display(Name = "Org Contribution Rate")]
        public string OrgContributionRate { get; set; }
        public IEnumerable<SelectListItem> PFTypeList { get; set; }
    }
}