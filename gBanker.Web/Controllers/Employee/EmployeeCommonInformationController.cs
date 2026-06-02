using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;

namespace gHRM.Web.Controllers
{
    public class EmployeeCommonInformationController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;

        public EmployeeCommonInformationController(
              IEmployeeSPService employeeSpService
            , IEmployeeService employeeService
            , IDocumentTypeService documentTypeService
            , IOfficeService officeService
            , IOfficeTypeService officeTypeService
            )
        {
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.documentTypeService = documentTypeService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            var result = string.Empty;
            try
            {
                var employeeCode = employee_code;
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();

                var param = new { EmployeeCode = employeeCode };
                var empList = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        OfficeId = row.Field<int>("OfficeId"),
                        OfficeName = row.Field<string>("OfficeName"),
                        DepartmentName = row.Field<string>("DepartmentName"),
                        DesignationName = row.Field<string>("DesignationName"),
                        EmployeeTypeId = row.Field<int?>("EmployeeTypeId"),
                        EmployeeStatus = row.Field<string>("EmployeeStatus"),
                        EmployeeStatusName=row.Field<string>("StatusName"),
                        GrossSalary = row.Field<decimal>("GrossSalary"),
                        TotalEarnings = row.Field<decimal>("TotalEarnings"),
                        OfficeDesignationName = row.Field<string>("OffcDesignName")
                    }).ToList();

                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AutoCompleteEmployee(string Term)
        {
            var obj = employeeService.GetMany(x => x.EmployeeCode.StartsWith(Term) || x.EmployeeName.StartsWith(Term))
                .Select(s => new { s.EmployeeId, EmployeeName = "(" + s.EmployeeCode + ") " + s.EmployeeName });
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }
    }
}