using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRWorkAreaViewModel
    {
        [Display(Name = "Work Area Name")]
        public string WorkAreaName { get; set; }
        [Display(Name = "PRWorkAreaID")]
        public int PRWorkAreaID { get; set; }
        //PRTranType Table

        public int PRTranTypeID { get; set; }
        [Display(Name = "Transaction Type")]
        public string TranType { get; set; }
        [Display(Name = "Transaction Type Description")]
        public string TranTypeDescription { get; set; }
        public string SummaryType { get; set; }

        [Display(Name = "Office Type")]
        public int? OfficeTypeId { get; set; }


        [Display(Name = "Bank Code")]
        public string BankCode { get; set; }

        public IEnumerable<SelectListItem> SummaryList { get; set; }
        public IEnumerable<SelectListItem> ReportList { get; set; }

        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

        [Display(Name = "Report Name")]
        public string ReportName { get; set; }

        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public int Year { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public int Month { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }

        [Display(Name ="Branch")]
        public int BranchId { get; set; }
        public IEnumerable<SelectListItem> BranchList { get; set; }

        [Display(Name = "Account")]
        public int AccountId { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }

        [Display(Name = "Salary Type")]
        public string SalaryType { get; set; }
        public IEnumerable<SelectListItem> SalaryTypeList { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }

        [Display(Name = "Component Type")]
        public string ComponentType { get; set; }
        public IEnumerable<SelectListItem> ComponentTypeList { get; set; }

        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        public IEnumerable<SelectListItem> ComponentNameList { get; set; }
        public IEnumerable<SelectListItem> HOList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }

        public int SalaryDay { get; set; }

        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }

        public string EmployeeName { get; set; }

        public bool IsHeadOffice { get; set; }
        public long PersonToContactFromBankId { get; set; }


        [Display(Name = "Date From")]
        public string DateFrom { get; set; }
        [Display(Name = "Date To")]
        public string DateTo { get; set; }


    }
}