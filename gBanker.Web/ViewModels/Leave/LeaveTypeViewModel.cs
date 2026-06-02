
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveTypeViewModel : BaseModel
    {
        public int LeaveTypeId { get; set; }

        [Display(Name="Leave Name (ছুটির নাম)")]
        public string LeaveTypeName { get; set; }

        [Display(Name = "Eligible From (ছুটি ভোগ)")]
        public string EligibleFrom { get; set; }

        [Display(Name = "Max Leave Days (সর্বোচ্চ ছুটির দিন)")]
        public int? MaxLeaveDays { get; set; }
        
        [Display(Name = "Max Avail Days (সর্বোচ্চ ছুটি ভোগ)")]
        public int? MaxAvailDays { get; set; }

        [Display(Name = "Leave Status (ছুটির ধরণ)")]
        public string LeaveStatus { get; set; }

        [Display(Name = "Gender (পরিচিতি)")]
        public string LeaveGender { get; set; }

        [Display(Name = "Days Per EL (দিনের সংখ্যা)")]
        public decimal? DaysPerEL { get; set; }

        [Display(Name="Leave Type Rank (ছুটির ক্রম)")]
        public int LeaveTypeRank { get; set; }
        [Display(Name = "Leave Quantity (ছুটির পরিমাণ)")]
        public int? leaveQty { get; set; }
        public string ELAdd { get; set; }

        [Display(Name = "Employee Status")]
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatus { get; set; }

        public string rowSl { get; set; }
        [Display(Name = "Leave Category (ছুটির ধরণ)")]
        public string LeaveCategory { get; set; }
        public bool Ledger { get; set; }

        [Display(Name = "Effective Start Date")]
        public DateTime? EffectiveStartDate { get; set; }

        [Display(Name = "Effective End Date")]
        public DateTime? EffectiveEndDate { get; set; }
        [Display(Name = "Count Weekly Holidays in between (মাঝখানে সাপ্তাহিক ছুটির দিনগুলি গণনা করুন)")]
        public string CountWeeklyHolidaysInBetween { get; set; }
        [Display(Name = "Count Other Holidays in between (মাঝখানে অন্যান্য ছুটির দিনগুলি গণনা করুন)")]
        public string CountOtherHolidaysInBetween { get; set; }
        [Display(Name = "Total Max Leave Days (মোট সর্বোচ্চ ছুটির দিন)")]
        public string TotalMaxLeaveDays { get; set; }

        public IEnumerable<SelectListItem> EligibleFromList { get; set; }
        
        public IEnumerable<SelectListItem> LeaveStatusList { get; set; }
        public IEnumerable<SelectListItem> LeaveGenderList { get; set; }
        public List<SelectListItem> EmployeeStatusList { get; set; }
        public IEnumerable<SelectListItem> LeaveCategoryList { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }

        //public List<string> EmpStatusList { get; set; }
        public List<int> EmployeeStatusIdList { get; set; }


    }
}