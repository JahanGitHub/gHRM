using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EASSCompany")]
    public partial class EASSCompany
    {
        [Key]
        public int EASSCompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyDetail { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
    }
}
