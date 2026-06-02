namespace gHRM.Data.CodeFirstMigration.Cooperative
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CooperativeSummaryConfiguration", Schema = "coo")]
    public partial class CooperativeConfiguration
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ComponentId { get; set; }
        public int MonthlyInstallment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ActivityStatus { get; set; }// Active=A,Delete=D,Close=C,Opening=O
        public decimal? ClosePaymentAmount { get; set; }
        public int? CreateBy { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

    }
}
