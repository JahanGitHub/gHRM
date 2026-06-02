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

namespace gHRM.Web.Controllers
{
    public class BranchController : BaseController
    {

        #region Variables
        private readonly IBranchService branchService;
        private readonly ICompanyService companyService;

        public BranchController(IBranchService branchService, ICompanyService companyService)
        {            
            this.branchService = branchService;
            this.companyService = companyService;         
        }
        #endregion

        #region Methods
        private void MapDropDownList(BranchViewModel model)
        {
            //Company Dropdown
            var companyList = companyService.GetAll();
            var ViewcompanyList = companyList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CompanyId.ToString(),
                Text = x.CompanyName.ToString()
            });
            var company_items = new List<SelectListItem>();
            company_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            company_items.AddRange(ViewcompanyList);
            model.CompanyList = company_items;

        }

        public JsonResult GetBranchList(int jtStartIndex, int jtPageSize, string jtSorting, string companyId, string branchId)
        {
            try
            {
                long TotCount;
                int BranchId = Convert.ToInt32(string.IsNullOrEmpty(branchId) ? "0" : branchId);
                int CompanyId = Convert.ToInt32(string.IsNullOrEmpty(companyId) ? "0" : companyId);
                var branchDetail = branchService.GetBranchDetail(CompanyId, BranchId, jtStartIndex, jtSorting, jtPageSize, out TotCount);

                var detail = branchDetail.ToList();
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
        public JsonResult GetCompanyList()
        {
            var CompanyList = companyService.GetAll();
            var viewCompany = CompanyList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CompanyId.ToString(),
                Text = x.CompanyName.ToString()
            });
            var company_items = new List<SelectListItem>();
            if (viewCompany.ToList().Count > 0)
            {
                company_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            company_items.AddRange(viewCompany);
            return Json(company_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSearchBranchList(string company_val)
        {

            var BranchList = branchService.GetAll().Where(c => c.IsActive==true && c.CompanyId == Convert.ToInt32(company_val));
            var viewBranch = BranchList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.BranchId.ToString(),
                Text = x.BranchName.ToString().Trim()
            });
            var branchn_items = new List<SelectListItem>();
            if (viewBranch.ToList().Count > 0)
            {
                branchn_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            branchn_items.AddRange(viewBranch);
            return Json(branchn_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BranchDelete(string branchId)
        {
            var entity = branchService.GetById(Convert.ToInt32(branchId));
            string Result = "OK";
            if (ModelState.IsValid)
            {
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                branchService.Update(entity);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Events
        //
        // GET: /Branch/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CompanyList"] = items;
            ViewData["BranchList"] = items;         
            return View();
        }

        //
        // GET: /Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Branch/Create
        public ActionResult Create()
        {
            var model = new BranchViewModel();
            MapDropDownList(model);
            return View(model);
        }

        //
        // POST: /Branch/Create
        [HttpPost]
        public ActionResult Create(BranchViewModel model)
        {
            var entity = Mapper.Map<BranchViewModel, Branch>(model);
            try
            {
                // entity.Status = true;
                branchService.Create(entity);
                return GetSuccessMessageResult();
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /Branch/Edit/5
        public ActionResult Edit(int id)
        {
            //if (branchService.IsContinued(id))
            //{
                var branch = branchService.GetById(Convert.ToInt32(id));
                var entity = Mapper.Map<Branch, BranchViewModel>(branch);
                //ViewData["EmployeeId"] = id.ToString();
                MapDropDownList(entity);
                return View(entity);
            //}
            //else
            //    ModelState.AddModelError("Validation", "Discontinued District, please enter a diferent District id and Name.");
            //return RedirectToAction("Index");
        }

        //
        // POST: /Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(BranchViewModel model)
        {
            try
            {
                var entity = Mapper.Map<BranchViewModel, Branch>(model);
                var getBranchDetails = branchService.GetById(Convert.ToInt32(entity.BranchId));

                if (ModelState.IsValid)
                {
                    //getDistrictDetails.CountrtyId = entity.CountrtyId;
                    getBranchDetails.BranchName = entity.BranchName;
                    getBranchDetails.BranchAddress = entity.BranchAddress;
                    getBranchDetails.BranchEmail = entity.BranchEmail;
                    getBranchDetails.BranchPhone = entity.BranchPhone;
                    getBranchDetails.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    getBranchDetails.UpdateDate = DateTime.Now;

                    branchService.Update(getBranchDetails);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /Branch/Delete/5
        public ActionResult Delete(int id)
        {
            //branchService.Delete(id);
            return RedirectToAction("Index");
        }

        //
        // POST: /Branch/Delete/5
        [HttpPost]
        public ActionResult Delete(BranchViewModel model)
        {
            try
            {
               var entity=Mapper.Map<BranchViewModel, Branch>(model);
               entity.IsActive = false;
               entity.InActiveDate = DateTime.Now;
               entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
               entity.UpdateDate = DateTime.Now;
               branchService.Update(entity);
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
