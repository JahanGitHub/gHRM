using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace gHRM.Web.Controllers
{
    public class NoticePayConfigController : BaseController
    {
        private readonly INoticePayConfigService _NoticePayConfigService;

        public NoticePayConfigController(INoticePayConfigService _NoticePayConfigService)
        {
            this._NoticePayConfigService = _NoticePayConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult Save()
        {
            try
            {
                string Message = "";
                NoticePayConfig Config = new JavaScriptSerializer().Deserialize<NoticePayConfig>(Request.Form["Data"].ToString());
                if (!_NoticePayConfigService.AddNPConfig(Config, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public JsonResult LoadNoticePayConfigList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                   var _List = DB.NoticePayConfigs.Where(x => x.IsActive).Select(x => new
                    {
                        x.Id,
                        x.NoticePeriod,
                        CalcFrom = x.IsCalcFromBasic ? "Basic Salary" : "Gross Salary",
                        x.SalaryPer,
                        x.EffectiveStartDate,
                        x.EffectiveEndDate
                    }).OrderByDescending(x => x.EffectiveStartDate).ToList();
                    var DataList = _List.Select(x => new
                    {
                        x.Id,
                        NoticePeriod = x.NoticePeriod + " Days",
                        x.CalcFrom,
                        x.SalaryPer,
                        EffectiveStartDate = x.EffectiveStartDate.ToString("dd-MMM-yyyy"),
                        EffectiveEndDate = null == x.EffectiveEndDate ? "" : x.EffectiveEndDate.Value.ToString("dd-MMM-yyyy")
                    });
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
        public JsonResult Delete()
        {
            try
            {
                string Message = "";
                int Id = Convert.ToInt32(Request.Form["Id"]);
                if (!_NoticePayConfigService.DeleteNoticePayConfig(Id, out Message)) return GetErrorMessageResult(Message);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}
