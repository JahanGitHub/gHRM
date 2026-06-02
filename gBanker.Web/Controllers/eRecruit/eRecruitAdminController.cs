#region Usings

using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Core.Filters;

//using eRecruitment.Helpers;
//using eRecruitment.Infrastructure.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using gHRM.Service.eRecruit;
using gHRM.Web.ViewModels.eRecruits;
using gHRM.Web.Infrastucture.Utility;
using gHRM.Core.Utilities.eRecruitUtilities;
using gHRM.Web.Helpers;
using System.Text;


#endregion

namespace gHRM.Web.Controllers.eRecruit
{
    public class eRecruitAdminController : BaseController
    {

        #region  variables

        private readonly IEmployeeSPService eRecruitSPService;
        private static List<SelectListItem> Component_items_UNiversity;
        private static List<SelectListItem> Component_items_Masters;
        private static List<SelectListItem> Component_items_Bachelor;
        private static List<SelectListItem> Component_items_ResultType;
        private static List<SelectListItem> Component_items_District;
        private static DataSet empList;
        private static DataSet empList2;
        //private static List<ApplicationInfoViewModel> List_InvMasterViewModel_onLoad;

        public eRecruitAdminController(IEmployeeSPService eRecruitSPService)
        {
            this.eRecruitSPService = eRecruitSPService;
        }

