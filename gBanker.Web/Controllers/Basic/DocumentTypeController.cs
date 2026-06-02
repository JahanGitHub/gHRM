#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers; 
#endregion

namespace gHRM.Web.Controllers
{
    public class DocumentTypeController : BaseController
    {

        #region Private Methods
        private readonly IDocumentTypeService documentTypeService;
        private readonly IDocumentTypeModuleService documentTypeModuleService; 
        #endregion

        #region Ctor
        public DocumentTypeController(
            IDocumentTypeService documentTypeService,
            IDocumentTypeModuleService documentTypeModuleService
        )
        {
            this.documentTypeService = documentTypeService;
            this.documentTypeModuleService = documentTypeModuleService;
        } 
        #endregion

        private void MapDropdownForProfileById(DocumentTypeViewModel model)
        {
            var DocumentTypeName = new List<SelectListItem>();
            DocumentTypeName.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            
            DocumentTypeName.Add(new SelectListItem() { Text = "Employee Profile", Value = "EP" });
            DocumentTypeName.Add(new SelectListItem() { Text = "Disciplinary Action", Value = "DA" });
            DocumentTypeName.Add(new SelectListItem() { Text = "Case Follow-up", Value = "CF" });
            model.DocumentTypeNameList = DocumentTypeName;

        }

        public JsonResult SaveDocumentType(DocumentType documentType)
        {
            var result = 0;
            var message = "";
            try
            {
                var isDuplicate = documentTypeService.GetAll().Where(p => p.IsActive == true && p.TypeName.ToUpper().Trim() == documentType.TypeName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = 0;
                    message = "documentType Already exists";
                }
                else
                {
                    var entity = new DocumentType();
                    entity.DocumentTypeId = documentType.DocumentTypeId;
                    entity.TypeName = documentType.TypeName;
                    entity.DocumentTypeModuleName = documentType.DocumentTypeModuleName;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    documentTypeService.Create(entity);
                    result = 1;
                    message = "Saved successfully";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult UpdateDocumentType(DocumentType documentType)
        {
            var result = 0;
            var message = "";
            try
            {
                var isDuplicate = documentTypeService.GetAll().Where(p => p.IsActive == true && p.DocumentTypeId != documentType.DocumentTypeId && p.TypeName.ToUpper().Trim() == documentType.TypeName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = 0;
                    message = "DocumentType Already exists";
                }
                else
                {
                    var model = documentTypeService.GetById(documentType.DocumentTypeId);
                    model.TypeName = documentType.TypeName;
                    model.DocumentTypeModuleName = documentType.DocumentTypeModuleName;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateDate = DateTime.UtcNow;
                    documentTypeService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListDocumentType(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var tdvinfo = documentTypeService.GetAll().Where(p => p.IsActive = true).OrderBy(p => p.TypeName).ToList();
            var currentPageRecords = tdvinfo.Skip(jtStartIndex).Take(jtPageSize);

            return
                Json(
                    new
                    {
                        Result = "OK",
                        Records = currentPageRecords,
                        TotalRecordCount = tdvinfo.LongCount(),
                        JsonRequestBehavior.AllowGet
                    });

        }

        public JsonResult InformationDeleteDocumentType(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = documentTypeService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                documentTypeService.Update(model);
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
        
        public ActionResult Index()
        {
            var model = new DocumentTypeViewModel();
            MapDropdownForProfileById(model);
            return View(model);
        }
    }
}