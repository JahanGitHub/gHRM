namespace gHRM.Data.CodeFirstMigration.Cooperative
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CooperativeLedger",Schema ="coo")]
    public class CooperativeLedger
    {
        [Key, Column(Order = 0)]
        public int SummaryMasterId { get; set; }
        [Key, Column(Order = 1)]
        public int InstallmentYear { get; set; }
        [Key, Column(Order = 2)]
        public int InstallmentMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Key, Column(Order = 3)]
        public string InstallmentType { get; set; }// Installment Opening=InsO,Installment=Ins,Installment Payment=InsP,InterestOpening-IntO,Interest=Int,Interest Payment=IntP,
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Remark { get; set; }
        public int? CreateBy { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}