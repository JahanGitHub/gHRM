namespace gHRM.Data.CodeFirstMigration.Payroll
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prl.OvertimeException")]
    public partial class OvertimeException
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ExceptionType { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime EffectiveEndDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
