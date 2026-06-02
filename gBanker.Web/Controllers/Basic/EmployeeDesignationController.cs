using System.Data;
using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class EmployeeDesignationController : BaseController
    {
        #region variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        //private readonly IEmployeeSalaryScaleService employeeSalaryScaleService;
        private readonly IEmployeeEquivalentDesignationService employeeEquivalentDesignationService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IEmployeeDesignationMappingService employeeDesignationMappingService;
        private readonly IEmployeeSPService emploeSpService;
        private readonly IEmployeeSignatureDesignationService employeeSignatureDesignationService;
        private readonly IOfficeTypeService officeTypeService;
        //private readonly IEmployeeDepartmentService employeeDepartmentService;
        //private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;

        public EmployeeDesignationController(
            IEmployeeDesignationService employeeDesignationService,
            //IEmployeeSalaryScaleService employeeSalaryScaleService,
            IEmployeeService employeeService,
            IEmployeeEquivalentDesignationService employeeEquivalentDesignationService,
            IOfficeDesignationService officeDesignationService,
            IEmployeeDesignationMappingService employeeDesignationMappingService,
            IEmployeeSPService emploeSpService,
            IEmployeeSignatureDesignationService employeeSignatureDesignationService,
            IOfficeTypeService officeTypeService
            //IEmployeeDepartmentService employeeDepartmentService
           // IEmployeeDepartmentSectionService employeeDepartmentSectionService
            )
        {
            this.employeeService = employeeService;
            this.employeeDesignationService = employeeDesignationService;
            //this.employeeSalaryScaleService = employeeSalaryScaleService;
            this.employeeEquivalentDesignationService = employeeEquivalentDesignationService;
            this.officeDesignationService = officeDesignationService;
            this.employeeDesignationMappingService = employeeDesignationMappingService;
            this.emploeSpService = emploeSpService;
            this.employeeSignatureDesignationService = employeeSignatureDesignationService;
            this.officeTypeService = officeTypeService;
            //this.employeeDepartmentService = employeeDepartmentService;
            //this.employeeDepartmentSectionService = employeeDepartmentSectionService;
        }
        #endregion

        #region Events

        // GET: EmployeeDesignation
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeDesignation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeDesignation/Create
        public ActionResult Create()
        {
            var model = new EmployeeDesignationViewModel();
            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmployeeDesignationViewModel model)
        {
            var message = "";
            try
            {
                var checkDuplicate =
                    employeeDesignationService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.DesignationName.ToUpper().Trim() == model.DesignationName.ToUpper().Trim())
                        .ToList();
                if (checkDuplicate.Any())
                {
                    message = "This Designation Name already exists, save denied";
                    ViewBag.Itemmsg = "This Designation Name already exists, save denied";
                    return GetErrorMessageResult(message);
                }
                else
                {
                    var entity = new EmployeeDesignation();
                    entity.DesignationCode = model.DesignationCode;
                    entity.Rank = model.Rank;
                    entity.DesignationName = model.DesignationName;
                    entity.DesignationShortName = model.DesignationShortName;
                    entity.DesignationType = model.DesignationType;
                    entity.InsuranceAmount = model.InsuranceAmount;
                    entity.IsActive = true;
                    entity.CreateUser = SessionHelper.LoggedInEmployeeID;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeDesignationService.Create(entity);
                    return GetSuccessMessageResult();
                }
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: EmployeeDesignation/Edit/5
        public ActionResult Edit(int id)
        {
            /*
            if (employeeService.IsContinued(id))
            {
                var designation = employeeDesignationService.GetById(Convert.ToInt32(id));
                var entity = Mapper.Map<EmployeeDesignation, EmployeeDesignationViewModel>(designation);
                //ViewData["EmployeeId"] = id.ToString();
                MapDropDownList(entity);
                return View(entity);
            }
            else
            {
                ModelState.AddModelError("Validation", "Discontinued Designation, please enter a diferent Designation Code and Name.");
            }

             return RedirectToAction("Index");

            */

            var designation = employeeDesignationService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<EmployeeDesignation, EmployeeDesignationViewModel>(designation);
            //ViewData["EmployeeId"] = id.ToString();
            MapDropDownList(entity);
            return View(entity);

           
        }


        private string CheckDuplicateEmployeeDesignation(EmployeeDesignationViewModel model)
        {
            string message = "";

            var previousDesignation = employeeDesignationService.GetById(model.DesignationId);

            if (model.DesignationName != previousDesignation.DesignationName)
            {
                if (employeeDesignationService.GetAll().Where(p => p.IsActive == true
                && p.DesignationName.ToUpper().Trim() == model.DesignationName.ToUpper().Trim()
                && p.DesignationId != model.DesignationId).Any())
                {
                    message = "Duplicate Designation Name already exist, update denied";
                    return message;
                }
            }

            if (model.DesignationShortName != previousDesignation.DesignationShortName)
            {
                if (employeeDesignationService.GetAll().Where(p => p.IsActive == true
                && p.DesignationShortName.ToUpper().Trim() == model.DesignationShortName.ToUpper().Trim()
                && p.DesignationId != model.DesignationId).Any())
                {
                    message = "Duplicate Designation Short Name already Exist, update denied";
                    return message;
                }
            }

            return message;
        }


        [HttpPost]
        public ActionResult Edit(EmployeeDesignationViewModel model)
        {
            try
            {
                string message = CheckDuplicateEmployeeDesignation(model);

                if (String.IsNullOrEmpty(message))
                {
                    var entity = employeeDesignationService.GetById(model.DesignationId);
                    entity.DesignationCode = model.DesignationCode;
                    entity.Rank = model.Rank;
                    entity.DesignationName = model.DesignationName;
                    entity.DesignationShortName = model.DesignationShortName;
                    entity.DesignationType = model.DesignationType;
                    entity.InsuranceAmount = model.InsuranceAmount;
                    entity.IsActive = true;
                    entity.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeDesignationService.Update(entity);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult(message);

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: EmployeeDesignation/Delete/5
        public ActionResult Delete(int id)
        {
            //employeeDesignationService.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: EmployeeDesignation/Delete/5
        [HttpPost]
        public ActionResult Delete(EmployeeDesignationViewModel model)
        {
            try
            {
                var entity = Mapper.Map<EmployeeDesignationViewModel, EmployeeDesignation>(model);
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeDesignationService.Update(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DesignationMapping()
        {
            var model = new EmployeeDesignationMappingViewModel();
            MapDropdownForDesignation(model);
            return View(model);
        }

        public ActionResult SignatureDesignation()
        {
            return View();
        }

        //public ActionResult DepartmentSection()
        //{
        //    var model = new EmployeeDepartmentSectionViewModel();
        //    MapDropdownForDepartmentSection(model);
        //    return View(model);
        //}

        public ActionResult GetDesignation([DataSourceRequest]DataSourceRequest request)
        {

            var designationList = employeeDesignationService.GetMany(p => p.IsActive == true).ToList();
            var viewList = designationList.AsEnumerable().Select((p, sl) => new EmployeeDesignationViewModel()
            {
                rowSl = sl + 1,
                DesignationId = p.DesignationId,
                DesignationName = p.DesignationName,
                DesignationCode = p.DesignationCode,
                Rank = p.Rank,
                DesignationShortName = p.DesignationShortName,
                DesignationType = p.DesignationType,
                InsuranceAmount = p.InsuranceAmount
            }).ToList();
            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSignatureDesignation([DataSourceRequest]DataSourceRequest request)
        {

            var signatureInfo = employeeSignatureDesignationService.GetMany(p => p.IsActive == true).ToList();
            var viewList = signatureInfo.AsEnumerable().Select((p, sl) => new EmployeeSignatureDesignationViewModel()
            {
                rowSl = sl + 1,
                SignatureId = p.SignatureId,
                SignatureCode = p.SignatureCode,
                SignatureName = p.SignatureName
            }).ToList();
            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HttpRequests

        public JsonResult DesignationDelete(string designationId)
        {
            var result = 0;
            int newDesignationId = Convert.ToInt32(designationId);
            var IsAssignEmployee =
                    employeeService.GetMany(
                            p =>
                                p.IsActive == true &&
                                p.DesignationId == newDesignationId)
                        .ToList();
            if (IsAssignEmployee.Any())
            {
                result = 0;
            }
            else
            {
                var entity = employeeDesignationService.GetById(Convert.ToInt32(designationId));
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.InActiveDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    employeeDesignationService.Update(entity);
                    result = 1;
                }
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmployeeDesignationMapping(EmployeeDesignationMapping obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var isDuplicate =
                    employeeDesignationMappingService.GetMany(
                            p =>
                                p.IsActive == 1 && p.EquivalentDesignationId == obj.EquivalentDesignationId &&
                                p.OrnamentalDesginationid == obj.OrnamentalDesginationid &&
                                p.OfficeDesignationId == obj.OfficeDesignationId)
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = 0;
                    message = "Employee designation mapping already exists, Save denied";
                }
                else
                {
                    var model = new EmployeeDesignationMapping();
                    model.EquivalentDesignationId = obj.EquivalentDesignationId;
                    model.OrnamentalDesginationid = obj.OrnamentalDesginationid;
                    model.OfficeDesignationId = obj.OfficeDesignationId;
                    model.IsActive = 1;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeDesignationMappingService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDesignationMappingList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var mappingList = emploeSpService.GetDataWithoutParameter("basic.SP_GetDesignationMappingList");
                var viewMappingList =
                    mappingList.Tables[0].AsEnumerable().Select(p => new EmployeeDesignationMappingViewModel()
                    {
                        DesignationMapId = p.Field<int>("DesignationMapId"),
                        EquivalentDesignationId = p.Field<int>("EquivalentDesignationId"),
                        OrnamentalDesginationid = p.Field<int>("OrnamentalDesginationid"),
                        OfficeDesignationId = p.Field<int>("OfficeDesignationId"),
                        EquivalentDesignationName = p.Field<string>("EquivalentDesignationName"),
                        OfficeDesginationName = p.Field<string>("OffcDesignName"),
                        EmployeeDesignationName = p.Field<string>("DesignationName")
                    }).ToList();

                var currentPageRecords = viewMappingList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewMappingList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult UpdateEmployeeDesignationMapping(EmployeeDesignationMapping obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var isDuplicate =
                    employeeDesignationMappingService.GetMany(
                            p =>
                                p.IsActive == 1 && p.DesignationMapId != obj.DesignationMapId && p.EquivalentDesignationId == obj.EquivalentDesignationId &&
                                p.OrnamentalDesginationid == obj.OrnamentalDesginationid &&
                                p.OfficeDesignationId == obj.OfficeDesignationId)
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = 0;
                    message = "Employee designation mapping already exists, Update denied";
                }
                else
                {
                    var model = employeeDesignationMappingService.GetById(obj.DesignationMapId);
                    model.EquivalentDesignationId = obj.EquivalentDesignationId;
                    model.OrnamentalDesginationid = obj.OrnamentalDesginationid;
                    model.OfficeDesignationId = obj.OfficeDesignationId;
                    model.IsActive = 1;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeDesignationMappingService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDesignationMapping(int DesignationMapId)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeDesignationMappingService.GetById(DesignationMapId);
                model.IsActive = 0;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeDesignationMappingService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSignatureDesignation(EmployeeSignatureDesignationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicate =
                    employeeSignatureDesignationService.GetMany(
                            p =>
                                p.IsActive == true &&
                                (p.SignatureCode.ToUpper().Trim() == obj.SignatureCode.ToUpper().Trim() || p.SignatureName.ToUpper().Trim() == obj.SignatureName.ToUpper().Trim()))
                        .ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate signature entry found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = new EmployeeSignatureDesignation();
                    model.SignatureCode = obj.SignatureCode;
                    model.SignatureName = obj.SignatureName;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeSignatureDesignationService.Create(model);
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

        //[HttpPost]
        //public ActionResult GetSignatureDesignation(int jtStartIndex, int jtPageSize, string jtSorting)
        //{
        //    var signatureInfo = employeeSignatureDesignationService.GetMany(p => p.IsActive == true).ToList();

        //    var currentPageRecords = signatureInfo.Skip(jtStartIndex).Take(jtPageSize);
        //    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = signatureInfo.LongCount(), JsonRequestBehavior.AllowGet });
        //}

        [HttpPost]
        public JsonResult UpdateSignatureDesignation(EmployeeSignatureDesignationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var previousSignatureDesignation = employeeSignatureDesignationService.GetById(obj.SignatureId);

                bool SignatureCodeSame = previousSignatureDesignation.SignatureCode.ToUpper().Trim() != obj.SignatureCode.ToUpper().Trim();
                bool SignatureNameSame = previousSignatureDesignation.SignatureCode.ToUpper().Trim() != obj.SignatureCode.ToUpper().Trim();

                if (!SignatureCodeSame && !SignatureNameSame)
                {

                    if (!SignatureCodeSame)
                    {
                        var checkDuplicate =
                        employeeSignatureDesignationService.GetMany(
                                p =>
                                    p.IsActive == true &&
                                    (p.SignatureCode.ToUpper().Trim() == obj.SignatureCode.ToUpper().Trim() && p.SignatureId != obj.SignatureId))
                            .ToList();

                        if (checkDuplicate.Any())
                        {
                            result = 0;
                            message = "Duplicate signature Code found, update denied";
                            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (!SignatureNameSame)
                    {
                        var checkDuplicate =
                        employeeSignatureDesignationService.GetMany(
                                p =>
                                    p.IsActive == true &&
                                    (p.SignatureName.ToUpper().Trim() == obj.SignatureName.ToUpper().Trim() && p.SignatureId != obj.SignatureId))
                            .ToList();

                        if (checkDuplicate.Any())
                        {
                            result = 0;
                            message = "Duplicate signature Name found, update denied";
                            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var model = employeeSignatureDesignationService.GetById(obj.SignatureId);
                    model.SignatureCode = obj.SignatureCode.Trim();
                    model.SignatureName = obj.SignatureName.Trim();
                    model.IsActive = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeSignatureDesignationService.Update(model);

                    result = 1;
                    message = "Updated successfully";
                }
                else
                {
                    result = 0;
                    message = "No Changes found";
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSignatureDesignation(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeSignatureDesignationService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeSignatureDesignationService.Update(model);
                result = 1;
                message = "Deleted sucessfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        private void MapDropDownList(EmployeeDesignationViewModel model)
        {
            //var SalaryList = employeeSalaryScaleService.GetAll();
            //var viewSalary = SalaryList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.SalaryScaleId.ToString(),
            //    Text = x.Salary.ToString()
            //});
            var pSal_items = new List<SelectListItem>();
            pSal_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //pSal_items.AddRange(viewSalary);
            model.SalaryScaleList = pSal_items;
            model.DesignationTypeList = getDesignationTypeList();

            var rankList = new List<SelectListItem>();
            rankList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            int MaxRank = 50;

            if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.Mousumi)
            {
                MaxRank = 100;
            }

            for (var i = 1; i <= MaxRank; i++)
            {
                rankList.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            model.RankList = rankList;

        }
        private string GetNewDesignationCode()
        {
            string new_code = "";

            var designationLastId = employeeDesignationService.GetAll().Select(d => d.DesignationId).Max();
            if (designationLastId == 0)
            {
                new_code = "0001";
            }
            else
            {
                long last_code = designationLastId + 1;
                new_code = last_code.ToString().PadLeft(4, '0');
            }

            return new_code;
        }

        public void MapDropdownForDesignation(EmployeeDesignationMappingViewModel model)
        {
            var equivList = employeeEquivalentDesignationService.GetMany(p => p.IsActive == true);
            var view_EquivList = equivList.Select(p => new SelectListItem()
            {
                Value = p.EquivalentDesigId.ToString(),
                Text = p.EquivalentDesignationName
            }).ToList();
            var listOfEquivDesig = new List<SelectListItem>();
            listOfEquivDesig.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listOfEquivDesig.AddRange(view_EquivList);
            model.EquivalenDesignationList = listOfEquivDesig;

            var empDesignation = employeeDesignationService.GetMany(p => p.IsActive == true);
            var view_empDesignation = empDesignation.Select(p => new SelectListItem()
            {
                Value = p.DesignationId.ToString(),
                Text = p.DesignationName
            }).ToList();
            var empDesig = new List<SelectListItem>();
            empDesig.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            empDesig.AddRange(view_empDesignation);
            model.OfficeDesignationList = empDesig;

            var offDesig = officeDesignationService.GetMany(p => p.IsActive == true);
            var view_offDesig = offDesig.Select(p => new SelectListItem()
            {
                Value = p.OfficeDesignationId.ToString(),
                Text = p.OffcDesignName
            }).ToList();
            var listOfOffDesig = new List<SelectListItem>();
            listOfOffDesig.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listOfOffDesig.AddRange(view_offDesig);
            model.OrnamentalDesignationList = listOfOffDesig;
        }

        //private void MapDropdownForDepartmentSection(EmployeeDepartmentSectionViewModel model)
        //{
        //    var officeTypeList = new List<SelectListItem>();
        //    officeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

        //    var offType = officeTypeService.GetMany(p => p.IsActive == true).FirstOrDefault(); //Head Office 
        //    //var officeType = officeTypeService.GetMany(p => p.IsActive == true).ToList();

        //    if (offType != null)
        //    {
        //        var viewOfficeType = new SelectListItem()
        //        {
        //            Text = offType.OfficeTypeName,
        //            Value = offType.OfficeTypeId.ToString()
        //        };

        //        //var viewOfficeType = officeType.AsEnumerable().Select(p => new SelectListItem()
        //        //{
        //        //    Text = p.OfficeTypeName,
        //        //    Value = p.OfficeTypeId.ToString()
        //        //}).ToList();

        //        officeTypeList.Add(viewOfficeType);
        //        //officeTypeList.AddRange(viewOfficeType);
        //    }

        //    model.OfficeTypeList = officeTypeList;

        //    var deptList = new List<SelectListItem>();
        //    deptList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
        //    model.DepartmentList = deptList;

        //}

        #endregion
    }
}
