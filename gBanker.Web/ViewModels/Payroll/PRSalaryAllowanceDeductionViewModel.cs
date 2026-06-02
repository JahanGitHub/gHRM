using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRSalaryAllowanceDeductionViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string DateStartFrom { get; set; }
        public string DateEndTo { get; set; }
        public int PrComponentId { get; set; }
        public string ComponentCategory { get; set; }
        public string ComponentName { get; set; }
        public string PrComponentAmount { get; set; }
        public string PrComponentHour { get; set; }
        public string DeductionDays { get; set; }
        public int ProductId { get; set; }
        public int SerialId { get; set; }
        public int IsProductDependent { get; set; }
        public string Remark { get; set; }
    }
}