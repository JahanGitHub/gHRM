using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.CostCenter
{
    public class CostCenterController : BaseController
    {

        private readonly IEmployeeDocumentService employeeDocumentService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeReportOptionService employeeReportOptionService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IEmployeeTranningDropDownService employeeTranningDropDownService;
        private readonly IEducationDegreeService educationDegreeService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeePromotionService employeePromotionService;


        public CommonDynamicDropDown commonDynamicDropDown;


        public CostCenterController(
            IEmployeeService employeeService,
            IEmployeeSPService employeeSpService,
            IOfficeService officeService,
            IOfficeTypeService officeTypeService,
            IEmployeeStatusService employeeStatusService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeReportOptionService employeeReportOptionService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployementTypeService employementTypeService,
            IEmployeeTrainingService employeeTrainingService,
            IEmployeeDocumentService employeeDocumentService,
            IEmployeeTranningDropDownService employeeTranningDropDownService,
            IEducationDegreeService educationDegreeService,
            IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService,
            IEmployeePromotionService employeePromotionService

            )
        {
            this.employeeDocumentService = employeeDocumentService;
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeReportOptionService = employeeReportOptionService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.employeeTrainingService = employeeTrainingService;
            this.employeeTranningDropDownService = employeeTranningDropDownService;
            this.educationDegreeService = educationDegreeService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeePromotionService = employeePromotionService;
            commonDynamicDropDown = new CommonDynamicDropDown();


        }


        public ActionResult Index()
        {
            return View();
        }




        public ActionResult GetCostCenterTypes()
        {
            try
            {
                var result = employeeSpService.GetDataWithoutParameter("sp_GetCostCenterTypes");
                var types = result.Tables[0].AsEnumerable()
                    .Select(row => row.Field<string>("Type"))
                    .Distinct()
                    .ToList();

                return Json(types, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<string>(), error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ✅ Save Cost Center selection
        [HttpPost]
        public ActionResult SaveCostCenter(string CostCenterName, string Type)
        {
            var result = 0;
            try
            {
                if (SessionHelper.LoggedInEmployeeID == null)
                {
                    return Json(new { result = 0, Message = "Session expired. Please log in again." }, JsonRequestBehavior.AllowGet);
                }

                var param = new
                {
                    CostCenterName = CostCenterName,
                    Type = Type,
                    CreateBy = SessionHelper.LoggedInEmployeeID
                };

                employeeSpService.GetDataWithParameter(param, "sp_SaveCostCenterSelection");
                result = 1;

                return Json(new { result = result, Message = "Saved successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // ✅ Load saved entries into grid
        public ActionResult GetSavedCostCenters()
        {
            try
            {
                var result = employeeSpService.GetDataWithoutParameter("sp_GetSavedCostCenters");

                var list = result.Tables[0].AsEnumerable().Select(row => new CostCenterViewModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    CostCenterName = row.Field<string>("Cost_Center_Name"),
                    Type = row.Field<string>("Type")
                }).ToList();

                return Json(new { data = list, total = list.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<CostCenterViewModel>(), total = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCostCenter(int Id, string CostCenterName, string Type)
        {
            try
            {
                if (SessionHelper.LoggedInEmployeeID == null)
                {
                    return Json(new { result = 0, Message = "Session expired. Please log in again." });
                }

                var param = new
                {
                    Id = Id,
                    CostCenterName = CostCenterName,
                    Type = Type,
                    ModifiedBy = SessionHelper.LoggedInEmployeeID
                };

                employeeSpService.GetDataWithParameter(param, "sp_UpdateCostCenterSelection");

                return Json(new { result = 1, Message = "Updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message });
            }
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteCostCenter(int Id)
        {
            try
            {
                var param = new { Id = Id };
                employeeSpService.GetDataWithParameter(param, "sp_DeleteCostCenterSelection");

                return Json(new { result = 1, Message = "Deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message });
            }
        }


        //------------------------------------

        public ActionResult EmpWiseCostCenterName()
        {
            return View();
        }

        // Employee info by code (same logic as SalaryDetails)
        public ActionResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                var param = new { EmployeeCode = employee_code };
                var result = employeeSpService.GetDataWithParameter(param, "sp_GetEmployeeInfoByCode");

                if (result.Tables[0].Rows.Count > 0)
                {
                    var emp = result.Tables[0].Rows[0];
                    return Json(new
                    {
                        result = 1,
                        data = new[]
                        {
                        new {
                            EmployeeName = emp["EmployeeName"].ToString()
                        }
                    }
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = 0, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Autocomplete for Cost Centers
        public ActionResult GetCostCenters(string term)
        {
            try
            {
                var param = new { Term = term ?? "" };
                var result = employeeSpService.GetDataWithParameter(param, "sp_GetCostCenters");

                var list = result.Tables[0].AsEnumerable()
                    .Select(r => new {
                        id = r.Field<int>("id"),
                        Cost_Center_Name = r.Field<string>("Cost_Center_Name")
                    }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }


        // Save mapping
        [HttpPost]
        public ActionResult SaveEmployeeCostCenter(string EmployeeCode, string EmployeeName, string CostCenterName)
        {
            try
            {
                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    CostCenter = CostCenterName,
                    Type = "",
                    SL = 0
                };

                var result = employeeSpService.GetDataWithParameter(param, "sp_SaveEmployeeCostCenter");

                return Json(new { result = 1, Message = "Saved successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // List for grid
        public ActionResult GetEmployeeCostCenterList()
        {
            try
            {
                var result = employeeSpService.GetDataWithoutParameter("sp_GetEmployeeCostCenterList");

                var list = result.Tables[0].AsEnumerable().Select(row => new
                {
                    SL = row.Field<string>("SL"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CostCenterName = row.Field<string>("CostCenterName"),
                    CreatedDate = row.Field<DateTime>("CreatedDate").ToString("dd/MM/yyyy")
                }).ToList();

                return Json(new { data = list, total = list.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<object>(), total = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //--------------------------------------------------------------------
        public ActionResult TransferCostCenter()
        {
            return View(); // This will load TransferCostCenter.cshtml
        }
        [HttpPost]
        public ActionResult UpdateEmployeeCostCenter(string EmployeeCode, int FromCostCenterId, int ToCostCenterId)
        {
            try
            {
                if (SessionHelper.LoggedInEmployeeID == null)
                    return Json(new { result = 0, Message = "Session expired" });

                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    FromCostCenterId = FromCostCenterId,
                    ToCostCenterId = ToCostCenterId,
                    ModifiedBy = SessionHelper.LoggedInEmployeeID
                };

                var result = employeeSpService.GetDataWithParameter(param, "sp_UpdateEmployeeCostCenter");

                return Json(new { result = 1, Message = "Updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message });
            }
        }


        //-----------------------------------------------------------------------
        public ActionResult EditCostCenterSL()

        {
            return View();
        }

        public ActionResult GetSavedCostCentersSL()
        {
            try
            {
                var list = new List<CostCenterViewModel>();

                var result = employeeSpService.GetDataWithoutParameter("sp_GetSavedCostCentersSL");

                list = result.Tables[0].AsEnumerable().Select(row => new CostCenterViewModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    CostCenterName = row.Field<string>("Cost_Center_Name"),
                    SL = row.Field<string>("SL")
                }).ToList();

                return Json(new { data = list, total = list.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<CostCenterViewModel>(), total = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UpdateCostCenterSL(int Id, string CostCenterName, string SL)
        {
            try
            {
                var param = new
                {
                    Id = Id,
                    Cost_Center_Name = CostCenterName,
                    SL = SL,
                    ModifiedBy = SessionHelper.LoggedInEmployeeID ?? 0
                };

                var result = employeeSpService.GetDataWithParameter(param, "sp_UpdateCostCenterSL");

                return Json(new { result = 1, Message = "Updated successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }

public class CostCenterViewModel
    {
        public int Id { get; set; }
        public string CostCenterName { get; set; }
        public string SL { get; set; }
        public string Type { get; set; }
        public int TypeOrder { get; set; }
    }
}