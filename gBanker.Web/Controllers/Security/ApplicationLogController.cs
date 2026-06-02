using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class ApplicationLogController : BaseController
    {
        private readonly IApplicationLogService applicationLogService;
        public ApplicationLogController(IApplicationLogService applicationLogService)
        {
            this.applicationLogService = applicationLogService;
        }
        //
        // GET: /ApplicationLog/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var log = applicationLogService.GetById(id);
            return View(log);
        }

        public JsonResult GetLogRecords(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                DateTime dt;
                if (filterColumn == "CreateDate" && !DateTime.TryParse(filterValue, out dt))
                    throw new Exception("Please enter a valid date for Date filter.");
                long totalCount;
                //TODO: PASS ORGANIZTION ID WHEN AVALIABLE
                var allloansummary = applicationLogService.GetApplicationLogPaged("", filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount);

                return Json(new { Result = "OK", Records = allloansummary, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}