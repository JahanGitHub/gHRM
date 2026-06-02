using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace gHRM.Web.Controllers
{
    public class PFReportController : BaseController
    {
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        

        public PFReportController(IEmployeeSPService employeeSPService, IOfficeTypeService officeTypeService, IOfficeService officeService)
        {
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
        }

        public ActionResult ContributionReport()
        {
            ContributionCollectionViewModel model = new ContributionCollectionViewModel();
            try
            {
                MapDropdown(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }
        public ActionResult LoanCollectionReport()
        {
            LoanCollectionViewModel model = new LoanCollectionViewModel();
            try
            {
                //  MapDropdown(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }

        public ActionResult AuditReport()
        {
            LoanCollectionViewModel model = new LoanCollectionViewModel();
            try
            {
                //  MapDropdown(model);
            }
            catch (Exception ex)
            {
            }
            return View(model);
        }


        private void MapDropdown(ContributionCollectionViewModel model)
        {
            var lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Please Select" });
            lst.AddRange(officeTypeService.GetAll().Select(x => new SelectListItem { Text = x.OfficeTypeName, Value = x.OfficeTypeId.ToString() }));
            model.OfficeTypeList = lst;
            GetAllZoneOffice(model);
            GetMonthList(model);
            GetYearList(model);
        }

        //New
        public JsonResult GetAllHeadOffice()
        {
            try
            {
                var officeList = officeService.GetAll().Where(O => O.IsActive == true && O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);

                var officeItems = new List<SelectListItem>();
                officeItems.AddRange(officeList.Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                }));
                return Json(officeItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        //New
        public JsonResult GetAllAuditZoneOffice()
        {
            try
            {
                var officeList = officeService.GetAll().Where(O => O.IsActive == true && O.OfficeTypeId == 3).OrderBy(x => x.OfficeCode);

                var officeItems = new List<SelectListItem>();
                officeItems.AddRange(officeList.Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                }));
                return Json(officeItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public void GetAllZoneOffice(ContributionCollectionViewModel model)
        {
            var ZOOfficeList = officeService.GetAll().Where(O => O.IsActive == true && O.OfficeTypeId == 2).OrderBy(x => x.OfficeCode);
            var viewZOOffice = ZOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });
            var zoOffice_items = new List<SelectListItem>();
            if (viewZOOffice.ToList().Count > 0)
            {
                zoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            }
            zoOffice_items.AddRange(viewZOOffice);
            model.ZoneOfficeList = zoOffice_items;
        }

        public JsonResult GetAreaOffice(string zoneOfficeId)
        {

            try
            {
                var areaOfficeList = GetChildOffices(Convert.ToInt32(zoneOfficeId)); //officeService.GetAll().Where(O => O.IsActive == true && O.OfficeTypeId == 4).OrderBy(x => x.OfficeCode);
                var areaOfficeItems = new List<SelectListItem>();
                areaOfficeItems.AddRange(areaOfficeList.Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                }));
                return Json(areaOfficeItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchOffice(string areaOfficeId)
        {
            try
            {
                var branchOfficeList = GetChildOffices(Convert.ToInt32(areaOfficeId)); //officeService.GetAll().Where(O => O.IsActive == true && O.OfficeTypeId == 5).OrderBy(x => x.OfficeCode);
                var branchOfficeItems = new List<SelectListItem>();
                branchOfficeItems.AddRange(branchOfficeList.Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                }));
                return Json(branchOfficeItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);

        }

        public void GetMonthList(ContributionCollectionViewModel model)
        {
            var months = Enumerable.Range(1, 12).Select(x =>
                 new SelectListItem()
                 {
                     Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],// + " (" + x + ")", //.AbbreviatedMonthNames
                     Value = x.ToString()
                     //,Selected = (x == Model.ExpirationMonth)
                 });

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Select Month", Value = "", Selected = true });
            monthList.AddRange(months);
            model.MonthList = monthList;
        }
        public void GetYearList(ContributionCollectionViewModel model)
        {
            var years = Enumerable.Range(DateTime.Today.Year - 40, 41).Select(x =>
                 new SelectListItem()
                 {
                     Text = x.ToString(),
                     Value = x.ToString()
                 });

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Select Year", Value = "", Selected = true });
            yearList.AddRange(years);
            model.YearList = yearList;
        }

        public JsonResult GetLoanListByEmployeeCode(string employeeCode)
        {
            try
            {
                //string empCode = employeeCode.PadLeft(4, '0');

                var param = new
                {
                    EmployeeCode = employeeCode
                };

                var values = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetLoanListByEmployeeCode");
                var loanList = values.Tables[0].AsEnumerable().Select(row => new LoanCollectionViewModel
                {
                    LoanId = row.Field<Int64>("LoanId").ToString(),
                    LoanStatus = row.Field<Int64>("LoanId").ToString() + " - " + (row.Field<bool>("IsInstallmentOver") == false ? "Running" : "Over")
                }).ToList();

                var loanItems = new List<SelectListItem>();
                loanItems.AddRange(loanList.Select(x => new SelectListItem
                {
                    Value = x.LoanId,
                    Text = x.LoanStatus,
                }));
                return Json(loanItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintAuditReport(string reportType = "", string fromDate = "", string toDate = "")
        {
            try
            {
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));


                if (reportType.Trim() == "1") //Contribution Statement
                {
                    var data = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionStatementSummary");
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionStatementSummary.rpt", data.Tables[0], reportParam);
                }
                //Undone: Just take report from baki and add to solution using TFS
                else if (reportType.Trim() == "2") //Loan Statement
                {
                    var data = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanStatementSummary");
                    ReportHelper.PrintReport("PF/PF_RPT_LoanStatementSummary.rpt", data.Tables[0], reportParam);
                }

                else if (reportType.Trim() == "3") //Finalize Statement
                {
                    var data = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetWithdrawnStatementSummary");
                    ReportHelper.PrintReport("PF/PF_RPT_WithdrawnStatementSummary.rpt", data.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        [HttpGet]
        public ActionResult PrintHeadOfficeContributionReport(string officeType = "", string fromDate = "", string toDate = "", string headOfficeId = "")
        {
            try
            {
                string title = string.Empty;

                int officeTypeId = 0;
                int? containingOfficeId = null;

                if (headOfficeId.Trim() == "")
                {
                    officeTypeId = 1;
                    title = "Summary of Head Office Departments";

                    var param = new
                    {
                        FromDate = Convert.ToDateTime(fromDate),
                        ToDate = Convert.ToDateTime(toDate),
                        OfficeTypeId = officeTypeId,
                        ContainingOfficeId = containingOfficeId
                    };
                    var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionCollectionSumByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("OfficeType", title);
                    reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionSumByOfficeType.rpt", loanLedger.Tables[0], reportParam);
                }
                else
                {
                    var param = new
                    {
                        OfficeId = Convert.ToInt32(headOfficeId),
                        FromDate = Convert.ToDateTime(fromDate),
                        ToDate = Convert.ToDateTime(toDate)
                    };
                    var loanLedger = employeeSPService.GetDataWithParameter(param, "pf.SP_RPT_GetContributionCollectionByOfficeId");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionByOfficeId.rpt", loanLedger.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
             {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PrintZoneOfficeContributionReport(string officeType = "", string fromDate = "", string toDate = "", string zoneId = "", string areaId = "", string branchId = "")
        {
            try
            {
                string title = string.Empty;

                int officeTypeId = 0;
                int? containingOfficeId = null;

                if (officeType.Trim() == "2") //Zonal Report
                {
                    if (zoneId.Trim() == "") //Zone Blank means Area and Branch also Blank
                    {
                        officeTypeId = 2;
                        title = "Summary of Zone Office";
                    }

                    if (zoneId.Trim() != "")
                    {

                        if (areaId.Trim() == "") //Area Blank means Branch also Blank
                        {
                            officeTypeId = 4;
                            containingOfficeId = Convert.ToInt32(zoneId.Trim());
                            title = "Summary of Arrea Office";
                        }
                        else
                        {
                            officeTypeId = 5;
                            containingOfficeId = Convert.ToInt32(areaId.Trim());
                            title = "Summary of Brance Office";
                        }
                    }
                }
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate),
                    OfficeTypeId = officeTypeId,
                    ContainingOfficeId = containingOfficeId
                };

                var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionCollectionSumByOfficeType");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("OfficeType", title);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));

                ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionSumByOfficeType.rpt", loanLedger.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PrintEmployeeWiseContributionByOfficeId(string officeType = "", string fromDate = "", string toDate = "", string zoneId = "", string areaId = "", string branchId = "")
        {
            try
            {

                int officeId = 0;
                if (!string.IsNullOrEmpty(branchId.Trim()) && branchId.Trim() != "undefined")
                {
                    officeId = Convert.ToInt32(branchId.Trim());
                }
                else if (!string.IsNullOrEmpty(areaId.Trim()) && areaId.Trim() != "undefined")
                {
                    officeId = Convert.ToInt32(areaId.Trim());
                }
                else if (!string.IsNullOrEmpty(zoneId.Trim()) && zoneId.Trim() != "undefined")
                {
                    officeId = Convert.ToInt32(zoneId.Trim());
                }
                var param = new
                {
                    OfficeId = officeId,
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };
                var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionCollectionByOfficeId");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));
                ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionByOfficeId.rpt", loanLedger.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PrintZoneAuditOfficeContributionReport(string reportType = "", string fromDate = "", string toDate = "", string zoneAuditId = "")
        {
            try
            {
                string officeType = string.Empty;

                int officeTypeId = 0;
                int? containingOfficeId = null;

                //if (reportType.Trim() == "3") //Zonal Report
                //{
                if (zoneAuditId.Trim() == "") //Zone Blank means Area and Branch also Blank
                {
                    officeTypeId = 3;
                    officeType = "Sum of Zone Audit Office";
                    //}
                    //}
                    var param = new
                    {
                        FromDate = Convert.ToDateTime(fromDate),
                        ToDate = Convert.ToDateTime(toDate),
                        OfficeTypeId = officeTypeId,
                        ContainingOfficeId = containingOfficeId
                    };

                    var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionCollectionSumByOfficeType");

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("OfficeType", officeType);
                    reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionSumByOfficeType.rpt", loanLedger.Tables[0], reportParam);
                }
                else
                {
                    var param = new
                    {
                        OfficeId = Convert.ToInt32(zoneAuditId),
                        FromDate = Convert.ToDateTime(fromDate),
                        ToDate = Convert.ToDateTime(toDate)
                    };
                    var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionCollectionByOfficeId");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("FromDate", Convert.ToDateTime(fromDate.Trim()).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(toDate.Trim()).ToString("dd-MMM-yyyy"));
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionCollectionByOfficeId.rpt", loanLedger.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult IndividualLoanLedger(string employeeCode = "", string loanId = "", string toDate = "")
        {
            try
            {
                //string empCode = employeeCode.PadLeft(4, '0');
                var param = new
                {
                    EmployeeCode = employeeCode,
                    LoanId = Convert.ToInt64(loanId),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanLedger");

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("CompanyName", SessionHelper.CompanyName);
                //reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                //reportParam.Add("ToDate", Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"));

                ReportHelper.PrintReport("PF/PF_RPT_LoanLedger.rpt", loanLedger.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult GetLoanCollectionSummary(string fromDate = "", string toDate = "")
        {
            try
            {
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanLedger = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanCollectionSummary");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintReport("PF/PF_RPT_LoanCollectionSummary.rpt", loanLedger.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetLoanDisbursementSummary(string fromDate = "", string toDate = "")
        {
            try
            {
                var param = new
                {
                    StartDate = Convert.ToDateTime(fromDate),
                    EndDate = Convert.ToDateTime(toDate)
                };

                var loanDisbursementSummaries = employeeSPService.GetDataWithParameter(param, "[gcpf].[LoanDisbursement_GetLoanDisbursementSummary]");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintReport("PF/PF_RPT_LoanDisbursementSummary.rpt", loanDisbursementSummaries.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetLoanWiseCollectionList(string fromDate = "", string toDate = "")
        {
            try
            {   //Delete after establish
                //SP_RPT_GetPrincipalAndInterestSumByMonth
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanWiseCollection");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"));

                ReportHelper.PrintReport("PF/PF_RPT_LoanWiseCollection.rpt", loanCollections.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetOfficeWiseLoanCollection(string fromDate = "", string toDate = "")
        {
            try
            {   //Delete after establish
                //SP_RPT_GetPrincipalAndInterestSumByMonth
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanCollectionByOfficeType");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"));

                ReportHelper.PrintReport("PF/PF_RPT_LoanCollectionByOfficeType.rpt", loanCollections.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetInterestSeparationByYear(string fromDate = "", string toDate = "")
        {
            try
            {   //Delete after establish
                //SP_RPT_GetPrincipalAndInterestSumByMonth
                var param = new
                {
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanCollectionDetails");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                reportParam.Add("FromDate", Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"));
                reportParam.Add("ToDate", Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"));
                ReportHelper.PrintReport("PF/PF_RPT_InterestSeparationByYear.rpt", loanCollections.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetLoanStatistics(string employeeCode = "", string fromDate = "", string toDate = "")
        {
            try
            {   //Delete after establish
                //SP_RPT_GetPrincipalAndInterestSumByMonth
                var param = new
                {
                    EmployeeCode = employeeCode,
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate)
                };

                var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanStatistics");

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("CompanyName", SessionHelper.CompanyName);
                //reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                //reportParam.Add("FromDate", Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"));
                //reportParam.Add("ToDate", Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy"));
                ReportHelper.PrintReport("PF/PF_RPT_LoanStatistics.rpt", loanCollections.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetLoanCollectionByVoucherNo(string voucherNo = "")
        {
            try
            {
                var param = new
                {
                    VoucherNo = Convert.ToInt64(voucherNo.Trim())
                };

                //var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetPrincipalAndInterestByVoucherNo");
                var loanCollections = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetLoanCollectionByVoucherNo");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                ReportHelper.PrintReport("PF/PF_LoanCollectionByVoucherNo.rpt", loanCollections.Tables[0], reportParam);
                //ReportHelper.PrintReport("PF_RPT_PrincipalAndInterestByVoucherNo.rpt", loanCollections.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PrintEmployeeSpecificLedger(string employeeCode, string startDate, string endDate)
        {

            try
            {
                var companyId = SessionHelper.CompanyID;
                var param = new
                {
                    EmployeeCode = employeeCode,
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    CompanyId = companyId,
                };

                var OverdueMls = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetEmployeeSpecificContribution");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("PF/PF_RPT_EmployeeSpecificContribution.rpt", OverdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PrintContributionSumByMonth(string monthId, string year)
        {

            try
            {
                var companyId = SessionHelper.CompanyID;
                var param = new
                {
                    MonthId = Convert.ToInt32(monthId),
                    Year = Convert.ToInt32(year),
                    CompanyId = companyId,
                };

                var OverdueMls = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionSumByMonth");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("PF/PF_RPT_ContributionSumByMonth.rpt", OverdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //Added on 08/02/2018
        [HttpGet]
        public ActionResult PrintPFReport(string reportType = "", string employeeCode = "", string startDate = "", string endDate = "", string monthId = "", string year = "", string voucherNo = "")
        {
            try
            {
                var companyId = SessionHelper.CompanyID;

                if (reportType.Trim() == "1")
                {
                    var param = new
                    {
                        EmployeeCode = employeeCode.Trim(),
                        StartDate = Convert.ToDateTime(startDate),
                        EndDate = Convert.ToDateTime(endDate),
                        CompanyId = companyId
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "pf.SP_RPT_GetEmployeeSpecificContribution");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("PF/PF_RPT_EmployeeSpecificContribution.rpt", OverdueMls.Tables[0], reportParam);

                    //ReportHelper.PrintReport("PF_RPT_EmployeeSpecificContribution.rpt", OverdueMls.Tables[0], reportParam);
                }
                else if (reportType.Trim() == "2")
                {
                    var param = new
                    {
                        MonthId = Convert.ToInt32(monthId.Trim()),
                        Year = Convert.ToInt32(year.Trim()),
                        CompanyId = companyId
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "pf.SP_RPT_GetContributionSumByMonth");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionSumByMonth.rpt", OverdueMls.Tables[0], reportParam);
                }

                //else if (reportType.Trim() == "3")
                //{
                //    var param = new
                //    {
                //        MonthId = Convert.ToInt32(monthId.Trim()),
                //        Year = Convert.ToInt32(year.Trim()),
                //        CompanyId = companyId
                //    };

                //    var OverdueMls = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetPrincipalAndInterestSumByMonth");
                //    var reportParam = new Dictionary<string, object>();
                //    ReportHelper.PrintReport("PF_RPT_PrincipalAndInterestSumByMonth.rpt", OverdueMls.Tables[0], reportParam);
                //}
                //else if (reportType.Trim() == "4")
                else if (reportType.Trim() == "3")
                {
                    var param = new
                    {
                        VoucherNo = Convert.ToInt64(voucherNo.Trim()),
                        CompanyId = companyId
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetContributionAndInterestByVoucherNo");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("PF/PF_RPT_ContributionAndInterestByVoucherNo.rpt", OverdueMls.Tables[0], reportParam);
                }
                //
                //else if (reportType.Trim() == "5")
                //{
                //    var param = new
                //    {
                //        VoucherNo = Convert.ToInt64(voucherNo.Trim()),
                //        CompanyId = companyId
                //    };

                //    var OverdueMls = employeeSPService.GetDataWithParameter(param, "gcpf.SP_RPT_GetPrincipalAndInterestByVoucherNo");
                //    var reportParam = new Dictionary<string, object>();
                //    ReportHelper.PrintReport("PF_RPT_PrincipalAndInterestByVoucherNo.rpt", OverdueMls.Tables[0], reportParam);
                //}

                //else if (reportType.Trim() == "6")
                else if (reportType.Trim() == "4")
                {
                    var param = new
                    {
                        EmployeeCode = employeeCode.Trim(),
                        ToDate = Convert.ToDateTime(endDate),
                        CompanyId = companyId
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "pf.SP_RPT_GetProvidentStatement");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("PF/PF_RPT_ProvidentStatement.rpt", OverdueMls.Tables[0], reportParam);
                }

                else if (reportType.Trim() == "5")
                {
                    var param = new
                    {
                        FromDate = Convert.ToDateTime(startDate),
                        ToDate = Convert.ToDateTime(endDate)
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "pf.SP_GetEmployeeWiseContributionAndInterestIncome");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("FromDate", Convert.ToDateTime(startDate).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(endDate).ToString("dd-MMM-yyyy"));

                    ReportHelper.PrintReport("PF/PF_RPT_EmployeeWiseContributionAndInterestIncome.rpt", OverdueMls.Tables[0], reportParam);
                }
                else if (reportType.Trim() == "6")
                {
                    var param = new
                    {
                        FromDate = Convert.ToDateTime(startDate),
                        ToDate = Convert.ToDateTime(endDate)
                    };

                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "pf.SP_RPT_GetContributionByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    reportParam.Add("FromDate", Convert.ToDateTime(startDate).ToString("dd-MMM-yyyy"));
                    reportParam.Add("ToDate", Convert.ToDateTime(endDate).ToString("dd-MMM-yyyy"));

                    ReportHelper.PrintReport("PF/PF_RPT_ContributionByOfficeTypeNew.rpt", OverdueMls.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public List<Office> GetChildOffices(int officeId)
        {
            var param = new
            {
                OfficeId = officeId,
            };
            var values = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetChildOffices");
            var officeList = values.Tables[0].AsEnumerable().Select(row => new Office
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")
            }).ToList();

            return officeList;
        }

    }
}
