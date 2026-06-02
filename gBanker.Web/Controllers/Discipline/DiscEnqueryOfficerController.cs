using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Models;
using gHRM.Web.ViewModels;
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
using gHRM.Service.StoreProcedure;
using gHRM.Service.Discipline;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Web.Controllers
{
    public class DiscEnqueryOfficerController : BaseController
    {
        #region Variables
        private readonly IDiscEnqueryOfficerService discEnqueryOfficerService;
        private readonly IEmployeeSPService employeeSPService;
        public DiscEnqueryOfficerController(IDiscEnqueryOfficerService discEnqueryOfficerService, IEmployeeSPService employeeSPService)
        {
            this.discEnqueryOfficerService = discEnqueryOfficerService;
            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Methods
        public JsonResult GetAvailableOfficeList(string EmployeeId)
        {
            try
            {
                List<DiscEnqueryOfficerViewModel> List_OfficeInfoViewModel = new List<DiscEnqueryOfficerViewModel>();
                if (Convert.ToInt64(EmployeeId) > 0 && EmployeeId != "")
                {
                    var param = new { EmployeeId = Convert.ToInt64(EmployeeId) };
                    var officeList = employeeSPService.GetDataWithParameter(param, "SP_GetAvailableOfficeListForEnqueryOfficer");

                    List_OfficeInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new DiscEnqueryOfficerViewModel
                    {
                        Sl = row.Field<long>("Sl"),
                        OfficeId = row.Field<int>("OfficeId"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName")

                    }).ToList();
                }
                return Json(List_OfficeInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectedOfficeList(string EmployeeId)
        {
            try
            {
                List<DiscEnqueryOfficerViewModel> List_SelectedOfficeInfoViewModel = new List<DiscEnqueryOfficerViewModel>();
                if (Convert.ToInt64(EmployeeId) > 0 && EmployeeId != "")
                {
                    var param = new { EmployeeId = Convert.ToInt64(EmployeeId) };
                    var officeList = employeeSPService.GetDataWithParameter(param, "SP_GetSelectedOfficeListForEnqueryOfficer");

                    List_SelectedOfficeInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new DiscEnqueryOfficerViewModel
                    {
                        Sl = row.Field<long>("Sl"),
                        OfficeId = row.Field<int>("OfficeId"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName")

                    }).ToList();
                }
                return Json(List_SelectedOfficeInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult EmployeeWiseEnqueryOfficeSave(List<string> allOfficeIds, string employeeId)
        {
            int enqueryOfficerId = 0;
            if (employeeId != "")
            {
                var EmployeeId = Convert.ToInt64(employeeId);
                foreach (var offId in allOfficeIds)
                {
                    DiscEnqueryOfficer entry = new DiscEnqueryOfficer();
                    entry.EmployeeId = EmployeeId;
                    entry.OfficeId = Convert.ToInt32(offId);
                    entry.IsActive = true;
                    entry.CreateUser = LoggedInEmployeeId;
                    entry.CreateDate = DateTime.Now;

                    var dealOfficerSave = discEnqueryOfficerService.Create(entry);
                    enqueryOfficerId = dealOfficerSave.EnqueryOfficerId;
                }
                return Json(enqueryOfficerId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeWiseEnqueryOfficeEdit(List<string> allOfficeIds, string employeeId)
        {
            int enqueryOfficerId = 0;
            var EmployeeId = Convert.ToInt64(employeeId);
            foreach (var offId in allOfficeIds)
            {
                var OfficeList = discEnqueryOfficerService.GetByEmployeeIdAndOfficeId(EmployeeId, Convert.ToInt32(offId));

                //foreach (var r in OfficeList)
                //{
                OfficeList.IsActive = false;
                OfficeList.InActiveDate = DateTime.Now;
                OfficeList.UpdateUser = LoggedInEmployeeId;
                OfficeList.UpdateDate = DateTime.Now;
                discEnqueryOfficerService.Update(OfficeList);
                enqueryOfficerId = OfficeList.EnqueryOfficerId;
                //}
            }
            return Json(enqueryOfficerId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        //
        // GET: /DiscEnqueryOfficer/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DiscEnqueryOfficer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DiscEnqueryOfficer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DiscEnqueryOfficer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DiscEnqueryOfficer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DiscEnqueryOfficer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DiscEnqueryOfficer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscEnqueryOfficer/Delete/5
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
        #endregion

    }
}
