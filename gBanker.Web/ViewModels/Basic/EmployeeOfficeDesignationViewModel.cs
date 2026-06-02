using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeOfficeDesignationViewModel:BaseModel
    {
        public long EmpOfficeDesigId { get; set; }

        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }

        [Display(Name = "Office Designation")]
        public int OfficeDesignationId { get; set; }
        public string OfficeDesignationIdMsg { get; set; }
         [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
         [Display(Name = "Date")]
        public DateTime? SartDate { get; set; }

        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeDesignationName { get; set; }
        public string DesignationName { get; set; }
        public DateTime? EndDate { get; set; }

        public int? Duration { get; set; }
        public IEnumerable<SelectListItem> EmployeeNameList { get; set; }
        public IEnumerable<SelectListItem> OfficeDesignationNameList { get; set; }
    }
}