using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Models;
using gHRM.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.Core.Extensions;
using System.Data.Entity.Validation;
using gHRM.Web.Helpers;
using gHRM.Web.Filters;
using System.Drawing;
using gHRM.Data.DBDetailModels;
using gHRM.Service.StoreProcedure;
using gHRM.Service.Discipline;

namespace gHRM.Web.Controllers
{
    public class DiscReportsController : BaseController
    {

        #region Variables
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IDiscPunishmentService discPunishmentService;
        private readonly IOfficeService officeService;

        public DiscReportsController(IEmployeeSPService employeeSPService, IEmployeeService employeeService, IDiscPunishmentService discPunishmentService, IOfficeService officeService)
        {
            this.employeeSPService      = employeeSPService;
            this.employeeService        = employeeService;
            this.discPunishmentService  = discPunishmentService;
            this.officeService          = officeService;
        }

        #endregion
        #region Method
   
        public ActionResult GenerateDisciplinaryReport(string ReportType,string DateFrom,string DateTo,string EmployeeCode,string PunishmentId)
        {
            try
            {//cddzwcc eddzwe  rddmywrcc  mddmywccc  yddmywcrcc
                if (ReportType == "cddzwcc")//Zone wise current case
                {
                    var param = new { Uptodate = Convert.ToDateTime(DateFrom)};
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_RptZonewiseCurrentCase");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateTo", DateFrom);

                    ReportHelper.PrintReport("Disciplinary/rpt_DiscCurrentChargeSheet.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else if (ReportType == "eddzwe")//Zone wise emblezzle //+ "-" + DateTo
                {
                    var param = new { FromDate = Convert.ToDateTime(DateFrom), ToDate = Convert.ToDateTime(DateTo) };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_ZonewiseEmbezzle");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("Disciplinary/Rpr_Disc_ZonewiseAnnexation.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else if (ReportType == "rddmywrcc")//Month and year wise running case comparison
                {
                    var param = new { SearchDate = Convert.ToDateTime(DateFrom)};
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_Disc_RunningChargeSheetMonthYearwise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateTo", DateFrom);
                    ReportHelper.PrintReport("Disciplinary/rpt_Disc_RunningChargeSheetMonthYearwise.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);

                }
                else if (ReportType == "mddmywccc")//Month and year wise closed case comparison
                {
                    var param = new { SearchDate = Convert.ToDateTime(DateFrom) };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_Disc_MonthYearwiseChargesheetClose");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateTo", DateFrom);
                    ReportHelper.PrintReport("Disciplinary/rpt_Disc_PunishmentwiseChargeSheetClose.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else if (ReportType == "yddmywcrcc")//Month and year wise closed and running case comparison 
                {
                    var param = new { SearchDate = Convert.ToDateTime(DateFrom) };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_Disc_CurrentAndCloseChargeSheetMonthYearwise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateTo", DateFrom);
                    ReportHelper.PrintReport("Disciplinary/rpt_Disc_StatisticalReportChargesheetClosedAndCurrentMonthYearWise.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else if (ReportType == "ddemppun")//Employee wise punishment
                {
                    //var Empployee = employeeService.GetAll().Where(x => x.EmployeeCode == EmployeeCode);
                    var Empployee = employeeService.GetMany(x => x.EmployeeCode == EmployeeCode);

                    long EmpId = 0;
                    if (Empployee.Count() >= 1)
                    {
                        EmpId = employeeService.GetByCode(EmployeeCode).EmployeeId;
                    }                                         
                        var param = new { EmployeeId = EmpId };
                        var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_RPT_EmployeeWisePunishment");
                        if (OverdueMls.Tables[0].Rows.Count == 0)
                        {
                           // return JavaScript("DataNotFound()");
                            return Content("<script language='javascript' type='text/javascript'>alert('No Data Found, System Could Not Generate Blank Report.'); this.window.close();</script>");

                           // return Content("<script language='javascript' type='text/javascript'>$.alert.open('No Data Found, System Could Not Generate Blank Report.');</script>");
                        }
                        var reportParam = new Dictionary<string, object>();
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        //reportParam.Add("DateTo", DateFrom);
                        ReportHelper.PrintReport("Disciplinary/rpt_Disc_EmployeeWisePunishment.rpt", OverdueMls.Tables[0], reportParam);
                        return Content(string.Empty);
                }
                else if (ReportType == "ddpunishEmp")//Punishmentwise employee
                {

                    var param = new { FromDate = Convert.ToDateTime(DateFrom), ToDate = Convert.ToDateTime(DateTo), PunishmentId = Convert.ToInt32(PunishmentId) };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_GetPunishmentwiseEmployeeList");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("Disciplinary/Rpt_Disc_PunishmentwiseEmployeeList.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    return Json(new { Result = "ERROR"});
                }
               
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ExplanationIssuingList(string CaseType,string DateFrom,string DateTo,string IssueingOffice, string IsExcel = "")
        {
            try
            {

                var param = new { 
                                    @CaseType       =   CaseType.Trim()         ,
                                    @EntryBy        =   IssueingOffice.Trim()   ,
                                    @FromDate       =   DateFrom                ,
                                    @ToDate         =   DateTo
                                };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfDiscipline");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                if (CaseType.Trim() == "Explanation")
                {
                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_ExplanationList.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_ExplanationList.rpt", OverdueMls.Tables[0], reportParam);
                    }

                }
                else
                {

                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_ChargeSheetIssueList.rpt", OverdueMls.Tables[0], reportParam);                        
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_ChargeSheetIssueList.rpt", OverdueMls.Tables[0], reportParam);
                    }
                }

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult RunningCaseList(string CaseType, string DateFrom, string DateTo, string IssueingOffice, string IsExcel = "", string orderByZone = "", string SelectedZoneOfficeID = "")
        {
            try
            {
                var param = new
                {
                    @CaseType = CaseType.Trim(),
                    @EntryBy = IssueingOffice.Trim(),
                    @FromDate = DateFrom,
                    @ToDate = DateTo
                };

                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfRunningCase");


                //if Zone THEn show Employee Wise

               var office = officeService.GetById((int) LoggedInOfficeID);
               if (office.OfficeTypeId == 2)
               {
                   var param2 = new
                   {
                       @CaseType = CaseType.Trim(),
                       @EntryBy = IssueingOffice.Trim(),
                       @FromDate = DateFrom,
                       @ToDate = DateTo,
                       @OfficeID = LoggedInOfficeID
                   };

                   OverdueMls = employeeSPService.GetDataWithParameter(param2, "disc.sp_GETDataOfRunningCaseForZone");
               
               }



                //@CaseType AS nvarchar(100), @EntryBy AS nvarchar(10), @FromDate AS Date, @ToDate AS Date
               

                if (orderByZone == "" && DateFrom.Trim() != "" && SelectedZoneOfficeID == "") // if only has date range.
                {
                    OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfRunningCaseByDateRange"); // Order By Zone
                }

                if (orderByZone == "Yes" && DateFrom.Trim() == "") // if Order By and Get All Data.
                {
                    OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfRunningCaseOrderByZone"); // Order By Zone
                }

                if (DateFrom.Trim() != "" && orderByZone == "Yes")// if Date Between AND Order BY Zone
                {
                    OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfRunningCaseOrderByZoneByDate");
                }

                if (SelectedZoneOfficeID != "" && DateFrom.Trim() == "") // if Zone Id Selected
                {

                    var param2 = new
                    {
                        @CaseType = CaseType.Trim()         ,
                        @EntryBy = IssueingOffice.Trim()    ,
                        @FromDate = DateFrom                ,
                        @ToDate = DateTo                    ,
                        @SelectedZoneOfficeID = SelectedZoneOfficeID
                    };

                    OverdueMls = employeeSPService.GetDataWithParameter(param2, "disc.sp_GETDataOfRunningCaseByZoneId");
                
                }

                if (SelectedZoneOfficeID != "" && DateFrom.Trim() != "") // if Zone Id Selected AND Date Range 
                {

                    var param2 = new
                    {
                        @CaseType = CaseType.Trim(),
                        @EntryBy = IssueingOffice.Trim(),
                        @FromDate = DateFrom,
                        @ToDate = DateTo,
                        @SelectedZoneOfficeID = SelectedZoneOfficeID
                    };

                    OverdueMls = employeeSPService.GetDataWithParameter(param2, "disc.sp_GETDataOfRunningCaseByZoneIdBetweenDate");

                }





                
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("CaseType", CaseType);

 
                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_ZoneWiseRunningExplanation.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_ZoneWiseRunningExplanation.rpt", OverdueMls.Tables[0], reportParam);
                    }

                
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult IssuingList(string CaseType, string DateFrom, string DateTo, string IssueingOffice, string IsExcel = "", string SelectedZoneOfficeID = "", string SearchPunishment = "")
        {
            try
            {
                //@CaseType AS nvarchar(100), @EntryBy AS nvarchar(10), @FromDate AS Date, @ToDate AS Date

                var param2 = new
                {
                    @CaseType = CaseType.Trim(),
                    @EntryBy = IssueingOffice.Trim(),
                    @FromDate = DateFrom,
                    @ToDate = DateTo
                };

                var OverdueMls = employeeSPService.GetDataWithParameter(param2, "disc.sp_GETDataOfIssueing");
                
                if (SelectedZoneOfficeID != "" && SearchPunishment == "1")
                {

                    var param3 = new
                    {
                        @CaseType = CaseType.Trim(),
                        @EntryBy = IssueingOffice.Trim(),
                        @FromDate = DateFrom,
                        @ToDate = DateTo,
                        @SelectedZoneOfficeID = SelectedZoneOfficeID

                    };
                    OverdueMls = employeeSPService.GetDataWithParameter(param3, "disc.sp_GETDataOfIssueingZoneIdPunishment");

                }

                else  if (SelectedZoneOfficeID == "" && SearchPunishment == "1")
                {

                    var param3 = new
                    {
                        @CaseType = CaseType.Trim(),
                        @EntryBy = IssueingOffice.Trim(),
                        @FromDate = DateFrom,
                        @ToDate = DateTo
                       
                    };
                    OverdueMls = employeeSPService.GetDataWithParameter(param3, "disc.sp_GETDataOfIssueingZoneIdPunishmentONLYPunishment");

                }
                else if (SelectedZoneOfficeID != "")
                {

                    var param = new
                    {
                        @CaseType = CaseType.Trim(),
                        @EntryBy = IssueingOffice.Trim(),
                        @FromDate = DateFrom,
                        @ToDate = DateTo,
                        @SelectedZoneOfficeID = SelectedZoneOfficeID

                    };
                    OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfIssueingZoneId");

                }


                 
                var reportParam = new Dictionary<string, object>();
                 reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
               

                if (CaseType == "Settled")
                {
                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_ZoneWiseRunningExplanation.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_ZoneWiseRunningExplanation.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    
                }
                else
                {

                    reportParam.Add("FromDate", DateFrom);
                    reportParam.Add("ToDate", DateTo);
                    reportParam.Add("CaseType", CaseType);


                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_ExplanationNotice.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_ExplanationNotice.rpt", OverdueMls.Tables[0], reportParam);
                    }

                }
                 
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult SuspendedList(string DateFrom, string DateTo, string IssueingOffice, string IsExcel = "")
        {
            try
            {
                //@CaseType AS nvarchar(100), @EntryBy AS nvarchar(10), @FromDate AS Date, @ToDate AS Date
                var param = new
                {
                    @EntryBy = IssueingOffice.Trim(),
                    @FromDate = DateFrom,
                    @ToDate = DateTo
                };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.sp_GETDataOfSuspended");

                var office = officeService.GetById((int)LoggedInOfficeID);
                if (office.OfficeTypeId == 2)
                {
                    var param2 = new
                    {
                        @EntryBy = IssueingOffice.Trim(),
                        @FromDate = DateFrom,
                        @ToDate = DateTo,
                        @OfficeID = LoggedInOfficeID
                    };
                    OverdueMls = employeeSPService.GetDataWithParameter(param2, "disc.sp_GETDataOfSuspendedForZone");
                
                }


                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

               
                    if (IsExcel == "Yes")// Print to Excel
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/RPT_SuspendedList.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/RPT_SuspendedList.rpt", OverdueMls.Tables[0], reportParam);
                    }

                

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult RunningChargeSheetExp(string CaseType, string ContinuingDate, string IssueingOffice, string IsExcel = "")
        {
            try
            {
                //@CaseType AS nvarchar(100), @EntryBy AS nvarchar(10), @FromDate AS Date, @ToDate AS Date
                var param = new
                {
                    @CaseType = CaseType.Trim(),
                    @EntryBy = IssueingOffice.Trim(),
                    @SearchDate = ContinuingDate.Trim()

                };
                //SP_Rpt_Disc_MonthYearwiseChargesheetCloseNw(@CaseType AS nvarchar(10), @EntryBy AS nvarchar(10), @SearchDate Date)
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_Disc_MonthYearwiseChargesheetClose");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", ContinuingDate.Trim());

               

                reportParam.Add("CaseType", CaseType.Trim());
                
                if (IsExcel == "Yes")// Print to Excel
                {
                    ReportHelper.ExportExcelReport("Disciplinary/rpt_Disc_PunishmentwiseChargeSheetClose.rpt", OverdueMls.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("Disciplinary/rpt_Disc_PunishmentwiseChargeSheetClose.rpt", OverdueMls.Tables[0], reportParam);
                }

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult RunningChargeSheetExpZoneWise(string CaseType, string ContinuingDate, string IssueingOffice, string IsExcel = "")
        {
            try
            {
                //@CaseType AS nvarchar(100), @EntryBy AS nvarchar(10), @FromDate AS Date, @ToDate AS Date
                var param = new
                {
                    @CaseType = CaseType.Trim(),
                    @EntryBy = IssueingOffice.Trim(),
                    @SearchDate = ContinuingDate.Trim()

                };
                //SP_Rpt_Disc_MonthYearwiseChargesheetCloseNw(@CaseType AS nvarchar(10), @EntryBy AS nvarchar(10), @SearchDate Date)
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_Rpt_Disc_ExplanationChargesheetZoneWise"); //SP_Rpt_Disc_MonthYearwiseChargesheetCloseZoneWise
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", ContinuingDate.Trim());

                reportParam.Add("CaseType", CaseType.Trim());

                if (IsExcel == "Yes")// Print to Excel
                {

                    if (CaseType.Trim() == "Explanation")
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/rpt_DiscExplanationAllZone.rpt", OverdueMls.Tables[0], reportParam);

                    }
                    else
                    {
                        ReportHelper.ExportExcelReport("rpt_DiscChargeSheetAllZone.rpt", OverdueMls.Tables[0], reportParam);
                    }

                    // WHEN Explanation THEN rpt_DiscExplanationAllZone.rpt
                    // WHen Charge Sheet THEN rpt_DiscChargeSheetAllZone.rpt
                }
                else
                {

                    if (CaseType.Trim() == "Explanation")
                    {
                        ReportHelper.PrintReport("Disciplinary/rpt_DiscExplanationAllZone.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/rpt_DiscChargeSheetAllZone.rpt", OverdueMls.Tables[0], reportParam);
                    }

                 //   ReportHelper.PrintReport("rpt_Disc_PunishmentwiseChargeSheetCloseZoneWise.rpt", OverdueMls.Tables[0], reportParam);
                }

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult PunishmentWiseEmp(int PunishmentId , string DateFrom, string DateTo, string IssueingOffice, string IsExcel = "", string ZoneOfficeId = "0")
        {
            try
            {
                var param = new {
                    @EntryBy = IssueingOffice.Trim(),
                    @FromDate = Convert.ToDateTime(DateFrom), 
                    @ToDate = Convert.ToDateTime(DateTo), 
                    @PunishmentId = Convert.ToInt32(PunishmentId),
                    @ZoneOfficeID = Convert.ToInt32(ZoneOfficeId)
                };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_GetPunishmentwiseEmployeeList");
                 
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                 if (IsExcel == "Yes")// Print to Excel     Rpt_Disc_PunishmentwiseEmployeeList.rpt
                    {
                        ReportHelper.ExportExcelReport("Disciplinary/Rpt_Disc_PunishmentwiseEmployeeListNW.rpt", OverdueMls.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Disciplinary/Rpt_Disc_PunishmentwiseEmployeeListNW.rpt", OverdueMls.Tables[0], reportParam);
                    }
 
                
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        #endregion
        #region Events
        //
        // GET: /DiscReports/
        public ActionResult Index()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            if (LoggedInOfficeID == 2664 || LoggedInOfficeID == 2673) //Establishment
            {
                items2.Add(new SelectListItem
                {
                    Text = "Please Select",
                    Value = "0"
                });
                items2.Add(new SelectListItem
                {
                    Text = "Zone wise current case",
                    Value = "cddzwcc"
                });
                items2.Add(new SelectListItem
                {
                    Text = "Zone wise emblezzle",
                    Value = "eddzwe"
                });
                //items2.Add(new SelectListItem
                //{
                //    //Text = "Month and year wise running case comparison",
                //    Text = "Monthly comparison statement of charge sheet issue, settle and running",
                //    Value = "rddmywrcc"
                //});

                //items2.Add(new SelectListItem
                //{
                //    //Text = "Month and year wise closed case comparison",
                //    Text = "Punishment wise Case Settled Through Explanation & Chargesheet",
                //    Value = "mddmywccc"
                //});

                items2.Add(new SelectListItem
                {
                    Text = "Month and year wise closed and running case comparison",
                    Value = "yddmywcrcc"
                });
                items2.Add(new SelectListItem
                {
                    Text = "Employee wise punishment",
                    Value = "ddemppun"
                });
                items2.Add(new SelectListItem
                {
                    Text = "Punishmentwise employee List",
                    Value = "ddlPunishmentWiseEmp"    //"ddpunishEmp"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Issuing List",
                    Value = "ddEIL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Settled List",
                    Value = "ddIssueing"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Running Case List",
                    Value = "ddRENL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Suspended List",
                    Value = "ddTSL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Punishment wise Case Settled",
                    Value = "ddPEC"
                });
                items2.Add(new SelectListItem
                {
                    //Text = "Month and year wise running case comparison",
                    Text = "Monthly Statement of Case Issue, Settle & Running",
                    Value = "ddRunn"
                });



            }
            else
            {
                items2.Add(new SelectListItem
                {
                    Text = "Please Select",
                    Value = "0"
                });
                items2.Add(new SelectListItem
                {
                    Text = "Employee wise punishment",
                    Value = "ddemppun"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Issuing List",
                    Value = "ddEIL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Settled List",
                    Value = "ddIssueing"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Running Case List",
                    Value = "ddRENL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Suspended List",
                    Value = "ddTSL"
                });

                items2.Add(new SelectListItem
                {
                    Text = "Punishment wise Case Settled ", //Through Explanation & Chargesheet
                    Value = "ddPEC"
                });
                items2.Add(new SelectListItem
                {
                    //Text = "Month and year wise running case comparison",
                    Text = "Monthly Statement of Case Issue, Settle & Running",
                    Value = "ddRunn"
                });


            }
            
            ViewData["ReportsType"] = items2;

            IEnumerable<SelectListItem> items = new SelectList(" ");
             
            ViewData["PunishmentList"] = items;

            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;

            var Officeinfo = officeService.GetById((int)LoggedInOfficeID);

            ViewData["Dispatch"] = Officeinfo.OfficeCode; // Officeinfo.Dispatch;
            ViewData["OfficeTypeId"] = Officeinfo.OfficeTypeId;

            ViewData["ZOOfficeList"] = items;

            return View();
        }


         



        public JsonResult GetPunishmentList()
        {
            try
            {
                var offices = discPunishmentService.GetAll().Where(w => w.IsActive == true).Select(c => new { DisplayText = c.PunishmentName, Value = c.PunishmentId });
                return Json(new { Result = "OK", Options = offices });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }      

        //
        // GET: /DiscReports/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DiscReports/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DiscReports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DiscReports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DiscReports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DiscReports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscReports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();

                if (LoggedInOfficeID == 2664 || LoggedInOfficeID == 2673) //Establishment
                {

                    //var Emp = employeeService.GetByCode(employee_code);
                    var param = new { employee_code = employee_code };
                    var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetEmployeeName");

                    List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new EmployeeViewModel
                   {
                       EmployeeId = row.Field<long>("EmployeeId"),
                       EmployeeName = row.Field<string>("EmployeeName"),
                       kMessage = row.Field<string>("KMessage")

                   }).ToList();

                } // End of Super Admin/ Establishment

                else
                {
                    //var Emp = employeeService.GetByCode(employee_code);
                    var officeTypeID = LoggedInOfficeType;
                    var LogInOfficeID = LoggedInOfficeID;
                                    //@EmployeeId BIGINT,  @OfficeType int, @OfficeID int
                    var param = new { EmployeeCode = employee_code, OfficeType = officeTypeID, OfficeID = LogInOfficeID }; 
                    var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetEmployeeByOficeType");

                    List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                  .Select(row => new EmployeeViewModel
                  {
                      EmployeeId = row.Field<long>("EmployeeId"),
                      EmployeeName = row.Field<string>("EmployeeName"),
                      kMessage = row.Field<string>("KMessage")

                  }).ToList();


                }


                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                EmployeeViewModel v = new EmployeeViewModel();
                 
                v.EmployeeName  = "";
                v.kMessage      = "";
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                List_EmployeeViewModel.Add(v);

               return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }// End of method

        #endregion
    }// End of Class   
}// End of Namespace
