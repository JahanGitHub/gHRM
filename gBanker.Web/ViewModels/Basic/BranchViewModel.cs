using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class BranchViewModel:BaseModel
    {       
        public int BranchId { get; set; }

        [Required]
       [Display(Name="Branch Name")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name="Address")]
        public string BranchAddress { get; set; }

        [Display(Name="Email Address")]
        public string BranchEmail { get; set; }

        [Display(Name="Phone No.")]
        public string BranchPhone { get; set; }

        [Display(Name="Company Name")]
        public int CompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyList { get; set; }

    }
}