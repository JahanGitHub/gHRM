using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Loan
{
    public class ApprovalOrDisbursGridViewModel
    {
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LoanType { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public string PurposeName { get; set; }

        public string LoanNo { get; set; }
    }

    public class ApprovalOrDisbursGridViewModel3
    {
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LoanType { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public string PurposeName { get; set; }

        public string ApplicationDate { get; set; }
        public string LoanNo { get; set; }

        public string LoanDate { get; set; }
    }
}