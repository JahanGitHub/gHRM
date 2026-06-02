using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace gHRM.Web.ViewModels
{
    public class AttendanceAccssDBViewModel : BaseModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime time { get; set; }

        //[Display(Name = "Access Data Upload")]
        //public HttpPostedFileBase AccessFile_AttachmentU { get; set; }

        [Display(Name = "Upload")]
        public HttpPostedFileBase AccessFile_AttachmentU { get; set; }


        [Display(Name = "Upload")]
        public HttpPostedFileBase TxtFile_AttachmentU { get; set; }

        //Office Day Type.
        public int AttOfficeDayTypeId { get; set; }

        public int AttOfficeDayTypeIdForAccess { get; set; }
        public int AttOfficeDayTypeIdForCSV { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string AttendanceDateMsg { get; set; }
        public string EmployeeCode { get; set; }

        [Display(Name ="Terminal")]
        [Required(ErrorMessage ="{0} is Required")]
        public string AttendanceTerminal { get; set; }

        public IEnumerable<SelectListItem> AttendanceDevicesDropdown { get; set; }

        public string Company { get; set; }
    } 
}