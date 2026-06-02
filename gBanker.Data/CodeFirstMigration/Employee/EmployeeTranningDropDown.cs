namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeTranningDropDown")]
    public partial class EmployeeTranningDropDown
    {
        [Key]
        public int EmployeeTrainingDropDownId { get; set; }

        public string EmployeeTrainingDropDownName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
    }
}
