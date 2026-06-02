using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseEnquiryOfficerViewModel : BaseModel
    {
        public int CaseEnquiryOfficerId { get; set; }

        public int CaseMasterId { get; set; }

        public long EmployeeId { get; set; }
        public string DespatchNo { get; set; }
        public long EnqueryOfficerId { get; set; }
        public DateTime? EnquiryOfficerAssignedDt { get; set; }

        public DateTime? InvestigationDt { get; set; }

        public DateTime? ReportReceivedDt { get; set; }

        public string EnquiryRemarks { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string OfficeName { get; set; }
        public string CaseNo { get; set; }

        public DateTime? CrimeFindOutFrom { get; set; }

        public DateTime? CrimeFindOutTo { get; set; }
        public IEnumerable<SelectListItem> EnqueryOfficerList { get; set; }

    }
}