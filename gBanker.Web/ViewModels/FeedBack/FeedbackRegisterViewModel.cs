using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.FeedBack
{
    public class FeedbackRegisterViewModel : BaseModel
    {
        [Display(Name ="Unit/Branch")]
        public int? UnitId { get; set; }

        public long? FeedbackRegisterID { get; set; }

        public int OfficeId { get; set; }
      
        [Required]
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        [Display(Name = "Employee Id")]
        [Required]
        public long? EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Feedback Category")]
        public int FeedbackCategoryID { get; set; }
        [Display(Name = "Feedback Category Name")]
        public string FeedbackCategoryName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Feedback Description")]
        public string FeedbackDescription { get; set; }


        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Feedback Date")]
        public DateTime FeedbackDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Feedback Date")]
        public string FeedbackDateSTR { get; set; }
        
        [Display(Name = "Is Checked (Yes/No)")]
        public bool IsChecked { get; set; }

         

        [Display(Name = "Is Solved (Yes/No)")]
        public bool IsSolved { get; set; }

        [Display(Name = "Solved By")]
        [StringLength(50)]
        public string SolvedBy { get; set; }

        [Display(Name = "Attachment")]
        public string FileLocation { get; set; }


        [Display(Name = "Attachment")]
        public string FileLocationReply { get; set; }

        [Display(Name = "Solved Date")]
        public string SolvedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Solved Date")]
        public string SolvedDateSTR { get; set; }

        public string ChkStatus { get; set; }
        public string SolvedStatus { get; set; }
        public string FeedbackDateMsg { get; set; }
        public string SolvedDateMsg { get; set; }
        public string ProblemDetails { get; set; }
        public string CorrectionDetails { get; set; }
        public string EntryDate { get; set; }
        public int PayRollFeedBackRegId { get; set; }
        public long rowSl { get; set; }
        public string ItemCode { get; set; }
        public string DisburseDate { get; set; }
        public string ContactMobileNo { get; set; }
        public string OfficeCode { get; set; }
        public string SolvedUnsolved { get; set; }
        [Display(Name = "Attachment")]
        public HttpPostedFileBase File_AttachmentU { get; set; }

        [Display(Name = "Attachment")]
        public HttpPostedFileBase File_AttachmentUReply { get; set; }
        public IEnumerable<SelectListItem> FeedbackCategoryList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> SolvedByList { get; set; }
        public IEnumerable<SelectListItem> SolvedUnsolvedList { get; set; }

        public int? ActualLoandisbursementAmount { get; set; }
        public int? WebLoanCollectionAmount { get; set; }
        public int? ActualCollectionDesktop { get; set; }
        public decimal? WebInterestCharge { get; set; }
        public int? WebInterestColleciton { get; set; }
        public decimal? DesktopInterestCharge { get; set; }
        public int? DesktopInterestCollection { get; set; }

    }
}