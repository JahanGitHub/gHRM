using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Payroll
{
    public class View_officeXSalaryAfterApproval
    {
        public string ComponentName { get; set; }
        public string TransactionType { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public decimal PRComponentAmount { get; set; }

    }
}