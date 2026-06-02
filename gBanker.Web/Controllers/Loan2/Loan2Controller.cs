using AutoMapper;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Loan;
using gHRM.Service.Loan.LoanCalculationService;
using gHRM.Service.Payroll;
using gHRM.Web.ViewModels.Loan;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using static Utility.Constants;

namespace gHRM.Web.Controllers.Loan
{
    public class Loan2Controller : BaseController
    {
        #region Private Variables
        private readonly IEmployeeService employeeService;
        private readonly ILoanDisbursementService disbursementService;
        private readonly IApplicantInfoService applicantInfoService;
        private readonly IApplicantInfoService3 applicantInfoService3;
        private readonly IApprovalMasterService approvalMasterService;
        private readonly IApproveDetailService approveDetailService;
        private readonly IPRComponentService prComponentService;
        private readonly ILoanEligibilityService loanEligibilityService;
        private readonly ILoanPurposeService loanPurposeService;
        #endregion Private Variables
        #region Ctor
        public Loan2Controller
            (IEmployeeService employeeService, ILoanDisbursementService disbursementService, IApplicantInfoService applicantInfoService
            , IApprovalMasterService approvalMasterService, IApproveDetailService approveDetailService, IPRComponentService prComponentService
            , ILoanEligibilityService loanEligibilityService, ILoanPurposeService loanPurposeService
           // , IApplicantInfoService3 applicantInfoService3
            )
        {
            this.employeeService = employeeService;
            this.disbursementService = disbursementService;
            this.applicantInfoService = applicantInfoService;          
            this.approvalMasterService = approvalMasterService;
            this.approveDetailService = approveDetailService;
            this.prComponentService = prComponentService;
            this.loanPurposeService = loanPurposeService;
            this.loanEligibilityService = loanEligibilityService;
          //  this.applicantInfoService3 = applicantInfoService3;
        }
        #endregion Ctor

