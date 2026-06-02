
#region Usings

using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service;
using gHRM.Service.PF;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFLoanDisbursementController : BaseController
    {
        #region Private Variables

        private readonly IProcessLogService processLogService;
       // private readonly ILoanDisbursementService loanDisbService;
        private readonly ILoanTypeService loanTypeService;
        private readonly IEmployeeService employeeService;
        private readonly ITransactionCategoryService transCategoryService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly int transactionCategoryId = 5; 

        #endregion

        #region Ctor 
        public PFLoanDisbursementController(IProcessLogService processLogService, 
            //ILoanDisbursementService loanDisbService, 
            ILoanTypeService loanTypeService,
            IEmployeeService employeeService,
            ITransactionCategoryService transCategoryService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            //this.loanDisbService = loanDisbService;
            this.loanTypeService = loanTypeService;
            this.employeeService = employeeService;
            this.transCategoryService = transCategoryService;
            this.employeeSPService = employeeSPService;
        }
        #endregion
        
        #region Listings

        public ActionResult Index()
        {
            var model = new LoanDisbursementViewModel();
            try
            {
                GetCustomDayStatus(model);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public JsonResult GetDisbursementList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string employeeCode = "", string fromDate = "", string toDate = "", string loanState = "")
        {
            try
            {
                jtStartIndex = jtStartIndex > 0 ? jtStartIndex : 1;

                DateTime? fDate = null;
                DateTime? tDate = null;
                if (!string.IsNullOrEmpty(fromDate))
                    fDate = Convert.ToDateTime(fromDate).Date;
                if (!string.IsNullOrEmpty(toDate))
                    tDate = Convert.ToDateTime(toDate).Date;

                var loanDisbursements = new List<LoanDisbursement>();

                var param = new { EmployeeCode = employeeCode, FromDate = fDate, ToDate = tDate, LoanState = loanState, PageNumber=jtStartIndex,  PageSize = jtPageSize };
                var objDisbursement = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetRunningLoanDisbursement");

                var disbursement =
                            objDisbursement.Tables[0].AsEnumerable().Select(p => new LoanDisbursementViewModel()
                            {
                                TotalCount = p.Field<int>("TotalCount"),
                                LoanId = p.Field<long>("LoanId"),
                                EmployeeId = p.Field<long>("EmployeeId").ToString(),
                                EmployeeCode = p.Field<string>("EmployeeCode").ToString(),
                                EmployeeName = p.Field<string>("EmployeeName"),

                                DisburseAmount = Math.Round(p.Field<decimal>("DisburseAmount"), 2).ToString(),
                                IntersetRate = Math.Round(p.Field<decimal>("IntersetRate"), 2).ToString(),
                                NoOfInstallment = p.Field<int>("NoOfInstallment").ToString(),
                                MonthlyInstallment = Math.Round(p.Field<decimal>("MonthlyInstallment"), 0).ToString(),

                                DisburseDate = Convert.ToDateTime(p.Field<DateTime>("DisburseDate")).ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture), // p.Field<DateTime>("DisburseDate").ToString(),

                                LoanPaid = p.Field<decimal>("LoanPaid") == 0 ? "0" : Math.Round(p.Field<decimal>("LoanPaid"), 2).ToString(),
                                InterestPaid = p.Field<decimal>("InterestPaid") == 0 ? "0" : Math.Round(p.Field<decimal>("InterestPaid"), 2).ToString(),
                                InterestCharge = p.Field<decimal>("InterestCharge") == 0 ? "0" : Math.Round(p.Field<decimal>("InterestCharge"), 2).ToString(),

                                LastInstallmentDate = Convert.ToDateTime(p.Field<DateTime>("LastInstallmentDate")).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture), //p.Field<DateTime>("LastInstallmentDate").ToString(),
                                IsInstallmentOver = p.Field<bool>("IsInstallmentOver")

                            });                

                var currentPageRecords = disbursement.ToList();//.Skip(jtStartIndex).Take(jtPageSize);
                var totalCount = 0;
                if (currentPageRecords.Any())
                    totalCount = currentPageRecords[0].TotalCount;

                return this.Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            LoanDisbursementViewModel model = new LoanDisbursementViewModel();
            try
            {
                GetCustomDayStatus(model);
                MapDropDownList(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        public JsonResult SaveLoanDisbursement(string employeeId, string loanTypeId, string disburseAmount, string intersetRate, string noOfInstallment, string monthlyInstallment)
        {            
            var objDisbursement = new LoanDisbursement();

            try
            {
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day closed, please open day" }, JsonRequestBehavior.AllowGet);

                //Check employee either EXIST or NOT
                var objEmployee = employeeService.GetById(Convert.ToInt32(employeeId));
                if (objEmployee == null)
                    return Json(new { message = "Employee does not exist" }, JsonRequestBehavior.AllowGet);

                //Disbursement Object
                //objDisbursement.LoanTypeId = Convert.ToInt32(loanTypeId);

                decimal monthlyInstall;

                bool success = GetInstallmentInfo(disburseAmount, intersetRate, noOfInstallment, out monthlyInstall);
                if (!success)
                    return Json(new { message = "Enter valid data" }, JsonRequestBehavior.AllowGet);

                //objDisbursement.EmployeeId = Convert.ToInt64(employeeId);
                //objDisbursement.DisburseAmount = Convert.ToDecimal(disburseAmount);
                //objDisbursement.IntersetRate = Convert.ToDecimal(intersetRate);
                //objDisbursement.NoOfInstallment = Convert.ToInt32(noOfInstallment);

                //objDisbursement.MonthlyInstallment = monthlyInstall;
                //objDisbursement.DisburseDate = processLog.StartDate;
                //objDisbursement.LoanPaid = 0;
                //objDisbursement.InterestPaid = 0;
                //objDisbursement.InterestCharge = 0;
                //objDisbursement.LastInstallmentDate = processLog.StartDate;
                //objDisbursement.IsInstallmentOver = false;

                //objDisbursement.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objDisbursement.CreateDate = DateTime.Now;

                //let's insert into [gcpf.LoanDisbursement ]
                SaveDisbursement(objDisbursement);
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
            var model = new LoanDisbursementViewModel();

            try
            {
                GetCustomDayStatus(model);
                //var disbursement = loanDisbService.GetById(id);
                //if (disbursement != null)
                //{
                //    model.LoanId = id;
                //    model.EmployeeId = disbursement.EmployeeId.ToString();
                //    //model.LoanTypeId = disbursement.LoanTypeId;
                //    //model.DisburseAmount = Math.Round(disbursement.DisburseAmount, 2).ToString();
                //    model.IntersetRate = Math.Round(disbursement.IntersetRate, 2).ToString();
                //    model.NoOfInstallment = disbursement.NoOfInstallment.ToString();
                //    //model.MonthlyInstallment = Math.Round(disbursement.MonthlyInstallment, 2).ToString();

                //    var objEmployee = employeeService.GetById(Convert.ToInt32(model.EmployeeId));
                //    model.EmployeeCode = objEmployee.EmployeeCode;

                //    //Getting Loan Percentage
                //    //var loanType = loanTypeService.GetById(disbursement.LoanTypeId);
                //    //if (loanType != null)
                //    //{
                //    //    decimal contributionAmount = GetContributionByEmpId(disbursement.EmployeeId);
                //    //    decimal? maxLoanLimit = 0;
                //    //    maxLoanLimit = loanType.LoanPercentage * contributionAmount / 100;
                //    //    maxLoanLimit = (maxLoanLimit == null) ? 0 : maxLoanLimit;

                //    //    model.MaxLoanLimit = maxLoanLimit.HasValue ? Decimal.Round(maxLoanLimit.Value, 2).ToString() : "0"; // maxLoanLimit.ToString();

                //    //    //Getting Employee Name
                //    //    var employee = employeeService.GetById(Convert.ToInt32(disbursement.EmployeeId));
                //    //    if (employee != null)
                //    //        model.EmployeeName = employee.EmployeeName;

                //    //    //Getting Finished Loan
                //    //    var disburse = loanDisbService.GetAll().Where(x => x.EmployeeId == disbursement.EmployeeId && x.LoanTypeId == disbursement.LoanTypeId && x.IsDeleted == false);
                //    //    int finishedLoan = disburse.Where(x => x.IsInstallmentOver == true).Count();
                //    //    model.FinishedLoan = finishedLoan.ToString();
                //    //    MapDropDownList(model);
                //    //}
                //}
            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        public JsonResult UpdateLoanDisbursement(string loanId, string employeeId)
        {
            LoanDisbursement objDisbursement = new LoanDisbursement();
            try
            {
                //Check employee either EXIST or NOT
                var objEmployee = employeeService.GetById(Convert.ToInt32(employeeId));
                if (objEmployee == null)
                    return Json(new { message = "Employee does not exist" }, JsonRequestBehavior.AllowGet);

                //Disbursement Object               
                //objDisbursement = loanDisbService.GetById(Convert.ToInt32(loanId));
                //if (objDisbursement == null)
                //    return Json(new { message = "Loan does not exist" }, JsonRequestBehavior.AllowGet);

                objDisbursement.LoanId = objDisbursement.LoanId;
                objDisbursement.EmployeeId = Convert.ToInt64(employeeId);
               // objDisbursement.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objDisbursement.UpdateDate = DateTime.Now;

                UpdateDisbursement(objDisbursement);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Others Methods 

        public JsonResult GetLoanInfoByEmpId(string employeeId, string loanTypeId)
        {
            int interestTypeId = 0;
            decimal interestRate = 0;
            string employeeName = string.Empty;
            string employeeExit = string.Empty;
            int finishedLoan = 0;
            int runningLoan = 0;
            decimal? maxLoanLimit = 0;

            try
            {
                GetLoanStatusByEmpId(employeeId, loanTypeId, out interestTypeId, out interestRate, out employeeExit, out employeeName, out finishedLoan, out runningLoan, out maxLoanLimit);
                maxLoanLimit = maxLoanLimit.HasValue ? Math.Round(maxLoanLimit.Value, 2) : 0;
            }
            catch (Exception ex)
            {
                return Json(new { InterestTypeId = interestTypeId, InterestRate = interestRate, FinishedLoan = finishedLoan, RunningLoan = runningLoan, MaxLoanLimit = maxLoanLimit, EmployeeExit = "Sorry for inconvenience!", EmployeeName = employeeName }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { InterestTypeId = interestTypeId, InterestRate = interestRate, FinishedLoan = finishedLoan, RunningLoan = runningLoan, MaxLoanLimit = maxLoanLimit, EmployeeExit = employeeExit, EmployeeName = employeeName }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private void GetLoanStatusByEmpId(string employeeId, string loanTypeId, out int interestTypeId, out decimal interestRate, out string employeeExit, out string employeeName, out int finishedLoan, out int runningLoan, out decimal? maxLLimit)
        {
            employeeName = string.Empty;
            //bool status = true;

            interestTypeId = 0;
            interestRate = 0;
            employeeExit = string.Empty;
            finishedLoan = 0;
            runningLoan = 0;
            maxLLimit = 0;


            try
            {
                int empId = Convert.ToInt32(employeeId);
                int loanTyId = Convert.ToInt32(loanTypeId);

                var objEmpConf = employeeService.GetById(empId);
                if (objEmpConf != null)
                {
                    employeeName = objEmpConf.EmployeeName;
                    employeeExit = "Yes";
                }

               // var disburse = loanDisbService.GetLoanDisburseInfoByEmployeeId(Convert.ToInt64(employeeId), loanTyId);

                //finishedLoan = disburse.Where(x => x.IsInstallmentOver == true).Count();  //2
                //runningLoan = disburse.Where(x => x.IsInstallmentOver == false).Count();  //3

                var loanType = loanTypeService.GetLoanTypeLoanTypeId(loanTyId);
                if (loanType != null)
                {
                    decimal? maxLoanLimit = 0;
                    maxLoanLimit = loanType.LoanPercentage * GetContributionByEmpId(empId) / 100;

                    maxLLimit = (maxLoanLimit == null) ? 0 : maxLoanLimit;
                    interestTypeId = loanType.InterestRateTypeId;
                    interestRate = loanType.InterestRate;
                }

            }
            catch (Exception ex)
            {

            }

        }

        private void UpdateDisbursement(LoanDisbursement objDisbursement)
        {
            var param = new
            {
                LoanId = objDisbursement.LoanId,
                EmployeeId = objDisbursement.EmployeeId,
                //UpdateUser = objDisbursement.UpdateUser,
                UpdateDate = objDisbursement.UpdateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_UpdateLoanDisbursement");
        }

        private void SaveDisbursement(LoanDisbursement objDisbursement)
        {
            var param = new
            {
                EmployeeId = objDisbursement.EmployeeId,
              //  LoanTypeId = objDisbursement.LoanTypeId,
                DisburseAmount = objDisbursement.DisburseAmount,
                IntersetRate = objDisbursement.IntersetRate,
                NoOfInstallment = objDisbursement.NoOfInstallment,
                MonthlyInstallment = objDisbursement.MonthlyInstallment,
                DisburseDate = objDisbursement.DisburseDate,

                LoanPaid = objDisbursement.LoanPaid,
                InterestPaid = objDisbursement.InterestPaid,
                InterestCharge = objDisbursement.InterestCharge,
                LastInstallmentDate = objDisbursement.LastInstallmentDate,
                IsInstallmentOver = objDisbursement.IsInstallmentOver,

                //CreateUser = objDisbursement.CreateUser,
                CreateDate = objDisbursement.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_SaveLoanDisbursement");
        }

        private decimal GetContributionByEmpId(long employeeId)
        {
            var param = new
            {
                EmployeeId = employeeId
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetContributionByEmpId");
            var result = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("ContributionAmount")).SingleOrDefault();

            return result;
        }

        private bool GetInstallmentInfo(string disburseAmount, string intersetRate, string noOfInstallment, out decimal monthlyInstallment)
        {
            bool result = true;
            monthlyInstallment = 0;
            try
            {
                decimal amount = Convert.ToDecimal(disburseAmount);
                decimal rate = Convert.ToDecimal(intersetRate);
                int noOfInstall = Convert.ToInt32(noOfInstallment);
                monthlyInstallment = Math.Ceiling((amount) / noOfInstall);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public JsonResult GetInstallmentInfo(string disburseAmount, string intersetRate, string noOfInstallment)
        {
            string employeeName = string.Empty;
            try
            {
                decimal monthlyInstall;
                bool success = GetInstallmentInfo(disburseAmount, intersetRate, noOfInstallment, out monthlyInstall);
                if (!success)
                {
                    return Json(new { MonthlyInstallment = 0, Success = "No" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MonthlyInstallment = Math.Round(monthlyInstall, 2), Success = "Yes" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MonthlyInstallment = 0, Success = "No" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropDownList(LoanDisbursementViewModel model)
        {
            var loanTypes = loanTypeService.GetAll().Where(x => x.IsDeleted == false);
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

        private void GetCustomDayStatus(LoanDisbursementViewModel model)
        {
            var objProcessLog = processLogService.GetCustomDayStatus();
            if (objProcessLog != null)
            {
                model.IsOpen = objProcessLog.IsOpen;
                model.DayStatus = objProcessLog.DayStatus;
                model.TransactionDate = (objProcessLog.TransactionDateString);
                model.SystemDate = objProcessLog.SystemDate;
            }
        }

        #endregion
    }
}
