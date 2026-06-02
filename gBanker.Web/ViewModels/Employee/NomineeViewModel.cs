using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace gHRM.Web.ViewModels
{
    public class NomineeViewModel : BaseModel
    {
        public string SNO { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Designation")]
        public string DesignationName { get; set; }

        [Display(Name = "Office")]
        public string OfficeName { get; set; }

        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Confirmation Date")]
        public string ConfirmationDate { get; set; }

        [Display(Name = "Confirmation Date")]
        public string ConfirmationDateMsg { get; set; }

        //[Display(Name = "Witness Name")]
        //public string WitnessName { get; set; }

        //[Display(Name = "Designation")]
        //public string WitnessDesignation { get; set; }

        //[Display(Name = "Address")]
        //public string WitnessAddress { get; set; }

        //[Display(Name = "Date")]
        //public DateTime? WitnessDate { get; set; }

        //[Display(Name = "Attachment")]
        //public byte[] WitnessAttachment { get; set; }


        //public int isOverPercentage { get; set; }
        //public int NomineeMasterId { get; set; }

        public long NomineeId { get; set; }

        //public long NomineeDetailId { get; set; }

        [Display(Name = "Nominee Type")]
        public int NomineeTypeId { get; set; }

        [Display(Name = "Relation")]

        public int NomineeRelationId { get; set; }

        public string NomineeRelation { get; set; }

        public string NomineeType { get; set; }

        [Display(Name = "Name")]
        public string NomineeName { get; set; }

        [Display(Name = "Address")]
        public string NomineeAddress { get; set; }

        [Display(Name = "Age")]
        public int? NomineeAge { get; set; }

    

        [Display(Name = "Percentage")]
        public decimal? NomineePercentage { get; set; }

        [Display(Name = "National Id")]
        public string NomineeNationalId { get; set; }

        [Display(Name = "Image")]
        public byte[] NomineeImage { get; set; }

        [Display(Name = "Remarks")]
        public string NomineeRemarks { get; set; }

        public string NomineeTypeValue { get; set; }

        public string NomineeImageMsg { get; set; }
      
        [Display(Name = "Contact No 1")]
        public string ContactNo1 { get; set; }

        [Display(Name = "Contact No 2")]
        public string ContactNo2 { get; set; }

        [Display(Name = "Date Of Birth")]
        public string DateOfBirthMsg { get; set; }
        public string BirthCertificateNo { get; set; }

        //[Display(Name = "Attachment")]
        //public HttpPostedFileBase ImgFile_WitnessAttachment { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase ImgFile_NomineeImage { get; set; }
        public IEnumerable<SelectListItem> NomineeTypeList { get; set; }
        public List<SelectListItem> RelationshipList { get; set; }


    }
}