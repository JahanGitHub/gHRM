using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("OrganizationPFSetup",Schema ="pf")]
   public class OrganizationPFSetup
    {
        [Key]
        public Int16 Id { get; set; }
        //public string PFType { get; set; }
        public int? SelfContribution_ComponentPayrollId { get; set; }
        public int? OfficeContribution_ComponentPayrollId { get; set; }
        public bool IsActive { get; set; }
    }
}
