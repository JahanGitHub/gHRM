using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundConfiguration
{
    public class HealthFundingViewModel
    {   

        public int Id { get; set; }

        public long EmployeeId { get; set; }
        [Display(Name = "Employee Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EmployeeName { get; set; }


        [Display(Name = "Fund Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal FundAmount { get; set; }


        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Purpose")]
        [Required(ErrorMessage = "{0} is Required")]
        public int PurposeId { get; set; }

        [Display(Name = "Remarks")]
        [Required(ErrorMessage = "{0} is Required")]
        public string remarks { get; set; }

        public string EmpInfo { get; set; }

        public string purposename { get; set; }

        public string CreateDateString { get; set; }

        public bool IsActive { get; set; } 

        public IEnumerable<SelectListItem> PurposeList { get; set; }
     
    }

    public class purposeListFund
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}