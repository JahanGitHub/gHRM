using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployementType")]
    public class EmployementType
    {
        [Key]
        public int EmployementTypeId { get; set; }
        public string EmployementTypeName { get; set; }
        public int ViewOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long? UpdateBy { get; set; }
    }
}
