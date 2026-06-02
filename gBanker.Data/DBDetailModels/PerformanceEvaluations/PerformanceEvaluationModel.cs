using gHRM.Core.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.PerformanceEvaluations
{
    public class PerformanceEvaluationModel
    {
        public int PerformanceEvaluationId { get; set; }
        public Int64 EmployeeId { get; set; }
        public int TotalSamity { get; set; }
        public int TotalMember { get; set; }
        public int EvaluationYear { get; set; }
        public int EvaluationMonth { get; set; }
        public int TotalLoanee { get; set; }
        public decimal OSP { get; set; }
        public decimal SpecialSavings { get; set; }
        public decimal GeneralSavings { get; set; }
        public decimal LoanDisburse { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal LoanOutstanding { get; set; }
        public int? CurrentDueNo { get; set; }
        public decimal? CurrentDue { get; set; }
        public int? OverDueNo { get; set; }
        public decimal? OverDue { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int64? UpdatedBy { get; set; }

        //additional
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public decimal SavingsTotals => OSP + SpecialSavings + GeneralSavings;
        public decimal  LoanTotals => LoanDisburse + LoanRepaid + LoanOutstanding;
        public decimal DueTotals => CurrentDue??0 + OverDue??0;
        public string EvaluationMonthInText => $"{MonthConstants.GetText(EvaluationMonth.ToString())}";
        public string EvaluationOn => $"{CreateDate.ToString("dd-MMM-yyyy")}";
    }
}
