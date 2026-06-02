using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Web.Mvc;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Web;
using System.Globalization;

namespace gHRM.Web.Controllers
{
    public class NomineeController : BaseController
    {
        #region Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly INomineeTypeService nomineeTypeService;
        private readonly IEmployeeNomineeService employeeNomineeService;
        private readonly INomineeRelationService nomineeRelationService;
        private readonly ICompanyService companyService;


        public NomineeController(
            IEmployeeSPService employeeSPService,
            IEmployeeNomineeService employeeNomineeService,
            INomineeTypeService nomineeTypeService,
            INomineeRelationService nomineeRelationService,
            ICompanyService companyService

            )
        {
            this.employeeSPService = employeeSPService;
            this.nomineeTypeService = nomineeTypeService;
            this.employeeNomineeService = employeeNomineeService;
            this.nomineeRelationService = nomineeRelationService;
            this.companyService = companyService;
        }

        #endregion

        #region Events

        public ActionResult Create()
        {
            var model = new NomineeViewModel();
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region HttpRequests 


        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            var result = 0;
            try
            {
                var param = new { EmployeeCode = employee_code };
                var empList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                var List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeTransferViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                        EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                        EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                        EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                        EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                        DateOfBirthMsg = row.Field<DateTime?>("DateOfBirth")!=null?(row.Field<DateTime>("DateOfBirth")).ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture):"",
                        ConfirmationDateMsg = row.Field<DateTime?>("ConfirmationDate") != null ? (row.Field<DateTime>("ConfirmationDate")).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",                       
                    }).ToList();

                result = 1;
                return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public decimal GetPersentage(double employeeId, int nomineeTypeId)
        {
            decimal percent = 0;

            try
            {

                var sumNomineePercentage = employeeNomineeService.GetMany(en => en.EmployeeId == employeeId && en.NomineeTypeId == nomineeTypeId && en.IsActive == true).Sum(s => s.NomineePercentage);
                return percent;
            }
            catch (Exception ex)
            {
                return percent;
            }
        }

