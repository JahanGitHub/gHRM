#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service.WelfareFund;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundSetting;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.WelfareFund
{
    public class HealthWelfareFundSettingController : BaseController
    {
        #region Private Variables      
        
        private readonly IHealthWelfareFundSettingService healthWelfareFundSettingService;

        #endregion

        #region Ctor
        public HealthWelfareFundSettingController(
            IHealthWelfareFundSettingService healthWelfareFundSettingService
            )
        {
            this.healthWelfareFundSettingService = healthWelfareFundSettingService;
        }

        #endregion

        #region Listing

        public ActionResult Index()
        {
            var model = new HealthWelfareFundSettingViewModel{};
            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult GetHealthWelfareFundSetting(int id)
        {
            var single = healthWelfareFundSettingService.GetById(id);
            if (single == null)
            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHealthWelfareFundSettingListing([DataSourceRequest]DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            var listing = healthWelfareFundSettingService.GetAll();
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNew(HealthWelfareFundSettingViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var newHealthWelfareFundSettingViewModel =
                             Mapper.Map<HealthWelfareFundSettingViewModel, HealthWelfareFundSetting>(model);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newHealthWelfareFundSettingViewModel.CreateUser = createdBy;
            newHealthWelfareFundSettingViewModel.IsActive = true;
            var response = healthWelfareFundSettingService.Create(newHealthWelfareFundSettingViewModel);

            var query = $"sp_update_health_fund {model.DeductionAmount}";
            using (var db = new gHRMDBContext())
            {
                db.Database.ExecuteSqlCommand(query);
            }


            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(HealthWelfareFundSettingViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var newHealthWelfareFundSettingViewModel =
                             Mapper.Map<HealthWelfareFundSettingViewModel, HealthWelfareFundSetting>(model);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newHealthWelfareFundSettingViewModel.UpdateUser = updatedBy;
            newHealthWelfareFundSettingViewModel.IsActive = true;

            var response = healthWelfareFundSettingService.Update(newHealthWelfareFundSettingViewModel);



            var query = $"sp_update_health_fund {model.DeductionAmount}";
            using (var db = new gHRMDBContext())
            {
                db.Database.ExecuteSqlCommand(query);
            }

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var healthWelfareFundSetting = healthWelfareFundSettingService.GetById(id);

            if (healthWelfareFundSetting == null)
                return Json(new { type = "success", message = "Warning, Health Welfare Fund Setting not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            healthWelfareFundSetting.UpdateUser = updatedBy;

            var response = healthWelfareFundSettingService.Delete(healthWelfareFundSetting);

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
