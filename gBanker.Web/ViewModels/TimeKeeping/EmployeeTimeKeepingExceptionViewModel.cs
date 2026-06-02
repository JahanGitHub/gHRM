using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeTimeKeepingExceptionViewModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public long DepartmentId { get;set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public long OfficeDesignationId { get; set; }
        public int AttendenceTypeId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }

        [Display(Name = "Last Login Time")]
        [Required]
        public DateTime LastLoginTime { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
      
        [Display(Name = "Reason")]
        public string AttenTypeFullName { get; set; }
        public string EmployeeName { get; set; }
        [Display(Name = "Responsibility")]
        public string OffcDesignName { get; set; }
        public string DepartmentName { get; set; }
        public string Justification { get; set; }
        public IEnumerable<SelectListItem> AttendenceTypeNameList { get; set; }
        public IEnumerable<SelectListItem> EmployeeNameList { get; set; }
        public IEnumerable<SelectListItem> OffcDesignNameList { get; set; }
        public IEnumerable<SelectListItem> EmployeeDepartmentNameList { get; set; }




    }
}