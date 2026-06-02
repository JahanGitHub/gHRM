using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.PRDeposit")]
    public class PRDeposit
    {
        [Key]
        public int Id { get; set; }        
        public int PRComponentId { get; set; }    
        public int ComponentPayrollId { get; set; }
        public string ComponentCategory { get; set; }
        public string ComponentName { get; set; }        
        public int ComponentGroupId { get; set; }        
        public string ComponentGroup { get; set; }        
        public string DepositeType { get; set; }
        public int OfficeLocationId { get; set; }        
        public int EmployeeType { get; set; }        
        public int EmployeeStatusId { get; set; }        
        public string EmployeeStatusName { get; set; }        
        public int IsDepositRequired { get; set; }        
        public int ReturnDepositeOnEmployeeStatusId { get; set; }        
        public string TransactionType { get; set; }
        public decimal? MaximumLimit { get; set; }
        public decimal? MinimumLimit { get; set; }        
        public int NoOfSalaryDays { get; set; }        
        public DateTime EffectiveStartDate { get; set; }        
        public DateTime EffectiveEndDate { get; set; }        
        public bool IsActive { get; set; }        
        public Int64 CreateUser { get; set; }        
        public DateTime CreateDate { get; set; }
        public Int64? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }        
    }
}
