namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeReference")]
    public partial class EmployeeReference
    {
        [Key]
        public long ReferenceId { get; set; }

        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceName { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceOccupation { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceDesignation { get; set; }

        [StringLength(50)]
        public string ConnectionWithEmployee { get; set; }

        public string ContactAddress { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "text")]
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

        public virtual Employee Employee { get; set; }
    }
}
