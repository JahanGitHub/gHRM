#region Usings
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Service.StoreProcedure; 
#endregion

namespace gHRM.Web.Controllers
{
    public class PFVoucherProcessController : BaseController
    {
        #region Private Variables
        private readonly IProcessLogService processLogService;
        private readonly IOrganizationSetupService orgSetupService;
        private readonly IEmployeeSPService employeeSPService;
        #endregion

        #region Ctor
        public PFVoucherProcessController(IProcessLogService processLogService, IOrganizationSetupService orgSetupService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.orgSetupService = orgSetupService;
            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Create

        public ActionResult Create()
        {
            var model = new VoucherProcessViewModel();

            try
            {
                var objProcessLog = processLogService.GetCustomDayStatus();
                model.TransactionDate = objProcessLog.TransactionDateString;
                model.IsOpen = objProcessLog.IsOpen;
                model.DayStatus = objProcessLog.DayStatus;
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        #endregion

        #region Process Voucher
        public JsonResult ProcessVoucher(string transDate)
        {
            var model = new YearEndProcessViewModel();

            try
            {
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                    return Json(new { message = "Please check Day End process log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day Clossed, please Open day and then start voucher processing" }, JsonRequestBehavior.AllowGet);

                if (processLog.StartDate.Date != Convert.ToDateTime(transDate))
                    return Json(new { message = "Submitted transaction date is invalid" }, JsonRequestBehavior.AllowGet);

                int totalTransaction = GetTotalTransaction(processLog.StartDate);
                if (totalTransaction <= 0)
                    return Json(new { message = "No transaction has been occured today." }, JsonRequestBehavior.AllowGet);
                
                model.TransactionDate = processLog.StartDate.ToString();
                model.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                model.CreateDate = DateTime.Now;
                Process(model);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Processed Successfully" }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        public JsonResult GetVoucherList(string SerialNo, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                var objVouchers = GetAllVouchers();

                var currentPageRecords = objVouchers.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objVouchers.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        #region VerifyVoucher

        public JsonResult VerifyVoucher(string transDate)
        {
            string message = string.Empty;

            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);
                if (!processLog.IsOpen)
                    return Json(new { message = "Day Clossed" }, JsonRequestBehavior.AllowGet);

                //Validation-2: Voucher verification
                message = VerifyVoucher(processLog.StartDate);
                if (!string.IsNullOrEmpty(message))
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Verified Successfully" }, JsonRequestBehavior.AllowGet);
        } 

        #endregion

        #region Private Methods

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

        private void Process(YearEndProcessViewModel model)
        {
            var param = new
            {
                TransactionDate = Convert.ToDateTime(model.TransactionDate),
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate,
                LoggedInOfficeID = LoggedInOfficeID
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_DI_ProcessPrepareVoucher");
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

        private List<VoucherViewModel> GetAllVouchers()
        {
            var param = new
            {             
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetAllVouchers");
            var objVouchers = val.Tables[0].AsEnumerable();

            var voucher = new VoucherViewModel();
            var vouchers = new List<VoucherViewModel>();
            foreach (DataRow row in objVouchers)
            {
                voucher = new VoucherViewModel();
                voucher.SerialNo = Convert.ToInt32(row["SerialNo"]);
                voucher.TransactionDate = Convert.ToDateTime(row["TransactionDate"]).ToString("dd/MMM/yyyy");
                voucher.VoucherNo = Convert.ToInt32(row["VoucherNo"]);

                voucher.AccountCode = Convert.ToString(row["AccountCode"]);
                voucher.Dr = Convert.ToInt64(row["Dr"]);
                voucher.Cr = Convert.ToInt64(row["Cr"]);
                voucher.TransactionType = Convert.ToString(row["TransactionType"]);
                voucher.Particulars = Convert.ToString(row["Particulars"]);
                vouchers.Add(voucher);
            }
            return vouchers;
        }
        #endregion
    }
}
