using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration
{
    
    [Table("Employee")]
    public partial class Employee
    {
        public Employee()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();        
            EmployeeFamilyInfoes = new HashSet<EmployeeFamilyInfo>();       
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeCode { get; set; }


        [Required]
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [StringLength(100)]
        public string EmployeeNameBng { get; set; }


        public int? CompanyId { get; set; }

        public int? BranchId { get; set; }

        public int? OfficeId { get; set; }

        public int? DepartmentId { get; set; }

        public int? SectionId { get; set; }

        public int? DesignationId { get; set; }

        public int? OfficeDesignationId { get; set; }

        public int? SignatureDesignationId { get; set; }

        public string EmployeeRank { get; set; }

        public int? EmployeeTypeId { get; set; }

        public int? EmploymentTypeId { get; set; }

        //[StringLength(10)]
        //public string EmployeeStatus { get; set; }

        public int EmployeeStatusId { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? DateOfEmployeeStatus { get; set; }

        public int? StatusPeriodInMonth { get; set; }

        public DateTime? StatusFromDate { get; set; }

        public DateTime? StatusToDate { get; set; }

        public DateTime? StatusDate { get; set; }

        public int? SeniorityLoss { get; set; }

        public string StatusChangeComment { get; set; }

        [StringLength(500)]
        public string TerminationCause { get; set; }

        public int? AgreementPeriodInMonth { get; set; }

        public DateTime? AgreementFromDate { get; set; }

        public DateTime? AgreementToDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime FirstJoiningDate { get; set; }

        public DateTime? ConfirmationDate { get; set; }        

        public Nullable<decimal> GrossSalary { get; set; }

        public Nullable<decimal> BasicSalary { get; set; }

        public string BankAccountNo { get; set; }

        public string BankName { get; set; }

        public string BankBranchName { get; set; }

        public int? GradeId { get; set; }

        public int? Step { get; set; }
        public Nullable<decimal> FractionStep { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstDateOfScale { get; set; }

        public Nullable<System.DateTime> EffectiveStartDate { get; set; }

        public Nullable<System.DateTime> EffectiveEndDate { get; set; }

       // public int? IncrementYearFrom { get; set; }

        //public Nullable<int> IncrementMonth { get; set; }

        public bool? IsPFApplicable { get; set; }

        public bool? IsPFClossed { get; set; }

        public Nullable<bool> IsOverTime { get; set; }

        //public Nullable<decimal> OvertimeHour { get; set; }

        public Nullable<decimal> OvertimeRate { get; set; }

        public Nullable<decimal> MaxOvertimePerDay { get; set; }

        public Nullable<decimal> MaxOvertimePerMonth { get; set; }

        public Nullable<decimal> TotalEarnings { get; set; }

        [StringLength(50)]
        public string TinNo { get; set; }

        public Nullable<System.DateTime> LoginTime { get; set; }

        public Nullable<System.DateTime> LogoutTime { get; set; }

        public Nullable<System.DateTime> LastLoginTime { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        public string BirthPlace { get; set; }

        [StringLength(500)]
        public string PermanentAddress { get; set; }

        [StringLength(500)]
        public string PresentAddress { get; set; }

        [StringLength(50)]
        public string MaritalStatus { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(50)]
        public string NationalId { get; set; }

        [StringLength(50)]
        public string Religion { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string OfficialEmail { get; set; }

        public string BloodGroup { get; set; }

        public string PassportNo { get; set; }

        public DateTime? PassportIssueDate { get; set; }

        public DateTime? PassportExpireDate { get; set; }

        [StringLength(15)]
        public string ContactNo1 { get; set; }

        [StringLength(15)]
        public string ContactNo2 { get; set; }

        public string PABXExtension { get; set; }

        public byte[] EmployeeImage { get; set; }

        public byte[] EmpSignature { get; set; }

        public string EmployeeImageLink { get; set; }

        [StringLength(50)]
        public string BatchNo { get; set; }

        public string ComputerEfficiency { get; set; }

        public string JobExperience { get; set; }

        public bool? IsInvestigation { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public bool IsOvertimeException { get; set; }
        public string ETinNo { get; set; }

        public virtual Office Office { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual EmployeeDepartment EmployeeDepartment { get; set; }

        public virtual EmployeeDesignation EmployeeDesignation { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }

        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        public virtual ICollection<EmployeeFamilyInfo> EmployeeFamilyInfoes { get; set; }

        public virtual ICollection<EmployeeTransfer> EmployeePostingHistories { get; set; }

        public virtual ICollection<EmployeeReference> EmployeeReferences { get; set; }

        public int? PFTypeId { get; set; }

        [NotMapped]
        public string Message { get; set; }
        public DateTime? PermanentDate { get; set; }

        [NotMapped]
        public string PayrollDesignation { get; set; }
    }
}
