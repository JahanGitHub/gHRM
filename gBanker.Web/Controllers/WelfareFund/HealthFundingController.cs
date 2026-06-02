#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service.StoreProcedure;
using gHRM.Service.WelfareFund;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundConfiguration;
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
    public class HealthFundingController : BaseController
    {
        #region Private Variables
        private readonly IHealthFundingService HealthFundingService;
        private readonly IEmployeeSPService employeeService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        #endregion

        #region Ctor
        public HealthFundingController(
            IHealthFundingService HealthFundingService,
            IEmployeeSPService employeeService
            )
        {
            this.HealthFundingService = HealthFundingService;
            this.employeeService = employeeService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion


        #region Listing

        public JsonResult GetHealthFundingListing([DataSourceRequest] DataSourceRequest request,
                  string searchTerm, string FilterColumn, string FilterValue)
        {
            try
            {
                List<HealthFundingViewModel> List_ViewModel = new List<HealthFundingViewModel>();

                var List = employeeService.GetDataWithoutParameter("SP_GET_Fund_List_Employee");

                List_ViewModel = List.Tables[0].AsEnumerable()
               .Select(row => new HealthFundingViewModel()
               {
                   Id = row.Field<Int32>("Id"),
                   EmployeeId = row.Field<long>("EmployeeId"),
                   PurposeId = row.Field<int>("PurposeId"),
                   FundAmount = row.Field<decimal>("FundAmount"),
                   remarks = row.Field<string>("remarks"),
                   EmpInfo = row.Field<string>("EmpInfo"),
                   purposename = row.Field<string>("purposename"),
                   CreateDateString = row.Field<string>("CreateDateString"),
                   IsActive = row.Field<bool>("IsActive"),

               }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }



        }



        public JsonResult GetPRpurposeListFund()
        {
            List<purposeListFund> List_ViewModel = new List<purposeListFund>();

            var empList = employeeService.GetDataWithoutParameter("sp_get_purposelistfund");
            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new purposeListFund
               {
                   Value = row.Field<int>("id"),
                   Text = row.Field<string>("purposename")
               }).ToList();

            return Json(List_ViewModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index()
        {
            var model = new HealthFundingViewModel();
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region Ajax Calls      

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var healthfund = HealthFundingService.GetById(id);

            if (healthfund == null)
                return Json(new { type = "success", message = "Warning,  Health Funding not found!" },
                             JsonRequestBehavior.AllowGet);

            healthfund.IsActive = false;

            var response = HealthFundingService.Delete(healthfund);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Configure(HealthFundingViewModel model)
        {
            try
            {
                long createUser = Convert.ToInt64( SessionHelper.LoggedInEmployeeID);

                var newHealthFunding =
                               Mapper.Map<HealthFundingViewModel, HealthFunding>(model);

                int createdBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                newHealthFunding.CreateUser = createdBy;
                newHealthFunding.IsActive = true;

                var response = HealthFundingService.Create(newHealthFunding);

                if (!response.IsSuccess)
                    return Json(new { type = "warning", message = response.Message },
                                 JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = response.Message },
                                  JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return Json(new { type = "error", message = ex.Message },
                           JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region Private Methods

        private void MapDropDownList(HealthFundingViewModel model)
        {
           
            var componentList = new List<SelectListItem>();
            componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

            model.PurposeList = componentList;


        }



        #endregion
    }
}
