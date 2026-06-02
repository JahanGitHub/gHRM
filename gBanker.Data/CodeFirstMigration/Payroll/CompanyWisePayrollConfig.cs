using gHRM.Core.Utilities.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.CompanyWisePayrollConfig")]
    public class CompanyWisePayrollConfig
    {
        [Key]
        public int Id { get; set; }
        public string CompanyCode { get; set; }
        public string PayrollType { get; set; }
        public string Description { get; set; }
        public int NoOfSalaryDays { get; set; }
        public string PayrollConfigurationType { get; set; }
        public bool IsActive { get; set; }
        public Int64? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public Int64? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        //additional
        [NotMapped]
        public string CompanyName => GHRMPlusCompanyConstants.GetText(CompanyCode);

        [NotMapped]
        public string PayrollTypeInText => PayrollTypeConstants.GetText(PayrollType);

        [NotMapped]
        public string CreateDateInString => CreateDate != null ? ((DateTime)CreateDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "";
    }
}
