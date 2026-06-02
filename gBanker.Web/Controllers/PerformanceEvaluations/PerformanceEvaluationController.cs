
#region Usings

using AutoMapper;
using gHRM.Core.Filters.PerformanceEvaluations;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.PerformanceEvaluations;
using gHRM.Service;
using gHRM.Service.PerformanceEvaluations;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.ViewModels.PerformanceEvaluations;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.PerformanceEvaluations
{
    public class PerformanceEvaluationController : BaseController
    {
        #region Private Variables      

        private readonly IPerformanceEvaluationService performanceEvaluationService;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;

        #endregion

        #region Ctor
        public PerformanceEvaluationController(
            IPerformanceEvaluationService performanceEvaluationService, IEmployeeService employeeService, IOfficeService officeService
            )
        {
            this.performanceEvaluationService = performanceEvaluationService; 
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = employeeService;
            this.officeService = officeService;
        }

        #endregion

        #region Listing

        public ActionResult Listing()
        {
            var filter = new PerformanceEvaluationSearchFilter
            {

            };

            var model = new PerformanceEvaluationListViewModel
            {
                SearchFilter = filter,
                Years = DateHelper.GetYears(3, 15),
                Months = DateHelper.GetMonths()
            };

            return View(model);
        }

        #endregion

        #region Manage

        public ActionResult Manage(int? performanceEvaluationId,string employeeCode)
        {
            var model = new AddOrEditPerformanceEvaluationViewModel
            {
                Years = DateHelper.GetYears(3, 15),
                Months = DateHelper.GetMonths()
            };

            if (!(performanceEvaluationId > 0))
            {
                model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
                model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
                model.AreaList = commonDynamicDropDown.ddlInitial();
                model.UnitList = commonDynamicDropDown.ddlInitial();

                return View(model);
            }
            var existingPerformanceEvaluation = performanceEvaluationService.GetById((int)performanceEvaluationId);

            if (existingPerformanceEvaluation==null)
                return View(model);

            //mapp object with model
            model =Mapper.Map< PerformanceEvaluation, AddOrEditPerformanceEvaluationViewModel>(existingPerformanceEvaluation);
            model.EmployeeCode = employeeCode;
            model.Years = DateHelper.GetYears(3, 15);
            model.Months = DateHelper.GetMonths();

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            return View(model);
        }

        #endregion

        #region Ajax Calls        

        public JsonResult GetEmployeeInfoByEmployeeCode(string employeeCode)
        {
            try
            {
                var withResignEmployee = false;

                //get employee information
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim(), withResignEmployee);

                if (employeeInfo == null)
                    return Json(new { type = "warning", message = "Employee not exist. Please try again!" },
                           JsonRequestBehavior.AllowGet);

                var employeeRelatedInfo = new
                {
                    EmployeeId = employeeInfo.EmployeeId,
                    EmployeeName = employeeInfo.EmployeeName,
                    EmployeeEmployeeStatus = EmployeeStatusConstants.GetText(employeeInfo.EmployeeStatusId.ToString()),
                    EmployeeDesignationStatus = employeeInfo.EmployeeDesignation.DesignationName,
                    EmployeeDepartment = employeeInfo.EmployeeDepartment.DepartmentName,
                    OfficeId = employeeInfo.OfficeId
                };

                var model = new AddOrEditPerformanceEvaluationViewModel();

                if (employeeRelatedInfo.OfficeId > 0)
                {
                    var officeTypeId = officeService.GetById(Convert.ToInt32(employeeRelatedInfo.OfficeId)).OfficeTypeId;
                    if (officeTypeId == 6)
                    {
                        var office = officeService.GetById(Convert.ToInt32(employeeRelatedInfo.OfficeId));
                        var thirdLevelOffice = officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault();

                        if (thirdLevelOffice != null)
                            model.AreaId = thirdLevelOffice.OfficeId;

                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = secondLevelOffice.OfficeId;
                        model.UnitId = employeeRelatedInfo.OfficeId;
                        model.OfficeTypeId = (int)officeTypeId;
                    }
                    else if (officeTypeId == 5)
                    {
                        var office = officeService.GetById(Convert.ToInt32(employeeRelatedInfo.OfficeId));
                        model.AreaId = employeeRelatedInfo.OfficeId;
                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = secondLevelOffice.OfficeId;
                        model.OfficeTypeId = (int)officeTypeId;
                    }
                    else if (officeTypeId == 4)
                    {
                        model.ZoneId = employeeRelatedInfo.OfficeId;
                        model.OfficeTypeId = (int)officeTypeId;
                    }
                    else if (officeTypeId == 3)
                    {
                        model.PVProjectId = employeeRelatedInfo.OfficeId;
                        model.OfficeTypeId = (int)officeTypeId;
                    }
                    else if (officeTypeId == 1)
                    {
                        model.PVHeadOfficeId = employeeRelatedInfo.OfficeId;
                        model.OfficeTypeId = (int)officeTypeId;
                    }
                }

                return Json(new { type = "success", employeeInfo = employeeRelatedInfo, officeInfo = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "Employee not exist. Please try again!" },JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPerformanceEvaluation(int id)
        {
            var single = performanceEvaluationService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPerformanceEvaluationListing([DataSourceRequest]DataSourceRequest request,
             string FilterColumn, string FilterValue, int year, int month, string employeeCode)
        {
            int? branchId = null;
            int? officeId = null;

            var filter = new PerformanceEvaluationSearchFilter
            {
                Year = year,
                Month = month,
                EmployeeCode = employeeCode,
                BranchId = branchId,
                OfficeId = officeId
            };

            var listing = performanceEvaluationService.GetByPerformanceEvaluationByFilter(filter);
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Manage(AddOrEditPerformanceEvaluationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            bool isValid = true;

            //validate office
            var validationResponse = ValidateOffice(model, out isValid);
            if (!isValid)
                return validationResponse;

            //get office id
            model.OfficeId = GetOfficeId(model);

            var response = new GlobalResponse<PerformanceEvaluation>();

            var newPerformanceEvaluationViewModel =
                             Mapper.Map<AddOrEditPerformanceEvaluationViewModel, PerformanceEvaluation>(model);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newPerformanceEvaluationViewModel.CreatedBy = createdBy;
            newPerformanceEvaluationViewModel.IsActive = true;

            var exisingPerformanceEvaluation = performanceEvaluationService.GetByYearMonthAndEmployeeId(model.EvaluationYear, model.EvaluationMonth, model.EmployeeId);

            if (exisingPerformanceEvaluation != null)
            {
                long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                newPerformanceEvaluationViewModel.UpdatedBy = updatedBy;
                newPerformanceEvaluationViewModel.PerformanceEvaluationId = exisingPerformanceEvaluation.PerformanceEvaluationId;

                //let's update [PerformanceEvaluation]
                response = performanceEvaluationService.Update(newPerformanceEvaluationViewModel);

                if (!response.IsSuccess)
                    return Json(new { type = "warning", message = response.Message },
                                 JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = response.Message, performanceEvaluationId = response.Result.PerformanceEvaluationId },
                                  JsonRequestBehavior.AllowGet);
            }

            //let's add into [PerformanceEvaluation]
            response = performanceEvaluationService.Create(newPerformanceEvaluationViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message, performanceEvaluationId = response.Result.PerformanceEvaluationId },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var performanceEvaluation = performanceEvaluationService.GetById(id);

            if (performanceEvaluation == null)
                return Json(new { type = "success", message = "Warning, Performance Evaluation not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            performanceEvaluation.UpdatedBy = updatedBy;

            var response = performanceEvaluationService.Delete(performanceEvaluation);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods       

        private JsonResult ValidateOffice(AddOrEditPerformanceEvaluationViewModel model, out bool isValid)
        {
            isValid = false;
            if (model.OfficeTypeId == 6 && (model.UnitId == null || model.UnitId <= 0))
                return Json(new { type = "warning", message = "Unit/Branch Office Required" }, JsonRequestBehavior.AllowGet);

            if (model.OfficeTypeId == 5 && (model.AreaId == null || model.AreaId <= 0))
                return Json(new { type = "warning", message = "Area Office Required" }, JsonRequestBehavior.AllowGet);

            if (model.OfficeTypeId == 4 && (model.ZoneId == null || model.ZoneId <= 0))
                return Json(new { type = "warning", message = "Zone Office Required" }, JsonRequestBehavior.AllowGet);

            if (model.OfficeTypeId == 3 && (model.PVProjectId == null || model.PVProjectId <= 0))
                return Json(new { type = "warning", message = "Project Office Required" }, JsonRequestBehavior.AllowGet);

            if (model.OfficeTypeId == 1 && (model.PVHeadOfficeId == null || model.PVHeadOfficeId <= 0))
                return Json(new { type = "warning", message = "Head Office Required" }, JsonRequestBehavior.AllowGet);

            isValid = true;
            return Json(new { type = "success", message = "Valid" }, JsonRequestBehavior.AllowGet);
        }

        private int GetOfficeId(AddOrEditPerformanceEvaluationViewModel model)
        {
            int officeId = 0;

            switch (model.OfficeTypeId)
            {
                case 6:
                    officeId = (int)model.UnitId;
                    break;

                case 5:
                    officeId = (int)model.AreaId;
                    break;

                case 4:
                    officeId = (int)model.ZoneId;
                    break;

                case 3:
                    officeId = (int)model.PVProjectId;
                    break;

                case 1:
                    officeId = (int)model.PVHeadOfficeId;
                    break;

                default:
                    officeId = (int)model.PVHeadOfficeId;
                    break;
            }

            return officeId;
        }


        #endregion
    }
}
