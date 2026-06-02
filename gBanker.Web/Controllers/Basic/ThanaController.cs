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
    public class ThanaController : BaseController
    {
        //
        // GET: /Thana/
        #region variables          
        private readonly ILgThanaService thanaService;
        private readonly IStateOrProvinceService stateOrProvinceService;
        private readonly IDistrictService districtService;
        private readonly ICountryService countryService;



        public ThanaController(ILgThanaService thanaService, IDistrictService districtService, IStateOrProvinceService stateOrProvinceService, ICountryService countryService)
        {
            this.countryService = countryService;
            this.thanaService = thanaService;
            this.districtService = districtService;
            this.stateOrProvinceService = stateOrProvinceService;
        }
        #endregion

        #region methods
        private void MapDropDownList(ThanaViewModel model)
        {
            // Country Dropdown
            var countryList = countryService.GetAll();
            var viewCountryList = countryList.Select(m => new SelectListItem() { Text = m.CountryName, Value = m.CountryId.ToString() });
            model.CountryList = viewCountryList;

            //var countryList = countryService.GetAll();
            //var viewCountryList = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.CountryId.ToString(),
            //    Text = x.CountryName.ToString()
            //});

            //var country_items = new List<SelectListItem>();
            //country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //country_items.AddRange(viewCountryList);
            //model.CountryList = country_items;

            //State/Province/Division Dropdown
            var stateList = new List<SelectListItem>();
            stateList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.StateOrProvinceList = stateList;

            //District Dropdown
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            //Thana Dropdown
            //var thanaList = new List<SelectListItem>();
            //thanaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            //model.ThanaList = thanaList;
        }


        public JsonResult GetStateList(int country_id)
        {
            var stateList = stateOrProvinceService.GetMany(c => c.CountryId == country_id).OrderBy(o => o.Name).ToList();
            var viewState = stateList.Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(state_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistrictList()
        {
            var districtList = districtService.GetMany(f => f.IsActive).OrderBy(f => f.district_name_eng).ToList();
            var viewDistrict = districtList.Select(x => new SelectListItem
            {
                Value = x.district_id.ToString(),
                Text = x.district_name_eng.ToString()
            });
            var district_items = new List<SelectListItem>();
            district_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            district_items.AddRange(viewDistrict);
            return Json(district_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThanaList(int district_id)
        {
            var thanaList = thanaService.GetMany(c => c.district_id == district_id).OrderBy(f => f.thana_name_eng).ToList();

            var viewThana = thanaList.Select(x => new SelectListItem
            {
                Value = x.thana_id.ToString(),
                Text = x.thana_name_eng.ToString()
            });
            var thana_items = new List<SelectListItem>();
            thana_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            thana_items.AddRange(viewThana);

            return Json(thana_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThana(int jtStartIndex, int jtPageSize, string jtSorting, string distId, string filterColumnName, string filterValue)
        {
            try
            {
                long TotCount;
                int DistId = Convert.ToInt32(string.IsNullOrEmpty(distId) ? "0" : distId);
                var thanadetails = thanaService.GetThanaDetail(DistId, filterColumnName, filterValue, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = thanadetails.ToList();
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



        #endregion

        #region events
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["DistList"] = items;
            return View();
        }

        //
        // GET: /Thana/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Thana/Create
        public ActionResult Create()
        {
            var model = new ThanaViewModel();
            MapDropDownList(model);
            return View(model);

        }

        //
        // POST: /Thana/Create
        [HttpPost]
        public ActionResult Create(ThanaViewModel model)
        {
            var entity = Mapper.Map<ThanaViewModel, LgThana>(model);
            try
            {
                entity.thana_code = thanaService.GetNewThanaCode(Convert.ToInt32(entity.district_id));
                var errors = thanaService.IsValidLgThana(entity.thana_code);
                if (errors.ToList().Count == 0)
                {
                    //entity.Status = true;
                    thanaService.Create(entity);

                    return GetSuccessMessageResult();
                    //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }
                else
                    return GetErrorMessageResult(errors);
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Thana/Edit/5
        public ActionResult Edit(int id)
        {
            var thana = thanaService.GetById(id);
            var entity = Mapper.Map<LgThana, ThanaViewModel>(thana);
            MapDropDownList(entity);
            return View(entity);
        }

        //
        // POST: /Thana/Edit/5
        [HttpPost]
        public ActionResult Edit(ThanaViewModel model)
        {
            try
            {
                var entity = Mapper.Map<ThanaViewModel, LgThana>(model);
                var getThanaDetails = thanaService.GetById(Convert.ToInt32(entity.thana_id));
                // LgThanaService.GetByThanaID(Convert.ToInt32(entity.thana_id));

                if (ModelState.IsValid)
                {
                    //getThanaDetails.district_id  = entity.district_id;                    
                    //getThanaDetails.thana_code = entity.thana_code;
                    getThanaDetails.thana_name_eng = entity.thana_name_eng;
                    // getDistrictDetails.district_name_eng = entity.district_name_eng;

                    thanaService.Update(getThanaDetails);
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
        // GET: /Thana/Delete/5
        public ActionResult Delete(int id)
        {

            // thanaService.Delete(id);
            return RedirectToAction("Index");

            //thanaService .Inactivate(id, null);
            //return RedirectToAction("Index");
        }

        //
        // POST: /Thana/Delete/5
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
        public string filterColumnName { get; set; }

        public int startRowIndex { get; set; }

        public int pageSize { get; set; }
    }
}
