using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.Repository;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using Itenso.TimePeriod;
using Microsoft.Owin.Security.Provider;

namespace gHRM.Web.Controllers
{
    public class EmployeeEquivalentDesignationController : BaseController
    {
        private readonly IEmployeeEquivalentDesignationService employeeEquivalentDesignationService;
        public EmployeeEquivalentDesignationController(
            IEmployeeEquivalentDesignationService employeeEquivalentDesignationService
        )
        {
            this.employeeEquivalentDesignationService = employeeEquivalentDesignationService;
        }

        //private void MapDropdownForTADAFairbytadavehicle(EmployeeApplyConfigViewModel model)
        //{


        //    var tadaDAC = tadaDailyAllowanceComponentService.GetAll().Where(p => p.IsActive == 1);
        //    var viewtadaDAC = tadaDAC.Select(a => new SelectListItem()
        //    {
        //        Value = a.DACompId.ToString(),
        //        Text = a.ComponentName
        //    });
        //    var tadaDAClist = new List<SelectListItem>();
        //    tadaDAClist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
        //    tadaDAClist.AddRange(viewtadaDAC);
        //    model.ComponentNameList = tadaDAClist;

        //}

        public JsonResult SaveEmployeeEquivalentDesignation(EmployeeEquivalentDesignation employeeEquivalentDesignation)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    employeeEquivalentDesignationService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.EquivalentDesignationName.ToUpper().Trim() == employeeEquivalentDesignation.EquivalentDesignationName.ToUpper().Trim())
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate employee Equivalent Designation found, Save denied";
                }
                else
                {
                    var entity = new EmployeeEquivalentDesignation();
                    entity.EquivalentDesigId = employeeEquivalentDesignation.EquivalentDesigId;
                    entity.EquivalentDesignationName = employeeEquivalentDesignation.EquivalentDesignationName;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeEquivalentDesignationService.Create(entity);
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateEmployeeEquivalentDesignation(EmployeeEquivalentDesignation employeeEquivalentDesignation)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   employeeEquivalentDesignationService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.EquivalentDesigId != employeeEquivalentDesignation.EquivalentDesigId &&
                               p.EquivalentDesignationName.ToUpper().Trim() == employeeEquivalentDesignation.EquivalentDesignationName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate employee Equivalent Designation found, Save denied";
                }
                else
                {
                    var entity = employeeEquivalentDesignationService.GetById(employeeEquivalentDesignation.EquivalentDesigId);
                    entity.EquivalentDesigId = employeeEquivalentDesignation.EquivalentDesigId;
                    entity.EquivalentDesignationName = employeeEquivalentDesignation.EquivalentDesignationName;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeEquivalentDesignationService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ListEmployeeEquivalentDesignation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var vmcar = employeeEquivalentDesignationService.GetMany(t => t.IsActive == true);
            var listVMcartype = vmcar.AsEnumerable().Select(a => new EmployeeEquivalentDesignation()
            {
                EquivalentDesigId = a.EquivalentDesigId,
                EquivalentDesignationName = a.EquivalentDesignationName
            }).ToList();

            var currentPageRecords = listVMcartype.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = listVMcartype.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult InformationDeleteEmployeeEquivalentDesignation(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeEquivalentDesignationService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeEquivalentDesignationService.Update(model);
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




        //public ActionResult index()
        //{
        //    var model = new EmployeeApplyConfigViewModel();
        //    MapDropdownForTADAFairbytadavehicle(model);
        //    return View(model);
        //}


        public ActionResult index()
        {
            return View();
        }

    }
}














