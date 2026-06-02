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
    public class CompanyWisePayrollConfigController : BaseController
    {
        #region Private Variables      

        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;

        #endregion

        #region Ctor
        public CompanyWisePayrollConfigController(
            ICompanyWisePayrollConfigService companyWisePayrollConfigService
            )
        {
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;
        }

        #endregion

        #region Manage

        public ActionResult Manage()
        {
            var model = new AddOrEditCompanyWisePayrollConfigViewModel
            {
                
            };

            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult GetCompanyWisePayrollConfig(int id)
        {
            var single = companyWisePayrollConfigService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyWisePayrollConfigListing([DataSourceRequest]DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            var listing = companyWisePayrollConfigService.GetAll();
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNew(AddOrEditCompanyWisePayrollConfigViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);
            
            var newMonthlySalaryProcessConfig =
                             Mapper.Map<AddOrEditCompanyWisePayrollConfigViewModel, CompanyWisePayrollConfig>(model);

            var isExistingThisConfig = companyWisePayrollConfigService
                                      .IsExistCompanyWisePayrollConfig(newMonthlySalaryProcessConfig);
            if (isExistingThisConfig)
                return Json(new { type = "warning", message = $"{model.CompanyCode} already eixist. Please try another!" },
                           JsonRequestBehavior.AllowGet);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newMonthlySalaryProcessConfig.CreateUser = createdBy;
            newMonthlySalaryProcessConfig.IsActive = true;
            
            var response = companyWisePayrollConfigService.Create(newMonthlySalaryProcessConfig);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(AddOrEditCompanyWisePayrollConfigViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var updateCompanyWisePayrollConfig =
                              Mapper.Map<AddOrEditCompanyWisePayrollConfigViewModel, CompanyWisePayrollConfig>(model);

            var isExistingThisConfig = companyWisePayrollConfigService
                                       .IsExistCompanyWisePayrollConfig(updateCompanyWisePayrollConfig);

            if (isExistingThisConfig)
                return Json(new { type = "warning", message = $"{model.CompanyCode} already eixist. Please try another!" },
                           JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            updateCompanyWisePayrollConfig.UpdateUser = updatedBy;
            updateCompanyWisePayrollConfig.IsActive = true;

            var response = companyWisePayrollConfigService.Update(updateCompanyWisePayrollConfig);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var companyWisePayrollConfig = companyWisePayrollConfigService.GetById(id);

            if (companyWisePayrollConfig == null)
                return Json(new { type = "success", message = "Warning, Salary Date Config not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            companyWisePayrollConfig.UpdateUser = updatedBy;

            var response = companyWisePayrollConfigService.Delete(companyWisePayrollConfig);

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
