using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class ThanaViewModel:BaseModel
    {
       
        public int thana_id { get; set; }


        [Display(Name = "Country Name (দেশের নাম)")]
        [Required(ErrorMessage = "Country Name Must Be Selected")]
        public int CountryId { get; set; }


        [Display(Name = "State/Province Name (বিভাগের নাম)")]
        [Required(ErrorMessage = "State/Province Name Must Be Selected")]
        public int StateOrProvinceId { get; set; }


        [Display(Name="District Name (জেলার নাম)")]
        [Required(ErrorMessage="District Name Must Be Selected")]
        public int district_id { get; set; }

        [Display(Name = "Thana Code (থানা কোড)")]
        public string thana_code { get; set; }

        [Display(Name = "Thana Name (থানার নাম )")]
        [Required(ErrorMessage = "Thana Name is Required")]
        public string thana_name_eng { get; set; }



        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateOrProvinceList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }

    }
}