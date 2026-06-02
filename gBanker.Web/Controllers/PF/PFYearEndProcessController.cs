#region Usings

using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.PF;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFYearEndProcessController : BaseController
    {
        #region Private Members

        private readonly IProcessLogService processLogService;
        private readonly IYearEndProcessLogService yearEndProcessLogService;
        private readonly IOrganizationSetupService orgSetupService;
        private readonly IEmployeeSPService employeeSPService;

        #endregion

        #region Ctor

        public PFYearEndProcessController(IProcessLogService processLogService, IYearEndProcessLogService yearEndProcessLogService, IOrganizationSetupService orgSetupService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.yearEndProcessLogService = yearEndProcessLogService;
            this.orgSetupService = orgSetupService;
            this.employeeSPService = employeeSPService;
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            var model = new YearEndProcessViewModel();
            var processLog = new ProcessLog();
            string message = string.Empty;

            var objOrgSetup = orgSetupService.GetMany(x => x.IsDeleted == false && x.IsActive == true).FirstOrDefault();

            if (objOrgSetup == null)
            {
                model.IsValidYearEnd = false;
                model.DayStatus = model.YearEndStatus = "Setup Organization first";
                return View(model);
            }

            processLog = processLogService.GetMany(x => x.IsDeleted == false 
                    && (x.StartDate >= objOrgSetup.YearStartDate && x.StartDate <= objOrgSetup.YearEndDate))
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();

            if (processLog == null)
            {
                model.TransactionDate = string.Empty; ;
                model.IsOpen = false;
                model.DayStatus = "No day opened";
            }
            else
            {
                model.TransactionDate = processLog.StartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                model.IsOpen = processLog.IsOpen;
                if (processLog.IsOpen)
                    model.DayStatus = "Day is open";
                else
                    model.DayStatus = "Day is close";
            }

            model.YearStartDate = objOrgSetup.YearStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.YearEndDate = objOrgSetup.YearEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            if (processLog == null)
            {
                model.IsValidYearEnd = false;
                model.YearEndStatus = "Year End is not possible";
                return View(model);
            }

            if (processLog.StartDate.Date != objOrgSetup.YearEndDate.Date)
            {
                model.IsValidYearEnd = false;
                model.YearEndStatus = "Transaction Date and Year End date is differtent";
                return View(model);
            }
                        
            bool isValid = yearEndProcessLogService.IsValidYearForEnding(objOrgSetup.YearStartDate, objOrgSetup.YearEndDate, out message);

            if (!isValid)
            {
                model.IsValidYearEnd = false;
                model.YearEndStatus = message;
                return View(model);
            }

            model.IsValidYearEnd = true;
            model.YearEndStatus = "Checking successfull, you can close year";
            return View(model);
        }
        #endregion

        #region Process Year End

        public JsonResult ProcessYearEnd(string yearStartDate, string yearEndDate, string transDate)
        {
            YearEndProcessViewModel model = new YearEndProcessViewModel();
            string message = string.Empty;
            try
            {
                DateTime yStartDate = Convert.ToDateTime(yearStartDate);
                DateTime yEndDate = Convert.ToDateTime(yearEndDate);

                var isvalid = yearEndProcessLogService.IsValidYearForEnding(yStartDate, yEndDate, out message);
                if (!isvalid)
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                isvalid = false; message = string.Empty;
                isvalid = yearEndProcessLogService.IsProfitDistributed(yStartDate, yEndDate, out message);
                if (!isvalid)
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                model.YearStartDate = yearStartDate;
                model.YearEndDate = yearEndDate;
                model.TransactionDate = yearEndDate;
                model.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                model.CreateDate = DateTime.Now;
                ProcessYear(model);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", status = "nok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Completed Successfully", status = "ok" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private void ProcessYear(YearEndProcessViewModel model)
        {
            var param = new
            {
                TransactionDate = Convert.ToDateTime(model.TransactionDate),
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_YE_ProcessYearEnd");
        }

        #endregion
    }
}
