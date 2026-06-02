
#region Usings

using gHRM.Core.Filters;
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.ViewModels.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

#endregion


namespace gHRM.Web.Controllers.Payroll
{
    public class PRSalaryAllowanceController : Controller
    {
        #region Private Variables

        private IPRComponentService prComponentService;
        private readonly IPRSalaryConfigurationService prSalaryConfigurationService;
        private IEmployeeSalaryIncentiveService empSalaryIncentiveService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeSalaryDeductionService employeeSalaryDeductionService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IProductTypeService productTypeService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeShortInfoService employeeShortInfoService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;

        private readonly IComponentPayrollService componentPayrollService;
        public PRSalaryAllowanceController(
              IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService
            , IEmployeeSalaryDeductionService employeeSalaryDeductionService, IPRComponentService prComponentService
            , IPRSalaryConfigurationService prSalaryConfigurationService
            , IEmployeeSalaryIncentiveService empSalaryIncentiveService
            , IEmployeeSPService employeeSPService
            , IEmployeeSPService employeeSpService
            , IProductGroupService productGroupService
            , IProductTypeService productTypeService
            , IProductItemService productItemService
            , IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IEmployeeShortInfoService employeeShortInfoService
            , IEmployeeService employeeService
            , ICompanyWisePayrollConfigService companyWisePayrollConfigService
            , IOfficeService officeService
            , IComponentPayrollService componentPayrollService
            )
        {
            this.employeeSPService = employeeSPService;
            this.prSalaryConfigurationService = prSalaryConfigurationService;
            this.prComponentService = prComponentService;
            this.empSalaryIncentiveService = empSalaryIncentiveService;
            this.employeeSalaryDeductionService = employeeSalaryDeductionService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeeSpService = employeeSpService;
            this.productTypeService = productTypeService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeShortInfoService = employeeShortInfoService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;
            this.componentPayrollService = componentPayrollService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            Response.AppendHeader("Cache-Control", "no-cache");
            var model = new EmployeeSalaryIncentiveViewModel();
            return View(model);
        }

        #endregion

        #region Salary Allowance

