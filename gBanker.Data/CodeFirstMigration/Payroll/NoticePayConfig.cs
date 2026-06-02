using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.NoticePayConfig")]
    public class NoticePayConfig
    {
        [Key]
        public int Id { get; set; }
        public int NoticePeriod { get; set; }
        public bool IsCalcFromBasic { get; set; }
        public int SalaryPer { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
	}
}
