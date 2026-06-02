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
    public class DistrictController : BaseController
    {
        #region variables
        private readonly IDistrictService districtService;
        private readonly IStateOrProvinceService sateOrProvinceService;
        private readonly ICountryService countryService;
        private readonly IEmployeeService employeeService;

        public DistrictController(IDistrictService districtService, IStateOrProvinceService stateOrProvinceService, ICountryService countryService, IEmployeeService employeeService)
        {
            this.districtService = districtService;
            this.sateOrProvinceService = stateOrProvinceService;
            this.countryService = countryService;        
            this.employeeService = employeeService;
        }
        #endregion

        #region Events

        // GET: /District/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CountryList"] = items;
            ViewData["DivList"] = items;
            ViewData["DistList"] = items;
            return View();
        }

        // GET: /District/Create
        public ActionResult Create()
        {
            var model = new DistrictViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // POST: /Country/Create
        [HttpPost]
        public ActionResult Create(DistrictViewModel model)
        {
            var entity = Mapper.Map<DistrictViewModel, District>(model);
            try
            {
                entity.district_code = districtService.GetNewDistrictCode();
                var errors = districtService.IsValidDistrict(entity.district_code);
                if (errors.ToList().Count == 0)
                {
                    //entity.Status = true;
                    districtService.Create(entity);
                    return GetSuccessMessageResult();
                    // return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            if (employeeService.IsContinued(id))
            {
                var district = districtService.GetById(Convert.ToInt32(id));
                var state = sateOrProvinceService.GetById(Convert.ToInt32(district.division_Id));

                //ViewData["CountryId"] = state.CountryId.ToString();

                var entity = Mapper.Map<District, DistrictViewModel>(district);
                entity.CountrtyId = state.CountryId;
                entity.division_Id = state.StateOrProvinceId;


                MapDropDownList(entity);
                //employeeDepartmentService.GetById(Convert.ToInt32(emp.DepartmentId)).DepartmentName;
                //ViewData["EmployeeId"] = id.ToString();

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued District, please enter a diferent District id and Name.");
            return RedirectToAction("Index");
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(DistrictViewModel model)
        {
            try
            {
                var entity = Mapper.Map<DistrictViewModel, District>(model);
                var getDistrictDetails = districtService.GetById(Convert.ToInt32(entity.district_id));

                if (ModelState.IsValid)
                {
                    //getDistrictDetails.CountrtyId = entity.CountrtyId;
                    getDistrictDetails.division_Id = entity.division_Id;
                    getDistrictDetails.district_id = entity.district_id;
                    getDistrictDetails.district_code = entity.district_code;
                    getDistrictDetails.district_name_eng = entity.district_name_eng;

                    districtService.Update(getDistrictDetails);
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

  
        // GET: /District/Delete/5
        public ActionResult Delete(int id)
        {
            districtService.Delete(id);
            return RedirectToAction("Index");
        }

   
        // POST: /District/Delete/5
        [HttpPost]
        public ActionResult Delete(DistrictViewModel model)
        {
            try
            {
                //var entity = Mapper.Map<DistrictViewModel, District>(model);
                //entity.IsActive = false;
                //entity.UpdateUser = "1";
                //entity.UpdateDate = DateTime.Now;
                //districtService.Update(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ChangeLanguage(string lang)
        {
            new SiteLanguages().SetLanguage(lang);
            return RedirectToAction("Create");
        }

        public ActionResult GetDistrictList([DataSourceRequest]DataSourceRequest request)
        {
            var divisionList = sateOrProvinceService.GetMany(s => s.Status == true).ToList();
            var districtList = districtService.GetMany(d => d.IsActive == true).ToList();

            var divDistList = from dis in districtList
                              join div in divisionList on dis.division_Id equals div.StateOrProvinceId
                              select new DistrictViewModel
                              {
                                  district_id = dis.district_id,
                                  division_Id = dis.division_Id,
                                  district_name_eng = dis.district_name_eng,
                                  division_name = div.Name,
                                  district_code = dis.district_code
                              };

            var viewList = divDistList.AsEnumerable().Select((p, sl) => new DistrictViewModel()
            {
                rowSl = sl + 1,
                district_id = p.district_id,
                division_Id = p.division_Id,
                district_name_eng = p.district_name_eng,
                division_name = p.division_name,
                district_code = p.district_code
            }).ToList();
            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetCountryList()
        {

            var CountryList = countryService.GetAll();
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

        public JsonResult GetDivisionList(string country_val)
        {

            var DivisionList = sateOrProvinceService.GetAll().Where(c => c.CountryId == Convert.ToInt32(country_val));
            var viewDivision = DivisionList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString().Trim()
            });
            var division_items = new List<SelectListItem>();
            if (viewDivision.ToList().Count > 0)
            {
                division_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            division_items.AddRange(viewDivision);
            return Json(division_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSearchDistrictList(string div_val)
        {

            var DistrictList = districtService.GetAll().Where(c => c.division_Id == Convert.ToInt32(div_val));
            var viewDistrict = DistrictList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.district_id.ToString(),
                Text = x.district_code.ToString() + " " + x.district_name_eng.ToString()
            });
            var district_items = new List<SelectListItem>();
            if (viewDistrict.ToList().Count > 0)
            {
                district_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            district_items.AddRange(viewDistrict);
            return Json(district_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStateList(string country_id)
        {
            var stateList = sateOrProvinceService.GetAll().Where(c => c.CountryId == Convert.ToInt32(country_id));
            var viewState = stateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });

            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(state_items, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Methods
        private void MapDropDownList(DistrictViewModel model)
        {
            //Country Dropdown
            var countryList = countryService.GetAll();
            var viewCountryList = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });

            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountryList);
            model.CountryList = country_items;


            //Division Dropdown
            var stateList = sateOrProvinceService.GetAll().Where(c => c.CountryId == Convert.ToInt32(model.CountrtyId));
            var viewState = stateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });

            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);

            model.StateOrProvinceList = state_items;
        }
   
        #endregion
    }
}