        #region Action Methods
        // GET: Loan
        [HttpGet]
        public ActionResult LoanApplication()
        {
            ApplicantInfoViewModel model = new ApplicantInfoViewModel();
            try
            {
                model.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
                model.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
                model.PurposeLst = new LoanConfigCommonDropdown().TypeXPropose("PFL", 0);
                var emp = employeeService.GetByEmpId((LoggedInEmployeeId ?? 0));
                model.EmployeeCode = emp.EmployeeCode;
                model.EmployeeId = emp.EmployeeId;
                model.EmployeeName = emp.EmployeeName;

                var disburse = disbursementService.GetMany(x => (x.IsDeleted ?? false) == false && x.EmployeeId == model.EmployeeId && x.LoanType == "PFL");
                if (disburse.Any())
                {
                    var d = disburse.OrderByDescending(x => x.LoanId).First();
                    model.PreviousLoanID = d.LoanId;
                    model.PreviousLoanNo = d.LoanNo;
                    model.PreviousLoanAmount = d.DisburseAmount;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                model = new ApplicantInfoViewModel();
                model.LoanTypeLst = new List<SelectListItem>();
                model.PurposeLst = new List<SelectListItem>();
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult LoanEdit()
        {
            ApplicantInfoViewModel2 model = new ApplicantInfoViewModel2();
            try
            {
                model.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
                model.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
                model.PurposeLst = new LoanConfigCommonDropdown().TypeXPropose("PFL", 0);
                var emp = employeeService.GetByEmpId((LoggedInEmployeeId ?? 0));
                model.EmployeeCode = emp.EmployeeCode;
                model.EmployeeId = emp.EmployeeId;
                model.EmployeeName = emp.EmployeeName;

                var disburse = disbursementService.GetMany(x => (x.IsDeleted ?? false) == false && x.EmployeeId == model.EmployeeId && x.LoanType == "PFL");
                if (disburse.Any())
                {
                    var d = disburse.OrderByDescending(x => x.LoanId).First();
                    model.PreviousLoanID = d.LoanId;
                    model.PreviousLoanNo = d.LoanNo;
                    model.PreviousLoanAmount = d.DisburseAmount;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                model = new ApplicantInfoViewModel2();
                model.LoanTypeLst = new List<SelectListItem>();
                model.PurposeLst = new List<SelectListItem>();
                return View(model);
            }
        }


        [HttpPost]
        public JsonResult PostLoanEdit(ApplicantInfoViewModel2 model)
        {
            try
            {


                int result = 0;
                string msg = "";

                var param = new
                {
                    EmployeeId = model.EmployeeId,
                    LoanId = model.Id,
                    InterestRate = model.InterestRate,
                    DisburseAmount = model.LoanAmount,
                    InstallmentNo = model.InstallmentNo,
                    InstallmentPrincipal = model.InstallmentPrincipal,
                    InstallmentInterest = model.InstallmentInterest,
                    MonthlyInstallment = model.InstallmentAmount,
                    InterestAmount = model.InterestAmount,
                    OpeningCollection = model.PreviousLoanAmount,
                    OpeningInt = model.MaxLoanAmount,
                    DisburseDate = model.DisburseDate,
                };

                var rr = employeeService.GetDataWithParameter(param, "loan.SP_UPDATE_LOANDATA");

                return Json(new { Result = 1, Message = "Update sucessfull " });
                                   
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public ActionResult LoanApplication2()
        {
            ApplicantInfoViewModel model = new ApplicantInfoViewModel();
            try
            {
                model.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
                model.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
                model.PurposeLst = new LoanConfigCommonDropdown().TypeXPropose("PFL", 0);
                var emp = employeeService.GetByEmpId((LoggedInEmployeeId ?? 0));
                model.EmployeeCode = emp.EmployeeCode;
                model.EmployeeId = emp.EmployeeId;
                model.EmployeeName = emp.EmployeeName;

                var disburse = disbursementService.GetMany(x => (x.IsDeleted ?? false) == false && x.EmployeeId == model.EmployeeId && x.LoanType == "PFL");
                if (disburse.Any())
                {
                    var d = disburse.OrderByDescending(x => x.LoanId).First();
                    model.PreviousLoanID = d.LoanId;
                    model.PreviousLoanNo = d.LoanNo;
                    model.PreviousLoanAmount = d.DisburseAmount;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                model = new ApplicantInfoViewModel();
                model.LoanTypeLst = new List<SelectListItem>();
                model.PurposeLst = new List<SelectListItem>();
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult LoanApplication3()
        {
            ApplicantInfoViewModel3 model = new ApplicantInfoViewModel3();
            try
            {
                model.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
                model.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
                model.PurposeLst = new LoanConfigCommonDropdown().TypeXPropose("PFL", 0);
                var emp = employeeService.GetByEmpId((LoggedInEmployeeId ?? 0));
                model.EmployeeCode = emp.EmployeeCode;
                model.EmployeeId = emp.EmployeeId;
                model.EmployeeName = emp.EmployeeName;

                var disburse = disbursementService.GetMany(x => (x.IsDeleted ?? false) == false && x.EmployeeId == model.EmployeeId && x.LoanType == "PFL");
                if (disburse.Any())
                {
                    var d = disburse.OrderByDescending(x => x.LoanId).First();
                    model.PreviousLoanID = d.LoanId;
                    model.PreviousLoanNo = d.LoanNo;
                    model.PreviousLoanAmount = d.DisburseAmount;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                model = new ApplicantInfoViewModel3();
                model.LoanTypeLst = new List<SelectListItem>();
                model.PurposeLst = new List<SelectListItem>();
                return View(model);
            }
        }


        [HttpPost]
        public JsonResult PostLoanApplication(ApplicantInfoViewModel model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else if (model.PurposeId == 0)
                msg = "Loan Purpose is required";
            else if (model.LoanAmount == 0)
                msg = "Loan Amount is required";
            else if (model.InstallmentNo == 0)
                msg = "Installment No is required";
            else if (disbursementService.GetMany(x => x.EmployeeId == model.EmployeeId && (x.IsDeleted ?? false) == false
             && x.IsClose == false && x.LoanType == model.LoanType).Any())
                msg = "Previous loan is open, please close the loan";
            else if (applicantInfoService.GetMany(x => x.EmployeeId == model.EmployeeId && x.LoanType == model.LoanType
            && x.ApplicationStatus == ApplicationStatus_const.Active).Any())
                msg = "Loan Application has been found";
            else
            {
                var dicObj = LoanEligibilityCheck((int)model.EmployeeId, model.LoanType, model.PurposeId);
                if (dicObj.Any() || dicObj == null || dicObj.Count == 0 )
                {
                    if (dicObj.FirstOrDefault(x => x.Key == "Msg").Value == "" || dicObj == null || dicObj.Count == 0)
                    {
                        try
                        {
                            int maxAmt = 0;
                            if (model.LoanType == "PFL")
                            {
                                string m = dicObj.FirstOrDefault(x => x.Key == "MaxAmt").Value;
                                if (string.IsNullOrEmpty(m)) m = "0";
                                maxAmt = Convert.ToInt32(m);
                                if (maxAmt < model.LoanAmount)
                                    msg = "";
                                        //  msg = "Your amount is not valid, check maximum amount.";
                            }

                            if (msg == "")
                            {
                                var obj = Mapper.Map<ApplicantInfoViewModel, ApplicantInfo>(model);
                                //var pur_obj=loanPurposeService.GetById(obj.PurposeId);
                                obj.NotificationStatus = NotificationStatus_const.Pending;
                                obj.ApplicationStatus = ApplicationStatus_const.Active;
                                //if(obj.GracePeriod)
                                //    obj.GracePeriod = pur_obj.GracePeriod;
                                obj.CreateBy = (int)model.EmployeeId;
                                applicantInfoService.Create(obj);
                                result = 1;
                                msg = "Save Successfully";
                            }

                        }
                        catch (Exception ex) { msg = ex.Message; }
                    }
                    else
                        msg = dicObj.FirstOrDefault(x => x.Key == "Msg").Value;
                }
                else
                    msg = "Not Eligible";

            }
            return Json(new { Result = result, Message = msg });
        }

        [HttpPost]
        public JsonResult PostLoanApplication2(ApplicantInfoViewModel model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else if (model.PurposeId == 0)
                msg = "Loan Purpose is required";
            else if (model.LoanAmount == 0)
                msg = "Loan Amount is required";
            else if (model.InstallmentNo == 0)
                msg = "Installment No is required";
            else if (disbursementService.GetMany(x => x.EmployeeId == model.EmployeeId && (x.IsDeleted ?? false) == false
             && x.IsClose == false && x.LoanType == model.LoanType && x.PurposeId == model.PurposeId ).Any())
                msg = "Previous loan is open, please close the loan";
            else if (applicantInfoService.GetMany(x => x.EmployeeId == model.EmployeeId && x.LoanType == model.LoanType && x.PurposeId == model.PurposeId
            && x.ApplicationStatus == ApplicationStatus_const.Active).Any())
                msg = "Loan Application has been found";
            else
            {
                var dicObj = LoanEligibilityCheck((int)model.EmployeeId, model.LoanType, model.PurposeId);
                if (dicObj.Any() || dicObj == null || dicObj.Count == 0)
                {
                    if (dicObj.FirstOrDefault(x => x.Key == "Msg").Value == "" || dicObj == null || dicObj.Count == 0)
                    {
                        try
                        {
                            int maxAmt = 0;
                            if (model.LoanType == "PFL")
                            {
                                string m = dicObj.FirstOrDefault(x => x.Key == "MaxAmt").Value;
                                if (string.IsNullOrEmpty(m)) m = "0";
                                maxAmt = Convert.ToInt32(m);
                                if (maxAmt < model.LoanAmount)
                                    msg = "";
                                //  msg = "Your amount is not valid, check maximum amount.";
                            }

                            if (msg == "")
                            {
                                var obj = Mapper.Map<ApplicantInfoViewModel, ApplicantInfo>(model);
                                //var pur_obj=loanPurposeService.GetById(obj.PurposeId);
                                obj.NotificationStatus = NotificationStatus_const.Pending;
                                obj.ApplicationStatus = ApplicationStatus_const.Active;
                                //if(obj.GracePeriod)
                                //    obj.GracePeriod = pur_obj.GracePeriod;
                                obj.CreateBy = (int)model.EmployeeId;
                                applicantInfoService.Create(obj);
                                result = 1;
                                msg = "Save Successfully";
                            }

                        }
                        catch (Exception ex) { msg = ex.Message; }
                    }
                    else
                        msg = dicObj.FirstOrDefault(x => x.Key == "Msg").Value;
                }
                else
                    msg = "Not Eligible";

            }
            return Json(new { Result = result, Message = msg });
        }


        [HttpPost]
        public JsonResult PostLoanApplication3(ApplicantInfoViewModel3 model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else if (model.PurposeId == 0)
                msg = "Loan Purpose is required";
            else if (model.LoanAmount == 0)
                msg = "Loan Amount is required";
            else if (model.InstallmentNo == 0)
                msg = "Installment No is required";
            else if (disbursementService.GetMany(x => x.EmployeeId == model.EmployeeId && (x.IsDeleted ?? false) == false
             && x.IsClose == false && x.LoanType == model.LoanType && x.PurposeId == model.PurposeId).Any())
                msg = "Previous loan is open, please close the loan";
            else if (applicantInfoService.GetMany(x => x.EmployeeId == model.EmployeeId && x.LoanType == model.LoanType && x.PurposeId == model.PurposeId
            && x.ApplicationStatus == ApplicationStatus_const.Active).Any())
                msg = "Loan Application has been found";
            else
            {
                var dicObj = LoanEligibilityCheck((int)model.EmployeeId, model.LoanType, model.PurposeId);
                if (dicObj.Any() || dicObj == null || dicObj.Count == 0)
                {
                    if (dicObj.FirstOrDefault(x => x.Key == "Msg").Value == "" || dicObj == null || dicObj.Count == 0)
                    {
                        try
                        {
                            int maxAmt = 0;
                            if (model.LoanType == "PFL")
                            {
                                string m = dicObj.FirstOrDefault(x => x.Key == "MaxAmt").Value;
                                if (string.IsNullOrEmpty(m)) m = "0";
                                maxAmt = Convert.ToInt32(m);
                                if (maxAmt < model.LoanAmount)
                                    msg = "";
                                //  msg = "Your amount is not valid, check maximum amount.";
                            }

                            if (msg == "")
                            {
                                var obj = Mapper.Map<ApplicantInfoViewModel3, ApplicantInfo2>(model);
                                //var pur_obj=loanPurposeService.GetById(obj.PurposeId);
                                obj.NotificationStatus = NotificationStatus_const.Pending;
                                obj.ApplicationStatus = ApplicationStatus_const.Active;
                                //if(obj.GracePeriod)
                                //    obj.GracePeriod = pur_obj.GracePeriod;
                                obj.CreateBy = (int)model.EmployeeId;
                                //applicantInfoService3.Create(obj);


                                var param = new
                                {
                                    LoanType = model.LoanType,
                                    PurposeId = model.PurposeId,
                                    EmployeeId = (int)model.EmployeeId,
                                    LoanAmount = model.LoanAmount,
                                    InstallmentNo = model.InstallmentNo,
                                    GracePeriod = model.GracePeriod,
                                    InterestRate = model.InterestRate,
                                    InterestAmount = model.InterestAmount,
                                    InstallmentPrincipal = model.InstallmentPrincipal,
                                    InstallmentInterest = model.InstallmentInterest,
                                    InstallmentAmount = model.InstallmentAmount,
                                    Remark = model.Remark, // can be null
                                    PreviousLoanID = model.PreviousLoanID, // can be null
                                    PreviousLoanAmount = model.PreviousLoanAmount, // can be null
                                    LevelPosition = 0,
                                    NotificationStatus = NotificationStatus_const.Pending,
                                    ApplicationStatus = ApplicationStatus_const.Active,
                                    ApplicationDate = model.ApplicationDate, // can be null
                                    CreateBy = (int)model.EmployeeId
                                };

                                employeeService.GetDataWithParameter(param, "loan.SP_InsertApplicantInfo");

                                result = 1;
                                msg = "Save Successfully";
                            }

                        }
                        catch (Exception ex) { msg = ex.Message; }
                    }
                    else
                        msg = dicObj.FirstOrDefault(x => x.Key == "Msg").Value;
                }
                else
                    msg = "Not Eligible";

            }
            return Json(new { Result = result, Message = msg });
        }

        [HttpGet]
        public ActionResult LoanApproval()
        {
            gHRMDBContext db = new gHRMDBContext();
            if ((from ap in db.ApprovalMasters
                 join ad in db.ApproveDetails on ap.ApprovalMasterId equals ad.ApprovalMasterId
                 where ap.IsActive && ad.IsActive && ap.FormName == NotificationStatus_const.Disburse_Approve && ad.EmployeeId == (LoggedInEmployeeId ?? 0)
                 select new { ap.ApprovalMasterId }).Any())
                return View();
            else
                return Content("You are not valid Approver");
        }

        [HttpGet]
        public ActionResult LoanApproval2()
        {
            //gHRMDBContext db = new gHRMDBContext();
            //if ((from ap in db.ApprovalMasters
            //     join ad in db.ApproveDetails on ap.ApprovalMasterId equals ad.ApprovalMasterId
            //     where ap.IsActive && ad.IsActive && ap.FormName == NotificationStatus_const.Disburse_Approve && ad.EmployeeId == (LoggedInEmployeeId ?? 0)
            //     select new { ap.ApprovalMasterId }).Any())
                return View();
            //else
            //    return Content("You are not valid Approver");
        }


        [HttpPost]
        public ActionResult PostLoanApproval(int id, string status)
        {
            int result = 0;
            string msg = "";
            if (id > 0 && !string.IsNullOrEmpty(status))
            {
                var obj = applicantInfoService.GetById(id);
                if (status == "A")
                {
                    obj.LevelPosition += 1;
                    var master = approvalMasterService.GetMany(x => x.IsActive && x.LoanType == obj.LoanType);
                    if (master.Any())
                        if (obj.LevelPosition == master.First().TotalLevel)
                            obj.NotificationStatus = NotificationStatus_const.Accounts;
                    obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                    obj.UpdateDate = DateTime.Now;

                }
                else if (status == "R")
                {
                    obj.ApplicationStatus = ApplicationStatus_const.Reject;
                    obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                    obj.UpdateDate = DateTime.Now;
                }
                applicantInfoService.Update(obj);
                msg = (status == "R" ? "Reject Completed" : "Successfuly Approved");
                result = 1;
            }
            else
                msg = "Wrong Information";
            return Json(new { Result = result, Message = msg });
        }

        [HttpPost]
        public ActionResult PostLoanApproval2(int id, string status)
        {
            int result = 0;
            string msg = "";
            if (id > 0 && !string.IsNullOrEmpty(status))
            {
                var obj = applicantInfoService.GetById(id);
                if (status == "A")
                {
                    obj.LevelPosition += 1;
                    var master = approvalMasterService.GetMany(x => x.IsActive && x.LoanType == obj.LoanType);
                    if (master.Any())
                    {
                        //if (obj.LevelPosition == master.First().TotalLevel)
                        obj.NotificationStatus = NotificationStatus_const.Accounts;
                        obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                        obj.UpdateDate = DateTime.Now;
                    }

                }
                else if (status == "R")
                {
                    obj.ApplicationStatus = ApplicationStatus_const.Reject;
                    obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                    obj.UpdateDate = DateTime.Now;
                }
                applicantInfoService.Update(obj);
                msg = (status == "R" ? "Reject Completed" : "Successfuly Approved");
                result = 1;
            }
            else
                msg = "Wrong Information";
            return Json(new { Result = result, Message = msg });
        }



        //[HttpGet]
        //public ActionResult LoanDisbursement()
        //{
        //    gHRMDBContext db = new gHRMDBContext();
        //    if ((from ap in db.ApprovalMasters
        //         join ad in db.ApproveDetails on ap.ApprovalMasterId equals ad.ApprovalMasterId
        //         where ap.IsActive && ad.IsActive && ap.FormName == NotificationStatus_const.Accounts && ad.EmployeeId == (LoggedInEmployeeId ?? 0)
        //         select new { ap.ApprovalMasterId }).Any())
        //        return View();
        //    else return Content("You are not valid user");
        //}

        [HttpGet]
        public ActionResult LoanDisbursement()
        {
            var param = new { LoggedInEmployeeId = LoggedInEmployeeId ?? 0 };

            var result = employeeService.GetDataWithParameter(param, "sp_LoanDisbursement");

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string status = result.Tables[0].Rows[0]["Result"].ToString();
                if (status == "VALID_USER")
                {
                    return View(); // Explicit return type
                }
                else
                {
                    return Content("You are not valid user"); // Explicit return type
                }
            }

            return Content("Error retrieving data.");
        }


        [HttpGet]
        public ActionResult LoanDisbursement2()
        {
            var param = new { LoggedInEmployeeId = LoggedInEmployeeId ?? 0 };

            var result = employeeService.GetDataWithParameter(param, "sp_LoanDisbursement");

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string status = result.Tables[0].Rows[0]["Result"].ToString();
                if (status == "VALID_USER")
                {
                    return View(); // Explicit return type
                }
                else
                {
                    return Content("You are not valid user"); // Explicit return type
                }
            }

            return Content("Error retrieving data.");
        }


        [HttpGet]
        public ActionResult LoanDisbursement3()
        {
            var param = new { LoggedInEmployeeId = LoggedInEmployeeId ?? 0 };

            var result = employeeService.GetDataWithParameter(param, "sp_LoanDisbursement");

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                string status = result.Tables[0].Rows[0]["Result"].ToString();
                if (status == "VALID_USER")
                {
                    return View(); // Explicit return type
                }
                else
                {
                    return Content("You are not valid user"); // Explicit return type
                }
            }

            return Content("Error retrieving data.");
        }



        [HttpPost]
        public ActionResult PostLoanDisbursement(int id, string status, string LoanNo)
        {
            int result = 0;
            string msg = "";
            if (id > 0)
            {
                if (!string.IsNullOrEmpty(LoanNo))
                {
                    if (!disbursementService.GetMany(x => x.IsDeleted == false && x.LoanNo == LoanNo).Any())
                    {
                        var obj = applicantInfoService.GetById(id);
                        if (!disbursementService.GetMany(x => x.IsDeleted == false && x.EmployeeId == obj.EmployeeId && x.LoanType == obj.LoanType && !x.IsClose).Any())
                        {

                            gHRMDBContext db = new gHRMDBContext();
                            var lstObj = (from pr in db.PRComponents
                                          join emp in db.Employees on pr.EmployeeTypeId equals emp.EmployeeTypeId
                                          join lp in db.LoanPurposes on pr.ComponentName equals lp.PurposeName
                                          where pr.ComponentCategory == "Loan" && emp.EmployeeStatusId == pr.EmployeeStatusId
                                          && pr.IsActive && emp.EmployeeId == obj.EmployeeId && lp.PurposeId == obj.PurposeId
                                          select new { pr.InterestRate, lp.MethodType });
                            if (lstObj.Any())
                            {
                                decimal interestrate = lstObj.First().InterestRate;
                                if (obj.InterestRate != interestrate)
                                    msg = "Interest Rate is not same, Please Check the configuration.";
                                else
                                {
                                    string methodtype = lstObj.First().MethodType;
                                    using (TransactionScope ts = new TransactionScope())
                                    {
                                        try
                                        {
                                            LoanDisbursement model = new LoanDisbursement()
                                            {
                                                CreateBy = (int)(LoggedInEmployeeId ?? 0),
                                                CreateDate = DateTime.UtcNow,
                                                DisburseAmount = obj.LoanAmount,
                                                DisburseDate = DateTime.Now.Date,
                                                EmployeeId = obj.EmployeeId,
                                                ApplicantId = obj.Id,
                                                MethodType = methodtype,
                                                InterestCharge = (methodtype == "D" ? 0 : methodtype == "F" ? obj.InterestAmount : 0),
                                                IntersetRate = obj.InterestRate,
                                                InstallmentInterest = obj.InstallmentInterest,
                                                InstallmentPrincipal = obj.InstallmentPrincipal,
                                                GracePeriod = obj.GracePeriod,
                                                LoanNo = LoanNo,
                                                LoanType = obj.LoanType,
                                                MonthlyInstallment = obj.InstallmentAmount,
                                                NoOfInstallment = obj.InstallmentNo,
                                                LastInstallmentDate = DateTime.Now.AddMonths(obj.InstallmentNo),
                                                PurposeId = obj.PurposeId,
                                                IsDeleted = false,
                                            };
                                            disbursementService.Create(model);

                                            obj.NotificationStatus = NotificationStatus_const.Disburse_Approve;
                                            obj.ApplicationStatus = ApplicationStatus_const.Disburse;
                                            applicantInfoService.Update(obj);
                                            ts.Complete();
                                            result = 1;
                                            msg = "Disburse has been completed";
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = ex.Message;
                                        }
                                        ts.Dispose();
                                    }
                                }
                            }
                            else msg = "Component not found";


                        }
                        else msg = (obj.LoanType == "CL" ? "Company" : obj.LoanType == "PFL" ? "PF" : obj.LoanType == "COL" ? "Co-operative" : "Other") + " Loan Found";
                    }
                    else
                        msg = "Loan no. already userd";
                }
                else
                    msg = "Loan no. is required!";
            }
            else
                msg = "Wrong Information";
            return Json(new { Result = result, Message = msg });
        }



        [HttpPost]
        public ActionResult PostLoanDisbursement2(int id, string status, string LoanNo)
        {
            int result = 0;
            string msg = "";
            if (id > 0)
            {
                if (!string.IsNullOrEmpty(LoanNo))
                {
                    if (!disbursementService.GetMany(x => x.IsDeleted == false && x.LoanNo == LoanNo).Any())
                    {
                        var obj = applicantInfoService.GetById(id);
                        if (!disbursementService.GetMany(x => x.IsDeleted == false && x.EmployeeId == obj.EmployeeId && x.LoanType == obj.LoanType && x.PurposeId == obj.PurposeId && !x.IsClose).Any())
                        {

                            gHRMDBContext db = new gHRMDBContext();
                            var lstObj = (from pr in db.PRComponents
                                          join emp in db.Employees on pr.EmployeeTypeId equals emp.EmployeeTypeId
                                          join lp in db.LoanPurposes on pr.ComponentName equals lp.PurposeName
                                          where pr.ComponentCategory == "Loan" && emp.EmployeeStatusId == pr.EmployeeStatusId
                                          && pr.IsActive && emp.EmployeeId == obj.EmployeeId && lp.PurposeId == obj.PurposeId
                                          select new { pr.InterestRate, lp.MethodType });
                            if (lstObj.Any())
                            {
                                decimal interestrate = lstObj.First().InterestRate;
                                //if (obj.InterestRate != interestrate)
                                //    msg = "Interest Rate is not same, Please Check the configuration.";
                                //else
                                //{
                                    string methodtype = lstObj.First().MethodType;
                                    using (TransactionScope ts = new TransactionScope())
                                    {
                                        try
                                        {
                                            LoanDisbursement model = new LoanDisbursement()
                                            {
                                                CreateBy = (int)(LoggedInEmployeeId ?? 0),
                                                CreateDate = DateTime.UtcNow,
                                                DisburseAmount = obj.LoanAmount,
                                                DisburseDate = DateTime.Now.Date,
                                                EmployeeId = obj.EmployeeId,
                                                ApplicantId = obj.Id,
                                                MethodType = methodtype,
                                                InterestCharge = (methodtype == "D" ? 0 : methodtype == "F" ? obj.InterestAmount : 0),
                                                IntersetRate = obj.InterestRate,
                                                InstallmentInterest = obj.InstallmentInterest,
                                                InstallmentPrincipal = obj.InstallmentPrincipal,
                                                GracePeriod = obj.GracePeriod,
                                                LoanNo = LoanNo,
                                                LoanType = obj.LoanType,
                                                MonthlyInstallment = obj.InstallmentAmount,
                                                NoOfInstallment = obj.InstallmentNo,
                                                LastInstallmentDate = DateTime.Now.AddMonths(obj.InstallmentNo),
                                                PurposeId = obj.PurposeId,
                                                IsDeleted = false,
                                            };
                                            disbursementService.Create(model);

                                            obj.NotificationStatus = NotificationStatus_const.Disburse_Approve;
                                            obj.ApplicationStatus = ApplicationStatus_const.Disburse;
                                            applicantInfoService.Update(obj);
                                            ts.Complete();
                                            result = 1;
                                            msg = "Disburse has been completed";
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = ex.Message;
                                        }
                                        ts.Dispose();
                                    }
                                //}
                            }
                            else msg = "Component not found";


                        }
                        else msg = (obj.LoanType == "CL" ? "Company" : obj.LoanType == "PFL" ? "PF" : obj.LoanType == "COL" ? "Co-operative" : "Other") + " Loan Found";
                    }
                    else
                        msg = "Loan no. already userd";
                }
                else
                    msg = "Loan no. is required!";
            }
            else
                msg = "Wrong Information";
            return Json(new { Result = result, Message = msg });
        }



        [HttpPost]
        public ActionResult PostLoanDisbursement3(int id, string status, string LoanNo, string LoanDate )
        {
            int result = 0;
            string msg = "";
            if (id > 0)
            {
                if (!string.IsNullOrEmpty(LoanNo))
                {
                    if (!disbursementService.GetMany(x => x.IsDeleted == false && x.LoanNo == LoanNo).Any())
                    {
                        var obj = applicantInfoService.GetById(id);
                        if (!disbursementService.GetMany(x => x.IsDeleted == false && x.EmployeeId == obj.EmployeeId && x.LoanType == obj.LoanType && x.PurposeId == obj.PurposeId && !x.IsClose).Any())
                        {

                            gHRMDBContext db = new gHRMDBContext();
                            var lstObj = (from pr in db.PRComponents
                                          join emp in db.Employees on pr.EmployeeTypeId equals emp.EmployeeTypeId
                                          join lp in db.LoanPurposes on pr.ComponentName equals lp.PurposeName
                                          where pr.ComponentCategory == "Loan" && emp.EmployeeStatusId == pr.EmployeeStatusId
                                          && pr.IsActive && emp.EmployeeId == obj.EmployeeId && lp.PurposeId == obj.PurposeId
                                          select new { pr.InterestRate, lp.MethodType });
                            if (lstObj.Any())
                            {
                                decimal interestrate = lstObj.First().InterestRate;
                                //if (obj.InterestRate != interestrate)
                                //    msg = "Interest Rate is not same, Please Check the configuration.";
                                //else
                                //{
                                string methodtype = lstObj.First().MethodType;
                                using (TransactionScope ts = new TransactionScope())
                                {
                                    try
                                    {
                                        LoanDisbursement model = new LoanDisbursement()
                                        {
                                            CreateBy = (int)(LoggedInEmployeeId ?? 0),
                                            CreateDate = DateTime.UtcNow,
                                            DisburseAmount = obj.LoanAmount,
                                            DisburseDate = Convert.ToDateTime(LoanDate),
                                            EmployeeId = obj.EmployeeId,
                                            ApplicantId = obj.Id,
                                            MethodType = methodtype,
                                            InterestCharge = (methodtype == "D" ? 0 : methodtype == "F" ? obj.InterestAmount : 0),
                                            IntersetRate = obj.InterestRate,
                                            InstallmentInterest = obj.InstallmentInterest,
                                            InstallmentPrincipal = obj.InstallmentPrincipal,
                                            GracePeriod = obj.GracePeriod,
                                            LoanNo = LoanNo,
                                            LoanType = obj.LoanType,
                                            MonthlyInstallment = obj.InstallmentAmount,
                                            NoOfInstallment = obj.InstallmentNo,
                                            LastInstallmentDate = DateTime.Now.AddMonths(obj.InstallmentNo),
                                            PurposeId = obj.PurposeId,
                                            IsDeleted = false,
                                        };
                                        disbursementService.Create(model);

                                        obj.NotificationStatus = NotificationStatus_const.Disburse_Approve;
                                        obj.ApplicationStatus = ApplicationStatus_const.Disburse;
                                        applicantInfoService.Update(obj);
                                        ts.Complete();
                                        result = 1;
                                        msg = "Disburse has been completed";
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = ex.Message;
                                    }
                                    ts.Dispose();
                                }
                                //}
                            }
                            else msg = "Component not found";


                        }
                        else msg = (obj.LoanType == "CL" ? "Company" : obj.LoanType == "PFL" ? "PF" : obj.LoanType == "COL" ? "Co-operative" : "Other") + " Loan Found";
                    }
                    else
                        msg = "Loan no. already userd";
                }
                else
                    msg = "Loan no. is required!";
            }
            else
                msg = "Wrong Information";
            return Json(new { Result = result, Message = msg });
        }


        [HttpGet]
        public ActionResult SpecialCollection()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoanApprovalStatus()
        {
            return View();
        }
        #endregion Action Methods

        #region    Method
        [HttpPost]
        public JsonResult PreviousLoan(string loantype, int purposeid, int? employeeid)
        {
            LoanDisbursement obj = new LoanDisbursement();
            var disburse = disbursementService.GetMany(x => x.IsDeleted == false && x.EmployeeId == (employeeid ?? (LoggedInEmployeeId ?? 0)) && x.LoanType == loantype && x.PurposeId == purposeid);
            if (disburse.Any())
                obj = disburse.OrderByDescending(x => x.LoanId).First();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]

        private Dictionary<string, string> LoanEligibilityCheck(int empid, string loanType, int? purposeid)
        {
            Dictionary<string, string> disObj = new Dictionary<string, string>();
            var objLst = loanEligibilityService.GetMany(x => x.IsActive && x.LoanType == loanType);
            if (objLst.Any())
            {
                var emp = employeeService.GetByEmpId(empid);
                var year = (decimal)((DateTime.Today - (emp.ConfirmationDate ?? DateTime.Today)).TotalDays / 365.2425);
                if (loanType == "PFL")
                {
                    var PFAmt = new gHRMDBContext().ContributionRegisters.Where(x => !(x.IsDeleted ?? false) && x.EmployeeId == emp.EmployeeId && x.TransactionDate <= DateTime.Today)
                        .GroupBy(r => 1)
                        .Select(g => new
                        {
                            SelfContribution = g.Sum(x => x.SelfContribution),
                            OrgContribution = g.Sum(x => x.OrgContribution)
                        });
                    if (PFAmt.Any())
                    {
                        if (objLst.Where(x => x.PurposeId == purposeid.Value && x.MinmumJobAge <= year && x.MaximumJobAge >= year).Any())
                        {
                            disObj.Add("Msg", "");
                            var obj = objLst.Where(x => x.PurposeId == purposeid.Value && x.MinmumJobAge <= year && x.MaximumJobAge >= year).First();
                            disObj.Add("Percentage", obj.LoanEligibleInPercent.ToString());
                            var totalAmt = (obj.PFContribution == "Self" ? PFAmt.First().SelfContribution : (PFAmt.First().OrgContribution + PFAmt.First().SelfContribution));
                            if (totalAmt > 0)
                                totalAmt = Math.Round((totalAmt * obj.LoanEligibleInPercent) / 100);
                            disObj.Add("MaxAmt", totalAmt.ToString());
                        }
                        //else if (objLst.Where(x => x.PurposeId == 0 && x.MinmumJobAge <= year && x.MaximumJobAge >= year).Any())
                        //{
                        //    disObj.Add("Msg", "");
                        //    var obj = objLst.Where(x => x.PurposeId == 0 && x.MinmumJobAge <= year && x.MaximumJobAge >= year).First();
                        //    disObj.Add("Percentage", obj.LoanEligibleInPercent.ToString());
                        //    var totalAmt = (obj.PFContribution == "Self" ? PFAmt.First().SelfContribution : (PFAmt.First().OrgContribution + PFAmt.First().SelfContribution));
                        //    if (totalAmt > 0)
                        //        totalAmt = Math.Round((totalAmt * obj.LoanEligibleInPercent) / 100);
                        //    disObj.Add("MaxAmt", totalAmt.ToString());
                        //}
                        else
                            disObj.Add("Msg", "");
                        //disObj.Add("Msg", "Not Eligible");
                    }
                    else
                        disObj.Add("Msg", "");
                    //disObj.Add("Msg", "Not Eligible");
                }
                else if (loanType == "COL")
                {
                    decimal amt = 0;
                    using (gHRMDBContext db = new gHRMDBContext())
                    {
                        var lst = (from co in db.CooperativeConfigurations
                                   join cl in db.CooperativeLedgers on co.Id equals cl.SummaryMasterId
                                   join e in db.Employees on co.EmployeeId equals e.EmployeeId
                                   where co.ActivityStatus == CoOperativeConstants.ActivityStatus_Active && co.EmployeeId == empid
                                   /* && cl.InstallmentType != CoOperativeConstants.InstallmentType_Installment_Payment && cl.InstallmentType != CoOperativeConstants.InstallmentType_Interest_Payment */
                                   select new { Amount = cl.Credit - cl.Debit }).ToList();
                        if (lst.Any())
                            amt = lst.Sum(x => x.Amount);
                    }
                    if (objLst.Where(x => x.PurposeId == purposeid.Value && x.MinmumJobAge <= year && x.MaximumJobAge >= year).Any() && amt > 0)
                    {
                        disObj.Add("Msg", "");
                        var obj = objLst.Where(x => x.PurposeId == purposeid.Value && x.MinmumJobAge <= year && x.MaximumJobAge >= year).First();
                        disObj.Add("Percentage", obj.LoanEligibleInPercent.ToString());

                        amt = Math.Round((amt * obj.LoanEligibleInPercent) / 100);
                        disObj.Add("MaxAmt", amt.ToString());
                    }
                    else
                        disObj.Add("Msg", "");
                        //disObj.Add("Msg", "Not Eligible");
                }
                else if (loanType == "CL")
                {
                    if (objLst.Where(x => x.PurposeId == purposeid.Value && x.MinmumJobAge <= year && x.MaximumJobAge >= year).Any())
                    {
                        disObj.Add("Msg", "");
                        disObj.Add("Percentage", "100");
                    }
                    else if (objLst.Where(x => x.PurposeId == 0 && x.MinmumJobAge <= year && x.MaximumJobAge >= year).Any())
                    {
                        disObj.Add("Msg", "");
                        disObj.Add("Percentage", "100");
                    }
                    else
                        disObj.Add("Msg", "");
                    //disObj.Add("Msg", "Not Eligible");
                }
            }
            return disObj;
        }


        [HttpGet]
        public JsonResult LoanData(int LoanNo, int empid)
        {
            var param = new { LoanId = LoanNo, EmployeeId = empid };
            var result = employeeService.GetDataWithParameter(param, "SP_GET_LOAN_DATA");

            // Initialize dictionary to hold result values
            Dictionary<string, string> disObj = new Dictionary<string, string>();
 

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                var row = result.Tables[0].Rows[0];

                disObj["DisburseDate"] = row["DisburseDate"].ToString();
                disObj["DisburseAmount"] = row["DisburseAmount"].ToString();
                disObj["InterestCharge"] = row["InterestCharge"].ToString();
                disObj["InterestRate"] = row["InterestRate"].ToString();
                disObj["InstallmentPrincipal"] = row["InstallmentPrincipal"].ToString();
                disObj["InstallmentInterest"] = row["InstallmentInterest"].ToString();
                
                disObj["MonthlyInstallment"] = row["MonthlyInstallment"].ToString();
                disObj["NoOfInstallment"] = row["NoOfInstallment"].ToString();
                disObj["LastInstallmentDate"] = row["LastInstallmentDate"].ToString();
                disObj["Coll_LoanAmount"] = row["Coll_LoanAmount"].ToString();
                disObj["Coll_InterestAmount"] = row["Coll_InterestAmount"].ToString();

             
            }

            return Json(new
            {
                MaxDis = disObj.ToList(),
          
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoanEligibilityCheckInfo(int empid, string loanType, int? purposeid)
        {
            Dictionary<string, string> disObj = new Dictionary<string, string>();
            disObj = LoanEligibilityCheck(empid, loanType, purposeid);
            var pur = loanPurposeService.GetById(purposeid ?? 0);
            string methodType = ""; int gracePeriod = 0;
            if (pur != null)
            {
                methodType = pur.MethodType;
                gracePeriod = pur.GracePeriod;
            }

            return Json(new { MaxDis = disObj.ToList(), MethodType = methodType, GracePeriod = gracePeriod }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ApplicationDelete(int id)
        {
            var obj = applicantInfoService.GetById(id);
            obj.ApplicationStatus = ApplicationStatus_const.Delete;
            applicantInfoService.Update(obj);
            return Json(obj);
        }

        [HttpGet]
        public JsonResult GetLoanDetailsById(int loanid, string uptodate)
        {
            try
            {
                DateTime up_dt = DateTime.Now;
                DateTime.TryParse(uptodate, out up_dt);
                if (DateTime.MinValue.Equals(up_dt))
                    return Json(new { Message = "Upto date format is not valid" }, JsonRequestBehavior.AllowGet);
                else
                {
                    var dis = disbursementService.GetById(loanid);

                    var charge = new LoanCalculationService().Interestcharge(loanid, up_dt, dis.DisburseDate, dis.DisburseAmount, dis.InterestCharge, dis.LoanPaid, dis.InterestPaid, dis.IntersetRate);
                    return Json(new { Message = "", dis = dis, charge = charge, interest_due= dis.InterestCharge-dis.InterestPaid }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public JsonResult PostSpecialCollection(LoanCollectionViewModel model)
        {
            string msg = ""; int status = 0;
            try
            {
                if (model == null) msg = "Data not found";
                else if (DateTime.MinValue.Equals(model.TransactionDate)) msg = "Date format is not correct";
                else if (model.TransactionAmount > (model.TotalDue + model.InterestCharge)) msg = "Transaction Amount is greater than total due amount";
                else
                {
                    var dis = disbursementService.GetById(model.LoanId);
                    var charge = new LoanCalculationService().Interestcharge(model.LoanId, model.TransactionDate, dis.DisburseDate, dis.DisburseAmount, dis.InterestCharge, dis.LoanPaid, dis.InterestPaid, dis.IntersetRate);
                    if (model.InterestCharge != charge) msg = "Interest Charge calculation is not match, Please try again.";
                    else
                    {
                        var lst = new List<EmployeeMonthlySalaryModel>();
                        lst.Add( new EmployeeMonthlySalaryModel()
                        {
                            LoanId=model.LoanId,
                            PRComponentAmount=model.TransactionAmount,
                            SalaryDate=model.TransactionDate,
                            Comments=model.Narration,
                            CreateUser=LoggedInEmployeeId
                        });
                        new LoanCalculationService().EmployeeMonthlySalaryApprovedProcess(lst, new gHRMDBContext());
                        msg = "Transaction Completed";status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { msg = msg, status = status });
        }
        #endregion Method

        #region    Grid
        //public JsonResult GetAllApplication(int? employeeId)  // sp_GetAllApplication
        //{
        //    gHRMDBContext db = new gHRMDBContext();
        //    var lst = (from ap in db.ApplicantInfos
        //               join lp in db.LoanPurposes on ap.PurposeId equals lp.PurposeId
        //               join ld in db.LoanDisbursements on ap.Id equals ld.ApplicantId into gj
        //               from x in gj.DefaultIfEmpty()
        //               where ap.EmployeeId == (employeeId ?? (LoggedInEmployeeId ?? 0)) && ap.ApplicationStatus != "Delete"
        //               select new ApplicantGridViewModel
        //               {
        //                   Id = ap.Id,
        //                   LoanType = ap.LoanType,
        //                   Purpose = lp.PurposeName,
        //                   LoanAmount = ap.LoanAmount,
        //                   InstallmentNo = ap.InstallmentNo,
        //                   NotificationStatus = ap.NotificationStatus,
        //                   ApplicationStatus = ap.ApplicationStatus,
        //                   DisburseDate = x.DisburseDate,
        //                   DisburseAmount = x.DisburseAmount,
        //                   NoOfInstallment = x.NoOfInstallment,
        //                   LastInstallmentDate = x.LastInstallmentDate,
        //                   IsClose = x.IsClose
        //               });
        //    return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetAllApplication(int? employeeId)  // sp_GetAllApplication
        {
            //var param = new { EmployeeId = employeeId ?? 0 };
            //var sp_result = employeeService.GetDataWithParameter(param, "sp_GetAllApplication");
            var employeeIdValue = employeeId ?? 0; // Ensure null safety
            var query = $"loan.sp_GetAllApplication {employeeIdValue}"; // Properly format the query

            using (var db = new gHRMDBContext())
            {
                var lst = db.Database.SqlQuery<ApplicantGridViewModel>(query).ToList();
                return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAllLoanApplication()
        {
            var lst = new gHRMDBContext().Database.SqlQuery<ApprovalOrDisbursGridViewModel>("[loan].[sp_ApprovalOrDisbursGrid] " + (LoggedInEmployeeId ?? 0) + "").ToList();
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLoanApplication2()
        {
            var lst = new gHRMDBContext().Database.SqlQuery<ApprovalOrDisbursGridViewModel>("[loan].[sp_ApprovalOrDisbursGrid2] " + (LoggedInEmployeeId ?? 0) + "").ToList();
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLoanApplication3()
        {
            var lst = new gHRMDBContext().Database.SqlQuery<ApprovalOrDisbursGridViewModel3>("[loan].[sp_ApprovalOrDisbursGrid3] " + (LoggedInEmployeeId ?? 0) + "").ToList();
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLoanApplicationStatus(string status)
        {
            status = (status == "Pending" ? ApplicationStatus_const.Active
                : status == "Disburse" ? ApplicationStatus_const.Disburse
                : status == "Reject" ? ApplicationStatus_const.Reject
                : status == "Running"? "Running"
                : "");

            var lst = new gHRMDBContext().Database.SqlQuery<loanStatusViewModel>("[loan].[sp_loanStatus] '" + status + "'").ToList();
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLoanNoByEmployeeForDropdown(int employeeId, bool? isClose)
        {
            var lst = disbursementService.GetMany(x => x.EmployeeId == employeeId && x.IsClose == (isClose ?? x.IsClose)).Select(x => new SelectListItem { Text = x.LoanNo, Value = x.LoanId.ToString() });
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLoanNoByEmployeeForDropdown3(int employeeId, bool? isClose)
        {
            // Start with base query for employeeId
            var query = disbursementService.GetMany(x => x.EmployeeId == employeeId);

            // Apply isClose filter only if a value is provided
            if (isClose.HasValue)
            {
                query = query.Where(x => x.IsClose == isClose.Value);
            }

            // Execute query and materialize results
            var disbursements = query.ToList();


            // Project to SelectListItem in memory
            var result = disbursements.Select(x => new SelectListItem
            {
                Text = $"{GetLoanPurposeNameById(x.PurposeId)} -- {x.LoanNo}",
                Value = x.LoanId.ToString()
            }).ToList();

            // Add default select item at the top
            result.Insert(0, new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLoanNoByEmployeeForDropdown4(int employeeId, bool? isClose)
        {
            // Get employee code first

            var db = new gHRMDBContext();

            string employeeCode = db.Employees
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => e.EmployeeCode)
                .FirstOrDefault();

            // Execute stored procedure
            var parameters = new[] {
        new System.Data.SqlClient.SqlParameter("@EmployeeCode", employeeCode),
        new System.Data.SqlClient.SqlParameter("@IsClose", isClose ?? (object)DBNull.Value)
    };

            var results = db.Database
                .SqlQuery<LoanDisbursementResult>("EXEC loan.GetLoanDisbursementsForDropdown @EmployeeCode, @IsClose", parameters)
                .ToList();

            // Format results
            var dropdownItems = results.Select(x => new SelectListItem
            {
                Text = $"{x.PurposeName} -- {x.LoanNo}",
                Value = x.LoanId.ToString()
            }).ToList();

            // Add default item
            dropdownItems.Insert(0, new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });

            return Json(dropdownItems, JsonRequestBehavior.AllowGet);
        }

        // Helper class for result mapping
        private class LoanDisbursementResult
        {
            public string EmployeeCode { get; set; }
            public int LoanId { get; set; }
            public string LoanNo { get; set; }
            public int PurposeId { get; set; }
            public string PurposeName { get; set; }
        }

        private string GetLoanPurposeNameById(int purposeId)
        {
            switch (purposeId)
            {
                case 1:
                    return "PF Loan";
                case 2:
                    return "Special Loan";
                case 3:
                    return "General Loan";
                case 4:
                    return "Motorcycle loan";
                case 5:
                    return "co-operative loan";
                default:
                    return "Unknown Purpose";
            }
        }




        [HttpGet]
        public JsonResult GetLoanNoByEmployeeForDropdown2(int employeeId, bool? isClose)
        {
            var lst = disbursementService
                .GetMany(x => x.EmployeeId == employeeId && x.IsClose == (isClose ?? x.IsClose))
                .Select(x => new SelectListItem
                {
                    Text = x.LoanNo,
                    Value = x.LoanId.ToString()
                })
                .ToList();

            // Add default select item at the top
            lst.Insert(0, new SelectListItem
            {
                Text = "--Select--",
                Value = "0"
            });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        #endregion Grid
    }
}