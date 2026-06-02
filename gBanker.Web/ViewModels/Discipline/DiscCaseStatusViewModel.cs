using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseStatusViewModel : BaseModel
    {
        public long CaseStatusId { get; set; }

        public int CaseMasterId { get; set; }

        public int StatusId { get; set; }

        public DateTime? StatusDt { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }

    }// End Class
}// ENd Namespace