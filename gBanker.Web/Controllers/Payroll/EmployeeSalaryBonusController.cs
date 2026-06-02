
#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System.Transactions;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Service.Basic;
using gHRM.Service.Payroll;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Data.CodeFirstMigration;

#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeSalaryBonusController : BaseController
    {
        #region Private Methods
        private readonly IEmployeeSalaryBonusService employeeSalaryBonusService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IPRSalaryConfigurationService prSalaryConfigurationService;
        private readonly IFestivalBonusCalendarService festivalBonusCalendarService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IBankNameService bankNameService;
        #endregion

        #region Ctor      

        public EmployeeSalaryBonusController(IEmployeeSalaryBonusService employeeSalaryBonusService
            , IEmployeeSPService employeeSpService
            , IPRComponentService prComponentService
            , IEmployeeSPService employeeSPService
            , IPRSalaryConfigurationService prSalaryConfigurationService
            , IFestivalBonusCalendarService festivalBonusCalendarService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IBankNameService bankNameService
            , IEmployeeService employeeService
            )
        {
            this.employeeSalaryBonusService = employeeSalaryBonusService;
            this.employeeSpService = employeeSpService;
            this.prComponentService = prComponentService;
            this.employeeSPService = employeeSPService;
            this.prSalaryConfigurationService = prSalaryConfigurationService;
            this.festivalBonusCalendarService = festivalBonusCalendarService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.bankNameService = bankNameService;
            this.employeeService = employeeService;
        }

        #endregion

        #region Add
        public ActionResult FestivalBonusCalendar()
        {
            var model = new FestivalBonusCalendarViewModel();
            MapDropdownForFestivalBonus(model);
            return View(model);
        }

        public JsonResult SaveFestivalBonusCalendar(FestivalBonusCalendar obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicate = festivalBonusCalendarService.GetAll()
                        .Any(p => p.IsActive == 1 && p.Year == obj.Year && p.Month == obj.Month);

                if (checkDuplicate)
                    return Json(new { result = 0, message = "This Bonus Calendar already exists" }, JsonRequestBehavior.AllowGet);

                var model = new FestivalBonusCalendar();
                model.ComponentId = obj.ComponentId;
                model.Year = obj.Year;
                model.Month = obj.Month;
                model.IsActive = 1;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;

                //let's insert into [prl.FestivalBonusCalendar]
                festivalBonusCalendarService.Create(model);

                result = 1;
                message = "Saved successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Save failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit

        public JsonResult UpdateFestivalBonusCalendar(FestivalBonusCalendar obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicate =
                    festivalBonusCalendarService.GetAll()
                        .Any(p => p.IsActive == 1 && p.Id != obj.Id && p.Year == obj.Year && p.Month == obj.Month);

                if (checkDuplicate)
                    return Json(new { result = 0, message = "This Bonus Calendar already exists" }, JsonRequestBehavior.AllowGet);

                var model = festivalBonusCalendarService.GetById(obj.Id);
                if(model==null)
                    return Json(new { result = 0, message = "Bonus Calendar not found!" }, JsonRequestBehavior.AllowGet);

                model.ComponentId = obj.ComponentId;
                model.Year = obj.Year;
                model.Month = obj.Month;
                model.IsActive = 1;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;

                //let's update [prl.FestivalBonusCalendar]
                festivalBonusCalendarService.Update(model);

                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Bonus Generate
        public ActionResult Index()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForSalaryBonus(model);
            return View(model);
        }

        public ActionResult Index3()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForSalaryBonus(model);
            return View(model);
        }


        public ActionResult Index2()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForSalaryBonus2(model);
            return View(model);
        }

        public ActionResult Index2_admin()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForSalaryBonus3(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveEmployeeSalaryBonus(EmployeeSalaryBonusViewModel obj)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                long empID = 0;
                try
                {
                    var salaryMonth = Convert.ToInt32(obj.SalaryMonth);
                    var salaryYear = Convert.ToInt32(obj.SalaryYear);
                    var bonusComponent = obj.ComponentName.Trim();
                    var revStampDeduction = Convert.ToInt32(obj.RevStampDeduction);
                    var bonusGenerationDate = Convert.ToDateTime(DateTime.UtcNow);

                    var checkDuplicateSalaryBonus = employeeSalaryBonusService.GetMany(p => p.SalaryMonth == salaryMonth && p.SalaryYear == salaryYear && p.IsActive == 1).ToList();

                    if (checkDuplicateSalaryBonus.Where(p => p.IsApproved == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Approved, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Where(p => p.IsSendForApproval == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Send for Approval, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Count > 0)
                    {
                        var param_ = new
                        {
                            SalaryYear = salaryYear,
                            SalaryMonth = salaryMonth
                        };
                        //let's inactive employee monthly bonus on [prl.EmployeeSalaryBonus]
                        employeeSPService.GetDataWithParameter(param_, "prl.SP_EmployeeSalaryBonusIsActiveFalse");
                    }

                    var checkComponent = new List<PRComponent>();
                    var salaryBonusList = new List<EmployeeSalaryBonus>();

                    var basicComp = "Basic Salary";
                    var basicSalaryComponents = prComponentService.GetMany(p => p.ComponentName == basicComp && p.IsActive == true).ToList();
                    var componentDetail = prComponentService.GetMany(p => p.ComponentName == bonusComponent && p.IsActive == true).ToList();


                    /*

                    // KHALID MAKE it Procedure


                    var param = new
                    {
                        FromDate = DateTime.Today,
                        ToDate = DateTime.Today
                    };
                    //let's inactive employee monthly bonus on [prl.EmployeeSalaryBonus]
                    var dats = employeeSPService.GetDataWithParameter(param, "prl.GetActivePRSalaryConfiguration");


                    var salaryconfigurations = dats.Tables[0].AsEnumerable()
                            .Select(row => new PRSalaryConfiguration
                            {
                                PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
                                OfficeID = row.Field<int?>("OfficeID"),
                                EmployeeID = row.Field<long>("EmployeeID"),
                                PRComponentID = row.Field<int>("PRComponentID"),
                                ComponentAmount = row.Field<decimal>("ComponentAmount"),
                                EffectiveStartDate = row.Field<DateTime>("EffectiveStartDate"),
                                EffectiveEndDate = row.Field<DateTime>("EffectiveEndDate"),
                                IsActive = row.Field<bool>("IsActive"),
                                ComponentCategory = row.Field<string>("ComponentCategory"),
                                TransactionType = row.Field<string>("TransactionType")
                            })
                            .ToList();


                     */



                    //get employee listings from [dbo.Employee]
                    List<EmployeeViewModel> employeeDetail = GetEmployeeListings();

                    

                    var salaryconfigurations = prSalaryConfigurationService.GetMany(p => p.IsActive == true && p.EffectiveStartDate <= DateTime.Today && p.EffectiveEndDate >= DateTime.Today).ToList();

                    

                    var distinctEmployeeinSalaryConfiguration = salaryconfigurations.GroupBy(test => test.EmployeeID).Select(grp => grp.First()).ToList();

                    foreach (var item in distinctEmployeeinSalaryConfiguration)
                    {
                        //for test purpose
                        //if (item.EmployeeID == 94)
                        //{
                        //    var test = item;
                        //}

                        
                        empID = item.EmployeeID;
                        if (empID == 49)
                        {
                            var x = "";
                        }
                        var basicSalaryComponent = new PRComponent();
                        var basicSalaryComponentId = 0;
                        double basicSalaryAmount = 0;
                        double grossSalaryAmount = 0;
                        double calculatedBonusAmount = 0;
                        var prComponentId = 0;

                        if (employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).Any())
                        {
                            var employee = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();

                            checkComponent = componentDetail.Where(p =>
                                                p.ComponentName == bonusComponent && p.EmployeeStatusId == employee.EmployeeStatusId &&
                                                p.EmployeeTypeId == employee.EmployeeTypeId && p.IsActive == true && p.OfficeLocationId == employee.OfficeLocationId).ToList();

                            if (checkComponent.Count == 1)
                            {
                                prComponentId = checkComponent[0].PRComponentID;
                                var prComponentAmountRatio = checkComponent[0].ComponentAmount;
                                var prRationBasedOn = checkComponent[0].RatioBasedOn;

                                if (prRationBasedOn == "B")
                                {
                                    basicSalaryComponent = basicSalaryComponents.Where(p => p.ComponentName == "Basic Salary" && p.EmployeeStatusId == employee.EmployeeStatusId
                                                           && p.IsActive == true && (p.EmployeeTypeId == employee.EmployeeTypeId)).FirstOrDefault();

                                    if (basicSalaryComponent != null)
                                    {
                                        basicSalaryComponentId = basicSalaryComponent.PRComponentID;
                                        var basicSalaryEmployee = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.PRComponentID == basicSalaryComponentId && p.IsActive == true).FirstOrDefault();
                                        if (basicSalaryEmployee != null)
                                        {
                                            basicSalaryAmount = Convert.ToDouble(basicSalaryEmployee.ComponentAmount);
                                            calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(basicSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                        }
                                    }
                                }

                                if (prRationBasedOn == "G")
                                {
                                    var grossSalaryDetail = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();//.GrossSalary;
                                    if (grossSalaryDetail != null)
                                    {
                                        var grossSalary = grossSalaryDetail.GrossSalary;
                                        grossSalaryAmount = Convert.ToDouble(grossSalary);
                                        calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(grossSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                    }
                                }

                                if (grossSalaryAmount > 0 || basicSalaryAmount > 0)
                                {
                                    var salaryBonus = new EmployeeSalaryBonus();
                                    salaryBonus.EmployeeId = item.EmployeeID;

                                    salaryBonus.OfficeId = Convert.ToInt32(employee.OfficeId);
                                    salaryBonus.OfficeTypeId = Convert.ToInt32(employee.OfficeTypeId);
                                    salaryBonus.DesignationId = employee.DesignationId;
                                    salaryBonus.DepartmentId = employee.DepartmentId;
                                    salaryBonus.EmployeeStatusId = employee.EmployeeStatusId;
                                    salaryBonus.BankCode = employee.BankCode;

                                    salaryBonus.ComponentId = prComponentId;
                                    salaryBonus.BonusAmount = calculatedBonusAmount - revStampDeduction;
                                    salaryBonus.RevStampDeduction = revStampDeduction;

                                    salaryBonus.SalaryYear = salaryYear;
                                    salaryBonus.SalaryMonth = salaryMonth;
                                    salaryBonus.BonusProcessingDate = DateTime.UtcNow;
                                    salaryBonus.IsActive = 1;
                                    salaryBonus.IsSendForApproval = 0;
                                    salaryBonus.IsApproved = 0;
                                    salaryBonus.IsRejected = 0;
                                    salaryBonus.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.CreateDate = DateTime.UtcNow;
                                    salaryBonus.UpdateDate = DateTime.UtcNow;
                                    salaryBonusList.Add(salaryBonus);
                                }
                            }
                        }
                    }

                    //let's insert into [prl.EmployeeSalaryBonus] 
                    employeeSalaryBonusService.AddEmployeeMonthlySalaryBonusList(salaryBonusList);
                    tran.Complete();

                    return Json(new { result = 1, message = "Bonus Generation Successfull" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var t = empID;
                    tran.Dispose();
                    return Json(new { result = 0, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
        }

        [HttpPost]
        public JsonResult SaveEmployeeSalaryBonus2(EmployeeSalaryBonusViewModel obj)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                long empID = 0;
                try
                {
                    var salaryMonth = Convert.ToInt32(obj.SalaryMonth);
                    var salaryYear = Convert.ToInt32(obj.SalaryYear);
                    var bonusComponent = obj.ComponentName.Trim();
                    var revStampDeduction = Convert.ToInt32(obj.RevStampDeduction);
                    var bonusGenerationDate = Convert.ToDateTime(DateTime.UtcNow);

                    var checkDuplicateSalaryBonus = employeeSalaryBonusService.GetMany(p => p.SalaryMonth == salaryMonth && p.SalaryYear == salaryYear && p.IsActive == 1).ToList();

                    if (checkDuplicateSalaryBonus.Where(p => p.IsApproved == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Approved, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Where(p => p.IsSendForApproval == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Send for Approval, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Count > 0)
                    {
                        var param = new
                        {
                            SalaryYear = salaryYear,
                            SalaryMonth = salaryMonth
                        };
                        //let's inactive employee monthly bonus on [prl.EmployeeSalaryBonus]
                        employeeSPService.GetDataWithParameter(param, "prl.SP_EmployeeSalaryBonusIsActiveFalse");
                    }

                    var checkComponent = new List<PRComponent>();
                    var salaryBonusList = new List<EmployeeSalaryBonus>();

                    var basicComp = "Basic Salary";
                    var basicSalaryComponents = prComponentService.GetMany(p => p.ComponentName == basicComp && p.IsActive == true).ToList();
                    var componentDetail = prComponentService.GetMany(p => p.ComponentName == bonusComponent && p.IsActive == true).ToList();

                    //get employee listings from [dbo.Employee]
                    List<EmployeeViewModel> employeeDetail = GetEmployeeListings();

                    //var salaryconfigurations = prSalaryConfigurationService.GetMany(p => p.IsActive == true && p.EffectiveStartDate <= DateTime.Today && p.EffectiveEndDate >= DateTime.Today).ToList();

                    var salaryconfigurations = prSalaryConfigurationService.GetMany(p => p.IsActive == true ).ToList();

                    var distinctEmployeeinSalaryConfiguration = salaryconfigurations.GroupBy(test => test.EmployeeID).Select(grp => grp.First()).ToList();

                    foreach (var item in distinctEmployeeinSalaryConfiguration)
                    {
                        //for test purpose
                        //if (item.EmployeeID == 94)
                        //{
                        //    var test = item;
                        //}
                        empID = item.EmployeeID;
                        var basicSalaryComponent = new PRComponent();
                        var basicSalaryComponentId = 0;
                        double basicSalaryAmount = 0;
                        double grossSalaryAmount = 0;
                        double calculatedBonusAmount = 0;
                        var prComponentId = 0;

                        if (employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).Any())
                        {
                            var employee = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();

                            checkComponent = componentDetail.Where(p =>
                                                p.ComponentName == bonusComponent && p.EmployeeStatusId == employee.EmployeeStatusId &&
                                                p.EmployeeTypeId == employee.EmployeeTypeId && p.IsActive == true && p.OfficeLocationId == employee.OfficeLocationId).ToList();

                            if (checkComponent.Count == 1)
                            {
                                prComponentId = checkComponent[0].PRComponentID;
                                var prComponentAmountRatio = checkComponent[0].ComponentAmount;
                                var prRationBasedOn = checkComponent[0].RatioBasedOn;

                                if (prRationBasedOn == "B")
                                {
                                    basicSalaryComponent = basicSalaryComponents.Where(p => p.ComponentName == "Basic Salary" && p.EmployeeStatusId == employee.EmployeeStatusId
                                                           && p.IsActive == true && (p.EmployeeTypeId == employee.EmployeeTypeId)).FirstOrDefault();

                                    if (basicSalaryComponent != null)
                                    {
                                        basicSalaryComponentId = basicSalaryComponent.PRComponentID;
                                        var basicSalaryEmployee = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.PRComponentID == basicSalaryComponentId && p.IsActive == true).FirstOrDefault();
                                        if (basicSalaryEmployee != null)
                                        {
                                            basicSalaryAmount = Convert.ToDouble(basicSalaryEmployee.ComponentAmount);
                                            calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(basicSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                        }
                                    }
                                }

                                if (prRationBasedOn == "G")
                                {
                                    var grossSalaryDetail = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();//.GrossSalary;
                                    if (grossSalaryDetail != null)
                                    {
                                        var grossSalary = grossSalaryDetail.GrossSalary;
                                        grossSalaryAmount = Convert.ToDouble(grossSalary);
                                        calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(grossSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                    }
                                }

                                if (grossSalaryAmount > 0 || basicSalaryAmount > 0)
                                {
                                    var salaryBonus = new EmployeeSalaryBonus();
                                    salaryBonus.EmployeeId = item.EmployeeID;

                                    salaryBonus.OfficeId = Convert.ToInt32(employee.OfficeId);
                                    salaryBonus.OfficeTypeId = Convert.ToInt32(employee.OfficeTypeId);
                                    salaryBonus.DesignationId = employee.DesignationId;
                                    salaryBonus.DepartmentId = employee.DepartmentId;
                                    salaryBonus.EmployeeStatusId = employee.EmployeeStatusId;
                                    salaryBonus.BankCode = employee.BankCode;

                                    salaryBonus.ComponentId = prComponentId;
                                    salaryBonus.BonusAmount = calculatedBonusAmount - revStampDeduction;
                                    salaryBonus.RevStampDeduction = revStampDeduction;

                                    salaryBonus.SalaryYear = salaryYear;
                                    salaryBonus.SalaryMonth = salaryMonth;
                                    salaryBonus.BonusProcessingDate = DateTime.UtcNow;
                                    salaryBonus.IsActive = 1;
                                    salaryBonus.IsSendForApproval = 0;
                                    salaryBonus.IsApproved = 0;
                                    salaryBonus.IsRejected = 0;
                                    salaryBonus.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.CreateDate = DateTime.UtcNow;
                                    salaryBonus.UpdateDate = DateTime.UtcNow;
                                    salaryBonusList.Add(salaryBonus);
                                }
                            }
                        }
                    }

                    //let's insert into [prl.EmployeeSalaryBonus] 
                    employeeSalaryBonusService.AddEmployeeMonthlySalaryBonusList(salaryBonusList);
                    tran.Complete();

                    return Json(new { result = 1, message = "Bonus Generation Successfull" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var t = empID;
                    tran.Dispose();
                    return Json(new { result = 0, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
        }


        [HttpPost]
        public JsonResult SaveEmployeeSalaryBonus3(EmployeeSalaryBonusViewModel obj)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                long empID = 0;
                try
                {
                    var salaryMonth = Convert.ToInt32(obj.SalaryMonth);
                    var salaryYear = Convert.ToInt32(obj.SalaryYear);
                    var bonusComponent = obj.ComponentName.Trim();
                    var revStampDeduction = Convert.ToInt32(obj.RevStampDeduction);
                    var bonusGenerationDate = Convert.ToDateTime(DateTime.UtcNow);

                    var checkDuplicateSalaryBonus = employeeSalaryBonusService.GetMany(p => p.SalaryMonth == salaryMonth && p.SalaryYear == salaryYear && p.IsActive == 1).ToList();

                    if (checkDuplicateSalaryBonus.Where(p => p.IsApproved == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Approved, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Where(p => p.IsSendForApproval == 1).Any())
                        return Json(new { result = 0, message = "Already Bonus Send for Approval, Bonus Regeneration Denied" }, JsonRequestBehavior.AllowGet);

                    if (checkDuplicateSalaryBonus.Count > 0)
                    {
                        var param = new
                        {
                            SalaryYear = salaryYear,
                            SalaryMonth = salaryMonth
                        };
                        //let's inactive employee monthly bonus on [prl.EmployeeSalaryBonus]
                        employeeSPService.GetDataWithParameter(param, "prl.SP_EmployeeSalaryBonusIsActiveFalse");
                    }

                    var checkComponent = new List<PRComponent>();
                    var salaryBonusList = new List<EmployeeSalaryBonus>();

                    var basicComp = "Basic Salary";
                    var basicSalaryComponents = prComponentService.GetMany(p => p.ComponentName == basicComp && p.IsActive == true).ToList();
                    var componentDetail = prComponentService.GetMany(p => p.ComponentName == bonusComponent && p.IsActive == true).ToList();

                    //get employee listings from [dbo.Employee]
                    List<EmployeeViewModel> employeeDetail = GetEmployeeListings();

                    var salaryconfigurations = prSalaryConfigurationService.GetMany(p => p.IsActive == true && p.EffectiveStartDate <= DateTime.Today && p.EffectiveEndDate >= DateTime.Today).ToList();

                    var distinctEmployeeinSalaryConfiguration = salaryconfigurations.GroupBy(test => test.EmployeeID).Select(grp => grp.First()).ToList();

                    foreach (var item in distinctEmployeeinSalaryConfiguration)
                    {
                        //for test purpose
                        //if (item.EmployeeID == 94)
                        //{
                        //    var test = item;
                        //}
                        empID = item.EmployeeID;
                        var basicSalaryComponent = new PRComponent();
                        var basicSalaryComponentId = 0;
                        double basicSalaryAmount = 0;
                        double grossSalaryAmount = 0;
                        double calculatedBonusAmount = 0;
                        var prComponentId = 0;

                        if (employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).Any())
                        {
                            var employee = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();

                            checkComponent = componentDetail.Where(p =>
                                                p.ComponentName == bonusComponent && p.EmployeeStatusId == employee.EmployeeStatusId &&
                                                p.EmployeeTypeId == employee.EmployeeTypeId && p.IsActive == true && p.OfficeLocationId == employee.OfficeLocationId).ToList();

                            if (checkComponent.Count == 1)
                            {
                                prComponentId = checkComponent[0].PRComponentID;
                                var prComponentAmountRatio = checkComponent[0].ComponentAmount;
                                var prRationBasedOn = checkComponent[0].RatioBasedOn;

                                if (prRationBasedOn == "B")
                                {
                                    //basicSalaryComponent = basicSalaryComponents.Where(p => p.ComponentName == "Basic Salary" && p.EmployeeStatusId == employee.EmployeeStatusId
                                    //  && p.IsActive == true && (p.EmployeeTypeId == employee.EmployeeTypeId)).FirstOrDefault();

                         basicSalaryComponent = basicSalaryComponents
                            .Where(p => p.ComponentName == "Basic Salary"
                                        && p.EmployeeStatusId == employee.EmployeeStatusId
                                        && p.IsActive == true
                                        && p.EmployeeTypeId == employee.EmployeeTypeId)
                            .ElementAtOrDefault(2); // Get the third element (zero-based index)



                                    if (basicSalaryComponent != null)
                                    {
                                        basicSalaryComponentId = basicSalaryComponent.PRComponentID;
                                        var basicSalaryEmployee = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.PRComponentID == basicSalaryComponentId && p.IsActive == true).FirstOrDefault();
                                        if (basicSalaryEmployee != null)
                                        {
                                            basicSalaryAmount = Convert.ToDouble(basicSalaryEmployee.ComponentAmount);
                                            calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(basicSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                        }
                                    }
                                }

                                if (prRationBasedOn == "G")
                                {
                                    var grossSalaryDetail = employeeDetail.Where(p => p.EmployeeId == item.EmployeeID).FirstOrDefault();//.GrossSalary;
                                    if (grossSalaryDetail != null)
                                    {
                                        var grossSalary = grossSalaryDetail.GrossSalary;
                                        grossSalaryAmount = Convert.ToDouble(grossSalary);
                                        calculatedBonusAmount = Convert.ToDouble((Convert.ToDouble(grossSalaryAmount) * Convert.ToDouble(prComponentAmountRatio)) / 100);
                                    }
                                }

                                if (grossSalaryAmount > 0 || basicSalaryAmount > 0)
                                {
                                    var salaryBonus = new EmployeeSalaryBonus();
                                    salaryBonus.EmployeeId = item.EmployeeID;

                                    salaryBonus.OfficeId = Convert.ToInt32(employee.OfficeId);
                                    salaryBonus.OfficeTypeId = Convert.ToInt32(employee.OfficeTypeId);
                                    salaryBonus.DesignationId = employee.DesignationId;
                                    salaryBonus.DepartmentId = employee.DepartmentId;
                                    salaryBonus.EmployeeStatusId = employee.EmployeeStatusId;
                                    salaryBonus.BankCode = employee.BankCode;

                                    salaryBonus.ComponentId = prComponentId;
                                    salaryBonus.BonusAmount = calculatedBonusAmount - revStampDeduction;
                                    salaryBonus.RevStampDeduction = revStampDeduction;

                                    salaryBonus.SalaryYear = salaryYear;
                                    salaryBonus.SalaryMonth = salaryMonth;
                                    salaryBonus.BonusProcessingDate = DateTime.UtcNow;
                                    salaryBonus.IsActive = 1;
                                    salaryBonus.IsSendForApproval = 0;
                                    salaryBonus.IsApproved = 0;
                                    salaryBonus.IsRejected = 0;
                                    salaryBonus.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                    salaryBonus.CreateDate = DateTime.UtcNow;
                                    salaryBonus.UpdateDate = DateTime.UtcNow;
                                    salaryBonusList.Add(salaryBonus);
                                }
                            }
                        }
                    }

                    //let's insert into [prl.EmployeeSalaryBonus] 
                    employeeSalaryBonusService.AddEmployeeMonthlySalaryBonusList(salaryBonusList);
                    tran.Complete();

                    return Json(new { result = 1, message = "Bonus Generation Successfull" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var t = empID;
                    tran.Dispose();
                    return Json(new { result = 0, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
        }

        #endregion

        #region Bonus Approval
        public ActionResult BonusApproval()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForBonusApproval(model);
            return View(model);
        }



        #endregion


        #region Events


        //Fund Transfer Advice
        public ActionResult PayrollReports()
        {
            var entity = new PRWorkAreaViewModel();
            mapDropDownList2(entity);
            mapBankDropDown2(entity);
            return View(entity);
        }


        private void mapBankDropDown2(PRWorkAreaViewModel model)
        {
            //var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            //viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.OfficeTypeList = officeTypeList;




            //var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "All", Value = "10000", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;



        }

        private void mapDropDownList2(PRWorkAreaViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(PleaseSelect);
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            entity.MonthList = monthList;

            var branchList = new List<SelectListItem>();
            branchList.Add(PleaseSelect);
            entity.BranchList = branchList;

            var accountList = new List<SelectListItem>();
            accountList.Add(PleaseSelect);
            entity.AccountList = accountList;

            var salaryTypeList = new List<SelectListItem>();
            //salaryTypeList.Add(PleaseSelect);
            salaryTypeList.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Fitre", Value = "Bonus for Eid-ul-Fitre" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Azha", Value = "Bonus for Eid-ul-Azha" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Incentive", Value = "Incentive" });
            entity.SalaryTypeList = salaryTypeList;

            var componentNameList = new List<SelectListItem>();
            componentNameList.Add(PleaseSelect);

            entity.ComponentNameList = componentNameList;

            var componentList = new List<SelectListItem>();
            componentList.Add(PleaseSelect);
            componentList.Add(new SelectListItem() { Text = "Incentive", Value = "In" });
            componentList.Add(new SelectListItem() { Text = "Deduction", Value = "De" });
            entity.ComponentTypeList = componentList;


            var applicationList = new List<SelectListItem>();
            //applicationList.Add(PleaseSelect);
            //applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application", Value = "Application" });
            //applicationList.Add(new SelectListItem() { Text = "Fund Transfer Advice", Value = "Advice" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application & Advice", Value = "ApplicationAdvice" });
            //applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            entity.ReportTypeList = applicationList;

            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Excel Format)", Value = "2" });
            lists.Add(new SelectListItem() { Text = "Rejected Employees Salary (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Excel Format)", Value = "5" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office(Pdf Format)", Value = "6" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office (Excel Format)", Value = "7" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Zone Area", Value = "8" });

            entity.ReportList = lists;
        }



        public ActionResult BonusApprovedReports()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            var model = new EmployeeSalaryBonusViewModel();
            mapBankDropDown(model);

            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);//.OrderBy(x => x.OfficeName);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            return View(model);
        }

        public ActionResult BonusApprovedReports2()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            var model = new EmployeeSalaryBonusViewModel();
            mapBankDropDown2(model);

            //var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);//.OrderBy(x => x.OfficeName);
            //var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeId.ToString(),
            //    Text = x.OfficeName.ToString()
            //});
            //var zone_items = new List<SelectListItem>();
            //zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            //model.ZoneList = zone_items;


            var applicationList = new List<SelectListItem>();
           // applicationList.Add(PleaseSelect);
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application", Value = "Application" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Advice", Value = "Advice" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application & Advice", Value = "ApplicationAdvice" });
            // applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            model.ReportTypeList = applicationList;

            return View(model);
        }

        public ActionResult EmployeeSalaryBonusReport()
        {
            var model = new EmployeeSalaryBonusViewModel();
            MapDropdownForSalaryBonus(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetGeneratedBonus([DataSourceRequest]DataSourceRequest request, string componentName, int year, int month)
        {

            var param = new { ComponentName = componentName.Trim(), Year = year, Month = month, IsSendForApproval = 0, IsRejected = 0, IsApproved = 0 };
            var view_SalaryBonusDetails = employeeSpService.GetDataWithParameter(param, "prl.SP_GetEmployeeSalaryBonus");
            var viewList = view_SalaryBonusDetails.Tables[0].AsEnumerable().Select((p, sl) => new EmployeeSalaryBonusViewModel
            {
                rowSl = sl + 1,
                ESBonusId = p.Field<int>("ESBonusId"),
                EmployeeId = p.Field<long>("EmployeeId"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                DesignationName = p.Field<string>("DesignationName"),
                DepartmentName = p.Field<string>("DepartmentName"),
                ComponentId = p.Field<int>("ComponentId"),
                ComponentName = p.Field<string>("ComponentName"),
                BonusAmount = p.Field<double>("BonusAmount"),
                RevStampDeduction = p.Field<int>("RevStampDeduction"),
                SalaryYear = p.Field<int>("SalaryYear"),
                SalaryMonth = p.Field<string>("SalaryMonth"),
                BonusProcessingDate = p.Field<string>("BonusProcessingDate")
            }).ToList();

            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetBonusSendForApproval([DataSourceRequest]DataSourceRequest request, string componentName, int year, int month)
        {

            //var param = new { ComponentName = componentName.Trim(), Year = year, Month = month, IsSendForApproval = 1, IsRejected = 0, IsApproved = 0 };
            //var view_SalaryBonusDetails = employeeSpService.GetDataWithParameter(param, "SP_GetEmployeeSalaryBonusForApproval");

            var param = new { ComponentName = componentName.Trim(), Year = year, Month = month, IsSendForApproval = 1, IsRejected = 0, IsApproved = 0 };
            var view_SalaryBonusDetails = employeeSpService.GetDataWithParameter(param, "prl.SP_GetEmployeeSalaryBonus");

            var viewList = view_SalaryBonusDetails.Tables[0].AsEnumerable().Select((p, sl) => new EmployeeSalaryBonusViewModel
            {
                rowSl = sl + 1,
                ESBonusId = p.Field<int>("ESBonusId"),
                EmployeeId = p.Field<long>("EmployeeId"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                DesignationName = p.Field<string>("DesignationName"),
                DepartmentName = p.Field<string>("DepartmentName"),
                ComponentId = p.Field<int>("ComponentId"),
                ComponentName = p.Field<string>("ComponentName"),
                BonusAmount = p.Field<double>("BonusAmount"),
                RevStampDeduction = p.Field<int>("RevStampDeduction"),
                SalaryYear = p.Field<int>("SalaryYear"),
                SalaryMonth = p.Field<string>("SalaryMonth"),
                BonusProcessingDate = p.Field<string>("BonusProcessingDate")
            }).ToList();

            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BonusReportBeforeSendForApprovalPdf(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report Before Send For Approval";
                var param = new { ComponentName = ComponentName, Year = Year, Month = Month, IsSendForApproval = 0, IsApproved = 0, IsRejected = 0, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult BonusReportBeforeSendForApprovalPdf2(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                //if (!String.IsNullOrEmpty(officeId))
                //{
                //    int _officeId = Convert.ToInt32(officeId);
                //    sb.Append(" AND O.OfficeId=" + _officeId);
                //}
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                   // sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                    sb.Append(_OfficeTypeId);
                }
                string reportName = "Bonus Report Before Send For Approval";
                var param = new { ComponentName = ComponentName, Year = Year, Month = Month, IsSendForApproval = 0, IsApproved = 0, IsRejected = 0, ReportName = reportName, AndCondition = OfficeTypeId.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");
                var reportParam = new Dictionary<string, object>();
                if(SessionHelper.CompanyInfo.CompanyShortName == "GT")
                   ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport_GT.rpt", Data.Tables[0], reportParam);
                else
                   ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusReportBeforeSendForApprovalExel(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report Before Send Approval";
                var param = new { ComponentName = ComponentName, Year = Year, Month = Month, IsSendForApproval = 0, IsApproved = 0, IsRejected = 0, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusReportBeforeSendForApprovalExel2(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                //if (!String.IsNullOrEmpty(officeId))
                //{
                //    int _officeId = Convert.ToInt32(officeId);
                //    sb.Append(" AND O.OfficeId=" + _officeId);
                //}
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report Before Send Approval";
                var param = new { ComponentName = ComponentName, Year = Year, Month = Month, IsSendForApproval = 0, IsApproved = 0, IsRejected = 0, ReportName = reportName, AndCondition = OfficeTypeId.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");

                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport_GT.rpt", Data.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusReportAfterApprovalPdf(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report After Approval";
                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 1, IsApproved = 1, IsRejected = 0, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusReportAfterApprovalExel(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report After Approval";

                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 1, IsApproved = 1, IsRejected = 0, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");


                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult BonusReportAfterApprovalPdf2(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                //if (!String.IsNullOrEmpty(officeId))
                //{
                //    int _officeId = Convert.ToInt32(officeId);
                //    sb.Append(" AND O.OfficeId=" + _officeId);
                //}
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report After Approval";
                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 1, IsApproved = 1, IsRejected = 0, ReportName = reportName, AndCondition = OfficeTypeId.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");

                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport_GT.rpt", Data.Tables[0], reportParam);
                else
                    ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusReportAfterApprovalExel2(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Bonus Report After Approval";

                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 1, IsApproved = 1, IsRejected = 0, ReportName = reportName, AndCondition = OfficeTypeId.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");


                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport_GT.rpt", Data.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusRejectedReportBeforeSendForApprovalPdf(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Rejected Bonus Report Before Send For Approval";
                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 0, IsApproved = 0, IsRejected = 1, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BonusRejectedReportAfterSendForApprovalPdf(string ComponentName, int Year, string Month, string officeId, string OfficeTypeId)
        {
            var result = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(officeId))
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND O.OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND O.OfficeTypeId=" + _OfficeTypeId);
                }
                string reportName = "Rejected Bonus Report After Send For Approval";

                var param = new { ComponentName = ComponentName.Trim(), Year = Year, Month = Month, IsSendForApproval = 1, IsApproved = 0, IsRejected = 1, ReportName = reportName, AndCondition = sb.ToString() };
                var Data = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_GetEmployeeSalaryBonus");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_EmployeeSalaryBonusReport.rpt", Data.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintBonusBankSummaryReportBeforeApproval(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult PrintBonusBankSummaryReportBeforeApproval2(int Year, int Month, int OfficeTypeId, string BankName, int? BranchId, int? AccountId, string SalaryType, string PersonToContactFromBankId, string ReportName)
        {
            try
            {

                var reportParama = new Dictionary<string, object>();


                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                //string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam_application = new Dictionary<string, object>();
                reportParam_application.Add("EmployeeName", employeeName);
                reportParam_application.Add("Email", email);
                reportParam_application.Add("MobileNumber", mobileNumber);
                reportParam_application.Add("CompanyName", companyName);
                //reportParam_application.Add("Month", Month);
                //reportParam_application.Add("Year", Year);



                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);





                var param = new { BankCode = BankName.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };

                DataSet data;

                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);

                    if (ReportName == "Advice")
                    {
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);

                        var db = new gHRMDBContext();
                        var ac = db.BankAccount.Where(z => z.AccountId == AccountId).Select(k => k.AccountNo).FirstOrDefault();

                        reportParam.Add("accountId", ac.ToString());

                        ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank_GT.rpt", data.Tables[0], reportParam);

                    }
                    else if (ReportName == "Application")
                    {
                        var param_app = new { BankCode = BankName.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus" };
                        data = employeeSpService.GetDataWithParameter(param_app, "prl.SP_Report_Salary_Bonus_FundTransferApplication");

                        reportParam = new Dictionary<string, object>();
                        ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam_application);

                        //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank_GT.rpt", data.Tables[0], reportParam_application);
                    }
                    else if (ReportName == "ApplicationAdvice")
                    {
                        var reportParam22 = new Dictionary<string, object>();
                        reportParam22.Add("EmployeeName", employeeName);
                        reportParam22.Add("Email", email);
                        reportParam22.Add("MobileNumber", mobileNumber);
                        reportParam22.Add("CompanyName", companyName);
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");
                        ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam22);
                    }



                }
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GSSB")
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);

                }
                else
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                }



                //var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                //var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                //var reportParam = new Dictionary<string, object>();
                //ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintBonusBankSummaryReportBeforeApprovalPDF(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PrintBonusBankSummaryReportAfterApproval(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepAfterApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrintBonusBankSummaryReportBeforeApprovalPDF2(int Year, int Month, int OfficeTypeId, string BankName, int? BranchId, int? AccountId,  string SalaryType, string PersonToContactFromBankId, string ReportName)
        {
            try
            {

                var reportParama = new Dictionary<string, object>();


                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                //string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam_application = new Dictionary<string, object>();
                reportParam_application.Add("EmployeeName", employeeName);
                reportParam_application.Add("Email", email);
                reportParam_application.Add("MobileNumber", mobileNumber);
                reportParam_application.Add("CompanyName", companyName);
                //reportParam_application.Add("Month", Month);
                //reportParam_application.Add("Year", Year);



                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);





                var param = new { BankCode = BankName.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };

                DataSet data;

                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);

                    if(ReportName == "Advice")
                    {
                         data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);

                        var db = new gHRMDBContext();
                        var ac = db.BankAccount.Where(z => z.AccountId == AccountId).Select(k => k.AccountNo).FirstOrDefault();

                        reportParam.Add("accountId", ac.ToString());

                        ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank_GT.rpt", data.Tables[0], reportParam);

                    }
                    else if(ReportName == "Application")
                    {
                        var param_app = new { BankCode = BankName.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus" };
                        data = employeeSpService.GetDataWithParameter(param_app, "prl.SP_Report_Salary_Bonus_FundTransferApplication");

                        reportParam = new Dictionary<string, object>();
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam_application);

                        //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank_GT.rpt", data.Tables[0], reportParam_application);
                    }
                    else if (ReportName == "ApplicationAdvice")
                    {
                        var reportParam22 = new Dictionary<string, object>();
                        reportParam22.Add("EmployeeName", employeeName);
                        reportParam22.Add("Email", email);
                        reportParam22.Add("MobileNumber", mobileNumber);
                        reportParam22.Add("CompanyName", companyName);
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam22);
                    }



                }
                else if(SessionHelper.CompanyInfo.CompanyShortName == "GSSB")
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);

                }
                else
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                }



                //var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                //var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                //var reportParam = new Dictionary<string, object>();
                //ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PrintBonusBankSummaryReportAfterApproval2(int Year, int Month, int OfficeTypeId, string BankName, int? BranchId, int? AccountId, string SalaryType, string PersonToContactFromBankId, string ReportName)
        {
            try
            {

                var reportParama = new Dictionary<string, object>();


                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                //string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam_application = new Dictionary<string, object>();
                reportParam_application.Add("EmployeeName", employeeName);
                reportParam_application.Add("Email", email);
                reportParam_application.Add("MobileNumber", mobileNumber);
                reportParam_application.Add("CompanyName", companyName);
                //reportParam_application.Add("Month", Month);
                //reportParam_application.Add("Year", Year);



                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);





                var param = new { BankCode = BankName.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };

                DataSet data;

                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);

                    if (ReportName == "Advice")
                    {
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);

                        var db = new gHRMDBContext();
                        var ac = db.BankAccount.Where(z => z.AccountId == AccountId).Select(k => k.AccountNo).FirstOrDefault();

                        reportParam.Add("accountId", ac.ToString());

                        ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank_GT.rpt", data.Tables[0], reportParam);

                    }
                    else if (ReportName == "Application")
                    {
                        var param_app = new { BankCode = BankName.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus" };
                        data = employeeSpService.GetDataWithParameter(param_app, "prl.SP_Report_Salary_Bonus_FundTransferApplication");

                        reportParam = new Dictionary<string, object>();
                        ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam_application);

                        //ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank_GT.rpt", data.Tables[0], reportParam_application);
                    }
                    else if (ReportName == "ApplicationAdvice")
                    {
                        var reportParam22 = new Dictionary<string, object>();
                        reportParam22.Add("EmployeeName", employeeName);
                        reportParam22.Add("Email", email);
                        reportParam22.Add("MobileNumber", mobileNumber);
                        reportParam22.Add("CompanyName", companyName);
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");
                        ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam22);
                    }



                }
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GSSB")
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);

                }
                else
                {

                    data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                }



                //var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                //var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                //var reportParam = new Dictionary<string, object>();
                //ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult PrintBonusBankSummaryReportAfterApprovalPDF(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepAfterApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


 

        public JsonResult PrintBonusBankSummaryReportAfterApprovalPDF2(int Year, int Month, int OfficeTypeId, string BankName, int? BranchId, int? AccountId, string SalaryType, string PersonToContactFromBankId)
        {
            try
            {


                var reportParama = new Dictionary<string, object>();


                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                //string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("EmployeeName", employeeName);
                reportParam.Add("Email", email);
                reportParam.Add("MobileNumber", mobileNumber);
                reportParam.Add("CompanyName", companyName);

                var param = new { BankCode = BankName.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };
                var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);
                }
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GSSB")
                {

                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);

                }
                else
                {
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                }



                //var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                //var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                //var reportParam = new Dictionary<string, object>();
                //ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult UpdateEmployeeSalaryBonus(EmployeeSalaryBonusViewModel obj)
        {
            var result = 0;
            var message = "";
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    var model = employeeSalaryBonusService.GetById(obj.ESBonusId);

                    if (model.IsApproved == 1)
                    {
                        return Json(new { result = result, message = "Bonus is Already Approved, Update Denied" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (model.IsSendForApproval == 1)
                    {
                        return Json(new { result = result, message = "Bonus Send for Approval, Update Denied" },
                            JsonRequestBehavior.AllowGet);
                    }
                    //model.BonusAmount = obj.BonusAmount;
                    model.IsActive = 0;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeSalaryBonusService.Update(model);

                    var salaryBonus = new EmployeeSalaryBonus();
                    salaryBonus.EmployeeId = model.EmployeeId;
                    salaryBonus.ComponentId = model.ComponentId;
                    salaryBonus.BonusAmount = obj.BonusAmount;
                    salaryBonus.SalaryYear = model.SalaryYear;
                    salaryBonus.SalaryMonth = model.SalaryMonth;
                    salaryBonus.RevStampDeduction = obj.RevStampDeduction;
                    salaryBonus.BankCode = model.BankCode;
                    salaryBonus.EmployeeStatusId = model.EmployeeStatusId;
                    salaryBonus.BonusProcessingDate = DateTime.UtcNow;
                    salaryBonus.IsActive = 1;
                    salaryBonus.IsSendForApproval = 0;
                    salaryBonus.IsApproved = 0;
                    salaryBonus.IsRejected = 0;
                    salaryBonus.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    salaryBonus.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    salaryBonus.CreateDate = DateTime.UtcNow;
                    salaryBonus.UpdateDate = DateTime.UtcNow;
                    employeeSalaryBonusService.Create(salaryBonus);
                    //salaryBonusList.Add(salaryBonus);
                    result = 1;
                    message = "Updated successfully";
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    result = 0;
                    message = "Update failed";
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateEmployeeSalaryBonusAfterSendForApproval(EmployeeSalaryBonusViewModel obj)
        {
            var result = 0;
            var message = "";
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    var model = employeeSalaryBonusService.GetById(obj.ESBonusId);

                    if (model.IsApproved == 1)
                    {
                        return Json(new { result = result, message = "Bonus is Already Approved, Update Denied" },
                            JsonRequestBehavior.AllowGet);
                    }

                    //model.BonusAmount = obj.BonusAmount;
                    model.IsActive = 0;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeSalaryBonusService.Update(model);

                    var salaryBonus = new EmployeeSalaryBonus();
                    salaryBonus.EmployeeId = model.EmployeeId;
                    salaryBonus.ComponentId = model.ComponentId;
                    salaryBonus.BonusAmount = obj.BonusAmount;
                    salaryBonus.SalaryYear = model.SalaryYear;
                    salaryBonus.SalaryMonth = model.SalaryMonth;
                    salaryBonus.BonusProcessingDate = DateTime.UtcNow;
                    salaryBonus.IsActive = 1;
                    salaryBonus.IsSendForApproval = 0;
                    salaryBonus.IsApproved = 0;
                    salaryBonus.IsRejected = 0;
                    salaryBonus.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    salaryBonus.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    salaryBonus.CreateDate = DateTime.UtcNow;
                    salaryBonus.UpdateDate = DateTime.UtcNow;
                    salaryBonus.BankCode = model.BankCode;
                    employeeSalaryBonusService.Create(salaryBonus);
                    //salaryBonusList.Add(salaryBonus);
                    result = 1;
                    message = "Updated successfully";
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    result = 0;
                    message = "Update failed";
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeeSalaryBonus(int ESBonusId)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeSalaryBonusService.GetById(ESBonusId);
                if (model.IsApproved == 1)
                {
                    return Json(new { result = result, message = "Bonus is Already Approved, Update Denied" },
                        JsonRequestBehavior.AllowGet);
                }
                if (model.IsSendForApproval == 1)
                {
                    return Json(new { result = result, message = "Bonus Send for Approval, Update Denied" },
                        JsonRequestBehavior.AllowGet);
                }
                model.IsRejected = 1;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeSalaryBonusService.Update(model);
                result = 1;
                message = "Hold successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Hold failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendForApproval(EmployeeSalaryBonusViewModel obj)
        {
            var result = 0;
            var message = "";
            var bonusComponent = obj.ComponentName.Trim();
            try
            {
                int SalaryMonthNum = Convert.ToInt32(obj.SalaryMonth);
                var param = new {
                    ComponentName = bonusComponent,
                    SalaryYear = obj.SalaryYear,
                    SalaryMonth = SalaryMonthNum
                };
                DataSet ResultData = employeeSalaryBonusService.GetDataWithParameter(param, "prl.SP_SendEmployeeSalaryBonusForApproval");
                result = Convert.ToBoolean(ResultData.Tables[0].Rows[0]["IsSuccess"]) ? 1 : 0;
                message = ResultData.Tables[0].Rows[0]["Message"].ToString();
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Send for approval denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveEmployeeSalaryBonus(EmployeeSalaryBonusViewModel obj)
        {
            var result = 0;
            var message = "";
            var bonusComponent = obj.ComponentName.Trim();
            try
            {
                int SalaryMonthNum = Convert.ToInt32(obj.SalaryMonth);
                var param = new
                {
                    ComponentName = bonusComponent,
                    SalaryYear = obj.SalaryYear,
                    SalaryMonth = SalaryMonthNum,
                    LoggedInEmployeeId = LoggedInEmployeeId ?? 0
                };
                DataSet ResultData = employeeSalaryBonusService.GetDataWithParameter(param, "prl.SP_ApproveEmployeeSalaryBonus");
                result = Convert.ToBoolean(ResultData.Tables[0].Rows[0]["IsSuccess"]) ? 1 : 0;
                message = ResultData.Tables[0].Rows[0]["Message"].ToString();
            }
            catch (Exception)
            {
                result = 0;
                message = "Approval denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult HoldEmployeeSalaryBonusAfterSendForApproval(int ESBonusId)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeSalaryBonusService.GetById(ESBonusId);
                if (model.IsApproved == 1)
                {
                    return Json(new { result = result, message = "Bonus is Already Approved, Update Denied" },
                        JsonRequestBehavior.AllowGet);
                }

                model.IsRejected = 1;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeSalaryBonusService.Update(model);
                result = 1;
                message = "Hold successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Hold failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEmployeeBonusApprovalStatus(int jtStartIndex, int jtPageSize, string jtSorting, int year, int month)
        {
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFestivalBonusList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var list = festivalBonusCalendarService.GetAll().Where(p => p.IsActive == 1).ToList();
            var view_list = list.AsEnumerable().Select(p => new FestivalBonusCalendarViewModel()
            {
                Id = p.Id,
                ComponentId = p.ComponentId,
                Year = p.Year,
                MonthNo = p.Month,
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.Month)
            }).ToList();
            var currentPageRecords = view_list.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCoun = view_list.LongCount() },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFestivalBonusCalendar(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = festivalBonusCalendarService.GetById(Id);
                model.IsActive = 0;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                festivalBonusCalendarService.Update(model);
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



        // For GSSB
        public JsonResult PrintBonusBankSummaryReportBeforeApprovalForGSSBPDF(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeeBonusSleepBeforeApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_View_EmployeeBonusSleepGSSB.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region Methods

        public void MapDropdownForSalaryBonus(EmployeeSalaryBonusViewModel model)
        {
            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Excel Format)", Value = "2" });
            lists.Add(new SelectListItem() { Text = "Print Rejected Bonus Before Approval (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Excel Format)", Value = "5" });
            lists.Add(new SelectListItem() { Text = "Print Rejected Bonus After Approval (Pdf Format)", Value = "6" });
            model.ReportList = lists;

            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;


            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });            
            model.OfficeList = ofc_items;

            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });

            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });           
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });          
            model.UnitList = unit_items;

            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();

            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;

            var revStampDeductionList = new List<SelectListItem>();
            revStampDeductionList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            revStampDeductionList.Add(new SelectListItem() { Text = "10 Taka", Value = "10" });
            revStampDeductionList.Add(new SelectListItem() { Text = "20 Taka", Value = "20" });
            revStampDeductionList.Add(new SelectListItem() { Text = "30 Taka", Value = "30" });
            model.RevStampDeductionList = revStampDeductionList;

        }

        public void MapDropdownForBonusApproval(EmployeeSalaryBonusViewModel model)
        {
            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();


            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;

        }

        private void mapBankDropDown(EmployeeSalaryBonusViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            var officeType = officeTypeService.GetMany(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;
        }


        public void MapDropdownForSalaryBonus2(EmployeeSalaryBonusViewModel model)
        {
            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Excel Format)", Value = "2" });
           // lists.Add(new SelectListItem() { Text = "Print Rejected Bonus Before Approval (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Excel Format)", Value = "5" });
          //  lists.Add(new SelectListItem() { Text = "Print Rejected Bonus After Approval (Pdf Format)", Value = "6" });
            model.ReportList = lists;


            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.OfficeTypeList = officeTypeList;




            //var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;


            //var ofc_items = new List<SelectListItem>();
            //ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //model.OfficeList = ofc_items;

            //var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
            //var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeId.ToString(),
            //    Text = x.OfficeName.ToString()
            //});

            //var zone_items = new List<SelectListItem>();
            //zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            //model.ZoneList = zone_items;

            //var area_items = new List<SelectListItem>();
            //area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //model.AreaList = area_items;

            //var unit_items = new List<SelectListItem>();
            //unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //model.UnitList = unit_items;

            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();

            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;

            var revStampDeductionList = new List<SelectListItem>();
            revStampDeductionList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            revStampDeductionList.Add(new SelectListItem() { Text = "10 Taka", Value = "10" });
            revStampDeductionList.Add(new SelectListItem() { Text = "20 Taka", Value = "20" });
            revStampDeductionList.Add(new SelectListItem() { Text = "30 Taka", Value = "30" });
            model.RevStampDeductionList = revStampDeductionList;

        }

        public void MapDropdownForSalaryBonus3(EmployeeSalaryBonusViewModel model)
        {
            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Excel Format)", Value = "2" });

            lists.Add(new SelectListItem() { Text = "Print Bonus Befor Approval (Excel Format)", Value = "0" });
            // lists.Add(new SelectListItem() { Text = "Print Rejected Bonus Before Approval (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Print Approved Bonus (Excel Format)", Value = "5" });
            //  lists.Add(new SelectListItem() { Text = "Print Rejected Bonus After Approval (Pdf Format)", Value = "6" });
            model.ReportList = lists;


            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.OfficeTypeList = officeTypeList;




            //var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;


            //var ofc_items = new List<SelectListItem>();
            //ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //model.OfficeList = ofc_items;

            //var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
            //var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeId.ToString(),
            //    Text = x.OfficeName.ToString()
            //});

            //var zone_items = new List<SelectListItem>();
            //zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            //model.ZoneList = zone_items;

            //var area_items = new List<SelectListItem>();
            //area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //model.AreaList = area_items;

            //var unit_items = new List<SelectListItem>();
            //unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //model.UnitList = unit_items;

            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();

            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;

            var revStampDeductionList = new List<SelectListItem>();
            revStampDeductionList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            revStampDeductionList.Add(new SelectListItem() { Text = "10 Taka", Value = "10" });
            revStampDeductionList.Add(new SelectListItem() { Text = "20 Taka", Value = "20" });
            revStampDeductionList.Add(new SelectListItem() { Text = "30 Taka", Value = "30" });
            model.RevStampDeductionList = revStampDeductionList;

        }

        public void MapDropdownForBonusApproval2(EmployeeSalaryBonusViewModel model)
        {
            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();


            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;

        }

        private void mapBankDropDown2(EmployeeSalaryBonusViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            //var officeType = officeTypeService.GetMany(w => w.IsActive == true);
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;


            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.OfficeTypeList = officeTypeList;


            var salaryTypeList = new List<SelectListItem>();
            //salaryTypeList.Add(PleaseSelect);
            //salaryTypeList.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Fitre", Value = "Bonus for Eid-ul-Fitre" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Azha", Value = "Bonus for Eid-ul-Azha" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Incentive", Value = "Incentive" });
            model.SalaryTypeList = salaryTypeList;
        }

        // Not used
        public void MapDropdownForFestivalBonus(FestivalBonusCalendarViewModel model)
        {
            var componentList = employeeSpService.GetDataWithoutParameter("prl.SP_GetPRComponentByBonus");
            var view_ComponentList = componentList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("ComponentName"),
                Value = p.Field<string>("ComponentName")
            }).ToList();

            var comList = new List<SelectListItem>();
            comList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            comList.AddRange(view_ComponentList);
            model.ComponentList = comList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem
                {
                    Text = (DateTime.Today.Year + i).ToString(),
                    Value = (DateTime.Today.Year + i).ToString()
                });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem
                {
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }
            model.MonthList = monthList;
        }

        private List<SelectListItem> Months()
        {
            List<SelectListItem> items3 = new List<SelectListItem>();
            items3.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            items3.Add(new SelectListItem
            {
                Text = "January",
                Value = "1"
            });
            items3.Add(new SelectListItem
            {
                Text = "February",
                Value = "2"
            });
            items3.Add(new SelectListItem
            {
                Text = "March",
                Value = "3"
            });
            items3.Add(new SelectListItem
            {
                Text = "April",
                Value = "4"
            });
            items3.Add(new SelectListItem
            {
                Text = "May",
                Value = "5"
            });
            items3.Add(new SelectListItem
            {
                Text = "June",
                Value = "6"
            });
            items3.Add(new SelectListItem
            {
                Text = "July",
                Value = "7"
            });
            items3.Add(new SelectListItem
            {
                Text = "August",
                Value = "8"
            });
            items3.Add(new SelectListItem
            {
                Text = "September",
                Value = "9"
            });
            items3.Add(new SelectListItem
            {
                Text = "October",
                Value = "10"
            });
            items3.Add(new SelectListItem
            {
                Text = "November",
                Value = "11"
            });
            items3.Add(new SelectListItem
            {
                Text = "December",
                Value = "12"
            });

            return items3;
        }// End of Month

        private List<SelectListItem> Years()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            for (int year = DateTime.Now.Year; year >= (DateTime.Now.Year) - 5; year--)////////////////////////////////////////////
            {
                items2.Add(new SelectListItem
                {
                    Text = Convert.ToString(year),
                    Value = Convert.ToString(year)
                });
            }

            return items2;
        }// End of Years

        #endregion
        
        #region Private  Methods
        private List<EmployeeViewModel> GetEmployeeListings()
        {
            var param1 = new { OfficeTypeID = 0 };
            var employeeData = employeeSPService.GetDataWithParameter(param1, "prl.SP_PR_Get_AllEmployeeDataforPayroll");

            var employeeDetail = employeeData.Tables[0].AsEnumerable()
                .Select(row => new EmployeeViewModel()
                {
                    EmployeeName = row.Field<string>("EmployeeName"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId"),
                    DesignationId = row.Field<int>("DesignationId"),
                    DepartmentId = row.Field<int>("DepartmentId"),
                    EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                    BankCode = row.Field<string>("BankCode"),
                    FirstJoiningDate = row.Field<DateTime>("FirstJoiningDate"),
                    TotalEarnings = row.Field<decimal>("TotalEarnings"),
                    GrossSalary = row.Field<decimal>("GrossSalary"),
                    EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                    OfficeLocationId = row.Field<int?>("OfficeLocationId")
                }).ToList();
            return employeeDetail;
        } 
        #endregion
    }
}