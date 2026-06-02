using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class ApprovalConfigDetailViewModel : BaseModel
    {
        public int ConfigDetailsId { get; set; }
        public int ConfigMasterId { get; set; }
        public int ApprovalLevel { get; set; }
        public int? ApproveOfficeTypeId { get; set; }
        public int? ApproveOfficeId { get; set; }
        
        public int? ApproveDepartmentId { get; set; }
        public int? ApprovalEmployeeId { get; set; }
        public int ApproveDesignationId { get; set; }
        public string ModuleName { get; set; }
        public bool IsApproverInSelfOffice { get; set; }

        public string FromDay { get; set; }
        public string ToDay { get; set; }
    }


    #region ViewModel with Master and detail
    public class ApprovalConfigurationViewModel : BaseModel
    {
        public string rowSl { get; set; }
        public int ConfigMasterId { get; set; }
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Display(Name = "Applicant's Office Type")]
        public string ConfigOfficeType { get; set; }
        public int ConfigOfficeTypeId { get; set; }
        [Display(Name = "Applicant's Office")]
        public int? ConfigOfficeId { get; set; }
        [Display(Name = "Applicant's Department")]
        public int? ConfigDepartmentId { get; set; }
        public string ConfigerDesignationType { get; set; }
        [Display(Name = "Applicant's Designation")]
        public int ConfigDesignationId { get; set; }
        public int CompanyId { get; set; }
        public string LeaveType { get; set; }
        public string DesignationType { get; set; }
        public int ConfigDetailsId { get; set; }
        [Display(Name = "Approval Level")]
        public int ApprovalLevel { get; set; }

        public string ApprovalLevelInString { get; set; }
        public string ApproveOfficeType { get; set; }
        [Display(Name = "Approver Office Type")]
        public int ApprovalOfficeTypeId { get; set; }
        [Display(Name = "Approver Office")]
        public int? ApproveOfficeId { get; set; }
        [Display(Name = "Approver Department")]
        public int? ApproveDepartmentId { get; set; }
        public string ApprovalDesignationType { get; set; }
        [Display(Name = "Applicant's Responsibility")]
        public int ApplicantDesignationId { get; set; }
        [Display(Name = "Approver Responsibility")]
        public int ApproveDesignationId { get; set; }
        public int? ApprovalEmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public int employeeId { get; set; }
        // public int ApprovalLevel { get; set; }
        #region View Index
        public string ModuleNameFull { get; set; }
        public string OfficeTypeName { get; set; }
        public string OfficeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeName { get; set; }
        public int TotalLevel { get; set; }

        public string FromDay { get; set; }
        public string ToDay { get; set; }


        #endregion

        public IEnumerable<SelectListItem> ApprovalEmployeeList { get; set; }
        public IEnumerable<SelectListItem> ApprovalOfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> ApprovalLevelList { get; set; }
        public IEnumerable<SelectListItem> ConfigOfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> ApproveOfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> ConfigOfficeList { get; set; }
        public IEnumerable<SelectListItem> ConfigDepartmentList { get; set; }
        public IEnumerable<SelectListItem> ConfigDesignationList { get; set; }
        public IEnumerable<SelectListItem> ApproveOfficeList { get; set; }
        public IEnumerable<SelectListItem> ApproveDepartmentList { get; set; }
        public IEnumerable<SelectListItem> ApplicantDesignationList { get; set; }
        public IEnumerable<SelectListItem> ApproveDesignationList { get; set; }
        public IEnumerable<SelectListItem> ModuleNameList { get; set; }
        public IEnumerable<SelectListItem> ConfigDesignationTypeList { get; set; }
        public IEnumerable<SelectListItem> ApproveDesignationTypeList { get; set; }
        public IEnumerable<SelectListItem> LeaveTypeList { get; set; }

    }
    #endregion
}