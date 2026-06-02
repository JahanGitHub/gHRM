using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using System.Globalization;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class PFContributionCollectionController : BaseController
    {
        //New

        private readonly IProcessLogService processLogService;
        private readonly IOrganizationSetupService orgSetupService;

        private readonly ICollectionService collectionService;
        private readonly IEmployeeService employeeService;

        private readonly ITransactionCategoryService transCategoryService;
        private readonly IEmployeeSPService employeeSPService;
        
        private readonly int transCatIdOfCollByCash = 2;  //Contribution By Cash
        private readonly int transCatIdOfCollFromPayroll = 1;  //Contribution By Cash
        public PFContributionCollectionController
                            (
                            IProcessLogService processLogService,
                            IOrganizationSetupService orgSetupService,
                            ICollectionService collectionService,

                            IEmployeeService employeeService,
                            ITransactionCategoryService transCategoryService,
                            IEmployeeSPService employeeSPService//,
                            )
        {

            this.processLogService = processLogService;
            this.orgSetupService = orgSetupService;
            this.collectionService = collectionService;
            this.employeeService = employeeService;
            this.transCategoryService = transCategoryService;
            this.employeeSPService = employeeSPService;
        }

        #region Methods
        //public JsonResult SaveContributionCollection(string employeeId, string transactionDate, string selfContribution, string orgContribution)
        public JsonResult SaveContributionCollection(ContributionCollectionViewModel model)
        {
            var obj = new ContributionRegister();
            try
            {
                if (string.IsNullOrEmpty(model.EmployeeId))
                    return Json(new { message = "Enter valid Employee Id" }, JsonRequestBehavior.AllowGet);

                //Check employee either EXIST or NOT
                var objEmployee = employeeService.GetById(Convert.ToInt32(model.EmployeeId));// empConfigurationService.GetById(Convert.ToInt32(employeeId));
                if (objEmployee == null)
                    return Json(new { message = "Employee does not exist" }, JsonRequestBehavior.AllowGet);

                else if (string.IsNullOrEmpty(model.Comment))
                    return Json(new { message = "Comment is required" }, JsonRequestBehavior.AllowGet);
                else if (string.IsNullOrEmpty(model.TransactionDate))
                    return Json(new { message = "Transaction Date is required" }, JsonRequestBehavior.AllowGet);
                DateTime tr_dt = DateTime.Now;
                DateTime.TryParse(model.TransactionDate, out tr_dt);
                if(DateTime.MinValue.Equals(tr_dt))
                    return Json(new { message = "Transaction Date format is not correct" }, JsonRequestBehavior.AllowGet);

                decimal selfContribution = 0, orgContribution = 0;
                decimal.TryParse(model.SelfContribution, out selfContribution);
                decimal.TryParse(model.OrgContribution, out orgContribution);

                if (selfContribution <= 0)
                    return Json(new { message = "self Contribution Amount is required." }, JsonRequestBehavior.AllowGet);
                
                obj.EmployeeId = objEmployee.EmployeeId;
                obj.SelfContribution = selfContribution;
                obj.OrgContribution = orgContribution;
                obj.TransactionDate = tr_dt;
                obj.Comments = model.Comment;

                obj.TransactionType = PFTransactionTypeConstants.Contribution;
                obj.IsDeleted = false;
                obj.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                obj.CreateDate = DateTime.Now;
                using (gHRMDBContext db=new gHRMDBContext())
                {
                    db.ContributionRegisters.Add(obj);
                    db.SaveChanges();
                }
                //SaveContribution(objCollection);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateContributionCollection(string collectionId, string employeeId, string selfContribution, string orgContribution)
        {
            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);
                if (!processLog.IsOpen)
                    return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

                //Form Validation
                if (string.IsNullOrEmpty(collectionId) || string.IsNullOrEmpty(employeeId))
                    return Json(new { message = "Enter valid input" }, JsonRequestBehavior.AllowGet);

                var orgSetup = orgSetupService.GetMany(x => x.IsDeleted == false && x.IsActive == true).SingleOrDefault();
                if (orgSetup == null)
                    return Json(new { message = "Setup Organization first." }, JsonRequestBehavior.AllowGet);

                //if (Convert.ToDateTime(transactionDate).Date < orgSetup.YearStartDate.Date || Convert.ToDateTime(transactionDate).Date > orgSetup.YearEndDate.Date)
                //    return Json(new { message = "Enter Transaction date between fiscal year" }, JsonRequestBehavior.AllowGet);                

                Collection objCollection = collectionService.GetById(Convert.ToInt32(collectionId));
                ////Is Exist in Store
                if (string.IsNullOrEmpty(objCollection.CollectionId.ToString()))
                    return Json(new { message = "Contribution did not collected" }, JsonRequestBehavior.AllowGet);


                objCollection.TransactionDate = processLog.StartDate;

                if (string.IsNullOrEmpty(selfContribution))
                    objCollection.SelfContribution = 0;
                else
                    objCollection.SelfContribution = Convert.ToDecimal(selfContribution);
                if (string.IsNullOrEmpty(orgContribution))
                    objCollection.OrgContribution = 0;
                else
                    objCollection.OrgContribution = Convert.ToDecimal(orgContribution);

                objCollection.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objCollection.UpdateDate = DateTime.Now;
                UpdateContribution(objCollection);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetContributionCollectionList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string employeeId = "", string employeeName = "", string collectionTypeId = "")
        public JsonResult GetContributionCollectionList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string employeeCode = "", string employeeName = "")
        {
            string message = "Sorry for inconvenience! please try again later";

            try
            {

                var dataset = collectionService.GetCollections(employeeCode, employeeName, transCatIdOfCollByCash);
                var List_ViewModel = dataset.Tables[0].AsEnumerable()
                .Select(row => new ContributionCollectionViewModel
                {
                    CollectionId = row.Field<Int64>("CollectionId").ToString(),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeId = row.Field<Int64>("EmployeeId").ToString(),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CollectionType = row.Field<string>("CollectionType"),
                    TransactionType = row.Field<string>("TransactionType"),
                    TransactionDateString = Convert.ToDateTime(row.Field<DateTime>("TransactionDate")).ToString("dd-MMM-yyyy"),
                    SelfContribution = row.Field<decimal>("SelfContribution") == 0 ? "0" : Math.Round(row.Field<decimal>("SelfContribution"), 2).ToString(),
                    OrgContribution = row.Field<decimal>("OrgContribution") == 0 ? "0" : Math.Round(row.Field<decimal>("OrgContribution"), 2).ToString() //row.Field<decimal>("OrgContribution")
                }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return this.Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropDownListAsTransCat(ContributionCollectionViewModel model)
        {
            var transCategories = transCategoryService.GetMany(x => x.IsDeleted == false);
            var transCatDataItems = transCategories.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.TransCategoryId.ToString(),
                Text = x.TransCategoryName
            });
            var transCatItems = new List<SelectListItem>();
            transCatItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            transCatItems.AddRange(transCatDataItems);
            model.TransactionCatList = transCatItems;
        }

        public JsonResult GetEmployeeByEmpId(string employeeId)
        {
            var processLog = processLogService.GetLastProcessLog();
            if (processLog == null)
                return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);
            if (!processLog.IsOpen)
                return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

            string employeeName = string.Empty;
            try
            {
                Employee objEmp = new Employee();
                objEmp = employeeService.GetById(Convert.ToInt32(employeeId));
                if (objEmp != null)
                    employeeName = objEmp.EmployeeName;

                return Json(new { EmployeeName = employeeName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { EmployeeName = employeeName }, JsonRequestBehavior.AllowGet);
            }
        }
        private void MapDropDownList(ContributionCollectionViewModel model)
        {
            var transType = new List<SelectListItem>();
            transType.Add(new SelectListItem() { Text = "Select Transaction Type", Value = "", Selected = true });
            transType.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            transType.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            model.TransactionTypeList = transType;
        }

        private void SaveContribution(Collection objCollection)
        {
            var param = new
            {
                @EmployeeId = objCollection.EmployeeId,
                @CollectionTypeId = objCollection.CollectionTypeId,
                @SelfContribution = objCollection.SelfContribution,
                @OrgContribution = objCollection.OrgContribution,
                @LoanAmount = objCollection.LoanAmount,
                @InterestAmount = objCollection.InterestAmount,
                @TransactionType = objCollection.TransactionType,
                @TransactionDate = objCollection.TransactionDate,
                @CreateUser = objCollection.CreateUser,
                @CreateDate = objCollection.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_SaveContributionCollection");
        }

        private void UpdateContribution(Collection objCollection)
        {
            var param = new
            {
                @CollectionId = objCollection.CollectionId,
                @EmployeeId = objCollection.EmployeeId,
                @CollectionTypeId = objCollection.CollectionTypeId,
                @SelfContribution = objCollection.SelfContribution,
                @OrgContribution = objCollection.OrgContribution,
                @LoanAmount = objCollection.LoanAmount,
                @InterestAmount = objCollection.InterestAmount,
                @TransactionType = objCollection.TransactionType,
                @TransactionDate = objCollection.TransactionDate,
                @UpdateUser = objCollection.UpdateUser,
                @UpdateDate = objCollection.UpdateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_UpdateContributionCollection");
        }
        public IEnumerable<SelectListItem> GetMonthList()
        {
            var months = Enumerable.Range(1, 12).Select(x =>
                 new SelectListItem()
                 {
                     Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1],// + " (" + x + ")",
                     Value = x.ToString()
                     //,Selected = (x == Model.ExpirationMonth)
                 });

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Select Month", Value = "", Selected = true });
            monthList.AddRange(months);
            return monthList;
        }

        public string GetEmployeeNameByEmpId(long employeeId)
        {
            string employeeName = string.Empty;
            string message = string.Empty;
            try
            {
                int empId = Convert.ToInt32(employeeId);

                Employee objEmployee = new Employee();
                objEmployee = employeeService.GetById(empId);
                if (objEmployee != null)
                    employeeName = objEmployee.EmployeeName;
            }
            catch (Exception ex)
            {
            }
            return employeeName;
        }

        #endregion

        #region Event Handlers
        public ActionResult ContributionCollectionList()
        {
            ContributionCollectionViewModel model = new ContributionCollectionViewModel();
            try
            {
                GetCustomDayStatus(model);
                MapDropDownListAsTransCat(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }
        public ActionResult SpecialContribution()
        {
            ContributionCollectionViewModel model = new ContributionCollectionViewModel();
            try
            {
                GetCustomDayStatus(model);
                var org = orgSetupService.GetOrganization().SingleOrDefault();
                if (org != null)
                    model.PFType = org.PFType.ShortName;
            }
            catch (Exception ex)
            {
            }
            MapDropDownList(model);
            return View(model);
        }

        public ActionResult EditContributionCollection(int id)
        {
            ContributionCollectionViewModel model = new ContributionCollectionViewModel();

            try
            {
                GetCustomDayStatus(model);
                var org = orgSetupService.GetOrganization().SingleOrDefault();
                if (org != null)
                    model.PFType = org.PFType.ShortName;

                //var collection = collectionService.GetCollectionByCollId(id);
                var collection = collectionService.GetById(id);
                if (collection != null)
                {
                    model.CollectionId = collection.CollectionId.ToString();
                    model.EmployeeId = collection.EmployeeId.ToString();
                    model.EmployeeName = GetEmployeeNameByEmpId(collection.EmployeeId);
                    model.TransactionType = collection.TransactionType;
                    if (collection.TransactionDate == null)
                        model.TransactionDateString = string.Empty;
                    else
                        model.TransactionDateString = Convert.ToDateTime(collection.TransactionDate).ToString("dd-MMM-yyyy");
                    model.SelfContribution = Math.Round(collection.SelfContribution, 2).ToString();
                    model.OrgContribution = Math.Round(collection.OrgContribution, 2).ToString();

                    var objEmployee = employeeService.GetById(Convert.ToInt32(model.EmployeeId));
                    model.EmployeeCode = objEmployee.EmployeeCode;

                }
            }
            catch (Exception ex)
            {
            }

            //MapDropDownList(model);
            return View(model);
        }
        private void GetCustomDayStatus(ContributionCollectionViewModel model)
        {
            var objProcessLog = processLogService.GetCustomDayStatus();
            if (objProcessLog != null)
            {
                model.IsOpen = objProcessLog.IsOpen;
                model.DayStatus = objProcessLog.DayStatus;
                model.TransactionDate = objProcessLog.TransactionDateString;
                model.SystemDate = objProcessLog.SystemDate;
            }
        }

        public ActionResult ContributionReport()
        {
            ContributionCollectionViewModel model = new ContributionCollectionViewModel();
            try
            {
                model.MonthList = GetMonthList();
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        #endregion
    }
}