        public List<SelectListItem> GetMastersSubjectList()
        {
            if (Component_items_Masters != null)
                return Component_items_Masters;

            List<ApplicationInfoViewModel> List_ViewModel = new List<ApplicationInfoViewModel>();
            var param = new { AndCondition = "" };
            var List = eRecruitSPService.GetDataWithParameter(param, "SP_PR_Get_MastersSubject_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ApplicationInfoViewModel
            {
                MastersSubject = row.Field<string>("MastersSubject"),
                MastersSubjectName = row.Field<string>("MastersSubject")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MastersSubject.ToString(),
                Text = string.Format("{0}", x.MastersSubjectName),
                // Text = string.Format("{0} - {1}", x.ComponentGroupName, x.PRComponentGroupID),
                //Selected = x.PRComponentGroupID == gid ? true : false
            });

            Component_items_Masters = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_Masters.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items_Masters.AddRange(Components);
            return Component_items_Masters;
        }
        public List<SelectListItem> GetUniversityNameList()
        {
            if (Component_items_UNiversity != null)
                return Component_items_UNiversity;

            List<ApplicationInfoViewModel> List_ViewModel = new List<ApplicationInfoViewModel>();
            var param = new { AndCondition = "" };
            var List = eRecruitSPService.GetDataWithParameter(param, "SP_PR_Get_UniversityName_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ApplicationInfoViewModel
            {
                UniversityName = row.Field<string>("UniversityName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.UniversityName.ToString(),
                Text = string.Format("{0}", x.UniversityName),
                // Text = string.Format("{0} - {1}", x.ComponentGroupName, x.PRComponentGroupID),
                //Selected = x.PRComponentGroupID == gid ? true : false
            });

            Component_items_UNiversity = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_UNiversity.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items_UNiversity.AddRange(Components);
            return Component_items_UNiversity;
        }
        public List<SelectListItem> GetBachelorSubjectList()
        {
            if (Component_items_Bachelor != null)
                return Component_items_Bachelor;
            List<ApplicationInfoViewModel> List_ViewModel = new List<ApplicationInfoViewModel>();
            var param = new { AndCondition = "" };
            var List = eRecruitSPService.GetDataWithParameter(param, "SP_PR_Get_HonorsSubject_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ApplicationInfoViewModel
            {
                HonorsSubject = row.Field<string>("HonorsSubject"),
                HonorsSubjectName = row.Field<string>("HonorsSubject")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.HonorsSubject.ToString(),
                Text = string.Format("{0}", x.HonorsSubjectName),
                // Text = string.Format("{0} - {1}", x.ComponentGroupName, x.PRComponentGroupID),
                //Selected = x.PRComponentGroupID == gid ? true : false
            });

            Component_items_Bachelor = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_Bachelor.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items_Bachelor.AddRange(Components);
            return Component_items_Bachelor;

        }
        public List<SelectListItem> GetResultTypeList()
        {
            if (Component_items_ResultType != null)
                return Component_items_ResultType;

            List<ApplicationInfoViewModel> List_ViewModel = new List<ApplicationInfoViewModel>();
            var param = new { AndCondition = "" };
            var List = eRecruitSPService.GetDataWithParameter(param, "SP_PR_Get_ResultTypeList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ApplicationInfoViewModel
            {
                ResultId = row.Field<int>("id"),
                ResultName = row.Field<string>("Name")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ResultId.ToString(),
                Text = string.Format("{0}", x.ResultName),
                // Text = string.Format("{0} - {1}", x.ComponentGroupName, x.PRComponentGroupID),
                //Selected = x.PRComponentGroupID == gid ? true : false
            });

            Component_items_ResultType = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_ResultType.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                //Component_items_ResultType.Add(new SelectListItem() { Text = "Both Result Type", Value = "4" });
                /*
                Component_items_ResultType.Add(new SelectListItem() { Text = "Only Masters Data Provided", Value = "5" });
                Component_items_ResultType.Add(new SelectListItem() { Text = "Division/Class in Honours or Masters", Value = "6" });
                Component_items_ResultType.Add(new SelectListItem() { Text = "Accurate Data- 1", Value = "7" });
                Component_items_ResultType.Add(new SelectListItem() { Text = "Accurate Data- 2", Value = "8" });
                Component_items_ResultType.Add(new SelectListItem() { Text = "Accurate Data- 3", Value = "9" });
                Component_items_ResultType.Add(new SelectListItem() { Text = "Accurate Data- 4", Value = "10" });
                */

            }
            Component_items_ResultType.AddRange(Components);
            return Component_items_ResultType;
        }
        public List<SelectListItem> GetHomeDistrictList()
        {
            if (Component_items_District != null)
                return Component_items_District;

            List<ApplicationInfoViewModel> List_ViewModel = new List<ApplicationInfoViewModel>();
            var param = new { AndCondition = "" };
            var List = eRecruitSPService.GetDataWithParameter(param, "SP_PR_Get_HomeDistrictList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ApplicationInfoViewModel
            {
                PerHomeDistrict = row.Field<string>("PerHomeDistrict"),
                PerHomeDistrictName = row.Field<string>("PerHomeDistrict")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PerHomeDistrict.ToString(),
                Text = string.Format("{0}", x.PerHomeDistrictName),

            });

            Component_items_District = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_District.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items_District.AddRange(Components);
            return Component_items_District;

        }
        #endregion
        // GET: eRecruit
        public ActionResult Index()
        {
            ViewData["componentBachelor"] = GetBachelorSubjectList();
            ViewData["componentMaster"] = GetMastersSubjectList();
            ViewData["componentResultType"] = GetResultTypeList();
            ViewData["componentHomeDistrict"] = GetHomeDistrictList();
            ViewData["componentUniversityName"] = GetUniversityNameList();

            List<SelectListItem> items2 = new List<SelectListItem>();

            items2.Add(new SelectListItem
            {
                Text = "All Data",
                Value = "AD"

            });

            items2.Add(new SelectListItem
            {
                Text = "Fresh Data",
                Value = "FD"
            });

            items2.Add(new SelectListItem
            {
                Text = "Inappropriate Data",
                Value = "BD"
            });

            ViewData["componentDataType"] = items2;

            return View();
        }
        public ActionResult GenerateInterViewCard()
        {
            return View();
        }

