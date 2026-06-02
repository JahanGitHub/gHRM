using AutoMapper;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.DBDetailModels.Apply;
using gHRM.Service;
using gHRM.Service.Apply;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Apply;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Globalization;
using System.Dynamic;

namespace gHRM.Web.Controllers.Apply
{
    public class ApplyController : BaseController
    {
        #region Variables
        private readonly IApplicantMasterService ApplicantMasterService;
        private readonly IApplicantJobExperienceService ApplicantJobExperienceService;
        private readonly IExamTitleService ExamTitleService;
        private readonly ILevelofEducationService LevelofEducationService;
        private readonly IApplicantAccademicService ApplicantAccademicService;
        private readonly IAppliedPostService AppliedPostService;
        private readonly IApplicantTrainingInfoService ApplicantTrainingInfoService;
        private readonly IApplicantReferenceInfoService ApplicantReferenceInfoService;
        private readonly IApplicantAddressInfoService ApplicantAddressInfoService;
        private readonly IJobsCircularService JobsCircularService;
        private readonly IQuestionAnsweredByApplicantService QuestionAnsweredByApplicantService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IKeyCloakService keyCloakService;
        //private CommonStaticDropDown commonStaticDropDown;
        //private CommonDynamicDropDown commonDynamicDropDown;

        public ApplyController(
            IApplicantMasterService ApplicantMasterService,
            IApplicantJobExperienceService ApplicantJobExperienceService,
            IApplicantAccademicService ApplicantAccademicService,
            IJobsCircularService JobsCircularService,
        IApplicantTrainingInfoService ApplicantTrainingInfoService,
            IApplicantReferenceInfoService ApplicantReferenceInfoService,
            IApplicantAddressInfoService ApplicantAddressInfoService,
            IAppliedPostService AppliedPostService,
            IQuestionAnsweredByApplicantService QuestionAnsweredByApplicantService,
        IExamTitleService ExamTitleService,
            ILevelofEducationService LevelofEducationService,
        IKeyCloakService keyCloakService,
            IEmployeeService employeeService,
            IEmployeeSPService employeeSPService

            )
        {
            this.ApplicantMasterService = ApplicantMasterService;
            this.ApplicantJobExperienceService = ApplicantJobExperienceService;
            this.ApplicantAccademicService = ApplicantAccademicService;
            this.ApplicantTrainingInfoService = ApplicantTrainingInfoService;
            this.ApplicantReferenceInfoService = ApplicantReferenceInfoService;
            this.ApplicantAddressInfoService = ApplicantAddressInfoService;
            this.JobsCircularService = JobsCircularService;
            this.AppliedPostService = AppliedPostService;
            this.QuestionAnsweredByApplicantService = QuestionAnsweredByApplicantService;
            this.ExamTitleService = ExamTitleService;
            this.LevelofEducationService = LevelofEducationService;
            this.keyCloakService = keyCloakService;
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;

        }
        #endregion

        #region Step_01
        public ActionResult Step_01(int? AppplicantId, int? JobId)
        {
            var model = new ApplicantMasterViewModel();

            model.UserId = LoggedInEmployeeId;

            long? UserId = model.UserId;
            if (AppplicantId > 0)
            {
                int Id = (int)Convert.ToInt64(AppplicantId);
                var _model = ApplicantMasterService.GetById(Id);
                model = Mapper.Map<ApplicantMaster, ApplicantMasterViewModel>(_model);
            }

            if (UserId > 0)
            {
                var _model = ApplicantMasterService.GetByUserId(UserId);

                if (_model !=null)
                {
                    model = Mapper.Map<ApplicantMaster, ApplicantMasterViewModel>(_model);
                    model.BirthDateMsg = _model.DateofBirth.ToString("dd-MMM-yyyy");

                    string data = Convert.ToBase64String(model.ImageByte);

                    ViewBag.Image = string.Format("data:image/png;base64,{0}", data);

                    if(_model.CoverLetterByte !=null)
                    {
                        ViewBag.CoverLetterVal = 1;
                    }
                    if (_model.AttachedCVByte != null || _model.ID !=null)
                    {
                        ViewBag.AttachedCVVal = 1;
                    }
                }
            }

            ViewBag.JobId = JobId;

            return View(model);

        }

        [HttpPost]
        public ActionResult Step_01(ApplicantMasterViewModel model, FormCollection collection)
        {

            return View();
        }

