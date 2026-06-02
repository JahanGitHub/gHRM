using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Text;
using gHRM.Web.CommonDropdown;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class AttendancePenaltyController : BaseController
    {

        #region Private Members

        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IAttendancePenaltyConfigurationService attendancePenaltyConfigurationService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveTypeService leaveTypeService;

        public CommonDynamicDropDown commonDynamicDropDown;
        private DateTime startDate;
        private DateTime endDate;

        public AttendancePenaltyController(
              IEmployeeSPService employeeSpService
            , IOfficeService officeService
            , IOfficeTypeService officeTypeService
            , IEmployeeDepartmentService employeeDepartmentService
            , IEmployeeDesignationService employeeDesignationService
            , IAttendancePenaltyConfigurationService attendancePenaltyConfigurationService
            , ILeaveHistoryService leaveHistoryService
            ,ILeaveTypeService leaveTypeService
            )
        {
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.attendancePenaltyConfigurationService = attendancePenaltyConfigurationService;
            this.leaveHistoryService = leaveHistoryService;
            this.leaveTypeService = leaveTypeService;
            commonDynamicDropDown = new CommonDynamicDropDown();
        }


        #endregion

        #region Events

        public ActionResult LeaveDeduction()
        {
            var model = new AttendancePenaltyConfigurationViewModel();
            MapDropdownForYearMonth(model);
            return View(model);
        }

        public ActionResult AbsentEmployees()
        {
            var model = new AttendancePenaltyConfigurationViewModel();
            MapDropdownForYearMonth(model);
            return View(model);
        }

        #endregion


        #region HttpRequests

        [HttpPost]
        public JsonResult ApproveLeaveDeduction(List<LeaveHistory> LeavePenalty)
        {
            var result = 0;
            var message = "";
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var penaltyLeaveList = new List<LeaveHistory>();

                    if (LeavePenalty.Count > 0)
                    {
                        foreach (var penalty in LeavePenalty)
                        {
                            var leaveHistory = new LeaveHistory();
                            leaveHistory.EmployeeId = penalty.EmployeeId;
                            leaveHistory.LeaveTypeId = penalty.LeaveTypeId;
                            leaveHistory.LeaveRequestDate = penalty.LeaveRequestDate;
                            leaveHistory.LeaveStartDate = penalty.LeaveStartDate;
                            leaveHistory.LeaveEndDate = penalty.LeaveEndDate;
                            leaveHistory.ReplacementEmployee = 0;
                            leaveHistory.TotalDays = penalty.TotalDays;
                            leaveHistory.LeaveReason = "Attendance Penalty";
                            leaveHistory.JoinDate = penalty.LeaveEndDate.AddDays(1);
                            leaveHistory.IsApproved = true;
                            leaveHistory.ApprovedDate = DateTime.Now;
                            leaveHistory.IsAdjustment = true;
                            leaveHistory.AdjustmentDate = DateTime.Now;
                            leaveHistory.IsActive = true;
                            leaveHistory.CreateDate = DateTime.UtcNow;
                            leaveHistory.CreateUser = LoggedInEmployeeId;
                            penaltyLeaveList.Add(leaveHistory);

                        }
                        leaveHistoryService.AddCLOpeningList(penaltyLeaveList);
                        scope.Complete();
                        result = 1;
                        message = "Attendance Penalty Approved Successfully";
                    }
                    else
                    {
                        scope.Dispose();
                        result = 0;
                        message = "Error Occured";
                    }
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    result = 0;
                    message = "Error Occured";
                }
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetLeaveDeductionInfo([DataSourceRequest]DataSourceRequest request, int Month, int Year, string officeId, string OfficeTypeId)
        {
            var viewPenaltyList = new List<AttendancePenaltyConfigurationViewModel>();
            try
            {
                var startDate = new DateTime(Year, Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var penaltyConfigList = attendancePenaltyConfigurationService.GetAll().Where(x => x.IsActive == true);
                var insertedToViewList = false;
                if (penaltyConfigList.Any())
                {
                    StringBuilder sb = new StringBuilder();

                    if (!String.IsNullOrEmpty(OfficeTypeId))
                    {
                        int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                        sb.Append(" AND o.OfficeTypeId=" + _OfficeTypeId);
                    }

                    if (!String.IsNullOrEmpty(officeId))
                    {
                        int _officeId = Convert.ToInt32(officeId);
                        sb.Append(" AND o.OfficeId=" + _officeId);
                    }

                    var param = new { AttendaceDateFrom = startDate, AttendanceDateTo = endDate, AndCondition = sb.ToString() };
                    var lateEmployeeList = employeeSpService.GetDataWithParameter(param, "att.SP_GetLateEmployeeList");
                    var employeeLateInfoList = lateEmployeeList.Tables[0].AsEnumerable()
                        .Select((p, sl) => new AttendancePenaltyConfigurationViewModel()
                        {
                            rowSl = (sl + 1).ToString(),
                            EmployeeId = p.Field<long>("EmployeeId"),
                            EmployeeCode = p.Field<string>("EmployeeCode"),
                            EmployeeName = p.Field<string>("EmployeeName"),
                            //EmployeeStatus = p.Field<string>("EmployeeStatus"),
                            EmployeeStatusId = p.Field<int>("EmployeeStatusId"),
                            OfficeName = p.Field<string>("OfficeName"),
                            DepartmentName = p.Field<string>("DepartmentName"),
                            OfficeDesignation = p.Field<string>("OffcDesignName"),
                            TotalLateDays = p.Field<int>("LateCount")

                        }).ToList();

                    foreach (var lateInfo in employeeLateInfoList)
                    {
                        var penaltyConfigListByStatus = penaltyConfigList.Where(p => p.StatusId == lateInfo.EmployeeStatusId);
                        int daysToDeduct = 0;

                        foreach (var config in penaltyConfigListByStatus)
                        {
                            var leaveType = config.LeaveType.Trim();
                            var leaveTypeId = config.LeaveTypeId;
                            int availableDays = TotalLeaveAvailable(leaveTypeId, leaveType, lateInfo.EmployeeId, Year);
                            daysToDeduct = lateInfo.TotalLateDays / config.TotalLateDays;

                            if (availableDays >= daysToDeduct)
                            {
                                lateInfo.LeaveTypeId = config.LeaveTypeId;
                                lateInfo.LeaveType = config.LeaveType;
                                lateInfo.LeaveDeduction = daysToDeduct;
                                viewPenaltyList.Add(lateInfo);
                                insertedToViewList = true;
                                break;
                            }
                        }

                        if (!insertedToViewList)
                        {
                            lateInfo.LeaveTypeId = 0;
                            lateInfo.LeaveType = "No Remaining Leave Found";
                            lateInfo.LeaveDeduction = daysToDeduct;
                            viewPenaltyList.Add(lateInfo);
                        }
                    }
                }
                else
                {
                    //no configuration
                }

            }
            catch (Exception ex)
            {
                var exception = ex.Message;
            }
            DataSourceResult result = viewPenaltyList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetEmployeesAbsentInfo([DataSourceRequest]DataSourceRequest request, int Month, int Year, string officeId, string OfficeTypeId)
        {
            var absentEmployees = new List<AttendancePenaltyConfigurationViewModel>();

            try
            {
                
                if(Month==0)
                {
                     startDate = new DateTime(Year, 1, 1);
                     endDate = new DateTime(Year, 12, 1);
                }
                else
                {
                     startDate = new DateTime(Year, Month, 1);
                     endDate = startDate.AddMonths(1).AddDays(-1);
                }
             


                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND OfficeTypeId=" + _OfficeTypeId);
                }

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND OfficeId=" + _officeId);
                }

                var param = new { AttendaceDateFrom = startDate, AttendanceDateTo = endDate, AndCondition = sb.ToString() };
                var lateEmployeeList = employeeSpService.GetDataWithParameter(param, "att.SP_Timekeeping_AbsentEmployees");


                absentEmployees = lateEmployeeList.Tables[0].AsEnumerable()
                        .Select((p, sl) => new AttendancePenaltyConfigurationViewModel()
                        {
                            rowSl = (sl + 1).ToString(),
                            EmployeeId = p.Field<long>("EmployeeId"),
                            EmployeeCode = p.Field<string>("EmployeeCode"),
                            EmployeeName = p.Field<string>("EmployeeName"),
                            //EmployeeStatus = p.Field<string>("EmployeeStatus"),
                            EmployeeStatusId = p.Field<int>("EmployeeStatusId"),
                            OfficeName = p.Field<string>("OfficeName"),
                            DepartmentName = p.Field<string>("DepartmentName"),
                            OfficeDesignation = p.Field<string>("OffcDesignName"),
                            AttendanceDate = p.Field<string>("AttendanceDate"),
                            Gender = p.Field<string>("Gender"),
                            StartDate = p.Field<string>("StartDate"),
                            EndDate = p.Field<string>("EndDate"),
                        }).ToList();

                absentEmployees.ForEach(f => f.LeaveTypeList = GetLeaveTypeListByEmployeeStatusAndGender(f.EmployeeStatusId, f.Gender));

                DataSourceResult result = absentEmployees.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                var exception = e.Message;
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }



        #endregion


        #region Methods

        private List<SelectListItem> GetLeaveTypeListByEmployeeStatusAndGender(int EmployeeStatusId, string EmployeeGender)
        {
            EmployeeGender = EmployeeGender ?? "M";
            var leaveTypeList = commonDynamicDropDown.GetLeaveTypeListByEmployeeStatusAndGender(EmployeeGender.Trim(), EmployeeStatusId);
            return leaveTypeList;
        }

        private int TotalLeaveAvailable(int LeaveTypeId, string LeaveType, long employeeId,int Year)
        {
            var availableLeave = 0;
            var param = new { LeaveTypeId = LeaveTypeId, EmployeeId = employeeId};
            if (LeaveType == "CL")
            {
                var param1 = new { LeaveTypeId = LeaveTypeId, EmployeeId = employeeId, @Year = Year };
                var availableDay = employeeSpService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_CL");
                availableLeave = Convert.ToInt32(availableDay.Tables[0].Rows[0]["CurrentBalance"]);
            }
            else if (LeaveType == "AL")
            {
                var availableDay = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_AL");
                availableLeave = Convert.ToInt32(availableDay.Tables[0].Rows[0]["CurrentBalance"]);
            }
            else if (LeaveType == "ML")
            {
                var availableDay = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_ML");
                availableLeave = Convert.ToInt32(availableDay.Tables[0].Rows[0]["CurrentBalance"]);
            }
            else if (LeaveType == "PL")
            {
                var availableDay = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_PL");
                availableLeave = Convert.ToInt32(availableDay.Tables[0].Rows[0]["CurrentBalance"]);
            }
            else
            {
                var availableDay = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_OL");
                availableLeave = Convert.ToInt32(availableDay.Tables[0].Rows[0]["CurrentBalance"]);
            }

            return availableLeave;
        }

        public void MapDropdownForYearMonth(AttendancePenaltyConfigurationViewModel model)
        {
            var yearList = new List<SelectListItem>();
            var currentYear = DateTime.Today.Year;
            var previousYear = currentYear - 1;

            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            yearList.Add(new SelectListItem() { Text = currentYear.ToString(), Value = currentYear.ToString() });
            yearList.Add(new SelectListItem() { Text = previousYear.ToString(), Value = previousYear.ToString() });
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i).ToString(CultureInfo.InvariantCulture), Value = i.ToString() });
            }
            model.MonthList = monthList;



            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;


            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //ofc_items.AddRange(viewOfcList);
            model.OfficeList = ofc_items;


            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            var dept = employeeDepartmentService.GetMany(p => p.IsActive == true);
            var viewDept = dept.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.DepartmentName,
                Value = p.DepartmentId.ToString()
            }).ToList();
            var deptList = new List<SelectListItem>();
            deptList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            deptList.AddRange(viewDept);
            model.DepartmentList = deptList;

            var departmentList = employeeDepartmentService.GetAll();
            var viewDepartmentList = departmentList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DepartmentCode, m.DepartmentName), Value = m.DepartmentId.ToString() });
            var dep_items = new List<SelectListItem>();
            dep_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            dep_items.AddRange(viewDepartmentList);
            model.DepartmentList = dep_items;


            var designationList = employeeDesignationService.GetAll();
            var viewDesignationList = designationList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_items = new List<SelectListItem>();
            desig_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_items.AddRange(viewDesignationList);
            model.DesignationList = desig_items;

        }
        public JsonResult CheckAvailableLeaveByType_NotUsed(string EmployeeCode, string LeaveType, DateTime StartDate, DateTime EndDate)
        {
            var result = false;
            try
            {
                var param3 = new { EmployeeCode = EmployeeCode, DateFrom = StartDate };
                var dayCount = ((EndDate - StartDate).TotalDays)+1;
                var CodeWiseAttendanceALCL = employeeSpService.GetDataWithParameter(param3, "leave.SP_RPT_GetLeaveBalance_ALCL");
                if (LeaveType == "CL")
                {
                    var clAvailable = Convert.ToInt32(CodeWiseAttendanceALCL.Tables[0].Rows[0]["BalanceCL"].ToString());
                    if (dayCount<= clAvailable) {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }
                else if (LeaveType == "AL")
                {
                    var clAvailable = Convert.ToInt32(CodeWiseAttendanceALCL.Tables[0].Rows[0]["BalanceAL"].ToString());
                    if (dayCount <= clAvailable)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckAvailableLeaveByType(string leaveTypeId, string employeeId, DateTime StartDate, DateTime EndDate)
        {
            var typeId = Convert.ToInt32(leaveTypeId);
            var empId = Convert.ToInt32(employeeId);
            var leaveHistory = new LeaveHistoryViewModel();
            var leaveType = leaveTypeService.GetById(typeId);
            var LeaveAmountHistory = new DataSet();
            var result = false;

            var dayCount = ((EndDate - StartDate).TotalDays) + 1;
            try
            {

                var param = new { LeaveTypeId = typeId, EmployeeId = empId };
                if (leaveType.LeaveCategory == LeaveCategoryConstants.Casual)
                {
                    var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = EndDate.Year };
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_CL");
                }
                else if (leaveType.LeaveCategory == LeaveCategoryConstants.Annual_EL)
                {
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_AL");
                }
                else if (leaveType.LeaveCategory == LeaveCategoryConstants.Maternity)
                {
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_ML");
                }
                else if (leaveType.LeaveCategory == LeaveCategoryConstants.Paternity)
                {
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_PL");
                }
                else if (leaveType.LeaveCategory == LeaveCategoryConstants.Medical)
                {
                    var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = EndDate.Year };
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_MEL");
                }
                else if (leaveType.LeaveCategory == LeaveCategoryConstants.Annual_EL_Laps)
                {
                    var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = EndDate.Year };
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_AL_Laps");
                }
                else
                {
                    LeaveAmountHistory = employeeSpService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_OL");
                }

                if (LeaveAmountHistory != null)
                {
                    leaveHistory.TotalDays = Convert.ToInt32(LeaveAmountHistory.Tables[0].Rows[0]["TotalLeave"].ToString());
                    leaveHistory.LeaveCount = Convert.ToInt32(LeaveAmountHistory.Tables[0].Rows[0]["LeaveTaken"].ToString());
                    leaveHistory.leaveGain = Convert.ToInt32(LeaveAmountHistory.Tables[0].Rows[0]["CurrentBalance"].ToString());
                    leaveHistory.MaxAvailDays = Convert.ToInt32(LeaveAmountHistory.Tables[0].Rows[0]["MaxAvailDays"].ToString());
                    leaveHistory.LeaveCategory = leaveType.LeaveCategory;
                }
                else
                {
                    leaveHistory.TotalDays = 0;
                    leaveHistory.LeaveCount = 0;
                    leaveHistory.leaveGain = 0;
                    leaveHistory.MaxAvailDays = 0;
                    leaveHistory.LeaveCategory = "";
                }

                int clAvailable = Convert.ToInt32(leaveHistory.leaveGain??0);

                if (dayCount <= clAvailable)
                {
                    result = true;
                }                
            }
            catch
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}