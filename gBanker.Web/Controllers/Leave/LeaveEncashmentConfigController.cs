using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using gHRM.Web.DropDownService;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;

using Kendo.Mvc.Extensions;
using gHRM.Web.CommonDropdown;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class LeaveEncashmentConfigController : BaseController
    {
        #region Variable

        private readonly IELEncashmentConfigurationService eLEncashmentConfigurationService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public LeaveEncashmentConfigController(IELEncashmentConfigurationService eLEncashmentConfigurationService)
        {
            this.eLEncashmentConfigurationService = eLEncashmentConfigurationService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion


        #region Events

        public ActionResult Index()
        {
            ViewBag.STAGE_DISABLED = "true" == GetSetting("EL_ENCASHMENT_CONFIGURATION_STAGE_DISABLED");
            var model = new ELEncashmentConfigurationViewModel();
            MapDropdownForELEncashmentConfiguration(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult ListELEncashmentConfiguration(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            List<ELEncashmentConfiguration> eLEncashmentConfigurations = new List<ELEncashmentConfiguration>();
            try
            {
                eLEncashmentConfigurations = eLEncashmentConfigurationService.GetMany(t => t.IsActive == true).ToList();

                var currentPageRecords = eLEncashmentConfigurations.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = eLEncashmentConfigurations.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                var meed = ex.InnerException.Message;
                return Json(new { Result = "OK", Records = 0, TotalRecordCount = 0, JsonRequestBehavior.AllowGet });
            }
        }


        public JsonResult SaveELEncashmentConfiguration(ELEncashmentConfiguration eLEncashmentConfiguration)
        {
            var result = string.Empty;
            try
            {
                var existingEncashmentStage = eLEncashmentConfigurationService
                                .GetMany(p => p.IsActive
                                                && p.EncashmentStage == eLEncashmentConfiguration.EncashmentStage)
                                .FirstOrDefault();

                if (existingEncashmentStage != null)
                {
                    existingEncashmentStage.IsActive = false;
                    eLEncashmentConfigurationService.Update(existingEncashmentStage);
                }

                /*
                var isDuplicate = eLEncashmentConfigurationService.GetMany(p =>p.IsActive == true && 
                                                    p.MinimumBalance == eLEncashmentConfiguration.MinimumBalance)
                                        .ToList();

                if (isDuplicate.Any())
                {
                    result = "Duplicate Minimum Balance found, Save denied";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                */

                var entity = new ELEncashmentConfiguration();
                entity.ConfigurationId = eLEncashmentConfiguration.ConfigurationId;
                entity.EncashmentStage = eLEncashmentConfiguration.EncashmentStage;
                entity.EligibleFrom = eLEncashmentConfiguration.EligibleFrom;
                entity.EligibilityDuration = eLEncashmentConfiguration.EligibilityDuration;
                entity.MinimumBalance = eLEncashmentConfiguration.MinimumBalance;

                entity.EncashmentEligibleQuantity = eLEncashmentConfiguration.EncashmentEligibleQuantity;
                entity.Formula = "" == eLEncashmentConfiguration.Formula ? null : eLEncashmentConfiguration.Formula;

                entity.IsActive = true;
                entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;

                //let's add el encashment configuration [leave.ELEncashmentConfiguration]
                eLEncashmentConfigurationService.Create(entity);

                result = "Save Successfull";
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateELEncashmentConfiguration(ELEncashmentConfiguration eLEncashmentConfiguration)
        {
            var result = string.Empty;
            try
            {
                /*
                var isDuplicate =
                   eLEncashmentConfigurationService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.ConfigurationId != eLEncashmentConfiguration.ConfigurationId &&
                               p.MinimumBalance == eLEncashmentConfiguration.MinimumBalance).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Minimum Balance found, Save denied";
                }
                else
                {
                    */
                var entity = eLEncashmentConfigurationService.GetById(eLEncashmentConfiguration.ConfigurationId);
                if (entity == null)
                {
                    result = "Duplicate Minimum Balance found, Save denied";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                entity.ConfigurationId = eLEncashmentConfiguration.ConfigurationId;
                //entity.EncashmentStage = eLEncashmentConfiguration.EncashmentStage;
                entity.EligibleFrom = eLEncashmentConfiguration.EligibleFrom;
                entity.EligibilityDuration = eLEncashmentConfiguration.EligibilityDuration;
                entity.MinimumBalance = eLEncashmentConfiguration.MinimumBalance;
                entity.IsActive = true;
                entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                entity.EncashmentEligibleQuantity = eLEncashmentConfiguration.EncashmentEligibleQuantity;
                entity.Formula = "" == eLEncashmentConfiguration.Formula ? null : eLEncashmentConfiguration.Formula;
                eLEncashmentConfigurationService.Update(entity);

                result = "Update Successfull";
                /*    
                }
                */
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteELEncashmentConfiguration(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = eLEncashmentConfigurationService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                eLEncashmentConfigurationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Method

        private void MapDropdownForELEncashmentConfiguration(ELEncashmentConfigurationViewModel model)
        {
            string STAGE_DEFAULT = GetSetting("EL_ENCASHMENT_CONFIGURATION_STAGE_DEFAULT");
            var EncashmentStage = new List<SelectListItem>();
            EncashmentStage.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            EncashmentStage.Add(new SelectListItem() { Text = "General", Value = "General" });
            EncashmentStage.Add(new SelectListItem() { Text = "First", Value = "First" });
            EncashmentStage.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            foreach (var StageItem in EncashmentStage)
            {
                if (StageItem.Value == STAGE_DEFAULT) StageItem.Selected = true;
            }
            var FormulaList = new List<SelectListItem>();
            FormulaList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            FormulaList.Add(new SelectListItem() { Text = EncashmentFormulaConstants.HalfIfLessThanMinimum, Value = EncashmentFormulaConstants.HalfIfLessThanMinimum });

            model.EncashmentStageList = EncashmentStage;
            model.EligibleFromList = commonStaticDropDown.GetLeaveEligibleDateList();
            model.EligibilityDurationList = commonStaticDropDown.Get1To10NumberList();
            model.FormulaList = FormulaList;
        }

        #endregion
    }
}
