using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("emp.ResignNotice")]
    public class ResignNotice
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime InformDate { get; set; }
        public DateTime ResignDate { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
	}
}
