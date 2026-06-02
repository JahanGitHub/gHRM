using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeAddressViewModel : BaseModel
    {
        public long AddressId { get; set; }

        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Address Type is required")]
        [Display(Name = "Address Type")]
        public string AddressType { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        [Display(Name = "Country Name")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Division Name is required")]
        [Display(Name = "State/Province Name")]
        public int StateOrProvinceId { get; set; }
        public string StateOrProvinceName { get; set; }

        [Display(Name = "District Name")]
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }

        [Display(Name = "Thana Name")]
        public int? ThanaId { get; set; }
        public string ThanaName { get; set; }

        [Display(Name = "Union Name")]
        public int? UnionId { get; set; }
        public string UnionName { get; set; }

        [Display(Name = "Street/House")]
        public string StreetOrHouse { get; set; }
        public string PostOffice { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string AddressDetail { get; set; }
        public IEnumerable<SelectListItem> AddressTypeList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }        
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> ThanaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }
        


    }
}