        public JsonResult GetApplicationList(string AppliedPost, string year, string txtApplicationNumber, string HonorsSubject, string MastersSubject, string ComponentHomeDistrict, string ComponentResultType, string txtFromDate, string txtToDate, string UniversityName, string txtFromResult, string txtToResult, string DataTypeSelected, string onLoad, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (AppliedPost != null)
                {
                    if (AppliedPost != "")
                        sb.Append(" AND  AppliedPostId = '" + AppliedPost.Trim() + "'");
                }

                if (txtApplicationNumber != null)
                {
                    if (txtApplicationNumber != "")
                        sb.Append(" AND ApplicationId = '" + txtApplicationNumber.Trim() + "'");
                }

                if (HonorsSubject != null)
                {
                    if (HonorsSubject != "")
                        sb.Append(" AND HonorsSubject = '" + HonorsSubject.Trim() + "'");
                }

                if (MastersSubject != null)
                {
                    if (MastersSubject != "")
                        sb.Append(" AND MastersSubject = '" + MastersSubject.Trim() + "'");
                }

                if (ComponentHomeDistrict != null)
                {
                    if (ComponentHomeDistrict != "")
                        sb.Append(" AND PerHomeDistrict = '" + ComponentHomeDistrict.Trim() + "'");
                }

                if (ComponentResultType != null)
                {
                    if (ComponentResultType != "")
                    {
                        if (ComponentResultType == "3")
                        {
                            sb.Append(" AND [Scale 4] = '" + ComponentResultType.Trim() + "'");
                        }
                        else if (ComponentResultType == "2")
                        {
                            sb.Append(" AND [Scale 5] = '" + ComponentResultType.Trim() + "'");
                        }
                        else if (ComponentResultType == "1")
                        {
                            sb.Append(" AND [Division] = '" + ComponentResultType.Trim() + "'");
                        }
                        else if (ComponentResultType == "4")
                        {
                            sb.Append(" AND [Scale 5] = '" + 2.ToString().Trim() + "'");
                            sb.Append(" OR [Scale 4] = '" + 1.ToString().Trim() + "'");
                            sb.Append(" AND Division = '" + 1.ToString().Trim() + "'");
                        }
                        else if (ComponentResultType == "5")
                        {
                            sb.Append(" AND  [Scale 4] IS NULL ");
                            sb.Append(" AND [Scale 5] IS NULL ");
                            sb.Append(" AND Division IS NULL ");
                        }
                        else if (ComponentResultType == "6")
                        {
                            sb.Append(" AND(Bachelor is NOT NULL AND ISNUMERIC(Bachelor) = 0) ");
                            sb.Append(" AND(Masters Is NOT NULL AND ISNUMERIC(Bachelor) = 0)  ");

                        }

                        else if (ComponentResultType == "7") // Accurate Data- 1
                        {
                            sb.Append(" AND((ISNUMERIC(SSC) = 1 AND SSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(SSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND((ISNUMERIC(HSC) = 1 AND HSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(HSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND(Bachelor is NOT NULL AND ISNUMERIC(Bachelor) = 1 AND CONVERT(DECIMAL(18, 2), replace(Bachelor, ',', ''))  Between 2.25 AND 4.00) ");
                            sb.Append(" AND [Scale 4] = '" + 3.ToString().Trim() + "'");
                        }

                        else if (ComponentResultType == "8") // Accurate Data- 2
                        {
                            sb.Append(" AND((ISNUMERIC(SSC) = 1 AND SSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(SSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND((ISNUMERIC(HSC) = 1 AND HSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(HSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND(Bachelor is NOT NULL AND ISNUMERIC(Bachelor) = 1 AND CONVERT(DECIMAL(18, 2), replace(Bachelor, ',', ''))  Between 2.25 AND 4.00) ");
                            sb.Append(" AND(Masters is NOT NULL AND ISNUMERIC(Masters) = 1 AND CONVERT(DECIMAL(18, 2), replace(Masters, ',', ''))  Between 2.25 AND 4.00) ");
                            sb.Append(" AND [Scale 4] = '" + 3.ToString().Trim() + "'");
                        }

                        else if (ComponentResultType == "9") // Accurate Data- 3
                        {
                            sb.Append(" AND((ISNUMERIC(SSC) = 1 AND SSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(SSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND((ISNUMERIC(HSC) = 1 AND HSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(HSC, ',', ''))  Between 2.00 AND 5.00))");
                            sb.Append(" AND(Bachelor is NOT NULL AND ISNUMERIC(Bachelor) = 1 AND CONVERT(DECIMAL(18, 2), replace(Bachelor, ',', ''))  Between 2.813 AND 5.00) ");
                            sb.Append(" AND(Masters is NOT NULL AND ISNUMERIC(Masters) = 1 AND CONVERT(DECIMAL(18, 2), replace(Masters, ',', ''))  Between 2.813 AND 5.00) ");
                            sb.Append(" AND [Scale 5] = '" + 2.ToString().Trim() + "'");
                        }

                        else if (ComponentResultType == "10") // Accurate Data- 4
                        {
                            sb.Append(" AND((ISNUMERIC(SSC) = 1 AND SSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(SSC, ',', ''))  Between 2.00 AND 5.00)) ");
                            sb.Append(" AND((ISNUMERIC(HSC) = 1 AND HSC Is Not NULL AND CONVERT(DECIMAL(18, 2), replace(HSC, ',', ''))  Between 2.00 AND 5.00)) ");
                            sb.Append(" AND(Bachelor is NOT NULL AND ISNUMERIC(Bachelor) = 0 )  ");
                            sb.Append(" AND(Masters is NOT NULL AND ISNUMERIC(Masters) = 0)  ");
                        }

                    }
                }

                if (txtFromDate != null && txtToDate != null)
                {
                    if (txtFromDate != "" && txtToDate != "")
                        sb.Append(" AND CreateDate between  '" + txtFromDate.Trim() + "' AND   '" + txtToDate.Trim() + "'");
                }

                if (txtFromResult != null && txtToResult != null)
                {
                    if (txtFromResult != "" && txtToResult != "")
                    {
                        if (ComponentResultType == "3" || ComponentResultType == "2")
                        {
                            sb.Append(" AND isNUmeric(Bachelor) = 1 ");

                            sb.Append(" AND Cast(rtrim(ltrim(Bachelor)) AS float) between  '" + Convert.ToDecimal(txtFromResult.Trim()) + "' AND   '" + Convert.ToDecimal(txtToResult.Trim()) + "'");
                        }
                        else
                            sb.Append(" AND convert(numeric(18,0),SummaryCGPA) between  '" + Convert.ToDecimal(txtFromResult.Trim()) + "' AND   '" + Convert.ToDecimal(txtToResult.Trim()) + "'");
                    }
                }

                if (UniversityName != null)
                {
                    if (UniversityName != "")
                        sb.Append("AND ApplicationID IN (SELECT EmployeeId FROM EmployeeEducation WHERE University_MoslemBhi = '" + UniversityName + "')");
                }


                //if (DataTypeSelected != null)
                //{
                //    if (DataTypeSelected != "")
                //    {
                //        if(DataTypeSelected == "FD")
                //            sb.Append(" AND convert(numeric(18,0),SummaryCGPA) between  '" + Convert.ToDecimal(1) + "' AND   '" + Convert.ToDecimal(5) + "'");

                //        if (DataTypeSelected == "BD")
                //            sb.Append(" AND convert(numeric(18,0),SummaryCGPA) >  '" + Convert.ToDecimal(5) + "'");
                //    }
                //}


                /*
                 
                if (ComponentName != null)
                {
                    if (ComponentName != "")
                        sb.Append(" AND ai.AppliedPostId LIKE '" + ComponentName.Trim() + "%'");
                }  

                 */

                List<ApplicationInfoViewModel> List_InvMasterViewModel = new List<ApplicationInfoViewModel>();

                if (ComponentResultType == "3" || ComponentResultType == "2")
                {
                    sb.Append(" ORDER BY [Bachelor]  desc ");
                }
                else
                {
                    //sb.Append(" ORDER BY cast([SummaryCGPA] AS DECIMAL(18,2))  desc ");
                }

                var param = new { AndCondition = sb.ToString() };
                if (onLoad == "Yes")
                {
                    if (empList2 == null)

                    {
                        empList2 = eRecruitSPService.GetDataWithParameter(param, "SP_Get_Applicants_List_ALLData");
                    }
                    empList = empList2;
                }
                else
                {
                    if (DataTypeSelected == "AD")
                        empList = eRecruitSPService.GetDataWithParameter(param, "SP_Get_Applicants_List_ALLData");
                    else if (DataTypeSelected == "BD")
                        empList = eRecruitSPService.GetDataWithParameter(param, "SP_Get_Applicants_List_BadData");
                    else
                        empList = eRecruitSPService.GetDataWithParameter(param, "SP_Get_Applicants_List");
                }

                List_InvMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new ApplicationInfoViewModel
                {
                    rowSlK = row.Field<long>("rowSl"),
                    ApplicationId = row.Field<long>("ApplicationId"),
                    ApplicantName = row.Field<string>("ApplicantName"),
                    PaentName = row.Field<string>("PaentName"),
                    DateOfBirthMsg = row.Field<string>("DateOfBirthMsg"),
                    Age = row.Field<string>("Age"),
                    PerHomeDistrict = row.Field<string>("PerHomeDistrict"),
                    PermanentThana = row.Field<string>("PermanentThana"),
                    SSC = row.Field<string>("SSC"),
                    HSC = row.Field<string>("HSC"),
                    Bachelor = row.Field<string>("Bachelor"),
                    Masters = row.Field<string>("Masters"),
                    SUMOfCGPA = row.Field<string>("SUMOfCGPA"),
                    SummaryCGPA = row.Field<string>("SummaryCGPA"),
                    SubjectName = row.Field<string>("Subject"),
                    UniversityName = row.Field<string>("UniversityName")

                }).ToList();

