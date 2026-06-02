#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Drawing;
using AutoMapper;
using gHRM.Web.ViewModels;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.Extensions;
using gHRM.Web.CommonDropdown;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.Helpers;
using System.Drawing.Imaging;
#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeSignatureController : BaseController
    {
        #region Private Variables
        private readonly IEmployeeService employeeService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeDocumentService employeeDocumentService;

        #endregion

        #region Ctor
        public EmployeeSignatureController(
            IEmployeeService employeeService,
            IEmployeeSPService employeeSPService,
            IEmployeeDocumentService employeeDocumentService
            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeDocumentService = employeeDocumentService;
        }
        #endregion

        #region Signature Upload
        public ActionResult SignatureUpload()
        {
            var model = new EmployeeDocumentViewModel { };
            return View(model);
        }

        [HttpPost]
        public ActionResult SignatureUpload(EmployeeDocumentViewModel model, string EmployeeId)
        {
            try
            {
                if (model.DocumentType == null || model.DocumentType == "")
                    //return GetErrorMessageResult("Document Type not found. Please try again!");
                    return Json(new { Result = "ERROR", Message = GetErrorMessageResult("Document Type not found. Please try again!") }, JsonRequestBehavior.AllowGet);

                if (model.DocumentType == EmployeeDocumentTypeConstants.Signature)
                {
                    if (model.ImgFile == null)
                    //return GetErrorMessageResult("File not found. Please try again!");
                    return Json(new { Result = "ERROR", Message = GetErrorMessageResult("File not found. Please try again!") }, JsonRequestBehavior.AllowGet);


                    byte[] data = new byte[model.ImgFile.ContentLength];
                    if (data == null)
                    //return GetErrorMessageResult("File not found. Please try again!");
                    return Json(new { Result = "ERROR", Message = GetErrorMessageResult("File not found. Please try again!")}, JsonRequestBehavior.AllowGet);
                   
                    var employeeSignature = new Employee { EmployeeId = Convert.ToInt64(EmployeeId) };
                    var getEmployeeBasicDetails = employeeService.GetByEmpId(employeeSignature.EmployeeId);

                    model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                    getEmployeeBasicDetails.EmpSignature = data;

                    //let's update employee info [dbo.Employee]
                    employeeService.Update(getEmployeeBasicDetails);

                    return GetSuccessMessageResult();
                }
                else
                {
                    var fileName = string.Empty;
                    //var documentPartialPathe = $"/WebShared/Uploads/EmployeeDocuments/{fileName}";
                    var employeeDocument = new EmployeeDocument
                    {
                        EmployeeId = Convert.ToInt32(EmployeeId),
                        DocumentType = model.DocumentType,
                        //DocumentUrl = documentPartialPathe,
                        DocumentRemarks = model.DocumentRemarks,
                        IsActive = true,
                        CreateUser = (long)SessionHelper.LoginUserEmployeeId
                    };

                    var updateEmployeeDocument = employeeDocumentService.GetByEmployeeId(employeeDocument.EmployeeId, employeeDocument.DocumentType);
                    if (updateEmployeeDocument == null)
                    {
                        //let's insert into [EmployeeDocument]
                        var response = employeeDocumentService.Create(employeeDocument);
                        if (!response.IsSuccess)
                       // return GetErrorMessageResult(response.Message);
                        return Json(new { Result = "ERROR", Message = GetErrorMessageResult(response.Message) }, JsonRequestBehavior.AllowGet);


                        //let's save image into server location
                        if (model.ImgFile != null)
                        {
                            fileName = $"{model.EmployeeId}-{DateTime.Now.Ticks}-{model.DocumentType.ToLower()}.png";
                            employeeDocument.DocumentUrl = SaveDocumentIntoServer(model, fileName);
                            employeeDocument.EmployeeDocumentId = response.Result.EmployeeDocumentId;
                            //let's update path in [EmployeeDocument]
                            employeeDocumentService.Update(employeeDocument);
                        }

                        return GetSuccessMessageResult(response.Message);
                    }
                    else
                    {
                        employeeDocument.EmployeeDocumentId = updateEmployeeDocument.EmployeeDocumentId;

                        //let's update [EmployeeDocument]
                        var response = employeeDocumentService.Update(employeeDocument);
                        if (!response.IsSuccess)
                            return GetErrorMessageResult(response.Message);

                        //let's save image into server location
                        if (model.ImgFile != null)
                        {
                            fileName = $"{model.EmployeeId}-{DateTime.Now.Ticks}-{model.DocumentType.ToLower()}.png";
                            employeeDocument.DocumentUrl = SaveDocumentIntoServer(model, fileName);                           
                            //let's update path in [EmployeeDocument]
                            employeeDocumentService.Update(employeeDocument);
                        }

                        //return GetSuccessMessageResult(response.Message);
                        return Json(new { Result = "Success", Message = GetSuccessMessageResult(response.Message) }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                //return GetErrorMessageResult(ex);
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }       

        #endregion

        #region Events

        public JsonResult GetEmployeeListByCode(string employee_Code)
        {
            try
            {
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { EmployeeCode = employee_Code };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "SP_Get_EmployeeInfoByCode");
                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeCode = row.Field<string>("EmployeeCode")
               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDocumentPartailPath(int employeeId, string documentType)
        {
            try
            {
                var employeeDocument = employeeDocumentService.GetByEmployeeId(employeeId, documentType);
                if(employeeDocument!=null && !string.IsNullOrWhiteSpace(employeeDocument.DocumentUrl))
                    return Json(new { documentPartialPath = employeeDocument.DocumentUrl, remark= employeeDocument .DocumentRemarks}, JsonRequestBehavior.AllowGet);

                return Json(new { documentPartialPath = "/images/blank-headshot.jpg" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { documentPartialPath = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RetrieveSignatureUploadImage(int id)
        {
            byte[] cover = GetSignatureUploadImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/*");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
        }

        public ActionResult GetEmployeeSignature(int id)
        {
            byte[] cover = GetSignatureUploadImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/*");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/Assets/img/signature-default.png");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
        }

        public byte[] GetSignatureUploadImageFromDataBase(int Id)
        {
            var NomineeDetail = employeeService.GetByEmpId(Id);
            var img = NomineeDetail.EmpSignature;
            byte[] cover = img;
            return cover;
        }

        #endregion

        #region Private Methods

        private string SaveDocumentIntoServer(EmployeeDocumentViewModel model, string fileName)
        {
            var imagesFolder = HttpContext.Server.MapPath("~\\WebShared\\uploads\\EmployeeDocuments\\");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder); // create the folder

            Bitmap newImage = null;
            var image = Image.FromStream(model.ImgFile.InputStream);
            newImage = new Bitmap(image, image.Width, image.Height);
            newImage.Save(imagesFolder + fileName, ImageFormat.Png);

            var paritalImagePath = $"/WebShared/uploads/EmployeeDocuments/{fileName}";
            return paritalImagePath;
        } 

        #endregion
    }
}
