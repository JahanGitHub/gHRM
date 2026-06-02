//using gHRM.Service;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//Created by Mansur 15-11-2016 with Morshed Bhai
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Models;
using gHRM.Web.ViewModels.FeedBack;
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
using gHRM.Web.Helpers;
using gHRM.Service.ReportServies;
using gHRM.Service.StoreProcedure;
using System.Text;
using gHRM.Web.ViewModels.FeedBack;

namespace gHRM.Web.Controllers
{
    public class FeedbackRegisterController : BaseController
    {

        #region Variables

        private readonly IFeedbackCategoryService feedbackCategoryService;
        private readonly IFeedbackRegisterService feedbackRegisterService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;

        public FeedbackRegisterController(IFeedbackCategoryService feedbackCategoryService, IFeedbackRegisterService feedbackRegisterService, IEmployeeSPService employeeSPService, IEmployeeService employeeService, IOfficeService officeService)
        {
            this.feedbackCategoryService = feedbackCategoryService;
            this.feedbackRegisterService = feedbackRegisterService;
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.officeService = officeService;
        }

        #endregion

        #region Methods

        public JsonResult GetEmployeeData(string EmpId)
        {
            List<FeedbackRegisterViewModel> List_Employee = new List<FeedbackRegisterViewModel>();
            var param = new { EmpId = EmpId };
            var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_EmpData");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FeedbackRegisterViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   OfficeId = row.Field<int>("OfficeId"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FeedbackRegisterSolvedStatusWise(int jtStartIndex, int jtPageSize, string jtSorting, string QType, string SolvedStatus)
        {

            List<FeedbackRegisterViewModel> List_FeedbackRegisterSolvedStatusWise = new List<FeedbackRegisterViewModel>();
            var param = new { qType = QType, solvedStatus = SolvedStatus };
            var visitor = employeeSPService.GetDataWithParameter(param, "SP_GET_FeedbackRegister");
            List_FeedbackRegisterSolvedStatusWise = visitor.Tables[0].AsEnumerable()
                  .Select(row => new FeedbackRegisterViewModel
                  {

                      FeedbackRegisterID = row.Field<long>("FeedbackRegisterID"),
                      OfficeName = row.Field<string>("OfficeName"),
                      EmployeeName = row.Field<string>("EmployeeName"),
                      FeedbackCategoryName = row.Field<string>("FeedbackCategoryName"),
                      FeedbackDescription = row.Field<string>("FeedbackDescription"),
                      FeedbackDateMsg = row.Field<string>("FeedbackDateMsg"),
                      SolvedStatus = row.Field<string>("SolvedStatus"),
                      ChkStatus = row.Field<string>("ChkStatus"),
                      FileLocation = row.Field<string>("FileLocation"),
                      EmployeeCode = row.Field<string>("EmployeeCode"),

                  }).ToList();
            var currentPageRecords = List_FeedbackRegisterSolvedStatusWise.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_FeedbackRegisterSolvedStatusWise.LongCount(), JsonRequestBehavior.AllowGet });
        }

        private void MapDropDownList(FeedbackRegisterViewModel model)
        {

            //Feedback Category List
            var feedbackCategoryList = feedbackCategoryService.GetAll();
            var viewfeedbackCategoryList = feedbackCategoryList.Select(m => new SelectListItem() { Text = m.FeedbackCategoryName, Value = m.FeedbackCategoryID.ToString() });
            var feedbackCategory_items = new List<SelectListItem>();
            feedbackCategory_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            feedbackCategory_items.AddRange(viewfeedbackCategoryList);
            model.FeedbackCategoryList = feedbackCategory_items;

            var UnitList = officeService.GetAll().Where(t=>t.OfficeTypeId==6&&t.IsActive==true);
            var viewUnitList = UnitList.Select(m => new SelectListItem() { Text = m.OfficeName, Value = m.OfficeId.ToString() });
            var UnitList_items = new List<SelectListItem>();
            UnitList_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            UnitList_items.AddRange(viewUnitList);
            model.UnitList = UnitList_items;
        }

        #endregion

        #region Events
        // GET: FeedbackRegister
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["FeedbackCategoryList"] = items;

