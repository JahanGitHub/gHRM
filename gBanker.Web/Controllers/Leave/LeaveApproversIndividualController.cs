using gHRM.Data.CodeFirstMigration;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gHRM.Service;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
    public class LeaveApproversIndividualController : BaseController
    {
        #region variables
  
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveApproversService leaveApproversService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IEmployeeService employeeService;

        public LeaveApproversIndividualController(
              IEmployeeSPService employeeSPService
            , ILeaveApproversService leaveApproversService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IEmployeeDepartmentService employeeDepartmentService
            , IOfficeDesignationService officeDesignationService
            , IEmployeeService employeeService
            )
        {
            this.employeeSPService = employeeSPService;
            this.leaveApproversService = leaveApproversService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.officeDesignationService = officeDesignationService;
            this.employeeService = employeeService;
        }

        #endregion

        #region Events
        public ActionResult Index()
        {
            var model = new LeaveApproversViewModel();
            model.ApproverOfficeTypeList = getApproverOfficeTypeList();
            model.ApproverDesignationList = getApproverDesignationList();
            return View();
        }
       
        public ActionResult EditConfiguration(long EmployeeId)
        {

            var selectList = new List<SelectListItem>();
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            selectList.Add(pleaseSelect);
         
            var levelList = new List<SelectListItem>();
            levelList.Add(pleaseSelect);
            for (int i = 1; i <= 5; i++)
            {
                var item = new SelectListItem() { Text = i.ToString(), Value = i.ToString() };
                levelList.Add(item);
            }

            var model = new LeaveApproversViewModel();
            model.ApproverOfficeTypeList = getApproverOfficeTypeList();
            model.ApproverDesignationList = getApproverDesignationList();
            model.ApproverOfficeList = selectList;
            model.ApproverDepartmentList = selectList;
            model.ApproverEmployeeList = selectList;
            model.ApprovalLevelList = levelList;

            model.EmployeeId = EmployeeId;
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult getApprovalListDashboard([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                List<LeaveApproversViewModel> List_ViewModel = new List<LeaveApproversViewModel>();

                //get active listing from [leave.LeaveApprovers]
                var officeList = employeeSPService.GetDataWithoutParameter("leave.SP_GetEmployeeWiseAllLeaveApproverLevelList");
                List_ViewModel = officeList.Tables[0].AsEnumerable()
                .Select(row => new LeaveApproversViewModel()
                {
                    EmployeeId = row.Field<long>("EmployeeId"),
                    ApplicantDetail = row.Field<string>("ApplicantDetail"),
                    TotalApprovalLevel = row.Field<int>("TotalApprovalLevel")
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult LeaveApproversIndividualUpdate(long EmployeeId)
        {
            int result = 0;
            string message = string.Empty;
            try
            {
                var employee = employeeService.GetByEmpId(EmployeeId);

                var param1 = new { employeeId = employee.EmployeeId, desinationID = employee.EmployeeRank };
                var leaveApproversConfig = employeeSPService.GetDataWithParameter(param1, "leave.SP_LeaveApproversIndividualUpdate");
                result = 1;
                message = "Leave Approvers Individual Update successfull";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Leave Approvers Individual Update failed";
            }
            return Json(new { result = result,messahe=message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getApprovalDetailListDashboard([DataSourceRequest]DataSourceRequest request, long EmployeeId)
        {
            try
            {
                //var employee = employeeService.GetByEmpId(EmployeeId);
                //var param1 = new { employeeId = employee.EmployeeId, desinationID = employee.EmployeeRank };
                //var leaveApproversConfig = employeeSPService.GetDataWithParameter(param1, "SP_LeaveApproversIndividualUpdate");

                List<LeaveApproversViewModel> List_ViewModel = new List<LeaveApproversViewModel>();

                var param = new { EmployeeId = EmployeeId };
                var officeList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetEmployeeWiseAllLeaveApproversList");

                List_ViewModel = officeList.Tables[0].AsEnumerable()
                .Select(row => new LeaveApproversViewModel()
                {
                    ID = row.Field<int>("ID"),
                    EmployeeId = row.Field<long>("ApplicantId"),
                    ApplicantDetail = row.Field<string>("ApplicantDetail"),
                    ApprovalLevel = row.Field<int>("ApprovalLevel"),
                    ApproverEmpId = row.Field<long>("ApprovalEmployeeId"),
                    ApproverDetail = row.Field<string>("ApproverDetail"),
                    ApproverOfficeTypeId = row.Field<int>("ApproverOfficeTypeId"),
                    ApproverOfficeId = row.Field<int>("ApproverOfficeId"),
                    ApproverDepartmentId = row.Field<int>("ApproverDepartmentId"),
                    ApproverDesignationId = row.Field<int>("ApproverDesignationId"),
                    ApproverSectionId = row.Field<int>("ApproverSectionId"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("DesignationName"),

                    //ApproverOfficeTypeList = getApproverOfficeTypeList(),
                    //ApproverOfficeList = getApproverOfficeList(row.Field<int>("ApproverOfficeTypeId")),
                    //ApproverDepartmentList = getApproverDepartmentList(row.Field<int>("ApproverOfficeTypeId")),
                    //ApproverDesignationList = getApproverDesignationList(),
                    //ApproverEmployeeList = getApproverEmployeeList(row.Field<int>("ApproverOfficeId"), row.Field<int>("ApproverDepartmentId"), row.Field<int>("ApproverDesignationId"))
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult ConfigurationDelete(int ID)
        {
            var result = 0;
            try
            {
                var entity = leaveApproversService.Get(x => x.IsActive == true && x.ID == ID);
                entity.IsActive = false;
                entity.UpdateUser = LoggedInEmployeeId;
                entity.UpdateDate = DateTime.UtcNow;
                entity.ManualUpdated = true;
                leaveApproversService.Update(entity);
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveConfiguration(LeaveApproversViewModel Entity)
        {
            var result = 0;
            var message = "";
            try
            {
                var levelExist = new List<LeaveApprovers>();
                if (Entity.ID > 0)
                {
                    levelExist = leaveApproversService.GetMany(x => x.ID != Entity.ID && x.EmployeeId == Entity.EmployeeId && x.ApproverEmpId == Entity.ApproverEmpId && x.IsActive == true).ToList();
                }
                else
                {
                    levelExist = leaveApproversService.GetMany(x => x.EmployeeId == Entity.EmployeeId && x.ApproverEmpId == Entity.ApproverEmpId && x.IsActive == true ).ToList();
                }

                var employee = employeeService.GetByEmpId(Entity.EmployeeId);

                if (!levelExist.Any() && employee != null)
                {
                    if (Entity.ID > 0)
                    {
                        var model = leaveApproversService.Get(x => x.IsActive == true && x.ID == Entity.ID);
                        // model.ApprovalLevel = Entity.ApprovalLevel;
                        // model.ID = Entity.ID;
                        model.ApprovalLevel = Entity.ApprovalLevel;
                        model.ApproverEmpId = Entity.ApproverEmpId;
                        model.ApproveOfficeId = Entity.ApproverOfficeId;
                        model.ApproveDesignationId = Entity.ApproverDesignationId;
                        model.ApproveDepartmentId = Entity.ApproverDepartmentId;
                        model.ManualUpdated = true;
                        model.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        model.UpdateDate = DateTime.UtcNow;
                        leaveApproversService.Update(model);
                        message = "Approval level updated successfully";
                    }
                    else
                    {
                        var model = new LeaveApprovers();
                        model.EmployeeId = Entity.EmployeeId;
                        model.EmployeeOfficeId = employee.OfficeId;
                        model.EmployeeDepartmentId = employee.DepartmentId;
                        model.EmployeeDesignationId = Convert.ToInt32(employee.EmployeeRank);

                        model.ApprovalLevel = Entity.ApprovalLevel;
                        model.ApproverEmpId = Entity.ApproverEmpId;
                        model.ApproveOfficeId = Entity.ApproverOfficeId;
                        model.ApproveDesignationId = Entity.ApproverDesignationId;
                        model.ApproveDepartmentId = Entity.ApproverDepartmentId;
                        model.ManualUpdated = true;
                        model.IsActive = true;
                        model.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                        model.CreateDate = DateTime.UtcNow;
                        leaveApproversService.Create(model);
                        message = "Approval level saved successfully";
                    }
                    result = 1;
                }
                else {
                    result = 0;
                    message = "Approver already exists";
                }
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(new { result=result, message= message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LeaveApprovalReConfigure()
        {
            var result = 0;
            var message = String.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
            {
                try
                {
                    var procedures =employeeSPService.GetDataWithoutParameter("leave.SP_LeaveApproversReconfigure");
                    scope.Complete();
                    result = 1;
                    message = "Approvers Re-Configured Successfully";

                }
                catch (Exception ex)
                {
                    result = 0;
                    message = "Approvers Re-Configure Failed";
                    scope.Dispose();
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getOfficeList(int OfficeTypeId)
        {
            var list = getApproverOfficeList(OfficeTypeId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDepartmentList(int OfficeTypeId)
        {
            var list = getApproverDepartmentList(OfficeTypeId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getOfficeEmployeeList(int ApproverOfficeId, int ApproverDepartmentId, int ApproverDesignationId)
        {

            var param = new { OfficeId = ApproverOfficeId, DepartmentId = ApproverDepartmentId, EmployeeRank = ApproverDesignationId };
            var leavepproverList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetDesignationWiseLeaveApprovers");

            var list = leavepproverList.Tables[0].AsEnumerable().Select(row => new SelectListItem()
            {
                Text = row.Field<string>("EmployeeCode") + " - " + row.Field<string>("EmployeeName"),
                Value = Convert.ToString(row.Field<long>("EmployeeId"))
            }).ToList();

            var viewList = new List<SelectListItem> {
               new SelectListItem { Text = "Please Select", Value = "" },
            };

            viewList.AddRange(list);

            return Json(viewList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        public List<SelectListItem> getApproverOfficeTypeList()
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var TypeList = officeTypeService.GetMany(o => o.IsActive == true).ToList();
            var list = TypeList.AsEnumerable().Select(row => new SelectListItem()
            {
                Text = row.OfficeTypeName,
                Value = row.OfficeTypeId.ToString()
            }).ToList();
            var viewList = new List<SelectListItem>();
            viewList.Add(PleaseSelect);
            viewList.AddRange(list);
            return viewList;
        }

        private List<SelectListItem> getApproverDesignationList()
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var TypeList = officeDesignationService.GetMany(o => o.IsActive == true).ToList();
            var list = TypeList.AsEnumerable().Select(row => new SelectListItem()
            {
                Text = row.OffcDesignName,
                Value = row.OfficeDesignationId.ToString()
            }).ToList();
            var viewList = new List<SelectListItem>();
            viewList.Add(PleaseSelect);
            viewList.AddRange(list);
            return viewList;
        }

        private List<SelectListItem> getApproverOfficeList(int p)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var TypeList = officeService.GetMany(o => o.IsActive == true && o.OfficeTypeId == p).ToList();
            var list = TypeList.AsEnumerable().Select(row => new SelectListItem()
            {
                Text = row.OfficeName,
                Value = row.OfficeId.ToString()
            }).ToList();
            var viewList = new List<SelectListItem>();
            viewList.Add(PleaseSelect);
            viewList.AddRange(list);
            return viewList;
        }

        private List<SelectListItem> getApproverDepartmentList(int p)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var TypeList = employeeDepartmentService.GetMany(o => o.IsActive == true).ToList();//&& o.OfficeTypeId == p
            var list = TypeList.AsEnumerable().Select(row => new SelectListItem()
            {
                Text = row.DepartmentName,
                Value = row.DepartmentId.ToString()
            }).ToList();
            var viewList = new List<SelectListItem>();
            viewList.Add(PleaseSelect);
            viewList.AddRange(list);
            return viewList;
        }

        #endregion
    }
}