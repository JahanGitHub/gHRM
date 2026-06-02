using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class OfficeViewModel : BaseModel
    {
        //public string rowSl { get; set; } 
        public int OfficeId { get; set; }

        public int? CompanyId { get; set; }

        [Display(Name = "Office Type (অফিসের ধরন)")]
        public int? OfficeTypeId { get; set; }

        [Display(Name = "Name Bangla (নাম বাংলায়)")]
        public string OfficeNameBn { get; set; }

        [Display(Name = "Code (কোড)")]
        public string OfficeCode { get; set; }

        [Display(Name = "Name (নাম)")]
        public string OfficeName { get; set; }

        public int OfficeLevel { get; set; }

        public string FirstLevel { get; set; }

        public string SecondLevel { get; set; }

        public string ThirdLevel { get; set; }

        public string FourthLevel { get; set; }

        [Display(Name = "Operation Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]

        public System.DateTime OperationStartDate { get; set; }

        [Display(Name = "Address (ঠিকানা)")]
        public string OfficeAddress { get; set; }

        [Display(Name = "Post Code (পোষ্ট অফিসের কোড)")]
        public string PostCode { get; set; }

        [Display(Name = "Email (ই-মেইল)")]
        public string Email { get; set; }

        [Display(Name = "Phone (ফোন)")]
        public string Phone { get; set; }

        [Display(Name = "Parent Code (প্রধান কোড)")]
        public string ParentId { get; set; }

        [Display(Name = "Operation Start Date (কার্যক্রম শুরুর তারিখ)")]
        public string OperationStartDateMsg { get; set; }

        public string DepartmentId { get; set; }
        [Display(Name = "Zone Name (যোনের নাম)")]

        public string ZoneId { get; set; }
        [Display(Name = "Area Name (এরিয়ার নাম)")]

        public string AreaId { get; set; }

        public string UnitId { get; set; }

        public string rowSl { get; set; }
        [Display(Name = "Area Name (এরিয়ার নাম)")]

        public string AreaCode { get; set; }
        [Display(Name = "Zone Name (যোনের নাম)")]

        public string ZoneCode { get; set; }

        public string OfficeTypeName { get; set; }

        [Display(Name = "Office Location")]
        public int? OfficeLocationId { get; set; }

        public IEnumerable<SelectListItem> OfficeLocationList { get; set; }

        public IEnumerable<SelectListItem> GeoLocationList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
    }
}