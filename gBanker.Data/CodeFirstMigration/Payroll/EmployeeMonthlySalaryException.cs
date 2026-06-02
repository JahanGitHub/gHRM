using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeMonthlySalaryException")]
    public partial class EmployeeMonthlySalaryException
    {
        [Key]
        public int Id { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<System.DateTime> EffectiveDateFrom { get; set; }
        public Nullable<System.DateTime> EffectiveDateTo { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
