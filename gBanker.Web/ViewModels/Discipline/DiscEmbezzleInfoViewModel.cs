using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscEmbezzleInfoViewModel
    {

        public int EmbezzleId { get; set; }

        public int CaseMasterId { get; set; }

        public DateTime? EmbezzleRcvDt { get; set; }
        [Display(Name = "Embezzle Received Date")]
        public string EmbezzleRcvDtMsg { get; set; }

        public int? OfficeId { get; set; }
        public long? rowSl { get; set; }
        public DateTime? AuditDateFrom { get; set; }

        [Display(Name = "Audit Date From")]
        public string AuditDateFromMsg { get; set; }

        public DateTime? AuditDateTo { get; set; }
        [Display(Name = "Audit Date To")]
        public string AuditDateToMsg { get; set; }

        [Display(Name = "No./Type of Audit")]
        public string BranchAuditNo { get; set; }
        [Display(Name = "No Of BM Accused")]
        public int? NoOfBMAccused { get; set; }
        [Display(Name = "No Of 2nd Signa. Accussed")]
        public int? NoOfSignatoryAccussed { get; set; }
        [Display(Name = "No Of CM Accussed")]
        public int? NoOfCMAccussed { get; set; }
        [Display(Name = "Total Embezzled Amount")]
        public decimal? TotEmbezzledAmount { get; set; }
        [Display(Name = "Total Return Amount")]
        public decimal? TotReturnAmount { get; set; }

        public string Remarks { get; set; }
        [Display(Name = "Total Accussed")]
        public string TotalAccussed { get; set; }
        [Display(Name = "Balance Amount")]
        public string BalanceAmount { get; set; }
        [Display(Name = "Case No")]
        public string ExplonatoryNo { get; set; }
        public string EmbMode { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public long EmployeeId { get; set; }
        // public string OfficeName { get; set; }
        public string EmployeeRank { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeCode { get; set; }
        public decimal? Balance { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Justification { get; set; }

        [Display(Name = "Office Type")]
        public int? OfficeTypeId { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
    }
}