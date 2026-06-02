
#region Usings

using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Service;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service.StoreProcedure;
using System.Globalization;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFLoanCollectionController : BaseController
    {
        #region Private Variables

        private readonly IProcessLogService processLogService;
        private readonly IOrganizationSetupService orgSetupService;
        private readonly ICollectionService collectionService;
        private readonly IEmployeeService employeeService;
        private readonly ITransactionCategoryService transCategoryService;
        private readonly ILoanTypeService loanTypeService;
        private readonly ILoanDisbursementService loanDisbService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly int transCatIdOfLoanCollByCash = 7;  //Loan Paid By Cash

        #endregion

        #region Ctor
        public PFLoanCollectionController(IProcessLogService processLogService, IOrganizationSetupService orgSetupService, ICollectionService collectionService, IEmployeeService employeeService,
                                          ITransactionCategoryService transCategoryService, ILoanTypeService loanTypeService, ILoanDisbursementService loanDisbService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.orgSetupService = orgSetupService;
            this.collectionService = collectionService;
            this.employeeService = employeeService;
            this.transCategoryService = transCategoryService;
            this.loanTypeService = loanTypeService;
            this.loanDisbService = loanDisbService;
            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Create Loan Collection
        public ActionResult CreateLoanCollection()
        {
            var model = new LoanCollectionViewModel();
            try
            {
                GetCustomDayStatus(model);
            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        public JsonResult SaveLoanCollection(string loanId, string employeeId, string amount, string loanInstallment, string interestInstallment, string interestCharge, string comment)
        {
            Collection objCollection = new Collection();
            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

                var orgSetup = orgSetupService.GetMany(x => x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (orgSetup == null)
                    return Json(new { message = "Setup Organization first." }, JsonRequestBehavior.AllowGet);

                //Check employee either EXIST or NOT
                var objEmployee = employeeService.GetById(Convert.ToInt32(employeeId));
                if (objEmployee == null)
                    return Json(new { message = "Employee does not exist" }, JsonRequestBehavior.AllowGet);

                //get Loan Installment by Cash for [gcpf.TransactionCategory]
                var objTransCategory = transCategoryService.GetById(transCatIdOfLoanCollByCash); //transCatIdOfLoanCollByCash=>loan paid by cash with id(Loan Installment by Cash)=7
                if (objTransCategory == null)
                    return Json(new { message = "Please check transaction category" }, JsonRequestBehavior.AllowGet);

                objCollection.EmployeeId = Convert.ToInt64(employeeId);
                objCollection.CollectionTypeId = objTransCategory.TransCategoryId;
                objCollection.TransactionType = objTransCategory.TransactionType;
                objCollection.TransactionDate = processLog.StartDate;

                if (!string.IsNullOrEmpty(loanInstallment))
                    objCollection.LoanAmount = Convert.ToDecimal(loanInstallment);
                if (!string.IsNullOrEmpty(interestInstallment))
                    objCollection.InterestAmount = Convert.ToDecimal(interestInstallment);
                objCollection.Comments = comment;

                objCollection.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objCollection.CreateDate = DateTime.Now;

                //let's insert into [gcpf.Collection]
                SaveLoan(objCollection, Convert.ToInt32(loanId), Convert.ToDecimal(amount), Convert.ToDecimal(interestCharge));
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Loan Installment Entry

        public ActionResult LoanInstallmentEntry()
        {
            var model = new LoanCollectionViewModel();
            try
            {
                GetCustomDayStatus(model);
            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        #endregion

        #region Events
        public ActionResult LoanCollectionList()
        {
            LoanCollectionViewModel model = new LoanCollectionViewModel();
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


        public ActionResult EditLoanCollection(int id)
        {
            LoanCollectionViewModel model = new LoanCollectionViewModel();
            try
            {
                GetCustomDayStatus(model);

                var collection = collectionService.GetById(id);
                if (collection != null)
                {
                    model.LoanId = collection.LoanId.ToString();
                    model.CollectionId = collection.CollectionId.ToString();
                    model.EmployeeId = collection.EmployeeId.ToString();
                    model.EmployeeName = GetEmployeeNameByEmpId(collection.EmployeeId);
                    model.TransactionType = collection.TransactionType;

                    if (collection.TransactionDate == null)
                        model.TransactionDate = string.Empty;
                    else
                        model.TransactionDate = Convert.ToDateTime(collection.TransactionDate).ToString("dd-MMM-yyyy");
                    model.Amount = Math.Round((collection.LoanAmount + collection.InterestAmount), 2).ToString();
                    model.LoanInstallment = Math.Round(collection.LoanAmount, 2).ToString();
                    model.InterestInstallment = Math.Round(collection.InterestAmount, 2).ToString();
                    decimal intCharge = collection.InterestCharge.HasValue ? Convert.ToDecimal(collection.InterestCharge) : 0;
                    model.InterestCharge = Math.Round(intCharge, 2).ToString();

                    var objEmployee = employeeService.GetById(Convert.ToInt32(model.EmployeeId));
                    model.EmployeeCode = objEmployee.EmployeeCode;
                }
            }
            catch (Exception ex)
            {
            }
            return View(model);
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

        #region Methods

        public JsonResult GetLoanCollectionList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string employeeCode = "", string employeeName = "")
        {
            try
            {
                var dataset = collectionService.GetCollections(employeeCode, employeeName, transCatIdOfLoanCollByCash);

                var List_ViewModel = dataset.Tables[0].AsEnumerable()
                .Select(row => new LoanCollectionViewModel
                {
                    LoanId = row.Field<Int64>("LoanId").ToString(),
                    CollectionId = row.Field<Int64>("CollectionId").ToString(),
                    EmployeeId = row.Field<Int64>("EmployeeId").ToString(),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CollectionType = row.Field<string>("CollectionType"),
                    TransactionType = row.Field<string>("TransactionType"),
                    TransactionDate = Convert.ToDateTime(row.Field<DateTime>("TransactionDate")).ToString("dd-MMM-yyyy"),
                    LoanAmount = row.Field<decimal>("LoanAmount") == 0 ? "0" : Math.Round(row.Field<decimal>("LoanAmount"), 2).ToString(),
                    InterestAmount = row.Field<decimal>("InterestAmount") == 0 ? "0" : Math.Round(row.Field<decimal>("InterestAmount"), 2).ToString(),
                    TotalInstallment = row.Field<decimal>("TotalInstallment") == 0 ? "0" : Math.Round(row.Field<decimal>("TotalInstallment"), 2).ToString(),
                    InterestCharge = row.Field<decimal>("InterestCharge") == 0 ? "0" : Math.Round(row.Field<decimal>("InterestCharge"), 2).ToString(),
                    Sundry = row.Field<decimal>("Sundry") == 0 ? "0" : Math.Round(row.Field<decimal>("Sundry"), 2).ToString(),
                    Comment = row.Field<string>("Comments")
                }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return this.Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateLoanCollection(string collectionId, string employeeId, string transactionDate, string loanInstallment, string interestInstallment)
        {
            try
            {

                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);
                if (!processLog.IsOpen)
                    return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

                if (string.IsNullOrEmpty(collectionId) || string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(transactionDate))
                    return Json(new { message = "Enter valid data" }, JsonRequestBehavior.AllowGet);

                var orgSetup = orgSetupService.GetMany(x => x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (orgSetup == null)
                    return Json(new { message = "Setup Organization first." }, JsonRequestBehavior.AllowGet);

                if (Convert.ToDateTime(transactionDate).Date < orgSetup.YearStartDate.Date || Convert.ToDateTime(transactionDate).Date > orgSetup.YearEndDate.Date)
                    return Json(new { message = "Enter Transaction date between fiscal year" }, JsonRequestBehavior.AllowGet);

                //Form Validation
                var objCollection = collectionService.GetById(Convert.ToInt32(collectionId));
                ////Is Exist in Store
                if (objCollection == null)
                    return Json(new { message = "Record does not exist" }, JsonRequestBehavior.AllowGet);

                var objTransCategory = transCategoryService.GetById(transCatIdOfLoanCollByCash);
                if (objTransCategory == null)
                    return Json(new { message = "Please check transaction category" }, JsonRequestBehavior.AllowGet);

                objCollection.CollectionId = Convert.ToInt64(collectionId);
                objCollection.EmployeeId = Convert.ToInt64(employeeId);

                objCollection.CollectionTypeId = objTransCategory.TransCategoryId;
                objCollection.TransactionType = objTransCategory.TransactionType;
                objCollection.TransactionDate = Convert.ToDateTime(transactionDate);

                objCollection.LoanAmount = Convert.ToDecimal(loanInstallment);
                objCollection.InterestAmount = Convert.ToDecimal(interestInstallment);

                objCollection.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objCollection.UpdateDate = DateTime.Now;
                UpdateLoan(objCollection);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeByLoanType(string loanTypeId, string employeeId)
        {
            string employeeName = string.Empty;
            int lTypeId = 0;

            try
            {
                int empId = Convert.ToInt32(employeeId);
                lTypeId = Convert.ToInt32(loanTypeId);

                var empployeeInfo = employeeService.GetByEmpId(empId);

                if (empployeeInfo == null)
                    return Json(new { EmployeeName = string.Empty, LoanId = string.Empty, status = "nok", message = "Employee does not exist." }, JsonRequestBehavior.AllowGet);

                //Getting Loan disbursement Information
                var disburse = loanDisbService.GetEmployeeWiseByLoanTypeId(empId, lTypeId);

                if (disburse == null)
                    return Json(new { EmployeeName = empployeeInfo.EmployeeName, LoanId = string.Empty, status = "nok", message = "Loan does not exist." }, JsonRequestBehavior.AllowGet);

                return Json(new { EmployeeName = empployeeInfo.EmployeeName, LoanId = disburse.LoanId, InterestCharge=disburse.InterestCharge, status = "ok", message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { EmployeeName = string.Empty, LoanId = string.Empty, status = "nok", message = "Sorry for inconvenience! please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoanInfoByLoanId(string loanId, string amount)
        {
            string employeeName = string.Empty;

            //out todaysPrinCollBeforeDayEnd, out todaysIntCollBeforeDayEnd,
            decimal todaysPrinCollBeforeDayEnd = 0;
            decimal todaysIntCollBeforeDayEnd = 0;


            decimal loanInstallment = 0;
            decimal interestInstallment = 0;

            decimal currentInterestCharge = 0;
            decimal totalInterestCharge = 0;
            decimal totalReceivable = 0;

            string message = string.Empty;
            DateTime tDate;

            if (string.IsNullOrEmpty(loanId))
                return Json(new { LoanInstallment = 0, InterestInstallment = 0, CurrentInterestCharge = 0, InterestCharge = 0, status = "nok", message = "Please enter valid employee." }, JsonRequestBehavior.AllowGet);

            try
            {
                //Additional
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);
                if (!processLog.IsOpen)
                    return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

                var orgSetup = orgSetupService.GetMany(x => x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (orgSetup == null)
                    return Json(new { message = "Setup Organization first." }, JsonRequestBehavior.AllowGet);

                if ((!string.IsNullOrEmpty(loanId) && !string.IsNullOrEmpty(amount)))
                {
                    long collectionId = 0;
                    long lId = Convert.ToInt64(loanId);
                    decimal amt = Convert.ToDecimal(amount);
                    GetLoanInfoByLoanId(collectionId, lId, amt, processLog.StartDate, out todaysPrinCollBeforeDayEnd, out todaysIntCollBeforeDayEnd, out loanInstallment, out interestInstallment, out currentInterestCharge, out totalInterestCharge, out totalReceivable, out message);

                    if (!string.IsNullOrEmpty(message))
                        return Json(new { LoanInstallment = loanInstallment, InterestInstallment = interestInstallment, CurrentInterestCharge = currentInterestCharge, InterestCharge = totalInterestCharge, status = "nok", message = message }, JsonRequestBehavior.AllowGet);
                    return Json(new { LoanInstallment = Math.Round(loanInstallment, 2), InterestInstallment = Math.Round(interestInstallment, 2), CurrentInterestCharge = Math.Round(currentInterestCharge, 2), InterestCharge = Math.Ceiling(totalInterestCharge), status = "ok", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "nok", message = "Sorry for inconvenience! please try again later." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { LoanInstallment = loanInstallment, InterestInstallment = interestInstallment, CurrentInterestCharge = currentInterestCharge, TotalInterestCharge = totalInterestCharge, status = "nok", message = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDisburseAndTodaysCollectionDetail(string loanId)
        {
            decimal todaysPrinCollBeforeDayEnd = 0;
            decimal todaysIntCollBeforeDayEnd = 0;
            decimal loanInstallment = 0;
            decimal interestInstallment = 0;
            decimal currentInterestCharge = 0;
            decimal totalInterestCharge = 0;
            decimal totalReceivable = 0;
            string message = string.Empty;
            long lnId = Convert.ToInt64(loanId);

            var processLog = processLogService.GetLastProcessLog();
            var transactionDate = processLog.StartDate;

            var objLoanInfo = new LoanCollectionViewModel();

            try
            {
                if (lnId <= 0)
                    return Json(new { data = objLoanInfo, message = "Warning, Employee Loan not found. Please try again!" }, JsonRequestBehavior.AllowGet);

                long collectionId = 0;
                decimal amt = 0;

                //get loan info by loanid
                GetLoanInfoByLoanId(collectionId, lnId, amt, transactionDate, out todaysPrinCollBeforeDayEnd, out todaysIntCollBeforeDayEnd, out loanInstallment, out interestInstallment, out currentInterestCharge, out totalInterestCharge, out totalReceivable, out message);

                //get disbursement and todays collection details by loan id
                objLoanInfo = GetDisburseAndTodaysCollectionDetail(lnId);

                if (objLoanInfo != null)
                {
                    objLoanInfo.InterestDue = (Math.Ceiling(totalInterestCharge - objLoanInfo.TodaysInterestCollectionAmount)).ToString();
                    objLoanInfo.TotalDue = (Convert.ToDecimal(objLoanInfo.PrincipalDue) + Convert.ToDecimal(objLoanInfo.InterestDue)).ToString();
                }

                return Json(new { data = objLoanInfo, message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = objLoanInfo, message = "Sorry for inconvenience! failed fetching loan information" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDisburseAndTodaysCollectionUptoDetail(long loanId, string interestUptoOn = "")
        {
            decimal todaysPrinCollBeforeDayEnd = 0;
            decimal todaysIntCollBeforeDayEnd = 0;
            decimal loanInstallment = 0;
            decimal interestInstallment = 0;
            decimal currentInterestCharge = 0;
            decimal totalInterestCharge = 0;
            decimal totalReceivable = 0;
            decimal interestBalance = 0;
            string message = string.Empty;

            int totalInstallment = 0;
            int totalComplete = 0;
            int totalDue = 0;
            string currentStatus = "";

            var processLog = processLogService.GetLastProcessLog();
            var transactionDate = processLog.StartDate;

            if(!string.IsNullOrWhiteSpace(interestUptoOn))
                transactionDate = Convert.ToDateTime(interestUptoOn);

            var objLoanInfo = new LoanCollectionViewModel();

            try
            {
                if (loanId <= 0)
                    return Json(new { data = objLoanInfo, message = "Warning, Employee Loan not found. Please try again!" }, JsonRequestBehavior.AllowGet);

                long collectionId = 0;
                decimal amt = 0;

                //get loan info by loanid
                GetUptoLoanInfoByLoanId(collectionId, loanId, amt, transactionDate,
                    out todaysPrinCollBeforeDayEnd, out todaysIntCollBeforeDayEnd,
                    out loanInstallment, out interestInstallment, out currentInterestCharge,
                    out totalInterestCharge, out totalReceivable,out interestBalance, out message,
                    out totalInstallment ,out totalComplete, out totalDue, out currentStatus
                    );

                //get disbursement and todays collection details by loan id
                objLoanInfo = GetDisburseAndTodaysCollectionDetail(loanId);

                if (objLoanInfo != null)
                {
                    objLoanInfo.InterestDue = (Math.Ceiling(interestBalance)).ToString();//(Math.Ceiling(totalInterestCharge - objLoanInfo.TodaysInterestCollectionAmount)).ToString();
                    objLoanInfo.TotalDue = (Convert.ToDecimal(objLoanInfo.PrincipalDue) + Convert.ToDecimal(objLoanInfo.InterestDue)).ToString(); //Convert.ToDecimal(objLoanInfo.InterestDue)).ToString();
                    objLoanInfo.CurrentInterestCharge = (Math.Ceiling(currentInterestCharge)).ToString();
                    objLoanInfo.TotalDueAmountUpto = (Math.Ceiling(totalReceivable)).ToString();

                    objLoanInfo.TotalInstallmentNo = totalInstallment;
                    objLoanInfo.TotalComplete = totalComplete;
                    objLoanInfo.TotalNoDue = totalDue;
                    objLoanInfo.CurrentStatus = currentStatus;

                    //out totalInstallment ,out totalComplete, out totalDue, out currentStatus
                }

                return Json(new { data = objLoanInfo, message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = objLoanInfo, message = "Sorry for inconvenience! failed fetching loan information" }, JsonRequestBehavior.AllowGet);
            }
        }

        private LoanCollectionViewModel GetDisburseAndTodaysCollectionDetail(long loanId)
        {
            var param = new
            {
                LoanId = loanId
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetDisburseAndTodaysCollectionDetail");

            LoanCollectionViewModel model = new LoanCollectionViewModel();

            model.LoanId = val.Tables[0].AsEnumerable().Select(row => row.Field<long>("LoanId")).FirstOrDefault().ToString(); //objDisburseInfo.LoanId;
            model.DisburseAmount = Math.Round(val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("DisburseAmount")).FirstOrDefault()).ToString();
            model.IntersetRate = Math.Round(val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("IntersetRate")).FirstOrDefault(), 2).ToString();
            model.NoOfInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("NoOfInstallment")).FirstOrDefault().ToString();

            model.TodaysLoanCollectionAmount = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysLoanCollectionAmount")).FirstOrDefault();
            model.TodaysInterestCollectionAmount = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysInterestCollectionAmount")).FirstOrDefault();

            model.MonthlyInstallment = Math.Ceiling(val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("MonthlyInstallment")).FirstOrDefault()).ToString();
            model.LoanPaid = (Math.Round(val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("LoanPaid")).FirstOrDefault() + val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysLoanCollectionAmount")).FirstOrDefault())).ToString();
            model.InterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestCharge")).FirstOrDefault().ToString();
            model.InterestPaid = (Math.Round((val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestPaid")).FirstOrDefault() + val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysInterestCollectionAmount")).FirstOrDefault()), 2)).ToString();

            model.DisburseDate = val.Tables[0].AsEnumerable().Select(row => row.Field<DateTime>("DisburseDate")).FirstOrDefault().ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
            model.PrincipalDue = (Math.Round((val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("DisburseAmount")).FirstOrDefault() -
                                 (val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("LoanPaid")).FirstOrDefault() + val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysLoanCollectionAmount")).FirstOrDefault())))).ToString();
            return model;
        }

        public JsonResult GetAdjustableLoanInfoByLoanId(string collectionId, string loanId, string amount, string transDate)
        {
            //Newly Added
            decimal todaysPrinCollBeforeDayEnd = 0;
            decimal todaysIntCollBeforeDayEnd = 0;

            decimal loanInstallment = 0;
            decimal interestInstallment = 0;

            decimal currentInterestCharge = 0;
            decimal totalInterestCharge = 0;
            decimal totalReceivable = 0;
            string message = string.Empty;
            DateTime tDate;

            if (string.IsNullOrEmpty(loanId))
                return Json(new { LoanInstallment = string.Empty, InterestInstallment = string.Empty, CurrentInterestCharge = string.Empty, InterestCharge = string.Empty, status = "nok", message = "Please enter valid employee." }, JsonRequestBehavior.AllowGet);

            try
            {
                tDate = Convert.ToDateTime(transDate);
            }
            catch
            {
                return Json(new { LoanInstallment = string.Empty, InterestInstallment = string.Empty, CurrentInterestCharge = string.Empty, InterestCharge = string.Empty, status = "nok", message = "Please enter valid employee." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if ((!string.IsNullOrEmpty(loanId) && !string.IsNullOrEmpty(amount)))
                {
                    long collId = Convert.ToInt64(collectionId);
                    long lId = Convert.ToInt64(loanId);
                    decimal amt = Convert.ToDecimal(amount);

                    GetLoanInfoByLoanId(collId, lId, amt, tDate, out todaysPrinCollBeforeDayEnd, out todaysIntCollBeforeDayEnd, out loanInstallment, out interestInstallment, out currentInterestCharge, out totalInterestCharge, out totalReceivable, out message);

                    if (!string.IsNullOrEmpty(message))
                        return Json(new { LoanInstallment = loanInstallment, InterestInstallment = interestInstallment, CurrentInterestCharge = currentInterestCharge, InterestCharge = totalInterestCharge, status = "nok", message = message }, JsonRequestBehavior.AllowGet);
                    return Json(new { LoanInstallment = Math.Round(loanInstallment, 2), InterestInstallment = Math.Round(interestInstallment, 2), CurrentInterestCharge = Math.Round(currentInterestCharge, 2), InterestCharge = Math.Floor(totalInterestCharge), status = "ok", message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "nok", message = "Sorry for inconvenience! please try again later." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { LoanInstallment = loanInstallment, InterestInstallment = interestInstallment, CurrentInterestCharge = currentInterestCharge, TotalInterestCharge = totalInterestCharge, status = "nok", message = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnpaidLoanTypeByEmployeeId(long employeeId)
        {
            try
            {
                var unpaidLoanType = loanDisbService.GetMany(x => x.EmployeeId == employeeId && x.IsDeleted == false && x.IsInstallmentOver == false).Select(x => x.LoanTypeId);

                //New Code on 30.01.2018
                var loanTypes = loanTypeService.GetMany(x => x.IsDeleted == false && unpaidLoanType.Contains(x.LoanTypeId));
                var loanTypeItems = new List<SelectListItem>();
                loanTypeItems.AddRange(loanTypes.Select(x => new SelectListItem
                {
                    Value = x.LoanTypeId.ToString(),
                    Text = x.LoanTypeName
                }));

                return Json(loanTypeItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods
        private void MapDropDownListAsTransCat(LoanCollectionViewModel model)
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

        private void GetCustomDayStatus(LoanCollectionViewModel model)
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

        private void SaveLoan(Collection objCollection, int loanId, decimal amount, decimal interestCharge)
        {
            var param = new
            {
                LoanId = loanId,
                EmployeeId = objCollection.EmployeeId,
                Amount = amount,
                TransactionType = objCollection.TransactionType,
                TransactionDate = objCollection.TransactionDate,
                CollectionTypeId = objCollection.CollectionTypeId,
                Comments = objCollection.Comments,
                CreateUser = objCollection.CreateUser,
                CreateDate = objCollection.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_SaveLoanCollection");
        }

        private void UpdateLoan(Collection objCollection)
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

            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_UpdateLoanCollection");
        }

        private void GetUptoLoanInfoByLoanId(long collectionId, long loanId, decimal amount, DateTime transDate
            , out decimal todaysPrinCollBeforeDayEnd
            , out decimal todaysIntCollBeforeDayEnd
            , out decimal loanInstallment
            , out decimal interestInstallment
            , out decimal currentInterestCharge
            , out decimal totalInterestCharge
            , out decimal totalReceivable
            , out decimal interestBalance
            , out string message
            , out int totalInstallment
            , out int totalComplete
            , out int totalDue
            , out string currentStatus
            )
        {
            var param = new
            {
                CollectionId = collectionId,
                LoanId = loanId,
                Amount = amount,
                TransDate = transDate
            };

            var val = employeeSPService.GetDataWithParameter(param, "[gcpf].[PF_GetUptoAdjustableLoanBalanceByLoanId]");
            todaysPrinCollBeforeDayEnd = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysPrinCollBeforeDayEnd")).FirstOrDefault();
            todaysIntCollBeforeDayEnd = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysIntCollBeforeDayEnd")).FirstOrDefault();
            loanInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("LoanInstallment")).FirstOrDefault();
            interestInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestInstallment")).FirstOrDefault();
            currentInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("CurrentInterestCharge")).FirstOrDefault();
            totalInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalInterestCharge")).FirstOrDefault();
            totalReceivable = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalReceivable")).FirstOrDefault();
            interestBalance = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestBalance")).FirstOrDefault();

            totalInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("TotalInstallment")).FirstOrDefault();
            totalComplete = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("TotalComplete")).FirstOrDefault();
            totalDue = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("TotalDue")).FirstOrDefault();
            currentStatus = val.Tables[0].AsEnumerable().Select(row => row.Field<string>("CurrentStatus")).FirstOrDefault();

            message = val.Tables[0].AsEnumerable().Select(row => row.Field<string>("Msg")).FirstOrDefault();
        }

        private void GetLoanInfoByLoanId(long collectionId, long loanId, decimal amount, DateTime transDate, 
            out decimal todaysPrinCollBeforeDayEnd, out decimal todaysIntCollBeforeDayEnd, 
            out decimal loanInstallment, out decimal interestInstallment, 
            out decimal currentInterestCharge, out decimal totalInterestCharge, 
            out decimal totalReceivable, out string message)
        {
            var param = new
            {
                CollectionId = collectionId,
                LoanId = loanId,
                Amount = amount,
                TransDate = transDate
            };

            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetAdjustableLoanBalanceByLoanId");
            todaysPrinCollBeforeDayEnd = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysPrinCollBeforeDayEnd")).FirstOrDefault();
            todaysIntCollBeforeDayEnd = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TodaysIntCollBeforeDayEnd")).FirstOrDefault();
            loanInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("LoanInstallment")).FirstOrDefault();
            interestInstallment = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestInstallment")).FirstOrDefault();
            currentInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("CurrentInterestCharge")).FirstOrDefault();
            totalInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalInterestCharge")).FirstOrDefault();
            totalReceivable = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalReceivable")).FirstOrDefault();
            
            message = val.Tables[0].AsEnumerable().Select(row => row.Field<string>("Msg")).FirstOrDefault();
        }
        private void MapDropDownList(LoanCollectionViewModel model)
        {
            var loanTypes = loanTypeService.GetMany(x => x.IsDeleted == false);
            var loanTypeDataItems = loanTypes.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.LoanTypeId.ToString(),
                Text = x.LoanTypeName
            });
            var loanTypeItems = new List<SelectListItem>();
            loanTypeItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            loanTypeItems.AddRange(loanTypeDataItems);
            model.LoanTypeList = loanTypeItems;
        }

        #endregion

    }
}
