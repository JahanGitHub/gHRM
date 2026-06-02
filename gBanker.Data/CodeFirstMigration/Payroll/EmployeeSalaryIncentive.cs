using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeSalaryIncentive")]
    public class EmployeeSalaryIncentive
    {
        [Key]
        public int SalaryIncentiveId { get; set; }
        //public int SalaryMonth { get; set; }
        //public int SalaryYear { get; set; }
        public long EmployeeId { get; set; }
        public int PRComponentId { get; set; }
        public int? ProductId { get; set; }
        public int? SerialId { get; set; }
        public decimal PRComponentAmount { get; set; }
        public Nullable<decimal> PRComponentHour { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }

        //additional
        [NotMapped]
        public string SalaryRoundType { get; set; }

    }
}
