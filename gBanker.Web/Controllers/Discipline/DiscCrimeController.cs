using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using gHRM.Service.Discipline;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;
using System.Collections.Generic;

namespace gHRM.Web.Controllers
{

    public class DiscCrimeController : BaseController
    {
        #region Variables

        private readonly IDisCrimeService disCrimeService;

        public DiscCrimeController(IDisCrimeService disCrimeService)
        {
            this.disCrimeService = disCrimeService;          
           
        }
        private void MapDropDownFrodiscCrime(DiscCrimeViewModel model)
        {
            var crimeType = new List<SelectListItem>();
            crimeType.Add(new SelectListItem() { Text = "Financial", Value = "1", Selected = true });
            crimeType.Add(new SelectListItem() { Text = "Non Financial", Value = "2" });
            model.CrimeTypeList = crimeType;
        }
        #endregion

        #region Methods
        public JsonResult GetCrimeList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                var CrimeDetail = disCrimeService.GetDiscCrimeDetail(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = CrimeDetail.ToList();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion

        #region Events
        // GET: /DiscCrime/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DiscCrime/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DiscCrime/Create
        public ActionResult Create()
        {
            var model = new DiscCrimeViewModel();
            MapDropDownFrodiscCrime(model);
            return View(model);
        }

        //
        // POST: /DiscCrime/Create
        [HttpPost]
        public ActionResult Create(DiscCrimeViewModel model)
        {
            try
            {
                var entity = Mapper.Map<DiscCrimeViewModel, DiscCrime>(model);
                if (ModelState.IsValid)
                {
                    var errors = disCrimeService.IsValidCrime(entity.CrimeCode);
                    //{
                    if (errors.ToList().Count == 0)
                    {
                        entity.IsActive = true;
                        entity.InActiveDate = DateTime.Now;
                        disCrimeService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                else
                    return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /DiscCrime/Edit/5
        public ActionResult Edit(int id)
        {

            var crime = disCrimeService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<DiscCrime, DiscCrimeViewModel>(crime);
            MapDropDownFrodiscCrime(entity);
            return View(entity);    
        }

        //
        // POST: /DiscCrime/Edit/5
        [HttpPost]
        public ActionResult Edit(DiscCrimeViewModel model)
        {
            try
            {

                var entity = Mapper.Map<DiscCrimeViewModel, DiscCrime>(model);
                var getCrimeDetails = disCrimeService.GetById(Convert.ToInt32(entity.CrimeId));
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getCrimeDetails.CrimeType = entity.CrimeType;
                    getCrimeDetails.CrimeName = entity.CrimeName;
                    getCrimeDetails.Remarks = entity.Remarks;
                   // getCrimeDetails.CrimeCode = entity.CrimeCode;
                    disCrimeService.Update(getCrimeDetails);
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
        // GET: /DiscCrime/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscCrime/Delete/5
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
