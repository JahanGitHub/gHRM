
#region Usings

using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFLoanTypeController : BaseController
    {
        #region Private Members

        private readonly ILoanTypeService loanTypeService;

        #endregion

        #region Ctor

        public PFLoanTypeController(ILoanTypeService loanTypeService)
        {
            this.loanTypeService = loanTypeService;
        }

        #endregion

        #region Listings

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLoanTypeList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string loanTypeId = "", string loanTypeName = "")
        {
            try
            {

                int lTypeId = 0;
                if (!string.IsNullOrEmpty(loanTypeId))
                    lTypeId = Convert.ToInt32(loanTypeId);

                IEnumerable<LoanType> objLoanTypes = new List<LoanType>();

                objLoanTypes = loanTypeService.GetAll().Where(x =>
                                                                       x.LoanTypeId == (lTypeId == 0 ? x.LoanTypeId : lTypeId) &&
                                                                       x.LoanTypeName == (loanTypeName == string.Empty ? x.LoanTypeName : loanTypeName)
                                                                       );


                var List_ViewModel = objLoanTypes.AsEnumerable()
               .Select(row => new LoanTypeViewModel
               {
                   Id = row.Id.ToString(),
                   LoanTypeId = row.LoanTypeId.ToString(),
                   LoanTypeName = row.LoanTypeName,
                   InterestRate = Math.Round(row.InterestRate, 2).ToString(),
                   LoanPercentage = row.LoanPercentage.HasValue ? (row.LoanPercentage.Value).ToString() : "0",
                   InterestRateType = row.InterestRateTypeId == 1 ? "Fixed Rate" : "Variable Rate"
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

        #region Create
        public ActionResult Create()
        {
            LoanTypeViewModel model = new LoanTypeViewModel();
            GetInterestRateType(model);
            return View(model);
        }
        public JsonResult SaveLoanType(string loanTypeId, string loanTypeName, string intersetRate, string interestRateTypeId, string loanPercentage)
        {

            LoanType objLoanType = new LoanType();
            try
            {
                var loanTypes = loanTypeService.GetAll().Where(x => x.IsDeleted == false && x.LoanTypeId.ToString() == loanTypeId && x.LoanTypeName == loanTypeName).ToList(); //.GetLoanTypeByName(loanTypeName).ToList();
                if (loanTypes.Count > 0)
                    return Json(new { message = "Already exist" }, JsonRequestBehavior.AllowGet);

                objLoanType.LoanTypeId = Convert.ToInt32(loanTypeId);
                objLoanType.LoanTypeName = loanTypeName;
                objLoanType.InterestRate = Convert.ToDecimal(intersetRate);
                objLoanType.InterestRateTypeId = Convert.ToInt32(interestRateTypeId);

                if (loanPercentage == string.Empty)
                    objLoanType.LoanPercentage = null;
                else
                    objLoanType.LoanPercentage = Convert.ToInt32(loanPercentage);

                objLoanType.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objLoanType.CreateDate = DateTime.Now;
                var loanType = loanTypeService.Create(objLoanType);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            LoanTypeViewModel model = new LoanTypeViewModel();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                var objLoanType = loanTypeService.GetById(Convert.ToInt32(id));
                if (objLoanType != null)
                {
                    model.Id = objLoanType.Id.ToString();
                    model.LoanTypeId = objLoanType.LoanTypeId.ToString();
                    model.LoanTypeName = objLoanType.LoanTypeName;
                    model.InterestRate = Math.Round(objLoanType.InterestRate, 2).ToString();
                    model.InterestRateTypeId = objLoanType.InterestRateTypeId;
                    model.LoanPercentage = objLoanType.LoanPercentage.HasValue ? objLoanType.LoanPercentage.Value.ToString() : "0";
                    GetInterestRateType(model);
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public JsonResult UpdateLoanType(string id, string loanTypeId, string loanTypeName, string interestRate, string interestRateTypeId, string loanPercentage)
        {
            try
            {
                LoanType objLoanType = loanTypeService.GetById(Convert.ToInt32(id));
                if (objLoanType == null)
                    return Json(new { message = "Record does not exist" }, JsonRequestBehavior.AllowGet);

                objLoanType.Id = Convert.ToInt32(id);
                objLoanType.LoanTypeId = Convert.ToInt32(loanTypeId);
                objLoanType.LoanTypeName = loanTypeName;
                objLoanType.InterestRate = Convert.ToDecimal(interestRate);
                objLoanType.InterestRateTypeId = Convert.ToInt32(interestRateTypeId);

                if (loanPercentage == string.Empty)
                    objLoanType.LoanPercentage = null;
                else
                    objLoanType.LoanPercentage = Convert.ToInt32(loanPercentage);

                objLoanType.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objLoanType.UpdateDate = DateTime.Now;
                loanTypeService.Update(objLoanType);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Loan Type       

        public JsonResult GetLoanType(string loanTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(loanTypeId))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                List<LoanType> List_LoanType = new List<LoanType>();
                List<LoanType> loanTypeList = loanTypeService.GetAll().Where(x => x.LoanTypeId == Convert.ToInt32(loanTypeId) && x.IsDeleted == false).ToList();

                if (loanTypeId != null)
                {
                    return Json(loanTypeList.ToList(), JsonRequestBehavior.AllowGet);
                }
                var currentPageRecords = loanTypeList.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = loanTypeList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function
        
        public JsonResult DeleteLoanType(int loanTypeId)
        {
            string result = "OK";
            try
            {
                LoanType objLoanType = loanTypeService.GetById(loanTypeId);
                if (string.IsNullOrEmpty(objLoanType.LoanTypeId.ToString()))
                {
                    Response.StatusCode = 403;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                objLoanType.IsDeleted = true;
                objLoanType.DeletedUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objLoanType.DeleteDate = DateTime.Now;
                loanTypeService.Update(objLoanType);
            }
            catch
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);           
        }

        #endregion

        #region Private Methods

        public void GetInterestRateType(LoanTypeViewModel model)
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Fixed Rate", Value = "1" });
            list.Add(new SelectListItem() { Text = "Variable Rate", Value = "2" });

            var InterestRateTypeList = new List<SelectListItem>();
            InterestRateTypeList.Add(new SelectListItem() { Text = "Interest Rate Type", Value = "", Selected = true });
            InterestRateTypeList.AddRange(list);
            model.InterestRateTypeList = InterestRateTypeList;
        } 

        #endregion
    }
}
