using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Loan
{
    public class loanStatusViewModel
    {
        public int SlNo { get; set; }
        public string PurposeName { get; set; }
        public string ApplicantCode { get; set; }
        public string ApplicantName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime CreateDate { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public string ApprovalEmpCode { get; set; }
        public string ApprovalEmpName { get; set; }
    }
}