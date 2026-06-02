using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Loan
{
    public class ApplicantGridViewModel
    {
        public int Id { get; set; }
        public string LoanType { get; set; }
        public string Purpose { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public string NotificationStatus { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime? DisburseDate { get; set; }
        public int? DisburseAmount { get; set; }
        public int? NoOfInstallment { get; set; }
        public DateTime? LastInstallmentDate { get; set; }
        public bool? IsClose { get; set; }
    }
}