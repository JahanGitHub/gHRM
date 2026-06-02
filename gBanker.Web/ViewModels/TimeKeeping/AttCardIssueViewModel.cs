using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class AttCardIssueViewModel : BaseModel
    {

        public long AttCardIssueId { get; set; }

        [Display(Name = "Employee Id")]
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Card No")]
        public string CardNo { get; set; }

        [Display(Name = "Card Issue Date")]
        public string CardIssueDateView { get; set; }
        [Display(Name = "Card Issue Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CardIssueDate { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public bool? IsActive { get; set; }
         
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }
         
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }
         
        public DateTime? UpdateDate { get; set; }


    }// End of Class
}// End of Namespace