using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class OfficeNevigationPartialForTADAViewModel
    {
        [Display(Name = "Travel Place Type")]
        public int OfficeTypeId { get; set; }
        [Display(Name = "Head Office (প্রধান কার্য্যালয়)")]
        public string PVHeadOfficeId { get; set; }
        [Display(Name = "Project Office (প্রোজেক্ট অফিস)")]
        public string PVProjectId { get; set; }
        
        public string DepartmentId { get; set; }
        [Display(Name = "Zone Name (যোনের নাম)")]
        public string ZoneId { get; set; }
        [Display(Name = "Area Name (এরিয়ার নাম)")]
        public string AreaId { get; set; }
        [Display(Name = "Unit/Branch Name (ইউনিট/ব্রাঞ্চের নাম)")]//"Branch Name (শাখার নাম)"
        public string UnitId { get; set; }
        public int ParentOfficeId { get; set; } 
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }

      
    }
}