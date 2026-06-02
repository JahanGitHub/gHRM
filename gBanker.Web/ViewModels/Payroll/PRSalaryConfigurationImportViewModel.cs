using System.Collections.Generic;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRSalaryConfigurationImportViewModel : BaseModel
    {
        public PRSalaryConfigurationImportViewModel()
        {
            this.SalaryConfigurationList = new List<PRSalaryConfigurationViewModel>();
        }

        public List<PRSalaryConfigurationViewModel> SalaryConfigurationList { get; set; }

        public int OfficeId { get; set; }
        public long EmployeeId { get; set; }
        public int NewDesignationId { get; set; }
        public int PromotionId { get; set; }
        public int PromotionTypeId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string PFTypeId { get; set; }
        public double GrossSalary { get; set; }
        public string GradeId { get; set; }
        public string Step { get; set; }
        public bool IsOverTime { get; set; }
        public string MaxOvertimePerDay { get; set; }
        public string MaxOvertimePerMonth { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string LastLoginTime { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string PromotionDate { get; set; }
        public string NextReviewDate { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }

    public class PRSalaryConfigExcelViewModel : BaseModel
    {
        public string EmployeeCode { get; set; }
        public double GrossSalary { get; set; }
        public bool IsOverTime { get; set; }
        public int? MaxOvertimePerDay { get; set; }
        public int? MaxOvertimePerMonth { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankAccountNo { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}