using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Payroll
{
    public class UpdatePreviousLoanAsClosedModel
    {
        public int EmployeeId { get; set; }
        public int LoanInstallmentDetailId { get; set; }
        public string PreviousLoanStatus { get; set; }
        public string NewLoanStatus { get; set; }       
    }
}
