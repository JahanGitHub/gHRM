using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanApprovalViewModel : BaseModel
    {
        public int ApprovalDetailId { get; set; }
        public int ApprovalMasterId { get; set; }
        [Display(Name = "Form Name")]
        public string FormName { get; set; } = "";
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; } = "";
        [Display(Name = "Total Level")]
        public int? TotalLevel { get; set; }
        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        [Display(Name = "Priority Level")]
        public int PriorityLevel { get; set; }
        [Display(Name = "Amount")]
        public int? ConditionalAmount { get; set; }
        [Display(Name = "Type")]
        public string ConditionType { get; set; }
        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> FormNameLst { get; set; }
    }
}