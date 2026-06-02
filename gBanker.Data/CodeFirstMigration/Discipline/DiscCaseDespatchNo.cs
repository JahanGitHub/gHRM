 

namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCaseDespatchNo")]
    public partial class DiscCaseDespatchNo
    {

        [Key]
        public int DespatchId { get; set; }

        public int? CaseMasterId { get; set; }

        public long? EmployeeId { get; set; }

        public int? CrimeId { get; set; }

        [StringLength(500)]
        public string DespatchNo { get; set; }

        [StringLength(50)]
        public string DespatchType { get; set; }
        public DateTime? DespatchDate { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }


    }// End Class
} // End Namespace
