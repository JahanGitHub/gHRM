namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspAdminPasswordTable")]
    public partial class AspAdminPasswordTable
    {
        [Key]
        public int PasswordId { get; set; }

        [StringLength(2)]
        public string UserType { get; set; }

        public long? EmployeeID { get; set; }

        public int? OfficeID { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserPwd { get; set; }
    }
}
