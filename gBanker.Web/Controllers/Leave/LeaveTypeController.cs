
#region Usings

using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;

#endregion

namespace gHRM.Web.Controllers
{
    public class LeaveTypeController : BaseController
    {
        #region Private Variables

        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeSPService employeeSPService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown commonDynamicDropDown;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveTypeLedgerService leaveTypeLedgerService;

        #endregion

        #region Ctor

       public LeaveTypeController(ILeaveTypeService leaveTypeService, IEmployeeStatusService employeeStatusService, IEmployeeSPService employeeSPService, ILeaveHistoryService leaveHistoryService, ILeaveTypeLedgerService leaveTypeLedgerService)
        {
            this.leaveTypeService = leaveTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeSPService = employeeSPService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.leaveHistoryService = leaveHistoryService;
            this.leaveTypeLedgerService = leaveTypeLedgerService;
        }
        #endregion

        #region Listing

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            var model = new LeaveTypeViewModel();
            MapDropDownList(model);
            ViewBag.LEAVE_TYPE_TOTAL_MAX_LEAVE_DAYS_ENABLED = AppSetting.GetBool(AppSetting.LEAVE_TYPE_TOTAL_MAX_LEAVE_DAYS_ENABLED, HttpContext);
            return View(model);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            var leave = leaveTypeService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<LeaveType, LeaveTypeViewModel>(leave);
            entity.CountWeeklyHolidaysInBetween = leave.IsCountWeeklyHolidaysInBetween ? "Y" : "N";
            entity.CountOtherHolidaysInBetween = leave.IsCountOtherHolidaysInBetween ? "Y" : "N";
            MapDropDownList(entity);
            return View(entity);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            return View();
        }

        #endregion

