using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Leave
{
    public class OutOfOfficeViewModel : BaseModel
    {

        public int OutofOfficeId { get; set; }

        [Display(Name = "Employee ID")]
        public long EmployeeId { get; set; }

        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Absent Type")]
        public string Category { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        [Display(Name = "Office Type")]
        public string CurrentOfficeType { get; set; }

        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }

        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }

        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }


        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string rowSl { get; set; }





        // List 

        public IEnumerable<SelectListItem> LeaveCategoryList { get; set; }

    }
}