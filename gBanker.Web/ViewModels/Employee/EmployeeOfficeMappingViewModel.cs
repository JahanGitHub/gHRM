using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeOfficeMappingViewModel : BaseModel
    {
        public int OfficeMappingID { get; set; }
        [Display(Name = "Employee Code (কর্মচারীর কোড)")]
        [Required]
        [MaxLength(10)]
        public string EmployeeCode { get; set; }
        public short EmployeeId { get; set; }

        [Display(Name = "Office Names (অফিসের নাম)")]
        public int OfficeID { get; set; }

        [Display(Name = "Head Office (প্রধান কার্যালয়)")]
        public string HeadOfficeCode { get; set; }
        [Display(Name = "Zone Office (জোন অফিস)")]
        public string ZoneCode { get; set; }
        [Display(Name = "Area Office (এরিয়া অফিস)")]
        public string AreaCode { get; set; }

        public IEnumerable<SelectListItem> SelectedOfficeList { get; set; }

    }
}