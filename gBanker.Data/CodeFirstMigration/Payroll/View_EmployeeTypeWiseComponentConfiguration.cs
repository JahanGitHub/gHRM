using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.View_EmployeeTypeWiseComponentConfiguration")]
    public class View_EmployeeTypeWiseComponentConfiguration
    {
        [Key]
        public Nullable<int> rowSl { get; set; }
        public string EmployeeTypeName { get; set; }
        public string ComponentGroupName { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public decimal ComponentAmount { get; set; }
        public string RatioBasedOn { get; set; }
        public string TransactionType { get; set; }
        public string ComponentCategory { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public int PRComponentGroupId { get; set; }
        public int PRComponentId { get; set; }
        public Nullable<decimal> MaximumLimit { get; set; }
        public Nullable<decimal> MinimumLimit { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeStatus { get; set; }
        public string SalaryAccCode { get; set; }
        public string TransactionTypeView { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string EffectiveEndDateMsg { get; set; }
        public string AccountName { get; set; }
        public bool IsProvidentFundComponent { get; set; }
        public int PFTypeId { get; set; }

    }
}
