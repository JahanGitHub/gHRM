#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.Payroll;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class SalaryDateConfigController : BaseController
    {
        #region Private Variables      

        private readonly ISalaryDateConfigService salaryDateConfigService;

        #endregion

        #region Ctor
        public SalaryDateConfigController(
            ISalaryDateConfigService salaryDateConfigService
            )
        {
            this.salaryDateConfigService = salaryDateConfigService;
        }

        #endregion

        #region Listing

        public ActionResult Manage()
        {
            var model = new SalaryDateConfigViewModel
            {
                IsCurrentlyUsing =true
            };

            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult GetSalaryDateConfig(int id)
        {
            var single = salaryDateConfigService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalaryDateConfigListing([DataSourceRequest]DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            var listing = salaryDateConfigService.GetAll();
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNew(SalaryDateConfigViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);
            
            var newMonthlySalaryProcessConfigViewModel =
                             Mapper.Map<SalaryDateConfigViewModel, SalaryDateConfig>(model);

            var validationResponse = salaryDateConfigService
                                        .IsValidSalaryDateConfig(newMonthlySalaryProcessConfigViewModel);

            if(!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newMonthlySalaryProcessConfigViewModel.CreateUser = createdBy;
            newMonthlySalaryProcessConfigViewModel.IsActive = true;
            
            var response = salaryDateConfigService.Create(newMonthlySalaryProcessConfigViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(SalaryDateConfigViewModel model)
        {
            var newSalaryDateConfigViewModel =
                              Mapper.Map<SalaryDateConfigViewModel, SalaryDateConfig>(model);

            var validationResponse = salaryDateConfigService
                                       .IsValidSalaryDateConfig(newSalaryDateConfigViewModel);

            if (!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newSalaryDateConfigViewModel.UpdateUser = updatedBy;
            newSalaryDateConfigViewModel.IsActive = true;

            var response = salaryDateConfigService.Update(newSalaryDateConfigViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var salaryDateConfig = salaryDateConfigService.GetById(id);

            if (salaryDateConfig == null)
                return Json(new { type = "success", message = "Warning, Salary Date Config not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            salaryDateConfig.UpdateUser = updatedBy;

            var response = salaryDateConfigService.Delete(salaryDateConfig);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
