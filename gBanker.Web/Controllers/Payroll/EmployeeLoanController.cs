
#region Usings

using CrystalDecisions.Shared;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Loan;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Owin.Security.Provider;
using OfficeOpenXml.VBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class EmployeeLoanController : BaseController
    {
        #region Private Members

        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeLoanRegisterService employeeLoanRegisterService;
        private readonly IEmployeeLoanInstallmentDetailService employeeLoanInstallmentDetailService;
        private readonly ILoanInstallmentDetailService loanInstallmentDetailService;

        #endregion

        #region Ctor

        public EmployeeLoanController(
            IEmployeeSPService employeeSPService,
            IEmployeeService employeeService,
            IPRComponentService prComponentService,
            IEmployeeLoanRegisterService employeeLoanRegisterService,
            IEmployeeLoanInstallmentDetailService employeeLoanInstallmentDetailService,
            ILoanInstallmentDetailService loanInstallmentDetailService)
        {
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.prComponentService = prComponentService;
            this.employeeLoanRegisterService = employeeLoanRegisterService;
            this.employeeLoanInstallmentDetailService = employeeLoanInstallmentDetailService;
            this.loanInstallmentDetailService = loanInstallmentDetailService;
        }

        #endregion

        #region Edit

        public ActionResult EmployeeShortLoanEdit(int LoanId)
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();
            var entity = loanInstallmentDetailService.Get(x => x.Id == LoanId);

            if (entity == null || entity.LoanStatus == LoanStatusConstants.Closed)
                return RedirectToAction("EmployeeLoanIndex");

            var empCode = employeeService.GetById(Convert.ToInt32(entity.EmployeeId)).EmployeeCode;
            model.LoanId = entity.Id;
            model.LoanStatus = entity.LoanStatus;
            model.LoanTypeId = entity.PRComponentId;
            model.TotalLoanAmt = entity.LoanDisburseAmount;
            model.LoanStartDateMsg = entity.DisburseDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.InstallmentAmount = entity.InstallmentAmount;
            model.InstallmentDateMsg = entity.InstallmentStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.LoanEndDateMsg = entity.InstallmentEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            model.EmployeeCode = empCode;

            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            var viewList = new List<SelectListItem>();
            viewList.Add(pleaseSelect);
            model.LoanTypeList = viewList;            

            return View(model);
        }

        [HttpPost]
        public JsonResult EditEmployeeShortLoanInfo(EmployeeLoanInstallmentDetailViewModel LoanObject)
        {
            var result = 0;
            var message = "";
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //get Loan Installment Detail from [prl.LoanInstallmentDetail]
                    var loanInfo = loanInstallmentDetailService.GetById(LoanObject.LoanId);

                    loanInfo.LoanDisburseAmount = LoanObject.TotalAmount;
                    loanInfo.DisburseDate = LoanObject.LoanStartDate;
                    loanInfo.LoanStatus = LoanObject.LoanStatus;
                    loanInfo.InstallmentAmount = LoanObject.InstallmentAmount;
                    loanInfo.InstallmentStartDate = LoanObject.InstallmentDate;
                    loanInfo.InstallmentEndDate = LoanObject.LoanEndDate;
                    loanInfo.UpdateBy = Convert.ToInt32(LoggedInEmployeeId);
                    loanInfo.UpdateDate = DateTime.UtcNow;

                    //let's update [prl.LoanInstallmentDetail]
                    loanInstallmentDetailService.Update(loanInfo);
                    scope.Complete();
                    result = 1;
                    message = "Data Saved Successfully";

                }
                catch (Exception e)
                {
                    result = 0;
                    message = "Error occured, Save denied";
                    scope.Dispose();
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeLoanCreate()
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();
            mapDropDown(model);
            return View(model);
        }

        public ActionResult EmployeeShortLoanCreate()
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();
            mapDropDown(model);
            return View(model);
        }


        public ActionResult EmployeeShortLoanCreate2()
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();
            mapDropDown(model);
            return View(model);
        }


        public ActionResult EmployeeLoanIndex()
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();

            return View(model);
        }

        public ActionResult EmployeeLoanIndex2()
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();

            return View(model);
        }
        public JsonResult GetEmployeeInfoByCodeEdit(int LoanId, string employeeCode)
        {
            var model = new EmployeeLoanInstallmentDetailViewModel();
            var entity = loanInstallmentDetailService.Get(x => x.Id == LoanId);

           
            var empCode = employeeService.GetById(Convert.ToInt32(entity.EmployeeId)).EmployeeCode;
            model.LoanId = entity.Id;
            model.LoanStatus = entity.LoanStatus;
            model.LoanTypeId = entity.PRComponentId;
            model.TotalLoanAmt = entity.LoanDisburseAmount;
            model.LoanStartDateMsg = entity.DisburseDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.InstallmentAmount = entity.InstallmentAmount;
            model.InstallmentDateMsg = entity.InstallmentStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.LoanEndDateMsg = entity.InstallmentEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            model.EmployeeCode = empCode;

            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            var viewList = new List<SelectListItem>();
            viewList.Add(pleaseSelect);
            model.LoanTypeList = viewList;


            var employeeInfo = new EmployeeLoanInstallmentDetailViewModel();
            ///var employee = employeeService.GetByCode(employeeCode.Trim());
            gHRMDBContext db = new gHRMDBContext();
            var employee =
                (from emp in db.Employees
                 join of in db.Offices on emp.OfficeId equals of.OfficeId
                 where emp.EmployeeCode == employeeCode.Trim()
                 select new { emp.EmployeeCode, emp.EmployeeId, of.OfficeLocationId, emp.EmployeeStatusId, emp.EmployeeTypeId }
                 ).FirstOrDefault();

            var result = 0;
            if (employee != null)
            {
                var empId = employee.EmployeeId;
                var status = employee.EmployeeStatusId;
                var employeementType = employee.EmployeeTypeId;
                var OfficeLocationId = employee.OfficeLocationId;
               
                //var loanComponent = prComponentService.GetMany(x => x.IsActive == true && x.ComponentCategory.Trim() == "Loan" && x.EmployeeStatusId == status && x.EmployeeTypeId == employeementType);
                var loanComponent = prComponentService.GetMany(x =>x.PRComponentID == model.LoanTypeId);
                var loanType = loanComponent.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.ComponentName,
                    Value = row.PRComponentID.ToString()
                }).ToList();
                viewList.Add(pleaseSelect);
                viewList.AddRange(loanType);


                var param = new { EmployeeCode = employee.EmployeeCode };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
                if (empOffcDesigList != null)
                {
                    employeeInfo.EmployeeId = Convert.ToInt64(empOffcDesigList.Tables[0].Rows[0]["EmployeeId"]);
                    employeeInfo.EmployeeCode = empOffcDesigList.Tables[0].Rows[0]["EmployeeCode"].ToString();
                    employeeInfo.EmployeeName = empOffcDesigList.Tables[0].Rows[0]["EmployeeName"].ToString();
                    employeeInfo.OfficeName = empOffcDesigList.Tables[0].Rows[0]["OfficeName"].ToString();
                    employeeInfo.DepartmentName = empOffcDesigList.Tables[0].Rows[0]["DepartmentName"].ToString();
                    employeeInfo.DesignationName = empOffcDesigList.Tables[0].Rows[0]["DesignationName"].ToString();
                }
                result = 1;
                employeeInfo.LoanComponentList = viewList;
            }
            else
            {
                result = 0;
            }
            return Json(new { result = result, data = employeeInfo }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeInfoByCode(string employeeCode)
        {
            var employeeInfo = new EmployeeLoanInstallmentDetailViewModel();
            ///var employee = employeeService.GetByCode(employeeCode.Trim());
            gHRMDBContext db = new gHRMDBContext();
            var employee =
                (from emp in db.Employees
                 join of in db.Offices on emp.OfficeId equals of.OfficeId
                 where emp.EmployeeCode == employeeCode.Trim()
                 select new { emp.EmployeeCode, emp.EmployeeId, of.OfficeLocationId, emp.EmployeeStatusId, emp.EmployeeTypeId }
                 ).FirstOrDefault();

            var result = 0;
            if (employee != null)
            {
                var empId = employee.EmployeeId;
                var status = employee.EmployeeStatusId;
                var employeementType = employee.EmployeeTypeId;
                var OfficeLocationId = employee.OfficeLocationId;
                var viewList = new List<SelectListItem>();
                var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
                //var loanComponent = prComponentService.GetMany(x => x.IsActive == true && x.ComponentCategory.Trim() == "Loan" && x.EmployeeStatusId == status && x.EmployeeTypeId == employeementType);
                var loanComponent = prComponentService.GetMany(x => x.IsActive == true && x.ComponentCategory.Trim() == "Loan" && x.EmployeeStatusId == status && x.EmployeeTypeId == employeementType && x.OfficeLocationId == OfficeLocationId && x.EffectiveStartDate<=DateTime.Now && x.EffectiveEndDate>= DateTime.Now );
                var loanType = loanComponent.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.ComponentName,
                    Value = row.PRComponentID.ToString()
                }).ToList();
                viewList.Add(pleaseSelect);
                viewList.AddRange(loanType);


                var param = new { EmployeeCode = employee.EmployeeCode };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
                if (empOffcDesigList != null)
                {
                    employeeInfo.EmployeeId = Convert.ToInt64(empOffcDesigList.Tables[0].Rows[0]["EmployeeId"]);
                    employeeInfo.EmployeeCode = empOffcDesigList.Tables[0].Rows[0]["EmployeeCode"].ToString();
                    employeeInfo.EmployeeName = empOffcDesigList.Tables[0].Rows[0]["EmployeeName"].ToString();
                    employeeInfo.OfficeName = empOffcDesigList.Tables[0].Rows[0]["OfficeName"].ToString();
                    employeeInfo.DepartmentName = empOffcDesigList.Tables[0].Rows[0]["DepartmentName"].ToString();
                    employeeInfo.DesignationName = empOffcDesigList.Tables[0].Rows[0]["DesignationName"].ToString();
                }
                result = 1;
                employeeInfo.LoanComponentList = viewList;
            }
            else
            {
                result = 0;
            }
            return Json(new { result = result, data = employeeInfo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeInfoByCode2(string employeeCode)
        {
            var employeeInfo = new EmployeeLoanInstallmentDetailViewModel();
            ///var employee = employeeService.GetByCode(employeeCode.Trim());
            gHRMDBContext db = new gHRMDBContext();
            var employee =
                (from emp in db.Employees
                 join of in db.Offices on emp.OfficeId equals of.OfficeId
                 where emp.EmployeeCode == employeeCode.Trim()
                 select new { emp.EmployeeCode, emp.EmployeeId, of.OfficeLocationId, emp.EmployeeStatusId, emp.EmployeeTypeId }
                 ).FirstOrDefault();

            var result = 0;
            if (employee != null)
            {
                var empId = employee.EmployeeId;
                var status = employee.EmployeeStatusId;
                var employeementType = employee.EmployeeTypeId;
                var OfficeLocationId = employee.OfficeLocationId;
                var viewList = new List<SelectListItem>();
                var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
                //var loanComponent = prComponentService.GetMany(x => x.IsActive == true && x.ComponentCategory.Trim() == "Loan" && x.EmployeeStatusId == status && x.EmployeeTypeId == employeementType);
                var loanComponent = prComponentService.GetMany(x => x.IsActive == true && x.ComponentCategory.Trim() == "Loan" && x.EmployeeStatusId == status  && x.OfficeLocationId == OfficeLocationId && x.EffectiveStartDate <= DateTime.Now && x.EffectiveEndDate >= DateTime.Now);  // /&& x.EmployeeTypeId == employeementType 
                var loanType = loanComponent.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.ComponentName,
                    Value = row.PRComponentID.ToString()
                }).ToList();
                viewList.Add(pleaseSelect);
                viewList.AddRange(loanType);


                var param = new { EmployeeCode = employee.EmployeeCode };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
                if (empOffcDesigList != null)
                {
                    employeeInfo.EmployeeId = Convert.ToInt64(empOffcDesigList.Tables[0].Rows[0]["EmployeeId"]);
                    employeeInfo.EmployeeCode = empOffcDesigList.Tables[0].Rows[0]["EmployeeCode"].ToString();
                    employeeInfo.EmployeeName = empOffcDesigList.Tables[0].Rows[0]["EmployeeName"].ToString();
                    employeeInfo.OfficeName = empOffcDesigList.Tables[0].Rows[0]["OfficeName"].ToString();
                    employeeInfo.DepartmentName = empOffcDesigList.Tables[0].Rows[0]["DepartmentName"].ToString();
                    employeeInfo.DesignationName = empOffcDesigList.Tables[0].Rows[0]["DesignationName"].ToString();
                }
                result = 1;
                employeeInfo.LoanComponentList = viewList;
            }
            else
            {
                result = 0;
            }
            return Json(new { result = result, data = employeeInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveEmployeeLoanInfo(EmployeeLoanInstallmentDetailViewModel LoanObject)
        {
            var result = 0;
            var message = "";

            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var loanNo = 1;
                var checkMaster = employeeLoanRegisterService.GetAll().ToList();
                if (checkMaster.Any())
                {
                    var maxLoanNo = checkMaster.Max(p => p.LoanId);
                    loanNo = maxLoanNo + 1;
                }
                if (LoanObject != null)
                {
                    var entity = new EmployeeLoanRegister();
                    entity.LoanId = loanNo;
                    entity.EmployeeId = LoanObject.EmployeeId;
                    entity.PRComponentId = LoanObject.PRComponentId;
                    entity.TotalAmount = LoanObject.TotalAmount;
                    entity.LoanOpening = LoanObject.LoanOpening;
                    entity.InterestRate = LoanObject.InterestRate;
                    entity.NoOfInstallMent = LoanObject.NoOfInstallMent;
                    entity.YearTotal = LoanObject.YearTotal;
                    entity.LoanStartDate = LoanObject.LoanStartDate;
                    entity.LoanClosingDate = LoanObject.LoanEndDate;
                    entity.LoanType = LoanObject.LoanType;
                    entity.IsActive = true;
                    entity.CreatedBy = Convert.ToInt64(LoggedInEmployeeId);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);
                    var loanId = employeeLoanRegisterService.Create(entity).LoanId;

                    if (loanId > 0)
                    {
                        var installmentList = new List<EmployeeLoanInstallmentDetail>();
                        var remainingInstallment = LoanObject.RestNoOfInstallMent;
                        var interval = LoanObject.InstallmentInterval;
                        entity.LoanStartDate = entity.LoanStartDate.AddMonths(-1);
                        for (int i = 1; i <= remainingInstallment; i++)
                        {

                            var scheduleDate = entity.LoanStartDate.AddMonths(i * LoanObject.InstallmentInterval);
                            var loan = new EmployeeLoanInstallmentDetail();
                            loan.LoanId = loanId;
                            loan.InstallmentDate = scheduleDate;
                            loan.InstallmentAmount = LoanObject.InstallmentAmount;
                            loan.IsActive = true;
                            loan.IsInstallmentPaid = false;
                            loan.EmployeeId = LoanObject.EmployeeId;
                            loan.PRComponentId = LoanObject.PRComponentId;
                            loan.EndingBalance = 0;
                            loan.PrincipalAmount = 0;
                            loan.InterestAmount = 0;
                            loan.ApprovalStatus = "A";
                            loan.CreatedBy = Convert.ToInt32(LoggedInEmployeeId);
                            loan.CreateDate = DateTime.UtcNow;
                            loan.UpdateDate = DateTime.UtcNow;
                            loan.UpdatedBy = Convert.ToInt32(LoggedInEmployeeId);
                            installmentList.Add(loan);
                        }
                        employeeLoanInstallmentDetailService.AddEmployeeLoanInstallmentDetail(installmentList);
                        //scope.Complete();
                        result = 1;
                        message = "Data Saved Successfully";
                    }
                }
                else
                {
                    result = 0;
                    message = "Error occured, Save denied";
                    //scope.Dispose();
                }

            }
            catch (Exception e)
            {
                result = 0;
                message = "Error occured, Save denied";
                //scope.Dispose();
            }
            //}
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> SaveEmployeeLoanInfo2(EmployeeLoanInstallmentDetailViewModel LoanObject)
        {
            var result = 0;
            var message = "";

            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var loanNo = 1;
                var checkMaster = employeeLoanRegisterService.GetAll().ToList();
                if (checkMaster.Any())
                {
                    var maxLoanNo = checkMaster.Max(p => p.LoanId);
                    loanNo = maxLoanNo + 1;
                }
                if (LoanObject != null)
                {
                    var entity = new EmployeeLoanRegister();
                    entity.LoanId = loanNo;
                    entity.EmployeeId = LoanObject.EmployeeId;
                    entity.PRComponentId = LoanObject.PRComponentId;
                    entity.TotalAmount = LoanObject.TotalAmount;
                    entity.LoanOpening = LoanObject.LoanOpening;
                    entity.InterestRate = LoanObject.InterestRate;
                    entity.NoOfInstallMent = LoanObject.NoOfInstallMent;
                    entity.YearTotal = LoanObject.YearTotal;
                    entity.LoanStartDate = LoanObject.LoanStartDate;
                    entity.LoanClosingDate = LoanObject.LoanEndDate;
                    entity.LoanType = LoanObject.LoanType;
                    entity.IsActive = true;
                    entity.CreatedBy = Convert.ToInt64(LoggedInEmployeeId);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);
                    var loanId = employeeLoanRegisterService.Create(entity).LoanId;

                    if (loanId > 0)
                    {
                        var installmentList = new List<EmployeeLoanInstallmentDetail>();
                        var remainingInstallment = LoanObject.RestNoOfInstallMent;
                        var interval = LoanObject.InstallmentInterval;
                        entity.LoanStartDate = entity.LoanStartDate.AddMonths(-1);
                        for (int i = 1; i <= remainingInstallment; i++)
                        {

                            var scheduleDate = entity.LoanStartDate.AddMonths(i * LoanObject.InstallmentInterval);
                            var loan = new EmployeeLoanInstallmentDetail();
                            loan.LoanId = loanId;
                            loan.InstallmentDate = scheduleDate;
                            loan.InstallmentAmount = LoanObject.InstallmentAmount;
                            loan.IsActive = true;
                            loan.IsInstallmentPaid = false;
                            loan.EmployeeId = LoanObject.EmployeeId;
                            loan.PRComponentId = LoanObject.PRComponentId;
                            loan.EndingBalance = 0;
                            loan.PrincipalAmount = 0;
                            loan.InterestAmount = 0;
                            loan.ApprovalStatus = "A";
                            loan.CreatedBy = Convert.ToInt32(LoggedInEmployeeId);
                            loan.CreateDate = DateTime.UtcNow;
                            loan.UpdateDate = DateTime.UtcNow;
                            loan.UpdatedBy = Convert.ToInt32(LoggedInEmployeeId);
                            installmentList.Add(loan);
                        }
                        employeeLoanInstallmentDetailService.AddEmployeeLoanInstallmentDetail(installmentList);
                        //scope.Complete();
                        result = 1;
                        message = "Data Saved Successfully";
                    }
                }
                else
                {
                    result = 0;
                    message = "Error occured, Save denied";
                    //scope.Dispose();
                }

            }
            catch (Exception e)
            {
                result = 0;
                message = "Error occured, Save denied";
                //scope.Dispose();
            }
            //}
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveEmployeeShortLoanInfo(EmployeeLoanInstallmentDetailViewModel LoanObject)
        {
            var result = 0;
            var message ="Success, Employee Loan Saved!";
            bool isOperationSuccess = true;
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var isExistRunningLoan = await employeeLoanInstallmentDetailService.IsExistRunningLoan((int)LoanObject.EmployeeId);

                    var isExistRunningLoan2 = await employeeLoanInstallmentDetailService.IsExistRunningLoan2((int)LoanObject.EmployeeId, LoanObject.PRComponentId );

                    if (isExistRunningLoan)
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "NGF" || SessionHelper.CompanyInfo.CompanyShortName == "GMPF")
                        {

                            if(isExistRunningLoan2)
                            {
                                return Json(new { result = 0, message = "Warning, There was a running loanee. Please closed and try again!" }, JsonRequestBehavior.AllowGet);
                            }

                            //Populate loan installment detail
                            var newLoanInstallmentDetails2 = PopulateLoanInstallmentDetail(LoanObject);

                            //let's create loan info
                            loanInstallmentDetailService.Create(newLoanInstallmentDetails2);

                            isOperationSuccess = true;
                        }
                        else
                        {
                            return Json(new { result = 0, message = "Warning, There was a running loanee. Please closed and try again!" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {

                        //Populate loan installment detail
                        var newLoanInstallmentDetails = PopulateLoanInstallmentDetail(LoanObject);

                        //let's create loan info
                        loanInstallmentDetailService.Create(newLoanInstallmentDetails);

                        var model = new UpdatePreviousLoanAsClosedModel
                        {
                            EmployeeId = (int)LoanObject.EmployeeId,
                            LoanInstallmentDetailId = newLoanInstallmentDetails.Id,
                            PreviousLoanStatus = LoanStatusConstants.Running,
                            NewLoanStatus = LoanStatusConstants.Closed
                        };

                        var response = await loanInstallmentDetailService.UpdatePreviousLoanAsClosed(model);
                        if (!response.IsSuccess) isOperationSuccess = false;
                    }
                }
                catch (Exception e)
                {
                    isOperationSuccess = false;
                    result = 0;
                    message = "Error occured, Save denied";
                }

                if (isOperationSuccess) 
                {
                    result = 1;
                    ts.Complete();
                }
                
                ts.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }        

        public ActionResult getLoanDashboard([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();

                int Result = 0;

                var loanList = employeeSPService.GetDataWithoutParameter("prl.SP_EmployeeLoanDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    TotalLoanAmt = row.Field<decimal>("LoanDisburseAmount"),
                    LoanStartDateMsg = row.Field<string>("DisburseDate"),
                    InstallmentAmount = row.Field<decimal>("InstallmentAmount"),
                    InstallmentDateMsg = row.Field<string>("InstallmentStartDate"),
                    LoanEndDateMsg = row.Field<string>("InstallmentEndDate"),
                    LoanStatus = row.Field<string>("LoanStatus")
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult CalculateLoan()
        {
            var result = 0;
            try
            {
                var paramProcess = new { ProcessDate = DateTime.Now };
                employeeSPService.GetDataWithParameter(paramProcess, "PF_LoanCalculation_SalaryProcess");

                //var param = new { ProcessDate = DateTime.Now };
                //employeeSPService.GetDataWithParameter(param, "SP_EmployeeLoanDetailList");
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods
        private LoanInstallmentDetail PopulateLoanInstallmentDetail(EmployeeLoanInstallmentDetailViewModel LoanObject)
        {
            var newLoanInstallmentDetail = new LoanInstallmentDetail
            {
                EmployeeId = LoanObject.EmployeeId,
                PRComponentId = LoanObject.PRComponentId,
                LoanDisburseAmount = LoanObject.TotalAmount,
                DisburseDate = LoanObject.LoanStartDate,
                InstallmentAmount = LoanObject.InstallmentAmount,
                InstallmentStartDate = LoanObject.InstallmentDate,
                InstallmentEndDate = LoanObject.LoanEndDate,
                IsActive = true,
                LoanStatus = LoanStatusConstants.Running,
                CreateBy = Convert.ToInt32(LoggedInEmployeeId),
                UpdateBy = Convert.ToInt32(LoggedInEmployeeId),
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            return newLoanInstallmentDetail;
        }
        private void mapDropDown(EmployeeLoanInstallmentDetailViewModel model)
        {
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };

            var installmentIntervalList = new List<SelectListItem>();
            installmentIntervalList.Add(pleaseSelect);
            installmentIntervalList.Add(new SelectListItem() { Text = "1 Month", Value = "1" });
            installmentIntervalList.Add(new SelectListItem() { Text = "4 Months", Value = "4" });
            installmentIntervalList.Add(new SelectListItem() { Text = "6 months", Value = "6" });
            installmentIntervalList.Add(new SelectListItem() { Text = "1 Year", Value = "12" });
            model.InstallmentIntervalList = installmentIntervalList;

            var viewList = new List<SelectListItem>();
            viewList.Add(pleaseSelect);
            model.LoanTypeList = viewList;

            var loanSchemeList = new List<SelectListItem>();
            loanSchemeList.Add(pleaseSelect);
            loanSchemeList.Add(new SelectListItem() { Text = "Amortization", Value = "A" });
            loanSchemeList.Add(new SelectListItem() { Text = "Decline", Value = "D" });
            loanSchemeList.Add(new SelectListItem() { Text = "Flat", Value = "F" });
            loanSchemeList.Add(new SelectListItem() { Text = "Classic", Value = "C" });
            model.LoanSchemeList = loanSchemeList;

        }

        #endregion
    }
}