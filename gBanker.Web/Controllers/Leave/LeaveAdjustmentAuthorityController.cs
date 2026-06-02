using System;
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
using gHRM.Web.DropDownService;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{
    public class LeaveAdjustmentAuthorityController : BaseController
    {
        #region Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeService employeeService;
        private readonly ILeaveAdjustmentAuthorityService leaveAdjustmentAuthorityService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;


        public LeaveAdjustmentAuthorityController(
               IEmployeeSPService employeeSPService
              , IEmployeeDepartmentService employeeDepartmentService
              , ILeaveAdjustmentAuthorityService leaveAdjustmentAuthorityService
              ,IEmployeeService employeeService
            )
        {
            this.employeeSPService = employeeSPService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.leaveAdjustmentAuthorityService = leaveAdjustmentAuthorityService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = employeeService;
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            var model = new LeaveAdjustmentAuthorityViewModel();
            MapDropdownForAdjustmentAuthority(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetDepartmentByOfficeTypeId(int OfficeTypeId)
        {
            var departmentType_items = new List<SelectListItem>();
            departmentType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            try
            {
                if (OfficeTypeId != 3)
                {
                    OfficeTypeId = 1;
                }
                var departmentList = employeeDepartmentService.GetAll().Where(d => d.IsActive == true && d.OfficeTypeId == OfficeTypeId);
                var viewdeaprtmentType = departmentList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.DepartmentId.ToString(),
                    Text = string.Format("{0}", x.DepartmentName)
                });

                departmentType_items.AddRange(viewdeaprtmentType);

            }
            catch (Exception e)
            {

            }
            return Json(departmentType_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListByOfficeIdDesignationId(int OfficeId, int OfficeDesignationId)
        {
            try
            {
                var EASS_List = new List<SelectListItem>();
                if (OfficeDesignationId > 0)
                {
                    var employeeRank = Convert.ToString(OfficeDesignationId).Trim();

                    var param = new { OfficeId = OfficeId, EmployeeRank = employeeRank };
                    var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_Get_Employees_ByOfficeIdAndEmployeeRank");

                    var employees = employeeList.Tables[0].AsEnumerable().Select(row => new SelectListItem()
                    {
                        Text = row.Field<string>("EmployeeCode") + " - " + row.Field<string>("EmployeeName"),
                        Value = Convert.ToString(row.Field<long>("EmployeeId"))
                    }).ToList();

                    EASS_List.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    EASS_List.AddRange(employees);
                }
                else
                {
                    EASS_List.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                }
                return Json(new { data = EASS_List }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AdjustmentAuthorityList([DataSourceRequest]DataSourceRequest request)
        {
            var empOffcDesigList = employeeSPService.GetDataWithoutParameter("leave.SP_GetAdjustmentAuthority");
            var List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
           .Select(row => new EmployeeViewModel
           {
               SlNo = row.Field<string>("rowSl"),
               Id = row.Field<int>("Id"),
               EmployeeId = row.Field<long>("EmployeeId"),
               EmployeeName = row.Field<string>("EmployeeName"),
               EmployeeCode = row.Field<string>("EmployeeCode"),

           }).ToList();

            DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteAdjustment(int Id)
        {
            int result = 0;
            var message = "";
            try
            {
                var model = leaveAdjustmentAuthorityService.GetById(Id);
                model.IsActive = false;

                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                leaveAdjustmentAuthorityService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAdjustmentAuthority(LeaveAdjustmentAuthorityViewModel AdjustmentlAuthority)
        {
            var result = 0;
            var message = string.Empty;

            try
            {
                long empId = AdjustmentlAuthority.EmployeeId;
                var isEmpAlreadyInAuthority = leaveAdjustmentAuthorityService.GetMany(x => x.IsActive == true && x.EmployeeId == empId).FirstOrDefault();
                if (isEmpAlreadyInAuthority == null)
                {
                    var employeeInfo = employeeService.GetEmployeeById(empId);

                    if (employeeInfo!=null)
                    {
                        var entity = new LeaveAdjustmentAuthority();
                        entity.EmployeeId = AdjustmentlAuthority.EmployeeId;
                        entity.EmployeeCode = employeeInfo.EmployeeCode;
                        entity.IsActive = true;
                        entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.UtcNow;
                        entity.UpdateDate = DateTime.UtcNow;

                        //let's insert into [leave.LeaveAdjustmentAuthority]
                        leaveAdjustmentAuthorityService.Create(entity);
                        result = 1;
                        message = "Save Successfull";
                    }
                }
                else
                {
                    result = 0;
                    message = "Employee already exists as adjustment authority";
                }
            }
            catch (Exception ex)
            {
                message = "Save Failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        public void MapDropdownForAdjustmentAuthority(LeaveAdjustmentAuthorityViewModel model)
        {

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();// officeType_items;
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();//zone_items;
            model.AreaList = commonDynamicDropDown.ddlInitial();//area_items;
            model.UnitList = commonDynamicDropDown.ddlInitial();//unit_items;
            model.OfficeDesignationNameList = commonDynamicDropDown.GetAllOfficeDesignationList();//listOfdesignationname;
            model.EmployeeDepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();//deptId;
            model.EmployeeCodeList = commonDynamicDropDown.ddlInitial(); ;
        }

        #endregion
    }
}