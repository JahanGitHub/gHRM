using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class EmployeePostingHistoryViewModel : BaseModel
    {
        public long EmpPostHistoryId { get; set; }
        public string OrderNo { get; set; }
        public long EmpPromotionId { get; set; }
        //[Display(Name="Department (বিভাগের নাম)")]        
        public int? DesignationId { get; set; }
        [Display(Name = "Designation (পদবীর নাম)")]
        public string DesignationName { get; set; }
        public int? DepartmentId { get; set; }
        [Display(Name = "Department (বিভাগের নাম)")]
        public string DepartmentName { get; set; }
        public int ZoneId { get; set; }
        public int MeritNo { get; set; }


        [Display(Name = "Employee Name (কর্মচারীর নাম)")]
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        [Display(Name = "Joining Date (যোগদানের তারিখ)")]
        public DateTime? JoiningDate { get; set; }
        [Display(Name = "Release Date (রিলিজের তারিখ)")]
        public DateTime? DepartureDate { get; set; }
        public string EmployeeIdMsg { get; set; }
        public string JoiningDateMsg { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string DepartureDateMsg { get; set; }
        [Display(Name = "Actual Release Date")]
        public DateTime? ActualReleaseDate { get; set; }
        public string Duration { get; set; }
        [Display(Name = "Order No. (অর্ডার নং)")]
        public string PostingOrderNo { get; set; }
        [Display(Name = "Order Date (অর্ডার তারিখ)")]
        public DateTime? OrderDate { get; set; }
        public int OfficeID { get; set; }
        [Display(Name = "Office Name (অফিসের নাম)")]
        public string OfficeName { get; set; }
        [Display(Name = "Office Designation (অফিসের পদবী)")]
        public int? OfficeDesignationId { get; set; }
        public decimal SalaryScale { get; set; }
        public int? SalaryScaleId { get; set; }
        public decimal? Tha_Dist { get; set; }
        [Display(Name = "Remarks (মন্তব্য)")]
        public string Remarks { get; set; }
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDateWhenRelease { get; set; }

        public string OffcDesignName { get; set; }

        [Display(Name = "ধরণ")]
        public string EmployeeRank { get; set; }
        public IEnumerable<SelectListItem> PostingOrderList { get; set; }
        public IEnumerable<SelectListItem> OfficeDesignationList { get; set; }

        //promotionemp search //   
        ////
        public string PromotionType { get; set; }

        public string Post { get; set; }
        public string rowSl { get; set; }
        public string OrderDateMsg { get; set; }
        public string NewOffice { get; set; }
        public string OrderCreateBy { get; set; }
        public decimal? Pay { get; set; }
        public int OrderId { get; set; }
        public string PostingTypeVal { get; set; }
        public IEnumerable<SelectListItem> PostingTypeList { get; set; }

    }// End Of Class
}