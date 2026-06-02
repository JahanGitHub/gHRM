using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeePromotionViewModelNew
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        public DateTime? FirstJoiningDate { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int? DesignationId { get; set; }
        public string OfficeName { get; set; }
        public int? TotalService { get; set; }
        public int? LastPromotionYear { get; set; }
        public long? PromotionId { get; set; }
        [Display(Name = "Promotion Type")]
        public string PromotionType { get; set; }
        public int? IsReviewed { get; set; }
        public string IsReviewedString { get; set; }
        public string PromotionDateMsg { get; set; }
        [Display(Name = "Promotion Date")]
        public DateTime? PromotionDate { get; set; }
        public string PromotionStatus { get; set; }
        public int? Score { get; set; }
    }
}
