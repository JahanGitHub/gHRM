using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveAdjustmentAuthorityViewModel
    {
        public string SlNo { get; set; }

        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsActive { get; set; }
        public int? OfficeTypeId { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        public int OfficeDesignationId { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> OfficeDesignationNameList { get; set; }
        public IEnumerable<SelectListItem> EmployeeCodeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeDepartmentList { get; set; }
    }
}