using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("training.ResultGrade")]
    public class ResultGrade
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
