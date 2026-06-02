using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Payroll
{
    public class ComponentPayrollController : BaseController
    {
        #region Variables
        //private readonly IVMServiceProviderService vmServiceProviderService;
        //private readonly IVMCarTypeService vmCarTypeService;
        private readonly IComponentPayrollService componentPayrollService;
        //private readonly IPRComponentService pRComponentService;
        //private readonly IEmployeeSPService employeeSpService;
        //private readonly IOfficeTypeService officeTypeService;
        //private readonly IEmployeeService employeeService;
        //private readonly IEmployeeDepartmentService employeeDepartmentService;
        //private readonly IEmployeeDesignationService employeeDesignationService;
        //private readonly ICompanyService companyService;

        public ComponentPayrollController(
            //IVMServiceProviderService vmServiceProviderService,
            //IVMCarTypeService vmCarTypeService,
            IComponentPayrollService componentPayrollService
            //IPRComponentService pRComponentService,
            //IEmployeeSPService employeeSpService,
            //IOfficeTypeService officeTypeService,
            //IEmployeeService employeeService,
            //IEmployeeDepartmentService employeeDepartmentService,
            //IEmployeeDesignationService employeeDesignationService,
            //ICompanyService companyService
        )
        {
            //this.vmServiceProviderService = vmServiceProviderService;
            //this.vmCarTypeService = vmCarTypeService;
            this.componentPayrollService = componentPayrollService;
            //this.pRComponentService = pRComponentService;
            //this.employeeSpService = employeeSpService;
            //this.officeTypeService = officeTypeService;
            //this.employeeService = employeeService;
            //this.employeeDepartmentService = employeeDepartmentService;
            //this.employeeDesignationService = employeeDesignationService;
            //this.companyService = companyService;
        }

        #endregion

        #region ActionMethods

        public ActionResult Index()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponent(model);
            return View(model);
        }

        #endregion

        #region HTTPRequest

        public JsonResult ListComponentPayroll(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var vmcar = componentPayrollService.GetAll().Where(t => t.IsActive == true && t.IsChangeable == true || t.IsChangeable == null);
            var listVMcartype = vmcar.AsEnumerable().Select(a => new ComponentPayroll()
            {
                Id = a.Id,
                ComponentName = a.ComponentName,
                ComponentCategory = a.ComponentCategory,
                IsChangeable = a.IsChangeable

            }).ToList();

            var currentPageRecords = listVMcartype.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = listVMcartype.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult InformationDelete(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = componentPayrollService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                componentPayrollService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveComponentPayroll(ComponentPayroll cp)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    componentPayrollService.GetAll()
                        .Where(
                            p => p.ComponentName.ToUpper().Trim() == cp.ComponentName.ToUpper().Trim())
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Component Name found, Save denied";
                }
                else
                {
                    var entity = new ComponentPayroll();
                    entity.ComponentName = cp.ComponentName;
                    entity.ComponentCategory = cp.ComponentCategory;
                    entity.IsChangeable = true;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    componentPayrollService.Create(entity);
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateComponentPayroll(ComponentPayroll cp)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   componentPayrollService.GetAll()
                       .Where(
                           p => p.Id != cp.Id &&
                               p.ComponentName.ToUpper().Trim() == cp.ComponentName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Car type found, Save denied";
                }
                else
                {
                    var entity = componentPayrollService.GetById(cp.Id);
                    entity.Id = cp.Id;
                    entity.ComponentName = cp.ComponentName;
                    entity.ComponentCategory = cp.ComponentCategory;
                    entity.IsChangeable = true;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    componentPayrollService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region MapDropDown
        private void MapDropdownForComponent(ComponentPayrollViewModel model)
        {
            var ComponentCategory = new List<SelectListItem>();
            ComponentCategory.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            ComponentCategory.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            ComponentCategory.Add(new SelectListItem() { Text = "Allowance", Value = "Allowance" });
            ComponentCategory.Add(new SelectListItem() { Text = "Deduction", Value = "Deduction" });
            ComponentCategory.Add(new SelectListItem() { Text = "Bonus", Value = "Bonus" });
            ComponentCategory.Add(new SelectListItem() { Text = "Loan", Value = "Loan" });
            ComponentCategory.Add(new SelectListItem() { Text = "Deposit", Value = "Deposit" });
            ComponentCategory.Add(new SelectListItem() { Text = "Deduction", Value = "Deduction" });
            model.ComponentCategoryList = ComponentCategory;

            var IsChangeable = new List<SelectListItem>();
            IsChangeable.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsChangeable.Add(new SelectListItem() { Text = "Yes", Value = "true" });
            IsChangeable.Add(new SelectListItem() { Text = "No", Value = "false" });
            model.IsChangeableList = IsChangeable;
        }

        #endregion
    }
}