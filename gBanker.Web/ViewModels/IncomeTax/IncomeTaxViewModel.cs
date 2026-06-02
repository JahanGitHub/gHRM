using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.IncomeTax
{
    public class IncomeTaxViewModel
    {
        public int? Id { get; set; }
        public long EmployeeID { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public int OfficeID { get; set; }

        public string OfficeName { get; set; }

        [Display(Name = "National ID")]
        public string NationalID { get; set; }

        [Display(Name = "TIN Number")]
        public string TIN { get; set; }

        [Display(Name = "Return Reg. Serial No.")]
        public string ReturnRegisterSlNo { get; set; }

        [Display(Name = "Return Reg. Volume No.")]
        public string ReturnRegisterVolNo { get; set; }

        [Display(Name = " Retun Filing Date")]
        public string ReturnFillingDate { get; set; }    // still string to match DB
                      
        [Display(Name = "Fiscal Year")]
        [Required]
        public string FiscalYear { get; set; }

        [Display(Name = "Circle")]
        public string Circle { get; set; }

        [Display(Name = "Tax Area")]
        public string TaxArea { get; set; }

        [Display(Name = "Total Income")]
        public string TotalIncome { get; set; }

        [Display(Name = "Total Tax Paid")]
        public string TotalTaxPaid { get; set; }

        [Display(Name = "File Attachment")]
        public string FileLocation { get; set; }

        public HttpPostedFileBase FileLocationU { get; set; }

        public DateTime? CreateDate { get; set; }

        // Dropdown (if needed)
        public List<SelectListItem> FiscalYearList { get; set; }
    }
}
