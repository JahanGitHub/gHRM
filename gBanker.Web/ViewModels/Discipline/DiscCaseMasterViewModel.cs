using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseMasterViewModel : BaseModel
    {
        public int CaseMasterId { get; set; }
        [Display(Name = "Case No.")]
        public string CaseNo { get; set; }

        [Display(Name = "Case Date")]
        public DateTime CaseDateFrom { get; set; }
        public DateTime? CaseDateTo { get; set; }
        public DateTime? AuditFrom { get; set; }
        public DateTime? AuditTo { get; set; }

        [Display(Name = "Case Type")]
        public string CaseType { get; set; }

        [Display(Name = "Case Description")]
        public string CaseDescription { get; set; }

        public string Sl { get; set; }
        public string ZoneName { get; set; }
        public string CaseDateMsg { get; set; }
        public string OfficeName { get; set; }
        public string Crimes { get; set; }
        public string Employees { get; set; }
        public decimal? TotAnnexationAmount { get; set; }


        public int? DealOfficerId { get; set; }

        public int? EnqueryOfficerId { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<SelectListItem> DealOfficerList { get; set; }
        public IEnumerable<SelectListItem> EnqueryOfficerList { get; set; }


    }
}