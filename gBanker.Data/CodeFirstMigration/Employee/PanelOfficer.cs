using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("PanelOfficer")]
    public class PanelOfficer
    {
        [Key]
        public int ID { get; set; }
        public long EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public DateTime AssignDt { get; set; }
        public DateTime? ReleaseDt { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreatrDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
