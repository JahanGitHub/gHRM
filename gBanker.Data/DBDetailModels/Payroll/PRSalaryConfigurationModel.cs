using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Payroll
{
    public class PRSalaryConfigurationModel
    {
        public long PRSalaryConfigurationID { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public decimal ComponentAmount { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
    }
}
