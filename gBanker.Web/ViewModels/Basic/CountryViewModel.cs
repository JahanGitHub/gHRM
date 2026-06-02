using gHRM.Data.CodeFirstMigration;
using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class CountryViewModel : BaseModel
    {
        public int CountryId { get; set; }


        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }


        [Required(ErrorMessage = "Country Short Code is required")]
        [Display(Name = "Country Short Code")]
        public string CountryShortCode { get; set; }

        [Required(ErrorMessage = "ISO Code is required")]
        [Display(Name = "ISO Code")]
        public string isoCode3 { get; set; }

        public bool Status { get; set; }

    }
}