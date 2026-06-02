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
    public class StateOrProvinceViewModel:BaseModel
    {
        public int StateOrProvinceId { get; set; }

        //[Required(ErrorMessage = "Country Name is required")]
        [Display(Name = "Country Name (দেশের নাম)")]
        public int CountryId { get; set; }

        [Display(Name = "State Or province Name (বিভাগের নাম)")]
        public string Name { get; set; }
              //[Required(ErrorMessage = "State Or province Code is required")]
        [Display(Name = "State/Province Short Code (বিভাগ কোড)")]
        public string Code { get; set; }

        //[Required(ErrorMessage = "State Or province Name is required")]       
       
         public IEnumerable<SelectListItem> CountryList { get; set; }
         public string CountryName { get; set; }
         public int rowSl { get; set; }

    }
}