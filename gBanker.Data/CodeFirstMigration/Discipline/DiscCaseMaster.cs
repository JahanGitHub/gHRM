namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCaseMaster")]
    public partial class DiscCaseMaster
    {

        [Key]
        public int CaseMasterId { get; set; }

        [StringLength(150)]
        public string CaseNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CaseDateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CaseDateTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AuditFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AuditTo { get; set; }

        [StringLength(2)]
        public string CaseType { get; set; }

        [StringLength(500)]
        public string CaseDescription { get; set; }
        public int? CrimeLocation { get; set; }

        public long? DealOfficerId { get; set; } //kk

        public long? EnqueryOfficerId { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? EnquiryOfficerAssignedDt { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? InvestigationDt { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? ReportReceivedDt { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? CrimeFindOutFrom { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? CrimeFindOutTo { get; set; }

        //[StringLength(500)]
        //public string EnquiryRemarks { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

    }// END CLASS
} // END NAmespace
