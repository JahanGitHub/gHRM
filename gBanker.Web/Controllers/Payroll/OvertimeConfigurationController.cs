using AutoMapper;
using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.Payroll;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Payroll;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Payroll
{
    public class OvertimeConfigurationController : BaseController
    {
        #region Private Variables
        private readonly IOvertimeConfigurationService overtimeConfigurationService;       

        public OvertimeConfigurationController(IOvertimeConfigurationService overtimeConfigurationService)
        {
            this.overtimeConfigurationService = overtimeConfigurationService;            
        }

        #endregion

        #region Methods

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getOverTimeConfurations([DataSourceRequest]DataSourceRequest request)
        {
            try
            {              
                var list_ViewModel = overtimeConfigurationService.GetAll().ToList();                                
                return Json(new { data = list_ViewModel, total = 5 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                var model = new OvertimeConfigurationViewModel();
                var lastOvertimeConfig = overtimeConfigurationService.LastOvertimeConfiguration();

                if (lastOvertimeConfig == null)
                {
                    model.PrevHourTo = 0;
                    model.HourFrom = 1;
                    model.Rank = 1;
                }
                else
                {
                    model.PrevHourTo = lastOvertimeConfig.HourTo;
                    model.HourFrom = lastOvertimeConfig.HourTo + 1;
                    model.Rank = Convert.ToInt16(lastOvertimeConfig.Rank) + 1;
                }

                //pupulate rule list
                List<SelectListItem> ruleList = PopulateRules();

                model.RuleList = ruleList;

                return View(model);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public JsonResult Create(OvertimeConfigurationViewModel model)
        {
            int result = 0;
            string message = "";
            try
            {
                var entity = Mapper.Map<OvertimeConfigurationViewModel, OvertimeConfiguration>(model);
                var response = overtimeConfigurationService.Create(entity);
                result = 1;
                message = "Data Saved Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int overtimeConfigId = id.Value;
                try
                {
                    var overtimeConfig = overtimeConfigurationService.GetById(overtimeConfigId);
                    int prevRank = Convert.ToInt16(overtimeConfig.Rank) - 1;
                    int laterRank = Convert.ToInt16(overtimeConfig.Rank) + 1;

                    var prevOvertimeConfig = overtimeConfigurationService.GetByRank(prevRank);
                    var leterOvertimeConfig = overtimeConfigurationService.GetByRank(laterRank);

                    //var prevOvertimeConfig = overtimeConfigurationService.GetAll().Where(b => b.Rank == prevRank).FirstOrDefault();
                    //var leterOvertimeConfig = overtimeConfigurationService.GetAll().Where(b => b.Rank == laterRank).FirstOrDefault();

                    var model = Mapper.Map<OvertimeConfiguration, OvertimeConfigurationViewModel>(overtimeConfig);
                   
                    //pupulate rule list
                    List<SelectListItem> ruleList = PopulateRules();
                    model.RuleList = ruleList;

                    model.PrevHourTo=(prevOvertimeConfig != null) ? prevOvertimeConfig.HourTo:0;
                    model.LaterHourFrom = (leterOvertimeConfig != null) ? leterOvertimeConfig.HourFrom : 0;                    

                    return View(model);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

       

        [HttpPost]
        public JsonResult Update(OvertimeConfigurationViewModel model)
        {
            int result = 0;
            string message = "";
            try
            {
                var entity = Mapper.Map<OvertimeConfigurationViewModel, OvertimeConfiguration>(model);
                overtimeConfigurationService.Update(entity);
                result = 1;
                message = "Data Updated Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int OvertimeConfigId = id.Value;

                try
                {
                    overtimeConfigurationService.Delete(OvertimeConfigId);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }      

        #endregion

        #region Private Methods

        private static List<SelectListItem> PopulateRules()
        {
            var ruleList = new List<SelectListItem>();
            ruleList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            ruleList.Add(new SelectListItem { Text = "Fixed", Value = "1" });
            ruleList.Add(new SelectListItem { Text = "Gross", Value = "2" });
            ruleList.Add(new SelectListItem { Text = "basic", Value = "3" });
            return ruleList;
        }

        #endregion
    }
}