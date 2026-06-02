using gHRM.Data.CodeFirstMigration;
using gHRM.Web.Filters;
using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gHRM.Web.ViewModels
{
    public class CompanyViewModel : BaseModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name Required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Image")]
        public HttpPostedFileBase ImgFile { get; set; }

        [Display(Name = "Company ImageCompany Signature")]
        public HttpPostedFileBase CopanySignatureFile { get; set; }

        [Display(Name = "Company Signature")]
        public string CompanySignaturePath { get; set; }

        [Display(Name = "Choose a Image")]
        public byte[] CompanyImage { get; set; }

        [Required(ErrorMessage = "Company Address Required")]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [Display(Name = "Company Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Company Mobile")]
        //[StringLength(11, ErrorMessage = "Mobile Phone should be 11 characters")]
        public string CompanyMobile { get; set; }

        [Display(Name = "Company Phone")]
        [StringLength(11, ErrorMessage = "Mobile Phone should be 11 characters")]
        public string CompanyPhone { get; set; }

        [Display(Name = "Company Type")]
        public string CompanyType { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Logo")]
        public string ImagePath { get; set; }

        public string ImagePreviewPath { get; set; }
        public string CompanySignaturePreviewPath { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }

        [Display(Name = "Company Slogan")]
        public string CompanySlogan { get; set; }

        [Display(Name = "Website URL")]
        public string WebsiteURL { get; set; }
    }
}