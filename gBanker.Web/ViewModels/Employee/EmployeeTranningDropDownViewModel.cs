using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Employee
{
    public class EmployeeTranningDropDownViewModel
    {
        public int EmployeeTrainingDropDownId { get; set; }
        [Display(Name = "Employee Training Title")]
        public string EmployeeTrainingDropDownName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
    }
}