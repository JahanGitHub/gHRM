using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveApproversViewModel
    {
        public int ID { get; set; }
        public long EmployeeId { get; set; }
        public int ApprovalLevel { get; set; }
        public long ApproverEmpId { get; set; }
        public bool ManualUpdated { get; set; }
        public string ApplicantDetail { get; set; }
        public string ApproverDetail { get; set; }
        public int TotalApprovalLevel { get; set; }

        public string OfficeTypeName { get; set; }
        public string OfficeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int ApproverOfficeTypeId { get; set; }
        public int ApproverOfficeId { get; set; }
        public int ApproverDepartmentId { get; set; }
        public int ApproverDesignationId { get; set; }
        public int ApproverSectionId { get; set; }

        public List<SelectListItem> ApproverOfficeTypeList { get; set; }
        public List<SelectListItem> ApproverOfficeList { get; set; }
        public List<SelectListItem> ApproverDepartmentList { get; set; }
        public List<SelectListItem> ApproverDesignationList { get; set; }
        public List<SelectListItem> ApproverEmployeeList { get; set; }
        public List<SelectListItem> ApprovalLevelList { get; set; }
        
       

    }
}