using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class AccountChartViewModel 
    {
        
        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Account Type")]
        public string AccountTypeName { get; set; }
        
        
        [Required]
        [MaxLength(3)]
        [Display(Name = "Account Type Code")]
        public string AccountTypeCode { get; set; }

        [Required]
        [Display(Name = "GL Level")]
        public int GLLevelId { get; set; }

        [Required]
        [Display(Name = "Voucher?")]
        public bool IsVoucher { get; set; }
        
        [MaxLength(25)]
        [Display(Name = "Parent Account")]
        public string ParentAccountCode { get; set; }
        //public IEnumerable<SelectListItem> AccountTypeList { get; set; }
        //public IEnumerable<SelectList> AccountTypeList { get; set; }
        public SelectList AccountTypeList { get; set; } 
        public IEnumerable<SelectListItem> GLLevelList { get; set; }
        public SelectList ParentAccountList { get; set; } 
        //public IEnumerable<SelectListItem> ParentAccountList { get; set; }


        //Additional for Temp
        public string acCode { get; set; }
        public string glLevel { get; set; }
        public string pAcCode { get; set; }

        
        
    }
}