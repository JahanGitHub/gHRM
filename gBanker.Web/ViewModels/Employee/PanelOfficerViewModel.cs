using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gHRM.Web.ViewModels
{
    public class PanelOfficerViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Display(Name = "Assign Date")]
        public DateTime AssignDt { get; set; }

        [Display(Name = "Release Date")]
        public DateTime? ReleaseDt { get; set; }

        [Display(Name = "Employee Rank")]
        public int EmployeeRank { get; set; }

        [Display(Name ="Zone")]
        public int ZoneId { get; set; }

        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        public IEnumerable<SelectListItem> EmployeeRankList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
    }
}