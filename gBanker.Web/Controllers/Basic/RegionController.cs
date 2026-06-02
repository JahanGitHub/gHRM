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

namespace gHRM.Web.Controllers.Basic
{
    public class RegionController : BaseController
    {
        private readonly IOfficeRegionService _OfficeRegionService;
        private readonly IOfficeService _OfficeService;

        public RegionController(IOfficeRegionService _OfficeRegionService, IOfficeService _OfficeService)
        {
            this._OfficeRegionService = _OfficeRegionService;
            this._OfficeService = _OfficeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapOffice(int Id)
        {
            ViewData["Name"] = _OfficeRegionService.GetNameById(Id);
            ViewBag.Id = Id;
            ViewBag.ZonalOfficeList = _OfficeService.GetAllZonalOfficeList();
            return View();
        }

        public JsonResult LoadRegionList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = DB.OfficeRegions.Where(x => x.IsActive).Select(x => new
                    {
                        x.Id,
                        x.Name
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

        public JsonResult LoadRegionOfficeList([DataSourceRequest] DataSourceRequest request, int RegionId)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = (from M in DB.OfficeRegionMappings
                                    join O in DB.Offices on M.OfficeId equals O.OfficeId
                                    where M.IsActive && M.RegionId == RegionId
                                    orderby O.OfficeName
                                    select new { M.Id, Name = O.OfficeName }).ToList();
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
                OfficeRegion _Region = new JavaScriptSerializer().Deserialize<OfficeRegion>(Request.Form["Data"].ToString());
                if (!_OfficeRegionService.Save(_Region, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
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
                _OfficeRegionService.DeleteRegion(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult SaveMapOffice()
        {
            try
            {
                string Message = "";
                OfficeRegionMapping _RegionMap = new JavaScriptSerializer().Deserialize<OfficeRegionMapping>(Request.Form["Data"].ToString());
                if (!_OfficeRegionService.SaveMapOffice(_RegionMap, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult DeleteMapOffice()
        {
            try
            {
                int Id = Convert.ToInt32(Request.Form["Id"]);
                _OfficeRegionService.DeleteMapOffice(Id);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}