                var currentPageRecords = List_InvMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_InvMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function
        public ActionResult ApplicationInfoReportById(string ApplicationId)
        {
            try
            {
                // Main Report

                ApplicationId = ApplicationId.ToString();

                var param = new { ApplicationId = ApplicationId };
                var MainReport = eRecruitSPService.GetDataWithParameter(param, "dbo.SP_RPT_ApplicationInfo_By_Id");

                // SUB Report
                var param2 = new { ApplicationId = ApplicationId };
                var rptBotomInfo = eRecruitSPService.GetDataWithParameter(param2, "dbo.SP_RPT_EducationInfo_By_Id");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("EducationInfo", rptBotomInfo.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("ApplicationinfoById.rpt", MainReport.Tables[0], reportParam, subReportDB);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }// END Function Show Report.

        public ActionResult ApplicationInterViewCardById(string ApplicationId)
        {
            try
            {
                // Main Report

                ApplicationId = ApplicationId.ToString();

                var param = new { ApplicationId = ApplicationId };
                //Write SP FOR Data.. 
                var MainReport = eRecruitSPService.GetDataWithParameter(param, "dbo.SP_RPT_ApplicationInfo_By_Id_IntervieCard");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("eRecruit/Rpt_eRecruitment_InterviewCard.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }// END Function Show Report.

        public ActionResult eRecruitReports(string Option, string ComponentResultType)
        {
            try
            {
                var param = new { Option = Option, ComponentResultType = ComponentResultType };
                var MainReport = eRecruitSPService.GetDataWithParameter(param, "dbo.sp_Get_eRecruitReport");

                string ReportSubject = "";
                string ColumName = "";
                if (Option == "district")
                {
                    ReportSubject = "District Wise No. of Applicants";
                    ColumName = "Name of District";
                }
                else if (Option == "subject")
                {
                    ReportSubject = "Honours Subject Wise No. of Applicants";
                    ColumName = "Name of Subject";
                }
                else if (Option == "university")
                {
                    ReportSubject = "University Wise No. of Applicants";
                    ColumName = "Name of University";
                }
                else if (Option == "result")
                {
                    ReportSubject = "Result Wise No. of Applicants";
                    ColumName = "CGPA Range";
                }

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("ReportSubject", ReportSubject);
                reportParam.Add("ColumName", ColumName);

                ReportHelper.PrintReport("eRecruitReports.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }// END Function Show Report.

        public ActionResult GenerateReportToExcell(string Excel)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("ReportSubject", ReportSubject);
                //reportParam.Add("ColumName", ColumName);

                ReportHelper.ExportExcelReport("eRecruitFilterReports.rpt", empList.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult SETInterviewDate(string InterviewDate)
        {
            string result = string.Empty;
            try
            {
                var param = new
                {
                    InterviewDate = InterviewDate

                };
                var val = eRecruitSPService.GetDataWithParameter(param, "SETInterviewDate"); //SP_eeLoanAdjustment
                result = "Date Updated Successfully.";
            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        } // End of process 1.


    }
}