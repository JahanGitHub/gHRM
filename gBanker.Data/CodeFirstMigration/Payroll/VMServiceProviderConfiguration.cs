using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("VMServiceProviderConfiguration")]
    public class VMServiceProviderConfiguration
    {
        [Key]
        public int VMServiceProviderConfigurationId { get; set; }
        public int VMServiceTypeId { get; set; }
        public int ServiceProviderId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
    }
}
