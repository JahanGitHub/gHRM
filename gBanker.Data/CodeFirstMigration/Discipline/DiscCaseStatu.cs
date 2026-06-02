using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.Discipline
{
    [Table("disc.discCaseStatus")]
    public partial class DiscCaseStatu
    {
        [Key]
        public long CaseStatusId { get; set; }

        public int CaseMasterId { get; set; }

        public int StatusId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StatusDt { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

    }// END Class
}// END Namespace
