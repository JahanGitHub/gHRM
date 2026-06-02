using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseAnnexationViewModel : BaseModel
    {
        public long AnnexationId { get; set; }

        public int CaseMasterId { get; set; }

        public int CrimeId { get; set; }

        [Display(Name = "Total Annexation Amount")]
        public decimal? TotAnnexationAmount { get; set; }
        public DateTime? ReturnNoticeDate { get; set; }
        public decimal? TotReturnAmount { get; set; }
    }
}