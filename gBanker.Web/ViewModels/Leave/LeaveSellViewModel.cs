using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveSellViewModel : BaseModel
    {
        public string Rowsl { get; set; }

        public int LeaveSellId { get; set; }

        //public string LeaveSellNo { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public int OfficeId { get; set; }

        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        public int DesignationId { get; set; }

        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }

        public int DepartmentId { get; set; }

        [Display(Name = "Department  Name")]
        public string DepartmentName { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Joining Date")]
        //public DateTime JoiningDate { get; set; }

        //[Display(Name = "Confirmation Date")]
        //public DateTime? ConfirmDate { get; set; }

        public string ConfirmDateMsg { get; set; }

        [Display(Name = "Request Date")]
        public DateTime? RequestDate { get; set; }


        [Display(Name = "Encashment Date")]
        public DateTime? SaleDate { get; set; }

        //public string EncashDate { get; set; }

        public string SaleDateMsg { get; set; }

        public int TotalDays { get; set; }

        public decimal EncashedAmount { get; set; }

        [Display(Name = "Authorized")]
        public bool IsAuthorized { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        public string IsApprovedMsg { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? OrderCreateOfficeId { get; set; }

        public string AnulipiTxt { get; set; }

        //public bool IsActive { get; set; }

        public string ActiveStatus { get; set; }

        public bool IsAmountPaid { get; set; }

        public bool? IsPaidWithSalary { get; set; }

        public DateTime? PaymentDate { get; set; }


        public string EligibleDateMsg { get; set; }

        //public string EligibleDateMsgForStaff { get; set; }

        public string NextEligibleDateMsg { get; set; }

        public int? TotalEarnLeave { get; set; }

        public int? TotalLeaveSold { get; set; }

        public string LastSellDateMsg { get; set; }

        public int? TotalEarnLeaveTaken { get; set; }

        public int? AvailableLeave { get; set; }

        public string LeaveSellMessage { get; set; }

        public string LeaveSellSave { get; set; }

        public string ScaleDate { get; set; }

        public decimal? BalanceEl { get; set; }

        public int ELOpeningId { get; set; }

        public string LeaveStartDateMsg { get; set; }

        public string LeaveEndDateMsg { get; set; }

        public int? ELFull { get; set; }

        public int? EnjoyFull { get; set; }

        public int? BalanceFull { get; set; }

        public int? ELHalf { get; set; }

        public int? EnjoyHalf { get; set; }

        public int? BalanceHalf { get; set; }
        public int EncashmentEligibleQuantity { get; set; }
        public int EncashmentEligibleYears { get; set; }

        public string LastSaleDateMsg { get; set; }

        public int? WithSeniority { get; set; }

        public int? WithoutSeniority { get; set; }

        public string Remark { get; set; }

        public int Date { get; set; }
        public List<SelectListItem> DateList { get; set; }

        public int Month { get; set; }
        public List<SelectListItem> MonthList { get; set; }

        public int Year { get; set; }
        public List<SelectListItem> YearList { get; set; }

        public string Payment { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; }
        public int EncashmentQuantity { get; set; }
    }
}