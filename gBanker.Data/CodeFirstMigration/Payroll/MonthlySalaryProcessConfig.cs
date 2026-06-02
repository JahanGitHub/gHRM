namespace gHRM.Data.CodeFirstMigration.Payroll
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prl.SalaryDateConfig")]
    public partial class SalaryDateConfig
    {

        public int Id { get; set; }

        public int DayOfMonthlySalary { get; set; }

        public bool IsCurrentlyUsing { get; set; }

        public bool IsActive { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }


        [NotMapped]
        public string CreateDateInString => CreateDate != null ? ((DateTime)CreateDate).ToString("dd MMM yyyy") : "";
    }
}
