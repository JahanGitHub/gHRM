using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanDetailViewModel : BaseModel
    {
        public int LoanDetailId { get; set; }

        public int? LoanMasterId { get; set; }

        public int? PRComponentId { get; set; }

        public int? EmployeeId { get; set; }
        public DateTime? InstallmentDate { get; set; }

        public decimal? PrincipalAmount { get; set; }

        public decimal? InterestAmount { get; set; }

        public decimal? EndingBalance { get; set; }

        public decimal? InstallmentAmount { get; set; }

        public decimal? AmountPaid { get; set; }
    }
}