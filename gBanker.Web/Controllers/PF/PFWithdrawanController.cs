
#region Usings

using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.StoreProcedure;
using System.Globalization;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFWithdrawanController : BaseController
    {
        #region Private Members
        private readonly IProcessLogService processLogService;
        private readonly IPFWithdrawanService withdrawanService;
        //private readonly ILoanDisbursementService loanDisbService;
        private readonly IEmployeeSPService employeeSPService;
        #endregion

        #region Ctor
        public PFWithdrawanController(IProcessLogService processLogService, IPFWithdrawanService withdrawanService,
            //ILoanDisbursementService loanDisbService,
             IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.withdrawanService = withdrawanService;
           // this.loanDisbService = loanDisbService;
            this.employeeSPService = employeeSPService;
        }
        #endregion
        
        #region Listings
        public ActionResult Index()
        {
            var model = new PFWithdrawanViewModel();
            try
            {
                GetCustomDayStatus(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        public JsonResult GetPFWithdrawanList(string WithdrawanId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                jtStartIndex = jtStartIndex > 0 ? jtStartIndex : 1;

                var param = new { PageNumber = jtStartIndex, PageSize = jtPageSize };
                var objWithdrawan = employeeSPService.GetDataWithParameter(param, "[gcpf].[PFWithdrawan_GetWithdrawanInfo]");

                var listings = objWithdrawan.Tables[0].AsEnumerable()
                .Select(row => new PFWithdrawanViewModel
                {
                    TotalCount = row.Field<int>("TotalCount"),
                    WithdrawanId = row.Field<long>("WithdrawanId").ToString(),
                    EmployeeId = row.Field<long>("EmployeeId").ToString(),
                    SelfContribution = row.Field<decimal>("SelfContribution"),
                    OrgContribution = row.Field<decimal>("OrgContribution"),
                    SelfInterestAmount = row.Field<decimal>("SelfInterestAmount").ToString(),
                    OrgInterestAmount = row.Field<decimal>("OrgInterestAmount").ToString(),
                    WithdrawnDate = row.Field<DateTime>("WithdrawnDate").ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                }).ToList();

                int totalCount = 0;
                if (listings.Any())
                    totalCount = listings[0].TotalCount;

                return Json(new { Result = "OK", Records = listings, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
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
            var model = new PFWithdrawanViewModel();
            try
            {
                GetCustomDayStatus(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        public JsonResult Withdraw(string employeeId, string calculationDate)
        {
            var objPFWithdrawan = new PFWithdrawan();
            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please check day end process log" });

                if (!processLog.IsOpen)
                    return Json(new { message = "Day is not open" });
                               
                var empId = Convert.ToInt64(employeeId);
                var isExistPFWithdrawan = withdrawanService.IsExistPFWithdrawan(new PFWithdrawan { EmployeeId = empId });

                if (isExistPFWithdrawan)                
                    return Json(new { message = "Fund has been withdrawn already" }, JsonRequestBehavior.AllowGet);
                
                WithdrawPF(empId, Convert.ToDateTime(calculationDate), processLog.StartDate, Convert.ToInt64(LoggedInEmployeeId.ToString()), DateTime.Now);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience, please try again later." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Withdrawn successfull" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Others
        public JsonResult GetPFWithdrawanInfo(string employeeId)
        {
            var processLog = processLogService.GetLastProcessLog();
            if (processLog == null)
                return Json(new { message = "Please check day end process log" });

            if (!processLog.IsOpen)
                return Json(new { message = "Day is not open" });

            long empId = Convert.ToInt64(employeeId);
            decimal selfContribution;
            decimal orgContribution;
            decimal selfInterestAmount;
            decimal orgInterestAmount;
            decimal totalPayable;
            DateTime calculationdate = DateTime.Now;

            //Loan related
            decimal currentInterestCharge = 0;
            decimal totalInterestCharge = 0;
            decimal totalReceivable = 0;
            string message = string.Empty;
            long collectionId = 0;
            try
            {
                //Contribution + Interest Information
                WithdrawanInfo(empId, calculationdate, out selfContribution, out orgContribution, out selfInterestAmount, out orgInterestAmount);
                if (selfContribution + orgContribution == 0)
                    return Json(new { SelfContribution = selfContribution.ToString(), OrgContribution = orgContribution.ToString(), SelfInterestAmount = selfInterestAmount.ToString(), OrgInterestAmount = orgInterestAmount.ToString(), message = "Does not have any contribution" }, JsonRequestBehavior.AllowGet);
                totalPayable = selfContribution + orgContribution + selfInterestAmount + orgInterestAmount;

                //Loan Information
                //var disburseInfo = loanDisbService.GetAll().Where(x => x.IsDeleted == false && x.EmployeeId == empId && x.IsInstallmentOver == false && x.LoanTypeId == 1).FirstOrDefault();
                //if (disburseInfo != null)
                //{
                //    GetLoanInfoByLoanId(collectionId, disburseInfo.LoanId, 0, processLog.StartDate, out currentInterestCharge, out totalInterestCharge, out totalReceivable, out message);
                //}
                return Json(new { SelfContribution = Math.Round(selfContribution, 2), OrgContribution = Math.Round(orgContribution, 2), SelfInterestAmount = Math.Round(selfInterestAmount, 2), OrgInterestAmount = Math.Round(orgInterestAmount, 2), TotalPayable = Math.Round(totalPayable, 2), LoanDue = Math.Round(totalReceivable, 2), message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Unable to fetch record " }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetWithdrawnInfo(string employeeId, string calculationDate)
        {
            PFWithdrawanViewModel model = new PFWithdrawanViewModel();

            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please check day end process status" });
                if (!processLog.IsOpen)
                    return Json(new { message = "Day is not open" });
                if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(calculationDate))
                    return Json(new { model = model, message = "Please enter valid information" });

                model = GetPFWithdrawnInfo(Convert.ToInt64(employeeId), Convert.ToDateTime(calculationDate));
                return Json(new { model = model, message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = model, message = "Unable to fetch record " }, JsonRequestBehavior.AllowGet);
            }
        }
        public PFWithdrawanViewModel GetPFWithdrawnInfo(long employeeId, DateTime calculationDate)
        {
            var param = new
            {
                EmployeeId = employeeId,
                @CalculationDate = calculationDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetWithdrawnInfo");

            PFWithdrawanViewModel model = new PFWithdrawanViewModel();

            model.SelfContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("SelfContribution")).FirstOrDefault();
            model.OrgContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgContribution")).FirstOrDefault();
            model.Contribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("Contribution")).FirstOrDefault();

            model.SelfInterestUptoInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("SelfInterestUptoInterim")).FirstOrDefault();
            model.OrgInterestUptoInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgInterestUptoInterim")).FirstOrDefault();
            model.InterestUptoInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestUptoInterim")).FirstOrDefault();

            model.SelfInterestAftInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("SelfInterestAftInterim")).FirstOrDefault();
            model.OrgInterestAftInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgInterestAftInterim")).FirstOrDefault();
            model.InterestAftInterim = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestAftInterim")).FirstOrDefault();

            model.LoanId = val.Tables[0].AsEnumerable().Select(row => row.Field<Int64>("LoanId")).FirstOrDefault();
            model.PrincipalBalance = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("PrincipalBalance")).FirstOrDefault();
            model.InterestBalance = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestBalance")).FirstOrDefault();
            model.OutStanding = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OutStanding")).FirstOrDefault();

            model.InterestIncome = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("InterestIncome")).FirstOrDefault();
            model.Fund = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("Fund")).FirstOrDefault();
            model.Payable = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("Payable")).FirstOrDefault();

            return model;
        }

        public JsonResult IsFinalized(string employeeId)
        {
            bool isFinalized = true;
            string message = string.Empty;

            try
            {
                var processLog = processLogService.GetLastProcessLog();
                if (processLog == null)
                    return Json(new { message = "Please check day end process log" });

                if (!processLog.IsOpen)
                    return Json(new { message = "Day is not open" });

                var param = new
                {
                    EmployeeId = Convert.ToInt64(employeeId)
                };
                var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_IsFinalized");
                isFinalized = val.Tables[0].AsEnumerable().Select(row => row.Field<bool>("IsFinalized")).FirstOrDefault();
                if (isFinalized == true)
                    message = "Already Finalized";
                else
                    message = string.Empty;

            }
            catch (Exception ex)
            {
                return Json(new { status = false, isFinalized = isFinalized, message = "Sorry for inconvenience, please try again later." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = true, isFinalized = isFinalized, message = message }, JsonRequestBehavior.AllowGet);
        }


        public void WithdrawPF(Int64 employeeId, DateTime calculationDate,
         DateTime transactionDate, Int64 createUser, DateTime createDate)
        {
            var param = new
            {
                EmployeeId = employeeId,
                CalculationDate = calculationDate,
                CreateUser = createUser,
                CreateDate = createDate,
                Message = string.Empty
            };

            //let's insert into [gcpf.PFWithdrawan]
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_WithdrawPF");
        }
        #endregion

        #region Private Methods

        private void WithdrawanInfo(long employeeId, DateTime calculationDate, out decimal selfContribution, out decimal orgContribution, out decimal selfInterestAmount, out decimal orgInterestAmount)
        {
            selfContribution = 0;
            orgContribution = 0;
            selfInterestAmount = 0;
            orgInterestAmount = 0;

            var param = new
            {
                EmployeeId = employeeId,
                @CalculationDate = calculationDate
            };

            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetPFWithdrawanInfo");
            selfContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("SelfContribution")).FirstOrDefault();
            orgContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgContribution")).FirstOrDefault();
            selfInterestAmount = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("SelfInterestAmount")).FirstOrDefault();
            orgInterestAmount = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgInterestAmount")).FirstOrDefault();
        }

        private void GetLoanInfoByLoanId(long collectionId, long loanId, decimal amount, DateTime transDate, out decimal currentInterestCharge, out decimal totalInterestCharge, out decimal totalReceivable, out string message)
        {
            var param = new
            {
                CollectionId = collectionId,
                LoanId = loanId,
                Amount = amount,
                TransDate = transDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetAdjustableLoanBalanceByLoanId");
            currentInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("CurrentInterestCharge")).FirstOrDefault();
            totalInterestCharge = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalInterestCharge")).FirstOrDefault();
            totalReceivable = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("TotalReceivable")).FirstOrDefault();
            message = val.Tables[0].AsEnumerable().Select(row => row.Field<string>("Msg")).FirstOrDefault();
        }

        private void GetCustomDayStatus(PFWithdrawanViewModel model)
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

        #endregion
    }
}