        #region Ajax Calls
        public JsonResult LeaveTypeDelete(int LeaveTypeId)
        {
            var result = 0;
            var message = "";
            try
            {
                var isUsed = leaveHistoryService.GetMany(p => p.IsActive == true && p.LeaveTypeId == LeaveTypeId);

                if (isUsed.Any())
                {
                    result = 0;
                    message = "LeaveType Already Assign In Leave History, Delete denied";
                }
                else
                {
                    var entity = leaveTypeService.GetById(LeaveTypeId);
                    entity.IsActive = false;
                    entity.InActiveDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    leaveTypeService.Update(entity);
                    result = 1;
                    message = "LeaveType deleted successfully";
                }

            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveType([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var leaveList = employeeSPService.GetDataWithoutParameter("leave.SP_GetLeaveTypeList");
                var List_ViewModel = leaveList.Tables[0].AsEnumerable()
                    .Select(row => new LeaveTypeViewModel()
                    {
                        rowSl = row.Field<string>("rowSl"),
                        LeaveTypeId = row.Field<int>("LeaveTypeId"),
                        LeaveTypeName = row.Field<string>("LeaveTypeName"),
                        //EmployeeStatus = row.Field<string>("EmployeeStatus"),
                        EmployeeStatus = row.Field<string>("StatusName"),
                        LeaveStatus = row.Field<string>("LeaveStatus"),
                        EligibleFrom = row.Field<string>("EligibleFrom"),
                        MaxLeaveDays = row.Field<int>("MaxLeaveDays"),
                        MaxAvailDays = row.Field<int>("MaxAvailDays"),
                        //LeaveStatus = row.Field<string>("LeaveStatus"),
                        LeaveGender = row.Field<string>("LeaveGender"),
                    }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult SaveLeaveType(LeaveTypeViewModel leaveType)
        {
            var result = string.Empty;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (int empStatusId in leaveType.EmployeeStatusIdList)
                    {
                        var entity = Mapper.Map<LeaveTypeViewModel, LeaveType>(leaveType);
                        if (ModelState.IsValid)
                        {
                            var errors = leaveTypeService.IsValidLeaveType(entity.LeaveTypeName, empStatusId, entity.LeaveCategory);
                            if (errors.ToList().Count == 0)
                            {
                                entity.EmployeeStatusId = empStatusId;
                                entity.LeaveTypeId = leaveType.LeaveTypeId;
                                entity.LeaveTypeName = leaveType.LeaveTypeName;
                                entity.LeaveCategory = leaveType.LeaveCategory;
                                entity.EligibleFrom = leaveType.EligibleFrom;
                                entity.MaxLeaveDays = leaveType.MaxLeaveDays;
                                entity.MaxAvailDays = leaveType.MaxAvailDays;
                                entity.LeaveStatus = leaveType.LeaveStatus;
                                entity.LeaveGender = leaveType.LeaveGender;
                                entity.LeaveTypeRank = leaveType.LeaveTypeRank;
                                entity.DaysPerEL = leaveType.DaysPerEL;
                                entity.ELAdd = leaveType.ELAdd;
                                entity.IsActive = true;
                                entity.CreateDate = DateTime.Now;
                                entity.CreateUser = LoggedInEmployeeId;
                                entity.IsCountWeeklyHolidaysInBetween = "Y" == leaveType.CountWeeklyHolidaysInBetween;
                                entity.IsCountOtherHolidaysInBetween = "Y" == leaveType.CountOtherHolidaysInBetween;
                                if (!string.IsNullOrEmpty(leaveType.TotalMaxLeaveDays)) entity.TotalMaxLeaveDays = Convert.ToInt32(leaveType.TotalMaxLeaveDays);
                                leaveTypeService.Create(entity);
                                if(leaveType.Ledger == true)
                                {
                                    var ledgerEntity = Mapper.Map<LeaveTypeViewModel, LeaveTypeLedger>(leaveType);
                                    leaveTypeLedgerService.Create(ledgerEntity);
                                }                                    
                                result = "Save Successfull";
                            }
                            else
                            {
                                scope.Dispose();
                                result = "Leave Type Already assinged. please insert Another Leave Type";
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            scope.Dispose();
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    scope.Complete();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = ex.InnerException.Message.ToString();
                    scope.Dispose();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult UpdateLeaveType(LeaveTypeViewModel leaveType)
        {
            var result = string.Empty;

            if (!ModelState.IsValid)
            {
                result = "Warning, You must fill all the required fields";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
            try
            {
                var errors = leaveTypeService.IsValidLeaveTypeEdit(leaveType.LeaveTypeName, leaveType.EmployeeStatusId, leaveType.LeaveCategory, leaveType.LeaveTypeId);
                if (errors.ToList().Count > 0)
                {
                    result = errors.ToList()[0].Message;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                
                var entity = Mapper.Map<LeaveTypeViewModel, LeaveType>(leaveType);
                var updateLeaveType = leaveTypeService.GetById(Convert.ToInt32(entity.LeaveTypeId));

                if (updateLeaveType==null)
                {
                    result = "This leave type not found. Please try another!";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                updateLeaveType.EmployeeStatusId = entity.EmployeeStatusId;
                updateLeaveType.LeaveTypeName = entity.LeaveTypeName;
                updateLeaveType.EligibleFrom = entity.EligibleFrom;
                updateLeaveType.MaxLeaveDays = entity.MaxLeaveDays;
                updateLeaveType.MaxAvailDays = entity.MaxAvailDays;
                updateLeaveType.LeaveStatus = entity.LeaveStatus;
                updateLeaveType.LeaveGender = entity.LeaveGender;
                updateLeaveType.LeaveTypeRank = entity.LeaveTypeRank;
                updateLeaveType.DaysPerEL = entity.DaysPerEL;
                updateLeaveType.LeaveQty = entity.LeaveQty;
                updateLeaveType.LeaveCategory = entity.LeaveCategory;
                updateLeaveType.IsCountWeeklyHolidaysInBetween = "Y" == leaveType.CountWeeklyHolidaysInBetween;
                updateLeaveType.IsCountOtherHolidaysInBetween = "Y" == leaveType.CountOtherHolidaysInBetween;
                if (!string.IsNullOrEmpty(leaveType.TotalMaxLeaveDays)) updateLeaveType.TotalMaxLeaveDays = Convert.ToInt32(leaveType.TotalMaxLeaveDays);
                else updateLeaveType.TotalMaxLeaveDays = null;
                leaveTypeService.Update(updateLeaveType);

                result = "Update Successfull"; 
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = "There was an error while updating. Please try again!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Private Methods

        private void MapDropDownList(LeaveTypeViewModel model)
        {
            model.EligibleFromList = commonStaticDropDown.GetLeaveEligibleDateList();
            model.LeaveStatusList = commonStaticDropDown.GetLeaveLapsOrCarryFoewardStatusList();
            model.LeaveGenderList = commonStaticDropDown.GetMaleFemaleAndBothGenderList();
            model.LeaveCategoryList = commonDynamicDropDown.GetAllLeaveCategoryList();
            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: true);
            model.YesNoList = commonStaticDropDown.GetYesNoList();
        }

        #endregion
    }
}
