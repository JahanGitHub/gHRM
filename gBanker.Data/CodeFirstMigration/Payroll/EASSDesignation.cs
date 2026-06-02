using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EASSDesignation")]
    public partial class EASSDesignation
    {
        [Key]
        public int EASSDesignationId { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public string Details { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
    }
}
