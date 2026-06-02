using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace gHRM.Web.Controllers
{
    public class TrainingController : BaseController
    {
        private readonly ICountryService countryService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IView_EmployeeTrainingService view_EmployeeTrainingService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeService employeeService;


        public TrainingController(IEducationDegreeService educationDegreeService
              , ICountryService countryService
              , IEmployeeTrainingService employeeTrainingService
              , IView_EmployeeTrainingService view_EmployeeTrainingService
              , IEmployeeSPService employeeSpService
              , IEmployeeService employeeService

        )
        {
            this.countryService = countryService;
            this.employeeTrainingService = employeeTrainingService;
            this.view_EmployeeTrainingService = view_EmployeeTrainingService;
            this.employeeSpService = employeeSpService;
            this.employeeService = employeeService;

        }



        private void MapDropdownForEmployeeTraining(EmployeeViewModel model)
        {
            var countryList = countryService.GetAll().Where(w => w.CountryId == CountryID);
            var viewCountryList = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountryList);
            model.CountryList = country_items;

            var CurrentOfficeTraining = new List<SelectListItem>();
            CurrentOfficeTraining.Add(new SelectListItem { Text = "Please Select", Value = "" });
            CurrentOfficeTraining.Add(new SelectListItem { Text = "Yes", Value = "1" });
            CurrentOfficeTraining.Add(new SelectListItem { Text = "No", Value = "0" });
            model.CurrentOfficeTrainingList = CurrentOfficeTraining;

            var isapproved = new List<SelectListItem>();
            isapproved.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isapproved.Add(new SelectListItem { Text = "Approved", Value = "1" });
            //isapproved.Add(new SelectListItem { Text = "NonApproved", Value = "0" });
            model.isapprovedList = isapproved;

            var isrejected = new List<SelectListItem>();
            isrejected.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isrejected.Add(new SelectListItem { Text = "Rejected", Value = "1" });
            //isrejected.Add(new SelectListItem { Text = "NonRejected", Value = "0" });
            model.isrejectedList = isrejected;

        }


        //[HttpPost]
        //public JsonResult SaveTraining(EmployeeTraining employeeTraining)
        //{
        //    var result = string.Empty;
        //    try
        //    {
        //        var isDuplicate =
        //            employeeTrainingService.GetAll()
        //                .Where(
        //                    p =>
        //                        p.IsActive == true &&
        //                        p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim())
        //                .ToList();
        //        if (isDuplicate.Any())
        //        {
        //            result = "Duplicate Employee Training InstituteName, Save denied";
        //        }
        //        else
        //        {
        //            var entity = employeeTraining;
        //            var employeeId = employeeService.GetByEmpId(Convert.ToInt64(employeeTraining.EmployeeCode)).EmployeeId;
        //            entity.EmployeeId = employeeId;
        //            entity.IsApproved = employeeTraining.IsApproved;
        //            entity.IsRejected = employeeTraining.IsRejected;
        //            entity.approveby = employeeTraining.approveby;
        //            entity.IsActive = true;
        //            entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //            entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //            entity.CreateDate = DateTime.UtcNow;
        //            entity.UpdateDate = DateTime.UtcNow;
        //            var savedEntity = employeeTrainingService.Create(entity);
        //            result = "Save Successfull";
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        result = ex.InnerException.Message.ToString();
        //    }
        //    return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        //}


        [HttpPost]
        public JsonResult SaveTraining(EmployeeTraining employeeTraining)
        {
            var result = string.Empty;
            try
            {
                //var isDuplicate =
                //    employeeTrainingService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim())
                //        .ToList();

                //if (isDuplicate.Any())
                //{
                //    result = "Duplicate Employee Training InstituteName, Save denied";
                //    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                //}

                //Populate Employee Training info
                EmployeeTraining entity = PopulateEmployeeTraining(employeeTraining);

                var savedEntity = employeeTrainingService.Create(entity);
                result = "Save Successfull";

                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }

        private EmployeeTraining PopulateEmployeeTraining(EmployeeTraining employeeTraining)
        {
            var employee = employeeService.GetByCode(employeeTraining.EmployeeCode);
            
            var entity = employeeTraining;
            
            entity.EmployeeCode = employee.EmployeeCode;
            entity.EmployeeId = employee.EmployeeId;
            entity.IsApproved = employeeTraining.IsApproved;
            entity.IsRejected = employeeTraining.IsRejected;
            entity.approveby = employeeTraining.approveby;
            entity.IsActive = true;
            entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;

            return entity;
        }

        public JsonResult UpdateTraining(EmployeeTraining employeeTraining)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   employeeTrainingService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.EmployeeTrainingId != employeeTraining.EmployeeTrainingId &&
                               p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Employee Training InstituteName, Update denied";
                }
                else
                {
                    var entity = employeeTrainingService.GetById(employeeTraining.EmployeeTrainingId);
                    entity.EmployeeTrainingId = employeeTraining.EmployeeTrainingId;
                    entity.TrainingTitle = employeeTraining.TrainingTitle;
                    entity.InstituteName = employeeTraining.InstituteName;
                    entity.TrainingCountryId = employeeTraining.TrainingCountryId;
                    entity.TrainingTopics = employeeTraining.TrainingTopics;
                    entity.Result = employeeTraining.Result;
                    entity.TrainingDateFrom = employeeTraining.TrainingDateFrom;
                    entity.TrainingDateTo = employeeTraining.TrainingDateTo;
                    entity.CurrentOfficeTraining = employeeTraining.CurrentOfficeTraining;
                    entity.IsApproved = employeeTraining.IsApproved;
                    entity.IsRejected = employeeTraining.IsRejected;
                    entity.approveby = employeeTraining.approveby;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeTrainingService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListTraning(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var vmcar = view_EmployeeTrainingService.GetAll().Where(t => t.IsActive == true);

            var currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult InformationDeleteTraning(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTrainingService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTrainingService.Update(model);
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

        public JsonResult GETTraningTimeTrainingDateTo(DateTime TrainingDateFrom, DateTime TrainingDateTo)
        {
            var result = 0;
            try
            {
                if (TrainingDateFrom <= TrainingDateTo)
                {
                    result = 1;
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = 0;
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult empss(string emps)
        {
            var result = 0;
            try
            {
                if (emps != "")
                {
                    var eid = employeeTrainingService.GetAll().Where(t => t.IsActive == true && t.EmployeeId == Convert.ToInt64(emps)).ToList();
                    result = 1;
                    return Json(new { result = result, eid = eid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = 0;
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeInfoByCode(string empCode)
        {
            var param = new { EmployeeCode = empCode };
            var empInfo = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
            var viewEmpInfo =
                empInfo.Tables[0].AsEnumerable().Select(p => new EmployeeViewModel()
                {
                    EmployeeId = p.Field<long>("EmployeeId"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    Department = p.Field<string>("DepartmentName"),
                    Designation = p.Field<string>("DesignationName"),
                    EmployeeName = p.Field<string>("EmployeeName")
                }).ToList();

            return Json(viewEmpInfo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index()
        {
            var model = new EmployeeViewModel();
            //model.EmployeeId = Convert.ToInt64(EmployeeId);
            MapDropdownForEmployeeTraining(model);
            return View(model);
        }
    }
}