#region Usings

using gHRM.Core.Utilities.Constants;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.Helpers;

#endregion

namespace gHRM.Web.Controllers
{
    public class CommonReportGeneratorController : BaseController
    {
        #region Private Members
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;


        #endregion

        #region Ctor

        public CommonReportGeneratorController(IEmployeeSPService employeeSpService, IEmployeeStatusService employeeStatusService, IOfficeTypeService officeTypeService, IEmployeeService employeeService, ICompanyService companyService)
        {
            this.employeeSpService = employeeSpService;
            this.employeeStatusService = employeeStatusService;
            this.officeTypeService = officeTypeService;
            this.employeeService=employeeService;
            this.companyService = companyService;
    }
        #endregion

        #region CommonMethod
        private void MapDropdownForDropoutReasonList(EmployeeReportViewModel model)
        {
            var dropOutReasonList = employeeStatusService.GetAll().Where(p => p.IsActive == true && p.IsValid == false);
            var viewDropOutReasonList = dropOutReasonList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.StatusName,
                Value = p.StatusId.ToString()
            });
            var dropOutReason = new List<SelectListItem>();
            dropOutReason.Add(new SelectListItem { Text = "Please Select", Value = "" });
            dropOutReason.AddRange(viewDropOutReasonList);
            model.ReasonList = dropOutReason;
        }
        private void MapDropdownForOfficeTypeList(EmployeeReportViewModel model)
        {
            var officeTypeList = officeTypeService.GetAll().Where(p => p.IsActive == true);
            var viewOfficeTypeList = officeTypeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.OfficeTypeName,
                Value = p.OfficeTypeId.ToString()
            });
            var officeType = new List<SelectListItem>();
            officeType.Add(new SelectListItem { Text = "Please Select", Value = "" });
            officeType.AddRange(viewOfficeTypeList);
            model.OfficeTypeList = officeType;
        }



        #endregion

        #region ReportCallingMethod
 
        public ActionResult EmployeeDropoutByReasonReport(int reasonId, string dateFrom, string dateTo, string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
               string type = "view";
                var param = new
                {
                    ReasonId = reasonId,
                    //OfficeTypeID = officeTypeId, 
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.rpt_DropOutByReason");

                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                var reportDataSourceName = "EmployeeDropoutByReasonReport";
                string reportTitle = "Employee Dropout By Reason Report";
                string reportPath = "";
                if (SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                    reportPath = "~/Reports/RDLC/Employee/DropOutByReason2.rdlc";
                else
                    reportPath = "~/Reports/RDLC/Employee/DropOutByReason.rdlc";               
                //string reportPath = "~/Reports/RDLC/Employee/DropOutByReasonTestForMousumi.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult ActiveEmployeeInfoByDesignationReport(string dateFrom, string dateTo, string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                string type = "view";
                var param = new
                {
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };


                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetActiveStaffByDesignation");
           
                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);

                var reportDataSourceName = "ActiveEmployeeInfoByDesignation";

                string reportTitle = "Active Employee Info By Designation";
                string reportPath = "~/Reports/RDLC/Employee/ActiveEmployeeByDesignation.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }
       
        public ActionResult EmployeePersonalInfoReport(string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                string type = "view";
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
          
                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetEmployeePersonalInfo");
                string reportTitle = "Employee Personal Info Report";
                var parameters = new Dictionary<string, object>();
                parameters.Add("ReportTitle", reportTitle);
                var reportDataSourceName = "EmployeePersonalInfo";
                string reportPath = "~/Reports/RDLC/Employee/OfficeWisePersonalInfo.rdlc";
                string reportViewMode = ReportViewModeConstants.Landscape;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }
   
        public ActionResult MonthWiseConfirmationReport(string dateFrom, string dateTo, string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                string type = "view";
                var param = new
                {                 
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                
                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetMonthWiseConfirmationList");
                
                string reportTitle = "Month Wise Confirmation Report";
                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                parameters.Add("ReportTitle", reportTitle);
                var reportDataSourceName = "MonthWiseConfirmation";
                string reportPath = "~/Reports/RDLC/Employee/MonthWiseConfirmation.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;
                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult MonthWiseConfirmationDueReport(string dateFrom, string dateTo, string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                string type = "view";
                var param = new
                {
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };

                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetMonthWiseConfirmationDueList");

                string reportTitle = "Month Wise Confirmation Due Report";
                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                parameters.Add("ReportTitle", reportTitle);
                var reportDataSourceName = "MonthWiseConfirmation";
                string reportPath = "~/Reports/RDLC/Employee/MonthWiseConfirmationDue.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;
                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {              
                return RedirectToAction("CommonReportGenerationError");
            }
        }





        // Report fort Mousumi

        public ActionResult EmployeeDropoutByReasonReportForMousumi(int reasonId, string dateFrom, string dateTo, string format, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status, string employeeCode)
        {
            try
            {
                string type = "view";
                var param = new
                {
                    ReasonId = reasonId,
                    //OfficeTypeID = officeTypeId, 
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility,

                    EmployeeCode = string.IsNullOrEmpty(employeeCode) ? 0 : Convert.ToInt32(employeeCode)
                };

                DataSet mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.rpt_DropOutByReason");

                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                var reportDataSourceName = "EmployeeDropoutByReasonReport";
                string reportTitle = "Employee Dropout By Reason Report";                
                string reportPath = "~/Reports/RDLC/Employee/DropOutByReasonTestForMousumi.rdlc";
           //     string reportPath = "~/Reports/RDLC/Employee/DropOutByReasonTestForMousumiTest.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }







        #endregion

        #region Common Report Generation Error

        public ActionResult CommonReportGenerationError()
        {
            return View();
        } 

        #endregion

        #region Private Methods
        private DataSet GetEmployeeDropoutByReasonData(int reasonId, string dateFrom, string dateTo, int officeTypeId)
        {        
                var param = new
                {
                    ReasonId = reasonId,
                    OfficeTypeID = officeTypeId, 
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };
                var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.rpt_DropOutByReason");
                return mainDataSource;
          
        }
        private DataSet GetActiveEmployeeInfoByDesignationData(string dateFrom, string dateTo, int officeTypeId)
        {
            var param = new { OfficeTypeID = officeTypeId, DateFrom = dateFrom, DateTo = dateTo };
            var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetActiveStaffByDesignation");
            return mainDataSource;
        }
        private DataSet GetEmployeePersonalInfoData(int officeTypeId)
        {
            var param = new { OfficeTypeID = officeTypeId};
            var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetEmployeePersonalInfo");
            return mainDataSource;
        }
        private DataSet GetMonthWiseConfirmationData(string dateFrom, string dateTo, int officeTypeId)
        {
            var param = new { OfficeTypeID = officeTypeId, DateFrom = dateFrom, DateTo = dateTo };
            var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.GetMonthWiseConfirmationList");
            return mainDataSource;
        }
        #endregion

        #region  ApplicantCV

        public ActionResult ApplicantCVReport(string format, int Id = 0)
        {
            try
            {
                string type = "view";
                //DataSet mainDataSource = GetApplicantDataById(Id);
                var parameters = new Dictionary<string, object>();

                var param = new { Id = Id };

                var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.SP_GetApplicantInformationDataById");

                string reportTitle = " Applicant CV";

                string reportPath = "~/Reports/RDLC/Applicant/ApplicantCV.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;
      
                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "ApplicantMaster",Value = mainDataSource.Tables[0] },
                    new ReportDataSource{ Name = "ApplicantJobExp",Value = mainDataSource.Tables[1] },
                    new ReportDataSource{ Name = "ApplicantAcademicInfo",Value = mainDataSource.Tables[2] },
                    new ReportDataSource{ Name = "ApplicantTrainingInfo",Value = mainDataSource.Tables[3] },
                    new ReportDataSource{ Name = "ApplicantReferenceInfo",Value = mainDataSource.Tables[4] }
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        #endregion

        #region StaffProfile
        public ActionResult IndividualStaffProfile(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }

                var Company= companyService.GetMany(p => p.CompanyId == employee.CompanyId).FirstOrDefault();
                string image = Company.ImagePath;
                string CompanyImagePath = new Uri(Server.MapPath(image)).AbsoluteUri;
                parameters.Add("CompanyImage", CompanyImagePath);

                var param = new { EmpID = employeeId, OfficeId= officeId };     
                var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.SP_StaffProfileDataById");
                            
                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/StaffProfileAtAGalanceNew.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "CompanyInfo",Value = mainDataSource.Tables[0] },
                    new ReportDataSource{ Name = "PersonalDetails",Value = mainDataSource.Tables[1] },
                    new ReportDataSource{ Name = "StaffAddress",Value = mainDataSource.Tables[2] },
                    new ReportDataSource{ Name = "StaffOffice",Value = mainDataSource.Tables[3] },
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }
        #endregion

        #region JCF Report

        public ActionResult AppointmentLetter(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }

                parameters.Add("Id", employee.EmployeeId);

                var param = new { EmpID = employeeId, OfficeId = officeId };
           
                var DataSource1 = employeeSpService.GetDataWithParameter(param, "dbo.SP_EmployeePersonalInfoById");

                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/AppointmentLetter.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "PersonalInfo",Value = DataSource1.Tables[0] },
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult JobConfirmationLetter(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }

                parameters.Add("Id", employee.EmployeeId);

                var param = new { EmpID = employeeId, OfficeId = officeId };
                var param2 = new { EmpID = employeeId };
                var DataSource1 = employeeSpService.GetDataWithParameter(param, "dbo.SP_EmployeePersonalInfoById");
                var DataSource2 = employeeSpService.GetDataWithParameter(param2, "dbo.SP_EmployeeSalaryDetailsById");

                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/JobConfirmationLetter.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "PersonalInfo",Value = DataSource1.Tables[0] },
                    new ReportDataSource{ Name = "EmployeeSalaryDetails",Value = DataSource2.Tables[0] }
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult IncrementLetter(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }

                parameters.Add("Id", employee.EmployeeId);

                var param = new { EmpID = employeeId, OfficeId = officeId };
                var param2 = new { EmpID = employeeId };
                var DataSource1 = employeeSpService.GetDataWithParameter(param, "dbo.SP_EmployeePersonalInfoById");
                var DataSource2 = employeeSpService.GetDataWithParameter(param2, "dbo.SP_EmployeeSalaryDetailsById");

                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/IncrementLetter.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "PersonalInfo",Value = DataSource1.Tables[0] },
                    new ReportDataSource{ Name = "EmployeeSalaryDetails",Value = DataSource2.Tables[0] }
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult PromotionLetter(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }

                parameters.Add("Id", employee.EmployeeId);

                var param = new { EmpID = employeeId, OfficeId = officeId };
                var param2 = new { EmpID = employeeId };
                var DataSource1 = employeeSpService.GetDataWithParameter(param, "dbo.SP_EmployeePersonalInfoById");
                var DataSource2 = employeeSpService.GetDataWithParameter(param2, "dbo.SP_EmployeeSalaryDetailsById");

                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/PromotionLetter.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "PersonalInfo",Value = DataSource1.Tables[0] },
                    new ReportDataSource{ Name = "EmployeeSalaryDetails",Value = DataSource2.Tables[0] }
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        public ActionResult JobSeparationLetter(string format, string empCode)
        {
            try
            {
                long employeeId = 0; int officeId = 0;
                string type = "";
                var parameters = new Dictionary<string, object>();

                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                    officeId = (int)employee.OfficeId;
                }


                parameters.Add("Id", employee.EmployeeId);

                var param = new { EmpID = employeeId, OfficeId = officeId };

                var DataSource1 = employeeSpService.GetDataWithParameter(param, "dbo.SP_EmployeePersonalInfoById");

                string reportTitle = "Staff Profile";
                string reportPath = "~/Reports/RDLC/Employee/JobSeparationLetter.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "PersonalInfo",Value = DataSource1.Tables[0] },
                };

                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }

            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        #endregion

    }
}
