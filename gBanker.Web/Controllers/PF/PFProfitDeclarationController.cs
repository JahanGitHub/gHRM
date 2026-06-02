#region Usings

using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
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
    public class PFProfitDeclarationController : BaseController
    {
        #region Private Methods
        private readonly IProfitDeclarationService profitDeclarationService;

        #endregion

        #region Ctor
        public PFProfitDeclarationController(IProfitDeclarationService profitDeclarationService)
        {
            this.profitDeclarationService = profitDeclarationService;
        }
        #endregion

        #region Listings
        public ActionResult Index()
        {
            ProfitDeclarationViewModel model = new ProfitDeclarationViewModel()
            {
                CalculationWithProfit = true
            };
            return View(model);
        }

        public JsonResult GetProfitDeclarationList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string Id = "")
        {
            try
            {
                var objProfitDeclarationList = new List<ProfitDeclaration>();

                //get profit declaration from [gcpf.ProfitDeclaration]
                if (string.IsNullOrEmpty(Id))
                    objProfitDeclarationList = profitDeclarationService.GetAll().OrderByDescending(x => x.YearStartDate).ToList();

                var List_ViewModel = objProfitDeclarationList.AsEnumerable()
                      .Select(row => new ProfitDeclarationViewModel
                      {
                          DeclararionId = row.DeclararionId.ToString(),
                          DeclarationYear = row.YearStartDate != null ? row.YearStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty + " - " + row.YearEndDate != null ? row.YearEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                          Profit = Math.Round(row.Profit, 2).ToString(),
                          ProfitRate = Math.Round(row.ProfitRate, 2).ToString(),
                          InduceRate = Math.Round(row.InduceRate, 2).ToString(),
                          DistribursAmount= Math.Round(row.DistribursAmount??0, 2).ToString(),
                          DeclarationStatus =   row.DeclarationStatus

                      }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            var model = new ProfitDeclarationViewModel();

            var profitDeclaration = profitDeclarationService.GetById(id);
            if (profitDeclaration != null)
            {
                model.DeclararionId = profitDeclaration.DeclararionId.ToString();

                model.DeclarationYear = profitDeclaration.YearStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) + " - " + profitDeclaration.YearEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                model.Profit = Math.Round(profitDeclaration.Profit, 2).ToString();
                model.ProfitRate = Math.Round(profitDeclaration.ProfitRate, 2).ToString();
                model.InduceRate = Math.Round(profitDeclaration.InduceRate, 2).ToString();

                bool isValid = isValidForEdit(profitDeclaration.YearStartDate.Year);
                if (isValid)
                    model.IsInduceRateReadonly = false;
                else
                    model.IsInduceRateReadonly = true;
            }

            return View(model);
        }

        public JsonResult UpdateProfitDeclaration(int declararionId, string induceRate, bool isDeclared)
        {
            try
            {
                ProfitDeclaration objProfitDeclaration = profitDeclarationService.GetById(declararionId);
                if (objProfitDeclaration == null)
                    return Json(new { message = "Does not exist" }, JsonRequestBehavior.AllowGet);

                bool isValid = isValidForEdit(objProfitDeclaration.YearStartDate.Year);
                if (!isValid)
                    return Json(new { message = "You are trying to edit back dated induce rate" }, JsonRequestBehavior.AllowGet);

                objProfitDeclaration.DeclararionId = declararionId;
                objProfitDeclaration.InduceRate = Convert.ToDecimal(induceRate);

                objProfitDeclaration.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objProfitDeclaration.UpdateDate = DateTime.Now;
                profitDeclarationService.Update(objProfitDeclaration);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region    Ajax Call
        [HttpGet]
        public JsonResult GetProfitRate(decimal? profitAmt, bool withprofit, DateTime enddate)
        {
            decimal profitRate = 0;
            if ((profitAmt ?? 0) > 0)
            {
                decimal cont_Amt = 0;
                try
                {
                    using (gHRMDBContext db = new gHRMDBContext())
                    {
                        if (withprofit)
                            cont_Amt = db.ContributionRegisters.Where(x => x.TransactionDate <= enddate && (x.TransactionType == PFTransactionTypeConstants.Contribution || x.TransactionType == PFTransactionTypeConstants.Profit)).Sum(x => x.SelfContribution + x.OrgContribution);
                        else cont_Amt = db.ContributionRegisters.Where(x => x.TransactionDate <= enddate && x.TransactionType == PFTransactionTypeConstants.Contribution).Sum(x => x.SelfContribution + x.OrgContribution);
                    }
                    if (cont_Amt > 0) profitRate = decimal.Round(((profitAmt.Value / cont_Amt)*100),2,MidpointRounding.AwayFromZero);
                }
                catch (Exception)
                {

                }
            }
            return Json(profitRate,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PostProfitDeclaration(ProfitDeclaration obj)
        {
            string msg = "";
            bool status = false;
            if (obj == null) msg = "Data is not found";
            else if (obj.DeclararionId > 0)/*Update*/
            {
                var _lst = profitDeclarationService.GetMany(x => x.DeclararionId != obj.DeclararionId && ((x.YearStartDate >= obj.YearStartDate && x.YearStartDate <= obj.YearEndDate) || (x.YearEndDate >= obj.YearStartDate && x.YearEndDate <= obj.YearEndDate)));
                if (_lst.Any()) msg = "Profit Time already declared";
                else
                {
                    obj.UpdateDate = DateTime.UtcNow;
                    obj.UpdateUser = (int)LoggedInEmployeeId;
                    profitDeclarationService.Update(obj);
                }
            }
            else/*Insert*/
            {
                var _lst = profitDeclarationService.GetMany(x => (x.YearStartDate >= obj.YearStartDate && x.YearStartDate <= obj.YearEndDate) || (x.YearEndDate >= obj.YearStartDate && x.YearEndDate <= obj.YearEndDate));
                if (_lst.Any()) msg = "Profit Time already declared";
                else
                {
                    obj.DeclarationStatus = ProfitDeclarationConstants.Entry;
                    obj.CreateDate = DateTime.UtcNow;
                    obj.CreateUser = (int)LoggedInEmployeeId;
                    profitDeclarationService.Create(obj);
                    status = true;
                    msg = "Save Successfully";
                }
            }
            return Json(new { Message = msg, Status = status });
        }
        [HttpPost]
        public JsonResult DeleteProfitDeclaration(int id) 
        {
            var obj=profitDeclarationService.GetById(id);
            obj.DeclarationStatus = ProfitDeclarationConstants.Delete;
            obj.DeletedUser= (int)LoggedInEmployeeId;
            obj.DeleteDate = DateTime.UtcNow;
            profitDeclarationService.Update(obj);
            return Json("Delete Successfully");
        }
        [HttpPost]
        public JsonResult ApprovedProfitDeclaration(int id) 
        {
            var obj = profitDeclarationService.GetById(id);
            obj.DeclarationStatus = ProfitDeclarationConstants.Approved;
            obj.UpdateUser = (int)LoggedInEmployeeId;
            obj.UpdateDate = DateTime.UtcNow;
            profitDeclarationService.Update(obj);
            return Json("Approved Successfully");
        }
        #endregion Ajax Call
        #region Private Methods  

        private bool isValidForEdit(int declarationYear)
        {
            bool isValid = false;
            try
            {
                int maxDeclarationYear = profitDeclarationService.GetMany(x => x.DeclarationStatus != ProfitDeclarationConstants.Delete).Max(x => x.YearStartDate.Year);
                isValid = maxDeclarationYear == declarationYear ? true : false;
            }
            catch (Exception)
            {

            }
            return isValid;
        }

        #endregion
    }
}
