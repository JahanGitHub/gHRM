using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EASSOvertimeHourConfiguration")]
    public class EASSOvertimeHourConfiguration
    {
        [Key]
        public int OTCalcId { get; set; }
        public int EASSDesignationId { get; set; }
        public int EASSCompanyId { get; set; }
        public decimal RateForOvertimeHour { get; set; }
        public decimal OvertimeHour { get; set; }
        public int CalculationRank { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
    }
}
