namespace gHRM.Data.CodeFirstMigration.PerformanceEvaluations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PerformanceEvaluationHistory")]
    public class PerformanceEvaluationHistory
    {
        [Key]        
        public int PerformanceEvaluationHistoryId { get; set; }
        public int PerformanceEvaluationId { get; set; }     
        public DateTime EvaluationHistoryDate { get; set; }        
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
        public decimal? CurrentDue { get; set; }
        public int? OverDueNo { get; set; }
        public decimal? OverDue { get; set; }        
        public DateTime CreateDate { get; set; }        
        public Int64 CreatedBy { get; set; }
        public int OfficeId { get; set; }
    }
}
