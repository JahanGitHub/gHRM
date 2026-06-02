using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using System.Text;
using gHRM.Web.DropDownService;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{

    public class EmployeeReportJCFController : BaseController
    {

        #region Variables
        private readonly IEmployeeReportOptionJCFService employeeReportOptionJCFService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;


        public EmployeeReportJCFController(
            IEmployeeReportOptionJCFService employeeReportOptionJCFService,
            IEmployeeSPService employeeSpService,
            IOfficeService officeService, 
            IOfficeTypeService officeTypeService, 
            IEmployeeStatusService employeeStatusService,
            IEmployeeDepartmentService employeeDepartmentService, 
            IEmployeeDesignationService employeeDesignationService,
            IEmployementTypeService employementTypeService,
            IEmployeeDepartmentSectionService employeeDepartmentSectionService)
        {
            this.employeeSpService = employeeSpService;
            this.employeeReportOptionJCFService = employeeReportOptionJCFService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.employeeDepartmentSectionService = employeeDepartmentSectionService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }
        #endregion

        #region Events
        public ActionResult ReportJCF()
        {
            var model = new EmployeeReportOptionJCFViewModel();
            MapDropdownForReport(model);
            return View(model);
        }

        // report 1
        public ActionResult OfficeWiseEmployeeRecruitmentReport(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string Gender, string DegreeCode, string ConcentrationCode, string ResponsibilityId)
        {
            try
            {
                StringBuilder officeAndondition = new StringBuilder();
                StringBuilder andCondition = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    officeAndondition.Append(" AND o.OfficeTypeId=" + _OfficeTypeId);
                }
                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    officeAndondition.Append(" AND o.OfficeId=" + _officeId);
                }

                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    andCondition.Append(" AND DepartmentId=" + _DepartmentId);
                }

                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    andCondition.Append(" AND SectionId=" + _SectionId);
                }
                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    andCondition.Append(" AND DesignationId=" + _DesignationId);
                }

                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    andCondition.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                //if (!String.IsNullOrEmpty(DegreeCode))
                //{
                //    string _DegreeCode = DegreeCode;
                //    andCondition.Append(" And DegreeCode='" + _DegreeCode + "'");
                //}
                //if (!String.IsNullOrEmpty(ConcentrationCode))
                //{
                //    string _ConcentrationCode = ConcentrationCode;
                //    andCondition.Append(" and ConcentrationCode='" + _ConcentrationCode + "'");
                //}

                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    andCondition.Append(" AND FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    andCondition.Append(" AND IsValid=" + _status);
                }

                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    andCondition.Append(" AND EmployementTypeId=" + _empType);
                }

                var param = new { OfficeAndondition = officeAndondition.ToString(), AndCondition = andCondition.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_EmployeeRecruitmentOfficeWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_ProjectWiseEmployeeRecruitment.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // report  2
        public ActionResult DepartmentWiseEmployeeRecruitment(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string ResponsibilityId)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                StringBuilder deptAndCondition = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    andCondition.Append(" AND OfficeTypeId=" + _OfficeTypeId);
                }

                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    andCondition.Append(" AND OfficeId=" + _officeId);
                }
               
                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    deptAndCondition.Append(" AND dep.DepartmentId=" + _DepartmentId);
                }

                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    andCondition.Append(" AND SectionId=" + _SectionId);
                }

                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    andCondition.Append(" AND DesignationId=" + _DesignationId);
                }

                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    andCondition.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                //if (!String.IsNullOrEmpty(DegreeCode))
                //{
                //    string _DegreeCode = DegreeCode;
                //    andCondition.Append(" And DegreeCode='" + _DegreeCode + "'");
                //}
                //if (!String.IsNullOrEmpty(ConcentrationCode))
                //{
                //    string _ConcentrationCode = ConcentrationCode;
                //    andCondition.Append(" and ConcentrationCode='" + _ConcentrationCode + "'");
                //}

                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    andCondition.Append(" AND FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    andCondition.Append(" AND IsValid=" + _status);
                }

                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    andCondition.Append(" AND EmployementTypeId=" + _empType);
                }

                var param = new { @AndCondition = andCondition.ToString(), @depAndCondition = deptAndCondition.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_EmployeeRecruitmentDepartmentWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_CoreProgramEmployeeRecruitmentReport.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // report  3
        public ActionResult DesignationWiseEmployeeRecruitment(string DateFrom, string DateTo, string status, string empType,  string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string Gender, string DegreeCode, string ConcentrationCode, string ResponsibilityId)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                StringBuilder desAndCondition = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    andCondition.Append(" AND OfficeTypeId=" + _OfficeTypeId);
                }

                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    andCondition.Append(" AND OfficeId=" + _officeId);
                }

                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    andCondition.Append(" AND DepartmentId=" + _DepartmentId);
                }

                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    andCondition.Append(" AND SectionId=" + _SectionId);
                }

                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    desAndCondition.Append(" AND ed.DesignationId=" + _DesignationId);
                }

                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    andCondition.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                //if (!String.IsNullOrEmpty(DegreeCode))
                //{
                //    string _DegreeCode = DegreeCode;
                //    andCondition.Append(" And DegreeCode='" + _DegreeCode + "'");
                //}
                //if (!String.IsNullOrEmpty(ConcentrationCode))
                //{
                //    string _ConcentrationCode = ConcentrationCode;
                //    andCondition.Append(" and ConcentrationCode='" + _ConcentrationCode + "'");
                //}

                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    andCondition.Append(" AND FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    andCondition.Append(" AND IsValid=" + _status);
                }

                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    andCondition.Append(" AND EmployementTypeId=" + _empType);
                }

                var param = new { @AndCondition = andCondition.ToString(),@desAndCondition = desAndCondition.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_EmployeeRecruitmentDesignationWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/DesignationWiseEmployeeRecruitment.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        //report 4

        public ActionResult RecruitmentEmployeeDetailsReport(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string Gender, string DegreeCode, string ConcentrationCode, string ResponsibilityId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND OfficeTypeId=" + _OfficeTypeId);
                }
                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND OfficeId=" + _officeId);
                }
               
                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    sb.Append(" AND DepartmentId=" + _DepartmentId);
                }

                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    sb.Append(" AND SectionId=" + _SectionId);
                }

                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    sb.Append(" AND DesignationId=" + _DesignationId);
                }

                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    sb.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                if (!String.IsNullOrEmpty(Gender))
                {

                    if (Gender == "F")
                    {
                        Gender = "Female";
                    }
                    else if (Gender == "M")
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Common";
                    }

                    string _Gender = Gender;
                    sb.Append(" AND Gender ='" + _Gender + "'");
                }

                //if (!String.IsNullOrEmpty(DegreeCode))
                //{
                //    string _DegreeCode = DegreeCode;
                //    sb.Append(" And edu.DegreeCode='" + _DegreeCode + "'");
                //}

                //if (!String.IsNullOrEmpty(ConcentrationCode))
                //{
                //    string _ConcentrationCode = ConcentrationCode;
                //    sb.Append(" and eduCon.ConcentrationCode='" + _ConcentrationCode + "'");
                //}
                
                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {

                    sb.Append(" AND FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    sb.Append(" AND IsValid=" + _status);
                }

                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    sb.Append(" AND EmployementTypeId=" + _empType);
                }

                var param = new { AndCondition = sb.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_EmployeeRecruitmentDetails");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_RecruitmentEmployeeDetails.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //report 5

        public ActionResult EmployeeAgeInformationReport(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId,string SectionId,string Gender,string Age, string AgeStatus, string ResponsibilityId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
               
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND OfficeTypeId=" + _OfficeTypeId);
                }

                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND OfficeId=" + _officeId);
                }
                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    sb.Append(" AND DesignationId=" + _DesignationId);
                }

                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    sb.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    sb.Append(" AND DepartmentId=" + _DepartmentId);
                }
              
                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    sb.Append(" AND SectionId=" + _SectionId);
                }
                if (!String.IsNullOrEmpty(Gender))
                {
                    if (Gender == "F")
                    {
                        Gender = "Female";
                    }
                    else if (Gender == "M")
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Common";
                    }

                    string _Gender = Gender;
                    sb.Append(" AND Gender ='" + _Gender + "'");
                }
             

                if (!String.IsNullOrEmpty(Age))
                {
                    if (AgeStatus == "A")
                    {
                        AgeStatus = ">";
                    }
                    else if (AgeStatus == "B")
                    {
                        AgeStatus = "<";
                    }
                    else if (AgeStatus == "E")
                    {
                        AgeStatus = "=";
                    }
                    sb.Append(" And DATEDIFF(year, DateOfBirth, GETDATE())" + AgeStatus + Convert.ToInt32(Age));
                }

                //if (!String.IsNullOrEmpty(DegreeCode))
                //{
                //    string _DegreeCode = DegreeCode;
                //    sb.Append(" And edu.DegreeCode='" + _DegreeCode + "'");
                //}

                //if (!String.IsNullOrEmpty(ConcentrationCode))
                //{
                //    string _ConcentrationCode = ConcentrationCode;
                //    sb.Append(" and eduCon.ConcentrationCode='" + _ConcentrationCode + "'");
                //}

                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    sb.Append(" AND FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    sb.Append(" AND IsValid=" + _status);
                }


                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    sb.Append(" AND EmployementTypeId=" + _empType);
                }

                var param = new { AndCondition = sb.ToString() };

                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_EmployeeAgeInformation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_EmployeeAgeInformation.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //report 6
        public ActionResult RecruitmentVSDropoutReport(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string ResponsibilityId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();

                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    //sb.Append(" AND emp.IsActive=" + _status);
                    sb.Append(" AND est.IsValid=" + _status);
                }
                if (!String.IsNullOrEmpty(OfficeTypeId))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    sb.Append(" AND ot.OfficeTypeId=" + _OfficeTypeId);
                }
                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    int _officeId = Convert.ToInt32(officeId);
                    sb.Append(" AND o.OfficeId=" + _officeId);
                    sb1.Append(" AND OfficeId=" + _officeId);
                }

                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    //sb.Append(" AND edepart.DepartmentId=" + _DepartmentId);
                    sb1.Append(" AND DepartmentId=" + _DepartmentId);

                }
                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    //sb.Append(" AND ed.DesignationId=" + _DesignationId);
                    sb1.Append(" AND DesignationId=" + _DesignationId);

                }
                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    //sb.Append(" AND eds.SectionId=" + _SectionId);
                    sb1.Append(" AND SectionId=" + _SectionId);

                }
                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    sb1.Append(" and EmployeeRank='" + _ResponsibilityId + "'");
                }

                var param = new { DateFrom = DateFrom, DateTo = DateTo, officeAndCondition = sb.ToString(), AndCondition = sb1.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_JCF_RecruitmentVSDropout");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_RecruitmentVSDropout.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //report 6
        public ActionResult EmployeeWiseGender(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string ResponsibilityId)
        {
            try
            {                
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = officeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "OfficeTypeId",
                    Value = string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId.ToString()
                });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "DesignationId",
                    Value = string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId.ToString()
                });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "DepartmentId",
                    Value = string.IsNullOrEmpty(DepartmentId) ? "0" : DepartmentId.ToString()
                });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "SectionId",
                    Value = string.IsNullOrEmpty(SectionId) ? "0" : SectionId.ToString()
                });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "EmployeeStatusArr",
                    Value = string.IsNullOrEmpty(status) ? "1,2,3,4,5,6,7,8,9,10,11" : status.ToString()
                });



                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "FromDate",
                    Value = string.IsNullOrEmpty(DateFrom) ? "2025-01-01" : DateFrom.ToString()
                });


                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = DateTo });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });

                // Call the correct SSRS report
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeWiseGender", paramValues.ToArray());
                // staff position adi 
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }


        }



        //report 6
        public ActionResult DesignationWiseJoiningResign(string DateFrom, string DateTo, string status, string empType, string officeId, string OfficeTypeId, string DepartmentId, string DesignationId, string SectionId, string ResponsibilityId)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = officeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "OfficeTypeId",
                    Value = string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId.ToString()
                });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "DesignationId",
                    Value = string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId.ToString()
                });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "DepartmentId",
                    Value = string.IsNullOrEmpty(DepartmentId) ? "0" : DepartmentId.ToString()
                });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "SectionId",
                    Value = string.IsNullOrEmpty(SectionId) ? "0" : SectionId.ToString()
                });

            
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue()
                {
                    Name = "FromDate",
                    Value = string.IsNullOrEmpty(DateFrom) ? "2025-01-01" : OfficeTypeId.ToString()
                });


                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = DateTo });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });

                // Call the correct SSRS report
                PrintSSRSReport("/gHRMPlus_Reports/DesignationWiseJoiningResign", paramValues.ToArray());
                // adi  ortho bosor desgination region ortho bosor 
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }


        }

        #endregion




        public JsonResult GetDepartmentWiseSection(int deptId)
        {
            var sectionList = employeeDepartmentSectionService.GetAll().Where(p => p.IsActive == true && p.DepartmentId == deptId).ToList();
            var viewSectionList = sectionList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.SectionName,
                Value = p.SectionId.ToString()
            }).ToList();
            var secList = new List<SelectListItem>();
            secList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            secList.AddRange(viewSectionList);
            return Json(secList, JsonRequestBehavior.AllowGet);
        }
        public void MapDropdownForReport(EmployeeReportOptionJCFViewModel model)
        {
            var empReportList = employeeReportOptionJCFService.GetMany(p => p.IsActive == true).OrderBy(p => p.DisplaySL);
            var viewList = empReportList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmpReportTypeName,
                Value = p.EmpReportTypeId.ToString()
            }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.ReportList = list;

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
            //ofc_items.AddRange(viewOfcList);
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
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            var empStatus = new List<SelectListItem>();
            empStatus.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            var statusList = employeeStatusService.GetMany(x => x.IsActive == true).OrderBy(p => p.ViewOrder);
            var getEmpStatus = statusList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.StatusName,
                Value = row.StatusValue

            }).ToList();
            empStatus.AddRange(getEmpStatus);
            model.EmployeeStatusList = empStatus;

            var empType = employementTypeService.GetMany(p => p.IsActive == true).OrderBy(p => p.ViewOrder).ToList();
            var viewEmpType = empType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmployementTypeName,
                Value = p.EmployementTypeId.ToString()
            }).ToList();
            var typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            typeList.AddRange(viewEmpType);
            model.EmploymentTypeList = typeList;

            var departmentList = employeeDepartmentService.GetAll();
            var viewDepartmentList = departmentList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DepartmentCode, m.DepartmentName), Value = m.DepartmentId.ToString() });
            var dep_items = new List<SelectListItem>();
            dep_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            dep_items.AddRange(viewDepartmentList);
            model.DepartmentList = dep_items;

            var designationList = employeeDesignationService.GetAll();
            var viewDesignationList = designationList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_items = new List<SelectListItem>();
            desig_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_items.AddRange(viewDesignationList);
            model.DesignationList = desig_items;
            model.ResponsibilityList = commonDynamicDropDown.GetAllOfficeDesignationList();


            var activeInactiveList = new List<SelectListItem>();
            activeInactiveList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            activeInactiveList.Add(new SelectListItem() { Text = "Active", Value = "1" });
            activeInactiveList.Add(new SelectListItem() { Text = "Inactive", Value = "2" });
            model.ActiveInactiveList = activeInactiveList;

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.SectionList = sectionList;

            var genderList = new List<SelectListItem>();
            genderList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            genderList.AddRange(commonStaticDropDown.GetGendersList());
            model.GenderList = genderList;
            model.EducationDegreeList = commonDynamicDropDown.GetEducationDegreeList();
            model.EducationConcentrationList = commonStaticDropDown.ddlInitial();
        }

    }
}
