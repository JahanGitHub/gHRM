using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class ComponentPayrollViewModel
    {
        public int Id { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCategory { get; set; }
        public bool? IsChangeable { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public IEnumerable<SelectListItem> DesignationNameList { get; set; }
        public IEnumerable<SelectListItem> DepartmentNameList { get; set; }
        public IEnumerable<SelectListItem> EmployeeNameList { get; set; }
        public IEnumerable<SelectListItem> ComponentCategoryList { get; set; }
        public IEnumerable<SelectListItem> ComponentNameList { get; set; }
        public IEnumerable<SelectListItem> IsChangeableList { get; set; }
        public IEnumerable<SelectListItem> IsApprovedList { get; set; }
        public IEnumerable<SelectListItem> IsEmpCodeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public int IsApproved { get; set; }
        public int IsEmpCode { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int OfficeTypeId { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
    }
}