        public async Task<JsonResult> SavePersonalInfo(string FirstName, string LastName, string FatherName, string MotherName, string GuardianName, string BirthDateMsg, string Gender, string Religion, string MaritalStatus,
    string Nationality, decimal? NationalId, decimal? PassportNumber, string PrimaryMobile, string SecondaryMobile, string BloodGroup, string Availablefor,
            string CareerObjective, decimal? PresentSalary, decimal? ExpectedSalary, string LookingforJob_Level, string CareerSummary, string SpecialQualification, string QualificationKeyword, string PresentAddress, string PermanentAddress, string PrimaryEmail
            )
        {

            int result = 0;
            string message = "";
            int Id = 0;

            var _model = new ApplicantMasterViewModel();

            _model.UserId = LoggedInEmployeeId;
            long? UserId = _model.UserId;

            if (UserId > 0)
            {

                var Resultmodel = ApplicantMasterService.GetByUserId(UserId);
                //Id = (int)Resultmodel.ID;
            }


            try
            {
                var model = new ApplicantMaster();

                model.UserId = LoggedInEmployeeId;
                model.FirstName = FirstName;
                model.LastName = LastName;
                model.FatherName = FatherName;
                model.MotherName = MotherName;
                model.GuardianName = GuardianName;
                model.DateofBirth = Convert.ToDateTime(BirthDateMsg);
                model.Gender = Gender;
                model.MaritalStatus = MaritalStatus;
                model.Religion = Religion;
                model.Nationality = Nationality;
                model.NationalId = NationalId;
                model.PassportNumber = PassportNumber;
                model.PrimaryMobile = PrimaryMobile;
                model.SecondaryMobile = SecondaryMobile;
                model.BloodGroup = BloodGroup;
                model.CareerObjective = CareerObjective;
                model.PresentSalary = PresentSalary;
                model.ExpectedSalary = ExpectedSalary;
                model.LookingforJob_Level = LookingforJob_Level;
                model.Availablefor = Availablefor;
                model.CareerSummary = CareerSummary;
                model.SpecialQualification = SpecialQualification;
                model.QualificationKeyword = QualificationKeyword;
                model.PresentAddress = PresentAddress;
                model.PermanentAddress = PermanentAddress;
                model.PrimaryEmail = PrimaryEmail;

                  if (Id > 0)
                {
                    //model.UpdateUser = LoggedInEmployeeId;
                    //model.UpdateDate = DateTime.Now;
                    model = ApplicantMasterService.GetById(Id);
                    model.FirstName = FirstName;
                    model.LastName = LastName;
                    model.FatherName = FatherName;
                    model.MotherName = MotherName;
                    model.GuardianName = GuardianName;
                    model.DateofBirth = Convert.ToDateTime(BirthDateMsg);
                    model.Gender = Gender;
                    model.MaritalStatus = MaritalStatus;
                    model.Religion = Religion;
                    model.Nationality = Nationality;
                    model.NationalId = NationalId;
                    model.PassportNumber = PassportNumber;
                    model.PrimaryMobile = PrimaryMobile;
                    model.SecondaryMobile = SecondaryMobile;
                    model.BloodGroup = BloodGroup;
                    model.CareerObjective = CareerObjective;
                    model.PresentSalary = PresentSalary;
                    model.ExpectedSalary = ExpectedSalary;
                    model.LookingforJob_Level = LookingforJob_Level;
                    model.Availablefor = Availablefor;
                    model.CareerSummary = CareerSummary;
                    model.SpecialQualification = SpecialQualification;
                    model.QualificationKeyword = QualificationKeyword;
                    model.PresentAddress = PresentAddress;
                    model.PermanentAddress = PermanentAddress;
                    model.PrimaryEmail = PrimaryEmail;
                    ApplicantMasterService.Update(model);
                    message = "Updated Successfully";
                }

                else
                {
                    ApplicantMasterService.Create(model);
                    message = "Saved Successfully";
                }

                result = 1;
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SavedApplicantModel(int? AppplicantId)
        {

            var model = new ApplicantMasterViewModel();

            if (AppplicantId > 0)
            {
                int Id = (int)Convert.ToInt64(AppplicantId);
                var _model = ApplicantMasterService.GetById(Id);
                model = Mapper.Map<ApplicantMaster, ApplicantMasterViewModel>(_model);
            }

            var result = model.ID;
            var message = "Saved Successfull";

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, string ID)
        {
            var Result = 0;
            var entity = ApplicantMasterService.GetById(Convert.ToInt32(ID));

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.ImageByte = data;
                ApplicantMasterService.Update(entity);
                Result = 1;
            }
            else
            {
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadCoverLetter(HttpPostedFileBase file, string ID)
        {
            var Result = 0;
            int Id = (int)Convert.ToInt64(ID);
            var entity = ApplicantMasterService.GetById(Id);

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.CoverLetterByte = data;
                ApplicantMasterService.Update(entity);
                Result = 1;
            }
            else
            {
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadAttachedCV(HttpPostedFileBase file, string ID)
        {
            var Result = 0;
            int Id = (int)Convert.ToInt64(ID);
            var entity = ApplicantMasterService.GetById(Id);

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.AttachedCVByte = data;
                ApplicantMasterService.Update(entity);
                Result = 1;
            }
            else
            {
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewIndividualIdCoverletter(int? Id)
        {
            try
            {
                var _model = new ApplicantMaster();

                if (Id > 0)
                {
                    int JCId = (int)Convert.ToInt64(Id);
                    _model = ApplicantMasterService.GetById(JCId);

                    if (_model.CoverLetterByte != null)
                    {

                        string data = Convert.ToBase64String(_model.CoverLetterByte);

                        ViewBag.PDF = string.Format("data:application/pdf;base64,{0}", data);
                    }
                    else
                    {
                        ViewBag.Message = "Do not have details PDF";
                    }

                }
                else
                {
                    ViewBag.Message = "Do not have details PDF";
                }

                return View();
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        public ActionResult ViewIndividualIdAttachedCV(int? Id)
        {
            try
            {
                var _model = new ApplicantMaster();

                if (Id > 0)
                {
                    int JCId = (int)Convert.ToInt64(Id);
                    _model = ApplicantMasterService.GetById(JCId);

                    if (_model.AttachedCVByte != null)
                    {

                        string data = Convert.ToBase64String(_model.AttachedCVByte);

                        ViewBag.PDF = string.Format("data:application/pdf;base64,{0}", data);
                    }
                    else
                    {
                        ViewBag.Message = "Do not have details PDF";
                    }

                }
                else
                {
                    ViewBag.Message = "Do not have details PDF";
                }

                return View();
            }
            catch (Exception ex)
            {

                return View();
            }

        }
        #endregion

        #region Step_02
        public ActionResult Step_02(int? Id)
        {
            var model = new ApplicantAccademicViewModel();


            if (Id > 0)
            {
                int JEId = (int)Convert.ToInt64(Id);
                var _model = ApplicantAccademicService.GetById(JEId);
                model = Mapper.Map<ApplicantAccademic, ApplicantAccademicViewModel>(_model);
                model.YearsofPassingMsg = _model.YearsofPassing.ToString("dd-MMM-yyyy");
            }
            MapDropDownListForAccademicInfo(model);


            return View(model);

        }

        [HttpPost]
        public ActionResult Step_02(ApplicantAccademicViewModel model, FormCollection collection)
        {

            return View();
        }

        private void MapDropDownListForAccademicInfo(ApplicantAccademicViewModel model)
        {

            var ExamList = ExamTitleService.GetMany(p => p.IsActive == true).ToList();
            var viewExamList = ExamList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = (p.Id).ToString()
            }).ToList();
            var ExamTTLList = new List<SelectListItem>();
            ExamTTLList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            ExamTTLList.AddRange(viewExamList);
            model.ExamTitleList = ExamTTLList;

            var LOEList = LevelofEducationService.GetMany(p => p.IsActive == true).ToList();
            var viewLOEList = LOEList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = (p.Id).ToString()
            }).ToList();
            var NewLOEList = new List<SelectListItem>();
            NewLOEList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            NewLOEList.AddRange(viewLOEList);
            model.LevelofEducationList = NewLOEList;


            //var isapproved = new List<SelectListItem>();
            //isapproved.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //isapproved.Add(new SelectListItem { Text = "Approved", Value = "1" });
            //model.IsApprovedList = isapproved;


        }

        public async Task<JsonResult> SaveAccademicInfo(int? Id, int? LevelofEducationId, int? ExamTitleId, string InstituteName, string ResultType, string Group,
            decimal? CGPA, decimal? Scale, string YearsofPassingMsg, string Duration_Years)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new ApplicantAccademic();

                var Mastermodel = new ApplicantMaster();

                Mastermodel.UserId = LoggedInEmployeeId;

                long? UserId = Mastermodel.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }

                if (Mastermodel !=null)
                {
                    model.ApplicantId = Mastermodel.ID;
                model.LevelofEducationId = LevelofEducationId;
                model.ExamTitleId = ExamTitleId;
                model.InstituteName = InstituteName;
                model.ResultType = ResultType;
                model.Group = Group;
                model.CGPA = CGPA;
                model.Scale = Scale;
                model.Duration_Years = Duration_Years;
                model.YearsofPassing = Convert.ToDateTime(YearsofPassingMsg);
                model.IsActive = true;
               
                    if (Id > 0)
                    {
                        int JEId = (int)Convert.ToInt64(Id);
                        model = ApplicantAccademicService.GetById(JEId);
                        model.ApplicantId = Mastermodel.ID;
                        model.LevelofEducationId = LevelofEducationId;
                        model.ExamTitleId = ExamTitleId;
                        model.InstituteName = InstituteName;
                        model.ResultType = ResultType;
                        model.Group = Group;
                        model.CGPA = CGPA;
                        model.Scale = Scale;
                        model.Duration_Years = Duration_Years;
                        model.YearsofPassing = Convert.ToDateTime(YearsofPassingMsg);
                        ApplicantAccademicService.Update(model);
                        message = "Updated Successfully";
                    }

                    else
                    {
                        ApplicantAccademicService.Create(model);
                        message = "Saved Successfully";
                    }
                }
                else
                {
                    message = "Please Save Personal Details First";
                }
                result = 1;
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAccdemicInfo([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<ApplicantAccademicViewModel> List_ViewModel = new List<ApplicantAccademicViewModel>();
                StringBuilder sb = new StringBuilder();

                var Mastermodel = new ApplicantMaster();

                var model = new ApplicantMasterViewModel();
                model.UserId = LoggedInEmployeeId;

                long? Id=0;

                long? UserId = model.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }
                if (Mastermodel != null)
                {
                    Id = Mastermodel.ID;

                    if (Id > 0)
                    {
                        sb.Append(" AND J.ApplicantId=" + Id);
                    }
                    var pram = new { AndCondition = sb.ToString() };
                    var AccademicInfoList = employeeSPService.GetDataWithParameter(pram, "apply.SP_GetAccdemicInfoById");

                    var AccademicInfoListViewList = AccademicInfoList.Tables[0].AsEnumerable()
                    .Select(row => new ApplicantAccademicViewModel()
                    {
                        rowSl = row.Field<string>("rowSl"),
                        ID = row.Field<long>("Id"),
                        LevelofEducation = row.Field<string>("LevelofEducation"),
                        ExamTitle = row.Field<string>("ExamTitle"),
                        InstituteName = row.Field<string>("InstituteName"),
                        ResultType = row.Field<string>("ResultType"),
                        CGPA = row.Field<decimal>("CGPA"),
                        YearsofPassingMsg = row.Field<string>("YearsofPassing"),
                        Duration_Years = row.Field<string>("Duration_Years"),
                        //OperationStartDateMsg = row.Field<string>("OperationStartDate")

                    }).ToList();

                    DataSourceResult result = AccademicInfoListViewList.ToDataSourceResult(request);


                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string Message = "No Data";
                    return Json(new { Result = "ERROR", Message = Message });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteAccademicInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = ApplicantAccademicService.GetById(Id);
                model.IsActive = false;
                //model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //model.UpdateDate = DateTime.UtcNow;
                ApplicantAccademicService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Step_03
        public ActionResult Step_03(int? Id)
        {
            var model = new ApplicantJobExperienceViewModel();

            if (Id > 0)
            {
                int JEId = (int)Convert.ToInt64(Id);
                var _model = ApplicantJobExperienceService.GetById(JEId);
                model = Mapper.Map<ApplicantJobExperience, ApplicantJobExperienceViewModel>(_model);
                model.JobStartDateMsg = _model.JobStartDate.ToString("dd-MMM-yyyy");
                model.JobEndDateMsg = _model.JobEndDate;
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult Step_03(ApplicantJobExperienceViewModel model, FormCollection collection)
        {

            return View();
        }

        public async Task<JsonResult> SaveJobExperienceInfo(int? Id, string CompanyName, string CompanyBusiness, string Designation, string AreaofExperiences,
            string Responsibilities, string CompanyLocation, string JobStartDateMsg, string JobEndDateMsg, string Continuing)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new ApplicantJobExperience();

                var Mastermodel = new ApplicantMaster();

                Mastermodel.UserId = LoggedInEmployeeId;

                long? UserId = Mastermodel.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }

                if (Mastermodel != null)
                {
                    model.ApplicantId = Mastermodel.ID;
                model.CompanyName = CompanyName;
                model.CompanyBusiness = CompanyBusiness;
                model.Designation = Designation;
                model.AreaofExperiences = AreaofExperiences;
                model.Responsibilities = Responsibilities;
                model.CompanyLocation = CompanyLocation;
                model.JobStartDate = Convert.ToDateTime(JobStartDateMsg);
                model.JobEndDate = JobEndDateMsg;
                model.Continuing = Continuing;
                model.IsActive = true;
            
                    if (Id > 0)
                    {
                        int JEId = (int)Convert.ToInt64(Id);
                        model = ApplicantJobExperienceService.GetById(JEId);
                        model.CompanyName = CompanyName;
                        model.CompanyBusiness = CompanyBusiness;
                        model.Designation = Designation;
                        model.AreaofExperiences = AreaofExperiences;
                        model.Responsibilities = Responsibilities;
                        model.CompanyLocation = CompanyLocation;
                        model.JobStartDate = Convert.ToDateTime(JobStartDateMsg);
                        model.JobEndDate = JobEndDateMsg;
                        model.Continuing = Continuing;
                        ApplicantJobExperienceService.Update(model);
                        message = "Updated Successfully";
                    }

                    else
                    {
                        ApplicantJobExperienceService.Create(model);
                        message = "Saved Successfully";
                    }
                }
                else
                {
                    message = "Please Save Personal Details First";
                }

                    result = 1;
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetJobExperienceInfo([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<ApplicantJobExperienceViewModel> List_ViewModel = new List<ApplicantJobExperienceViewModel>();
                StringBuilder sb = new StringBuilder();

                var Mastermodel = new ApplicantMaster();

                var model = new ApplicantMasterViewModel();

                long? Id = 0;
                model.UserId = LoggedInEmployeeId;

                long? UserId = model.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }
                if (Mastermodel != null)
                {
                    Id = Mastermodel.ID;

                    if (Id > 0)
                    {
                        sb.Append(" AND J.ApplicantId=" + Id);
                    }
                    var pram = new { AndCondition = sb.ToString() };
                    var JobExperienceList = employeeSPService.GetDataWithParameter(pram, "apply.SP_GetJobExperienceInfoById");

                    var JobExperienceListViewList = JobExperienceList.Tables[0].AsEnumerable()
                    .Select(row => new ApplicantJobExperienceViewModel()
                    {
                        rowSl = row.Field<string>("rowSl"),
                        Id = row.Field<long>("Id"),
                        CompanyName = row.Field<string>("CompanyName"),
                        Designation = row.Field<string>("Designation"),
                        AreaofExperiences = row.Field<string>("AreaofExperiences"),
                        Responsibilities = row.Field<string>("Responsibilities"),
                    //OperationStartDateMsg = row.Field<string>("OperationStartDate")

                }).ToList();

                    DataSourceResult result = JobExperienceListViewList.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string Message = "No Data";
                    return Json(new { Result = "ERROR", Message = Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteJobExperience(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = ApplicantJobExperienceService.GetById(Id);
                model.IsActive = false;
                //model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //model.UpdateDate = DateTime.UtcNow;
                ApplicantJobExperienceService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Step_04
        public ActionResult Step_04(int? Id)
        {
            var model = new ApplicantTrainingInfoViewModel();


            if (Id > 0)
            {
                int JEId = (int)Convert.ToInt64(Id);
                var _model = ApplicantTrainingInfoService.GetById(JEId);
                model = Mapper.Map<ApplicantTrainingInfo, ApplicantTrainingInfoViewModel>(_model);
                model.TrainingYearMsg = _model.TrainingYear.ToString("dd-MMM-yyyy");
            }



            return View(model);

        }

        [HttpPost]
        public ActionResult Step_04(ApplicantTrainingInfoViewModel model, FormCollection collection)
        {

            return View();
        }

        public async Task<JsonResult> SaveTrainingInfo(int? Id, string TrainingTitle, string TopicsCovered, string TrainingYearMsg, string Institute,
            string Duration)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new ApplicantTrainingInfo();

                var Mastermodel = new ApplicantMaster();

                Mastermodel.UserId = LoggedInEmployeeId;

                long? UserId = Mastermodel.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }

                if (Mastermodel != null)
                {
                 model.ApplicantId = Mastermodel.ID;
                model.TrainingTitle = TrainingTitle;
                model.TopicsCovered = TopicsCovered;
                model.Institute = Institute;
                model.Duration = Duration;
                model.TrainingYear = Convert.ToDateTime(TrainingYearMsg);
                model.IsActive = true;
                    if (Id > 0)
                {
                    int JEId = (int)Convert.ToInt64(Id);
                    model = ApplicantTrainingInfoService.GetById(JEId);
                    model.ApplicantId = Mastermodel.ID;
                    model.TrainingTitle = TrainingTitle;
                    model.TopicsCovered = TopicsCovered;
                    model.Institute = Institute;
                    model.Duration = Duration;
                    model.TrainingYear = Convert.ToDateTime(TrainingYearMsg);

                    ApplicantTrainingInfoService.Update(model);
                    message = "Updated Successfully";
                }

                else
                {
                    ApplicantTrainingInfoService.Create(model);
                    message = "Saved Successfully";
                }
                }
                else
                {
                    message = "Please Save Personal Details First";
                }

                result = 1;
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTrainingInfo([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<ApplicantTrainingInfoViewModel> List_ViewModel = new List<ApplicantTrainingInfoViewModel>();
                StringBuilder sb = new StringBuilder();

                var Mastermodel = new ApplicantMaster();

                var model = new ApplicantMasterViewModel();
                model.UserId = LoggedInEmployeeId;

                long? UserId = model.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }
                if (Mastermodel != null)
                {
                    long? Id = Mastermodel.ID;
                if (Id > 0)
                {
                    sb.Append(" AND J.ApplicantId=" + Id);
                }
                var pram = new { AndCondition = sb.ToString() };
                var TrainingInfoList = employeeSPService.GetDataWithParameter(pram, "apply.SP_GetTrainingInfoById");

                var TrainingInfoViewList = TrainingInfoList.Tables[0].AsEnumerable()
                .Select(row => new ApplicantTrainingInfoViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    Id = row.Field<long>("Id"),
                    TrainingTitle = row.Field<string>("TrainingTitle"),
                    TopicsCovered = row.Field<string>("TopicsCovered"),
                    Institute = row.Field<string>("Institute"),
                    Duration = row.Field<string>("Duration"),
                    TrainingYearMsg = row.Field<string>("TrainingYear")

                }).ToList();

                DataSourceResult result = TrainingInfoViewList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string Message = "No Data";
                    return Json(new { Result = "ERROR", Message = Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteTrainingInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = ApplicantTrainingInfoService.GetById(Id);
                model.IsActive = false;
                //model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //model.UpdateDate = DateTime.UtcNow;
                ApplicantTrainingInfoService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Step_05
        public ActionResult Step_05(int? Id)
        {
            var model = new ApplicantReferenceInfoViewModel();


            if (Id > 0)
            {
                int JEId = (int)Convert.ToInt64(Id);
                var _model = ApplicantReferenceInfoService.GetById(JEId);
                model = Mapper.Map<ApplicantReferenceInfo, ApplicantReferenceInfoViewModel>(_model);

            }
            var Mastermodel = new ApplicantMaster();

            Mastermodel.UserId = LoggedInEmployeeId;

            long? UserId = Mastermodel.UserId;

            if (UserId > 0)
            {
                Mastermodel = ApplicantMasterService.GetByUserId(UserId);

                ViewBag.ApplicantId=(Mastermodel != null)? Mastermodel.ID : 0;
            }
         
            return View(model);

        }

        [HttpPost]
        public ActionResult Step_05(ApplicantReferenceInfoViewModel model, FormCollection collection)
        {

            return View();
        }

        public async Task<JsonResult> SaveApplicantReferenceInfo(int? Id, string Name, string Designation, string Organization, string Email,
            string Relation)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new ApplicantReferenceInfo();

                var Mastermodel = new ApplicantMaster();

                Mastermodel.UserId = LoggedInEmployeeId;

                long? UserId = Mastermodel.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }

                if (Mastermodel != null)
                {
                    model.ApplicantId = Mastermodel.ID;
                model.Name = Name;
                model.Designation = Designation;
                model.Organization = Organization;
                model.Email = Email;
                model.Relation = Relation;
                model.IsActive = true;
            
                    if (Id > 0)
                    {
                        int JEId = (int)Convert.ToInt64(Id);
                        model = ApplicantReferenceInfoService.GetById(JEId);
                        model.Name = Name;
                        model.Designation = Designation;
                        model.Organization = Organization;
                        model.Email = Email;
                        model.Relation = Relation;

                        ApplicantReferenceInfoService.Update(model);
                        message = "Updated Successfully";
                    }

                    else
                    {
                        ApplicantReferenceInfoService.Create(model);
                        message = "Saved Successfully";
                    }
                }
                else
                {
                    message = "Please Save Personal Details First";
                }
                result = 1;
                }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReferenceInfoById([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<ApplicantReferenceInfoViewModel> List_ViewModel = new List<ApplicantReferenceInfoViewModel>();
                StringBuilder sb = new StringBuilder();

                var Mastermodel = new ApplicantMaster();

                var model = new ApplicantMasterViewModel();
                model.UserId = LoggedInEmployeeId;

                long? UserId = model.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }
                if (Mastermodel != null)
                {
                    long? Id = Mastermodel.ID;
                if (Id > 0)
                {
                    sb.Append(" AND J.ApplicantId=" + Id);
                }
                var pram = new { AndCondition = sb.ToString() };
                var ReferenceInfoList = employeeSPService.GetDataWithParameter(pram, "apply.SP_GetApplicantReferenceInfoById");

                var ReferenceInfoListViewList = ReferenceInfoList.Tables[0].AsEnumerable()
                .Select(row => new ApplicantReferenceInfoViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    Id = row.Field<long>("Id"),
                    Name = row.Field<string>("Name"),
                    Designation = row.Field<string>("Designation"),
                    Organization = row.Field<string>("Organization"),
                    Email = row.Field<string>("Email"),
                    Relation = row.Field<string>("Relation")

                }).ToList();

                DataSourceResult result = ReferenceInfoListViewList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
                else
            {
                string Message = "No Data";
                return Json(new { Result = "ERROR", Message = Message });
            }
        }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteReferenceInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = ApplicantReferenceInfoService.GetById(Id);
                model.IsActive = false;
                //model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //model.UpdateDate = DateTime.UtcNow;
                ApplicantReferenceInfoService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ViewProfile

        public ActionResult ViewProfile(int? Id)
        {
            var model = new CompleteProfileViewModel();

            try
            {
                if (Id > 0)
                {
                    int MId = (int)Convert.ToInt64(Id);
                    List<ApplicantMaster> a = new List<ApplicantMaster>();
                    a.Add(ApplicantMasterService.GetById(MId));
                    model.ApplicantMaster = a;


                    var MasterModel = new ApplicantMaster();
                    MasterModel = ApplicantMasterService.GetById(MId);

                    if (MasterModel.ImageByte != null)
                    {
                        string data = Convert.ToBase64String(MasterModel.ImageByte);
                        ViewBag.Image = string.Format("data:image/png;base64,{0}", data);

                        //ViewBag.Image = string.Format("data:application/pdf;base64,{0}", data);
                    }
                        ViewBag.ID = MasterModel.ID;                   
                    ViewBag.FullName = String.Concat(MasterModel.FirstName, MasterModel.LastName);
                    ViewBag.Presentaddress = String.Concat("Address: :", MasterModel.PresentAddress);
                    ViewBag.PrimaryMobile = String.Concat("Mobile: ", MasterModel.PrimaryMobile);
                    ViewBag.PrimaryEmail = String.Concat("Email: ", MasterModel.PrimaryEmail);
                    ViewBag.CareerObjective = MasterModel.CareerObjective;
                    ViewBag.CareerSummary = MasterModel.CareerSummary;
                    ViewBag.SpecialQualification = MasterModel.SpecialQualification;

                    var pram = new { Id = Id };

                    var DetailsList = employeeSPService.GetDataWithParameter(pram, "dbo.SP_GetApplicantInformationDataById");

                    model.ApplicantJobExperienceViewModel = new List<ApplicantJobExperienceViewModel>();


                    var JobExperienceListViewList = DetailsList.Tables[1].AsEnumerable()
                    .Select(row => new ApplicantJobExperienceViewModel()
                    {

                        CompanyName = row.Field<string>("CompanyName"),
                        Designation = row.Field<string>("Designation"),
                        WorkingPeriod = row.Field<string>("JobStartDate"),
                        Responsibilities = row.Field<string>("Responsibilities"),
                        CompanyLocation = row.Field<string>("CompanyLocation"),
                    //OperationStartDateMsg = row.Field<string>("OperationStartDate")

                }).ToList();

                    model.ApplicantJobExperienceViewModel = JobExperienceListViewList;

                    model.ApplicantAccademicViewModel = new List<ApplicantAccademicViewModel>();

                    var AccademicInfoListViewList = DetailsList.Tables[2].AsEnumerable()
              .Select(row => new ApplicantAccademicViewModel()
              {


                  ExamTitle = row.Field<string>("ExamName"),
                  InstituteName = row.Field<string>("InstituteName"),

                  CGPA = row.Field<decimal>("CGPA"),
                  Scale = row.Field<decimal?>("Scale"),
                  YearsofPassingMsg = row.Field<string>("PassingYear"),
              //OperationStartDateMsg = row.Field<string>("OperationStartDate")

          }).ToList();

                    model.ApplicantAccademicViewModel = AccademicInfoListViewList;

                    model.ApplicantTrainingInfoViewModel = new List<ApplicantTrainingInfoViewModel>();

                    var TrainingInfoViewList = DetailsList.Tables[3].AsEnumerable()
            .Select(row => new ApplicantTrainingInfoViewModel()
            {

                TrainingTitle = row.Field<string>("TrainingTitle"),
                TopicsCovered = row.Field<string>("TopicsCovered"),
                Institute = row.Field<string>("Institute"),
                TrainingYearMsg = row.Field<string>("TrainingYear")

            }).ToList();

                    model.ApplicantTrainingInfoViewModel = TrainingInfoViewList;

                    model.ApplicantReferenceInfoViewModel = new List<ApplicantReferenceInfoViewModel>();


                    var ReferenceInfoListViewList = DetailsList.Tables[4].AsEnumerable()
                    .Select(row => new ApplicantReferenceInfoViewModel()
                    {

                        Name = row.Field<string>("RName"),
                        Designation = row.Field<string>("Designation"),
                        Organization = row.Field<string>("Organization"),
                        Email = row.Field<string>("Email"),
                        Relation = row.Field<string>("Relation")

                    }).ToList();

                    model.ApplicantReferenceInfoViewModel = ReferenceInfoListViewList;

                }
                ViewBag.Message = "";

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Complete your full Profile";
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult ViewProfile(CompleteProfileViewModel model, FormCollection collection)
        {

            return View();
        }
        #endregion

        #region JobApplied


        public ActionResult ApplyJobs()
        {
            
            var model = new AppliedPostViewModel();
            try
            {
                var modelMaster = new ApplicantMasterViewModel();

                modelMaster.UserId = LoggedInEmployeeId;

                long? UserId = modelMaster.UserId;
                if (UserId > 0)
                {
                    var _model = ApplicantMasterService.GetByUserId(UserId);

                    if (_model != null)
                    {
                        var filterList = new List<AppliedPostViewModel>();

                        using (var db = new gHRMDBContext())
                        {
                            var ApplicantId = _model.ID;

                            var sqlCommand = $@"[apply].[SP_GetAppliedInfoById]
                                '{ApplicantId}'
                                ";

                            filterList = db.Database.SqlQuery<AppliedPostViewModel>(sqlCommand)
                                .AsParallel().ToList();
                        }

                        model.AppliedPostList = filterList;


                    }

                    else
                    {
                        ViewBag.CvNotCompleted = 1;
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Complete your full Profile";
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult ApplyJobs(AppliedPostViewModel model, FormCollection collection)
        {

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SubmitApplication(int? JobId, List<QuestionAnsweredByApplicant> QAnswerList)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new AppliedPost();

                var QAnswerModel = new QuestionAnsweredByApplicant();

                var Mastermodel = new ApplicantMaster();

                Mastermodel.UserId = LoggedInEmployeeId;

                long? UserId = Mastermodel.UserId;

                if (UserId > 0)
                {
                    Mastermodel = ApplicantMasterService.GetByUserId(UserId);
                }

                if (Mastermodel != null)
                {
                    if (JobId > 0)
                    {

                        if(QAnswerList != null)
                        {
                            foreach(QuestionAnsweredByApplicant item in QAnswerList)
                            {
                                QAnswerModel.ApplicantId= Mastermodel.ID;
                                QAnswerModel.QId = item.QId;
                                QAnswerModel.QAnswer = item.QAnswer;
                                QAnswerModel.IsActive = true;

                                QuestionAnsweredByApplicantService.Create(QAnswerModel);

                            }
                        }
                        model.ApplicantId = Mastermodel.ID;
                        model.JobId = JobId;
                        model.IsActive = true;
                        model.AlreadyApplied = 1;

                        AppliedPostService.Create(model);
                        message = "You have Successfully Submitted your Application";
                        
                    }
                    else
                    {               
                        message = "Error; JobId";
                    }
                }
                else
                {
                    message = "Please Save Personal Details First";
                }
                result = 1;
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewPdfIndividualJob(int? JobId)
        {
            try
            {
                var _model = new JobsCircular();

                if (JobId > 0)
                {
                    int JCId = (int)Convert.ToInt64(JobId);
                    _model = JobsCircularService.GetById(JCId);

                    if (_model.PdfByte != null)
                    {

                        string data = Convert.ToBase64String(_model.PdfByte);

                        ViewBag.PDF = string.Format("data:application/pdf;base64,{0}", data);
                    }
                    else
                    {
                        ViewBag.Message = "Do not have details PDF";
                    }

                }
                else
                {
                    ViewBag.Message ="Do not have details PDF";
                }

                return View();
            }
            catch (Exception ex)
            {
                
                return View();
            }

        }

        #endregion

        #region JobsCircular

        public ActionResult JobsCircular(int? Id)
        {
            var model = new JobsCircularViewModel();

            if (Id > 0)
            {
                int JCId = (int)Convert.ToInt64(Id);
                var _model = JobsCircularService.GetById(JCId);
                model = Mapper.Map<JobsCircular, JobsCircularViewModel>(_model);
                if (_model.PdfByte != null)
                {
                    ViewBag.IsPDF = 1;
                }

            }

            return View(model);

        }

        [HttpPost]
        public ActionResult JobsCircular(ApplicantReferenceInfoViewModel model, FormCollection collection)
        {

            return View();
        }

        public async Task<JsonResult> SaveJobsCircular(int?JobId, string PostName, string PostDescription)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new JobsCircular();

                var Mastermodel = new ApplicantMaster();

                    model.PostName = PostName;
                    model.PostDescription = PostDescription;
                    model.CreatedBy = LoggedInEmployeeId;
                    model.IsActive = true;

                    if (JobId > 0)
                    {
                    int Id = (int)Convert.ToInt64(JobId);
                    model = JobsCircularService.GetById(Id);
                    model.PostName = PostName;
                    model.PostDescription = PostDescription;

                 
                    result = (int)model.JobId;
                    message = "Updated Successfully";
                    }

                    else
                    {
                    model=JobsCircularService.Create(model);
                    result = (int)model.JobId;
                    message = "Saved Successfully";
                    }
                
            }

            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult UploadPdf(HttpPostedFileBase file, string ID)
        {
            var Result = 0;
            int Id = (int)Convert.ToInt64(ID);
            var entity = JobsCircularService.GetById(Id);      

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.PdfByte = data;
                JobsCircularService.Update(entity);
                Result = 1;
            }
            else
            {
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetJobsCircularByCreatedId([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<JobsCircularViewModel> List_ViewModel = new List<JobsCircularViewModel>();
                StringBuilder sb = new StringBuilder();

                var Mastermodel = new ApplicantMaster();

                var model = new JobsCircular();

                long? UserId = LoggedInEmployeeId;

                if (UserId > 0)
                {
                    model = JobsCircularService.GetByCreatedBy(UserId);
                }
                if (model != null)
                {
                    long? Id = model.CreatedBy;
                    if (Id > 0)
                    {
                        sb.Append(" AND J.CreatedBy=" + Id);
                    }
                    var pram = new { AndCondition = sb.ToString() };
                    var JobsCircularList = employeeSPService.GetDataWithParameter(pram, "apply.SP_GetJobsCircularByCreatedById");

                    var JobsCircularViewList = JobsCircularList.Tables[0].AsEnumerable()
                    .Select(row => new JobsCircularViewModel()
                    {
                        rowSl = row.Field<string>("rowSl"),
                        JobId = row.Field<long>("JobId"),
                        PostName = row.Field<string>("PostName"),
                        PostDescription = row.Field<string>("PostDescription"),
                     

                    }).ToList();

                    DataSourceResult result = JobsCircularViewList.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string Message = "No Data";
                    return Json(new { Result = "ERROR", Message = Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult InformationDeleteJobCircularInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model =JobsCircularService.GetById(Id);
                model.IsActive = false;
                //model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //model.UpdateDate = DateTime.UtcNow;
                JobsCircularService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult Test()
        {
            
            return View();

        }

    }
}



