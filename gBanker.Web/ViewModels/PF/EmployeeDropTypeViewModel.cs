using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class EmployeeDropTypeViewModel
    {
        [Display(Name = "Drop Id")]
        public string DropId { get; set; }
        [Display(Name = "Drop Type")]
        public string DropType { get; set; }
        public IEnumerable<SelectListItem> DropTypeList { get; set; }
    }
}