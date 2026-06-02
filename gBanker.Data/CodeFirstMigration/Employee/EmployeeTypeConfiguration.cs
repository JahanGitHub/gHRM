using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeTypeConfiguration")]
    public class EmployeeTypeConfiguration
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public bool IsActive { get; set; }

    }
}
