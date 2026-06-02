using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveSellAdviseViewModel : BaseModel
    {
        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Zone")]
        public string Zone { get; set; }

        [Display(Name = "DMC")]
        public string DMC { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Display(Name = "Designation")]
        public string DesignationName { get; set; }

        [Display(Name = "Leave Sell No")]
        public string LeaveSellNo { get; set; }

        [Display(Name = "Encashed Amount")]
        public string EncashedAmount { get; set; }

        [Display(Name = "Total Days")]
        public string TotalDays { get; set; }

        [Display(Name = "Sale Date")]
        public string SaleDate { get; set; }

        [Display(Name = "Request Date")]
        public string RequestDate { get; set; }

        [Display(Name = "Approved Date")]
        public string ApprovedDate { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public bool IsAuthorized { get; set; }
    }
}