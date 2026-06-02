using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.WelfareFund
{
    [Table("fund.FundSetup")]
    public class FundSetup
    {
        [Key]
        public int Id { get; set; }
        public string FundType { get; set; }
        public string ComponentType { get; set; }
        public decimal ComponentAmount { get; set; }
        public string RatioBasedOn { get; set; }
        public int PRComponentId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }


        [NotMapped]
        public string CreateDateInString => CreateDate != null ? ((DateTime)CreateDate).ToString("dd MMM yyyy") : "";
    }
}
