using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using gHRM.Web.DropDownService;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{
    public class AttendancePenaltyConfigurationController : BaseController
    {

        #region variables

        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly ILeaveCategoryService leaveCategoryService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IAttendancePenaltyConfigurationService attendancePenaltyConfigurationService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public AttendancePenaltyConfigurationController(
              IEmployeeSPService employeeSpService
            , IEmployeeStatusService employeeStatusService
            , IAttendancePenaltyConfigurationService attendancePenaltyConfigurationService
            , ILeaveCategoryService leaveCategoryService
            , ILeaveTypeService leaveTypeService
            )
        {
            this.employeeSpService = employeeSpService;
            this.employeeStatusService = employeeStatusService;
            this.attendancePenaltyConfigurationService = attendancePenaltyConfigurationService;
            this.leaveCategoryService = leaveCategoryService;
            this.leaveTypeService = leaveTypeService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }


        #endregion

        #region Events

        public ActionResult Index()
        {
            var model = new AttendancePenaltyConfigurationViewModel();
            MapDropdown(model);
            return View(model);
        }

        #endregion


        #region HttpRequests

        public ActionResult GetAttendancePenaltyInfoKendo([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                List<AttendancePenaltyConfigurationViewModel> List_ViewModel = new List<AttendancePenaltyConfigurationViewModel>();

                var attendancePenaltyList = employeeSpService.GetDataWithoutParameter("att.SP_GetAttendancePenaltyInfo");
                List_ViewModel = attendancePenaltyList.Tables[0].AsEnumerable()
                .Select(row => new AttendancePenaltyConfigurationViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    Id = row.Field<int>("Id"),
                    TotalLateDays = row.Field<int>("TotalLateDays"),
                    LeaveDeduction = row.Field<int>("LeaveDeduction"),
                    LeaveTypeName = row.Field<string>("LeaveTypeName"),
                    EmployeeStatusFull = row.Field<string>("StatusName"),
                    LeaveOrder = row.Field<int>("LeaveOrder"),
                    EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                    //EmployeeStatus = row.Field<string>("EmployeeStatus"),
                    LeaveType = row.Field<string>("LeaveType"),

                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveAttendancePenalty(AttendancePenaltyConfigurationViewModel obj)
        {
            var result = 0;
            var message = "";
            var penaltyList = new List<AttendancePenaltyConfiguration>();
            try
            {
                var leaveTypeList = leaveTypeService.GetAll().Where(l => l.IsActive == true);
                var empStatusList = employeeStatusService.GetAll().Where(s => s.IsActive == true);
                var existingConfiguration = attendancePenaltyConfigurationService.GetAll().Where(x => x.IsActive == true);

                foreach (int selectedStatusId in obj.SelectedStatusId)
                {
                    if (selectedStatusId>0)
                    {
                        var checkDuplicateOrder = existingConfiguration.Where(p => p.LeaveOrder == obj.LeaveOrder && p.StatusId== selectedStatusId);
                        if (checkDuplicateOrder.Any())
                        {
                            result = 0;
                            message = "Same leave order already exists, save denied";
                            break;
                        }
                        var checkDuplicateType = existingConfiguration.Where(p => p.IsActive == true && (p.LeaveType.ToUpper().Trim() == obj.LeaveType.ToUpper().Trim() && p.StatusId== selectedStatusId));
                        if (checkDuplicateType.Any())
                        {
                            result = 0;
                            message = "Same leave type already exists, save denied";
                            break;
                        }
                        var leaveType = leaveTypeList.Where(p => p.LeaveCategory.Trim() == obj.LeaveType.Trim() && p.EmployeeStatusId == selectedStatusId).FirstOrDefault();
                        var leavetypeId = 0;
                        if (leaveType != null)
                        {
                            leavetypeId = leaveType.LeaveTypeId;
                        }
                        //var statusId = empStatusList.Where(x => x.StatusValue.Trim() == selectedStatus.Trim()).First().StatusId;

                        gHRMDBContext db = new gHRMDBContext();
                        var selectedStatus = db.EmployeeStatus.Where(x => x.StatusId == selectedStatusId).Select(z => z.StatusValue).FirstOrDefault();

                        if (leavetypeId > 0)
                        {
                            var model = new AttendancePenaltyConfiguration();
                            model.TotalLateDays = obj.TotalLateDays;
                            model.LeaveType = obj.LeaveType;
                            model.LeaveDeduction = obj.LeaveDeduction;
                            model.LeaveOrder = obj.LeaveOrder;
                            model.LeaveTypeId = leavetypeId;
                            model.StatusId = selectedStatusId;
                            //model.EmployeeStatus = selectedStatus.Trim();
                            model.EmployeeStatus = selectedStatus;
                            model.IsActive = true;
                            model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            model.CreateDate = DateTime.UtcNow;
                            model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            model.UpdateDate = DateTime.UtcNow;
                            penaltyList.Add(model);
                        }
                        else
                        {
                            result = 0;
                            message = "No leave type found for given employee status";
                            break;
                        }
                    }
                    else
                    {
                        result = 0;
                        message = "No Employee Status Found";
                    }
                }
                if (penaltyList.Count > 0)
                {
                    attendancePenaltyConfigurationService.AddAttendancePenaltyConfigurationList(penaltyList);
                    result = 1;
                    message = "Saved successfully";
                }

            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateAttendancePenalty(AttendancePenaltyConfigurationViewModel obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = attendancePenaltyConfigurationService.GetById(obj.Id);

                var existingConfiguration = attendancePenaltyConfigurationService.GetAll().Where(x => x.IsActive == true && x.Id != obj.Id);
                var checkDuplicateOrder = existingConfiguration.Where(p => p.LeaveOrder == obj.LeaveOrder && p.StatusId== model.StatusId);
                if (checkDuplicateOrder.Any())
                {
                    result = 0;
                    message = "Same leave order already exists, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                var checkDuplicateType = existingConfiguration.Where(p => p.IsActive == true && (p.LeaveType.ToUpper().Trim() == obj.LeaveType.ToUpper().Trim() && p.StatusId == model.StatusId));
                if (checkDuplicateType.Any())
                {
                    result = 0;
                    message = "Same leave type already exists, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                model.TotalLateDays = obj.TotalLateDays;
                model.LeaveType = obj.LeaveType;
                model.LeaveDeduction = obj.LeaveDeduction;
                model.LeaveOrder = obj.LeaveOrder;
                model.IsActive = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                attendancePenaltyConfigurationService.Update(model);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAttendancePenalty(int id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = attendancePenaltyConfigurationService.GetById(id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                attendancePenaltyConfigurationService.Update(model);
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

        #region Methods

        private void MapDropdown(AttendancePenaltyConfigurationViewModel model)
        {
            var lateDays = new List<SelectListItem>();
            lateDays.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            for (int i = 1; i <= 31; i++)
            {
                lateDays.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.TotalLateDaysCount = lateDays;

            var leaveType = leaveCategoryService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewLeaveType = leaveType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Detail,
                Value = p.Value
            }).ToList();
            var typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            typeList.AddRange(viewLeaveType);
            model.LeaveTypeList = typeList;


            var deductionDays = new List<SelectListItem>();
            deductionDays.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            for (int i = 1; i <= 31; i++)
            {
                deductionDays.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.TotalDeductionDays = deductionDays;

            var orderList = new List<SelectListItem>();
            orderList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            for (int i = 1; i <= 10; i++)
            {
                orderList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.TotalOrderList = orderList;

            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: true);

        }

        #endregion
    }
}