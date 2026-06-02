using gHRM.Core.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Payroll
{
    public class DBPRComponentListViewModel_designation
    {
        public int PRComponentID { get; set; }
        public string AccountName { get; set; }
        public int? ComponentPayrollId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public decimal ComponentAmount { get; set; }
        public string TransactionType { get; set; }
        public string AccountCode { get; set; }
        public string LoanAccCode { get; set; }
        public string InterestAccCode { get; set; }
        public string IncomeAccCode { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public string EffectiveEndDate { get; set; }

        public string EffectiveEndDateMsg { get; set; }
        public int PRComponentGroupID { get; set; }
        public string ComponentCategory { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string RatioBasedOn { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string[] EmpStatusList { get; set; }
        public int[] EmployeeStatusIdList { get; set; }
        public string ComponentGroupName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ValidateDurtion { get; set; }
        public string SalaryChangesByComponent { get; set; }
        public Nullable<decimal> MaximumLimit { get; set; }
        public Nullable<decimal> MinimumLimit { get; set; }
        public int? OfficeLocationId { get; set; }
        public decimal? InterestRate { get; set; }
        public int? LoanCalculationId { get; set; }
        public string OfficeLocationName { get; set; }
        public int[] OffLocationList { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public bool IsProductDependent { get; set; }
        public bool? SalaryEffect { get; set; }
        public bool IsSalaryEffect { get; set; }
        public bool? IsProvidentFundComponent { get; set; }
        public bool? IsAdjustable { get; set; }
        public bool? IsSalaryImpactProhibited { get; set; }
        public string SalaryRoundType { get; set; }
        public int? PFTypeId { get; set; }
        public string EmployeeStatusName { get; set; }
        public string EmployeeTypeName { get; set; }
        //additional
        public string ComponentTypeInText => SalaryCalculationTypeConstants.GetText(ComponentType);
        public string TransactionTypeInText => SalaryAccountTransactionTypeConstants.GetText(TransactionType);
        public string RatioBasedOnInText => SalaryRatioConstants.GetText(RatioBasedOn); 
        public string PFTypeInText => PFTypeId>0?ProvidentFundTypeConstants.GetText(PFTypeId.ToString())
                                            : ProvidentFundTypeConstants.GetText(ProvidentFundTypeConstants.NotApplicable);     
        
       
         public string DesignationName { get; set; }
		 public string DesignationShortName { get; set; }
		 public int DesignationId { get; set; }
		 public string DesignationCode { get; set; }

    }
}
