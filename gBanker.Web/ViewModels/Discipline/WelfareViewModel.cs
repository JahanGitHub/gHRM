using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class WelfareViewModel : BaseModel
    {
        public long WelfareId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Welfare Type")]
        public string WelfareType { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Display(Name = "Age")]
        public int? PatientAge { get; set; }
        [Display(Name = "Relationship")]
        public string PatientRelationship { get; set; }

        [Display(Name = "Disease Description")]
        public string DiseaseDescription { get; set; }
        [Display(Name = "Treatment Voucher")]
        public string TreatmentVoucher { get; set; }
        [Display(Name = "Voucher Attachment")]
        public byte[] VoucherAttachment { get; set; }
        [Display(Name = "Applied Loan Amount")]
        public decimal? AppliedLoanAmount { get; set; }
        [Display(Name = "Personally Beared")]
        public decimal? PersonallyBeared { get; set; }
        [Display(Name = "Other Source")]
        public decimal? OtherSource { get; set; }
        [Display(Name = "Grant Amount")]
        public decimal? GrantAmount { get; set; }
        [Display(Name = "Loan Amount")]
        public decimal? LoanAmount { get; set; }
        [Display(Name = "Remarks")]
        public string WelfareRemarks { get; set; }
        [Display(Name = "Witness-1 Name")]
        public string WitnessOneName { get; set; }
        [Display(Name = "Address")]
        public string WitnessOneAddress { get; set; }
        [Display(Name = "Witness-2 Name")]
        public string WitnessTwoName { get; set; }
        [Display(Name = "Address")]
        public string WitnessTwoAddress { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Designation")]
        public string DesignationName { get; set; }
        [Display(Name = "Office")]
        public string OfficeName { get; set; }
        [Display(Name = "Joining Date")]
        public string JoiningDateMsg { get; set; }
        [Display(Name = "Duration")]
        public string JobDuration { get; set; }
        [Display(Name = "Salary Scale")]
        public string CurrentSalaryScale { get; set; }
        [Display(Name = "Attachment")]
        public HttpPostedFileBase File_VoucherAttachment { get; set; }

        public decimal? Total { get; set; }
        public string CreateDateMsg { get; set; }
    }
}