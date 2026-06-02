
#region Usings

using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFTransactionCategoryController : BaseController
    {
        #region Private Variables


        private readonly ITransactionCategoryService transCategoryService;
        private readonly IAccountChartService accountChartService;

        #endregion

        #region Ctor
        public PFTransactionCategoryController(ITransactionCategoryService transCategoryService, IAccountChartService accountChartService)
        {
            this.transCategoryService = transCategoryService;
            this.accountChartService = accountChartService;
        }
        #endregion

        #region Listing

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTransCategoryList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string categoryId = "", string transName = "")
        {
            try
            {

                int catId = 0;
                if (!string.IsNullOrEmpty(categoryId))
                    catId = Convert.ToInt32(categoryId);

                IEnumerable<TransactionCategory> objTransCatList = new List<TransactionCategory>();
                objTransCatList = transCategoryService.GetAll().Select(x => x).Where(x =>
                                                                         x.TransCategoryId == (catId == 0 ? x.TransCategoryId : catId) &&
                                                                         x.TransCategoryName == (transName == string.Empty ? x.TransCategoryName : transName)
                                                                       ).ToList();


                var currentPageRecords = objTransCatList.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objTransCatList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion

        #region Create


        public ActionResult Create()
        {
            TransactionCategoryViewModel model = new TransactionCategoryViewModel();
            MapDropDownList(model);
            return View(model);
        }

        public JsonResult SaveTransactionCategory(
                                                  string transCatId,
                                                  string TransCatName,
                                                  string transGroup,
                                                  string accountId,
                                                  string transType,
                                                  string particulars,
                                                  string revAccountId,
                                                  string revTransType,
                                                  string revParticulars
                                                 )
        {
            try
            {
                TransactionCategory objTransCategory = new TransactionCategory();

                var objTCat = transCategoryService.GetById(Convert.ToInt32(transCatId));
                if (objTCat != null)
                    return Json(new { status = "nok", message = "Already Exist" }, JsonRequestBehavior.AllowGet);

                objTransCategory.TransCategoryId = Convert.ToInt32(transCatId);
                objTransCategory.TransCategoryName = TransCatName;
                objTransCategory.TransGroupName = transGroup;

                objTransCategory.AccountId = Convert.ToInt32(accountId);
                objTransCategory.TransactionType = transType;
                objTransCategory.Particulars = particulars;

                objTransCategory.ReverseAccountId = Convert.ToInt32(accountId);
                objTransCategory.ReverseTransactionType = transType;
                objTransCategory.ReverseParticulars = particulars;

                objTransCategory.CreateUser = SessionHelper.LoggedInEmployeeID;
                objTransCategory.CreateDate = DateTime.Now;
                objTransCategory.IsDeleted = false;

                transCategoryService.Create(objTransCategory);
            }
            catch (Exception ex)
            {
                return Json(new { status = "nok", message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "ok", message = "Added Successfully" }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            TransactionCategoryViewModel model = new TransactionCategoryViewModel();

            var objTransaCategory = transCategoryService.GetById(id);
            if (objTransaCategory != null)
            {
                model.TransCategoryId = objTransaCategory.TransCategoryId.ToString();
                model.TransCategoryName = objTransaCategory.TransCategoryName;
                model.TransGroupName = objTransaCategory.TransGroupName;

                model.AccountId = objTransaCategory.AccountId.ToString();
                model.TransactionType = objTransaCategory.TransactionType;
                model.Particulars = objTransaCategory.Particulars;

                model.ReverseAccountId = objTransaCategory.ReverseAccountId.ToString();
                model.ReverseTransactionType = objTransaCategory.ReverseTransactionType;
                model.ReverseParticulars = objTransaCategory.ReverseParticulars;
            }
            MapDropDownList(model);
            return View(model);
        }

        public JsonResult UpdateTransactionCategory(
                                                   string transCatId,
                                                   string TransCatName,
                                                   string transGroup,
                                                   string accountId,
                                                   string transType,
                                                   string particulars,
                                                   string revAccountId,
                                                   string revTransType,
                                                   string revParticulars
                                                    )
        {

            try
            {
                TransactionCategory objTransCategory = transCategoryService.GetById(Convert.ToInt32(transCatId));
                if (objTransCategory == null)
                    return Json(new { status = "nok", message = "Does not exist" }, JsonRequestBehavior.AllowGet);

                objTransCategory.TransCategoryId = Convert.ToInt32(transCatId);
                objTransCategory.TransCategoryName = TransCatName;
                objTransCategory.TransGroupName = transGroup;

                objTransCategory.AccountId = Convert.ToInt32(accountId);
                objTransCategory.TransactionType = transType;
                objTransCategory.Particulars = particulars;

                objTransCategory.ReverseAccountId = Convert.ToInt32(revAccountId);
                objTransCategory.ReverseTransactionType = revTransType;
                objTransCategory.ReverseParticulars = revParticulars;

                objTransCategory.UpdateUser = SessionHelper.LoggedInEmployeeID;
                objTransCategory.UpdateDate = DateTime.Now;
                transCategoryService.Update(objTransCategory);
            }
            catch (Exception ex)
            {
                return Json(new { status = "nok", message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "ok", message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get AccountType By AccountCode

        public JsonResult GetAccountTypeByAccountCode(string accountCode)
        {
            string accountName = string.Empty;
            string voucherStatus = string.Empty;
            try
            {
                AccountChart objAccountChart = new AccountChart();
                objAccountChart = accountChartService.GetAccountChartByAccountCode(accountCode);
                if (objAccountChart != null)
                {
                    if (objAccountChart.IsVoucher == true)
                        accountName = objAccountChart.AccountName;
                }

                return Json(new { AccountName = accountName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AccountName = accountName }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods

        private void MapDropDownList(TransactionCategoryViewModel model)
        {
            var transGroup = new List<SelectListItem>();
            transGroup.Add(new SelectListItem() { Text = "Select Transaction Group", Value = "", Selected = true });
            transGroup.Add(new SelectListItem() { Text = TransactionGroupConstants.PF, Value = TransactionGroupConstants.PF });
            transGroup.Add(new SelectListItem() { Text = TransactionGroupConstants.LN, Value = TransactionGroupConstants.LN });
            model.TransactionGroupList = transGroup;

            var transType = new List<SelectListItem>();
            transType.Add(new SelectListItem() { Text = "Select Transaction Type", Value = "", Selected = true });
            transType.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            transType.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            model.TransactionTypeList = transType;

            //get listings from [gcpf.AccountChart]
            var objAccountChartList = accountChartService.GetAll().Where(x => x.IsDeleted == false);
            //&& x.IsVoucher == true);
            var accountChartDataItems = objAccountChartList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccountId.ToString(),
                Text = x.AccountCode + "- " + x.AccountName
            });

            var accountChartItems = new List<SelectListItem>();
            accountChartItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            accountChartItems.AddRange(accountChartDataItems);
            model.AccountChartList = accountChartItems;
            model.ReverseAccountChartList = accountChartItems;
        }


        #endregion
    }
}