        public ActionResult SalaryAllowance()
        {
            var model = new EmployeeSalaryIncentiveViewModel();
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region Ajax Calls

        public JsonResult ApproveSalaryAllowance(int SalaryIncentiveId)
        {
            var message = "";

            try
            {
                var model = empSalaryIncentiveService.GetById(SalaryIncentiveId);
                model.IsApproved = true;
                model.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.Now;
                empSalaryIncentiveService.Update(model);
                message = "Incentive Approved Successfully";
            }
            catch (Exception e)
            {
                message = "Could not approve";
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ApproveAllIncentive()
        {
            var message = "";

            try
            {
                var model = empSalaryIncentiveService.GetAll().Where(z => z.IsActive == true && z.IsApproved == false);
                if (model.Any())
                {
                    foreach (var obj in model)
                    {
                        obj.IsApproved = true;
                        obj.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        obj.UpdateDate = DateTime.Now;
                        empSalaryIncentiveService.Update(obj);
                    }
                    message = "All Incentive Approved Successfully";
                }
                else
                {
                    message = " No Pending Incentive";
                }


            }
            catch (Exception e)
            {
                message = "Could not approve";
            }
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SalaryIncentiveList(string TranTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            Response.AppendHeader("Cache-Control", "no-cache");
            var list = employeeSPService.GetDataWithoutParameter("prl.SP_Get_NotApprovedSalaryIncentiveList");
            var IncentiveList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncentiveViewModel()
            {
                rowSl = row.Field<string>("rowSl"),
                SalaryIncentiveId = row.Field<int>("SalaryIncentiveId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                EmployeeName = row.Field<string>("EmployeeName"),
                StartDate = row.Field<string>("StartDate"),
                EndDate = row.Field<string>("EndDate"),
                ComponentName = row.Field<string>("ComponentName"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                PRComponentHour = row.Field<decimal>("PRComponentHour")

            }).ToList();
            var currentPageRecords = IncentiveList.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = IncentiveList.LongCount(), JsonRequestBehavior.AllowGet });

        }

        public JsonResult GetProuductTypebyProductGroupId(int productGroupId)
        {
            try
            {
                var productType_List = new List<SelectListItem>();
                if (productGroupId > 0)
                {
                    productType_List = commonDynamicDropDown.GetPayrollGroupWiseProductType(productGroupId).ToList();
                }
                else
                {
                    productType_List.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                }
                return Json(new { data = productType_List }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetProductListByProductType(int productGroupId, int productTypeId, int productAssignId)
        {
            try
            {
                var productList = new List<SelectListItem>();
                if (productGroupId > 0 && productTypeId > 0)
                {
                    var param = new { ProductGroupId = productGroupId, ProductTypeId = productTypeId, ProductAssignId = productAssignId };
                    var prodListByProdType = employeeSpService.GetDataWithParameter(param, "prl.SP_GetProductName_SalaryAllowanceAndDeduction");
                    var view_ProductList = prodListByProdType.Tables[0].AsEnumerable().Select(row => new SelectListItem()
                    {
                        Value = row.Field<int>("ProductId").ToString(),
                        Text = row.Field<string>("ProductItemName"),
                    }).ToList();

                    productList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    productList.AddRange(view_ProductList);
                }
                else
                {
                    productList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                }
                return Json(new { data = productList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CheckProductDependent(int componentId)
        {
            var productDependent = string.Empty;
            try
            {
                var component = prComponentService.GetAll().Where(p => p.IsActive == true && p.PRComponentID == componentId).FirstOrDefault();
                if (component != null)
                {
                    if (component.IsProductDependent == true)
                    {
                        productDependent = "Y";
                    }
                    else
                    {
                        productDependent = "N";
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = productDependent }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSerialNnumberByProductId(int productId, int employeeId)
        {
            try
            {
                var serialList = new List<SelectListItem>();
                if (productId > 0 && employeeId > 0)
                {
                    var param = new { ProductId = productId, EmployeeId = employeeId };
                    var serialNoByProdId = employeeSpService.GetDataWithParameter(param, "SP_GetSerialNumberByProductItemId");
                    var view_SerialList = serialNoByProdId.Tables[0].AsEnumerable().Select(row => new SelectListItem()
                    {
                        Value = row.Field<int>("SerialId").ToString(),
                        Text = row.Field<string>("SerialNo"),
                    }).ToList();

                    serialList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    serialList.AddRange(view_SerialList);
                }
                else
                {
                    serialList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                }
                return Json(new { data = serialList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetEmployeeBasicInfo(string EmpCode)
        {
            List<EmployeeSalaryIncentiveViewModel> List_ViewModel = new List<EmployeeSalaryIncentiveViewModel>();
            var param = new { EmpCode = EmpCode };
            var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_EmpBasicSalaryInfo");
            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeSalaryIncentiveViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   OfficeId = row.Field<int>("OfficeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                   EmployeeStatusId = row.Field<int?>("EmployeeStatusId"),
                   MaxOvertimePerMonth = row.Field<decimal>("OvertimeHour"),
                   OvertimeRate = row.Field<decimal>("OvertimeRate")
               }).ToList();
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPRComponentList(int EmployeeTypeId, int EmployeeStatusId, string ComponentCategory, int EmployeeId)
        {
            var status = EmployeeStatusId;
            var list = new List<PRComponent>();
            var componemtList = new List<SelectListItem>();
            var employeeInfo = employeeService.GetByEmpId(EmployeeId);
            if (employeeInfo != null)
            {
                var officeDetail = officeService.GetById((int)employeeInfo.OfficeId);
                if (officeDetail != null)
                {
                    if (ComponentCategory == ComponentCategoryConstants.Deduction)
                    {
                        list = prComponentService.GetAll().Where(c => c.IsActive = true
                                            && c.OfficeLocationId == officeDetail.OfficeLocationId
                                            && c.EmployeeTypeId == EmployeeTypeId
                                            && c.EmployeeStatusId == status
                                            && c.ComponentCategory == ComponentCategory).ToList();
                    }
                    else if (ComponentCategory == ComponentCategoryConstants.Allowance)
                    {
                        list = prComponentService.GetAll().Where(c => c.IsActive = true
                                                && c.OfficeLocationId == officeDetail.OfficeLocationId
                                                && c.EmployeeTypeId == EmployeeTypeId
                                                && c.EmployeeStatusId == status
                                                && (c.ComponentCategory == ComponentCategory))
                                                .ToList();
                    }

                    var compList = list.Select(row => new SelectListItem()
                    {
                        Text = row.ComponentName,
                        Value = row.PRComponentID.ToString()
                    }).ToList();

                    componemtList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    componemtList.AddRange(compList);
                }
            }

            return Json(componemtList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPRComponentList2(int EmployeeTypeId, int EmployeeStatusId, string ComponentCategory, int EmployeeId)
        {
            var status = EmployeeStatusId;
            var list = new List<PRComponent>();
            List<string> list22 = new List<string>();
            var componemtList = new List<SelectListItem>();
            var employeeInfo = employeeService.GetByEmpId(EmployeeId);
            if (employeeInfo != null)
            {
                var officeDetail = officeService.GetById((int)employeeInfo.OfficeId);
                if (officeDetail != null)
                {
                    if (ComponentCategory == ComponentCategoryConstants.Deduction)
                    {
                        list22 = prComponentService.GetAll()
                          .Where(c => c.IsActive == true && c.ComponentCategory == ComponentCategory)
                          .Select(z => z.ComponentName)
                          .Distinct()
                          .ToList();
                    }
                    else if (ComponentCategory == ComponentCategoryConstants.Allowance)
                    {
                         list22 = prComponentService.GetAll()
                             .Where(c => c.IsActive == true && c.ComponentCategory == ComponentCategory)
                             .Select(z => z.ComponentName)
                             .Distinct()
                             .ToList();
                    }

                    var compList = list22.Select(row => new SelectListItem()
                    {
                        Text = row,
                        Value = row
                    }).ToList();


                    componemtList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    componemtList.AddRange(compList);
                }
            }

            return Json(componemtList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPRComponentListFund( string ComponentCategory)
        {           
            var list = new List<ComponentPayroll>();
            var componemtList = new List<SelectListItem>();
          
            if (ComponentCategory == ComponentCategoryConstants.Deduction)
            {
            list = componentPayrollService.GetAll().Where(c => c.IsActive = true  && c.ComponentCategory == ComponentCategory && c.ComponentName.ToLower().Contains("fund") ).ToList();
            }
            else if (ComponentCategory == ComponentCategoryConstants.Allowance)
            {
            list = componentPayrollService.GetAll().Where(c => c.IsActive = true
                                              
                                    && (c.ComponentCategory == ComponentCategory))
                                    .ToList();
            }

            var compList = list.Select(row => new SelectListItem()
            {
            Text = row.ComponentName,
            Value = row.Id.ToString()
            }).ToList();

            componemtList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            componemtList.AddRange(compList);
          

            return Json(componemtList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetExistingSalaryConfigurationListbyEmployeeCode(string employeeCode, int salaryYear, int salaryMonth)
        {
            try
            {
                var salaryStartDate = new DateTime(salaryYear, salaryMonth, 1);
                var salaryEndDate = salaryStartDate.AddMonths(1).AddDays(-1);

                var param = new
                {
                    EmployeeCode = employeeCode,
                    EffectiveStartDate = salaryStartDate,//DateTime.Today,
                    EffectiveEndDate = salaryEndDate//lastDateOfMonth
                };

                var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GetSalaryConfiguredForThisMonthByEmployeeCode");

                var dataList = new List<View_EmployeeSalaryConfiguration>();

                dataList = list.Tables[0].AsEnumerable().Select(row => new View_EmployeeSalaryConfiguration()
                {
                    PRComponentId = row.Field<int>("PRComponentId"),
                    ComponentName = row.Field<string>("ComponentName"),
                    CalculatedAmount = row.Field<decimal>("CalculatedAmount"),
                    FirstJoiningDate = row.Field<DateTime>("FirstJoiningDate"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    TransactionTypeView = row.Field<string>("TransactionTypeView")
                }).ToList();

                if (dataList.Any())
                {
                    var currentDate = DateTime.Today;
                    var firstJoiningDate = dataList[0].FirstJoiningDate;
                    var month = DateTime.Today.Month;
                    var year = DateTime.Today.Year;
                    var firstDateOfMonth = new DateTime(year, month, 1);
                    decimal componentAmount = 0;

                    if (firstDateOfMonth < firstJoiningDate)
                    {
                        var daysInMonth = 0;
                        var dateDifference = 0;

                        DateTime firstOfNextMonth = new DateTime(year, month, 1).AddMonths(1);
                        var lastDate = firstOfNextMonth.AddDays(-1);
                        daysInMonth = DateTime.DaysInMonth(year, month);
                        dateDifference = Convert.ToInt32((lastDate - firstJoiningDate.AddDays(-1)).TotalDays);

                        var components = prComponentService.GetAll().ToList();

                        for (int i = 0; i < dataList.Count; i++)
                        {
                            var prComponentId = dataList[i].PRComponentId;
                            var componentName = components.Where(p => p.PRComponentID == prComponentId).FirstOrDefault().ComponentName.Trim();

                            if (componentName == "Revenue Stamp")
                            {
                                componentAmount = dataList[i].CalculatedAmount;
                                componentAmount = Math.Truncate(componentAmount * 1000m) / 1000m;
                            }
                            else
                            {
                                componentAmount = ((dataList[i].CalculatedAmount / daysInMonth) * dateDifference);
                                componentAmount = Math.Truncate(componentAmount * 1000m) / 1000m;
                            }
                            dataList[i].CalculatedAmount = componentAmount;
                        }

                    }
                }

                return Json(new { Result = "OK", dataList, Message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool CheckApprovedSalary(int salaryYear, int salaryMonth)
        {
            var checkApproved = employeeMonthlySalaryApprovedService
                                    .CheckApprovedEmployeeSalaryExist(salaryYear, salaryMonth);
            return checkApproved;
        }

        public JsonResult DeleteSalaryAllowance(int SalaryIncentiveId, int salaryYear, int salaryMonth)
        {
            var message = "";

            var model = empSalaryIncentiveService.GetById(SalaryIncentiveId);
            var isApprovedSalaryFound = CheckApprovedSalary(salaryYear, salaryMonth);
            if (isApprovedSalaryFound)
            {
                message = "Approved Salary Found, Delete Denied";
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var filter = new EmployeeMonthlySalarySearchFilter
                    {
                        IsActive = true,
                        IsSendForApproval = false,
                        IsApproved = false,
                        IsRejected = false,
                        PRComponentId = model.PRComponentId,
                        EmployeeId = (int)model.EmployeeId,
                        SalaryYear = salaryYear,
                        SalaryMonth = salaryMonth
                    };

                    var isInMonthlySalary = employeeMonthlySalaryService.CheckMonthlySalaryByFilter(filter);

                    if (isInMonthlySalary)
                    {
                        var param = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, UserAction = "Delete Incentive", PRComponentId = model.PRComponentId, EmployeeId = model.EmployeeId };
                        employeeSPService.GetDataWithParameter(param, "prl.SP_MohtlySalaryComponentDelete");
                    }

                    model.IsActive = false;
                    model.UpdateDate = DateTime.Now;
                    model.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    empSalaryIncentiveService.Update(model);

                    message = "Incentive successfully deleted";
                    scope.Complete();
                }
                catch (Exception e)
                {
                    message = e.InnerException.ToString();
                    scope.Dispose();
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSalaryDeduction(int Id, int salaryMonth, int salaryYear)
        {
            var message = "";
            var model = employeeSalaryDeductionService.GetById(Id);
            var isExistApproveSalary = CheckApprovedSalary(salaryMonth, salaryYear);
            if (isExistApproveSalary)
            {
                message = "Approved Salary Found, Delete Denied";
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var filter = new EmployeeMonthlySalarySearchFilter
                    {
                        IsActive = true,
                        IsSendForApproval = false,
                        IsApproved = false,
                        IsRejected = false,
                        PRComponentId = model.ComponentId,
                        EmployeeId = (int)model.EmployeeId,
                        SalaryYear = salaryYear,
                        SalaryMonth = salaryMonth
                    };

                    var isInMonthlySalary = employeeMonthlySalaryService.CheckMonthlySalaryByFilter(filter);

                    if (isInMonthlySalary)
                    {
                        var param = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, UserAction = "Delete Deduction", PRComponentId = model.ComponentId, EmployeeId = model.EmployeeId };
                        employeeSPService.GetDataWithParameter(param, "prl.SP_MohtlySalaryComponentDelete");
                    }

                    model.IsActive = false;
                    model.UpdateDate = DateTime.Now;
                    model.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    employeeSalaryDeductionService.Update(model);

                    message = "Component successfully deleted";
                    scope.Complete();
                }
                catch (Exception e)
                {
                    message = "Error Occured";
                    scope.Dispose();
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> CreateIncentive(int employeeId, string dateStartFrom, string dateEndTo, int prComponentId, string prComponentAmount,
          string prComponentHour, string deductionDays, string conponentCategory, int productId,
          int serialId, int isProductDependent, string remark, int salaryYear, int salaryMonth)
        {
            var response = new BaseResponse();
            string result = string.Empty;

            if(SessionHelper.CompanyInfo.CompanyShortName == "NGF")
            {
                if (!String.IsNullOrEmpty(dateEndTo))
                {
                    var param = new { prComponentId = prComponentId, EmployeeId = Convert.ToInt64(employeeId) };
                    var EndDate = employeeSpService.GetDataWithParameter(param, "prl.SP_GET_PRAllowanceAndDeductionEndDate");     //"31-May-2024";
                    if (EndDate.Tables.Count > 0)
                    {
                        string dateEndTo22 = EndDate.Tables[0].Rows[0]["dateEndTo"].ToString();
                        if (dateEndTo22 != "0")
                        {
                            dateEndTo = dateEndTo22;
                        }
                    }
                }

            }

            var sDate = Convert.ToDateTime(dateStartFrom);
            var eDate = Convert.ToDateTime(dateEndTo);

            var salaryStartDate = new DateTime(salaryYear, salaryMonth, 1);
            var salaryEndDate = salaryStartDate.AddMonths(1).AddDays(-1);

            var endDateGeneration = new DateTime(Convert.ToDateTime(dateEndTo).Year, Convert.ToDateTime(dateEndTo).Month, 1);
            DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(dateEndTo).Year, Convert.ToDateTime(dateEndTo).Month, 1).AddMonths(1);
            var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

            if (Convert.ToDateTime(dateEndTo) < endDateLastDate)
            {
                result = "End Date Must be last date of the Month";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (isProductDependent == 1 && productId <= 0)
            {
                result = "This Component Depends on Product, Please provide product before calculation, Save Denied";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (sDate < salaryStartDate)
            {
                result = "Effective Start Date can't be smaller than current salary month";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (sDate > salaryEndDate)
            {
                result = "Effective Start Date can't be bigger than current salary month";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var checkSalaryConfigured = prSalaryConfigurationService.GetPREmployeeSalaryCurrentConfigurationAllowanceAndDeduction
                (Convert.ToInt64(employeeId), Convert.ToDateTime(dateStartFrom), Convert.ToDateTime(dateEndTo));

            if (!checkSalaryConfigured.Any())
            {
                result = "No Valid Salary Configuration found for the employee, Please update employee salary configuration";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            DateTime dateStart = Convert.ToDateTime(dateStartFrom);
            var dateEnd = Convert.ToDateTime(dateEndTo);
            var startMonth = dateStart.Month;
            var endMonth = dateEnd.Month;

            var firstDayOfStartMonth = new DateTime(dateStart.Year, dateStart.Month, 1);
            var firstDayOfEndMonth = new DateTime(dateEnd.Year, dateEnd.Month, 1);
            var lastDayOfEndMonth = firstDayOfEndMonth.AddMonths(1).AddDays(-1);

            var checkDuplicateIncentive = empSalaryIncentiveService.CheckAllowanceExist(employeeId, prComponentId, conponentCategory, firstDayOfStartMonth, lastDayOfEndMonth);

            if (checkDuplicateIncentive)
            {
                result = "Already this allowance is configured for the employee for this month, Duplicate entry denied";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
            try
            {
                //if allowance then incentive. 
                if (conponentCategory.Trim() == ComponentCategoryConstants.Allowance)
                {
                    //let's insert into [EmployeeSalaryIncentive] and [prl.EmployeeMonthlySalary] 
                    response = await IncentiveCreate(employeeId, dateStartFrom, dateEndTo, prComponentId, prComponentAmount, prComponentHour, productId, serialId, checkSalaryConfigured[0].OfficeID, remark, salaryMonth, salaryYear);

                } //if insert into [prl.EmployeeSalaryDeduction] and [prl.EmployeeMonthlySalary] 
                else if (conponentCategory.Trim() == ComponentCategoryConstants.Deduction)
                {
                    response = await DeductionCreate(employeeId, dateStartFrom, dateEndTo, prComponentId, prComponentAmount, deductionDays, productId, serialId, checkSalaryConfigured[0].OfficeID, remark, salaryMonth, salaryYear);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No component category found.";                    
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "There was an error while adding. Please try again!";               
            }

            return Json(response.Message, JsonRequestBehavior.AllowGet);
        }

        public async Task<BaseResponse> IncentiveCreate(int employeeId, string dateStartFrom, string dateEndTo,
            int prComponentId, string prComponentAmount, string prComponentHour, int productId,
            int serialId, int officeId, string remark, int salaryMonth, int salaryYear)
        {
            var response = new BaseResponse();
            var result = "";
            var hour = 0;
            var isOperationSuccess = true;

            var entity = new EmployeeSalaryIncentive();
            DateTime dateStart = Convert.ToDateTime(dateStartFrom);
            var dateEnd = Convert.ToDateTime(dateEndTo);
            var startMonth = dateStart.Month;
            var endMonth = dateEnd.Month;

            var firstDayOfStartMonth = new DateTime(dateStart.Year, dateStart.Month, 1);
            var firstDayOfEndMonth = new DateTime(dateEnd.Year, dateEnd.Month, 1);
            var lastDayOfEndMonth = firstDayOfEndMonth.AddMonths(1).AddDays(-1);

            //check employee incentive for date range and component [EmployeeSalaryIncentive]
            var filterExistDeduction = new BaseSearchFilter
            {
                EmployeeId = employeeId,
                StartDate = firstDayOfStartMonth,
                EndDate = lastDayOfEndMonth
                                              ,
                PRComponentId = prComponentId,
                ProductId = productId,
                SerialId = serialId
            };
            var responseIsExistDeduction = await empSalaryIncentiveService.IsValidIncentiveByEffectiveDates(filterExistDeduction);

            if (responseIsExistDeduction.ReturnCode == BaseResonseConstants.Failed)
            {
                result = "Already this incentive is configured for the employee for this month, Duplicate entry denied";
                response.Message = result;
                return response;
            }

            entity.StartDate = Convert.ToDateTime(dateStartFrom);
            entity.EndDate = Convert.ToDateTime(dateEndTo);
            entity.EmployeeId = employeeId;
            entity.PRComponentId = prComponentId;
            entity.PRComponentAmount = Convert.ToDecimal(prComponentAmount);
            entity.Remark = remark;

            if (prComponentHour != "")
                hour = Convert.ToInt32(prComponentHour);

            entity.PRComponentHour = Convert.ToDecimal(hour);
            entity.ProductId = productId;
            entity.IsActive = true;
            entity.IsApproved = true;
            entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;
            entity.ProductId = productId;
            entity.SerialId = serialId;


            //check employee monthly salary in [dbo.EmployeeMonthlySalary]
            var checkEmployeeMonthlySalaryFound = employeeMonthlySalaryService.CheckEmployeeMonthlySalary
                                           (entity.EmployeeId, entity.StartDate, entity.EndDate);

            if (!checkEmployeeMonthlySalaryFound)
            {
                result = "Employee monthly salary not found. Please try again!";
                response.Message = result;
                return response;
            }

            //get component details by component id
            var componentDetail = prComponentService.GetById(entity.PRComponentId);
            if (componentDetail == null)
            {
                result = "No component found. Please try another!";
                response.Message = result;
                return response;
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    //let's add into [EmployeeSalaryIncentive]
                    var newEmployeeSalaryIncentive = empSalaryIncentiveService.Create(entity);

                    if (newEmployeeSalaryIncentive == null || newEmployeeSalaryIncentive.SalaryIncentiveId <= 0)
                    {
                        result = "There was an error while adding incentive. Please try again!";
                        response.Message = result;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        //check existing monthly salary in [[prl.EmployeeMonthlySalary]]
                        var isExistThisComponentForThisMonth = employeeMonthlySalaryService.CheckMonthlySalaryByComponent(salaryMonth, salaryYear, Convert.ToInt32(entity.EmployeeId), entity.PRComponentId);
                        if (!isExistThisComponentForThisMonth)
                        {
                            var salary = new EmployeeMonthlySalary();

                            //populate Employee monthly salary
                            salary = PopulateEmployeeMonthlySalary(officeId, entity, componentDetail, salaryMonth, salaryYear);

                            //let's add employee monthly salary [prl.EmployeeMonthlySalary]
                            var newEmployeeMonthlySalary = employeeMonthlySalaryService.Create(salary);

                            if (newEmployeeMonthlySalary == null)
                            {
                                result = "There was an error while generating incentive";
                                response.Message = result;
                                isOperationSuccess = false;
                            }
                        }
                    }

                    if (isOperationSuccess)
                    {
                        result = "Incentive Successfully Generated";
                        response.Message = result;
                        response.IsSuccess = true;
                    }

                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                    response.IsSuccess = false;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    ts.Complete();

                ts.Dispose();
            }

            return response;
        }

        public async Task<BaseResponse> DeductionCreate(int employeeId, string dateStartFrom, string dateEndTo,
            int prComponentId, string prComponentAmount, string deductionDays, int productId,
            int serialId, int officeId, string remark, int salaryMonth, int salaryYear)
        {
            var response = new BaseResponse();
            var result = "";
            var days = 0;
            var isOperationSuccess = true;

            var entity = new EmployeeSalaryDeduction();
            DateTime dateStart = Convert.ToDateTime(dateStartFrom);
            var dateEnd = Convert.ToDateTime(dateEndTo);
            var firstDayOfStartMonth = new DateTime(dateStart.Year, dateStart.Month, 1);
            var firstDayOfEndMonth = new DateTime(dateEnd.Year, dateEnd.Month, 1);
            var lastDayOfEndMonth = firstDayOfEndMonth.AddMonths(1).AddDays(-1);

            //check employee salary deduction in [prl.EmployeeSalaryDeduction]   
            var filterExistDeduction = new BaseSearchFilter
            {
                EmployeeId = employeeId,
                StartDate = firstDayOfStartMonth,
                EndDate = lastDayOfEndMonth
                                              ,
                PRComponentId = prComponentId,
                ProductId = productId,
                SerialId = serialId
            };

            var responseIsExistIncentive = await employeeSalaryDeductionService.IsValidDeductionByEffectiveDates(filterExistDeduction);

            if (responseIsExistIncentive.ReturnCode == BaseResonseConstants.Failed)
            {
                result = "Already this deduction is configured for the employee for this month, Duplicate entry denied";
                response.Message = result;
                return response;
            }

            entity.StartDate = Convert.ToDateTime(dateStartFrom);
            entity.EndDate = Convert.ToDateTime(dateEndTo);
            entity.EmployeeId = employeeId;
            entity.ComponentId = prComponentId;
            entity.DeductedAmount = Convert.ToDecimal(prComponentAmount);
            entity.Remark = remark;

            if (deductionDays != "")
                days = Convert.ToInt32(deductionDays);

            entity.DeductionDays = Convert.ToInt32(days);
            entity.IsActive = true;
            entity.IsApproved = true;
            entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;
            entity.ProductId = productId;
            entity.SerialId = serialId;

            //check monthly salary in [prl.EmployeeMonthlySalary]
            var checkEmployeeMonthlySalaryFound = employeeMonthlySalaryService.CheckEmployeeMonthlySalary
                                           (entity.EmployeeId, entity.StartDate, entity.EndDate);

            if (!checkEmployeeMonthlySalaryFound)
            {
                result = "Employee monthly salary not found!";
                response.Message = result;
                return response;
            }

            var componentDetail = prComponentService.GetById(entity.ComponentId);
            if (componentDetail == null)
            {
                result = "Component not found. Please try another!";
                response.Message = result;
                return response;
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    //let's add into [prl.EmployeeSalaryDeduction]
                    var newEmployeeSalaryDeduction = employeeSalaryDeductionService.Create(entity);

                    if (newEmployeeSalaryDeduction == null || newEmployeeSalaryDeduction.Id <= 0)
                    {
                        result = "There was an error while adding employee salary deduction. Please try again!";
                        response.Message = result;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        //check existing monthly salary in [[prl.EmployeeMonthlySalary]]
                        var isExistThisComponentForThisMonth = employeeMonthlySalaryService.CheckMonthlySalaryByComponent(salaryMonth, salaryYear, Convert.ToInt32(entity.EmployeeId), entity.ComponentId);
                        if (!isExistThisComponentForThisMonth)
                        {
                            //populate deduction employee monthly salary
                            var salary = PopulateDeductionEmployeeMonthlySalary(officeId, entity, componentDetail, salaryMonth, salaryYear);

                            //lets add into [prl.EmployeeMonthlySalary]
                            var newEmployeeMonthlySalary = employeeMonthlySalaryService.Create(salary);

                            if (newEmployeeMonthlySalary == null)
                            {
                                result = "There was an error while generating deduction. Please try another!";
                                response.Message = result;
                                isOperationSuccess = false;
                            }
                        }
                    }

                    if (isOperationSuccess)
                    {
                        result = "Deduction Successfully Generated";
                        response.Message = result;
                        response.IsSuccess = true;
                    }
                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                    response.IsSuccess = false;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    ts.Complete();

                ts.Dispose();
            }

            return response;
        }

        public JsonResult SalaryIncentiveListByEmployeeCode(string EmployeeCode, int salaryYear, int salaryMonth)
        {
            var incentiveList = new List<EmployeeSalaryIncentiveViewModel>();
            try
            {
                var salaryStartDate = new DateTime(salaryYear, salaryMonth, 1);
                var salaryEndDate = salaryStartDate.AddMonths(1).AddDays(-1);

                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    EffectiveStartDate = salaryStartDate,
                    EffectiveEndDate = salaryEndDate
                };

                //get listing from [prl.EmployeeSalaryIncentive]
                var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryIncentiveListByEmployeeCodes");

                incentiveList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncentiveViewModel()
                {
                    SalaryIncentiveId = row.Field<int>("SalaryIncentiveId"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    PRComponentId = row.Field<int>("PRComponentId"),
                    ComponentName = row.Field<string>("ComponentName"),
                    Remark = row.Field<string>("Remark"),
                    PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                    PRComponentHour = row.Field<decimal>("PRComponentHour"),
                    StartDateMsg = row.Field<string>("StartDate"),
                    EndDateMsg = row.Field<string>("EndDate")
                }).ToList();
            }
            catch (Exception e)
            {

            }
            return Json(new { Result = "OK", dataList = incentiveList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalaryDeductionListByEmployeeCode(string EmployeeCode, int salaryYear, int salaryMonth)
        {
            var deductionList = new List<EmployeeSalaryIncentiveViewModel>();
            try
            {
                var salaryStartDate = new DateTime(salaryYear, salaryMonth, 1);
                var salaryEndDate = salaryStartDate.AddMonths(1).AddDays(-1);

                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    EffectiveStartDate = salaryStartDate,
                    EffectiveEndDate = salaryEndDate
                };

                //get listing from [prl.EmployeeSalaryDeduction]
                var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryDeductionListByEmployeeCodes");

                deductionList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncentiveViewModel()
                {
                    SalaryIncentiveId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    PRComponentId = row.Field<int>("ComponentId"),
                    ComponentName = row.Field<string>("ComponentName"),
                    Remark = row.Field<string>("Remark"),
                    PRComponentAmount = row.Field<decimal>("DeductedAmount"),
                    PRComponentHour = row.Field<int>("DeductionDays"),
                    StartDateMsg = row.Field<string>("StartDate"),
                    EndDateMsg = row.Field<string>("EndDate")
                }).ToList();
            }
            catch (Exception e)
            {

            }
            return Json(new { Result = "OK", dataList = deductionList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveIncentive(List<EmployeeSalaryIncentive> employeeSalaryIncentives)
        {
            string result = string.Empty;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (var item in employeeSalaryIncentives)
                    {
                        var entity = new EmployeeSalaryIncentive();
                        entity.IsApproved = true;
                        entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateDate = DateTime.UtcNow;
                        empSalaryIncentiveService.Update(entity);
                    }
                    result = "Incentive Successfully Approved";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = ex.InnerException.Message.ToString();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }


        }

        #endregion

        #region Private Methods

        private EmployeeMonthlySalary PopulateDeductionEmployeeMonthlySalary(int officeId,
            EmployeeSalaryDeduction entity, PRComponent componentDetail, int salaryMonth, int salaryYear)
        {
            var salary = new EmployeeMonthlySalary();
            salary.SalaryMonth = DateTime.Now.Month;
            salary.SalaryYear = DateTime.Now.Year;
            salary.SalaryDate = DateTime.Now;
            salary.EmployeeId = entity.EmployeeId;
            salary.PRComponentId = entity.ComponentId;
            salary.PRSalaryConfigurationId = null;
            salary.PRComponentAmount = entity.DeductedAmount;
            salary.ComponentCategory = componentDetail.ComponentCategory;
            salary.TransactionType = componentDetail.TransactionType;
            salary.OfficeId = officeId;
            salary.IsActive = true;
            salary.IsSendForApproval = false;
            salary.IsApproved = false;
            salary.IsRejected = false;
            salary.CreateDate = DateTime.UtcNow;
            salary.UpdateDate = DateTime.UtcNow;
            salary.CreatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            salary.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            return salary;
        }

        private EmployeeMonthlySalary PopulateEmployeeMonthlySalary(int officeId, EmployeeSalaryIncentive entity, PRComponent componentDetail
            , int salaryMonth, int salaryYear)
        {
            var employeeMonthlySalary = new EmployeeMonthlySalary();

            employeeMonthlySalary.SalaryMonth = salaryMonth;
            employeeMonthlySalary.SalaryYear = salaryYear;
            employeeMonthlySalary.SalaryDate = DateTime.Now;
            employeeMonthlySalary.EmployeeId = entity.EmployeeId;
            employeeMonthlySalary.PRComponentId = entity.PRComponentId;
            employeeMonthlySalary.PRSalaryConfigurationId = null;
            employeeMonthlySalary.PRComponentAmount = entity.PRComponentAmount;
            employeeMonthlySalary.ComponentCategory = componentDetail.ComponentCategory;
            employeeMonthlySalary.TransactionType = componentDetail.TransactionType;
            employeeMonthlySalary.IsActive = true;
            employeeMonthlySalary.IsSendForApproval = false;
            employeeMonthlySalary.IsApproved = false;
            employeeMonthlySalary.IsRejected = false;
            employeeMonthlySalary.OfficeId = officeId;
            employeeMonthlySalary.CreateDate = DateTime.UtcNow;
            employeeMonthlySalary.UpdateDate = DateTime.UtcNow;
            employeeMonthlySalary.CreatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            employeeMonthlySalary.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

            return employeeMonthlySalary;
        }

        private void MapDropDownList(EmployeeSalaryIncentiveViewModel model)
        {
            model.ComponentList = commonStaticDropDown.ddlInitial();
            model.YearList = DateHelper.GetYears(1, 7);
            model.MonthList = DateHelper.GetMonths();

            var componentList = new List<SelectListItem>();
            componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Allowance), Value = ComponentCategoryConstants.Allowance });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
            model.ComponentCategoryList = componentList;
            model.productType_List = commonStaticDropDown.ddlInitial();
            model.productList = commonStaticDropDown.ddlInitial();
            model.serialList = commonStaticDropDown.ddlInitial();
            model.ProductGroupList = commonDynamicDropDown.GetPayrollProductGroup();
        }

        private List<ProductType> GetProductType(int productGroupId)
        {
            return productTypeService.GetAll().Where(p => p.ProductGroupId == productGroupId).ToList();
        }

        #endregion
    }
}