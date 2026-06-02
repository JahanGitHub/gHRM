using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace gHRM.Web.Controllers
{
    public class ResignNoticeController : BaseController
    {
        private readonly IResignNoticeService _ResignNoticeService;

        public ResignNoticeController(IResignNoticeService _ResignNoticeService)
        {
            this._ResignNoticeService = _ResignNoticeService;
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
            string Message = "";
            try
            {
                ResignNotice _Data = new JavaScriptSerializer().Deserialize<ResignNotice>(Request.Form["Data"].ToString());
                if (!IsSaveValid(_Data, out Message)) return GetErrorMessageResult(Message);
                _Data.IsActive = true;
                _Data.CreateUser = LoggedInEmployeeId ?? 0;
                _Data.CreateDate = DateTime.Now;
                _ResignNoticeService.Create(_Data);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public JsonResult LoadResignNoticeList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var _List = (from RN in DB.ResignNotices
                                 join E in DB.Employees on RN.EmployeeId equals E.EmployeeId
                                 where RN.IsActive
                                 orderby RN.CreateDate descending
                                 select new
                                 {
                                     Id = RN.Id,
                                     E.EmployeeCode,
                                     E.EmployeeName,
                                     RN.InformDate,
                                     RN.ResignDate,
                                     RN.Remark
                                 }).ToList();
                    var DataList = _List.Select(x => new
                    {
                        Id = x.Id,
                        x.EmployeeCode,
                        x.EmployeeName,
                        InformDate = x.InformDate.ToString("dd-MMM-yyyy"),
                        ResignDate = x.ResignDate.ToString("dd-MMM-yyyy"),
                        x.Remark
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
                int Id = Convert.ToInt32(Request.Form["Id"]);
                _ResignNoticeService.DeleteResignNotice(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        private bool IsSaveValid(ResignNotice _Data, out string Message)
        {
            Message = "";
            if (_Data.EmployeeId == 0)
            {
                Message = "Employee is required";
                return false;
            }
            if (_ResignNoticeService.HasDuplicate(_Data.EmployeeId))
            {
                Message = "Duplicate Resign Notice found with this Employee";
                return false;
            }
            return true;
        }
    }
}
