using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Models;
using gHRM.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.Core.Extensions;
using gHRM.Web.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
    public class EmployeeDepartmentController : BaseController
    {
        #region variables
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly ICompanyService companyService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;

        public EmployeeDepartmentController(
            IEmployeeDepartmentService employeeDepartmentService
            , ICompanyService companyService
            , IOfficeTypeService officeTypeService
            , IEmployeeService employeeService
            , IEmployeeDepartmentSectionService employeeDepartmentSectionService)
        {
            this.companyService = companyService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.officeTypeService = officeTypeService;
            this.employeeService = employeeService;
            this.employeeDepartmentSectionService = employeeDepartmentSectionService;

        }

        #endregion

        #region Events
        // GET: EmployeeDepartment
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeDepartment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeDepartment/Create
        public ActionResult Create()
        {
            var model = new EmployeeDepartmentViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // POST: EmployeeDepartment/Create
        [HttpPost]
        public JsonResult OfficeTypeWiseDepartmentAddLoop(EmployeeDepartmentViewModel model)
        {
            var result = 0;
            var message = "";
            try
            {
                // bool savedData = false;
                foreach (var item in model.OfficeTypeIdList)
                {
                    var entity = Mapper.Map<EmployeeDepartmentViewModel, EmployeeDepartment>(model);
                    entity.OfficeTypeId = Convert.ToInt32(item);
                    if (ModelState.IsValid)
                    {
                        var errors = employeeDepartmentService.IsValidDepartment(entity.DepartmentId);

                        if (errors.ToList().Count == 0)
                        {
                            var checkDuplicateDepartment = employeeDepartmentService.GetMany(p => p.IsActive == true && p.DepartmentName.Trim().ToUpper() == model.DepartmentName.Trim().ToUpper() && p.DepartmentShortName == model.DepartmentShortName).ToList();
                            if (checkDuplicateDepartment.Any())
                            {
                                //continue;
                                result = 0;
                                message = "Duplicate Department Short Name found, Save denied";
                            }
                            else
                            {
                                entity.CompanyId = CompanyID;
                                entity.IsActive = true;
                                entity.InActiveDate = DateTime.Now;
                                employeeDepartmentService.Create(entity);
                                result = 1;
                                message = "Department Saved Successfully";
                                // savedData = true;
                            }
                        }
                        else
                        {
                            result = 0;
                            message = "Department Save Failed";
                        }
                    }
                    else
                    {
                        result = 0;
                        message = "Department Save Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Department Save Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        // GET: EmployeeDepartment/Edit/5
        public ActionResult Edit(int id)
        {
            var department = employeeDepartmentService.GetById(Convert.ToInt32(id));
            var state = employeeDepartmentService.GetById(Convert.ToInt32(department.DepartmentId));
            var entity = Mapper.Map<EmployeeDepartment, EmployeeDepartmentViewModel>(department);
            MapDropDownList(entity);
            return View(entity);
        }

        // POST: EmployeeDepartment/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeDepartmentViewModel model)
        {
            try
            {
                var checkDuplicateDepartment = employeeDepartmentService.GetMany(p => p.OfficeTypeId == model.OfficeTypeId && p.DepartmentName.ToUpper().Trim() == model.DepartmentName.ToUpper().Trim() && p.DepartmentId != model.DepartmentId && p.IsActive == true).ToList();
                if (checkDuplicateDepartment.Any())
                {
                    return Json(new { Result = "Error", Message = "Duplicate Department Found, Edit Denied" }, JsonRequestBehavior.AllowGet);
                }

                var entity = Mapper.Map<EmployeeDepartmentViewModel, EmployeeDepartment>(model);
                var getDepartmentDetails = employeeDepartmentService.GetById(Convert.ToInt32(entity.DepartmentId));
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getDepartmentDetails.DepartmentName = entity.DepartmentName;
                    getDepartmentDetails.DepartmentCode = entity.DepartmentCode;
                    getDepartmentDetails.DepartmentShortName = entity.DepartmentShortName;
                    getDepartmentDetails.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    getDepartmentDetails.UpdateDate = DateTime.Now;
                    getDepartmentDetails.OfficeTypeId = entity.OfficeTypeId;
                    employeeDepartmentService.Update(getDepartmentDetails);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }

        }

        // GET: EmployeeDepartment/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = employeeDepartmentService.GetById(id);
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    employeeDepartmentService.Update(entity);
                }

                return Json(new { Result = "OK" });
            }
            catch
            {
                return View();
            }

        }

        public ActionResult DepartmentSection()
        {
            var model = new EmployeeDepartmentSectionViewModel();
            MapDropdownForDepartmentSection(model);
            return View(model);
        }

        #endregion
        
        #region HttpRequests

        public JsonResult GetDepartment(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                var departmentDetail = employeeDepartmentService.GetDepartmentDetail(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = departmentDetail.ToList();
                var currentPageRecords = detail.ToList();

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //public JsonResult GetDepartment([DataSourceRequest]DataSourceRequest request)
        //{
        //    try
        //    {
        //        long TotCount;

        //        var departmentDetail = employeeDepartmentService.GetAll().Where(x => x.IsActive == true);
        //        var detail = departmentDetail.ToList();

        //        DataSourceResult result = detail.ToDataSourceResult(request);
        //        return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public JsonResult GetSubDeptList(string employeeDeptId)
        {
            var subDeptList = employeeDepartmentService.GetMany(w => w.DepartmentId == Convert.ToInt32(employeeDeptId));
            var viewSubDept = subDeptList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DepartmentId.ToString(),
                Text = string.Format("{0} - {1}", x.DepartmentCode, x.DepartmentName)
            });
            var subDept_items = new List<SelectListItem>();
            subDept_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            subDept_items.AddRange(viewSubDept);
            return Json(subDept_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult departmentDelete(string departmentId)
        {
            var result = 0;
            var message = "";
            var entity = employeeDepartmentService.GetById(Convert.ToInt32(departmentId));
            var checkEmployeeExistsInDepartemnt = employeeService.GetMany(p => p.IsActive == true && p.DepartmentId == entity.DepartmentId);
            if (checkEmployeeExistsInDepartemnt.Any())
            {
                result = 0;
                message = "Employee exists in this departemnt, delete denied";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeDepartmentService.Update(entity);
                result = 1;
                message = "Deleted Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetOfficeTypeWiseDepartment(int officeType)
        {
            var department = employeeDepartmentService.GetMany(p => p.IsActive == true && p.OfficeTypeId == officeType).ToList();
            var viewDepartment = department.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.DepartmentName,
                Value = p.DepartmentId.ToString()
            }).ToList();
            var deptList = new List<SelectListItem>();
            deptList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            deptList.AddRange(viewDepartment);
            return Json(deptList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveDepartmentSection(EmployeeDepartmentSectionViewModel obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var section =
                    employeeDepartmentSectionService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true && p.DepartmentId == obj.DepartmentId && (p.SectionCode.ToUpper().Trim() == obj.SectionCode.ToUpper().Trim() ||
                                p.SectionName.ToUpper().Trim() == obj.SectionName.ToUpper().Trim()))
                        .ToList();
                if (section.Any())
                {
                    result = 0;
                    message = "Duplicate section found, save Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = new EmployeeDepartmentSection();
                    model.DepartmentId = obj.DepartmentId;
                    model.SectionCode = obj.SectionCode;
                    model.SectionName = obj.SectionName;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeDepartmentSectionService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionInfo([DataSourceRequest]DataSourceRequest request, int DeptId)
        {
            var sectionInfo = employeeDepartmentSectionService.GetMany(p => p.IsActive == true && p.DepartmentId == DeptId).ToList();
            DataSourceResult result = sectionInfo.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDepartmentSection(EmployeeDepartmentSectionViewModel obj)
        {
            int result = 0;
            var message = "";
            try
            {
                var checkDuplicateSection = employeeDepartmentSectionService.GetMany(p =>
                                p.IsActive == true && p.SectionId != obj.SectionId &&
                               (p.SectionCode.ToUpper().Trim() == obj.SectionCode.ToUpper().Trim() || p.SectionName.ToUpper().Trim() == obj.SectionName.ToUpper().Trim())).ToList();


                if (checkDuplicateSection.Any())
                {
                    message = "Duplicate section found";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = employeeDepartmentSectionService.GetById(obj.SectionId);
                    model.DepartmentId = obj.DepartmentId;
                    model.SectionCode = obj.SectionCode;
                    model.SectionName = obj.SectionName;
                    model.IsActive = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeDepartmentSectionService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
            }
            catch (Exception ex)
            {
                message = "Updated Denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDepartmentSection(int Id)
        {

            var result = 0;
            var message = "";
            var IsAssignEmployee =
                    employeeService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.SectionId == Convert.ToInt32(Id))
                        .ToList();
            if (IsAssignEmployee.Any())
            {
                result = 0;
                message = "Employee Already Assigned Cannot Delete Department Section";
            }
            else
            {
                var model = employeeDepartmentSectionService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeDepartmentSectionService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Methods

        private void MapDropDownList(EmployeeDepartmentViewModel model)
        {
            //Parent Department
            var officeTypeList = officeTypeService.GetMany(w => w.IsActive == true && w.OfficeTypeCode.Trim() == "HO");
            var viewOfficeType = officeTypeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeTypeCode, x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewOfficeType);
            model.OfficeTypeList = officeType_items;
        }

        private void MapDropdownForDepartmentSection(EmployeeDepartmentSectionViewModel model)
        {
            var officeTypeList = new List<SelectListItem>();
            officeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

            var offType = officeTypeService.GetMany(p => p.IsActive == true).FirstOrDefault(); //Head Office 
            //var officeType = officeTypeService.GetMany(p => p.IsActive == true).ToList();

            if (offType != null)
            {
                var viewOfficeType = new SelectListItem()
                {
                    Text = offType.OfficeTypeName,
                    Value = offType.OfficeTypeId.ToString()
                };

                //var viewOfficeType = officeType.AsEnumerable().Select(p => new SelectListItem()
                //{
                //    Text = p.OfficeTypeName,
                //    Value = p.OfficeTypeId.ToString()
                //}).ToList();

                officeTypeList.Add(viewOfficeType);
                //officeTypeList.AddRange(viewOfficeType);
            }

            model.OfficeTypeList = officeTypeList;

            var deptList = new List<SelectListItem>();
            deptList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            model.DepartmentList = deptList;

        }
        #endregion

    }
}
