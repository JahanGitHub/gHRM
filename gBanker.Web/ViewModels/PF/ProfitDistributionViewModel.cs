using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class ProfitDistributionViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal OrgContribution { get; set; }
        public decimal SelfContribution { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal ProfitContribution { get; set; }
    }
}