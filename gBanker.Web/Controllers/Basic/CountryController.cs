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
    public class CountryController : BaseController
    {
        #region variables
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;            
        }
        #endregion

        #region Methods
        public JsonResult GetCountries(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allcountries = countryService.SearchCountry();
                var totalCount = allcountries.Count();
                var entities = allcountries.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Country>, IEnumerable<CountryViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var entities = Mapper.Map<IEnumerable<Investor>, IEnumerable<InvestorViewModel>>(allinvestors);
                //return Json(new { Result = "OK", Records = entities });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        #endregion

        #region Events
        //
        // GET: /Country/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Country/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Country/Create
        public ActionResult Create()
        {
            var model = new CountryViewModel();            
            return View(model);
        }

        //
        // POST: /Country/Create
        [HttpPost]
        public ActionResult Create(CountryViewModel model)
        {
            var entity = Mapper.Map<CountryViewModel, Country>(model);
            try
            {
                if (ModelState.IsValid)
                {
                    var errors = countryService.IsValidCountry(entity.CountryShortCode);
                    if (errors.ToList().Count == 0)
                    {
                        entity.Status = true;
                        countryService.Create(entity);
                        return GetSuccessMessageResult();
                        //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelErrors(errors);
                        return GetErrorMessageResult(errors);
                        //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                    }
                }                
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Country/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Country/Edit/5
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
        // GET: /Country/Delete/5
        public ActionResult Delete(int id)
        {
            countryService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        //
        // POST: /Country/Delete/5
        [HttpPost]
        public ActionResult Delete(CountryViewModel model)
        {
            try
            {
                //var entity = Mapper.Map<CountryViewModel, Country>(model);
                //entity.IsActive = false;
                //entity.UpdateUser = "1";
                //entity.UpdateDate = DateTime.Now;
                //countryService.Update(entity);
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
