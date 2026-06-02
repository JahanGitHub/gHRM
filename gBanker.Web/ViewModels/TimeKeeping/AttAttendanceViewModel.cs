using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class AttAttendanceViewModel : BaseModel
    {
        public long AttAttendanceId { get; set; }
        [Display(Name = "Card Issue ID")]
        public long AttCardIssueId { get; set; }

        [Display(Name = "Office Machine")]//////////////////
        public int OfficeMachineID { get; set; }///////////////

        [Display(Name = "Attendance Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AttenDate { get; set; }

        public string EmployeeCode { get; set; }

        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public string Clock { get; set; }
        public string CurrentDate { get; set; }
        public string LogInType { get; set; }
        public string InOutType { get; set; }
        [Display(Name = "Time")]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t} 
        public DateTime InOutTime { get; set; }
        [Display(Name = "In Time")]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t} 
        public DateTime LoginTime { get; set; }
        [Display(Name = "Out Time")]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t} 
        public DateTime LogoutTime { get; set; }

        [Display(Name = "Remarks")]        
        public string Remarks { get; set; }

        [Display(Name = "Log in Time")]
        public string strLoginTime { get; set; }
        [Display(Name = "Log out Time")]
        public string strLogoutTime { get; set; }
        public int? AttOfficeMachineId { get; set; }
        public int AttAttendanceTypeId { get; set; }

        //Office Day Type.
        public int AttOfficeDayTypeId { get; set; }
        public string OfficeDayTypeShortName { get; set; }
        public string OfficeDayTypeFullName { get; set; }
        public int ReportType { get; set; }
        public int DepartmentId { get; set; }
        public int SectionId { get; set; }
        public int Month { get; set; }

      
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> SectionList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public string ValidOfficeEmployee { get; set; }
        public int OfficeId { get; set; }

        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        public string Justification { get; set; }

        [Display(Name = "Office Type")]
        public int? OfficeTypeId { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }

        public string CompanyCode { get; set; }

    }// End of class
}// End of namespace