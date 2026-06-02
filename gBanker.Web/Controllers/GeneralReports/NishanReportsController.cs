using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Web.Helpers;
using System.Globalization;
using Kendo.Mvc.Extensions;
using gHRM.Web.Reports;
using gHRM.Web.Reports.TimeKeeping;
using gHRM.Core.Filters.TimeKeepings;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.payroll;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.OverTimes;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers.GeneralReports
{
    public class NishanReportsController : BaseController
    {

        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IOvertimeConfigurationService overtimeConfigurationService;
        private readonly IOvertimeHourEmployeeService overtimeHourEmployeeService;

        private readonly IAttAttendanceService AttAttendanceService;
        private readonly IView_TimeKeepingDetailService view_TimeKeepingDetailService;

        public NishanReportsController(
              IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
           
            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
          

        }

        #endregion

        #region events

        public ActionResult EmployeeTranferHistory()
        {
            return View( );
        }

       

        #endregion

        #region HttpRequests
         
        #endregion

        #region Methods

     

        #endregion


    }//End of Class
}//End of Namespace