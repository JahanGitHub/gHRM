using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.payroll
{
    public class EmployeeMonthlySalaryViewModel
    {
        public int SalaryId { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public System.DateTime SalaryDate { get; set; }
        public long EmployeeId { get; set; }
        public long? PRSalaryConfigurationId { get; set; }
        public int PRComponentId { get; set; }
        public decimal PRComponentAmount { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string OffcDesignName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string rowSl { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public string TransactionType { get; set; }
        //public decimal PRComponentAmount { get; set; }
        public string ComponentName { get; set; }

    }
}