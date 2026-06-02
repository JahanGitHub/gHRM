using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Itenso.TimePeriod;

namespace gHRM.Web.ViewModels.Loan
{
    public class EmployeeLoanRegisterViewModel
    {
        public int LoanId { get; set; }
        public string EmployeeCode { get; set; }
        public int PRComponentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int NoOfInstallMent { get; set; }
        public Date? LoanStartDate { get; set; }
        public Date? LoanEndDate { get; set; }
        public string EmployeeName { get; set; }
        public string LoanType { get; set; }
        public string EmployeeDepartment { get; set; }
        public string EmployeeDesignation { get; set; }
        public int ComponentType { get; set; }
        public string TotalYear { get; set; }
        public string MinimumLimit { get; set; }
        public string MaximumLimit { get; set; }
        public int TotalNoOfInstallment { get; set; }
        public List<SelectListItem> LoanTypeList { get; set; }
        public List<SelectListItem> ComponentTypeList { get; set; }
        public int LoanNo { get; set; }

        public long EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public decimal? MinLoanAmount { get; set; }
        public decimal? MaxLoanAmount { get; set; }
        public string ComponentCategory { get; set; }
        public string ComponentName { get; set; }
        public int PRComponentID { get; set; }



        public int StatusId { get; set; }
        public string EmployeeStatus { get; set; }
        public int OfficeLocationId { get; set; }
        public string OfficeLocationName { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }

        public decimal ActualLoan { get; set; }
        public int InstallmentMonth { get; set; }
        public decimal InsuranceCharge { get; set; }


        public IEnumerable<SelectListItem> LoneCalculationList { get; set; }
        public int? LoanCalculationId { get; set; }
    }
}