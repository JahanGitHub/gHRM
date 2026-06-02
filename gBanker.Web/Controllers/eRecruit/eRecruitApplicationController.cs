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



#endregion Usings

namespace gHRM.Web.Controllers.eRecruit
{
    public class eRecruitApplicationController : BaseController
    {

        #region Private Members
        private readonly IApplicationInfoService applicationInfoService;
       // private readonly IApplicantProfileSettingService applicantProfileSettingService;
        private readonly IeRecruitDegreeService educationDegreeService;
        private readonly IeRecruitEducationService employeeEducationService;
        private readonly ICountryService countryService;
        private readonly IDistrictService districtService;
        private readonly ILgThanaService thanaService;
        private readonly IUnionService unionService;
        private readonly IStateOrProvinceService sateOrProvinceService;
        private GetCommonDataList getCommonDataList;
        private readonly IEmployeeSPService employeeSpService;

        #endregion

        #region Ctor

        public eRecruitApplicationController(IApplicationInfoService applicationInfoService, IeRecruitDegreeService educationDegreeService, IeRecruitEducationService employeeEducationService, ICountryService countryService, IDistrictService districtService, ILgThanaService thanaService, IUnionService unionService, IStateOrProvinceService sateOrProvinceService, IEmployeeSPService employeeSpService)
        {
            this.applicationInfoService = applicationInfoService;
           // this.applicantProfileSettingService = applicantProfileSettingService;
            this.educationDegreeService = educationDegreeService;
            this.employeeEducationService = employeeEducationService;
            this.countryService = countryService;
            this.districtService = districtService;
            this.thanaService = thanaService;
            this.unionService = unionService;
            this.sateOrProvinceService = sateOrProvinceService;
            getCommonDataList = new GetCommonDataList();
            this.employeeSpService = employeeSpService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            //check valid to submit application
            if (!IsValidToSubmitApplication())
                return Redirect("/ApplicationInfo/SubmissionExpire");

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ddlList"] = items;
            var model = new ApplicationInfoViewModel();
            //populate dropdowns
            MapDropDownList(model);
            model.ServerCurrentDate = DateTime.Now;
            ViewData["ApplicationId"] = 0;
            return View(model);
        }

        #endregion

        #region Create Recruitement

