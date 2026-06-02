using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class EmployeeSalaryIncentiveViewModel
    {

        public string rowSl { get; set; }
        public int rowSlInt { get; set; }

        public int SalaryIncentiveId { get; set; }

        public int SalaryMonth { get; set; }

        public string SalaryMonthMsg { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public int SalaryYear { get; set; }

        public long EmployeeId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StartDateMsg { get; set; }

        public string EndDateMsg { get; set; }

        [Display(Name = "Component Category")]
        public string ComponentCategory { get; set; }

        [Display(Name = "Component Name")]
        public int PRComponentId { get; set; }

        [Display(Name = "Max Overtime Per Month")]
        public decimal MaxOvertimePerMonth { get; set; }

        [Display(Name = "Overtime Hour")]
        public decimal OvertimeHour { get; set; }

        [Display(Name = "Deduction Days")]
        public string DeductionDays { get; set; }

        [Display(Name = "Overtime Rate")]
        public decimal OvertimeRate { get; set; }

        [Display(Name = "Component Amount")]
        public decimal PRComponentAmount { get; set; }

        public int OfficeId { get; set; }

        public string EmployeeStatus { get; set; }
        public int? EmployeeStatusId { get; set; }

        public int EmployeeTypeId { get; set; }

        public string ComponentName { get; set; }

        public decimal totalAlowance { get; set; }

        public decimal totalDeduction { get; set; }

        public decimal totalSalary { get; set; }

        public decimal totalPauable { get; set; }

        public Nullable<decimal> PRComponentHour { get; set; }

        public string ProductGroup { get; set; }

        public string ProductType { get; set; }

        public string ProductName { get; set; }

        public int? ProductId { get; set; }

        public int? SerialId { get; set; }

        public string Remark { get; set; }

        public IEnumerable<SelectListItem> ComponentList { get; set; }

        public IEnumerable<SelectListItem> YearList { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }

        public IEnumerable<SelectListItem> ComponentCategoryList { get; set; }

        public IEnumerable<SelectListItem> ProductGroupList { get; set; }

        public IEnumerable<SelectListItem> productType_List { get; set; }

        public IEnumerable<SelectListItem> productList { get; set; }

        public IEnumerable<SelectListItem> serialList { get; set; }
    }
}