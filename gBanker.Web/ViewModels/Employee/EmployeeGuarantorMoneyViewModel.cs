using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeGuarantorMoneyViewModel
    {

        public long ID { get; set; }

        public string BranchName { get; set; }

        public decimal? balance { get; set; }

        public double? deposit { get; set; }
        public string TransactionType { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public string TransactionDate { get; set; }

        public decimal TransactionAmount { get; set; }

        public string  PaymentType { get; set; }
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

        public string  BankName { get; set; }

        public string  AccountNo { get; set; }


        [Display(Name = "Cheque No")]
        public string  CheckNo { get; set; }

        public long? EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string ReportType { get; set; }

        public string BloodGroup { get; set; }

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

        public string EmployeeStatus { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        [Display(Name = "Department (বিভাগ)")]
        public int DepartmentId { get; set; }

        [Display(Name = "Employment Type (কাজের ধরন)")]
        public string EmploymentType { get; set; }

        [Display(Name = "Designation (পদবী)")]
        public int DesignationId { get; set; }

        [Display(Name = "Section (সেকশন)")]
        public string Section { get; set; }

        [Display(Name = "Responsibility (দায়িত্ব)")]
        public int ResponsibilityId { get; set; }

        [Display(Name = "Service Duration in Company")]
        public string Age { get; set; }

        [Display(Name = "Service Duration in Office")]
        public string AgeOffice { get; set; }

        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        public string CurrentOfficeType { get; set; }

        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }

        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }

        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }
        public int EmployeeCurrentOfficeId { get; internal set; }

        public string SNO { get; set; }
    }
}