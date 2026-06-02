using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using System.Web.Script.Serialization;
using gHRM.Service;

namespace gHRM.Web.Controllers
{
    public class TrainingTypeController : BaseController
    {
        private readonly ITrainingTypeService _TrainingTypeService;

        public TrainingTypeController(ITrainingTypeService _TrainingTypeService)
        {
            this._TrainingTypeService = _TrainingTypeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadTrainingTypeList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = DB.TrainingTypes.Where(x => x.IsActive).Select(x => new
                    {
                        x.Id,
                        Name = x.Name
                    }).OrderBy(x => x.Name).ToList();
                    DataSourceResult result = DataList.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult Save()
        {
            try
            {
                string Message = "";
                TrainingType _TrainingType = new JavaScriptSerializer().Deserialize<TrainingType>(Request.Form["Data"].ToString());
                if (!_TrainingTypeService.Save(_TrainingType, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult Delete()
        {
            try
            {
                int Id = Convert.ToInt32(Request.Form["Id"]);
                _TrainingTypeService.DeleteTrainingType(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}
