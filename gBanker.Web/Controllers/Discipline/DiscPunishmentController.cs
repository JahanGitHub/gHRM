using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Service.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Web.Controllers
{
    public class DiscPunishmentController : BaseController
    {
        #region Variables

        private readonly IDiscPunishmentService discPunishmentService;

        public DiscPunishmentController(IDiscPunishmentService discPunishmentService)
        {
            this.discPunishmentService = discPunishmentService;
           
        }
        #endregion

        #region Methods
        public JsonResult GetPunishmentList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                var PunishmentDetail = discPunishmentService.GetDiscPunishmentDetail(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = PunishmentDetail.ToList();
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
        //
        // GET: /DiscPunishment/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DiscPunishment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DiscPunishment/Create
        public ActionResult Create()
        {
            var model = new DiscPunishmentViewModel();
            return View(model);
        }

        //
        // POST: /DiscPunishment/Create
        [HttpPost]
        public ActionResult Create(DiscPunishmentViewModel model)
        {
            try
            {
                var entity = Mapper.Map<DiscPunishmentViewModel, DiscPunishment>(model);
                if (ModelState.IsValid)
                {
                    var errors = discPunishmentService.IsValidPunishment(entity.PunishmentCode);
                    //{
                    if (errors.ToList().Count == 0)
                    {
                        entity.IsActive = true;
                        entity.InActiveDate = DateTime.Now;
                        discPunishmentService.Create(entity);
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
        // GET: /DiscPunishment/Edit/5
        public ActionResult Edit(int id)
        {
            var punishment = discPunishmentService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<DiscPunishment, DiscPunishmentViewModel>(punishment);
            return View(entity);    
        }

        //
        // POST: /DiscPunishment/Edit/5
        [HttpPost]
        public ActionResult Edit(DiscPunishmentViewModel model)
        {
            try
            {

                var entity = Mapper.Map<DiscPunishmentViewModel, DiscPunishment>(model);
                var getPunishmentDetails = discPunishmentService.GetById(Convert.ToInt32(entity.PunishmentId));
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getPunishmentDetails.PunishmentName = entity.PunishmentName;
                    getPunishmentDetails.Remarks = entity.Remarks;
                    // getCrimeDetails.CrimeCode = entity.CrimeCode;
                    discPunishmentService.Update(getPunishmentDetails);
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
        // GET: /DiscPunishment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscPunishment/Delete/5
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
