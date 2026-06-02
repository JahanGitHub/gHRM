using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeePublication")]
    public class EmployeePublication
    {
        [Key]
        public int PublicationId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string PublicationName { get; set; }
        public string PublicationDetail { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
