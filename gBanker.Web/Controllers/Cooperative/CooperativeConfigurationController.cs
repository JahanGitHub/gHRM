#region Usings

using AutoMapper;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service.Cooperative;
using gHRM.Service.Payroll;
using gHRM.Service.WelfareFund;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Cooperative;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.WelfareFund
{
    public class CooperativeConfigurationController : BaseController
    {
        #region Private Variables      

        private readonly ICooperativeConfigurationService cooperativeConfigurationService;
        private readonly IComponentPayrollService componentPayrollService;
        private readonly ICooperativeLedgerService cooperativeLedgerService;
        #endregion

        #region Ctor
        public CooperativeConfigurationController(
            ICooperativeConfigurationService cooperativeConfigurationService,
            IComponentPayrollService componentPayrollService,
            ICooperativeLedgerService cooperativeLedgerService
            )
        {
            this.cooperativeConfigurationService = cooperativeConfigurationService;
            this.componentPayrollService = componentPayrollService;
            this.cooperativeLedgerService = cooperativeLedgerService;
        }

        #endregion


        #region dropdown
        private List<SelectListItem> MapDropDownList()
        {
            int componentID = 0;
            var obj = new gHRMDBContext().CooperativeConfigurations.FirstOrDefault(x => x.ActivityStatus == CoOperativeConstants.ActivityStatus_Active);
            if (obj != null)
                componentID = obj.ComponentId;
            var lst = componentPayrollService.GetMany(x => x.ComponentCategory == "Deduction" && x.Id == (componentID > 0 ? componentID : x.Id)).ToList()
                .Select(s => new SelectListItem { Text = s.ComponentName, Value = s.Id.ToString() }).OrderBy(o => o.Text).ToList();
            return lst;
        }

        #endregion

        #region Listing

        public ActionResult Index()
        {
            var model = new CooperativeConfigurationViewModel { };
            model.ComponentLst = MapDropDownList();
            return View(model);
        }

        public ActionResult InterestDeclared() 
        {
            return Content("Under developed");
        }
        public ActionResult SavingClosed() 
        {
            return Content("Under developed");
                }
        public ActionResult CooperativeReport() {
            return View();
        }
        #endregion
        #region    Report
        public ActionResult CooperativeReportView(string FromDate, string ToDate, string EmployeeCode,string report,string reportFormat)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "report", Value = report });
                if (report == "ledger" || report == "empledger") {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = FromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = ToDate });
                    report = "CooperativeLedgerReport";
                }
                else report = "CooperativeConfigurationReport";

                PrintSSRSMultiformat(reportFormat, $"/gHRMPlus_Reports/{report}", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion Report
        #region Ajax Calls
        public JsonResult GetCooperativeConfigurationListing([DataSourceRequest] DataSourceRequest request,
             string searchTerm, string FilterColumn, string FilterValue)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var lst = (from co in db.CooperativeConfigurations
                           join cp in db.ComponentPayroll on co.ComponentId equals cp.Id
                           join emp in db.Employees on co.EmployeeId equals emp.EmployeeId
                           where co.ActivityStatus == CoOperativeConstants.ActivityStatus_Active && !co.EndDate.HasValue
                           select new 
                           {
                               Id = co.Id,
                               ComponentName = cp.ComponentName,
                               ComponentId = cp.Id,
                               EmployeeId = (int)emp.EmployeeId,
                               EmployeeCode = emp.EmployeeCode,
                               EmployeeName = emp.EmployeeName,
                               MonthlyInstallment=co.MonthlyInstallment,
                               StartDate= co.StartDate,
                           }).AsEnumerable().Select(s=>new CooperativeConfigurationViewModel {
                               Id = s.Id,
                               ComponentName = s.ComponentName,
                               ComponentId = s.ComponentId,
                               EmployeeId = s.EmployeeId,
                               EmployeeCode = s.EmployeeCode,
                               EmployeeName = s.EmployeeName,
                               MonthlyInstallment = s.MonthlyInstallment,
                               StartDate = s.StartDate.ToString("dd-MMM-yyyy"),
                           }).ToList();
                DataSourceResult result = lst.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new DataSourceResult().Data, total = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddCooperativeConfiguration(CooperativeConfiguration model)
        {
            string status = "warning", message = "";
            if (model == null)
                message = "Data format is not correct";
            else if (model.EmployeeId == 0) message = "Employee is required";
            else if (model.MonthlyInstallment == 0) message = "Installment Amount is required";
            else if(DateTime.MinValue.Equals(model.StartDate) || DateTime.MaxValue.Equals(model.StartDate)) message = "Start date is not correct format";
            else
            {
                try
                {
                    if (model.Id > 0)                // Update
                    {
                        if (!cooperativeLedgerService.GetMany(x => x.SummaryMasterId == model.Id).Any())
                        {
                            model.UpdateBy = (int)SessionHelper.LoggedInEmployeeID;
                            model.UpdateDate = DateTime.Now;
                            model.ActivityStatus = CoOperativeConstants.ActivityStatus_Active;
                            cooperativeConfigurationService.Update(model);
                            message = "Update Successfully"; status = "Info";
                        }
                        else
                        {
                            status = "warning";
                            message = "Since this data is in the ledger, it is not possible to modify it.";
                        }
                    }
                    else            // Save
                    {
                        var lst = cooperativeConfigurationService.GetMany(x => x.EmployeeId == model.EmployeeId && !x.EndDate.HasValue);
                        if (lst.Any())
                        {
                            foreach(var l in lst)
                            {
                                l.UpdateBy = (int)SessionHelper.LoggedInEmployeeID;
                                l.UpdateDate = DateTime.Now;
                                l.EndDate = model.StartDate.AddDays(-1);
                                cooperativeConfigurationService.Update(l);
                            }
                        }
                        model.CreateBy = (int)SessionHelper.LoggedInEmployeeID;
                        model.CreateDate = DateTime.Now;
                        model.ActivityStatus = CoOperativeConstants.ActivityStatus_Active;
                        cooperativeConfigurationService.Create(model);
                        message = "Save Successfully"; status = "Info";
                    }
                }
                catch (Exception ex) { message = ex.Message; status = "Error"; }
            }
            return Json(new { message = message, status = status });
            
        }
        [HttpPost]
        public JsonResult InfoDelete(int id)
        {
            if (!cooperativeLedgerService.GetMany(x => x.SummaryMasterId == id).Any())
            {
                var obj = cooperativeConfigurationService.GetById(id);
                obj.ActivityStatus = CoOperativeConstants.ActivityStatus_Delete;
                obj.UpdateBy = (int)SessionHelper.LoggedInEmployeeID;
                obj.UpdateDate = DateTime.Now;
                cooperativeConfigurationService.Update(obj);
                return Json(new { message = "Delete Success", status = "Info" });
            }
            else
                return Json(new { message = "Since this data is in the ledger, it is not possible to modify it.", status = "warning" });
        }
        #endregion
    }
}
