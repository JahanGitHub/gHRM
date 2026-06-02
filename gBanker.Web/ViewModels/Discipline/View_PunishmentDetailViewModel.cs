using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class View_PunishmentDetailViewModel
    {
        public int? RowSl { get; set; }
        public int PunishmentMasterId { get; set; }
        public long EmployeeId { get; set; }
        public int PunishmentId { get; set; }

        [Display(Name = "Punishment Date")]
        public DateTime? PunishmentDate { get; set; }

        [Display(Name = "Punishment Dispatch Number")]
        public string PunishmentDispatchNumber { get; set; }
        public int DaysLose { get; set; }
        public int CaseMasterId { get; set; }

        [Display(Name = "Case No")]
        public string CaseNo { get; set; }

        [Display(Name = "Case Date From")]
        public string CaseDateFrom { get; set; }

        [Display(Name = "Case Date To")]
        public DateTime? CaseDateTo { get; set; }
        public DateTime? AuditFrom { get; set; }
        public DateTime? AuditTo { get; set; }
        public string CaseType { get; set; }
        public string CaseTypeName { get; set; }
        public string CaseDescription { get; set; }
        public string CaseMasterRemarks { get; set; }
        public string CaseDispatchNumber { get; set; }
        public DateTime? CrimeDateFrom { get; set; }
        public DateTime? CrimeDateTo { get; set; }
        public decimal? AnnexationAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CrimeCode { get; set; }
        public string CrimeName { get; set; }
        public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        [Display(Name = "Payroll Designation")]
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        [Display(Name = "Salary Type")]
        public IEnumerable<SelectListItem> EmployeeSalaryType { get; set; }

        public IEnumerable<SelectListItem> SalaryGenerationTypeList { get; set; }

        public IEnumerable<SelectListItem> GradeList { get; set; }

        [Display(Name = "Step")]
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }

        public IEnumerable<SelectListItem> OverTimeList { get; set; }

        [Display(Name = "Provident Fund Type")]
        public IEnumerable<SelectListItem> PFTypeList { get; set; }

        [Display(Name = "Promotion Type")]
        public IEnumerable<SelectListItem> PromotionTypeList { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }

        public IEnumerable<SelectListItem> IncrementYearFromList { get; set; }

        public IEnumerable<SelectListItem> BankList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
    }
}