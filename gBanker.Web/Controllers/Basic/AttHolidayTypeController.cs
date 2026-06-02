using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers.Basic
{
    public class AttHolidayTypeController : BaseController
    {
        #region Varibles

        private readonly IAttHolidayTypeService attHolidayTypeService;
        public AttHolidayTypeController(
            IAttHolidayTypeService attHolidayTypeService
        )
        {
            this.attHolidayTypeService = attHolidayTypeService;
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region HttpRequests 

        public JsonResult SaveAttHolidayType(AttHolidayType AttHolidayType)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    attHolidayTypeService.GetMany( p =>
                                p.IsActive == true &&
                                p.HolidayTypeFullName.ToUpper().Trim() == AttHolidayType.HolidayTypeFullName.ToUpper().Trim())
                        .ToList();

                if (isDuplicate.Any())
                {
                    result = "Duplicate HolidayType FullName found, Save denied";
                }
                else
                {
                    var entity = new AttHolidayType();
                    entity.HolidayTypeShortName = AttHolidayType.HolidayTypeShortName;
                    entity.HolidayTypeFullName = AttHolidayType.HolidayTypeFullName;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    attHolidayTypeService.Create(entity);
                    result = "Save Successfull";
                }
            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateAttHolidayType(AttHolidayType AttHolidayType)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   attHolidayTypeService.GetMany( p =>
                               p.IsActive == true && p.AttHolidayTypeId != AttHolidayType.AttHolidayTypeId &&
                               p.HolidayTypeFullName.ToUpper().Trim() == AttHolidayType.HolidayTypeFullName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate  HolidayType FullName found, Update denied";
                }
                else
                {
                    var entity = attHolidayTypeService.GetById(AttHolidayType.AttHolidayTypeId);
                    entity.AttHolidayTypeId = AttHolidayType.AttHolidayTypeId;
                    entity.HolidayTypeShortName = AttHolidayType.HolidayTypeShortName;
                    entity.HolidayTypeFullName = AttHolidayType.HolidayTypeFullName;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    attHolidayTypeService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAttHolidayType([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request)
        {
            try
            {
                var attHolidayType = attHolidayTypeService.GetMany(t => t.IsActive == true);
                var listattHolidayType = attHolidayType.AsEnumerable().Select(a => new AttHolidayType()
                {
                    AttHolidayTypeId = a.AttHolidayTypeId,
                    HolidayTypeShortName = a.HolidayTypeShortName,
                    HolidayTypeFullName = a.HolidayTypeFullName
                }).ToList();

                DataSourceResult result = listattHolidayType.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteAttHolidayType(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = attHolidayTypeService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                attHolidayTypeService.Update(model);
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
    }
}