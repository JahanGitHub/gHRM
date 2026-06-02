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
    public class TrainingAreaController : BaseController
    {
        private readonly ITrainingAreaService _TrainingAreaService;

        public TrainingAreaController(ITrainingAreaService _TrainingAreaService)
        {
            this._TrainingAreaService = _TrainingAreaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadTrainingAreaList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = DB.TrainingAreas.Where(x => x.IsActive).Select(x => new
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
                TrainingArea _TrainingArea = new JavaScriptSerializer().Deserialize<TrainingArea>(Request.Form["Data"].ToString());
                if (!_TrainingAreaService.Save(_TrainingArea, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
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
                _TrainingAreaService.DeleteTrainingArea(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}
