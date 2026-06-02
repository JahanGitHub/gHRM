using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{

    public class View_EmployeeMonthlySalary
    {
        [Key]
        public int rowSl { get; set; }

        public int SalaryId { get; set; }

        public int SalaryYear { get; set; }

        public int SalaryMonth { get; set; }

        public System.DateTime SalaryDate { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        public int? OfficeTypeId { get; set; }

        public int? OfficeId { get; set; }

        public int? DesignationId { get; set; }

        public string DesignationName { get; set; }

        public int? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int PRComponentId { get; set; }

        public decimal PRComponentAmount { get; set; }

        public string ComponentCategory { get; set; }

        public string TransactionType { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsSendForApproval { get; set; }

        public string BankCode { get; set; }
    
        public Nullable<long> PRSalaryConfigurationId { get; set; }
    }


    public class EmployeeMonthlySalaryModel
    {       
        public int rowSl { get; set; }

        public int SalaryId { get; set; }

        public int SalaryYear { get; set; }

        public int SalaryMonth { get; set; }

        public System.DateTime SalaryDate { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        public int? OfficeTypeId { get; set; }

        public int? OfficeId { get; set; }

        public int? DesignationId { get; set; }       

        public int? DepartmentId { get; set; }

        public int PRComponentId { get; set; }

        public decimal PRComponentAmount { get; set; }

        public string ComponentCategory { get; set; }

        public string TransactionType { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsSendForApproval { get; set; }

        public string BankCode { get; set; }
        public string BankName { get; set; }
        public int? LoanId { get; set; }
        public string Comments { get; set; }
        public long? CreateUser { get; set; }
        public Nullable<long> PRSalaryConfigurationId { get; set; }
    }



    public class View_EmployeeMonthlySalary_Challan
    {
        [Key]
        public int rowSl { get; set; }

        public int SalaryId { get; set; }

        public int SalaryYear { get; set; }

        public int SalaryMonth { get; set; }

        public System.DateTime SalaryDate { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        public int? OfficeTypeId { get; set; }

        public int? OfficeId { get; set; }

        public int? DesignationId { get; set; }

        public string DesignationName { get; set; }

        public int? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int PRComponentId { get; set; }

        public decimal PRComponentAmount { get; set; }

        public string ComponentCategory { get; set; }

        public string TransactionType { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsSendForApproval { get; set; }

        public string BankCode { get; set; }

        public Nullable<long> PRSalaryConfigurationId { get; set; }

        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
    }


    public class EmployeeMonthlySalaryModel_Challan
    {
        public int rowSl { get; set; }

        public int SalaryId { get; set; }

        public int SalaryYear { get; set; }

        public int SalaryMonth { get; set; }

        public System.DateTime SalaryDate { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        public int? OfficeTypeId { get; set; }

        public int? OfficeId { get; set; }

        public int? DesignationId { get; set; }

        public int? DepartmentId { get; set; }

        public int PRComponentId { get; set; }

        public decimal PRComponentAmount { get; set; }

        public string ComponentCategory { get; set; }

        public string TransactionType { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public bool IsSendForApproval { get; set; }

        public string BankCode { get; set; }
        public string BankName { get; set; }
        public int? LoanId { get; set; }
        public string Comments { get; set; }
        public long? CreateUser { get; set; }
        public Nullable<long> PRSalaryConfigurationId { get; set; }
    }
}
