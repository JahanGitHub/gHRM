#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service;
using gHRM.Service.WelfareFund;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Employee;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeTranningDropDownController : BaseController
    {
        #region Private Variables      

        private readonly IEmployeeTranningDropDownService EmployeeTranningDropDownService;

        #endregion

        #region Ctor
        public EmployeeTranningDropDownController(
            IEmployeeTranningDropDownService EmployeeTranningDropDownService
            )
        {
            this.EmployeeTranningDropDownService = EmployeeTranningDropDownService;
        }

        #endregion

        #region Listing

        public ActionResult Index()
        {
            var model = new EmployeeTranningDropDownViewModel { };
            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult GetEmployeeTranningDropDown(int id)
        {
            var single = EmployeeTranningDropDownService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeTranningDropDownListing([DataSourceRequest]DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            var listing = EmployeeTranningDropDownService.GetAll();
            DataSourceResult result = listing.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddNew(EmployeeTranningDropDownViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var newEmployeeTranningDropDownViewModel =
                             Mapper.Map<EmployeeTranningDropDownViewModel, EmployeeTranningDropDown>(model);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newEmployeeTranningDropDownViewModel.CreateBy = createdBy;
            newEmployeeTranningDropDownViewModel.IsActive = true;
            var response = EmployeeTranningDropDownService.Create(newEmployeeTranningDropDownViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(EmployeeTranningDropDownViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var newEmployeeTranningDropDownViewModel =
                             Mapper.Map<EmployeeTranningDropDownViewModel, EmployeeTranningDropDown>(model);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newEmployeeTranningDropDownViewModel.UpdateBy = updatedBy;

            var response = EmployeeTranningDropDownService.Update(newEmployeeTranningDropDownViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var EmployeeTranningDropDown = EmployeeTranningDropDownService.GetById(id);

            if (EmployeeTranningDropDown == null)
                return Json(new { type = "success", message = "Warning, Employee Tranning DropDown not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            EmployeeTranningDropDown.UpdateBy = updatedBy;

            var response = EmployeeTranningDropDownService.Delete(EmployeeTranningDropDown);

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
