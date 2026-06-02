using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Service;
using gHRM.Service.Basic;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.ViewModels.Discipline;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Discipline
{
    public class DiscPunishmentImpactController : Controller
    {
        #region Variables
        private readonly IEmployeeSPService employeeSPService;
        private readonly IPRSalaryConfigurationService prSalaryConfigurationService;
        private readonly IEmployeeService employeeService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeeGradeListService employeeGradeListService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeSalaryConfigurationHistoryService employeeSalaryConfigurationHistoryService;
        private readonly IBankNameService bankNameService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployeePromotionService employeePromotionService;

        private readonly IOfficeService officeService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;

        public DiscPunishmentImpactController(
            IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService,
            IEmployeeSPService employeeSPService, IPRSalaryConfigurationService prSalaryConfigurationService,
            IEmployeeService employeeService, IEmployeeGradeListService employeeGradeListService,
            IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService,
            IEmployeeSalaryConfigurationHistoryService employeeSalaryConfigurationHistoryService,
            IEmployeeMonthlySalaryService employeeMonthlySalaryService,
            IBankNameService bankNameService,
            IPRComponentService prComponentService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployeePromotionService employeePromotionService,
            IOfficeService officeService)
        {
            this.employeeSPService = employeeSPService;
            this.prSalaryConfigurationService = prSalaryConfigurationService;
            this.employeeService = employeeService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeeGradeListService = employeeGradeListService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.employeeSalaryConfigurationHistoryService = employeeSalaryConfigurationHistoryService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.bankNameService = bankNameService;
            this.prComponentService = prComponentService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.officeService = officeService;
            this.employeePromotionService = employeePromotionService;

            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion
        public void MapDropDown(View_PunishmentDetailViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            model.DesignationList = CommonDynamicDropDown.GetAllPayrollDesignationList();
            model.EmployeeSalaryType = commonStaticDropDown.SalaryStructuredTypeList();
            model.SalaryGenerationTypeList = commonStaticDropDown.SalaryGenerationTypeList();
            model.GradeList = CommonDynamicDropDown.GetEmployeeGradeList();
            model.SalaryScaleList = commonStaticDropDown.NumberSerialDropDown(0, 15);
            model.OverTimeList = commonStaticDropDown.YesNoDropDown_bool();
            model.PFTypeList = CommonDynamicDropDown.ProvidentFundType();
            model.MonthList = commonStaticDropDown.MonthList();
            model.BankList = CommonDynamicDropDown.PayrollBankNameWithCode();
            model.PromotionTypeList = CommonDynamicDropDown.PromotionTypeList();

            var yearList = new List<SelectListItem>();
            yearList.Add(pleaseSelect);

            for (int i = 0; i < 2; i++)
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
            model.IncrementYearFromList = yearList;

            var employeeStatusList = CommonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

        }
        public DiscPunishmentImpactController(IEmployeeSPService employeeSPService)
        {
            this.employeeSPService = employeeSPService;
        }
        // GET: DiscPunishmentImpact
        public ActionResult Index(string employeeCode)
        {
            var model = new View_PunishmentDetailViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            
            MapDropDown(model);

            model.EmployeeCode = employeeCode;
            var employeeDetail = employeeService.GetMany(p => p.EmployeeCode == employeeCode).FirstOrDefault();
            model.EmployeeStatusId = employeeDetail.EmployeeStatusId;

            return View(model);
            
        }

        public JsonResult GetPunishmentDetail(string dispatchNumber, string employeeCode )
        {
            try
            {
                var punishmentDetail = new List<View_PunishmentDetailViewModel>();
                var param = new { PunishmentDispatchNumber = dispatchNumber.Trim(), EmployeeCode = employeeCode.Trim() };
                var empList = employeeSPService.GetDataWithParameter(param, "Disc.GetPunishMentByCaseNo");

                punishmentDetail = empList.Tables[0].AsEnumerable()
                    .Select(row => new View_PunishmentDetailViewModel
                    {
                        PunishmentMasterId = row.Field<int>("PunishmentMasterId"),
                        EmployeeId = row.Field<long>("EmployeeId"),
                        PunishmentId = row.Field<int>("PunishmentId"),
                        EmployeeCode = row.Field<string>("EmployeeCode"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CrimeCode = row.Field<string>("CrimeCode"),
                        CrimeName = row.Field<string>("CrimeName"),
                        ReturnAmount = row.Field<decimal>("ReturnAmount"),
                        AnnexationAmount = row.Field<decimal>("AnnexationAmount"),
                    }).ToList();

                return Json(punishmentDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex ;
            }           
        }
    }
}