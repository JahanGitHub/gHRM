using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeDocumentViewModel : BaseModel
    {
        public int EmployeeDocumentId { get; set; }
        public int EmployeeId { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public string DocumentType { get; set; }
        

        public string DocumentUrl { get; set; }

        [Display(Name = "Remark")]
        public string DocumentRemarks { get; set; }

        [Display(Name = "Upload")]
        public byte[] EmpSignature { get; set; }

        [Display(Name = "File")]
        public HttpPostedFileBase ImgFile { get; set; }

        //additional
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
    }
}