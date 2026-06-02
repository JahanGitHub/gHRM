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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;



namespace gHRM.Web.Controllers
{
    public class StateOrProvinceController :BaseController
    {
      
        #region variables
        private readonly IEmployeeService employeeService; 
        private readonly ICountryService countryService;
        private readonly IStateOrProvinceService stateOrProvinceService;
        public StateOrProvinceController(IStateOrProvinceService stateOrProvinceService, ICountryService countryService, IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
            this.countryService = countryService;
            this.stateOrProvinceService = stateOrProvinceService;
        }
        #endregion

   

        #region Events
        //
        // GET: /StateOrProvince/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CountryList"] = items;
            ViewData["StateList"] = items;
            return View();
        }

        //
        // GET: /StateOrProvince/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        //
        // GET: /StateOrProvince/Create
        public ActionResult Create()
        {
            var model = new StateOrProvinceViewModel();
            MapDropDownList(model);
            return View(model);
           
        }

        //
        // POST: /StateOrProvince/Create
        [HttpPost]
        public ActionResult Create(StateOrProvinceViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                var entity = Mapper.Map<StateOrProvinceViewModel, StateOrProvince>(model);
                try
                {
                    var errors = stateOrProvinceService.IsValidStateOrProvince(entity.Code);
                    if (errors.ToList().Count == 0)
                    {
                        //entity.Code = GetNewEmployeeCode();
                        entity.Status = true;
                        stateOrProvinceService.Create(entity);
                        return GetSuccessMessageResult();
                        //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return GetSuccessMessageResult();
                        //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return GetSuccessMessageResult();
                   // return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }

                return GetSuccessMessageResult();
            }
            catch
            {
                return GetSuccessMessageResult();
            }
        }

        //
        // GET: /StateOrProvince/Edit/5
        public ActionResult Edit(int id)
        {
            if (employeeService.IsContinued(id))
            {
                var stateorprovince = stateOrProvinceService.GetById(id);
                var entity = Mapper.Map<StateOrProvince, StateOrProvinceViewModel>(stateorprovince);
                MapDropDownList(entity);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Duplicate State Or province Or Division, please enter a diferent State Or province Or Division id and name.");
            return RedirectToAction("Index");
            
        }

        //
        // POST: /StateOrProvince/Edit/5
        [HttpPost]
        public ActionResult Edit(StateOrProvinceViewModel model)
        {
            try
            {

                var entity = Mapper.Map<StateOrProvinceViewModel, StateOrProvince>(model);
                var getStateOrProvinceDetails = stateOrProvinceService.GetById(Convert.ToInt32(entity.StateOrProvinceId));
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getStateOrProvinceDetails.CountryId = entity.CountryId;
                    getStateOrProvinceDetails.Code = entity.Code;
                    getStateOrProvinceDetails.Name = entity.Name;
                    //getEmployeeBasicDetails.EmployeeId;
                    stateOrProvinceService.Update(getStateOrProvinceDetails);
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
        // GET: /StateOrProvince/Delete/5
        public ActionResult Delete(int id)
        {
            stateOrProvinceService.Delete(id);
            return RedirectToAction("Index");
           
        }

        //
        // POST: /StateOrProvince/Delete/5
        [HttpPost]
        public ActionResult Delete(StateOrProvinceViewModel model)
        {
            try
            {
             //  var entity=Mapper.Map<StateOrProvinceViewModel, StateOrProvince>(model);
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion


        #region HttpRequests
        public ActionResult GetStates([DataSourceRequest]DataSourceRequest request)
        {

            var DivisionList = stateOrProvinceService.GetMany(p => p.Status == true).ToList();
            var CountryList = countryService.GetMany(c => c.Status == true).ToList();

            var countryDivisionList = from div in DivisionList
                                      join co in CountryList on div.CountryId equals co.CountryId
                                      select new StateOrProvinceViewModel
                                      {
                                          StateOrProvinceId = div.StateOrProvinceId,
                                          CountryId = div.CountryId,
                                          CountryName = co.CountryName,
                                          Name = div.Name,
                                          Code = div.Code
                                      };

            var viewList = countryDivisionList.AsEnumerable().Select((p, sl) => new StateOrProvinceViewModel()
            {
                rowSl = sl + 1,
                StateOrProvinceId = p.StateOrProvinceId,
                CountryId = p.CountryId,
                CountryName = p.CountryName,
                Name = p.Name,
                Code = p.Code
            }).ToList();
            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult GetStates(int jtStartIndex, int jtPageSize, string jtSorting, string cotryId, string statProId)
        //{
        //    try
        //    {
        //        long TotCount;
        //        int StateOrProvinceId = Convert.ToInt32(string.IsNullOrEmpty(statProId) ? "0" : statProId);
        //        int CountryId = Convert.ToInt32(string.IsNullOrEmpty(cotryId) ? "0" : cotryId);
        //        var stateorProvinceDetail = stateOrProvinceService.GetStateOrProvinceOrDivisionDetail(CountryId, StateOrProvinceId, jtStartIndex, jtSorting, jtPageSize, out TotCount);

        //        var detail = stateorProvinceDetail.ToList();
        //        var currentPageRecords = detail.ToList();
        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}
        public JsonResult GetCountryList()
        {

            var CountryList = countryService.GetAll().Where(x => x.CountryId == CountryID);
            //var CountryList = countryService.GetAll();
            var viewCountry = CountryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            if (viewCountry.ToList().Count > 0)
            {
                country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            country_items.AddRange(viewCountry);
            return Json(country_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStateOrProvinceSearch(string country_val)
        {

            var StateList = stateOrProvinceService.GetAll().Where(c => c.CountryId == Convert.ToInt32(country_val));
            var viewState = StateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString().Trim()
            });
            var state_items = new List<SelectListItem>();
            if (viewState.ToList().Count > 0)
            {
                state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            state_items.AddRange(viewState);
            return Json(state_items, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetStates(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue)
        //{
        //    try
        //    {
        //        long TotCount;
        //        var allstates = stateOrProvinceService.GetStateOrProvinceOrDivisionDetail( filterColumnName, filterValue, startRowIndex, jtSorting,  pageSize, out TotCount);
        //        var totalCount = allstates.Count();
        //        var entities = allstates.Skip(jtStartIndex).Take(jtPageSize);
        //        var currentPageRecords = Mapper.Map<IEnumerable<StateOrProvince>, IEnumerable<StateOrProvinceViewModel>>(entities);

        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

        //        //var entities = Mapper.Map<IEnumerable<Investor>, IEnumerable<InvestorViewModel>>(allinvestors);
        //        return Json(new { Result = "OK", Records = entities });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public JsonResult GetStateList(string countryId)
        {
            var stateList = countryService.GetAll().Where(w => w.CountryId == Convert.ToInt32(countryId));
            var viewState = stateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(state_items, JsonRequestBehavior.AllowGet);
        }

        private void MapDropDownList(StateOrProvinceViewModel model)
        {
            //Country DropDownList
            var stateList = countryService.GetAll().Where(w => w.CountryId == CountryID); ;
            var viewState = stateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            model.CountryList = state_items;
        }

        #endregion

        //public string filterColumnName { get; set; }

        //public int startRowIndex { get; set; }

        //public int pageSize { get; set; }
    }
}
