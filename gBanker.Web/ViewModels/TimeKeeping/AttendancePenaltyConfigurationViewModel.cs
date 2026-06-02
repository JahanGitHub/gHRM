using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class AttendancePenaltyConfigurationViewModel
    {
        public int Id { get; set; }
        public string rowSl { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string DepartmentName { get; set; }
        public string OfficeDesignation { get; set; }
        public int EmployeeStatusId { get; set; }

        public string EmployeeStatus { get; set; }
        public string EmployeeStatusFull { get; set; }
        public string Gender { get; set; }
        public List<int> SelectedStatusId { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypeName { get; set; }
        public int LeaveOrder { get; set; }

        public int AttendanceTypeId { get; set; }

        public string AttendanceDate { get; set; }

        public int CountLateAttendance { get; set; }
        public int TotalLateDays { get; set; }
        public int LeaveDeduction { get; set; }

        public List<SelectListItem> TotalLateDaysCount { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }
        public List<SelectListItem> TotalDeductionDays { get; set; }
        public List<SelectListItem> TotalOrderList { get; set; }

        public string Year { get; set; }
        public string Month { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MonthList { get; set; }


        [Display(Name = "Office Type")]
        public int OfficeTypeId { get; set; }

        [Display(Name = "Zone Name")]
        public string ZoneId { get; set; }

        [Display(Name = "Area Name")]
        public string AreaId { get; set; }

        [Display(Name = "Unit Name")]
        public string UnitId { get; set; }

        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public int OfficeId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
    }
}