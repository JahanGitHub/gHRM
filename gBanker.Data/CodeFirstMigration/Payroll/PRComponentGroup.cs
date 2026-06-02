using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.PRComponentGroup")]
    public class PRComponentGroup
    {
        [Key]
        public int PRComponentGroupID { get; set; }
        public string ComponentGroupName { get; set; }
        public string ComponentGroupShortName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
