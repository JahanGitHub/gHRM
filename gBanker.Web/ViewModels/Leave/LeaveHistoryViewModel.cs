using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveHistoryViewModel : BaseModel
    {
        public long LeaveId { get; set; }

        [Display(Name = "Leave No.")]
        public string LeaveNo { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = " Type (ধরণ)")]
        public string EmployeeRank { get; set; }

        [Display(Name = "Office(অফিস)")]
        public string OfficeName { get; set; }

        public int DepartmentId { get; set; }
        [Display(Name = "Department(বিভাগ)")]
        public string DepartmentName { get; set; }

        [Display(Name = "Designation(পদবী)")]
        public string DesignationName { get; set; }

        [Display(Name = "Leave Type(ছুটির ধরণ)")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Max Leave Days(সর্বোচ্চ ছুটির দিন)")]
        public int? MaxLeaveDays { get; set; }

        [Display(Name = "Max Leave Days(সর্বোচ্চ ছুটির দিন)")]
        public string MaxLeaveDaysInfo { get; set; }

        public int? MaxAvailDays { get; set; }

        public int? MaxLeaveAccType { get; set; }

        [Display(Name = "Request Date (আবেদনের তারিখ)")]
        public DateTime? LeaveRequestDate { get; set; }

        public string LeaveStartDateMsg { get; set; }

        [Display(Name = "Leave Start Date(ছুটি শুরুর তারিখ)")]
        public DateTime? LeaveStartDate { get; set; }

        public string LeaveEndDateMsg { get; set; }

        [Display(Name = "Leave End Date(ছুটির শেষ তারিখ)")]
        public DateTime? LeaveEndDate { get; set; }

        [Display(Name = "Total Days(মোট দিন)")]
        public decimal? TotalDays { get; set; }
        [Display(Name = "Leave Day Duration (ছুটির দিনের সময়কাল)")]
        public string LeaveDayDuration { get; set; }

        [Display(Name = "Leave Reason(ছুটির কারণ)")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string LeaveReason { get; set; }

        [Display(Name = "Address & Mobile During Leave(ছুটি থাকাকালীন ঠিকানা ও মোবাইল নম্বর)")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string AddressDuringLeave { get; set; }

        [Display(Name = "File attachment")]
        public byte[] LeaveAttachment { get; set; }

        public double? leaveGain { get; set; }
        public int? leaveSell { get; set; }
        public string Rowsl { get; set; }

        public int SlNo { get; set; }
        
         [Display(Name = "Employee Code(কোড)")]
        public string EmployeeCode { get; set; }

         [Display(Name = "Name(নাম)")]
        public string EmployeeName { get; set; }

        public string LeaveTypeName { get; set; }

        public string EmpGender { get; set; }

        public double? LeaveCount { get; set; }       

        public string LeaveCategory { get; set; }

        public int? leaveQty { get; set; }

        public string comment { get; set; }

        public long ? DispatchLeaveId { get; set; }

        public string Remarks { get; set; }

        public string chkEvidence { get; set; }

        public string chkRecommendation { get; set; }

        public int? Adjustment { get; set; }

        public int? ValidApproverCount { get; set; }

        [Display(Name = "Attachment")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string leaveDispatchRemarks { get; set; }

        // public byte[] LeaveAttachment { get; set; }
        // public string LeaveEndDateMsg { get; set; }  

        public string Mode { get; set; }

        [Display(Name = "Recommendation")]
        [DataType(DataType.MultilineText)]
        public string LeaveRecommendation { get; set; }

        [Display(Name = "Leave Note (Remarks)")]
        [DataType(DataType.MultilineText)]

        public string LeaveNote { get; set; }

        [Display(Name = "Leave Header")]
        [DataType(DataType.MultilineText)]
        public string LeaveHeader { get; set; }

        [Display(Name = "Leave Footer")]
        [DataType(DataType.MultilineText)]
        public string LeaveFooter { get; set; }

        [Display(Name = "Join Date")]
        public DateTime? JoinDate { get; set; }

        public string JoinDateMsg { get; set; }

        public string LeaveRequestDateMsg { get; set; }

         [Display(Name = "Leave Up To")]
        public string LeaveUpTo { get; set; }

        public DateTime? LeaveUpToDate { get; set; }

        public long NotificationId { get; set; }

        public int ApprovalDetailId { get; set; }

        public int ApprovalMasterId { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAdjustment { get; set; }
        public int IsAbsentLeave { get; set; }
        public string LeaveStatus { get; set; }

        public string EmpStatus { get; set; }


        public string EmployeeStatus { get; set; }

        public int EmployeeStatusId { get; set; }

        public int OfficeId { get; set; }

        [Display(Name="Responsibility")]
        public string OffcDesignName { get; set; }

        [Display(Name = "Replacement Employee")]
        public long? ReplacementEmployee { get; set; }

        [Display(Name = "Replacement Employee")]
        public string ReplacementEmployeeName { get; set; }
        public string PreviousApprover { get; set; }
        public string SignatureName { get; set; }

        public IEnumerable<SelectListItem> EmployeeList { get; set; }

        public IEnumerable<SelectListItem> LeaveTypeList { get; set; }

        public IEnumerable<SelectListItem> LeaveStatusList { get; set; }
        public IEnumerable<SelectListItem> LeaveDayDurationList { get; set; }

    }
}