        public JsonResult GetAvailablePersentage(int EmployeeId, int NomineeTypeId)
        {
            var result = 0;

            try
            {

                var sumNomineePercentage = employeeNomineeService.GetMany(en => en.EmployeeId == EmployeeId && en.NomineeTypeId == NomineeTypeId && en.IsActive == true).Sum(s => s.NomineePercentage);
                var AvailablePersentage = 100 - sumNomineePercentage;
                result = 1;
                return Json(new { result = result, AvailablePersentage= AvailablePersentage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }
        

        [HttpPost]
        public JsonResult SaveEmployeeNomineeInformation(EmployeeNominee employeeNominee)
        {
            long result = 0;
            var message = string.Empty;

            var entity = new EmployeeNominee();
            var savedEntity = new EmployeeNominee();

            try
            {
                if (employeeNominee.NomineeId == 0)
                {
                    /*var isDuplicate = employeeNomineeService.GetMany(p => p.IsActive == true && p.NomineeTypeId == employeeNominee.NomineeTypeId && p.NomineeRelationId == employeeNominee.NomineeRelationId && p.EmployeeId == employeeNominee.EmployeeId);

                    if (isDuplicate.Any())
                    {
                        message = "Duplicate NomineeType And Relation, Save denied";
                    }
                    else
                    {*/

                        var sumNomineePercentage = employeeNomineeService.GetMany(en => en.EmployeeId == employeeNominee.EmployeeId && en.NomineeTypeId == employeeNominee.NomineeTypeId && en.IsActive == true).Sum(s => s.NomineePercentage);

                        if (employeeNominee.NomineeId > 0)
                        {
                            entity = employeeNomineeService.GetByDetailId(employeeNominee.NomineeId);

                            var newSumNomineePercentage = sumNomineePercentage - entity.NomineePercentage + employeeNominee.NomineePercentage;

                            if (newSumNomineePercentage > 100)
                            {
                                message = "Nominee Percentage maximum 100 percent is allowed, Save Denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var newSumNomineePercentage = sumNomineePercentage + employeeNominee.NomineePercentage;
                            if (newSumNomineePercentage > 100)
                            {
                                message = "Nominee Percentage maximum 100 percent is allowed, Save Denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        entity.EmployeeId = employeeNominee.EmployeeId;
                        entity.NomineeTypeId = employeeNominee.NomineeTypeId;
                        entity.NomineeRelationId = employeeNominee.NomineeRelationId;
                        entity.NomineeName = employeeNominee.NomineeName;
                        entity.NomineeAddress = employeeNominee.NomineeAddress;
                        entity.NomineeAge = employeeNominee.NomineeAge;
                        entity.NomineePercentage = employeeNominee.NomineePercentage;
                        entity.ContactNo1 = employeeNominee.ContactNo1;
                        entity.ContactNo2 = employeeNominee.ContactNo2;
                        entity.BirthCertificateNo = employeeNominee.BirthCertificateNo;
                        entity.NomineeImage = employeeNominee.NomineeImage;
                        entity.NomineeRemarks = employeeNominee.NomineeRemarks;
                        entity.NomineeNationalId = employeeNominee.NomineeNationalId;

                        entity.IsActive = true;
                        entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.UtcNow;
                        entity.UpdateDate = DateTime.UtcNow;

                        savedEntity = employeeNomineeService.Create(entity);
                        message = "Save Successfull";
                        result = savedEntity.NomineeId;
                    //}
                }
                else
                {
                    //var loggedInOrg = SessionHelper.OrganizationName;
                    //var companyInfo = companyService.GetById((int) SessionHelper.CompanyID);
                    var isDuplicate =
                   employeeNomineeService.GetAll().Where(
                           p => p.IsActive == true && p.NomineeTypeId == employeeNominee.NomineeTypeId && p.NomineeRelationId == employeeNominee.NomineeRelationId && p.EmployeeId == employeeNominee.EmployeeId && p.NomineeId != employeeNominee.NomineeId).ToList();
                    if (isDuplicate.Any()) //&& companyInfo.CompanyShortName != "addin"
                    {
                        message = "Duplicate NomineeType And Relation, Update denied";
                    }
                    else
                    {
                        var entityUpdate = employeeNomineeService.GetById(Convert.ToInt16(employeeNominee.NomineeId));
                        var sumNomineePercentage = employeeNomineeService.GetMany(en => en.EmployeeId == employeeNominee.EmployeeId && en.NomineeTypeId == employeeNominee.NomineeTypeId && en.IsActive == true).Sum(s => s.NomineePercentage);

                        if (employeeNominee.NomineeId > 0)
                        {
                            entityUpdate = employeeNomineeService.GetByDetailId(employeeNominee.NomineeId);

                            var newSumNomineePercentage = sumNomineePercentage - entityUpdate.NomineePercentage + employeeNominee.NomineePercentage;

                            if (newSumNomineePercentage > 100)
                            {
                                message = "Nominee Percentage maximum 100 percent is allowed, Update Denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var newSumNomineePercentage = sumNomineePercentage + employeeNominee.NomineePercentage;
                            if (newSumNomineePercentage > 100)
                            {
                                message = "Nominee Percentage maximum 100 percent is allowed, Update Denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        entityUpdate.EmployeeId = employeeNominee.EmployeeId;
                        entityUpdate.NomineeId = employeeNominee.NomineeId;
                        entityUpdate.NomineeTypeId = employeeNominee.NomineeTypeId;
                        entityUpdate.NomineeRelationId = employeeNominee.NomineeRelationId;
                        entityUpdate.NomineeName = employeeNominee.NomineeName;
                        entityUpdate.NomineeAddress = employeeNominee.NomineeAddress;
                        entityUpdate.NomineeAge = employeeNominee.NomineeAge;
                        entityUpdate.NomineePercentage = employeeNominee.NomineePercentage;
                        entityUpdate.ContactNo1 = employeeNominee.ContactNo1;
                        entityUpdate.ContactNo2 = employeeNominee.ContactNo2;
                        entityUpdate.BirthCertificateNo = employeeNominee.BirthCertificateNo;
                        entityUpdate.NomineeRemarks = employeeNominee.NomineeRemarks;
                        entityUpdate.NomineeNationalId = employeeNominee.NomineeNationalId;
                        //entity.GuarantorImage = employeeGuarantorInformation.GuarantorImage;

                        entityUpdate.IsActive = true;
                        entityUpdate.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entityUpdate.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entityUpdate.CreateDate = DateTime.UtcNow;
                        entityUpdate.UpdateDate = DateTime.UtcNow;
                        employeeNomineeService.Update(entityUpdate);

                        message = "Update Successfull";
                        result = employeeNominee.NomineeId;
                    }
                }
            }

            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult ListEmployeeNomineeInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, int EmployeeId)
        {
            var param = new { EmployeeId = EmployeeId };
            var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeNomineeList");

            var List_NomineeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
           .Select(row => new NomineeViewModel
           {
               SNO = row.Field<string>("SNO"),
               EmployeeId = row.Field<long>("EmployeeId"),
               NomineeId = row.Field<long>("NomineeId"),
               NomineeTypeId = row.Field<int>("NomineeTypeId"),
               NomineeType = row.Field<string>("NomineeTypeName"),
               NomineeTypeValue = row.Field<string>("NomineeTypeValue"),
               NomineeName = row.Field<string>("NomineeName"),
               NomineeAddress = row.Field<string>("NomineeAddress"),
               NomineeAge = row.Field<int?>("NomineeAge"),
               NomineeRelationId = row.Field<int>("NomineeRelationId"),
               NomineeRelation = row.Field<string>("NomineeRelation"),
               NomineePercentage = row.Field<decimal>("NomineePercentage"),
               NomineeNationalId = row.Field<string>("NomineeNationalId"),
               NomineeRemarks = row.Field<string>("NomineeRemarks"),
               ContactNo1 = row.Field<string>("ContactNo1"),
               ContactNo2 = row.Field<string>("ContactNo2"),
               BirthCertificateNo = row.Field<string>("BirthCertificateNo")
           }).ToList();
            var currentPageRecords = List_NomineeViewModel.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_NomineeViewModel.LongCount(), JsonRequestBehavior.AllowGet });
        }


        public JsonResult InformationDeleteNomineeInformation(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeNomineeService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeNomineeService.Update(model);
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


        [HttpPost]
        public ActionResult UploadNomineeImage(HttpPostedFileBase file, string NomineeId)
        {
            var Result = 0;
            var entity = employeeNomineeService.GetByGurId(Convert.ToInt32(NomineeId));

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.NomineeImage = data;
                employeeNomineeService.Update(entity);
                Result = 1;
            }
            else
            {
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RetrieveNomineeImage(int id)
        {
            byte[] cover = GetNomineeImageFromDataBase(id);
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

        public byte[] GetNomineeImageFromDataBase(int Id)
        {
            var NomineeDetail = employeeNomineeService.GetByGurId(Id);
            var img = NomineeDetail.NomineeImage;
            byte[] cover = img;
            return cover;
        }

        

        #endregion

        #region Methods
        

        private void MapDropDownList(NomineeViewModel model)
        {
            var nomineeTypeList = nomineeTypeService.GetMany(w => w.IsActive == true).ToList();
            var nomineeTypeListView = nomineeTypeList.OrderBy(k=>k.ViewOrder).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NomineeTypeId.ToString(),
                Text = x.NomineeTypeName.ToString()
            });

            var nominee_type = new List<SelectListItem>();
            if (nomineeTypeList.Count > 1)
            {
                nominee_type.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            }
            nominee_type.AddRange(nomineeTypeListView);

            model.NomineeTypeList = nominee_type;


            var relationList = nomineeRelationService.GetMany(p => p.IsActive == true).ToList();
            var viewRelationList = relationList.Select(p => new SelectListItem()
            {
                Text = p.RelationName,
                Value = p.RelaitonId.ToString()
            }).ToList();
            var relation = new List<SelectListItem>();
            relation.Add(new SelectListItem { Text = "Please Select", Value = "" });
            relation.AddRange(viewRelationList);
            model.RelationshipList = relation;
        }

        #endregion

    }
}
