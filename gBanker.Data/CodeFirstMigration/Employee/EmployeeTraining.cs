namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeTraining")]
    public class EmployeeTraining
    {
        [Key]
        public int EmployeeTrainingId { get; set; }

        public long EmployeeId { get; set; }
        public string TrainingTitle { get; set; }
        public string InstituteName { get; set; }
        public int? TrainingCountryId { get; set; }
        public string TrainingTopics { get; set; }
        public string Result { get; set; }
        public DateTime? TrainingDateFrom { get; set; }
        public DateTime? TrainingDateTo { get; set; }
        public string CurrentOfficeTraining { get; set; }
        public DateTime? ApproveAndRejectionDate { get; set; }
        public DateTime? InActiveDate { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public long? approveby { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string OrganisedBy { get; set; }
        public string SupportedBy { get; set; }

        [NotMapped]
        public string CreateDateInString => CreateDate != null ? ((DateTime)CreateDate).ToString("dd MMM yyyy") : "";


    }
}
