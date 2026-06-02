using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseDealingOfficerViewModel : BaseModel
    {
        public int CaseDealingOfficerId { get; set; }

        public int CaseMasterId { get; set; }

        public long EmployeeId { get; set; }
    }
}