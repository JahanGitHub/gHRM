
#region Usings

using AutoMapper;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Service.WelfareFund;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.WelfareFund
{
    public class FundSetupController : BaseController
    {
        #region Private Variables      
        private readonly IEmployeeSPService employeeService;
        private readonly IFundSetupService fundSetupService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        #endregion

        #region Ctor
        public FundSetupController(
            IFundSetupService fundSetupService,
            IEmployeeSPService employeeService
            )
        {
            this.fundSetupService = fundSetupService;
            this.employeeService = employeeService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Listing


        public ActionResult Index()
        {
            var model = new FundSetupViewModel { };
            MapDropDownList(model);
            return View(model);
        }

        private void MapDropDownList(FundSetupViewModel model)
        {
            var staffWelfareFundSettingList = new List<SelectListItem>();
            staffWelfareFundSettingList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            staffWelfareFundSettingList.Add(new SelectListItem() { Text = "Health", Value = "Health" });
            staffWelfareFundSettingList.Add(new SelectListItem() { Text = "Walfare", Value = "Walfare" });
            
            model.StaffWelfareFundSettingList = staffWelfareFundSettingList;

            model.RatioBasedList = commonStaticDropDown.ddlSalaryRatio();

            model.ComponentTypeList = commonStaticDropDown.SalaryCalculationType();

            model.ComponentList = commonStaticDropDown.ddlInitial();

            var componentList = new List<SelectListItem>();
            componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
            model.ComponentCategoryList = componentList;
        }


        #endregion

        #region Ajax Calls

        public JsonResult GetStaffWelfareFundSetting(int id)
        {
            var single = fundSetupService.GetById(id);
            if(single==null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStaffWelfareFundSettingListing([DataSourceRequest]DataSourceRequest request, 
             string searchTerm,string FilterColumn, string FilterValue)
        {
            try
            {
                List<FundSetupViewModel> List_ViewModel = new List<FundSetupViewModel>();        

                var List = employeeService.GetDataWithoutParameter("SP_GET_Fund_List");


                 List_ViewModel = List.Tables[0].AsEnumerable()
                .Select(row => new FundSetupViewModel()
                {
                    Id = row.Field<Int32>("Id"),
                    FundType = row.Field<string>("FundType"),
                    ComponentType = row.Field<string>("ComponentType"),
                    ComponentAmount = row.Field<decimal>("ComponentAmount"),
                    RatioBasedOn = row.Field<string>("RatioBasedOn"),
                    ComponentName = row.Field<string>("ComponentName"),
                    IsActive = row.Field<bool>("IsActive"),
                    CreateUserName = row.Field<string>("CreateUser"),
                    CreateDateString = row.Field<string>("CreateDate")

                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }



        }

        [HttpPost]
        public JsonResult AddNew(FundSetupViewModel model)
        {
            if(!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var alreadyExist = fundSetupService.GetAll().Where(u => u.IsActive && u.FundType == model.FundType && u.PRComponentId == model.PRComponentId);

            if (!alreadyExist.Any())
            {

                var newFundSetup =
                                 Mapper.Map<FundSetupViewModel, FundSetup>(model);

                int createdBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                newFundSetup.CreateUser = createdBy;
                newFundSetup.IsActive = true;

                var response = fundSetupService.Create(newFundSetup);

                if (!response.IsSuccess)
                    return Json(new { type = "warning", message = response.Message },
                                 JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = response.Message },
                                  JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { type = "warning", message = "Fund Type and Component Already Active , Please Inactive First !" },
                                JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var staffWelfareFundSetting = fundSetupService.GetById(id);

            if (staffWelfareFundSetting == null)
                return Json(new { type = "success", message = "Warning,  Fund Setting not found!" },
                             JsonRequestBehavior.AllowGet);

            staffWelfareFundSetting.IsActive = false;   // newww 

            var response = fundSetupService.Delete(staffWelfareFundSetting);

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
