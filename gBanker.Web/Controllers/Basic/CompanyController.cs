#region Usings
using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.Helpers;
using System.Drawing;
using System.Drawing.Imaging; 
#endregion

namespace gHRM.Web.Controllers
{
    public class CompanyController : BaseController
    {
        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;
        private readonly IStateOrProvinceService stateOrProvinceService;
        private readonly ICountryService countryService;
        #endregion

        #region Ctor

        public CompanyController(ICompanyService companyService, IEmployeeService employeeService, IStateOrProvinceService stateOrProvinceService, ICountryService countryService)
        {
            this.countryService = countryService;
            this.stateOrProvinceService = stateOrProvinceService;
            this.employeeService = employeeService;
            this.companyService = companyService;

        }

        #endregion

        #region Methods
        public byte[] GetImageFromDataBase(int Id)
        {
            var employeeDetail = employeeService.GetById(Id);
            var img = employeeDetail.EmployeeImage;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;
        }
        public ActionResult RetrieveImage(int id)
        {
            //byte[] cover = GetImageFromDataBase(id);
            //if (cover != null)
            //{
            //    return File(cover, "image/*");
            //}
            //else
            //{
            //    return null;
            //}
            byte[] cover = GetImageFromDataBase(id);
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

                return File(blnk, "image/*"); ;
            }
        }
        public JsonResult GetCompanies(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                var companyDetail = companyService.GetCompanyDetail(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = companyDetail.ToList();
                //var totCount = detail.Count();
                var currentPageRecords = detail.ToList();
                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        public JsonResult GetCompanyList(int companyId)
        {
            var companyList = companyService.GetAll().Where(w => w.CompanyId == (companyId));
            var viewCompany = companyList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CompanyId.ToString(),
                Text = string.Format("{0} - {1}", x.CompanyName, x.CompanyPhone)
            });
            var company_items = new List<SelectListItem>();
            company_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            company_items.AddRange(viewCompany);
            return Json(company_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountryList(string countryCode)
        {
            var countryList = countryService.GetAll().Where(c => c.CountryId == Convert.ToInt32(countryCode));
            var viewCountry = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountry);
            return Json(country_items, JsonRequestBehavior.AllowGet);
        }

        private void MapDropDownList(CompanyViewModel model)
        {
            //Country DropDownList
            var countryList = countryService.SearchCountry();
            var viewCountry = countryList.Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName
            });

            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountry);
            model.CountryList = country_items;

            var natureOfCompany = new List<SelectListItem>();
            natureOfCompany.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            natureOfCompany.Add(new SelectListItem() { Text = "Private Company" });
            natureOfCompany.Add(new SelectListItem() { Text = "Public Company" });
            natureOfCompany.Add(new SelectListItem() { Text = "Join Venture Company" });
            model.CompanyList = natureOfCompany;
        }
        public JsonResult CompanyDelete(string companyId)
        {
            var entity = companyService.GetById(Convert.ToInt32(companyId));
            string Result = "OK";
            if (ModelState.IsValid)
            {
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                companyService.Update(entity);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Events
        //
        // GET: /Company/
        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: /Company/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        //
        // GET: /Company/Create
        public ActionResult Create()
        {
            var model = new CompanyViewModel();
            MapDropDownList(model);
            return View(model);

        }


        // POST: /Company/Create
        [HttpPost]
        public ActionResult Create(CompanyViewModel model, HttpPostedFileBase ImgFile)
        {
            var a = Request.Files;
            var entity = Mapper.Map<CompanyViewModel, Company>(model);
            try
            {
                var errors = companyService.IsValidCompany(entity.CompanyId);
                if (errors.ToList().Count == 0)
                {
                    if (model.ImgFile != null)
                    {
                        byte[] data = new byte[model.ImgFile.ContentLength];
                        model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                        entity.CompanyImage = data;
                    }
                    else
                    {
                        entity.CompanyImage = null;
                    }
                    entity.IsActive = true;
                    companyService.Create(entity);
                    return GetSuccessMessageResult();
                    //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }
                else
                    return GetSuccessMessageResult();
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return GetSuccessMessageResult();
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }

        }
        /*
        private string SaveCompanyImagetoFileSystem(string base64)
        {
                    bool exists = System.IO.Directory.Exists(Server.MapPath("/CompanyLogo"));
            if (!exists)
            {
                        System.IO.Directory.CreateDirectory(Server.MapPath("/CompanyLogo"));
            }    
                    var filePath = Server.MapPath("/CompanyLogo/CompanyLogo.png");
            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
                   // var hostAddress = Request.Url.OriginalString.Replace(Request.Url.LocalPath, "");
                    //return "/CompanyLogo/CompanyLogo.png";
                    return filePath;
                }
         * byte[] data
         * 
          */
        private string SaveCompanyImagetoFileSystem(byte[] data)
        {
            bool exists = System.IO.Directory.Exists(Server.MapPath("/CompanyLogo"));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/CompanyLogo"));
            }
            var filePath = "";


            ImageFormat format = ImageFormat.Png;
            SizeF logoSize = SizeF.Empty;
            logoSize.Height = 66;
            logoSize.Width = 246;
            logoSize.Height /= (float)1.4;
            logoSize.Width /= (float)1.7;
            int w = Convert.ToInt32(logoSize.Width);
            int h = Convert.ToInt32(logoSize.Height);
            System.Drawing.Image OrginalImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));
            Graphics tmpGraphics = default(Graphics);

            Bitmap setResizeImage = new Bitmap(w, h);
            tmpGraphics = Graphics.FromImage(setResizeImage);
            tmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            tmpGraphics.DrawImage(OrginalImage, 0, 0, setResizeImage.Width + 1, setResizeImage.Height + 1);
            System.Drawing.Image outputImage = setResizeImage;

            var strFileName = "/CompanyLogo/";
            strFileName += "CompanyLogo";
            strFileName += "." + format.ToString();
            string FileLocation = Server.MapPath(strFileName);
            outputImage.Save(FileLocation, format);
            // var hostAddress = Request.Url.OriginalString.Replace(Request.Url.LocalPath, "");
            //return "/CompanyLogo/CompanyLogo.png";
            return strFileName;
        }
        //
        // GET: /Company/Edit/5
        public ActionResult Edit(int id)
        {
            if (employeeService.IsContinued(id))
            {
                var company = companyService.GetById(id);
                var entity = Mapper.Map<Company, CompanyViewModel>(company);
                MapDropDownList(entity);
                if (entity.ImagePath == null)
                {
                    entity.ImagePath = "~/Images/blank-headshot.jpg";
                }
                else
                {
                    entity.ImagePath = entity.ImagePath;//.Replace(@"\", @"\\");
                }

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Duplicate Company, please enter a diferent Company id and name.");
            return RedirectToAction("Index");

        }

        //
        // POST: /Company/Edit/5
        [HttpPost]
        public ActionResult Edit(CompanyViewModel model)
        {
            try
            {

                var entity = Mapper.Map<CompanyViewModel, Company>(model);
                var getCompanyDetail = companyService.GetById(Convert.ToInt32(entity.CompanyId));
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    getCompanyDetail.CompanyName = entity.CompanyName;
                    getCompanyDetail.CompanyAddress = entity.CompanyAddress;
                    getCompanyDetail.CompanyEmail = entity.CompanyEmail;
                    getCompanyDetail.CompanyPhone = entity.CompanyPhone;
                    getCompanyDetail.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    getCompanyDetail.UpdateDate = DateTime.Now;
                    if (model.ImgFile != null)
                    {
                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            getCompanyDetail.CompanyImage = data;
                            var base64 = Convert.ToBase64String(data);
                            var imgPath = SaveCompanyImagetoFileSystem(data);
                            //getCompanyDetail.ImagePath = imgPath;
                        }
                    }
                    companyService.Update(getCompanyDetail);
                    //Page.Response.Redirect(Page.Request.Url.ToString(), true);
                    return GetSuccessMessageResult();

                }

                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
            //return View();
        }

        //
        // GET: /Company/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = companyService.GetById(id);
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    companyService.Update(entity);
                }

                return Json(new { Result = "OK" });
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Manage

        public ActionResult Manage()
        {            
            var companyId = (int)SessionHelper.CompanyID;
            var company = companyService.GetById(companyId);

            var model = Mapper.Map<Company, CompanyViewModel>(company);

            model.ImagePreviewPath = @"/Assets/img/company-default-logo.jpg";
            model.CompanySignaturePreviewPath = @"/Assets/img/company-default-logo.jpg";

            if(!string.IsNullOrWhiteSpace(company.ImagePath))
                model.ImagePreviewPath=$@"{company.ImagePath}";
            
            if(!string.IsNullOrWhiteSpace(company.CompanySignaturePath))
                model.CompanySignaturePreviewPath = $@"{company.CompanySignaturePath}";

            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Manage(CompanyViewModel model)
        {            
            if (!ModelState.IsValid)
                return RedirectToAction("Manage");

            var entity = Mapper.Map<CompanyViewModel, Company>(model);
            try
            {              
                if (model.ImgFile != null)
                {
                    var fileName = $"{SessionHelper.CompanyCode}-company-logo.jpg";
                    var logoUrl = SaveDocumentIntoServer(model, fileName);
                    entity.ImagePath = logoUrl;

                    //set company logo in session
                    SessionHelper.CompanyImage = entity.ImagePath;
                }

                if (model.CopanySignatureFile != null)
                {
                    var fileName = $"{SessionHelper.CompanyCode}-company-signature.jpg";
                    var signatureUrl = SaveCompanySignatureIntoServer(model, fileName);
                    entity.CompanySignaturePath = signatureUrl;

                    //set company logo in session
                    SessionHelper.CompanySignature = entity.CompanySignaturePath;
                }

                companyService.UpdateCompanyInfo(entity);

                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Manage");
        }

        #endregion

        #region Private Methods

        private string SaveCompanySignatureIntoServer(CompanyViewModel model, string fileName)
        {
            var imagesFolder = HttpContext.Server.MapPath("~\\WebShared\\uploads\\CompanySignatures\\");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder); // create the folder

            Bitmap newImage = null;
            var image = Image.FromStream(model.CopanySignatureFile.InputStream);
            newImage = new Bitmap(image, image.Width, image.Height);
            newImage.Save(imagesFolder + fileName, ImageFormat.Png);

            var paritalImagePath = $"/WebShared/uploads/CompanySignatures/{fileName}";
            return paritalImagePath;
        }

        private string SaveDocumentIntoServer(CompanyViewModel model, string fileName)
        {
            var imagesFolder = HttpContext.Server.MapPath("~\\WebShared\\uploads\\CompanyDocuments\\");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder); // create the folder

            Bitmap newImage = null;
            var image = Image.FromStream(model.ImgFile.InputStream);
            newImage = new Bitmap(image, image.Width, image.Height);
            newImage.Save(imagesFolder + fileName, ImageFormat.Png);

            var paritalImagePath = $"/WebShared/uploads/CompanyDocuments/{fileName}";
            return paritalImagePath;
        }

        #endregion
    }
}
