using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeOfficeTimeExceptionViewModel
    {
        public int Id { get; set; }
        public int OfficeTypeId { get; set; }
        public int OfficeId { get; set; }

        [Display(Name ="Login Time")]
        public DateTime? LogInTime { get; set; }

        [Display(Name = "Last Login Time")]
        public DateTime? LastLogInTime { get; set; }

        [Display(Name = "Logout Time")]
        public DateTime? LogOutTime { get; set; }

        [Display(Name = "Effective Date From")]
        public DateTime? EffectiveDateFrom { get; set; }

        [Display(Name = "Effective Date To")]
        public DateTime? EffectiveDateTo { get; set; }

        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }

        [Display(Name = "Time Exception Reason")]
        public string TimeExceptionReason { get; set; }

        public IEnumerable<SelectListItem> HOList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }    
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> TimeExceptionReasonList { get; set; }
        public int TimeKeepingRosterId { get; set; }
        public IEnumerable<SelectListItem> RosterNameList { get; set; }
    }
}