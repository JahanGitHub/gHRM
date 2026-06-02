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
    public class DiscDealingOfficerController : BaseController
    {
        #region Variables
        private readonly IDiscDealingOfficerService discDealingOfficerService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeService officeService;
        public DiscDealingOfficerController(IDiscDealingOfficerService discDealingOfficerService, IEmployeeSPService employeeSPService, IOfficeService officeService)
        {
            this.discDealingOfficerService = discDealingOfficerService;
            this.employeeSPService = employeeSPService;
            this.officeService = officeService;
        }
        #endregion

        #region Methods
        public JsonResult GetAvailableOfficeList(string EmployeeId, string Dispatch)
        {
            try
            {
                List<DiscDealingOfficerViewModel> List_OfficeInfoViewModel = new List<DiscDealingOfficerViewModel>();
                if (Convert.ToInt64(EmployeeId) > 0 && EmployeeId != "")
                {


                    var param = new { EmployeeId = Convert.ToInt64(EmployeeId), Dispatch = Dispatch };
                    var officeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetAvailableOfficeListForDealingOfficer");

                    List_OfficeInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new DiscDealingOfficerViewModel
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
        public JsonResult GetSelectedOfficeList(string EmployeeId, string Dispatch)
        {
            try
            {
                List<DiscDealingOfficerViewModel> List_SelectedOfficeInfoViewModel = new List<DiscDealingOfficerViewModel>();
                if (Convert.ToInt64(EmployeeId) > 0 && EmployeeId != "")
                {
                    var param = new { EmployeeId = Convert.ToInt64(EmployeeId), Dispatch = Dispatch };
                    var officeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetSelectedOfficeListForDealingOfficer");

                    List_SelectedOfficeInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new DiscDealingOfficerViewModel
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


        public JsonResult EmployeeWiseDealingOfficeSave(List<string> allOfficeIds, string employeeId, string Dispatch)
        {
            int dealOfficerId = 0;
            if (employeeId != "")
            {
                var EmployeeId = Convert.ToInt64(employeeId);
                foreach (var offId in allOfficeIds)
                {

                    DiscDealingOfficer entry = new DiscDealingOfficer();
                    entry.EmployeeId = EmployeeId;
                    entry.OfficeId = Convert.ToInt32(offId);
                    entry.Dispatch = Dispatch;
                    entry.IsActive = true;
                    entry.CreateUser = LoggedInEmployeeId;
                    entry.CreateDate = DateTime.Now;

                   var dealOfficerSave = discDealingOfficerService.Create(entry);
                   dealOfficerId = dealOfficerSave.DealOfficerId;
                }
                return Json(dealOfficerId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeWiseDealingOfficeEdit(List<string> allOfficeIds, string employeeId)
        {
            int dealOfficerId = 0;
            var EmployeeId = Convert.ToInt64(employeeId);
            foreach (var offId in allOfficeIds)
            {
                var OfficeList = discDealingOfficerService.GetByEmployeeIdAndOfficeId(EmployeeId, Convert.ToInt32(offId));

                //foreach (var r in OfficeList)
                //{
                OfficeList.IsActive = false;
                OfficeList.InActiveDate = DateTime.Now;
                OfficeList.UpdateUser = LoggedInEmployeeId;
                OfficeList.UpdateDate = DateTime.Now;
                discDealingOfficerService.Update(OfficeList);
                dealOfficerId = OfficeList.DealOfficerId;
                //}
            }
            return Json(dealOfficerId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        //
        // GET: /DiscDealingOfficer/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DiscDealingOfficer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DiscDealingOfficer/Create
        public ActionResult Create()
        {

            var Officeinfo = officeService.GetById((int)LoggedInOfficeID);

            ViewData["Dispatch"] = Officeinfo.OfficeCode; // Officeinfo.Dispatch;


            return View();
        }

        //
        // POST: /DiscDealingOfficer/Create
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
        // GET: /DiscDealingOfficer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DiscDealingOfficer/Edit/5
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
        // GET: /DiscDealingOfficer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscDealingOfficer/Delete/5
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
