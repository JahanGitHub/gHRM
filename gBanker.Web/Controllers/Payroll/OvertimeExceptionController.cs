using AutoMapper;
using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.Payroll;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Payroll
{
    public class OvertimeExceptionController : Controller
    {
        private readonly IOvertimeExceptionService overtimeExceptionService;

        public OvertimeExceptionController(IOvertimeExceptionService overtimeExceptionService)
        {
            this.overtimeExceptionService = overtimeExceptionService;
        }

        public ActionResult CreateOvertimeException()
        {            
            var model = new OvertimeExceptionViewModel();
            return View(model);
        }
        [HttpPost]
        public JsonResult AddOvertimeException(OvertimeExceptionViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var overtimeExceptionViewModel =
                             Mapper.Map<OvertimeExceptionViewModel, OvertimeException>(model);

            var validationResponse = overtimeExceptionService
                                        .IsValidOvertimeExceptionEffectiveDate(overtimeExceptionViewModel);

            if (!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            overtimeExceptionViewModel.CreateUser = createdBy;
            overtimeExceptionViewModel.IsActive = true;

            var response = overtimeExceptionService.Create(overtimeExceptionViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOvertimeExceptionConfigListing([DataSourceRequest]DataSourceRequest request,
            string searchTerm, string FilterColumn, string FilterValue)
        {
            var filter = new BaseSearchFilter();
            var exceptionInfoListing = overtimeExceptionService.GetListByFilter(filter);
            DataSourceResult result = exceptionInfoListing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOvertimeExceptionList(int id)
        {
            var filter = new BaseSearchFilter { Id=id};
            var listings = overtimeExceptionService.GetListByFilter(filter);
            if (!listings.Any())
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = listings.FirstOrDefault(), isSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateOvertimeConfiguration(OvertimeExceptionViewModel model)
        {
            var newOvertimeExceptionViewModel =
                              Mapper.Map<OvertimeExceptionViewModel, OvertimeException>(model);

            var validationResponse = overtimeExceptionService.IsValidOvertimeExceptionEffectiveDate(newOvertimeExceptionViewModel);

            if (!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newOvertimeExceptionViewModel.UpdateUser = updatedBy;
            newOvertimeExceptionViewModel.IsActive = true;

            var response = overtimeExceptionService.Update(newOvertimeExceptionViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            var overtimeException = overtimeExceptionService.GetById(id);

            if (overtimeException == null)
                return Json(new { type = "success", message = "Warning, TADA Purpose not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            overtimeException.UpdateUser = updatedBy;

            var response = overtimeExceptionService.Delete(overtimeException);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }
    }
}