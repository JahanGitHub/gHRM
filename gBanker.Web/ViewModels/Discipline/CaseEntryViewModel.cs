using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class CaseEntryViewModel : EmployeeViewModel
    {


        public CaseEntryViewModel()
        {
            AnnexationAmount = 0;
            IndiReturnAmountMsg = "0";
        }

        public string SlNo { get; set; }
        [Display(Name = "Designation")]
        public string DesignationName { get; set; }

        [Display(Name = "Present Office")]
        public string OfficeName { get; set; }
        public int CaseDealingOfficerId { get; set; }
        public long? DealOfficerId { get; set; } //kk
        public int EnqueryOfficerId { get; set; }

        public decimal? TotalAnnexationAmount { get; set; }
        public string TotalAnnexationAmountMsg { get; set; }
        public decimal? AnnexationAmount { get; set; }
        public string AnnexationAmountMsg { get; set; }
        public decimal? TotReturnAmount { get; set; }
        public string TotReturnAmountMsg { get; set; }
        public string IndiReturnAmountMsg { get; set; }
        public string CaseDesPatchNo { get; set; }

        public string PunishmentName { get; set; }
        public int CrimeLocationId { get; set; }

        public string EmployeeRank { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Case Issue Date")]
        public DateTime? currentDate { get; set; }
        public int CaseDetailsId { get; set; }

        [Display(Name = "Case Master Id")]
        public int CaseMasterId { get; set; }

        [Display(Name = "Days")]
        public int DaysLose { get; set; } // ... জ্যেষ্ঠতা কর্তনসহ বিনা বেতনে ছুটি মনজুর

        [DataType(DataType.MultilineText)]
        [Display(Name = "Case Description")]
        public string CaseDescription { get; set; }

        [Display(Name = "Charge Sheet")]
        public string ChargeSheet { get; set; }

        [Display(Name = "Employee Name")]
        public long EmployeeId { get; set; }
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        public string DesginationName { get; set; }
        public string ZoneName { get; set; }
        public string Crimes { get; set; }
        public string Employees { get; set; }
        public string DespatchNo { get; set; }
        public string PunishmentDespatchNo { get; set; }
        public DateTime? CrimeDateFrom { get; set; }
        public DateTime? EnquiryOfficerAssignedDt { get; set; }

        public DateTime? CrimeFindOutFrom { get; set; }
        public string CrimeLocationBng { get; set; }

        public DateTime? CrimeFindOutTo { get; set; }
        public DateTime? InvestigationDt { get; set; }
        public DateTime? ReportReceivedDt { get; set; }

        public int AnulipiId { get; set; }
        public string AnulipiText { get; set; }


        public string EnquiryRemarks { get; set; }
        public DateTime? CrimeDateTo { get; set; }
        public int? PunishmentId { get; set; }
        public DateTime? PunishmentDt { get; set; }
        public DateTime? ActivatedDt { get; set; }
        public DateTime? FirstIncSuspendDt { get; set; }
        public DateTime? SecondIncSuspendDt { get; set; }
        public DateTime? ThirdIncSuspendDt { get; set; }
        public DateTime? FourthIncSuspendDt { get; set; }

        [Display(Name = "Punished By")]
        public long? PunishedBy { get; set; }


        [Display(Name = "Punished Date")]
        public DateTime? PunishedDate { get; set; }

        public string PunishmentDateMsg { get; set; }

        public int? CrimeId { get; set; }
        public string CrimeName { get; set; }
        public string CrimeDateMsg { get; set; }
        //public int CaseMasterId { get; set; }

        [Display(Name = "Investigator")]
        public long? InvestigatorId { get; set; }

        [Display(Name = "Case No.")]
        public string CaseNo { get; set; }

        [Display(Name = "Case Date")]
        public DateTime CaseDate { get; set; }
        public string CaseDateMsg { get; set; }
        public string CaseDateFromMsg { get; set; }
        public string CaseDateToMsg { get; set; }


        public string FirstIncSuspendDtmsg { get; set; }
        public string SecondIncSuspendDtmsg { get; set; }
        public string ThirdIncSuspendDtmsg { get; set; }
        public string FourthIncSuspendDtmsg { get; set; }


        public string ActivatedFromMsg { get; set; }

        [Display(Name = "Office Type")]
        public int? OfficeTypeId { get; set; }
        [Display(Name = "Office Name")]
        public int? OfficeId { get; set; }
        [Display(Name = "Case Type")]
        public string CaseType { get; set; }

        [Display(Name = "Crime Date")]
        public DateTime? CrimeDate { get; set; }
        public string CrimeLocationName { get; set; }
        public string OfficeCode { get; set; }
        public int? CrimeLocation { get; set; }
        public int? OfficeLevel { get; set; }
        public string DealerName { get; set; }
        public string EnquiryName { get; set; }

        [Display(Name = "Crime Description")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string CrimeDescription { get; set; }

        public string OffcDesignName { get; set; }

        // public bool IsActive { get; set; }

        // public DateTime? InActiveDate { get; set; }

        public long? CaseStatusId { get; set; }
        public string CaseStatusIdMsg { get; set; }
        public string StatusMsg { get; set; }
        public DateTime? ReturnNoticeDate { get; set; }
        public DateTime? IndiReturnNoticeDate { get; set; }
        public string StatusDtMsg { get; set; }

        //public int? PunishmentId { get; set; }
        // public string EmployeeCode { get; set; }
        public string TotBalanceMsg { get; set; }
        public string DispatchNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? AuditFrom { get; set; }
        public DateTime? AuditTo { get; set; }
        public string AuditFromMsg { get; set; }
        public string AuditToMsg { get; set; }

        public int? FirstLevelId { get; set; }
        public int? SecondLevelId { get; set; }
        public int? ThirdLevelId { get; set; }
        public int? FourthLevelId { get; set; }
        public string CrimeDateFromMsg { get; set; }
        public string CrimeDateToMsg { get; set; }
        public string IndReturnNoticeDateMsg { get; set; }
        public string ReturnNoticeDateMsg { get; set; }
        public decimal? TotAnnexationAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
        public long? AnnexationId { get; set; }
        public int? CaseEnquiryOfficerId { get; set; }
        public string EnquiryOfficerAssignedDtMsg { get; set; }
        public string CrimeFindOutFromMsg { get; set; }
        public string CrimeFindOutToMsg { get; set; }
        public string InvestigationDtMsg { get; set; }
        public string ReportReceivedDtMsg { get; set; }
        public string CaseEmployeeName { get; set; }
        public string PunishmentEmployeeName { get; set; }
        public string Mode { get; set; }

        public string EnqueryOfficerName { get; set; }
        public IEnumerable<SelectListItem> CrimeList { get; set; }
        public IEnumerable<SelectListItem> CaseTypeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> InvestigatorList { get; set; }

        public IEnumerable<SelectListItem> DealOfficerList { get; set; }
        public IEnumerable<SelectListItem> EnqueryOfficerList { get; set; }

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

    }// END Class
}// END NAmespace