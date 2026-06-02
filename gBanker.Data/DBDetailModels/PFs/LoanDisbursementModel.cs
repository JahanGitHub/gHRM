using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.OverTimes
{
    public class LoanDisbursementModel
    {
        public Int64 EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public decimal DisburseAmount { get; set; }
        public decimal IntersetRate { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string DisburseDate { get; set; }
    }
}
