namespace gHRM.Data.CodeFirstMigration.PerformanceEvaluations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PerformanceEvaluation")]
    public class PerformanceEvaluation
    {
        [Key]
        public int PerformanceEvaluationId { get; set; }

        public long EmployeeId { get; set; }

        public int EvaluationYear { get; set; }
        public int EvaluationMonth { get; set; }
        public DateTime EvaluationDate { get; set; }

        public int TotalSamity { get; set; }

        public int TotalMember { get; set; }

        public int TotalLoanee { get; set; }

        public decimal OSP { get; set; }

        public decimal SpecialSavings { get; set; }

        public decimal GeneralSavings { get; set; }

        public decimal LoanDisburse { get; set; }

        public decimal LoanRepaid { get; set; }

        public decimal LoanOutstanding { get; set; }
        
        public int? CurrentDueNo { get; set; }
        public int? OverDueNo { get; set; }

        public decimal? CurrentDue { get; set; }

        public decimal? OverDue { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }
        public int OfficeId { get; set; }
    }
}
