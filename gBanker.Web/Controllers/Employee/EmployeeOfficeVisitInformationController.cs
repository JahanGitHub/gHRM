using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;

namespace gHRM.Web.Controllers
{
    public class EmployeeOfficeVisitInformationController : BaseController
    {
        private readonly IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly ILinkWithEmployeeService linkWithEmployeeService;
        private readonly IInternalOrganizationService internalOrganizationService;

        public EmployeeOfficeVisitInformationController(
            IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService, IEmployeeService employeeService, IEmployeeSPService employeeSpService, ILinkWithEmployeeService linkWithEmployeeService, IInternalOrganizationService internalOrganizationService)
        {
            this.employeeOfficeVisitInformationService = employeeOfficeVisitInformationService;
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.linkWithEmployeeService = linkWithEmployeeService;
            this.internalOrganizationService = internalOrganizationService;
        }
        public ActionResult OfficeVisitInfo()
        {
            var model = new EmployeeOtherInformationViewModel();
            MapDropDownListForOfficeVisit(model);
            return View(model);
        }

        private void MapDropDownListForOfficeVisit(EmployeeOtherInformationViewModel model)
        {
            var visitType = new List<SelectListItem>();
            visitType.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            visitType.Add(new SelectListItem() { Text = "Local", Value = "L" });
            visitType.Add(new SelectListItem() { Text = "International", Value = "IN" });
            model.VisitTypeList = visitType;

            var offProvided = new List<SelectListItem>();
            offProvided.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            offProvided.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            offProvided.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.OfficeProvidedList = offProvided;
        }


        [HttpPost]
        public JsonResult SaveOfficeVisitInfo(EmployeeOfficeVisitInformation obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var employeeId = employeeService.GetByEmpId(Convert.ToInt64(obj.EmployeeCode)).EmployeeId;
                var model = new EmployeeOfficeVisitInformation();
                model.EmployeeId = employeeId;
                model.EmployeeCode = obj.EmployeeCode;
                model.VisitType = obj.VisitType;
                model.Location = obj.Location;
                model.Reason = obj.Reason;
                model.CurrentOfficeProvided = obj.CurrentOfficeProvided;
                model.IsActive = true;
                model.IsApproved = false;
                model.IsRejected = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Create(model);
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Save failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeOfficeVisitInformation(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var list = employeeOfficeVisitInformationService.GetMany(p => p.IsActive == true && (p.IsApproved==false || p.IsRejected==false )).ToList();
            var view_List = list.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                EmpOfficeVisitId = p.EmpOfficeVisitId,
                EmployeeId = p.EmployeeId,
                EmployeeCode = p.EmployeeCode,
                VisitType = p.VisitType,
                Location = p.Location,
                Reason = p.Reason,
                CurrentOfficeProvidedVal = p.CurrentOfficeProvided,
                CurrentOfficeProvided = p.CurrentOfficeProvided == 1 ? "Yes" : "No"
            }).ToList();
            var currentPageRecords = view_List.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_List.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateOfficeVisitInfo(EmployeeOfficeVisitInformation obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeOfficeVisitInformationService.GetById(obj.EmpOfficeVisitId);
                model.VisitType = obj.VisitType;
                model.Location = obj.Location;
                model.Reason = obj.Reason;
                model.CurrentOfficeProvided = obj.CurrentOfficeProvided;
                model.IsActive = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOfficeVisitInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeInfoByCode(string empCode)
        {
            var param = new { EmployeeCode = empCode };
            var empInfo = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
            var viewEmpInfo =
                empInfo.Tables[0].AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    EmployeeId = p.Field<long>("EmployeeId"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    Department = p.Field<string>("DepartmentName"),
                    Designation = p.Field<string>("DesignationName"),
                    EmployeeName = p.Field<string>("EmployeeName")
                }).ToList();

            return Json(viewEmpInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeOfficeVisitApprove(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsApproved = true;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Approved Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Approval denied";
            }
            return Json(new {result = result, message = message}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeOfficeVisitReject(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsRejected = true;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Rejection denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public void MapDropdownForOrg(EmployeeOtherInformationViewModel model)
        {
            var organizationList = internalOrganizationService.GetMany(p => p.IsActive == true).ToList();
            var viewOrgList = organizationList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.OrganizationName,
                Value = p.OrganizationCode
            }).ToList();
            var orgList = new List<SelectListItem>();
            orgList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            orgList.AddRange(viewOrgList);
            model.OrganizationList = orgList;
        }



        [HttpPost]
        public JsonResult SaveEmployeeLink(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = new LinkWithEmployee();
                model.OrganizationCode = obj.OrganizationCode;
                model.EmployeeCode = obj.RelativeEmployeeCode;
                model.Department = obj.RelativeDepartmentName;
                model.Designation = obj.RelativeDesignationName;
                model.EmployeeName = obj.RelativeEmployeeName;
                model.IsActive = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Create(model);
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeRelativeInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var relativeInfo = linkWithEmployeeService.GetMany(p => p.IsActive == true).ToList();
            var viewRelativeInfo = relativeInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                LinkId = p.LinkId,
                OrganizationCode = p.OrganizationCode,
                RelativeEmployeeCode = p.EmployeeCode,
                RelativeDepartmentName = p.Department,
                RelativeDesignationName = p.Designation,
                RelativeEmployeeName = p.EmployeeName
            }).ToList();
            var currentPageRecords = viewRelativeInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewRelativeInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }


        [HttpPost]
        public JsonResult UpdateEmployeeLink(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = linkWithEmployeeService.GetById(obj.LinkId);
                model.OrganizationCode = obj.OrganizationCode;
                model.EmployeeCode = obj.RelativeEmployeeCode;
                model.Department = obj.RelativeDepartmentName;
                model.Designation = obj.RelativeDesignationName;
                model.EmployeeName = obj.RelativeEmployeeName;
                model.IsActive = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Update(model);
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeeRelativeInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = linkWithEmployeeService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Update(model);
                result = 1;
                message = "Deleted successfully";
                
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult OfficeVisitApproval()
        {
            return View();
        }

        public ActionResult RelationWithEmployee()
        {
            var model = new EmployeeOtherInformationViewModel();
            MapDropdownForOrg(model);
            return View(model);
        }

    }
}