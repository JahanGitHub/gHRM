using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;

namespace gHRM.Web.Controllers
{
    public class FeedbackCategoryController : BaseController
    {
        #region Variables

        private readonly IFeedbackCategoryService feedbackCategoryService;
        private readonly IEmployeeSPService employeeSPService;

        public FeedbackCategoryController(IFeedbackCategoryService feedbackCategoryService, IEmployeeSPService employeeSPService)
        {
            this.feedbackCategoryService = feedbackCategoryService;
            this.employeeSPService = employeeSPService;
        
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        public ActionResult CreateCategory()
        {
            return View();
        }
        public JsonResult CreateNCategory(string FeedbackCategoryName, string FeedbackCategoryType)
        {
            string result = "OK";
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime CreateDate = DateTime.Now;
                //SP_FeedBackCatCreate](@FeedbackCategoryName nvarchar(500),@FeedbackCategoryType nvarchar(100), @CreateUser varchar(100), @CreateDate datetime)
                var param = new { FeedbackCategoryName = FeedbackCategoryName,FeedbackCategoryType = FeedbackCategoryType, CreateUser = CreateUser, CreateDate = CreateDate };
                var val = employeeSPService.GetDataWithParameter(param, "SP_FeedBackCatCreate");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateFeedBackCat(string FeedbackCategoryName, string FeedbackCategoryType, int FeedbackCategoryID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_FeedBackCaTUpdate](@FeedbackCategoryName nvarchar(500),@FeedbackCategoryType nvarchar(100), @FeedbackCategoryID int, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new { FeedbackCategoryName = FeedbackCategoryName, FeedbackCategoryType = FeedbackCategoryType, FeedbackCategoryID = FeedbackCategoryID, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = employeeSPService.GetDataWithParameter(param, "SP_FeedBackCaTUpdate");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // 
        public JsonResult DeleteFeedBack(int FeedbackCategoryID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //.[SP_DeleteFeedBackCat](@FeedbackCategoryID int, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new { FeedbackCategoryID = FeedbackCategoryID, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = employeeSPService.GetDataWithParameter(param, "SP_DeleteFeedBackCat");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // Show List
        public JsonResult GetFeedBackList(string Id, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string Ids = Convert.ToString(Id);

                if (Id != null) //"0"
                    sb.Append(" AND FBC.FeedbackCategoryID =" + Ids);

                List<FeedbackCategoryViewModel> List_ViewModel = new List<FeedbackCategoryViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_FeedBackCat_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedbackCategoryViewModel
                {
                    FeedbackCategoryID = row.Field<int>("FeedbackCategoryID"),
                    FeedbackCategoryName = row.Field<string>("FeedbackCategoryName"),
                    FeedbackCategoryType = row.Field<string>("FeedbackCategoryType")

                }).ToList();

                if (Id != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function





        /////
        ///Uodate Feedback Register :: :: :: 
        ///
        //FeedbackRegisterViewModel


        public ActionResult UpdateFeedbackRegister()
        {
            return View();
        }




        // Show List
        public JsonResult GetFeedBackRegisterList(string Id, string IsChecked, string IsSolved,  int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

               // string Ids = Convert.ToString(Id);

                if (Id != null)
                {  
                    if (Id != "")
                        sb.Append(" AND FdR.FeedbackRegisterID = " + Id);
                }

                if (IsChecked != null)
                { //"0"
                    if (IsChecked != "")
                    {
                        if (IsChecked == "1")
                        {
                            sb.Append(" AND FdR.IsChecked = 1 ");
                        }
                        else
                        {
                            sb.Append(" AND FdR.IsChecked = 0 ");
                        }
                    }
                }

                if (IsSolved != null)
                { //
                    if (IsSolved != "") 
                    {
                        if (IsSolved == "1")
                        {
                            sb.Append(" AND FdR.IsSolved = 1 ");
                        }
                        else 
                        {
                            sb.Append(" AND FdR.IsSolved = 0 ");
                        }
                    }
                }


                 List<FeedbackRegisterViewModel> List_ViewModel = new List<FeedbackRegisterViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_FeedBackReg_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedbackRegisterViewModel
                {
                    FeedbackRegisterID = row.Field<long>("FeedbackRegisterID"),
                    EmployeeId = row.Field<long?>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    FeedbackDateSTR = row.Field<string>("FeedbackDate"),
                    FeedbackCategoryName = row.Field<string>("FeedbackCategoryName"),
                    FeedbackDescription = row.Field<string>("FeedbackDescription"),
                    ChkStatus = row.Field<string>("IsChecked"),
                    SolvedStatus = row.Field<string>("IsSolved"),
                    SolvedBy = row.Field<string>("SolvedBy"),
                    Remarks = row.Field<string>("Remarks"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),

                }).ToList();

                if (Id != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        //
        public JsonResult UpdateFeedBackReg(string FeedbackRegisterID, string IsChecked, string IsSolved, string SolvedBy, string SolvedDate )
        {
            string result = "OK";
            try
            {

                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_FeedBackRegUpdate](@FeedbackCategoryID int,@IsChecked bit ,@IsSolved bit, @SolvedBy varchar(100), @SolvedDate datetime, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new { FeedbackRegisterID = FeedbackRegisterID, IsChecked = IsChecked, IsSolved = IsSolved, SolvedBy = SolvedBy, SolvedDate = SolvedDate, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = employeeSPService.GetDataWithParameter(param, "SP_FeedBackRegUpdate");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateFeedBackReg_GC()
        {
            string result = "OK";
            try
            {

                string FeedbackRegisterID = Request.Form["FeedbackRegisterID"].ToString();
                string IsChecked = Request.Form["IsChecked"].ToString();
                string IsSolved = Request.Form["IsSolved"].ToString();
                string SolvedBy = Request.Form["SolvedBy"].ToString();
                string SolvedDate = Request.Form["SolvedDate"].ToString();
                string Remarks = Request.Form["Remarks"].ToString();

                DateTime dt = DateTime.Now;
                string uploadDay = dt.Day + "-" + dt.Month + "-" + dt.Year;
                uploadDay = "FeedBack_" + uploadDay;
                string fname;
                var path = "";
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  

                            HttpPostedFileBase file = files[i];
                          

                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            // Get the complete folder path and store the file inside it.
                            // 
                            var fileName = Path.GetFileName(fname);
                            var fileType = Path.GetFileName(file.ContentType);

                            //var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);//E:\Project\UploadedFile
                             path = Path.Combine(@"E:\IIS\ghrm\GC\UploadFeedBackAttachment\Reply", uploadDay + fileName);

                            //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                            file.SaveAs(path);
                        }
                        // Returns message that successfully uploaded  
                       // return Json("File Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                   // return Json("No files selected.");
                }


                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_FeedBackRegUpdate](@FeedbackCategoryID int,@IsChecked bit ,@IsSolved bit, @SolvedBy varchar(100), @SolvedDate datetime, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new { FeedbackRegisterID = FeedbackRegisterID, IsChecked = IsChecked, IsSolved = IsSolved, SolvedBy = SolvedBy, SolvedDate = SolvedDate, UpdateUser = UpdateUser, UpdateDate = UpdateDate, FilePath = path, Remarks = Remarks };
                var val = employeeSPService.GetDataWithParameter(param, "SP_FeedBackRegUpdate");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
