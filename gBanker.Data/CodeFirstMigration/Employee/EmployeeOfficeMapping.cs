using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeOfficeMapping")]
    public partial class EmployeeOfficeMapping
    {
        [Key]
        public int OfficeMappingID { get; set; }

        public short EmployeeId { get; set; }

        public int OfficeID { get; set; }

        public bool? IsActive { get; set; }

        public long? CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }

        public virtual Office Office { get; set; }
    }
}
