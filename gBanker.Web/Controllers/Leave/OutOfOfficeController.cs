using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Leave;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Leave
{
    public class OutOfOfficeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IOutOfOfficeService outOfOfficeService;
        private readonly IEmployeeSPService employeeSpService;

        private readonly CommonDynamicDropDown commonDynamicDropDown;

        public OutOfOfficeController(IEmployeeService employeeService, IOutOfOfficeService outOfOfficeService, IEmployeeSPService employeeSpService)
        {

            this.employeeService = employeeService;
            this.outOfOfficeService = outOfOfficeService;
            this.employeeSpService = employeeSpService;

            commonDynamicDropDown = new CommonDynamicDropDown();

        }


        // GET: OutOfOffice
        public ActionResult Index()
        {
            var model = new OutOfOfficeViewModel();
            MapDropDownList(model);

          //  model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
          //  model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;

            return View(model);
        }


        // Save
        public JsonResult EmployeeOutOfficeSave(OutOfOffice obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = new OutOfOffice();

                if (outOfOfficeService.GetMany(x => x.IsActive && x.EmployeeId == obj.EmployeeId
                && x.FromDate == obj.FromDate).Any())
                {
                    message = "Data Already Exists";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    model.OutofOfficeId = obj.OutofOfficeId;
                    model.EmployeeId = obj.EmployeeId;
                    model.FromDate = obj.FromDate;
                    model.ToDate = obj.ToDate;
                    model.IsActive = obj.IsActive = true;
                    model.Category = obj.Category;
                    model.CreateDate = obj.CreateDate = DateTime.Today;
                    model.CreateUser = obj.CreateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    outOfOfficeService.Create(model);

                    message = "Data Save Successfully";
                    return Json(new { result = 1, message = message }, JsonRequestBehavior.DenyGet);
                }


                //  return Json(new { result = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        // Get List 
        public ActionResult GetOutofOfficeList([DataSourceRequest] DataSourceRequest request)
        
        {
            try
            {
                List<OutOfOfficeViewModel> List_ViewModel = new List<OutOfOfficeViewModel>();


                int Result = 0;


                var outOfficeList = employeeSpService.GetDataWithoutParameter("prl.SP_EmployeeOutOfOfficeDetailList");
                List_ViewModel = outOfficeList.Tables[0].AsEnumerable()
                .Select(row => new OutOfOfficeViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    OutofOfficeId = row.Field<int>("OutofOfficeId"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DateFrom = row.Field<string>("FromDate"),
                    DateTo = row.Field<string>("ToDate"),
                    EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                    EmployeeCurrentDesignation = row.Field<string>("OffcDesignName"),                    
                    Category = row.Field<string>("Category"),
                    EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                    CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                }).ToList();


                
                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // Get For Edit(id) 
        public JsonResult GetById(int Id)
        {
            List<OutOfOfficeViewModel> List_Employee = new List<OutOfOfficeViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "prl.SP_GetEmployeeOutOfOfficeById");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new OutOfOfficeViewModel
               {
                   OutofOfficeId = row.Field<int>("OutofOfficeId"),
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   DateFrom = row.Field<string>("FromDate"),
                   DateTo = row.Field<string>("ToDate"),
                   EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                   EmployeeCurrentDesignation = row.Field<string>("OffcDesignName"),
                   Category = row.Field<string>("Category"),
                   EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                   CurrentOfficeType = row.Field<string>("OfficeTypeName"),
               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }


        // Delete

        public JsonResult DeleteOutofOffice(int Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var outofOfficeList = employeeSpService.GetDataWithParameter(param, "SP_Delete_OutofOffice");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }




        #region Private Method

        private void MapDropDownList(OutOfOfficeViewModel model)
        {



            List<SelectListItem> ObjItem = new List<SelectListItem>()
            {
          new SelectListItem {Text="Select",Value="Plaese Select",Selected=true },
          new SelectListItem {Text="Late",Value="Late" },
          new SelectListItem {Text="Absent",Value="Absent"},
          new SelectListItem {Text="Outside Office",Value="Outside Office"},
          //new SelectListItem {Text="Sick",Value="Sick" },
            };

            model.LeaveCategoryList = ObjItem;


            // model.LeaveCategoryList = commonDynamicDropDown.GetAllLeaveCategoryList();

        }

        #endregion



    }
}