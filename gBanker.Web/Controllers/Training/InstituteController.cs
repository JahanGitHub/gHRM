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
    public class InstituteController : BaseController
    {
        private readonly IInstituteService _InstituteService;

        public InstituteController(IInstituteService _InstituteService)
        {
            this._InstituteService = _InstituteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadInstituteList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = DB.Institutes.Where(x => x.IsActive).Select(x => new
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
                Institute _Institute = new JavaScriptSerializer().Deserialize<Institute>(Request.Form["Data"].ToString());
                if (!_InstituteService.Save(_Institute, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
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
                _InstituteService.DeleteInstitute(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}
