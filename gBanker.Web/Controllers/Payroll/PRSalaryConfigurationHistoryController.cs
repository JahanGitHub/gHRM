
#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Payroll;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class PRSalaryConfigurationHistoryController : BaseController
    {
        #region Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;

        public PRSalaryConfigurationHistoryController(
            IEmployeeSPService employeeSPService,
            IEmployeeService employeeService,
            IOfficeService officeService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeDesignationService employeeDesignationService,
            IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService
            )
        {
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;            
            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region ActionResult

        public ActionResult Index()
        {
            var model = new PRSalaryConfigurationViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            MapDropDown(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetExistingSalaryConfigurationListbyEmployeeCode(string employeeCode)
        {
            try
            {
                var dataList = new List<View_EmployeeSalaryConfiguration>();
                dataList = viewSalaryConfigurationService.GetEmployeeSalaryConfigurationListbyCode(employeeCode);
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim());
                var officeInfo = officeService.Get(b => b.OfficeId == employeeInfo.OfficeId);
                var joiningDate = Convert.ToDateTime(employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                var confirmationDate = Convert.ToDateTime(employeeInfo.ConfirmationDate).ToString("dd-MMM-yyyy");
                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employeeInfo.DepartmentId)).DepartmentName;
                var designationName = employeeDesignationService.GetById(Convert.ToInt32(employeeInfo.DesignationId)).DesignationName;
                if (dataList.Count <= 0)
                {
                    dataList = GenerateDataList(employeeCode);
                }
                return Json(new { Result = "OK", dataList, Message = "OK", JoiningDate = joiningDate, ConfirmationDate = confirmationDate, DepartmentName = departmentName, DesignationName = designationName, OfficeId = officeInfo.OfficeId, OfficeLocationId = officeInfo.OfficeLocationId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadEmployeeSalaryHistory(string employee_code)
        {
            try
            {
                var employeeInfo = employeeService.GetByCode(employee_code.Trim());
                int EmployeeId = Convert.ToInt32(employeeInfo.EmployeeId);

                List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
                var param = new { EmployeeId = EmployeeId };
                var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeTypeWiseComponentConfigurationHistory");
                List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new PRSalaryConfigurationViewModel
                   {
                       EmployeeID = row.Field<long>("EmployeeId"),
                       SalaryAmount = row.Field<decimal>("SalaryAmount"),
                       CreateDate = row.Field<DateTime>("CreateDate"),
                       CreateDateMsg = row.Field<string>("CreateDateMsg"),
                       CreateDateTime = row.Field<string>("CreateDateTime")

                   }).ToList();



                return Json(new { Result = "OK", List_ViewModel, Message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult LoadEmployeeSalaryHistoryDetails(int EmployeeId, string CreateDateMsg)
        {
            try
            {
                List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
                var param = new { EmployeeId = EmployeeId, CreateDateMsg = CreateDateMsg };
                var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeTypeWiseComponentConfigurationHistoryDetails");
                List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new PRSalaryConfigurationViewModel
                   {
                       EmployeeID = row.Field<long>("EmployeeId"),
                       CreateDate = row.Field<DateTime>("CreateDate"),
                       CreateDateMsg = row.Field<string>("CreateDateMsg"),
                       //IsActive = row.Field<bool>("IsActive"),
                       PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
                       PRComponentID = row.Field<int>("PRComponentID"),
                       ComponentAmount = row.Field<decimal>("ComponentAmount"),
                       EffectiveStartDate = row.Field<DateTime>("EffectiveStartDate"),
                       EffectiveEndDate = row.Field<DateTime>("EffectiveEndDate"),
                       EffectiveStartDateMsg = row.Field<string>("EffectiveStartDateMsg"),
                       EffectiveEndDateMsg = row.Field<string>("EffectiveEndDateMsg"),
                       ComponentCategory = row.Field<string>("ComponentCategory"),
                       TransactionType = row.Field<string>("TransactionType"),
                       ComponentName = row.Field<string>("ComponentName"),


                   }).ToList();


                return Json(new { Result = "OK", List_ViewModel, Message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region Methods

        public void MapDropDown(PRSalaryConfigurationViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            model.EmployeeSalaryType = commonStaticDropDown.SalaryStructuredTypeList();
            model.OverTimeList = commonStaticDropDown.YesNoDropDown_bool();
            model.SalaryScaleList = commonStaticDropDown.NumberSerialDropDown(0, 15);
            model.MonthList = commonStaticDropDown.MonthList();
            model.GradeList = CommonDynamicDropDown.GetEmployeeGradeList();
            model.SalaryGenerationTypeList = commonStaticDropDown.SalaryGenerationTypeList();
            model.BankList = CommonDynamicDropDown.PayrollBankNameWithCode();

            var yearList = new List<SelectListItem>();
            yearList.Add(pleaseSelect);

            for (int i = 0; i < 2; i++)
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
            model.IncrementYearFromList = yearList;

        }

        private List<View_EmployeeSalaryConfiguration> GenerateDataList(string employeeCode)
        {
            var empList = new List<View_EmployeeSalaryConfiguration>();
            // var empdataList = employeeService.GetAll().Where(p => p.EmployeeCode == employeeCode && p.IsActive==true).ToList();

            var param = new { EmployeeCode = Convert.ToString(employeeCode) };
            var employeeData = employeeSPService.GetDataWithParameter(param, "prl.SP_GetPayroll_EmployeebyEmployeeCode");

            var empdataList = employeeData.Tables[0].AsEnumerable()
            .Select(row => new EmployeeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeNameBng = row.Field<string>("EmployeeNameBng"),
                EmployeeTypeId = row.Field<int?>("EmployeeTypeId"),
                EmployeeStatusId = Convert.ToInt32(row.Field<int?>("EmployeeStatusId")),
                EmployeeStatusName = row.Field<string>("EmployeeStatusName"),
                EmployeeStatusValue = row.Field<string>("EmployeeStatusValue"),
                IsSalaryApplicable = row.Field<bool?>("IsSalaryApplicable"),
                DepartmentName = row.Field<string>("DepartmentName"),
                DesignationName = row.Field<string>("OffcDesignName"),
                FirstJoiningDateMsg = row.Field<string>("FirstJoiningDate"),
                ConfirmationDateMsg = row.Field<string>("ConfirmationDate"),
                BankAccountNo = row.Field<string>("BankAccountNo")
            }).ToList();

            foreach (var item in empdataList)
            {
                var data = new View_EmployeeSalaryConfiguration();
                data.OfficeID = Convert.ToInt32(item.OfficeId);
                data.EmployeeID = item.EmployeeId;
                data.PRComponentId = 0;
                data.EmployeeTypeName = "";
                data.ComponentGroupName = "";
                data.ComponentName = "";
                data.IsActive = true;
                data.CalculatedAmount = 0;
                data.ComponentType = "";
                data.RatioBasedOn = "";
                data.EmployeeTypeId = item.EmployeeTypeId == null ? 0 : item.EmployeeTypeId.Value;
                data.EffectiveStartDate = DateTime.Now.ToString("dd-MMM-yyyy");//DateTime.Today.ToString();
                var dateAdv = DateTime.Now.AddYears(3);
                data.EffectiveEndDate = dateAdv.ToString("dd-MMM-yyyy");
                data.GrossSalary = 0;
                data.BankAccountNo = "";
                data.Step = 0;
                data.GradeId = 0;
                data.LogInTime = "10:00:00";
                data.LogOutTime = "18:00:00";
                data.LastLoginTime = "10:00:00";
                data.IsOverTime = false;
                //data.OvertimeHour = 0;
                //data.IncrementMonth = 0;
                data.EmployeeCode = item.EmployeeCode;
                data.EmployeeName = item.EmployeeName;
                data.EmployeeNameBng = item.EmployeeNameBng;
                data.EmployeeStatusId = item.EmployeeStatusId;
                data.EmployeeStatusName = item.EmployeeStatusName;
                // data.EmployeeStatusName = ReturnEmployeeStatusReverse(item.EmployeeStatus.Trim());
                //data.DepartmentName = item.DepartmentName;
                //data.DesignationName = item.DesignationName;

                empList.Add(data);
            }
            return empList;
        }

        #endregion
    }
}