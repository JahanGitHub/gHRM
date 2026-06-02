namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCaseEnquiryOfficer")]
    public partial class DiscCaseEnquiryOfficer
    {
        [Key]
        public int CaseEnquiryOfficerId { get; set; }

        public int CaseMasterId { get; set; }

        public long EmployeeId { get; set; }

        [StringLength(500)]
        public string DespatchNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnquiryOfficerAssignedDt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvestigationDt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReportReceivedDt { get; set; }

        [StringLength(500)]
        public string EnquiryRemarks { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CrimeFindOutFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CrimeFindOutTo { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
