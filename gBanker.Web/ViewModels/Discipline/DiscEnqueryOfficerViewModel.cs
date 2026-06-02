using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscEnqueryOfficerViewModel : BaseModel
    {
        public int EnqueryOfficerId { get; set; }

        public long EmployeeId { get; set; }

        public int OfficeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRank { get; set; }

        public string DesignationName { get; set; }

        public long Sl { get; set; }
    }
}