using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class TransactionCategoryViewModel
    {
        [Display(Name=("Category ID"))]
        public string TransCategoryId { get; set; }

        [Display(Name = ("Transaction Name"))]
        public string TransCategoryName { get; set; }

        [Display(Name = ("Transaction Group"))]
        public string TransGroupName { get; set; }

        //Primary Transaction
        [Display(Name = ("Account"))]
        public string AccountId { get; set; }

        [Display(Name = ("Account Code"))]
        public string AccountCode { get; set; }
        
        [Display(Name = ("Transaction Type"))]
        public string TransactionType { get; set; }

        [Display(Name = ("Particulars"))]
        public string Particulars { get; set; }

        //Reverse Transaction
        [Display(Name = ("Reverse Account"))]
        public string ReverseAccountId { get; set; }

        [Display(Name = ("Reverse Transaction Type"))]
        public string ReverseTransactionType { get; set; }

        [Display(Name = ("Reverse Particulars"))]
        public string ReverseParticulars { get; set; }


        //additional
        public IEnumerable<SelectListItem> TransactionGroupList { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public IEnumerable<SelectListItem> AccountChartList { get; set; }
        public IEnumerable<SelectListItem> ReverseAccountChartList { get; set; }  
    }
}