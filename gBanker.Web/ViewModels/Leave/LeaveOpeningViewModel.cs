using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class LeaveOpeningViewModel
    {
        public int rowSl { get; set; }

        public int TotalDays { get; set; }

        public int TotalRemainingDays { get; set; }

        public string LeaveTypeName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string StatusName { get; set; }

        public int ELFull { get; set; }

        public int EnjoyFull { get; set; }

        public int BalanceFull { get; set; }

        public int BalanceHalf { get; set; }

        public string LastSaleDate { get; set; }
    }
}