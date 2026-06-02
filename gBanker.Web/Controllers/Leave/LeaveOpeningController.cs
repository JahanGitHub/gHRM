using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class LeaveOpeningController : BaseController
    {
        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly ILeaveELOpeningService leaveELOpeningService;

        private readonly IOfficeTypeService officeTypeService;

        public LeaveOpeningController(
              IEmployeeService employeeService
            , ILeaveHistoryService leaveHistoryService
            , ILeaveTypeService leaveTypeService
            , IEmployeeSPService employeeSPService
            , ILeaveELOpeningService leaveELOpeningService
            , IOfficeTypeService officeTypeService
            )
        {
            this.employeeService = employeeService;
            this.leaveHistoryService = leaveHistoryService;
            this.leaveTypeService = leaveTypeService;
            this.employeeSPService = employeeSPService;
            this.leaveELOpeningService = leaveELOpeningService;
            this.officeTypeService = officeTypeService;
        }

        #endregion

        #region Events

        // GET: LeaveOpening
        public ActionResult ClIndex()
        {
            return View();
        }

        public ActionResult ELIndex()
        {
            return View();
        }

        public ActionResult CasualOpening()
        {
            return View();
        }

        public ActionResult ELOpening()
        {
            return View();
        }

        #endregion

        #region HttpRequests

        public ActionResult GetCasualLeaveOpeningList([DataSourceRequest]DataSourceRequest request)
        {
            var infoList = employeeSPService.GetDataWithoutParameter("leave.SP_GetOpeningBalanceCasualLeave");

            var viewInfoList = infoList.Tables[0].AsEnumerable().Select((p, sl) => new LeaveOpeningViewModel()
            {
                rowSl = sl + 1,

                EmployeeCode = p.Field<string>("EmployeeCode"),
                EmployeeName = p.Field<string>("EmployeeName"),
                TotalDays = p.Field<int>("TotalDays"),
                TotalRemainingDays = p.Field<int>("TotalBalance"),
                LeaveTypeName = p.Field<string>("LeaveTypeName"),
                StatusName = p.Field<string>("StatusName")
            }).ToList();

            DataSourceResult result = viewInfoList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEarnLeaveOpeningList([DataSourceRequest]DataSourceRequest request)
        {
            var infoList = employeeSPService.GetDataWithoutParameter("leave.SP_GetOpeningBalanceEarnLeave");
            var viewInfoList = infoList.Tables[0].AsEnumerable().Select((p, sl) => new LeaveOpeningViewModel()
            {
                rowSl = sl + 1,
                EmployeeCode = p.Field<string>("EmployeeCode"),
                EmployeeName = p.Field<string>("EmployeeName"),
                StatusName = p.Field<string>("StatusName"),
                ELFull = p.Field<int>("ELFull"),
                EnjoyFull = p.Field<int>("EnjoyFull"),
                BalanceFull = p.Field<int>("BalanceFull"),
                BalanceHalf = p.Field<int>("BalanceHalf"),
                LastSaleDate = p.Field<string>("LastSaleDate")

            }).ToList();

            DataSourceResult result = viewInfoList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        #region CLOpening

        public JsonResult GetCasualOpeningData(int jtStartIndex, int jtPageSize, string jtSorting, string empCode, string empId)
        {
            long employeeId = 0;

            if (!string.IsNullOrEmpty(empId))
            {
                employeeId = Convert.ToInt64(empId);
            }


            var list = leaveHistoryService.GetMany(p => p.IsActive == true && p.EmployeeId == employeeId && p.LeaveReason.ToUpper().Trim() == "OPENING").ToList();
            //var list = leaveHistoryService.GetAll().Where(p => p.IsActive == true && p.EmployeeId == employeeId && p.LeaveReason.ToUpper().Trim() == "OPENING").ToList();
            var viewList = list.AsEnumerable().Select((p, sl) => new LeaveHistoryViewModel()
            {
                //SlNo = a++,
                SlNo = sl + 1,
                LeaveId = p.LeaveId,
                EmployeeId = p.EmployeeId,
                TotalDays =  p.TotalDays,
                LeaveReason = p.LeaveReason
            }).ToList();

            var currentPageRecords = viewList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewList.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult SaveCasualOpening(LeaveHistoryViewModel obj)
        {
            var result = 0;
            var message = "";
            long employeeId = 0;
            var leaveTypeId = 0;

            try
            {
                employeeId = obj.EmployeeId;
                int empStatusId = obj.EmployeeStatusId;

                var leavetypeDetail =
                    leaveTypeService.GetMany(
                            p =>
                                p.IsActive == true && p.LeaveCategory.Trim() == LeaveCategoryConstants.Casual &&
                                p.EmployeeStatusId == empStatusId)
                        .FirstOrDefault();

                if (leavetypeDetail == null)
                {
                    result = 0;
                    message = "Casual Leave Configuration not found. Please add casual leave config first.";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                leaveTypeId = leavetypeDetail.LeaveTypeId;

                var checkDuplicate =
                                leaveHistoryService
                                        .GetMany(p =>
                                            p.IsActive == true &&
                                            p.EmployeeId == employeeId &&
                                            p.LeaveReason.ToUpper().Trim() == LeaveReasonConstants.OPENING)
                                        .ToList();

                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Casual Leave Opening Data Already Taken for this Employee, Save Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                var model = new LeaveHistory();
                model.EmployeeId = employeeId;
                model.LeaveTypeId = leaveTypeId;
                model.TotalDays = Convert.ToInt32(obj.TotalDays);
                model.LeaveRequestDate = new DateTime(DateTime.Today.Year, 1, 1);
                model.LeaveStartDate = new DateTime(DateTime.Today.Year, 1, 1);
                model.LeaveEndDate = model.LeaveStartDate.AddDays(Convert.ToDouble(model.TotalDays - 1));
                model.ReplacementEmployee = 0;
                model.LeaveReason = LeaveReasonConstants.OPENING;
                model.AddressDuringLeave = "A";
                model.LeaveAttachment = null;
                model.JoinDate = model.LeaveEndDate.AddDays(1);
                model.LeaveUpToDate = null;
                model.IsApproved = true;
                model.ApprovedBy = 0;
                model.ApprovedDate = DateTime.Today;
                model.IsAdjustment = true;
                model.AdjustmentDate = DateTime.Today;
                model.AdjustmentBy = null;
                model.DispatchLeaveId = null;
                model.LWPSalaryDeduction = null;
                model.IsSalaryDeducted = null;
                model.leaveDispatchRemarks = null;
                model.IsEvidence = false;
                model.IsRecommendation = false;
                model.LeaveRecommendation = null;
                model.LeaveNote = null;
                model.LeaveHeader = null;
                model.LeaveFooter = null;
                model.Remarks = null;
                model.IsActive = true;
                model.InActiveDate = null;
                model.CreateUser = SessionHelper.LoggedInEmployeeID;
                model.CreateDate = DateTime.UtcNow;
                model.UpdateUser = SessionHelper.LoggedInEmployeeID;
                model.UpdateDate = DateTime.UtcNow;

                //let's add leave hostory [leave].[LeaveHistory]
                leaveHistoryService.Create(model);

                result = 1;
                message = "Saved successfully";

                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCasualOpening(LeaveHistoryViewModel obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = leaveHistoryService.GetById(Convert.ToInt32(obj.LeaveId));
                model.TotalDays = Convert.ToInt32(obj.TotalDays);
                model.UpdateUser = SessionHelper.LoggedInEmployeeID;
                model.UpdateDate = DateTime.UtcNow;

                //lets update leeave hostory [leave].[LeaveHistory]
                leaveHistoryService.Update(model);

                result = 1;
                message = "Updated successfully";
            }

            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCasualOpening(int id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = leaveHistoryService.GetById(id);
                model.IsActive = false;
                model.UpdateUser = SessionHelper.LoggedInEmployeeID;
                model.UpdateDate = DateTime.UtcNow;
                leaveHistoryService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ELOpening

        public JsonResult GetLeaveOpening(string EmployeeCode)
        {
            int result = 0;

            try
            {
                StringBuilder sb = new StringBuilder();

                if (EmployeeCode != "" && EmployeeCode != "0" && EmployeeCode != null)
                {
                    sb.Append("AND EM.EmployeeCode ='" + EmployeeCode + "'");
                }

                List<LeaveSellViewModel> List_LeaveSellViewModel = new List<LeaveSellViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveOpeningListForUpdate");
                if (empList.Tables[0].Rows.Count == 0)
                {
                    result = 1;
                    var empName = employeeService.GetByCode(EmployeeCode).EmployeeName;
                    var empId = employeeService.GetByCode(EmployeeCode).EmployeeId;
                    var codeName = EmployeeCode + '-' + empName;
                    return Json(new { Result = result, CodeName = codeName, EmpId = empId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = 2;
                    List_LeaveSellViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new LeaveSellViewModel
                    {
                        //Rowsl = row.Field<string>("Rowsl"),
                        ELOpeningId = row.Field<int>("ELOpeningId"),
                        //EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        LeaveStartDateMsg = row.Field<string>("LeaveStartDateMsg"),
                        LeaveEndDateMsg = row.Field<string>("LeaveEndDateMsg"),
                        ELFull = row.Field<int?>("ELFull"),
                        EnjoyFull = row.Field<int?>("EnjoyFull"),
                        BalanceFull = row.Field<int?>("BalanceFull"),
                        ELHalf = row.Field<int?>("ELHalf"),
                        EnjoyHalf = row.Field<int?>("EnjoyHalf"),
                        BalanceHalf = row.Field<int?>("BalanceHalf"),
                        LastSaleDateMsg = row.Field<string>("LastSaleDateMsg"),
                        WithSeniority = row.Field<int?>("WithSeniority"),
                        WithoutSeniority = row.Field<int?>("WithoutSeniority"),
                        //HasOpened = row.Field<int>("HasOpened")
                    }).ToList();

                    return Json(new { Result = result, Data = List_LeaveSellViewModel }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                return Json(new { Result = result, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditELOpening(string ELOpeningId, string LeaveStartDateMsg,
            string LeaveEndDateMsg, string ELFull, string EnjoyFull, 
            string BalanceFull, string ELHalf, string EnjoyHalf, string BalanceHalf, 
            string LastSaleDateMsg)
        {
            int result = 0;
            string message = string.Empty;

            try
            {
                var leaveELOpening = leaveELOpeningService.GetById(Convert.ToInt32(ELOpeningId));

                if (leaveELOpening == null)
                {
                    message = "Leave opening not found.";
                    return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
                }

                if (LeaveStartDateMsg != "")
                {
                    leaveELOpening.LeaveStartDate = Convert.ToDateTime(LeaveStartDateMsg);
                }
                if (LeaveEndDateMsg != "")
                {
                    leaveELOpening.LeaveEndDate = Convert.ToDateTime(LeaveEndDateMsg);
                }
                if (LastSaleDateMsg != "")
                {
                    leaveELOpening.LastSaleDate = Convert.ToDateTime(LastSaleDateMsg);
                }

                leaveELOpening.ELFull = Convert.ToInt32(string.IsNullOrEmpty(ELFull) ? "0" : ELFull);
                leaveELOpening.EnjoyFull = Convert.ToInt32(string.IsNullOrEmpty(EnjoyFull) ? "0" : EnjoyFull);
                leaveELOpening.BalanceFull = Convert.ToInt32(string.IsNullOrEmpty(BalanceFull) ? "0" : BalanceFull);
                leaveELOpening.ELHalf = Convert.ToInt32(string.IsNullOrEmpty(ELHalf) ? "0" : ELHalf);
                leaveELOpening.EnjoyHalf = Convert.ToInt32(string.IsNullOrEmpty(EnjoyHalf) ? "0" : EnjoyHalf);
                leaveELOpening.BalanceHalf = Convert.ToInt32(string.IsNullOrEmpty(BalanceHalf) ? "0" : BalanceHalf);
                leaveELOpening.IsActive = true;
                leaveELOpening.CreateDate = DateTime.Now;

                //let's add employee leave opening in [leave.LeaveELOpening]
                leaveELOpeningService.Update(leaveELOpening);

                result = 1;
                message = "Opening data update successfully";
            }
            catch (Exception ex)
            {
                message = "Opening data update failed";
                return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertELOpening(long EmployeeId, string LeaveStartDateMsg, string LeaveEndDateMsg, string ELFull, string EnjoyFull, string BalanceFull, string ELHalf, string EnjoyHalf, string BalanceHalf, string LastSaleDateMsg)//
        {
            int result = 0;
            string message = string.Empty;

            try
            {
                var entity = new LeaveELOpening();
                entity.EmployeeId = EmployeeId;
                entity.LeaveStartDate = Convert.ToDateTime(LeaveStartDateMsg);
                entity.LeaveEndDate = Convert.ToDateTime(LeaveEndDateMsg);
                entity.LastSaleDate = Convert.ToDateTime(LastSaleDateMsg);
                entity.ELFull = Convert.ToInt32(string.IsNullOrEmpty(ELFull) ? "0" : ELFull);
                entity.EnjoyFull = Convert.ToInt32(string.IsNullOrEmpty(EnjoyFull) ? "0" : EnjoyFull);
                entity.BalanceFull = Convert.ToInt32(string.IsNullOrEmpty(BalanceFull) ? "0" : BalanceFull);
                entity.ELHalf = Convert.ToInt32(string.IsNullOrEmpty(ELHalf) ? "0" : ELHalf);
                entity.EnjoyHalf = Convert.ToInt32(string.IsNullOrEmpty(EnjoyHalf) ? "0" : EnjoyHalf);
                entity.BalanceHalf = Convert.ToInt32(string.IsNullOrEmpty(BalanceHalf) ? "0" : BalanceHalf);
                entity.IsActive = true;
                entity.CreateDate = DateTime.Now;
                leaveELOpeningService.Create(entity);
                result = 1;
                message = "Opening data saved successfully";

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Opening data saved failed";
                return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #endregion

    }
}