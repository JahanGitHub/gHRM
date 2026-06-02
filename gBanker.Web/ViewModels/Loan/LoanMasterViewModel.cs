using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanMasterViewModel : BaseModel
    {
        public int LoanMasterId { get; set; }

        public int? PRComponentId { get; set; }

        public int? EmployeeId { get; set; }

        public decimal? TotalAppliedAmount { get; set; }

        public decimal? InsuranceCharge { get; set; }

        public decimal? ProvisionAmount { get; set; }

        public decimal? OpeningAmount { get; set; }

        public decimal? ActualLoanAmount { get; set; }

        public decimal? InterestRate { get; set; }

        public int? NoOfInstallment { get; set; }
        public DateTime? LoanStartDate { get; set; }
        public DateTime? LoanEndDate { get; set; }

        public int? LoanCalculationId { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsRejected { get; set; }

        public bool? IsSendForApproval { get; set; }

        public int? LoanNo { get; set; }

        public decimal? TotalPrincipalAmount { get; set; }

        public decimal? TotalInterestAmount { get; set; }

        public decimal? TotalPayable { get; set; }

        public int? CurrentStatusId { get; set; }

        public bool? IsActive { get; set; }
    }
}