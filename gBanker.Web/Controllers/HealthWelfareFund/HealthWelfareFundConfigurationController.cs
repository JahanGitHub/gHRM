#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service;
using gHRM.Service.WelfareFund;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundConfiguration;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundSetting;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.HealthWelfareFund
{
    public class HealthWelfareFundConfigurationController : BaseController
    {
        #region Private Variables      

        private readonly IHealthWelfareFundSettingService healthWelfareFundSettingService;
        private readonly IHealthWelfareFundConfigurationService healthWelfareFundConfigurationService;
        private readonly IEmployeeService employeeService;

        #endregion

        #region Ctor
        public HealthWelfareFundConfigurationController(
            IHealthWelfareFundSettingService healthWelfareFundSettingService,
            IHealthWelfareFundConfigurationService healthWelfareFundConfigurationService,
            IEmployeeService employeeService
            )
        {
            this.healthWelfareFundSettingService = healthWelfareFundSettingService;
            this.healthWelfareFundConfigurationService = healthWelfareFundConfigurationService;
            this.employeeService = employeeService;
        }

        #endregion
       
        #region Listing

        public ActionResult Index()
        {
            var model = new HealthWelfareFundConfigurationViewModel();
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult GetHealthWelfareFundConfiguration(int id)
        {
            var single = healthWelfareFundConfigurationService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHealthWelfareFundConfigurationListing([DataSourceRequest]DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            var listing = healthWelfareFundConfigurationService.GetAll();
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult Configure(HealthWelfareFundConfigurationViewModel model)

        //int year, int month, int staffWellfareFundSettingsId)
        {
            try
            {
                long createUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                if (model.HealthWelfareFundSettingId <= 0)
                    return Json(new { type = "warning", errorLisings = false, message = "No Health Welfare Fund Setting found" },
                              JsonRequestBehavior.AllowGet);

                var resonse = healthWelfareFundConfigurationService.ConfigureHealthWelfareFund(model.CollectionYear, model.CollectionMonth, model.HealthWelfareFundSettingId, model.CreateUser);
                if (!resonse.IsSuccess)
                    return Json(new { type = "warning", message = resonse.Message },
                             JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = resonse.Message },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [HttpPost]
        public JsonResult Update(HealthWelfareFundConfigurationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var newHealthWelfareFundConfigurationViewModel =
                             Mapper.Map<HealthWelfareFundConfigurationViewModel, HealthWelfareFundConfiguration>(model);
            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            
            var employees = employeeService.GetAll().Select(x => x).ToList();
            foreach (var employee in employees)
            {
                newHealthWelfareFundConfigurationViewModel.EmployeeId = Convert.ToInt32(employee.EmployeeId);
                newHealthWelfareFundConfigurationViewModel.HealthWelfareFundSettingId = model.HealthWelfareFundSettingId;
                newHealthWelfareFundConfigurationViewModel.CollectionDate = DateTime.Now;
                newHealthWelfareFundConfigurationViewModel.CreateUser = createdBy;
                newHealthWelfareFundConfigurationViewModel.IsActive = true;
                var response = healthWelfareFundConfigurationService.Create(newHealthWelfareFundConfigurationViewModel);
            }

            return Json(new { type = "success", message = "success" },
                              JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult Delete(int id)
        //{
        //    var healthWelfareFundConfiguration = healthWelfareFundConfigurationService.GetById(id);

        //    if (healthWelfareFundConfiguration == null)
        //        return Json(new { type = "success", message = "Warning, Health Welfare Fund Configuration not found!" },
        //                     JsonRequestBehavior.AllowGet);

        //    long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //    healthWelfareFundConfiguration.UpdateUser = updatedBy;

        //    var response = healthWelfareFundConfigurationService.Delete(healthWelfareFundConfiguration);

        //    if (!response.IsSuccess)
        //        return Json(new { type = "warning", message = response.Message },
        //                     JsonRequestBehavior.AllowGet);

        //    return Json(new { type = "success", message = response.Message },
        //                      JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Private Methods

        private void MapDropDownList(HealthWelfareFundConfigurationViewModel model)
        {
            var healtWelfareFundSettingList = healthWelfareFundSettingService.GetAll();
            var viewhealtWelfareFundSettingList = healtWelfareFundSettingList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.HealthWelfareFundSettingId.ToString(),
                Text = GetDeductionAmountWithConfiguration(x)
            });
            var healthWelfareFundSetting_items = new List<SelectListItem>();
            healthWelfareFundSetting_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            healthWelfareFundSetting_items.AddRange(viewhealtWelfareFundSettingList);
            model.HealthWelfareFundSettingList = healthWelfareFundSetting_items;

            model.YearList = DateHelper.GetYears(3, 15);
            model.MonthList = DateHelper.GetMonths();
        }

        private string GetDeductionAmountWithConfiguration(HealthWelfareFundSetting healthWelfareFundSetting)
        {
            var deductionAmount = "";
            var isPercentage = healthWelfareFundSetting.IsPercentage ? "%" : "Tk";

            deductionAmount = $"{healthWelfareFundSetting.DeductionAmount} {isPercentage}";

            return deductionAmount;
        }

        #endregion
    }
}
