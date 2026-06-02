
#region Usings

using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFProcessLogController : BaseController
    {
        #region Private Variables

        private readonly IProcessLogService processLogService;
        private IOrganizationSetupService orgSetupService;

        #endregion

        #region Ctor

        public PFProcessLogController(IProcessLogService processLogService, IOrganizationSetupService orgSetupService)
        {
            this.processLogService = processLogService;
            this.orgSetupService = orgSetupService;
        }

        #endregion

        #region Listings

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetProcessLogList(string ProcessLogId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                List<ProcessLog> List_PFType = new List<ProcessLog>();
                var objProcessLogs = processLogService.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.StartDate);

                var List_ViewModel = objProcessLogs.AsEnumerable()
                .Select(row => new ProcessLogViewModel
                {
                    ProcessLogId = row.ProcessLogId.ToString(),
                    StartDate = Convert.ToDateTime(row.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    IsOpen = row.IsOpen,
                    //EndDate = row.EndDate == null ? string.Empty : Convert.ToDateTime(row.EndDate).ToString("dd-MMM-yyyy"),
                    SystemDateAtDayStart = row.SystemDateAtDayStart == null ? string.Empty : Convert.ToDateTime(row.SystemDateAtDayStart).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    SystemDateAtDayEnd = row.SystemDateAtDayEnd == null ? string.Empty : Convert.ToDateTime(row.SystemDateAtDayEnd).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)

                }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        #endregion

        #region Create
        public ActionResult Create()
        {
            var model = new ProcessLogViewModel();

            //get process log details from [gcpf.ProcessLog]
            var objProcessLog = processLogService.GetDayStatus();
            if (objProcessLog != null)
            {
                model.TransactionDate = objProcessLog.TransactionDateString;
                model.IsOpen = objProcessLog.IsOpen;
                model.DayStatus = objProcessLog.DayStatus;
            }

            return View(model);
        }

        public JsonResult SaveProcessLog(string startDate)
        {

            ProcessLog objProcessLog = new ProcessLog();
            string dayStatus = string.Empty;
            string transactionDate = string.Empty;

            try
            {
                string message = IsValidDayStartDate(Convert.ToDateTime(startDate));
                if (!string.IsNullOrEmpty(message))
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                objProcessLog.StartDate = Convert.ToDateTime(startDate).Date;
                objProcessLog.SystemDateAtDayStart = DateTime.Now;
                objProcessLog.SystemDateAtDayEnd = null;
                objProcessLog.IsOpen = true;

                objProcessLog.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objProcessLog.CreateDate = DateTime.Now;

                //let's insert into [gcpf.ProcessLog]
                var processLog = processLogService.Create(objProcessLog);

                //Asad Added
                var model = new ProcessLogViewModel();
               
                var objPLog = processLogService.GetDayStatus();
                if (objPLog != null)
                {                    
                    transactionDate = objPLog.TransactionDateString;                   
                    dayStatus = objPLog.DayStatus;
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", status = "nok", DayStatus = dayStatus, TransactionDate = transactionDate }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved Successfully", status = "ok", DayStatus = dayStatus, TransactionDate = transactionDate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Is Valid Start Date

        public JsonResult IsValidStartDate(string startdate)
        {
            string message = string.Empty;
            try
            {
                message = IsValidDayStartDate(Convert.ToDateTime(startdate));
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update ProcessLog

        public JsonResult UpdateProcessLog(string processLogId, string startDate, bool isOpen)
        {
            try
            {
                ProcessLog objProcessLog = processLogService.GetById(Convert.ToInt32(processLogId));
                if (objProcessLog == null)
                    return Json(new { message = "Record does not exist" }, JsonRequestBehavior.AllowGet);

                objProcessLog.ProcessLogId = Convert.ToInt32(processLogId);
                objProcessLog.StartDate = Convert.ToDateTime(startDate).Date;
                objProcessLog.IsOpen = isOpen;

                objProcessLog.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objProcessLog.UpdateDate = DateTime.Now;
                processLogService.Update(objProcessLog);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        } 

        #endregion

        #region Private Methods

        private string IsValidDayStartDate(DateTime startDate)
        {

            string result = string.Empty;
            var model = new ProcessLogViewModel();

            var objProcessLog = processLogService.GetLastProcessLog();
            var objOrgSetup = orgSetupService.GetAll().FirstOrDefault(x => x.IsDeleted == false && x.IsActive == true);

            if (objOrgSetup == null)
                result = "Setup Organization first then try to open day";

            if (objOrgSetup != null)
            {
                if (!(startDate >= objOrgSetup.YearStartDate && startDate <= objOrgSetup.YearEndDate))
                    result = "Choose year between " + objOrgSetup.YearStartDate.ToString("dd/MMM/yyyy") + " and " + objOrgSetup.YearEndDate.ToString("dd/MMM/yyyy");
            }

            if (objProcessLog != null && objOrgSetup != null)
            {
                if (!(startDate >= objOrgSetup.YearStartDate && startDate <= objOrgSetup.YearEndDate))
                    result = "Choose year between " + objOrgSetup.YearStartDate.ToString("dd/MMM/yyyy") + " and " + objOrgSetup.YearEndDate.ToString("dd/MMM/yyyy");

                if (objProcessLog.IsOpen == true && objProcessLog.StartDate == startDate)
                    result = "Already opened";

                if (!objProcessLog.IsOpen == true && objProcessLog.StartDate == startDate)
                    result = "Already closed";

                if (startDate < objProcessLog.StartDate)
                    result = "You have already passed " + startDate.ToString("dd/MMM/yyyy") + ", please initialize valid day";

                if (startDate > DateTime.Now)
                    result = "You can not exceed current day that is; " + DateTime.Now.ToString("dd/MMM/yyyy");
            }
            return result;

        }

        #endregion
    }
}
