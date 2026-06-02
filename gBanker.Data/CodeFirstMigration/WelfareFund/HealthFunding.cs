using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.WelfareFund
{
    [Table("fund.HealthFunding")]
    public class HealthFunding
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public int PurposeId { get; set; }
        public decimal FundAmount { get; set; }
        public string remarks { get; set; }
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}
