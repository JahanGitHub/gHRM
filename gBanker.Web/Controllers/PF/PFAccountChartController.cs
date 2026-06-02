using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using gHRM.Service.PF;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class PFAccountChartController : Controller
    {

        #region Initialization

        private readonly IAccountChartService accountChartService;
        private readonly IAccountTypeService accountTypeService;
        private readonly IGLLevelService gLLevelService;
        public PFAccountChartController(IAccountChartService accountChartService, IAccountTypeService accountTypeService, IGLLevelService gLLevelService)
        {
            this.accountChartService = accountChartService;
            this.accountTypeService = accountTypeService;
            this.gLLevelService = gLLevelService;
        }

        #endregion

        #region Methods

        public JsonResult GetAccountChartList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string accountCode = "", string accountName = "", string parentAccountCode = "")
        {
            try
            {
                //var objAccountChartList = accountChartService.GetAll().Where(x => x.IsDeleted == false).ToList();
                IEnumerable<AccountChart> objAccountCharts = new List<AccountChart>();
                objAccountCharts = accountChartService.GetAll().Where(x =>
                                                                       x.AccountCode == (accountCode == string.Empty ? x.AccountCode : accountCode) &&
                                                                       x.AccountName == (accountName == string.Empty ? x.AccountName : accountName) &&
                                                                       x.ParentAccountCode == (parentAccountCode == string.Empty ? x.ParentAccountCode : parentAccountCode)
                                                                       );

                var currentPageRecords = objAccountCharts.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objAccountCharts.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveAccountChart(string accountCode, string accountName, string accountTypeCode, int glLevelId, string parentAccountCode, bool isVoucher)
        {
            AccountChart objAccountChart = new AccountChart();
            try
            {

                var accChart = accountChartService.GetAccountChartByAccountCode(accountCode);
                if (accChart != null)
                {  
                    return Json(new { status = "nok", message = "Account Code already exist" }, JsonRequestBehavior.AllowGet);
                }

                var accountChart = accountChartService.GetAccountChartByName(accountName).ToList();
                if (accountChart.Count > 0)
                {
                    return Json(new { status = "nok",  message = "Account Name already exist" }, JsonRequestBehavior.AllowGet);
                }

                objAccountChart.AccountCode = accountCode;
                objAccountChart.AccountName = accountName;
                objAccountChart.AccountTypeCode = accountTypeCode;
                objAccountChart.GLLevelId = glLevelId;
                objAccountChart.ParentAccountCode = parentAccountCode;
                objAccountChart.IsVoucher = isVoucher;
                objAccountChart.CreateUser = SessionHelper.LoggedInEmployeeID;
                objAccountChart.CreateDate = DateTime.Now;
                objAccountChart.IsDeleted = false;


                if (string.IsNullOrEmpty(parentAccountCode)) //No Parent
                {
                    accountChartService.Create(objAccountChart);
                }

                if (!string.IsNullOrEmpty(parentAccountCode))  // Have parent
                {
                    List<AccountChart> objAccountCharts = new List<AccountChart>();
                    objAccountCharts.Add(objAccountChart);

                    var acChart = accountChartService.GetAccountChartByAccountCode(parentAccountCode);
                    acChart.IsVoucher = false;
                    acChart.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    acChart.UpdateDate = DateTime.Now;
                    objAccountCharts.Add(acChart);
                    accountChartService.AddAccountChartAndParent(objAccountCharts);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "nok", message = "Sorry for inconvenience! Please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "ok", message = "Account Added Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAccountChart(int accountId, string accountCode, string accountName, string accountTypeCode, int glLevelId, string parentAccountCode, bool isVoucher)
        {
            try
            {
                AccountChart objAccountChart = accountChartService.GetAccountChartByAccountCode(accountCode);
                if (string.IsNullOrEmpty(objAccountChart.AccountId.ToString()))
                {
                    return Json(new { status = "nok", message = "Account does not exist" }, JsonRequestBehavior.AllowGet);
                }

                AccountChart objAccChart = accountChartService.GetAccountChartExceptThisAccountCode(accountCode, accountName);
                if (objAccChart != null)
                {
                    return Json(new { status = "nok", message = "Account Name already exist" }, JsonRequestBehavior.AllowGet);
                }

                objAccountChart.AccountId = accountId;
                objAccountChart.AccountCode = accountCode;

                objAccountChart.AccountName = accountName;
                objAccountChart.AccountTypeCode = accountTypeCode;
                objAccountChart.GLLevelId = glLevelId;
                objAccountChart.ParentAccountCode = parentAccountCode;
                objAccountChart.IsVoucher = isVoucher;
                objAccountChart.UpdateUser = SessionHelper.LoggedInEmployeeID;
                objAccountChart.UpdateDate = DateTime.Now;
                accountChartService.Update(objAccountChart);
            }
            catch
            {
                return Json(new { status = "nok",  message = "Sorry for inconvenience! Please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "ok", message = "Account Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(AccountChartViewModel model)
        {
            model.AccountTypeList = new SelectList(accountTypeService.GetAll(), "AccountTypeCode", "AccountTypeName");
            model.GLLevelList = new SelectList(gLLevelService.GetAll(), "GLLevelId", "GLLevelName");
        }
        public JsonResult GetParentAccount(string accountTypeCode, int glLevelId)
        {
            try
            {
                if (!string.IsNullOrEmpty(accountTypeCode))
                {
                    var accountChart = accountChartService.GetAll().Where(x => x.AccountTypeCode == accountTypeCode && x.GLLevelId == glLevelId - 1);
                    var viewAccountChart = accountChart.Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.AccountCode,
                        Text = x.AccountName
                    });
                    var accountChartItems = new List<SelectListItem>();
                    accountChartItems.Add(new SelectListItem() { Text = "Select Parent Account", Value = "", Selected = true });
                    accountChartItems.AddRange(viewAccountChart);
                    return Json(accountChartItems, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVoucherableAccountChart(string voucherType)
        {
            var objAccountChartList = new List<AccountChart>();
            try
            {
                objAccountChartList = accountChartService.GetVoucherableAccountChart(voucherType).ToList();
            }
            catch(Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objAccountChartList, JsonRequestBehavior.AllowGet);
        }
       
        #endregion
        
        #region Events
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            AccountChartViewModel model = new AccountChartViewModel();
            MapDropDownList(model);
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            AccountChartViewModel model = new AccountChartViewModel();
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                AccountChart objAccountChart = accountChartService.GetAccountChartByAccountCode(id.ToString());
                model.AccountId = objAccountChart.AccountId;
                model.AccountCode = objAccountChart.AccountCode;
                model.GLLevelId = objAccountChart.GLLevelId;
                model.glLevel = objAccountChart.GLLevelId.ToString();
                model.ParentAccountCode = objAccountChart.ParentAccountCode;
                model.AccountName = objAccountChart.AccountName;

                model.AccountTypeCode = objAccountChart.AccountTypeCode;
                model.acCode = objAccountChart.AccountTypeCode;
                model.IsVoucher = objAccountChart.IsVoucher;

                MapDropDownList(model);
                //For Parent Accounts
                var parentAccounts = accountChartService.GetAll().Where(x => x.AccountTypeCode == objAccountChart.AccountTypeCode && x.GLLevelId == objAccountChart.GLLevelId - 1).ToList();
                model.ParentAccountList = new SelectList(parentAccounts, "AccountCode", "AccountName");
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        #endregion
    }
}
