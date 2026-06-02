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
using gHRM.Web.ViewModels.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Service.Discipline;

namespace gHRM.Web.Controllers
{
    public class DiscCaseStatusController : BaseController
    {
        #region Variables
        private readonly IDiscCaseStatusService discCaseStatusService;
        private readonly IDiscStatusService discStatusService;
        private readonly IEmployeeSPService employeeSPService;     
        private readonly IDiscCaseDespatchNoService discCaseDespatchNoService;
        public DiscCaseStatusController(IDiscCaseStatusService discCaseStatusService, IDiscStatusService discStatusService, IEmployeeSPService employeeSPService,IDiscCaseDespatchNoService discCaseDespatchNoService)
        {
            this.discCaseStatusService = discCaseStatusService;
            this.discStatusService = discStatusService;
            this.employeeSPService = employeeSPService;
            this.discCaseDespatchNoService=discCaseDespatchNoService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(DiscCaseStatusViewModel model)
        {
            var statusList = discStatusService.GetAll();
            var statusDetails = statusList.Select(m => new SelectListItem() { Text = m.StatusMsg.ToString(), Value = m.StatusId.ToString() });
            var dicsStatusList = new List<SelectListItem>();
            dicsStatusList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            dicsStatusList.AddRange(statusDetails);
            model.StatusList = dicsStatusList;
        }
        public JsonResult GetCaseInfoByCaseNo(string case_no,string DespatchNo)
        {
            try
            {

                    string CaseNo = "";
                    if (DespatchNo != "" && DespatchNo != null)
                    {
                        var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == DespatchNo && x.IsActive == true);

                        foreach (var r in master)
                        {
                            CaseNo = (string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()).ToString(); ;//MasterId = Case No
                        }
                    }
                  else
                    {
                        CaseNo = case_no;
                    }

                List<CaseEntryViewModel> List_CaseViewModel = new List<CaseEntryViewModel>();
                var param = new { CaseNo = CaseNo };
                var crimeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GET_Disc_CaseInfoByCaseNo");

                List_CaseViewModel = crimeList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseDateToMsg = row.Field<string>("CaseDateTo"),
                    CaseType = row.Field<string>("CaseType"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),
                    //StatusDtMsg = row.Field<string>("StatusDt"),
                    //StatusMsg = row.Field<string>("StatusMsg"),
                    CrimeName = row.Field<string>("CrimeName"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFrom"),
                    CrimeDateToMsg = row.Field<string>("CrimeDateTo"),
                    AnnexationAmountMsg = row.Field<string>("AnnexationAmount"),
                    DispatchNo = row.Field<string>("DispatchNo") 

                }).ToList();

                return Json(List_CaseViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCasewiseCrimeList(int jtStartIndex, int jtPageSize, string jtSorting, string case_no, string DespatchNo)
        {
            try
            {

                 string CaseNo = "";
                    if (DespatchNo != "" && DespatchNo != null)
                    {
                        var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == DespatchNo && x.IsActive == true);

                        foreach (var r in master)
                        {
                            CaseNo = (string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()).ToString(); ;//MasterId = Case No
                        }
                    }
                  else
                    {
                        CaseNo = case_no;
                    }


                List<CaseEntryViewModel> List_CaseViewModel = new List<CaseEntryViewModel>();
                var param = new { CaseNo = CaseNo };
                var crimeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GET_Disc_CaseInfoByCaseNo");

                List_CaseViewModel = crimeList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseDateToMsg = row.Field<string>("CaseDateto"),
                    CaseType = row.Field<string>("CaseType"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    CrimeName = row.Field<string>("CrimeName"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFrom"),
                    CrimeDateToMsg = row.Field<string>("CrimeDateTo"),
                    AnnexationAmountMsg = row.Field<string>("AnnexationAmount")
                }).ToList();

                var currentPageRecords = List_CaseViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_CaseViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetCasewiseStatus(int jtStartIndex, int jtPageSize, string jtSorting, string case_no, string DespatchNo)
        {
            try
            {

                string CaseNo = "";
                if (DespatchNo != "" && DespatchNo != null)
                {
                    var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == DespatchNo && x.IsActive == true);

                    foreach (var r in master)
                    {
                        CaseNo = (string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()).ToString(); ;//MasterId = Case No
                    }
                }
                else
                {
                    CaseNo = case_no;
                }

                List<CaseEntryViewModel> List_CaseViewModel = new List<CaseEntryViewModel>();
                var param = new { CaseNo = CaseNo };
                var crimeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GET_Disc_CaseStatusByCaseNo");

                List_CaseViewModel = crimeList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseStatusId = row.Field<long>("CaseStatusId"),
                    StatusMsg = row.Field<string>("StatusMsg"),
                    StatusDtMsg = row.Field<string>("StatusDt")                    
                }).ToList();

                var currentPageRecords = List_CaseViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_CaseViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult StatusDelete(string CaseStatusId)
        {
            var entity = discCaseStatusService.GetByDiscCaseId(Convert.ToInt64(CaseStatusId));
            string Result = "OK";
            if (ModelState.IsValid)
            {
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                discCaseStatusService.Update(entity);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            DiscCaseStatusViewModel model = new DiscCaseStatusViewModel();
            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DiscCaseStatusViewModel model, string CaseMasterId)
        {
            var entity = Mapper.Map<DiscCaseStatusViewModel, DiscCaseStatu>(model);
            try
            {
                if (ModelState.IsValid)
                {
                    if (CaseMasterId != "" && CaseMasterId != "0")
                        entity.CaseMasterId = Convert.ToInt32(CaseMasterId);
                    entity.StatusId = model.StatusId;
                    entity.StatusDt = model.StatusDt;
                    entity.IsActive = true;
                    entity.CreateUser = LoggedInEmployeeId;
                    entity.CreateDate = DateTime.Now;
                    discCaseStatusService.Create(entity);
                    //return GetSuccessMessageResult();
                    return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
                //return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
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
