using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeDepartmentViewModel : BaseModel
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Office Type(অফিসের ধরণ)")]
        public int OfficeTypeId { get; set; }

        //[Required(ErrorMessage = "Deparment Code is required")]
        [Display(Name = "Department Code(বিভাগের কোড)")]
        public string DepartmentCode { get; set; }

        //[Required(ErrorMessage = "Deparment Name is required")]
        [Display(Name = "Department Name(বিভাগের নাম)")]
        public string DepartmentName { get; set; }

        [Display(Name = "Department Short Name( বিভাগের সংক্ষিপ্ত নাম )")]
        public string DepartmentShortName { get; set; }
        public int? CompanyId { get; set; }
        public List<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public List<string> OfficeTypeIdList { get; set; }


    }
}