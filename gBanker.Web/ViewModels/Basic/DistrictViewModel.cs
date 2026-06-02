using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels
{
    public class DistrictViewModel : BaseModel
    {
        public long EmployeeAddressId { get; set; }
        [Display(Name="District ID (জেলার কোড)")]
       // [Display(Name = "DistrictID", ResourceType = typeof(Resource))]
        public int district_id { get; set; }

        //[Required(ErrorMessage = "Batch No is required")]

       // [Display(Name = "Country Name (দেশের নাম)")]
        [Display(Name = "CountrtyId", ResourceType = typeof(Resource))]
        public int CountrtyId { get; set; }

       // [Required(ErrorMessage="Division Name is required")]
        [Display(Name = "State/Province/Division Name (বিভাগের নাম)")]
       // [Display(Name = "State/Province/Division Name", ResourceType = typeof(Resource))]
        public int division_Id { get; set; }
       
        [Display(Name = "District Code (জেলা কোড)")]
       // [Display(Name = "DistrictCode", ResourceType = typeof(Resource))]
        public string district_code { get; set; }

        [Required(ErrorMessage = "District Name is required")]
       // [Display(Name = "DistrictName", ResourceType = typeof(Resource))]
        [Display(Name = "District Name (জেলার নাম)")]
        public string district_name_eng { get; set; }
        public string division_name { get; set; }

        public int rowSl { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }        
        public IEnumerable<SelectListItem> StateOrProvinceList { get; set; }

    }
}