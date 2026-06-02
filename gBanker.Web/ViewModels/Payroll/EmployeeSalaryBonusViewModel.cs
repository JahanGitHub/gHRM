using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class EmployeeSalaryBonusViewModel
    {
        public int rowSl { get; set; }

        public int ESBonusId { get; set; }

        public long EmployeeId { get; set; }
        public int ComponentId { get; set; }
        public double BonusAmount { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryYearMsg { get; set; }
        public string SalaryMonth { get; set; }
        public string SalaryMonthMsg { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string ComponentName { get; set; }
        public string ComponentNameMsg { get; set; }
        public string BonusProcessingDate { get; set; }
        public DateTime BonusDate { get; set; }
        public int IsApproved { get; set; }
        public int IsRejected { get; set; }
        public string BankCode { get; set; }
        public int RevStampDeduction { get; set; }
        public string ReportType { get; set; }
        [Display(Name = "Office Type")]
        public int OfficeTypeId { get; set; }

        [Display(Name = "Zone Name")]
        public string ZoneId { get; set; }
        [Display(Name = "Area Name")]
        public string AreaId { get; set; }
        [Display(Name = "Unit Name")]
        public string UnitId { get; set; }

        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }


        [Display(Name = "Bonus Type")]
        public string SalaryType { get; set; }
        public IEnumerable<SelectListItem> SalaryTypeList { get; set; }


        [Display(Name = "Report Name")]
        public string ReportName { get; set; }

        public IEnumerable<SelectListItem> ReportTypeList { get; set; }

        public long? PersonToContactFromBankId { get; set; }
        public List<SelectListItem> ComponentList { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> RevStampDeductionList { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }



        public List<SelectListItem> ReportList { get; set; }

    }
}