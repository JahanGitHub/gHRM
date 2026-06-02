using AutoMapper;
using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Service.TaDa;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.TaDa;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.TaDa
{
    public class TADAPurposeController : Controller
    {
        private readonly ITADAPurposeService tADAPurposeService;

        public TADAPurposeController(ITADAPurposeService tADAPurposeService)
        {
            this.tADAPurposeService = tADAPurposeService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new TADAPurposeViewModel { };
            return View(model);
        }
        [HttpPost]
        public JsonResult AddTADAPurpose(TADAPurposeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "warning", message = "You must fill all the asteric(*) required fields." },
                            JsonRequestBehavior.AllowGet);

            var tadaPurposeViewModel =
                             Mapper.Map<TADAPurposeViewModel, TADAPurpose>(model);

            var validationResponse = tADAPurposeService
                                        .IsValidTADAPurpose(tadaPurposeViewModel);

            if (!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            tadaPurposeViewModel.CreateUser = createdBy;
            tadaPurposeViewModel.IsActive = true;

            var response = tADAPurposeService.Create(tadaPurposeViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTADAPurposeListing([DataSourceRequest]DataSourceRequest request,
            string searchTerm, string FilterColumn, string FilterValue)
        {            
            var tadaPurposeList = tADAPurposeService.GetAll().Where(p=> p.IsActive == true);
            DataSourceResult result = tadaPurposeList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTADAPurposeList(int id)
        {
            var single = tADAPurposeService.GetById(id);
            if (single == null)
                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);

            return Json(new { data = single, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateTADAPurpose(TADAPurposeViewModel model)
        {
            var newTADAPurposeViewModel =
                              Mapper.Map<TADAPurposeViewModel, TADAPurpose>(model);

            var validationResponse = tADAPurposeService
                                       .IsValidTADAPurpose(newTADAPurposeViewModel);

            if (!validationResponse.IsSuccess)
                return Json(new { type = "warning", message = validationResponse.Message },
                           JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            newTADAPurposeViewModel.UpdateUser = updatedBy;
            newTADAPurposeViewModel.IsActive = true;

            var response = tADAPurposeService.Update(newTADAPurposeViewModel);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            var tADAPurpose = tADAPurposeService.GetById(id);

            if (tADAPurpose == null)
                return Json(new { type = "success", message = "Warning, TADA Purpose not found!" },
                             JsonRequestBehavior.AllowGet);

            long updatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            tADAPurpose.UpdateUser = updatedBy;

            var response = tADAPurposeService.Delete(tADAPurpose);

            if (!response.IsSuccess)
                return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

            return Json(new { type = "success", message = response.Message },
                              JsonRequestBehavior.AllowGet);
        }
    }
}