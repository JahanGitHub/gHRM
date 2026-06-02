using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.PRSalaryRegister")]
    public class PRSalaryRegister
    {
         [Key]
        public long PRSalaryRegisterID { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public System.DateTime SalaryDate { get; set; }
        public long? PRSalaryConfigurationID { get; set; }//PRSalaryConfiurationID
        public int? OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public decimal ComponentAmount { get; set; }
        public Nullable<int> PRTranTypeID { get; set; }
        public bool IsPosted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public Nullable<long> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string ComponentName { get; set; }
    }
}
