using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCrimeViewModel : BaseModel
    {
        public int CrimeId { get; set; }
        public int? CrimeType { get; set; }

        [Display(Name = "Crime Code (অপরাধ কোড)")]
        public string CrimeCode { get; set; }

        [Display(Name = "Crime Name (অপরাধের নাম )")]
        public string CrimeName { get; set; }
        public int? SortOrder { get; set; }
        [Display(Name = "Remarks (মন্তব্য)")]
        public string Remarks { get; set; }

        //public bool IsActive { get; set; }


        //public DateTime? InActiveDate { get; set; }

        //public long? CreateUser { get; set; }


        //public DateTime? CreateDate { get; set; }

        //public long? UpdateUser { get; set; }

        //public DateTime? UpdateDate { get; set; }

        public IEnumerable<SelectListItem> CrimeList { get; set; }
        public IEnumerable<SelectListItem> CaseType { get; set; }
        public IEnumerable<SelectListItem> CrimeTypeList { get; set; }
    }
}