            return View();

        }


        public ActionResult UpdateFeedbackRegister()
        {
            return View();
        }

        public ActionResult Print(string Id)
        {
            try
            {
                var companyID = (int)LoggedInOfficeID;
                var param = new { Id = Id };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "SP_Get_FeedbackDetails");
                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintReport("Rpt_Feedback.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        // GET: FeedbackRegister/Details/5
        public ActionResult Details(int id)
        {
            var model = new FeedbackRegisterViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // GET: FeedbackRegister/Create
        public ActionResult Create()
        {
            //ViewData["OfficeId"] = items;

            FeedbackRegisterViewModel model = new FeedbackRegisterViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // POST: FeedbackRegister/Create
        [HttpPost]
        public ActionResult Create(FeedbackRegisterViewModel model)
        {
            model.IsActive = true;
            var entity = Mapper.Map<FeedbackRegisterViewModel, FeedbackRegister>(model);
            try
            {
                // File attachment
                DateTime dt = DateTime.Now;
                string uploadDay = dt.Day + "-" + dt.Month + "-" + dt.Year;
                uploadDay = "FeedBack_" + uploadDay;
                if (model.File_AttachmentU != null)
                {
                    var fileName = Path.GetFileName(model.File_AttachmentU.FileName);
                    var fileType = Path.GetFileName(model.File_AttachmentU.ContentType);

                    //var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);//E:\Project\UploadedFile
                    var path = Path.Combine(@"E:\IIS\ghrm\GC\UploadFeedBackAttachment\Create", uploadDay + fileName);

                    //file.SaveAs(path);
                    model.File_AttachmentU.SaveAs(path);

                    entity.FileLocation = path;
                }

                //var officeIID = LoggedInOfficeID;
                ////InsertFeedBackRegister(@OfficeId int, @EmployeeId bigint , @FeedbackCategoryID int, @FeedbackDescription varchar(500) , @FeedbackDate Date , @FileLocation varchar(500) )
                //var param = new { @OfficeId = officeIID, @EmployeeId = entity.EmployeeId, @FeedbackCategoryID = entity.FeedbackRegisterID, @FeedbackDescription = entity.FeedbackDescription, @FeedbackDate = entity.FeedbackDate, @FileLocation = entity.FileLocation };
                //var OverdueMls = employeeSPService.GetDataWithParameter(param, "InsertFeedBackRegister");





                feedbackRegisterService.Create(entity);

                TempData["Success"] = "Success message text.";
                return RedirectToAction("Create");

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error message text.";
                return RedirectToAction("Create");
                //return GetErrorMessageResult(ex);
            }
        }

        public ActionResult DownloadFile(string FileUploadId)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");



            var getFeedbackRegisterDetails = feedbackRegisterService.GetById(Convert.ToInt32(FileUploadId));



            var location = getFeedbackRegisterDetails.FileLocation;// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;
            var param = new { OfficeID = OfficeId, FileId = FileUploadId };
            // var empList = employeeSPService.GetDataWithParameter(param, "SP_UpdateDownloadTimes");
            ///

            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }

        public ActionResult DownloadFile2(string FileUploadId)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");



            var getFeedbackRegisterDetails = feedbackRegisterService.GetById(Convert.ToInt32(FileUploadId));



            var location = getFeedbackRegisterDetails.FileLocationReply;// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;
            var param = new { OfficeID = OfficeId, FileId = FileUploadId };
            // var empList = employeeSPService.GetDataWithParameter(param, "SP_UpdateDownloadTimes");
            ///

            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }

        // GET: FeedbackRegister/Edit/5
        public ActionResult Edit(int id)
        {
            var feedbackRegister = feedbackRegisterService.GetById(Convert.ToInt32(id));

            var ExistEmp = employeeService.GetByEmpId(feedbackRegister.EmployeeId);
            //feedbackRegister.EmployeeCode = feedbackRegister.EmployeeId;// ExistEmp.EmployeeId;
            //feedbackRegister.EmployeeName  = ExistEmp.EmployeeName;
            var entity = Mapper.Map<FeedbackRegister, FeedbackRegisterViewModel>(feedbackRegister);
            entity.EmployeeCode = Convert.ToString(ExistEmp.EmployeeCode);// ExistEmp.EmployeeId;
            entity.EmployeeId = ExistEmp.EmployeeId;// ExistEmp.EmployeeId;
            entity.EmployeeName = ExistEmp.EmployeeName;
            MapDropDownList(entity);

            return View(entity);
        }

        // POST: FeedbackRegister/Edit/5
        [HttpPost]
        public ActionResult Edit(FeedbackRegisterViewModel model)
        {
            try
            {
                var entity = Mapper.Map<FeedbackRegisterViewModel, FeedbackRegister>(model);
                var getFeedbackRegisterDetails = feedbackRegisterService.GetById(Convert.ToInt32(entity.FeedbackRegisterID));

                if (getFeedbackRegisterDetails.IsChecked == true)
                {
                    Exception ex = new Exception("This is already Checked");
                    return GetErrorMessageResult(ex);
                }


                getFeedbackRegisterDetails.EmployeeId = entity.EmployeeId;
                getFeedbackRegisterDetails.FeedbackCategoryID = entity.FeedbackCategoryID;
                getFeedbackRegisterDetails.FeedbackDescription = entity.FeedbackDescription;
                getFeedbackRegisterDetails.FeedbackDate = entity.FeedbackDate;



                feedbackRegisterService.Update(getFeedbackRegisterDetails);

                // return GetSuccessMessageResult();
                //  return RedirectToAction("Index");

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: FeedbackRegister/Delete/5
        public ActionResult Delete(int id)
        {
            string Result = "OK";

            var getFeedbackRegisterDetails = feedbackRegisterService.GetById(Convert.ToInt32(id));

            getFeedbackRegisterDetails.IsActive = false;
            getFeedbackRegisterDetails.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            getFeedbackRegisterDetails.UpdateDate = DateTime.Now;
            feedbackRegisterService.Update(getFeedbackRegisterDetails);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // POST: FeedbackRegister/Delete/5
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


        public ActionResult PayrollFeedback()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;


            // FOR Office DropDown
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;

            return View();
        }

        public ActionResult PayrollFeedbackReview()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;

            // FOR Office DropDown
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;

            var model = new FeedbackRegisterViewModel();

            var solvedByList = new List<SelectListItem>();
            solvedByList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            solvedByList.Add(new SelectListItem() { Text = "Md.Babul", Value = "Md.Babul" });
            solvedByList.Add(new SelectListItem() { Text = "Md.Sayeed", Value = "Md.Sayeed" });
            model.SolvedByList = solvedByList;

            var solvedUnsolvedList = new List<SelectListItem>();
            solvedUnsolvedList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            solvedUnsolvedList.Add(new SelectListItem() { Text = "All", Value = "All" });
            solvedUnsolvedList.Add(new SelectListItem() { Text = "Solved", Value = "Solved" });
            solvedUnsolvedList.Add(new SelectListItem() { Text = "Unsolved", Value = "Unsolved" });
            model.SolvedUnsolvedList = solvedUnsolvedList;

            

            return View(model);
        }


        public JsonResult UpdateSavePayrollFeedbackRegister(
             string selectedOfficeId
           , string EmployeeId
           , string PayRollFeedBackRegId
           , string ContactMobileNo
           , string ItemCode
           , string DisburseDate
           , string ProblemDetails
           , string CorrectionDetails
           , string FeedbackDescription
           , string SolvedBy
           , string SolvedDate

           , int ActualLoandisbursementAmount
           , int WebLoanCollectionAmount
           , int ActualCollectionDesktop
           , decimal WebInterestCharge
           , int WebInterestColleciton
           , decimal DesktopInterestCharge
           , int DesktopInterestCollection


       ) // End of Parameter
        {
            string result = "OK";
            try
            {
                var param = new
                {

                    selectedOfficeId = selectedOfficeId,
                    EmployeeId = EmployeeId,
                    PayRollFeedBackRegId = PayRollFeedBackRegId,
                    ContactMobileNo = ContactMobileNo,
                    ItemCode = ItemCode,
                    DisburseDate = DisburseDate,
                    ProblemDetails = ProblemDetails,
                    CorrectionDetails = CorrectionDetails,
                    FeedbackDescription = FeedbackDescription,
                    SolvedBy = SolvedBy,
                    SolvedDate = SolvedDate,
                    CreatedBy = LoggedInOfficeID,
                    ActualLoandisbursementAmount = ActualLoandisbursementAmount,
                    WebLoanCollectionAmount = WebLoanCollectionAmount,
                    ActualCollectionDesktop = ActualCollectionDesktop,
                    WebInterestCharge = WebInterestCharge,
                    WebInterestColleciton = WebInterestColleciton,
                    DesktopInterestCharge = DesktopInterestCharge,
                    DesktopInterestCollection = DesktopInterestCollection

                };
                var val = employeeSPService.GetDataWithParameter(param, "SP_PR_FeedBackRegister");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }// End of Update Salary Register


        

        public JsonResult GetPayrollFeedbackList(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterColumn, string filterValue, string EmployeeID, string selectedOfficeId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (EmployeeID == null || EmployeeID == "")
                {
                    EmployeeID = "";
                }
                else
                {
                    sb.Append("AND fb.EmployeeId = " + EmployeeID);
                }

                if (selectedOfficeId == null || selectedOfficeId == "")
                {

                }
                else
                {
                    var officeInfo = officeService.GetById(Convert.ToInt32(selectedOfficeId));

                    if (officeInfo.OfficeTypeId == 1)
                    { // Head Office 

                    }
                    else
                    {
                        sb.Append("AND fb.OfficeId = " + selectedOfficeId);
                    }
                
                }
                 
                int OfficeId = (int)LoggedInOfficeID;
                int? OfficeTypeId = (int)LoggedInOfficeType;
                //if (selectedOfficeId != null)
                //{
                //    if (selectedOfficeId != "0")
                //    {

                //        OfficeId = Convert.ToInt32(selectedOfficeId);  //(int)LoggedInOfficeID; //Modified as Per Requirement

                //        var officeInfo = officeService.GetById(OfficeId);


                //        OfficeTypeId = officeInfo.OfficeTypeId;  //(int)LoggedInOfficeType;
                //    }

                //}

                List<FeedbackRegisterViewModel> List_InvMasterViewModel = new List<FeedbackRegisterViewModel>();
                var param = new { @AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "SP_PR_FeedbackRegisterList");

                List_InvMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedbackRegisterViewModel
                {
                    OfficeId = row.Field<int>("OfficeId"),
                    EmployeeId = row.Field<long>("EmployeeID"),
                    ProblemDetails = row.Field<string>("ProblemDetails"),
                    EntryDate = row.Field<string>("EntryDate"),
                    PayRollFeedBackRegId = row.Field<int>("PayRollFeedBackRegId"),
                    rowSl = row.Field<long>("rowSl"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    FeedbackDescription = row.Field<string>("FeedbackDescription"),
                    CorrectionDetails = row.Field<string>("CorrectionDetails"),
                    ItemCode = row.Field<string>("ItemCode"),
                    DisburseDate = row.Field<string>("DisburseDate"),
                    ContactMobileNo = row.Field<string>("ContactMobileNo"),
                    SolvedBy = row.Field<string>("SolvedBy"),
                    SolvedDate = row.Field<string>("SolvedDate"),
                    OfficeCode = row.Field<string>("OfficeCode"),

                    ActualLoandisbursementAmount = row.Field<int?>("ActualLoandisbursementAmount"),
                    WebLoanCollectionAmount = row.Field<int?>("WebLoanCollectionAmount"),
                    ActualCollectionDesktop = row.Field<int?>("ActualCollectionDesktop"),
                    WebInterestCharge = row.Field<decimal?>("WebInterestCharge"),
                    WebInterestColleciton = row.Field<int?>("WebInterestColleciton"),
                    DesktopInterestCharge = row.Field<decimal?>("DesktopInterestCharge"),
                    DesktopInterestCollection = row.Field<int?>("DesktopInterestCollection"),

                }).ToList();

                var currentPageRecords = List_InvMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_InvMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function





        #endregion
    }
}
