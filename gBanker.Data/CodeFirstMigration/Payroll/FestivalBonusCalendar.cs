using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.FestivalBonusCalendar")]
    public class FestivalBonusCalendar
    {
        [Key]
        public int Id { get; set; }
        public string ComponentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int IsActive { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
