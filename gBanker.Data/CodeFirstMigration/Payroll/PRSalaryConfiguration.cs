using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.PRSalaryConfiguration")]
    public class PRSalaryConfiguration
    {
        [Key]
        public long PRSalaryConfigurationID { get; set; }
        public int? OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public decimal ComponentAmount { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        //public int? IncrementYear { get; set; }
        //public int? IncrementMonth { get; set; }

        //public int OfficeID { get; set; }
    }
}
