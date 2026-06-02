using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseDetailViewModel : BaseModel
    {
        public int CaseDetailsId { get; set; }

        public int CaseMasterId { get; set; }

        public long? AnnexationId { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = "Crime")]
        public int CrimeId { get; set; }

        public decimal? AnnexationAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
        public DateTime? ReturnNoticeDate { get; set; }
        public int? PunishmentId { get; set; }
        public DateTime? PunishmentDt { get; set; }
        public string DispatchNo { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<SelectListItem> CrimeList { get; set; }

    }
}