        [HttpPost]
        public ActionResult Recruitment(ApplicationInfoViewModel model, FormCollection formCollection, List<ApplicationInfoViewModel> ProposalList)
        {
            //check valid to submit application
            if (!IsValidToSubmitApplication())
                return Json(new { result = 0, message = "Warning, Application submission date has been expired!" }, JsonRequestBehavior.AllowGet);

            long result = 0;

            bool isOperationSuccess = true;
            var message = "Application Received.";
            var entity = new ApplicationInfo();

            var applicantFilter = new BaseSearchFilter
            {
                NationalId = model.NationalId,
                ApplicantName = model.ApplicantName,
                ApplicationId = model.ApplicationId
            };

            var isExistApplicantInfo = applicationInfoService.IsExistApplicationInfo(applicantFilter);

            if (isExistApplicantInfo)
            {
                message = "This Applicant Already applied. ";
                return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
            }

            if (Request == null || Request.Files.Count <= 0)
            {
                message = "Warning, Please give image and signature.";
                return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
            }

            HttpPostedFileBase file = Request.Files["UploadImage"];
            HttpPostedFileBase fileSignature = Request.Files["UploadSignatureImage"];

            if (file == null)
            {
                message = "Warning, Please give image ";
                return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
            }

            if (fileSignature == null)
            {
                message = "Warning, Please give signature ";
                return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
            }

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
            {
                try
                {
                    //Populate Applicant Info
                    entity = PopulateApplicantInfo(model);

                    if (model.ApplicationId == 0) //THEN Create NEW
                    {
                        var empInfo = applicationInfoService.Create(entity);
                        result = empInfo.ApplicationId;
                        model.ApplicationId = empInfo.ApplicationId;

                        HttpCookie reqCookiesCreate = Request.Cookies["userInfo"];

                        if (reqCookiesCreate == null)
                        {
                            //Track Applicant Info Into Cookie
                            TrackApplicantInfoIntoCookie(model);
                        }
                    }
                    else // ELSE UPDATE
                    {
                        var employeeUpdate = applicationInfoService.GetByEmpId(model.ApplicationId);

                        //Populate Application Info For Update
                        PopulateApplicationInfoForUpdate(model, employeeUpdate);

                        applicationInfoService.Update(employeeUpdate);
                    }

                    //Delete Education Info
                    var educationFilter = new BaseSearchFilter { ApplicantId = model.ApplicationId };
                    var educationInfos = employeeEducationService.GetEmployeeEducationsByFilterByFilter(educationFilter);
                    if (educationInfos.Any())
                    {
                        foreach (var EducationId in educationInfos)
                            employeeEducationService.Delete((int)EducationId.EducationId);
                    }

                    var empID = model.ApplicationId;

                    if (ProposalList != null && ProposalList.Any())
                    {
                        foreach (var employeeEducation in ProposalList)
                        {
                            //Populate Employee Education Info
                            var newEmployeeEducation = PopulateEmployeeEducationInfo(empID, employeeEducation);

                            //let's create employee education info
                            employeeEducationService.Create(newEmployeeEducation);
                        }
                    }

                    if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        //populate applicant image info
                        ApplicationInfo employee = PopulateApplicantImageInfo(model, file, empID);
                        //let's update applicant image info
                        applicationInfoService.Update(employee);
                    }

                    if ((fileSignature != null) && (fileSignature.ContentLength != 0) && !string.IsNullOrEmpty(fileSignature.FileName))
                    {
                        //populate applicant signature info
                        ApplicationInfo employee = PopulateApplicantSignatureInfo(model, file, fileSignature, empID);
                        //let's update applicant signature info
                        applicationInfoService.Update(employee);
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    result = 0;
                    message = "Warning, There was a problem while Submitting Application or check your internet connection!";
                }

                if (isOperationSuccess)
                {
                    string key = ApplicantConfirmationConstants.ApplicantConfirmationInfoCookieKey;
                    string label = ApplicantConfirmationConstants.ApplicantSubmittedInfoCookieLabel;
                    string value = $@"{model.ApplicantName}_{model.ApplicationId}_{model.FatherName}_{model.MotherName}_{model.NationalId}
                                   _{model.MobileNo}_{model.Email}_{model.Nationality}_{model.Gender}_{model.Religion}_{model.DateOfBirth.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}";
                    //Track Applicant Cookie Info
                    TrackApplicantCookieInfo(key, label, value);

                    result = 1;
                    message = "Success, Application Submission Completed!";
                    ts.Complete();
                }

                ts.Dispose();
            }

            return Json(new { result = result, applicationId = model.ApplicationId, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Confirmation       

        public ActionResult Confirmation()
        {
            var applicantName = string.Empty;
            try
            {
                var applicationInfoCookieKeyInfo = Request.Cookies[ApplicantConfirmationConstants.ApplicantConfirmationInfoCookieKey];
                var applicationInfo = Request.Cookies[ApplicantConfirmationConstants.ApplicantConfirmationInfoCookieKey][ApplicantConfirmationConstants.ApplicantSubmittedInfoCookieLabel];
                if (applicationInfoCookieKeyInfo != null && applicationInfo != null)
                    applicantName = applicationInfo.Trim().ToString();

                if (string.IsNullOrWhiteSpace(applicantName))
                    return Redirect("/");

                var applicatntInfos = applicantName.Split('_');

                if (applicatntInfos == null || applicatntInfos.Length <= 0)
                    return Redirect("/");

                var model = new ApplicationConfirmationViewModel
                {
                    ApplicantName = applicatntInfos[0] != null ? applicatntInfos[0].ToString() : "N/A",
                    ApplicantId = applicatntInfos[1] != null ? Convert.ToInt32(applicatntInfos[1]) : 0,
                    FatherName = applicatntInfos[2] != null ? applicatntInfos[2].ToString() : "N/A",
                    MotherName = applicatntInfos[3] != null ? applicatntInfos[3].ToString() : "N/A",
                    NationalId = applicatntInfos[4] != null ? applicatntInfos[4].ToString() : "N/A",
                    MobileNo = applicatntInfos[5] != null ? applicatntInfos[5].ToString() : "N/A",
                    Email = applicatntInfos[6] != null ? applicatntInfos[6].ToString() : "N/A",
                    Nationality = applicatntInfos[7] != null ? applicatntInfos[7].ToString() : "N/A",
                    Gender = applicatntInfos[8] != null ? applicatntInfos[8].ToString() : "N/A",
                    Religion = applicatntInfos[9] != null ? applicatntInfos[9].ToString() : "N/A",
                    DateOfBirth = applicatntInfos[10] != null ? applicatntInfos[10].ToString() : "N/A"
                };

                string cookieName = ApplicantConfirmationConstants.ApplicantConfirmationInfoCookieKey;
                ResetCookie(cookieName);

                return View(model);
            }
            catch (Exception ex)
            {
                return Redirect("/");
            }
        }
        #endregion

        #region Submission Expire   

        public ActionResult SubmissionExpire()
        {
            //check valid to submit application
            if (IsValidToSubmitApplication())
                return Redirect("/applicationinfo/index");

            return View();
        }
        #endregion

        #region Others

        [HttpPost]
        public JsonResult CreateApplication(ApplicationInfoViewModel Model)
        {
            var entity = new ApplicationInfo();
            entity.ApplicantName = Model.ApplicantName;
            entity.ApplicantName = Model.ApplicantName;
            entity.IsActive = true;

            applicationInfoService.Create(entity);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Ajax Calls

        public JsonResult ApplicantNationalId(string NationalId)
        {
            var result = string.Empty;
            try
            {
                var appllicantInfo = applicationInfoService.GetByNID(NationalId);
                if (appllicantInfo != null && appllicantInfo.ApplicationId > 0)
                {
                    result = "1";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result = "0";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = "0";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ApplicantBirthRegistrationNo(string BirthRegistrationNo)
        {
            var result = string.Empty;
            try
            {
                var appllicantInfo = applicationInfoService.GetByBirthRegistrationNo(BirthRegistrationNo);
                if (appllicantInfo != null && appllicantInfo.ApplicationId > 0)
                {
                    result = "1";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result = "0";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = "0";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RollNoChecking(string DegreeCode, string RollNo, string BoardName, string PassingYear)
        {
            try
            {
                var filter = new BaseSearchFilter
                {
                    DegreeTitle = DegreeCode,
                    RollNoVerify = RollNo,
                    BoardName = BoardName,
                    PassingYear = PassingYear

                };

                var appllicantInfo = employeeEducationService.GetEmployeeEducationInfoByFilter(filter);

                if (appllicantInfo == null)
                    return Json("Error", JsonRequestBehavior.AllowGet);

                var employeeRollNumber = appllicantInfo.RollNo;
                var data = new { employeeRollNumber = employeeRollNumber, RollNo = RollNo };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new { RollNo = RollNo };
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ApplicantVerificy(string RollNoVerify, string BoardName, string PassingYear)
        {
            try
            {
                var filter = new BaseSearchFilter
                {
                    DegreeTitle = DegreeTitleConstants.SSC,
                    RollNoVerify = RollNoVerify,
                    BoardName = BoardName,
                    PassingYear = PassingYear
                };

                var appllicantInfo = employeeEducationService.GetEmployeeEducationInfoByFilter(filter);

                if (appllicantInfo == null)
                    return Json("Error", JsonRequestBehavior.AllowGet);

                var ApplicantRoll = appllicantInfo.EmployeeId;
                return Json(ApplicantRoll, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

      

        public JsonResult GetStateList(string country_id)
        {
            var filter = new BaseSearchFilter { CountryId = Convert.ToInt32(country_id) };
            var stateList = sateOrProvinceService.GetStateOrProvinceListByFilter(filter);

            var viewState = stateList.Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(new { Data = state_items }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictList(int state_id)
        {
            if (state_id > 0)
            {
                var filter = new BaseSearchFilter { StateOrProvinceId = Convert.ToInt32(state_id) };

                var districtList = districtService.GetDistrictListByFilter(filter);
                var viewDistrict = districtList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.district_id.ToString(),
                    Text = x.district_name_eng.ToString()
                });
                var district_items = new List<SelectListItem>();
                district_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                district_items.AddRange(viewDistrict);
                return Json(new { Data = district_items }, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThanaList(string district_id)
        {
            if (district_id != "null")
            {
                var filter = new BaseSearchFilter { DistrictId = Convert.ToInt32(district_id) };

                var thanaList = thanaService.GetLgThanaListByFilter(filter);
                var viewThana = thanaList.Select(x => new SelectListItem
                {
                    Value = x.thana_id.ToString(),
                    Text = x.thana_name_eng.ToString()
                });
                var thana_items = new List<SelectListItem>();
                thana_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                thana_items.AddRange(viewThana);
                return Json(thana_items, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnionList(string thana_id)
        {
            if (thana_id != "null")
            {
                var filter = new BaseSearchFilter { ThanaId = Convert.ToInt32(thana_id) };

                var unionList = unionService.GetLgUnionListByFilter(filter);
                var viewUnion = unionList.Select(x => new SelectListItem
                {
                    Value = x.union_id.ToString(),
                    Text = x.union_name_eng.ToString()
                });
                var union_items = new List<SelectListItem>();
                union_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                union_items.AddRange(viewUnion);
                return Json(union_items, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApplicationInfoReport()
        {
            try
            {
                //var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true).Select(p => p.EmployeeId).FirstOrDefault();
                //var param = new { officeId = officeId, OfficeTypeId = OfficeTypeId };
                var MainReport = employeeSpService.GetDataWithoutParameter("dbo.SP_RPT_ApplicationInfo");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Applicationinfo.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Report

        public ActionResult ApplicationInfoReportById(string ApplicationId)
        {
            try
            {
                var param = new { ApplicationId = ApplicationId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "dbo.SP_RPT_ApplicationInfo_By_Id");

                // SUB Report
                var param2 = new { ApplicationId = ApplicationId };
                var rptBotomInfo = employeeSpService.GetDataWithParameter(param2, "dbo.SP_RPT_EducationInfo_By_Id");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("EducationInfo", rptBotomInfo.Tables[0]);

                var reportParam = new Dictionary<string, object>();

                if (MainReport.Tables[0].Rows.Count == 0)
                    ReportHelper.PrintReport("ErrorMessage.rpt", MainReport.Tables[0], reportParam);

                else
                    ReportHelper.PrintWithSubReport("ApplicationinfoById.rpt", MainReport.Tables[0], reportParam, subReportDB);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "Please close your browser and try again." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods

        private bool IsValidToSubmitApplication()

        {
            bool isValid = true;
            DateTime todayDate = DateTime.Now;
            DateTime dateLine = new DateTime(2020, 10, 16, 0, 0, 0, DateTimeKind.Local);

            if (todayDate > dateLine)
                isValid = false;

            return isValid;
        }

        private ApplicationInfo PopulateApplicantSignatureInfo(ApplicationInfoViewModel model, HttpPostedFileBase file, HttpPostedFileBase fileSignature, long empID)
        {
            string fileName = fileSignature.FileName;
            string fileContentType = fileSignature.ContentType;
            byte[] fileBytes = new byte[fileSignature.ContentLength];
            file.InputStream.Read(fileBytes, 0, file.ContentLength);
            var data = fileSignature.InputStream.Read(fileBytes, 0, Convert.ToInt32(fileSignature.ContentLength));
            var fileTYpe = Path.GetExtension(fileSignature.FileName).GetType();

            string imgFolder = "ApplicantSignatures";

            bool exists = System.IO.Directory.Exists(Server.MapPath("~//" + imgFolder));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~//" + imgFolder));
            }
            var employee = applicationInfoService.GetByEmpId(empID);
            var filePath = Server.MapPath("~//" + imgFolder + "/") + employee.ApplicantName + "_" + empID.ToString() + "_" + fileName.Trim();
            System.IO.File.WriteAllBytes(filePath, fileBytes);
            var imgUrl = "/" + imgFolder + "/" + employee.ApplicantName + "_" + empID.ToString() + "_" + fileName.Trim();
            employee.EmployeeSignatureLink = imgUrl;
            employee.ApplicantSignature = fileBytes;
            model.ApplicantSignature = fileBytes;
            return employee;
        }

        private void ResetCookie(string cookieName)
        {
            var userInfo = Request.Cookies[cookieName];
            if (userInfo != null)
            {
                userInfo.Expires = DateTime.Now.AddDays(-1);
                Response.SetCookie(userInfo);
            }
        }

        private void TrackApplicantCookieInfo(string key, string label, string value)
        {
            HttpCookie applicantConfirmationInfoCookie = new HttpCookie(key);
            applicantConfirmationInfoCookie[label] = value;

            applicantConfirmationInfoCookie.Expires.Add(new TimeSpan(0, 0, 30, 2));
            Response.SetCookie(applicantConfirmationInfoCookie);

            return;
        }

        private ApplicationInfo PopulateApplicantImageInfo(ApplicationInfoViewModel model, HttpPostedFileBase file, long empID)
        {
            string fileName = file.FileName;
            string fileContentType = file.ContentType;
            byte[] fileBytes = new byte[file.ContentLength];
            file.InputStream.Read(fileBytes, 0, file.ContentLength);
            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            var fileTYpe = Path.GetExtension(file.FileName).GetType();

            string imgFolder = "ApplicantImages";

            bool exists = System.IO.Directory.Exists(Server.MapPath("~//" + imgFolder));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~//" + imgFolder));
            }
            var employee = applicationInfoService.GetByEmpId(empID);
            var filePath = Server.MapPath("~//" + imgFolder + "/") + employee.ApplicantName + "_" + empID.ToString() + "_" + fileName.Trim();
            System.IO.File.WriteAllBytes(filePath, fileBytes);
            var imgUrl = "/" + imgFolder + "/" + employee.ApplicantName + "_" + empID.ToString() + "_" + fileName.Trim();
            employee.EmployeeImageLink = imgUrl;
            employee.ApplicantImage = fileBytes;
            model.ApplicantImage = fileBytes; // NEW ADDED KHALID
            return employee;
        }

        private eRecruitEmployeeEducation PopulateEmployeeEducationInfo(long EmpID, ApplicationInfoViewModel employeeEducation)
        {
            var newEmployeeEducation = new eRecruitEmployeeEducation();
            newEmployeeEducation.EmployeeId = EmpID;
            newEmployeeEducation.DegreeTitle = employeeEducation.DegreeCode;
            newEmployeeEducation.InstitutionName = employeeEducation.Comment;
            newEmployeeEducation.UniversityName = employeeEducation.Comment2;
            newEmployeeEducation.PassingYear = employeeEducation.PassingYear;
            newEmployeeEducation.GPA = employeeEducation.GPA;
            newEmployeeEducation.RollNo = employeeEducation.RollNo;
            newEmployeeEducation.RegNo = employeeEducation.RegNo;
            newEmployeeEducation.ObtainedMarks = employeeEducation.ObtainedMarks;
            newEmployeeEducation.SubjectName = employeeEducation.SubjectName;
            newEmployeeEducation.GroupName = employeeEducation.GroupName;
            newEmployeeEducation.BoardName = employeeEducation.BoardName;
            newEmployeeEducation.GradeTypeId = employeeEducation.GradeTypeId;
            newEmployeeEducation.IsActive = true;
            newEmployeeEducation.CreateUser = 1;
            newEmployeeEducation.UpdateUser = 1;
            newEmployeeEducation.CreateDate = DateTime.UtcNow;
            newEmployeeEducation.UpdateDate = DateTime.UtcNow;
            return newEmployeeEducation;
        }

        private void PopulateApplicationInfoForUpdate(ApplicationInfoViewModel model, ApplicationInfo employeeUpdate)
        {
            employeeUpdate.ApplicationId = model.ApplicationId;
            employeeUpdate.ApplicantName = model.ApplicantName;
            employeeUpdate.FatherName = model.FatherName;
            employeeUpdate.MotherName = model.MotherName;
            employeeUpdate.DateOfBirth = model.DateOfBirth;
            employeeUpdate.Age = model.Age;
            employeeUpdate.Gender = model.Gender;
            employeeUpdate.BloodGroup = model.BloodGroup;
            employeeUpdate.MaritalStatus = model.MaritalStatus;
            employeeUpdate.NationalId = model.NationalId;
            employeeUpdate.BirthRegistrationNo = model.BirthRegistrationNo;
            employeeUpdate.Religion = model.Religion;
            employeeUpdate.Height = model.Height;
            employeeUpdate.Weight = model.Weight;

            employeeUpdate.ReferenceName = model.ReferenceName;
            employeeUpdate.ReferenceFatherName = model.ReferenceFatherName;
            employeeUpdate.ReferenceMotherName = model.ReferenceMotherName;
            employeeUpdate.ReferenceRelation = model.ReferenceRelation;
            employeeUpdate.ReferenceAddress = model.ReferenceAddress;
            employeeUpdate.ReferenceContactNo = model.ReferenceContactNo;

            employeeUpdate.SecondReferenceName = model.SecondReferenceName;
            employeeUpdate.SecondReferenceFatherName = model.SecondReferenceFatherName;
            employeeUpdate.SecondReferenceMotherName = model.SecondReferenceMotherName;
            employeeUpdate.SecondReferenceRelation = model.SecondReferenceRelation;
            employeeUpdate.SecondReferenceAddress = model.SecondReferenceAddress;
            employeeUpdate.SecondReferenceContactNo = model.SecondReferenceContactNo;

            employeeUpdate.Nationality = model.Nationality;
            employeeUpdate.GBIdNo = model.GBIdNo;
            employeeUpdate.Expreience = model.Expreience;
            employeeUpdate.ExtraCurriculum = model.ExtraCurriculum;
            employeeUpdate.ApplicationDate = model.ApplicationDate;

            employeeUpdate.PresentCountryId = model.GuarantorPresentCountryId == null ? 0 : model.GuarantorPresentCountryId;
            employeeUpdate.PresentDivisionId = model.GuarantorPresentDivisionId == null ? 0 : model.GuarantorPresentDivisionId;
            employeeUpdate.PresentDistrictId = model.GuarantorPresentDistrictId == null ? 0 : model.GuarantorPresentDistrictId;
            employeeUpdate.PresentThanaId = model.GuarantorPresentThanaId == null ? "" : model.GuarantorPresentThanaId;
            employeeUpdate.PresentUnionId = model.GuarantorPresentUnionId == null ? "" : model.GuarantorPresentUnionId;
            employeeUpdate.PresentStreetOrHouse = model.GuarantorPresentStreetOrHouse == null ? "" : model.GuarantorPresentStreetOrHouse;
            employeeUpdate.PresentZipCode = model.GuarantorPresentZipCode == null ? "" : model.GuarantorPresentZipCode;
            employeeUpdate.PresentPostOffice = model.GuarantorPresentPostOffice == null ? "" : model.GuarantorPresentPostOffice;

            employeeUpdate.PermanentCountryId = model.GuarantorPermanentCountryId == null ? 0 : model.GuarantorPermanentCountryId;
            employeeUpdate.PermanentDivisionId = model.GuarantorPermanentDivisionId == null ? 0 : model.GuarantorPermanentDivisionId;
            employeeUpdate.PermanentDistrictId = model.GuarantorPermanentDistrictId == null ? 0 : model.GuarantorPermanentDistrictId;
            employeeUpdate.PermanentThanaId = model.GuarantorPermanentThanaId == null ? "" : model.GuarantorPermanentThanaId;
            employeeUpdate.PermanentUnionId = model.GuarantorPermanentUnionId == null ? "" : model.GuarantorPermanentUnionId;
            employeeUpdate.PermanentStreetOrHouse = model.GuarantorPermanentStreetOrHouse == null ? "" : model.GuarantorPermanentStreetOrHouse;
            employeeUpdate.PermenantZipCode = model.GuarantorPermanentZipCode == null ? "" : model.GuarantorPermanentZipCode;
            employeeUpdate.PermenantPostOffice = model.GuarantorPermenantPostOffice == null ? "" : model.GuarantorPermenantPostOffice;
            employeeUpdate.MobileNo = model.MobileNo;
            employeeUpdate.Email = model.Email;
            employeeUpdate.ApplicantImage = model.ApplicantImage;
            employeeUpdate.ApplicantSignature = model.ApplicantSignature;
            employeeUpdate.ApplicantProfileSettingId = model.ApplicantProfileSettingId;
            employeeUpdate.AppliedPostId = model.AppliedPostId;
            employeeUpdate.IsActive = true;
            employeeUpdate.CreateUser = 1;
            employeeUpdate.UpdateUser = 1;
            //employeeUpdate.CreateDate = DateTime.UtcNow.AddMinutes(1);
            employeeUpdate.UpdateDate = DateTime.UtcNow.AddMinutes(1);

            employeeUpdate.IsFinalSubmit = true;
        }

        private void TrackApplicantInfoIntoCookie(ApplicationInfoViewModel model)
        {
            HttpCookie userInfo = new HttpCookie("userInfo");
            userInfo["UserName"] = model.ApplicationId.ToString();

            userInfo.Expires.Add(new TimeSpan(0, 0, 30, 2));
            Response.SetCookie(userInfo);
        }

        private ApplicationInfo PopulateApplicantInfo(ApplicationInfoViewModel model)
        {
            var entity = new ApplicationInfo();

            entity.ApplicantName = model.ApplicantName;
            entity.FatherName = model.FatherName;
            entity.MotherName = model.MotherName;
            entity.DateOfBirth = model.DateOfBirth;
            entity.Age = model.Age;
            entity.Gender = model.Gender;
            entity.BloodGroup = model.BloodGroup;
            entity.MaritalStatus = model.MaritalStatus;
            entity.NationalId = model.NationalId;
            entity.BirthRegistrationNo = model.BirthRegistrationNo;
            entity.Religion = model.Religion;
            entity.Height = model.Height;
            entity.Weight = model.Weight;

            entity.ReferenceName = model.ReferenceName;
            entity.ReferenceFatherName = model.ReferenceFatherName;
            entity.ReferenceMotherName = model.ReferenceMotherName;
            entity.ReferenceRelation = model.ReferenceRelation;
            entity.ReferenceAddress = model.ReferenceAddress;
            entity.ReferenceContactNo = model.ReferenceContactNo;

            entity.SecondReferenceName = model.SecondReferenceName;
            entity.SecondReferenceFatherName = model.SecondReferenceFatherName;
            entity.SecondReferenceMotherName = model.SecondReferenceMotherName;
            entity.SecondReferenceRelation = model.SecondReferenceRelation;
            entity.SecondReferenceAddress = model.SecondReferenceAddress;
            entity.SecondReferenceContactNo = model.SecondReferenceContactNo;

            entity.Nationality = model.Nationality;
            entity.GBIdNo = model.GBIdNo;
            entity.Expreience = model.Expreience;
            entity.ExtraCurriculum = model.ExtraCurriculum;
            entity.ApplicationDate = model.ApplicationDate;

            entity.PresentCountryId = model.GuarantorPresentCountryId == null ? 0 : model.GuarantorPresentCountryId;
            entity.PresentDivisionId = model.GuarantorPresentDivisionId == null ? 0 : model.GuarantorPresentDivisionId;
            entity.PresentDistrictId = model.GuarantorPresentDistrictId == null ? 0 : model.GuarantorPresentDistrictId;
            entity.PresentThanaId = model.GuarantorPresentThanaId == null ? "" : model.GuarantorPresentThanaId;
            entity.PresentUnionId = model.GuarantorPresentUnionId == null ? "" : model.GuarantorPresentUnionId;
            entity.PresentStreetOrHouse = model.GuarantorPresentStreetOrHouse == null ? "" : model.GuarantorPresentStreetOrHouse;
            entity.PresentZipCode = model.GuarantorPresentZipCode == null ? "" : model.GuarantorPresentZipCode;
            entity.PresentPostOffice = model.GuarantorPresentPostOffice == null ? "" : model.GuarantorPresentPostOffice;

            entity.PermanentCountryId = model.GuarantorPermanentCountryId == null ? 0 : model.GuarantorPermanentCountryId;
            entity.PermanentDivisionId = model.GuarantorPermanentDivisionId == null ? 0 : model.GuarantorPermanentDivisionId;
            entity.PermanentDistrictId = model.GuarantorPermanentDistrictId == null ? 0 : model.GuarantorPermanentDistrictId;
            entity.PermanentThanaId = model.GuarantorPermanentThanaId == null ? "" : model.GuarantorPermanentThanaId;
            entity.PermanentUnionId = model.GuarantorPermanentUnionId == null ? "" : model.GuarantorPermanentUnionId;
            entity.PermanentStreetOrHouse = model.GuarantorPermanentStreetOrHouse == null ? "" : model.GuarantorPermanentStreetOrHouse;
            entity.PermenantZipCode = model.GuarantorPermanentZipCode == null ? "" : model.GuarantorPermanentZipCode;
            entity.PermenantPostOffice = model.GuarantorPermenantPostOffice == null ? "" : model.GuarantorPermenantPostOffice;
            entity.MobileNo = model.MobileNo;
            entity.Email = model.Email;
            entity.ApplicantImage = model.ApplicantImage;
            entity.ApplicantSignature = model.ApplicantSignature;
            entity.ApplicantProfileSettingId = model.ApplicantProfileSettingId;
            entity.AppliedPostId = model.AppliedPostId;
            entity.IsActive = true;
            entity.CreateUser = 1;
            entity.UpdateUser = 1;
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;

            return entity;
        }

        private void MapDropDownList(ApplicationInfoViewModel model)
        {
            var empGroupName = new List<SelectListItem>();
            empGroupName.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            empGroupName.Add(new SelectListItem() { Text = "Finance", Value = "Finance" });
            empGroupName.Add(new SelectListItem() { Text = "Marketing", Value = "Marketing" });
            empGroupName.Add(new SelectListItem() { Text = "Management", Value = "Management" });
            empGroupName.Add(new SelectListItem() { Text = "Accounting", Value = "Accounting" });
            empGroupName.Add(new SelectListItem() { Text = "Physics", Value = "Physics" });
            empGroupName.Add(new SelectListItem() { Text = "Applied Physics", Value = "Applied Physics" });

            empGroupName.Add(new SelectListItem() { Text = "Electronic Engineering", Value = "Electronic Engineering" });

            empGroupName.Add(new SelectListItem() { Text = "Chemistry", Value = "Chemistry" });
            empGroupName.Add(new SelectListItem() { Text = "Bio-Chemistry", Value = "Bio-Chemistry" });
            empGroupName.Add(new SelectListItem() { Text = "Molecular Biology", Value = "Molecular Biology" });

            empGroupName.Add(new SelectListItem() { Text = "Applied Chemistry", Value = "Applied Chemistry" });
            empGroupName.Add(new SelectListItem() { Text = "Chemical Engineering", Value = "Chemical Engineering" });


            empGroupName.Add(new SelectListItem() { Text = "Mathematics", Value = "Mathematics" });
            empGroupName.Add(new SelectListItem() { Text = "Applied Mathematics", Value = "Applied Mathematics" });
            empGroupName.Add(new SelectListItem() { Text = "Statistics", Value = "Statistics" });
            empGroupName.Add(new SelectListItem() { Text = "Applied Statistics", Value = "Applied Statistics" });
            empGroupName.Add(new SelectListItem() { Text = "Computer Science", Value = "Computer Science" });
            empGroupName.Add(new SelectListItem() { Text = "CSE (Computer Science & Engineering)", Value = "CSE (Computer Science & Engineering)" });
            empGroupName.Add(new SelectListItem() { Text = "Software Engineering", Value = "Software Engineering" });
            empGroupName.Add(new SelectListItem() { Text = "IT", Value = "IT" });
            empGroupName.Add(new SelectListItem() { Text = "ICT", Value = "ICT" });

            empGroupName.Add(new SelectListItem() { Text = "Economics", Value = "Economics" });
            empGroupName.Add(new SelectListItem() { Text = "Sociology", Value = "Sociology" });
            empGroupName.Add(new SelectListItem() { Text = "Social Welfare", Value = "Social Welfare" });
            empGroupName.Add(new SelectListItem() { Text = "Social Work", Value = "Social Work" });
            empGroupName.Add(new SelectListItem() { Text = "Political Science", Value = "Political Science" });
            empGroupName.Add(new SelectListItem() { Text = "Government & Politics", Value = "Government & Politics" });
            empGroupName.Add(new SelectListItem() { Text = "Politics & Governance", Value = "Politics & Governance" });
            empGroupName.Add(new SelectListItem() { Text = "Law", Value = "Law" });
            empGroupName.Add(new SelectListItem() { Text = "Law & Land Administration", Value = "Law & Land Administration" });
            empGroupName.Add(new SelectListItem() { Text = "Bangla/Bengali", Value = "Bangla/Bengali" });
            empGroupName.Add(new SelectListItem() { Text = "English", Value = "English" });

            empGroupName.Add(new SelectListItem() { Text = "Science/Agriculture Science/Equivalent", Value = "Science/Agriculture Science/Equivalent" });
            empGroupName.Add(new SelectListItem() { Text = "Arts/Humanity/Equivalent", Value = "Arts/Humanity/Equivalent" });
            empGroupName.Add(new SelectListItem() { Text = "Commerce/Business Studies/Equivalent", Value = "Commerce/Business Studies/Equivalent" });
            empGroupName.Add(new SelectListItem() { Text = "Finance & Banking", Value = "Finance & Banking" });

            model.GroupNameList = empGroupName.OrderBy(x => x.Value).ToList();

            //model.GroupNameList = empGroupName;

            var empBoardName = new List<SelectListItem>();
            empBoardName.Add(new SelectListItem() { Text = "Dhaka", Value = "Dhaka", Selected = true });
            empBoardName.Add(new SelectListItem() { Text = "Chattogram", Value = "Chattogram" });
            empBoardName.Add(new SelectListItem() { Text = "Barishal", Value = "Barishal" });
            empBoardName.Add(new SelectListItem() { Text = "Comilla", Value = "Comilla" });
            empBoardName.Add(new SelectListItem() { Text = "Dinajpur", Value = "Dinajpur" });
            empBoardName.Add(new SelectListItem() { Text = "Jessore", Value = "Jessore" });
            empBoardName.Add(new SelectListItem() { Text = "Rajshahi", Value = "Rajshahi" });
            empBoardName.Add(new SelectListItem() { Text = "Sylhet", Value = "Sylhet" });
            empBoardName.Add(new SelectListItem() { Text = "Mymensingh", Value = "Mymensingh" });
            empBoardName.Add(new SelectListItem() { Text = "Madrasah", Value = "Madrasah" });
            empBoardName.Add(new SelectListItem() { Text = "Technical", Value = "Technical" });
            model.BoardNameList = empBoardName;



            var empReligion = new List<SelectListItem>();

            empReligion.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            empReligion.Add(new SelectListItem() { Text = "Islam", Value = "Islam" });
            empReligion.Add(new SelectListItem() { Text = "Hinduism", Value = "Hinduism" });
            empReligion.Add(new SelectListItem() { Text = "Buddhism", Value = "Buddhism" });
            empReligion.Add(new SelectListItem() { Text = "Christian", Value = "Christian" });
            model.ReligionList = empReligion;

            var empGender = new List<SelectListItem>();
            empGender.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            empGender.Add(new SelectListItem() { Text = "Male", Value = "M" });
            empGender.Add(new SelectListItem() { Text = "Female", Value = "F" });
            empGender.Add(new SelectListItem() { Text = "Common", Value = "C" });
            model.GenderList = empGender;
            //model.GenderList = getCommonDataList.GetGendersList(); //empGender;


            var appliedPost = new List<SelectListItem>();
            appliedPost.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            appliedPost.Add(new SelectListItem() { Text = "B.Sc. Engineer (Civil)", Value = "1" });
            appliedPost.Add(new SelectListItem() { Text = "Diploma Engineer", Value = "2" });
            appliedPost.Add(new SelectListItem() { Text = "Trainee Center Manager", Value = "3" });
            appliedPost.Add(new SelectListItem() { Text = "Trainee Officer", Value = "4" });

            model.AppliedPostList = appliedPost;

            var gradeType = new List<SelectListItem>();
            gradeType.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            gradeType.Add(new SelectListItem() { Text = "Division/Class", Value = "1" });
            gradeType.Add(new SelectListItem() { Text = "CGPA/GPA Scale 5.00", Value = "2" });
            gradeType.Add(new SelectListItem() { Text = "CGPA/GPA Scale 4.00", Value = "3" });
            model.GradeTypeList = gradeType;


            var empblood = new List<SelectListItem>();
            empblood.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            empblood.Add(new SelectListItem() { Text = "A+", Value = "A+" });
            empblood.Add(new SelectListItem() { Text = "B+", Value = "B+" });
            empblood.Add(new SelectListItem() { Text = "O+	", Value = "O+" });
            empblood.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
            empblood.Add(new SelectListItem() { Text = "A-", Value = "A-" });
            empblood.Add(new SelectListItem() { Text = "B-", Value = "B-" });
            empblood.Add(new SelectListItem() { Text = "O-", Value = "O-" });
            empblood.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
            model.BloodGroupList = empblood;


            var empMarital = new List<SelectListItem>();
            empMarital.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            empMarital.Add(new SelectListItem() { Text = "Married", Value = "M" });
            empMarital.Add(new SelectListItem() { Text = "Unmarried", Value = "U" });
            empMarital.Add(new SelectListItem() { Text = "Divorced", Value = "D" });
            model.MaritalList = empMarital;

            //Country Dropdown
            var countryList = countryService.GetAll();
            var viewCountryList = countryList.Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountryList);
            model.CountryList = country_items;

            model.StateOrProvinceList = getCommonDataList.GetEmptyListWithPleaseSelect();//stateList;

            model.DistrictList = getCommonDataList.GetEmptyListWithPleaseSelect();// districtList;


            model.ThanaList = getCommonDataList.GetEmptyListWithPleaseSelect();//thanaList;
            model.UnionList = getCommonDataList.GetEmptyListWithPleaseSelect();


            //Degree Level
            var filter = new BaseSearchFilter { CompanyId = 1 };
            var degreeLevelList = educationDegreeService.GetEducationDegreeListByFilter(filter);
            if (degreeLevelList.Any())
                degreeLevelList = degreeLevelList.DistinctBy(w => new { w.DegreeLevelId, w.DegreeLevel }).ToList();

            var viewdegreeList = degreeLevelList.Select(x => new SelectListItem
            {
                Value = x.DegreeLevelId.ToString(),
                Text = x.DegreeLevel.ToString()
            });

            var degree_items = new List<SelectListItem>();
            degree_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            degree_items.AddRange(viewdegreeList);
            model.DegreeLevelList = degree_items;
        }

        #endregion

    }
}