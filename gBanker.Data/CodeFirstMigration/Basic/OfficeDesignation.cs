using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("OfficeDesignation")]
    public partial class OfficeDesignation
    {
        [Key]
        public int OfficeDesignationId { get; set; }

        [StringLength(100)]
        public string OffcDesignName { get; set; }

        [StringLength(100)]
        public string OffcDesignNameBn { get; set; }

        [StringLength(5)]
       
        public string OffcType { get; set; }

        public int DesignationOrder { get; set; }

        public bool? IsSectionDependent { get; set; }

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
