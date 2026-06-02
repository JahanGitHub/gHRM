namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_EmployeeTraining
    {
        [Key]
        public int? RowSl { get; set; }
        public int EmployeeTrainingId { get; set; }
        public long? EmployeeId { get; set; }
        public string TrainingTitle { get; set; }
        public string InstituteName { get; set; }
        public int? TrainingCountryId { get; set; }
        public string TrainingTopics { get; set; }
        public string Result { get; set; }
        public string TDF { get; set; }
        public string TDT { get; set; }
        public string AR { get; set; }
        public string CurrentOfficeTraining { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string CountryName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public long? approveby { get; set; }
        public string EmployeeTrainingStatus { get; set; }

        public string SupportedBy { get; set; }
        public string OrganisedBy { get; set; }

    }
}
