using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("gr.GratuityGlobalConfig")]
    public class GratuityGlobalConfig
    {
        [Key]
        public int GratuityGlobalConfigId { get; set; }
        public int EmployeeStatusId { get; set; }
        public int ServiceAgeFrom { get; set; }
        public int ServiceAgeTo { get; set; }
        public double GratuityTimes { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string EligibleFrom { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
