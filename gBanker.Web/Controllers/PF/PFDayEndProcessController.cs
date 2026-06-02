using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gHRM.Service.StoreProcedure;
namespace gHRM.Web.Controllers
{
    public class PFDayEndProcessController : BaseController
    {

        private readonly IProcessLogService processLogService;
        private readonly IEmployeeSPService employeeSPService;

        public PFDayEndProcessController(IProcessLogService processLogService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.employeeSPService = employeeSPService;
        }

        public JsonResult ProcessDayEnd(string transDate, string systemDate)
        {

            DayEndProcessViewModel model = new DayEndProcessViewModel();
            string dayStatus = string.Empty;
            string transactionDate = string.Empty;
            string message = string.Empty;

            try
            {
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day Clossed" }, JsonRequestBehavior.AllowGet);

                //Validation-1: Transaction Verification (Occured or Not)
                //int totalTransaction = GetTotalTransaction(processLog.StartDate);
                //if(totalTransaction <=0)
                //    return Json(new { message = "No transaction occured, either make transaction or proceed to next transaction day" }, JsonRequestBehavior.AllowGet);

                //Validation-2: Voucher verification
                message = VerifyVoucher(processLog.StartDate);
                if(!string.IsNullOrEmpty(message))
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);
                
                model.TransactionDate = processLog.StartDate.ToString();
                model.SystemDate = DateTime.Now.ToString();
                model.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                model.CreateDate = DateTime.Now;
                ProcessDay(model);

                var objPLog = processLogService.GetDayStatus();
                if (objPLog != null)
                {
                    transactionDate = objPLog.TransactionDate.ToString("dd/MMM/yyyy");
                    dayStatus = objPLog.DayStatus;
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", status = "nok", DayStatus = dayStatus, TransactionDate = transactionDate }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Processed Successfully", status = "ok", DayStatus = dayStatus, TransactionDate = transactionDate }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTotalTransaction(string transDate)
        {
            
            string message = string.Empty;
            int totalTransaction = 0;
            try
            {
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                    return Json(new { message = "Please Check Process Log", status = "nok", TotalTransaction = totalTransaction }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day Clossed", status = "nok", TotalTransaction = totalTransaction }, JsonRequestBehavior.AllowGet);
                if(processLog.StartDate.Date != Convert.ToDateTime(transDate).Date)
                    return Json(new { message = "Provoded transaction date is not correct", status = "nok", TotalTransaction = totalTransaction }, JsonRequestBehavior.AllowGet);

                //Validation-1: Transaction Verification (Occured or Not)
                totalTransaction = GetTotalTransaction(processLog.StartDate);
                if (totalTransaction <= 0)
                    message = "No transaction occured, either make transaction or proceed to next transaction day";

            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", status = "nok", TotalTransaction = totalTransaction }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = message, status = "ok", TotalTransaction = totalTransaction }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CloseDayWithoutAccounting(string transDate)
        {
            
            string message = string.Empty;
            string dayStatus = string.Empty;
            try
            {
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day already clossed" }, JsonRequestBehavior.AllowGet);
                if(processLog.StartDate.Date != Convert.ToDateTime(transDate).Date)
                    return Json(new { message = "Provided transaction date is not correct" }, JsonRequestBehavior.AllowGet);

                processLog.IsOpen       = false;  //Day Closed
                processLog.SystemDateAtDayEnd = DateTime.Now; 
                processLog.UpdateUser   = Convert.ToInt64(LoggedInEmployeeId.ToString());
                processLog.UpdateDate   = DateTime.Now;
                processLogService.Update(processLog);
                
                if(!processLog.IsOpen)
                    dayStatus = "Close";
                else
                    dayStatus = "Open";

            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", DayStatus = dayStatus, status = "nok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Day clossed Successfully, please open next transaction day and proceed", DayStatus = dayStatus, status = "ok" }, JsonRequestBehavior.AllowGet); 
        }

        private void ProcessDay(DayEndProcessViewModel model)
        {
            var param = new
            {
                TransactionDate = Convert.ToDateTime(model.TransactionDate),
	            SystemDate = Convert.ToDateTime(model.SystemDate),
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_DE_ProcessDayEnd");
        }

        private int GetTotalTransaction(DateTime transDate)
        {
            int totalTransaction = 0;
            var param = new
            {
                TransactionDate = transDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetTotalTransaction");
            totalTransaction = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("TotalTransaction")).SingleOrDefault();

            return totalTransaction;
        }

        private string VerifyVoucher(DateTime transDate)
        {
            string message = string.Empty;
            var param = new
            {
                TransactionDate = transDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_VerifyVoucher");
            message = val.Tables[0].AsEnumerable().Select(row => row.Field<string>("Message")).SingleOrDefault();
            return message;
        }


        public ActionResult Create()
        {
            DayEndProcessViewModel model = new DayEndProcessViewModel();

            var objProcessLog = processLogService.GetDayStatus();
            if (objProcessLog != null)
            {
                model.TransactionDate = objProcessLog.TransactionDateString;
                model.IsOpen = objProcessLog.IsOpen;
                model.DayStatus = objProcessLog.DayStatus;
                model.SystemDate = DateTime.Now.ToString("dd/MMM/yyyy");
            }
            return View(model);
        }
    }
}
