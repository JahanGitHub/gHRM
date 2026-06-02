using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class AttHolidayDeclarationViewModel : BaseModel
    {
       
        public long AttHolidayDeclarationId { get; set; }
        [Display(Name = " Year (বছর)")]
        public int HolidayYear { get; set; }
        [Display(Name = "Date From")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HolidayDate { get; set; }
        [Display(Name = "Weekend Day (সাপ্তাহিক ছুটির দিন)")]
        public int AttHolidayTypeId { get; set; }
        public string HolidayDateForView { get; set; }

        [Display(Name = "Type")]
        public string HolidayTypeShortName { get; set; }
        [Display(Name = "Holiday Type")]
        public string HolidayTypeFullName { get; set; }
        public int HolidayId { get; set; }
        public int OfficeId { get; set; }
        //public int? ZoneId { get; set; }
        //public int? AreaId { get; set; }
        //public int? UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DayList { get; set; }
        public string OfficeName { get; set; }
        public bool IfDataExists { get; set; }
        public string DayName { get; set; }
        public int HolidayYearSearch { get; set; }
        public IEnumerable<SelectListItem> HolidayYearList { get; set; }
        public string SlNo { get; set; }


        [Display(Name = "Office Type (অফিসের ধরণ)")]
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
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        [Display(Name = "Date To")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HolidayDateTo { get; set; }

    }// End of Class
}// End of Namespace