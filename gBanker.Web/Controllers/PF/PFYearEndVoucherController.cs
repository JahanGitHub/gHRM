#region Usings

using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Service.StoreProcedure;
using System.Globalization;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFYearEndVoucherController : BaseController
    {
        #region Private Members

        private readonly IYearEndVoucherService yearEndVoucherService;
        private readonly IEmployeeSPService employeeSPService;

        #endregion

        #region Ctor

        public PFYearEndVoucherController(IYearEndVoucherService yearEndVoucherService,
            IEmployeeSPService employeeSPService)
        {
            this.yearEndVoucherService = yearEndVoucherService;
            this.employeeSPService = employeeSPService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            return View();
        } 

        #endregion

        #region Get Year End Voucher List

        public JsonResult GetYearEndVoucherList(string SerialNo, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                //get year vouchers from [gcpf.YearEndVoucher]
                var objYearEndVouchers = GetAllVouchers();

                var currentPageRecords = objYearEndVouchers.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objYearEndVouchers.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        } 

        #endregion

        #region Private Methods

        private List<YearEndVoucherViewModel> GetAllVouchers()
        {
            var param = new
            {
                //TransactionDate = Convert.ToDateTime(model.TransactionDate),
                //CreateUser = model.CreateUser,
                //CreateDate = model.CreateDate
            };
            //get all year end vouchers from [gcpf.YearEndVoucher]
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetAllYearEndVouchers");
            var objVouchers = val.Tables[0].AsEnumerable();

            var voucher = new YearEndVoucherViewModel();
            var vouchers = new List<YearEndVoucherViewModel>();
            foreach (DataRow row in objVouchers)
            {
                voucher = new YearEndVoucherViewModel();
                voucher.SerialNo = Convert.ToInt32(row["SerialNo"]);
                voucher.TransactionDate = Convert.ToDateTime(row["TransactionDate"]).ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
                voucher.VoucherNo = Convert.ToInt32(row["VoucherNo"]);

                voucher.AccountCode = Convert.ToString(row["AccountCode"]);
                voucher.AccountName = Convert.ToString(row["AccountName"]);
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
