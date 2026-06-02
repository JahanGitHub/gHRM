using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("VMServiceProvider")]
    public class VMServiceProvider
    {
        [Key]
        public int VMServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
    }
}
