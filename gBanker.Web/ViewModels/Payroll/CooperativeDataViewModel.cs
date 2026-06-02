using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Payroll
{
    public class CooperativeDataViewModel
    {
        public long EmployeeId { get; set; }
        public int? SummaryID { get; set; }
        public decimal? PRComponentAmount { get; set; }
    }
}