using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.Repository.Basic;
using gHRM.Service;
using gHRM.Service.Basic;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Payroll
{
    public class EmployeeAllowenceController : BaseController
    {
        private readonly IEmployeeAllowenceService employeeAllowanceService;
        private readonly IEmployeeGradeListService employeeGradeListService;

        private CommonDynamicDropDown commonDynamicDropDown;


        public EmployeeAllowenceController(IEmployeeAllowenceService employeeAllowenceService, IEmployeeGradeListService employeeGradeListService)
        {
            this.employeeAllowanceService = employeeAllowenceService;
            this.employeeGradeListService = employeeGradeListService;

            commonDynamicDropDown = new CommonDynamicDropDown();

        }


        // Index: EmployeeAllowence
        public ActionResult Index()
        {
            var model = new EmployeeAllowanceViewModel();
            MapDropDownList(model);
            return View(model);
            //return View();
        }


        // Get List 
        public JsonResult GetEmployeeAllowanceList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allowanceList = employeeAllowanceService.GetMany(p => p.IsActive == true).ToList();
                var view_AllowanceList = allowanceList.AsEnumerable().Select(p => new EmployeeAllowanceViewModel()
                {
                    Id = p.Id,
                    EmpGradeId = p.GradeId,
                    EmpStatusId = p.EmployeeStatusId,
                    Allowance = p.Allowance,
                    ComponentId = p.ComponentId

                }).ToList();

                var currentPageRecords = view_AllowanceList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_AllowanceList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        // Save Data
        public JsonResult SaveEmployeeAllowance(EmployeeAllowence obj)
        {
            var result = 0;
            var message = "";

            try
            {
                obj.IsActive = true;
                obj.CreateBy = (int)LoggedInEmployeeId;
                obj.CreateDate = DateTime.Now;
                if (obj.Allowance > 0)
                {
                    employeeAllowanceService.Create(obj);

                    message = "Saved successfully";
                    return Json(new { result = 1, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = "Please Input Valid Data";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        // Edit/ Update 

        public JsonResult UpdateEmployeeAllowance(EmployeeAllowence obj)
        {
            var result = 0;
            var message = "";

            try
            {
                if (obj.Allowance > 0)
                {
                    obj.IsActive = true;
                    obj.UpdateBy = (int)LoggedInEmployeeId;
                    obj.UpdateDate = DateTime.Now;
                    employeeAllowanceService.Update(obj);
                    result = 1;
                    message = "Updated successfully";
                }
                else
                {
                    message = "Update Error! ";
                }

            }
            catch (Exception)
            {

                result = 0;
                message = "Update denied";
            }


            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        // Delete 
        public JsonResult DeleteEmpAllowance(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeAllowanceService.GetById(Id);
                model.IsActive = false;
                employeeAllowanceService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        // Get Join List 
        public JsonResult GetAllAllowance(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allowanceList = employeeAllowanceService.GetAllowanceList().ToList();

                var view_AllowanceList = allowanceList.AsEnumerable().Select(p => new EmployeeAllowanceViewModel()
                {
                    Id = p.Id,
                    GradeName = p.GradeName,
                    StatusName = p.StatusName,
                    ComponentName = p.ComponentName,
                    Allowance = p.Allowance,
                    ComponentId = p.ComponentId,
                    EmpGradeId = p.EmpGradeId ?? 0,
                    EmpStatusId = p.EmpStatusId ?? 0,
                    RatioOn = p.RatioOn

                }).ToList();


                var currentPageRecords = view_AllowanceList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_AllowanceList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




        #region Private Method


        private void MapDropDownList(EmployeeAllowanceViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            model.GradeList = commonDynamicDropDown.GetEmployeeGradeList();
           // model.EmployeeTypeList = commonDynamicDropDown.ddlEmployeeType();
            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: true);
            model.ComponentList = commonDynamicDropDown.PayrollComponentXPRComponent();
        }


        #endregion



    }
}