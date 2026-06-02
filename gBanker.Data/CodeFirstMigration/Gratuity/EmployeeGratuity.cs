using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("gr.EmployeeGratuity")]
    public class EmployeeGratuity
    {
        public long EmployeeGratuityId { get; set; }
        public long EmployeeId { get; set; }
        public int GratuityGlobalConfigId { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime SalaryDate { get; set; }
        public decimal BasicSalary { get; set; }
        public string EligibleFrom { get; set; }
        public int SerMonth { get; set; }
        public double CurGratuity { get; set; }
        public double CumGratuity { get; set; }
        public double GratuityTimes { get; set; }
        public bool IsActive { get; set; }
        public bool IsSendForApproval { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
