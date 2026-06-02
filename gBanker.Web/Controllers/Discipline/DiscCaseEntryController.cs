using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Service;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using System.Text;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Service.Discipline;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{

    public class DiscCaseEntryController : BaseController
    {

        #region Variables
        private readonly IDisCrimeService discCrimeService;
        private readonly IDiscCaseDetailService discCaseDetailService;
        private readonly IDiscCaseMasterService discCaseMasterService;
        private readonly IDiscDealingOfficerService discDealingOfficerService;
        private readonly IDiscEnqueryOfficerService discEnqueryOfficerService;
        private readonly IDiscCaseDealingOfficerService discCaseDealingOfficerService;
        private readonly IDiscCaseEnquiryOfficerService discCaseEnquiryOfficerService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IDiscCaseAnnexationService discCaseAnnexationService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IDiscStatusService discStatusService;
        private readonly IDiscPunishmentService discPunishmentService;
        private readonly IDiscCaseDespatchNoService discCaseDespatchNoService;
        private readonly IDiscEmbezzleService discEmbezzleService;
        private readonly IDiscCaseCrimeLocationService discCaseCrimeLocationService;
        private readonly IDiscCasePunishmentMasterService discCasePunishmentMasterService;
        private readonly IDiscCasePunishmentDetailService discCasePunishmentDetailService;

        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;


        public DiscCaseEntryController(IDisCrimeService discCrimeService, IDiscCaseDetailService discCaseDetailService, IDiscCaseMasterService discCaseMasterService, IOfficeService officeService, IEmployeeService employeeService, IEmployeeDesignationService employeeDesignationService, IOfficeTypeService officeTypeService, IDiscDealingOfficerService discDealingOfficerService, IDiscEnqueryOfficerService discEnqueryOfficerService, IDiscCaseAnnexationService discCaseAnnexationService, IEmployeeSPService employeeSPService, IDiscStatusService discStatusService, IDiscPunishmentService discPunishmentService, IDiscCaseDealingOfficerService discCaseDealingOfficerService, IDiscCaseEnquiryOfficerService discCaseEnquiryOfficerService, IDiscCaseDespatchNoService discCaseDespatchNoService, IDiscEmbezzleService discEmbezzleService, IDiscCaseCrimeLocationService discCaseCrimeLocationService, IDiscCasePunishmentMasterService discCasePunishmentMasterService, IDiscCasePunishmentDetailService discCasePunishmentDetailService)
        {
            this.discCrimeService = discCrimeService;
            this.discCaseDetailService = discCaseDetailService;
            this.discCaseMasterService = discCaseMasterService;
            this.discCaseDealingOfficerService = discCaseDealingOfficerService;
            this.officeService = officeService;
            this.employeeService = employeeService;
            this.employeeDesignationService = employeeDesignationService;
            this.officeTypeService = officeTypeService;
            this.discDealingOfficerService = discDealingOfficerService;
            this.discEnqueryOfficerService = discEnqueryOfficerService;
            this.discCaseAnnexationService = discCaseAnnexationService;
            this.employeeSPService = employeeSPService;
            this.discStatusService = discStatusService;
            this.discPunishmentService = discPunishmentService;
            this.discCaseEnquiryOfficerService = discCaseEnquiryOfficerService;
            this.discCaseDespatchNoService = discCaseDespatchNoService;
            this.discEmbezzleService = discEmbezzleService;
            this.discCaseCrimeLocationService = discCaseCrimeLocationService;
            this.discCasePunishmentMasterService = discCasePunishmentMasterService;
            this.discCasePunishmentDetailService = discCasePunishmentDetailService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ZOOfficeList"] = items;
            ViewData["CrimeList"] = items;
            ViewData["StatusList"] = items;
            ViewData["AOOfficeList"] = items;

            ViewBag.LoggedInOfficeID = LoggedInOfficeID;

            return View();
        }


        public ActionResult PunishmentIndex()
        {
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["ZOOfficeList"] = items;
            //ViewData["CrimeList"] = items;
            //ViewData["StatusList"] = items;

            return View();
        }

        public ActionResult DisciplineLetter()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            CaseEntryViewModel model = new CaseEntryViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            MapDropDownList(model);

            ViewData["OfficeId"] = items;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            ViewData["EmployeeList"] = items;

            return View(model);
        }

        public ActionResult ChargeSheetCreate()
        {
            CaseEntryViewModel model = new CaseEntryViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            MapDropDownList(model);

            ViewData["OfficeId"] = items;
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["EmployeeList"] = items;

            return View(model);
        }

        public ActionResult PunishmentEntry()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["PunishmentList"] = items;
            return View();
        }

        public ActionResult CaseEdit(int id)
        {
            var caseEntry = discCaseMasterService.GetById(id);
            var entity = Mapper.Map<DiscCaseMaster, CaseEntryViewModel>(caseEntry);


            entity.CaseNo = caseEntry.CaseNo;
            entity.currentDate = caseEntry.CaseDateFrom;
            entity.AuditFromMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditFrom);
            entity.AuditToMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditTo);
            entity.DealOfficerId = caseEntry.DealOfficerId;
            entity.CaseDescription = caseEntry.CaseDescription;

            entity.CrimeLocation = caseEntry.CrimeLocation;
            MapDropDownList(entity);
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeId"] = items;
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["EmployeeList"] = items;
            return View(entity);
        }

        public ActionResult ChargeSheetEdit(int id)
        {
            var caseEntry = discCaseMasterService.GetById(id);
            var entity = Mapper.Map<DiscCaseMaster, CaseEntryViewModel>(caseEntry);

            entity.CaseNo = caseEntry.CaseNo;
            entity.currentDate = caseEntry.CaseDateFrom;
            entity.AuditFromMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditFrom);
            entity.AuditToMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditTo);
            entity.DealOfficerId = caseEntry.DealOfficerId;
            entity.CaseDescription = caseEntry.CaseDescription;

            entity.CrimeLocation = caseEntry.CrimeLocation;
            MapDropDownList(entity);
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeId"] = items;
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["EmployeeList"] = items;
            return View(entity);
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
        public ActionResult EnquiryOfficer(int id)
        {

            DiscCaseEnquiryOfficerViewModel model = new DiscCaseEnquiryOfficerViewModel();
            var Case = discCaseMasterService.GetById(id);
            var DespatchNo = discCaseDespatchNoService.GetAll().Where(m=>m.CaseMasterId == id).Select(m=>m.DespatchNo).FirstOrDefault();
            model.CaseMasterId = id;
            model.CaseNo = Case.CaseNo;
            model.DespatchNo = DespatchNo;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EnqueryOfficerList"] = items;

            return View(model);
        }
        [HttpPost]
        public ActionResult EnquiryOfficer(int id, FormCollection collection)
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

        public ActionResult CaseEntryShortForm()
        {
            try
            {
                CaseEntryViewModel model = new CaseEntryViewModel();//
                MapDropDownList(model);
                IEnumerable<SelectListItem> items = new SelectList(" ");

                ViewData["PunishmentList"] = items;
                //ViewData["HOList"] = items;
                ViewData["ZOOfficeList"] = items;
                ViewData["ZAOOfficeList"] = items;
                //ViewData["AOOfficeList"] = items;
                //ViewData["BOOfficeList"] = items;
                ViewData["OtherOfficeList"] = items;

                ViewData["OfficeId"] = items;

                model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
                model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
                model.AreaList = commonDynamicDropDown.ddlInitial();
                model.UnitList = commonDynamicDropDown.ddlInitial();

                ViewData["EmployeeList"] = items;

                return View(model);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CaseEntryShortFormEDIT()
        {
            try
            {
                CaseEntryViewModel model = new CaseEntryViewModel();//
                MapDropDownList(model);
                IEnumerable<SelectListItem> items = new SelectList(" ");

                ViewData["PunishmentList"] = items;
                ViewData["HOList"] = items;
                ViewData["ZOOfficeList"] = items;
                ViewData["ZAOOfficeList"] = items;
                ViewData["AOOfficeList"] = items;
                ViewData["BOOfficeList"] = items;
                ViewData["OtherOfficeList"] = items;

                return View(model);
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

        //
        // POST: /CaseEntry/Delete/5
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



        public ActionResult CreateEmbezzal(int CaseMasterId)
        {
            var Embleze = discEmbezzleService.GetAll().Where(e => e.IsActive == true && e.CaseMasterId == Convert.ToInt32(CaseMasterId));

            if (Embleze.Count() >= 1)
            {
                var EmblezzId = 0;
                foreach (var r in Embleze)
                {
                    EmblezzId = r.EmbezzleId;
                }
                var Embleze_Case = discEmbezzleService.GetById(EmblezzId);
                DiscEmbezzleInfoViewModel model = new DiscEmbezzleInfoViewModel();


                model.EmbMode = "U";
                model.AuditDateFrom = Embleze_Case.AuditDateFrom;
                model.AuditDateTo = Embleze_Case.AuditDateTo;
                model.BranchAuditNo = Embleze_Case.BranchAuditNo;
                model.EmbezzleId = Embleze_Case.EmbezzleId;
                model.EmbezzleRcvDt = Embleze_Case.EmbezzleRcvDt;
                model.ExplonatoryNo = discCaseMasterService.GetById(Convert.ToInt32(CaseMasterId)).CaseNo;
                model.NoOfBMAccused = Embleze_Case.NoOfBMAccused;
                model.NoOfCMAccussed = Embleze_Case.NoOfCMAccussed;
                model.NoOfSignatoryAccussed = Embleze_Case.NoOfSignatoryAccussed;
                model.OfficeId = Embleze_Case.OfficeId;
                model.Remarks = Embleze_Case.Remarks;
                model.TotEmbezzledAmount = Embleze_Case.TotEmbezzledAmount;
                model.TotReturnAmount = Embleze_Case.TotReturnAmount;

                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["OfficeId"] = items;
                ViewData["HOList"] = items;
                ViewData["ZOOfficeList"] = items;
                ViewData["ZAOOfficeList"] = items;
                ViewData["AOOfficeList"] = items;
                ViewData["BOOfficeList"] = items;
                return View(model);
            }
            else
            {
                DiscEmbezzleInfoViewModel model = new DiscEmbezzleInfoViewModel();
                model.ExplonatoryNo = discCaseMasterService.GetById(Convert.ToInt32(CaseMasterId)).CaseNo;
                model.EmbMode = "S";

                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["OfficeId"] = items;
                ViewData["HOList"] = items;
                ViewData["ZOOfficeList"] = items;
                ViewData["ZAOOfficeList"] = items;
                ViewData["AOOfficeList"] = items;
                ViewData["BOOfficeList"] = items;

                return View(model);
            }
        }

        public ActionResult EditDisciplineLetter(string CaseMasterId)
        {

            ViewData["CaseMasterId"] = CaseMasterId;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["AnulipiText"] = items;
            ViewData["DepartmentNameList"] = items;
            ViewData["ZonalOfficeList"] = items;
            ViewData["ZonalAuditList"] = items;
            ViewData["ZOOfficeListBn"] = items;
            ViewData["AOOfficeListBn"] = items;
            ViewData["BOOfficeListBn"] = items;


            return View();
        }

        #endregion

        #region Methods


        private void MapDropDownList(CaseEntryViewModel model)
        {
            var crimeList = discCrimeService.GetAll().OrderBy(c => c.SortOrder);
            var crimeListDetails = crimeList.Select(m => new SelectListItem() { Text = string.Format("{0}  {1}", m.CrimeCode, m.CrimeName), Value = m.CrimeId.ToString() });
            var dicsCrimeList = new List<SelectListItem>();
            dicsCrimeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            dicsCrimeList.AddRange(crimeListDetails);
            model.CrimeList = dicsCrimeList;

            var dealingList = discDealingOfficerService.GetEmployeeByOfficeId(Convert.ToInt32(LoggedInOfficeID));
            var dealorDetails = dealingList.Select(m => new SelectListItem() { Text = string.Format("{0}-{1}", m.EmployeeCode, m.EmployeeName), Value = m.EmployeeId.ToString() });
            var discDealingLists = new List<SelectListItem>();
            discDealingLists.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            discDealingLists.AddRange(dealorDetails);
            model.DealOfficerList = discDealingLists;

            var enqueryList = discEnqueryOfficerService.GetEmployeeByOfficeId(Convert.ToInt32(LoggedInOfficeID));
            var enqueryorDetails = enqueryList.Select(m => new SelectListItem() { Text = string.Format("{0}-{1}", m.EmployeeCode, m.EmployeeName), Value = m.EmployeeId.ToString() });
            var discEnqueryLists = new List<SelectListItem>();
            discEnqueryLists.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            discEnqueryLists.AddRange(enqueryorDetails);
            model.EnqueryOfficerList = discEnqueryLists;
        }


        #endregion

        #region ActionResult 

        public ActionResult FNGenerateLetter(string CaseMasterId, string CaseType)
        {
            try
            {
                var param = new { CaseMasterId = CaseMasterId };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_GetPunishmentwiseEmployeeListForLetter");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Disciplinary/Rpt_Disc_Letter.rpt", OverdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EnquiryOfficerSave(string EnqueryofficerId, string EnqOfficerAssignedDt, string InvestigationDt, string ReportReceivedDt, string EnquiryRemarks, string DespatchNo, string masterId)
        {
            try
            {
                DiscCaseEnquiryOfficer EnquiryOfficer = new DiscCaseEnquiryOfficer();

                EnquiryOfficer.EmployeeId = Convert.ToInt64(EnqueryofficerId);
                EnquiryOfficer.CaseMasterId = Convert.ToInt32(masterId);
                if (EnqOfficerAssignedDt != "")
                    EnquiryOfficer.EnquiryOfficerAssignedDt = Convert.ToDateTime(EnqOfficerAssignedDt);
                if (InvestigationDt != "")
                    EnquiryOfficer.InvestigationDt = Convert.ToDateTime(InvestigationDt);
                if (ReportReceivedDt != "")
                    EnquiryOfficer.ReportReceivedDt = Convert.ToDateTime(ReportReceivedDt);
                EnquiryOfficer.EnquiryRemarks = EnquiryRemarks;
                //EnquiryOfficer.DespatchNo = DespatchNo + "-" + DateTime.Now.Year.ToString();
                EnquiryOfficer.DespatchNo = DespatchNo;

                EnquiryOfficer.IsActive = true;
                EnquiryOfficer.CreateDate = DateTime.Now;
                EnquiryOfficer.CreateUser = SessionHelper.LoggedInEmployeeID;

                discCaseEnquiryOfficerService.Create(EnquiryOfficer);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditCase(string masterId, string CaseDates, string AuditFrom, string AuditTo, string CaseDescriptions, List<string> allCrimeLocations, Dictionary<string, string> SaveModes, List<string> SNo, Dictionary<string, string> CaseDetailIds, Dictionary<string, string> AllAnnexationIds, Dictionary<string, string> allEmoloyeeIds, Dictionary<string, string> allCrimeDates, Dictionary<string, string> allCrimeDatesTo, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allAnnexationAmts, Dictionary<string, string> allIndiReturnAmounts, Dictionary<string, string> alltxtIndiReturnNoticeDates, Dictionary<string, string> allTotalAnnexationAmts, Dictionary<string, string> allTotReturnAmount, Dictionary<string, string> allReturnNoticeDates, Dictionary<string, string> allNewSls, string CaseNos, Dictionary<string, string> allDispatchNos, Dictionary<string, string> allDesRemarks, List<string> allDealingOfficerIds)
        {
            try
            {   //Case Master Edit   SaveModes

                var Master_Id = Convert.ToInt32(masterId);
                //  long annexId = 0;
                var currDate = DateTime.Now;
                var master = discCaseMasterService.GetById(Master_Id);
                if (CaseDates != "")
                {
                    master.CaseDateFrom = Convert.ToDateTime(CaseDates);
                }
                if (AuditFrom != "")
                {
                    master.AuditFrom = Convert.ToDateTime(AuditFrom);
                }
                if (AuditTo != "")
                {
                    master.AuditTo = Convert.ToDateTime(AuditTo);
                }

                master.CaseDescription = CaseDescriptions;
                master.CrimeLocation = Convert.ToInt32(string.IsNullOrEmpty(allCrimeLocations.First().ToString()) ? "0" : allCrimeLocations.First().ToString());//Convert.ToInt32(string.IsNullOrEmpty(CrimeLocations) ? "0" : CrimeLocations);
                master.UpdateUser = SessionHelper.LoggedInEmployeeID;
                master.UpdateDate = DateTime.Now;
                if (allDealingOfficerIds != null)
                {
                    master.DealOfficerId = Convert.ToInt64(string.IsNullOrEmpty(allDealingOfficerIds.First()) ? "0" : allDealingOfficerIds.First());
                }

                discCaseMasterService.Update(master);

                var annexId = discCaseAnnexationService.GetAll().Where(x => x.CaseMasterId == Master_Id && x.IsActive == true).First().AnnexationId;

                if (allDealingOfficerIds != null)
                {
                    foreach (var id in allDealingOfficerIds)
                    {
                        DiscCaseDealingOfficer dofficer = new DiscCaseDealingOfficer();

                        dofficer.CaseMasterId = Master_Id;
                        dofficer.EmployeeId = Convert.ToInt64(id);
                        dofficer.IsActive = true;
                        dofficer.InActiveDate = DateTime.Now;
                        dofficer.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dofficer.CreateDate = DateTime.Now;
                        discCaseDealingOfficerService.Create(dofficer);
                    }
                }

                foreach (var r in SNo)
                {

                    var Annex = 1;
                    var deatail_Id = 0;
                    var annexation_Id = 0;
                    var employee_Id = 0;
                    var crime_Id = 0;
                    // DateTime? crimeDate_From;// = DateTime.Now;
                    // DateTime? crimeDate_to;// = DateTime.Now;
                    string crime_Date_From = string.Empty;
                    string crime_Date_to = string.Empty;
                    var annexation_Am = 0;
                    var return_Am = 0;
                    string return_Date = string.Empty;
                    string ditch_No = string.Empty;
                    string remarktxt = string.Empty;
                    string deMode = string.Empty;

                    var Ind_AnnexAm = 0;
                    var Ind_AnnexAmReturn = 0;
                    string Ind_ReturnNoticeDate = string.Empty;

                    if (CaseDetailIds.ContainsKey(r))
                        int.TryParse(CaseDetailIds[r], out deatail_Id);

                    if (AllAnnexationIds.ContainsKey(r))
                        int.TryParse(AllAnnexationIds[r], out annexation_Id);

                    if (allEmoloyeeIds.ContainsKey(r))
                        int.TryParse(allEmoloyeeIds[r], out employee_Id);

                    if (allCrimeIds.ContainsKey(r))
                        int.TryParse(allCrimeIds[r], out crime_Id);

                    if (allCrimeDates.ContainsKey(r))
                        crime_Date_From = allCrimeDates[r];

                    if (allCrimeDatesTo.ContainsKey(r))
                        crime_Date_to = allCrimeDatesTo[r];


                    if (allTotalAnnexationAmts.ContainsKey(r))
                        int.TryParse(allTotalAnnexationAmts[r], out annexation_Am);

                    if (allTotReturnAmount.ContainsKey(r))
                        int.TryParse(allTotReturnAmount[r], out return_Am);

                    if (allReturnNoticeDates.ContainsKey(r))
                        return_Date = allReturnNoticeDates[r];

                    if (allDispatchNos.ContainsKey(r))
                        ditch_No = allDispatchNos[r];

                    if (allDesRemarks.ContainsKey(r))
                        remarktxt = allDesRemarks[r];

                    if (SaveModes.ContainsKey(r))
                        deMode = SaveModes[r];

                    if (allAnnexationAmts.ContainsKey(r))
                        int.TryParse(allAnnexationAmts[r], out Ind_AnnexAm);

                    if (allIndiReturnAmounts.ContainsKey(r))
                        int.TryParse(allIndiReturnAmounts[r], out Ind_AnnexAmReturn);

                    if (alltxtIndiReturnNoticeDates.ContainsKey(r))
                        Ind_ReturnNoticeDate = alltxtIndiReturnNoticeDates[r];


                    if (deMode == "S")//Annexation
                    {

                        if (employee_Id != 0 || crime_Id != 0)
                        {

                            DiscCaseDetail dEntry = new DiscCaseDetail();

                            dEntry.CaseMasterId = Master_Id;
                            dEntry.AnnexationId = Convert.ToInt64(annexId);
                            dEntry.EmployeeId = Convert.ToInt64(employee_Id);
                            dEntry.CrimeId = Convert.ToInt32(crime_Id);
                            if (crime_Date_From != "")
                            {
                                dEntry.CrimeDateFrom = Convert.ToDateTime(crime_Date_From);
                            }
                            else
                            {
                                dEntry.CrimeDateFrom = null;
                            }
                            if (crime_Date_to != "")
                            {
                                dEntry.CrimeDateTo = Convert.ToDateTime(crime_Date_to);
                            }
                            else
                            {
                                dEntry.CrimeDateTo = null;
                            }
                            dEntry.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAm.ToString()) ? "0" : Ind_AnnexAm.ToString());
                            dEntry.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAmReturn.ToString()) ? "0" : Ind_AnnexAmReturn.ToString());
                            if (return_Date != "")
                            {
                                dEntry.ReturnNoticeDate = Convert.ToDateTime(return_Date);
                            }
                            else
                            {
                                dEntry.ReturnNoticeDate = null;
                            }

                            //dEntry.DispatchNo = ditch_No + "-" + DateTime.Now.Year.ToString();
                            dEntry.DispatchNo = ditch_No;

                            dEntry.Remarks = remarktxt;
                            dEntry.IsActive = true;
                            dEntry.CreateDate = DateTime.Now;

                            discCaseDetailService.Create(dEntry);

                            //Crime Location
                            //var CrimeLocationId = 0;
                            //if (allCrimeLocations.ContainsKey(r))
                            //    int.TryParse(allCrimeLocations[r], out CrimeLocationId);
                            //if (CrimeLocationId != 0)
                            //{
                            //    DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                            //    Crime_Location.CaseMasterId = Master_Id;
                            //    Crime_Location.OfficeId = CrimeLocationId;
                            //    Crime_Location.IsActive = true;
                            //    Crime_Location.CreateDate = DateTime.Now;
                            //    Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                            //    discCaseCrimeLocationService.Create(Crime_Location);
                            //}

                            DiscCaseDespatchNo discCaseDespatchNo = new DiscCaseDespatchNo();

                            discCaseDespatchNo.CaseMasterId = Master_Id;
                            discCaseDespatchNo.EmployeeId = Convert.ToInt64(employee_Id);
                            discCaseDespatchNo.CrimeId = Convert.ToInt32(crime_Id);
                            //discCaseDespatchNo.DespatchNo = ditch_No + "-" + DateTime.Now.Year.ToString();
                            discCaseDespatchNo.DespatchNo = ditch_No;

                            //discCaseDespatchNo.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAmReturn.ToString()) ? "0" : Ind_AnnexAmReturn.ToString());
                            //discCaseDespatchNo.TotalReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(return_Am.ToString()) ? "0" : return_Am.ToString());
                            discCaseDespatchNo.IsActive = true;
                            discCaseDespatchNo.CreateDate = DateTime.Now;
                            discCaseDespatchNo.CreateUser = SessionHelper.LoggedInEmployeeID;

                            discCaseDespatchNoService.Create(discCaseDespatchNo);
                        }
                    }
                    if (allCrimeLocations != null)
                    {
                        foreach (var L in allCrimeLocations)
                        {
                            DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                            Crime_Location.CaseMasterId = Master_Id;
                            Crime_Location.OfficeId = Convert.ToInt32(L);
                            Crime_Location.IsActive = true;
                            Crime_Location.CreateDate = DateTime.Now;
                            Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                            discCaseCrimeLocationService.Create(Crime_Location);
                        }
                    }

                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveCase(Dictionary<string, string> allEmoloyeeIds, Dictionary<string, string> allCrimeDates, Dictionary<string, string> allCrimeDatesTo, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allAnnexationAmts, Dictionary<string, string> allIndiReturnAmounts, Dictionary<string, string> alltxtIndiReturnNoticeDates, Dictionary<string, string> allTotalAnnexationAmts, Dictionary<string, string> allTotReturnAmount, Dictionary<string, string> allReturnNoticeDates, Dictionary<string, string> allCrimeType, Dictionary<string, string> allNewSls, string CaseDates, string AuditFrom, string AuditTo, string CaseDescriptions, List<string> allCrimeLocations, Dictionary<string, string> allDispatchNos, Dictionary<string, string> allDesRemarks, List<string> allDealingOfficerIds, string CaseType)
        {
            try
            {
                int counter = 0;
                int counter2 = 0;
                var curDate = DateTime.Now;

                var caseMasterId = discCaseMasterService.GetAll()
                                        .OrderByDescending(m => m.CaseMasterId)
                                        .Select(m => m.CaseMasterId)
                                        .FirstOrDefault();

                caseMasterId = caseMasterId > 0 ? caseMasterId : 0;
                              
                var casenumber = (caseMasterId + 1).ToString();

                DiscCaseMaster mEntry = new DiscCaseMaster();
                mEntry.CaseNo = casenumber;
                if (CaseDates == "")
                {
                    mEntry.CaseDateFrom = null;
                }
                else
                {
                    mEntry.CaseDateFrom = Convert.ToDateTime(CaseDates);
                }
                if (AuditFrom == "")
                {
                    mEntry.AuditFrom = null;
                }
                else
                {
                    mEntry.AuditFrom = Convert.ToDateTime(AuditFrom);
                }

                if (AuditTo == "")
                {
                    mEntry.AuditTo = null;
                }
                else
                {
                    mEntry.AuditTo = Convert.ToDateTime(AuditTo);
                }

                //mEntry.CaseType = "D";
                mEntry.CaseType = CaseType;
                mEntry.CaseDescription = CaseDescriptions;
                mEntry.CrimeLocation = Convert.ToInt32(string.IsNullOrEmpty(allCrimeLocations.First().ToString()) ? "0" : allCrimeLocations.First().ToString());//Convert.ToInt32(string.IsNullOrEmpty(CrimeLocations) ? "0" :CrimeLocations); //Convert.ToInt32(CrimeLocations);
                mEntry.DealOfficerId = Convert.ToInt64(string.IsNullOrEmpty(allDealingOfficerIds.First()) ? "0" : allDealingOfficerIds.First());//Convert.ToInt32(string.IsNullOrEmpty(DealingOfficerIds) ? "0" : DealingOfficerIds);                        
                mEntry.IsActive = true;
                mEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                mEntry.CreateDate = DateTime.Now;
                var mStatus = discCaseMasterService.Create(mEntry);

                if (allDealingOfficerIds != null)
                {
                    foreach (var id in allDealingOfficerIds)
                    {
                        DiscCaseDealingOfficer dofficer = new DiscCaseDealingOfficer();

                        dofficer.CaseMasterId = mStatus.CaseMasterId;
                        dofficer.EmployeeId = Convert.ToInt64(id);
                        dofficer.IsActive = true;
                        dofficer.InActiveDate = DateTime.Now;
                        dofficer.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dofficer.CreateDate = DateTime.Now;
                        discCaseDealingOfficerService.Create(dofficer);
                    }
                }

                if (mStatus.CaseMasterId > 0)
                {
                    var CrimeSl = allCrimeIds.Zip(allNewSls, (c, s) => new { allCrimeIds = c, NewSl = s });
                    var CrimeSlType = allCrimeType.Zip(CrimeSl, (t, c) => new { allCrimeType = t, CrimeSl = c });
                    var CrimeSlTypeAnnex = allTotalAnnexationAmts.Zip(CrimeSlType, (a, t) => new { allTotalAnnexationAmts = a, CrimeSlType = t });
                    var CrimeSlTypeAnnexReturn = allTotReturnAmount.Zip(CrimeSlTypeAnnex, (r, a) => new { allTotReturnAmount = r, allTotalAnnexationAmts = a });
                    var CrimeSlTypeAnnexReturnNoticeDate = allReturnNoticeDates.Zip(CrimeSlTypeAnnexReturn, (n, c) => new { allReturnNoticeDates = n, CrimeSlTypeAnnexReturn = c });

                    long annexStatus = 0;

                    decimal? TotReturnAmountK = Convert.ToDecimal(0.00);


                    foreach (var crime in allCrimeIds.Values.Distinct())
                    {
                        var crimeType = CrimeSlTypeAnnexReturnNoticeDate.Where(w => w.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.CrimeSl.allCrimeIds.Value == crime).FirstOrDefault();

                        if (crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.allCrimeType.Value == "1") // Attosat Type
                        {
                            DiscCaseAnnexation aEntry = new DiscCaseAnnexation();
                            aEntry.CaseMasterId = mStatus.CaseMasterId;
                            aEntry.CrimeId = Convert.ToInt32(crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.CrimeSl.allCrimeIds.Value);
                            aEntry.TotAnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString()) ? "0" : crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString());
                            aEntry.TotReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.CrimeSlTypeAnnexReturn.allTotReturnAmount.Value.ToString()) ? "0" : crimeType.CrimeSlTypeAnnexReturn.allTotReturnAmount.Value.ToString());

                            TotReturnAmountK = aEntry.TotReturnAmount;
                            //AnnexationAmountk = (decimal) aEntry.TotAnnexationAmount;
                            //ReturnAmountk = (decimal) aEntry.TotReturnAmount;

                            if (crimeType.allReturnNoticeDates.Value != null)
                            {
                                aEntry.ReturnNoticeDate = Convert.ToDateTime(crimeType.allReturnNoticeDates.Value);
                            }
                            else
                            {
                                aEntry.ReturnNoticeDate = null;
                            }
                            aEntry.IsActive = true;
                            aEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                            aEntry.CreateDate = DateTime.Now;
                            var aStatus = discCaseAnnexationService.Create(aEntry);



                            annexStatus = aStatus.AnnexationId;
                            counter = counter + 1;
                        }
                    }

                    var allCrimeIdAndSl = allCrimeIds.Zip(allNewSls, (c, s) => new { CrimeId = c, NewSl = s });
                    var allCrimeIdAndSlAndCrimeDate = allCrimeDates.Zip(allCrimeIdAndSl, (d, cs) => new { CrimeDate = d, CrimeIdAndSl = cs });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeId = allEmoloyeeIds.Zip(allCrimeIdAndSlAndCrimeDate, (e, csd) => new { EmployeeId = e, CrimeIdAndSlAndCrimeDate = csd });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt = allAnnexationAmts.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeId, (a, csde) => new { AnnexationAmt = a, CrimeIdAndSlAndCrimeDateAndEmployeeId = csde });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt = allTotalAnnexationAmts.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt, (ta, csdea) => new { TotalAnnexationAmt = ta, CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt = csdea });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount = allTotReturnAmount.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt, (ra, csdeat) => new { allTotReturnAmount = ra, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt = csdeat });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo = allDispatchNos.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount, (d, csdeatr) => new { allDispatchNos = d, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount = csdeatr });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks = allDesRemarks.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo, (r, csdeatrre) => new { allDesRemarks = r, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo = csdeatrre });
                    var allCrimeDetails1 = allCrimeDatesTo.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks, (d, c) => new { allCrimeDatesTo = d, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks = c });
                    var allCrimeDetails2 = allIndiReturnAmounts.Zip(allCrimeDetails1, (r, c) => new { allIndiReturnAmounts = r, allCrimeDetails1 = c });
                    var allCrimeDetails3 = alltxtIndiReturnNoticeDates.Zip(allCrimeDetails2, (r, c) => new { alltxtIndiReturnNoticeDates = r, allCrimeDetails2 = c });

                    /*
                     
                    var AnnexAmo = 0;
                    var totAnnex = 0;

                    foreach(var f in allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt)
                    {
                        AnnexAmo = Convert.ToInt32( f.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value);

                        totAnnex = Convert.ToInt32( f.TotalAnnexationAmt.Value);
                    
                    }
                     
                    */

                    foreach (var csdata in allCrimeDetails3)
                    {
                        DiscCaseDetail dEntry = new DiscCaseDetail();
                        dEntry.CaseMasterId = mStatus.CaseMasterId;
                        if (annexStatus > 0)
                            dEntry.AnnexationId = annexStatus;
                        dEntry.EmployeeId = Convert.ToInt64(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.EmployeeId.Value);
                        dEntry.CrimeId = Convert.ToInt32(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeIdAndSl.CrimeId.Value);

                        if (csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeDate.Value == "")
                        {
                            dEntry.CrimeDateFrom = null;
                        }
                        else
                        {
                            dEntry.CrimeDateFrom = Convert.ToDateTime(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeDate.Value);
                        }
                        if (csdata.allCrimeDetails2.allCrimeDetails1.allCrimeDatesTo.Value == "")
                        {
                            dEntry.CrimeDateTo = null;
                        }
                        else
                        {
                            dEntry.CrimeDateTo = Convert.ToDateTime(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeDatesTo.Value);
                        }

                        var IndivisualAnnexationAmount = Convert.ToDecimal(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString());


                        var JointlyAnnexationAmount = Convert.ToDecimal(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.TotalAnnexationAmt.Value.ToString());

                        dEntry.AnnexationAmount = IndivisualAnnexationAmount + JointlyAnnexationAmount;


                        // dEntry.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString()) ? "0" : csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString());


                        // dEntry.DispatchNo = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allDispatchNos.Value + "-" + DateTime.Now.Year.ToString();
                        dEntry.DispatchNo = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allDispatchNos.Value;

                        dEntry.Remarks = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allDesRemarks.Value;

                        dEntry.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(csdata.allCrimeDetails2.allIndiReturnAmounts.Value) ? "0" : csdata.allCrimeDetails2.allIndiReturnAmounts.Value);

                        dEntry.ReturnAmount = dEntry.ReturnAmount + TotReturnAmountK;

                        //if (dEntry.ReturnAmount == 0 ||  dEntry.ReturnAmount == null)
                        //{
                        //    dEntry.ReturnAmount = TotReturnAmountK;
                        //}


                        if (csdata.alltxtIndiReturnNoticeDates.Value != "")
                        {
                            dEntry.ReturnNoticeDate = Convert.ToDateTime(csdata.alltxtIndiReturnNoticeDates.Value);
                        }
                        else
                        {
                            dEntry.ReturnNoticeDate = null;
                        }

                        // dEntry.AnnexationAmount = AnnexationAmountk;
                        // dEntry.ReturnAmount = ReturnAmountk;





                        dEntry.IsActive = true;
                        dEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dEntry.CreateDate = DateTime.Now;
                        var dStatus = discCaseDetailService.Create(dEntry);
                        counter2 = counter2 + 1;

                    }
                    if (counter > 0)
                    {
                        for (int i = 1; i <= counter2; i++)
                        {
                            string r = i.ToString();
                            var Ind_AnnexAmReturn = 0;
                            var Emoloyee_Id = 0;
                            var Crime_Id = 0;
                            var Despach_No = "";
                            var ReturnAmount = 0;

                            if (allIndiReturnAmounts.ContainsKey(r))
                                int.TryParse(allIndiReturnAmounts[r], out Ind_AnnexAmReturn);

                            if (allEmoloyeeIds.ContainsKey(r))
                                int.TryParse(allEmoloyeeIds[r], out Emoloyee_Id);

                            if (allCrimeIds.ContainsKey(r))
                                int.TryParse(allCrimeIds[r], out Crime_Id);

                            if (allTotReturnAmount.ContainsKey(r))
                                int.TryParse(allTotReturnAmount[r], out ReturnAmount);


                            if (allDispatchNos.ContainsKey(r))
                                Despach_No = allDispatchNos[r];

                            DiscCaseDespatchNo discCaseDespatchNo = new DiscCaseDespatchNo();

                            discCaseDespatchNo.CaseMasterId = mStatus.CaseMasterId;
                            discCaseDespatchNo.EmployeeId = Emoloyee_Id;
                            discCaseDespatchNo.CrimeId = Crime_Id;
                            //discCaseDespatchNo.DespatchNo = Despach_No + "-" + DateTime.Now.Year.ToString();
                            discCaseDespatchNo.DespatchNo = Despach_No;

                            //discCaseDespatchNo.ReturnAmount = Ind_AnnexAmReturn;
                            //discCaseDespatchNo.TotalReturnAmount = ReturnAmount;
                            discCaseDespatchNo.IsActive = true;
                            discCaseDespatchNo.CreateDate = DateTime.Now;
                            discCaseDespatchNo.CreateUser = SessionHelper.LoggedInEmployeeID;

                            discCaseDespatchNoService.Create(discCaseDespatchNo);
                        }
                    }
                    if (allCrimeLocations != null)
                    {
                        foreach (var L in allCrimeLocations)
                        {
                            DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                            Crime_Location.CaseMasterId = mStatus.CaseMasterId;
                            Crime_Location.OfficeId = Convert.ToInt32(L);
                            Crime_Location.IsActive = true;
                            Crime_Location.CreateDate = DateTime.Now;
                            Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                            discCaseCrimeLocationService.Create(Crime_Location);
                        }
                    }
                }
                var CaseMaster = mStatus.CaseMasterId;
                return Json(new { Result = CaseMaster }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveChargeSheet(Dictionary<string, string> allEmoloyeeIds, Dictionary<string, string> allCrimeDates, List<string> allCrimeLocations, Dictionary<string, string> allCrimeDatesTo, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allAnnexationAmts, Dictionary<string, string> allIndiReturnAmounts, Dictionary<string, string> allIndiReturnNoticeDates, Dictionary<string, string> allTotalAnnexationAmts, Dictionary<string, string> allTotReturnAmount, Dictionary<string, string> allReturnNoticeDates, Dictionary<string, string> allCrimeType, Dictionary<string, string> allNewSls, string CaseNos, string CaseDates, string CaseDescriptions, Dictionary<string, string> allDispatchNos, Dictionary<string, string> allDesRemarks, string AuditFrom, string AuditTo, List<string> allDealingOfficerIds)
        {
            try
            {
                int counter = 0;
                int counter2 = 0;

                DiscCaseMaster mEntry = new DiscCaseMaster();
                // mEntry.CaseNo = CaseNos;
                if (CaseDates != "")
                {
                    mEntry.CaseDateFrom = Convert.ToDateTime(CaseDates);
                }
                else
                {
                    mEntry.CaseDateFrom = null;
                }

                if (AuditFrom != "")
                {
                    mEntry.AuditFrom = Convert.ToDateTime(AuditFrom);
                }
                else
                {
                    mEntry.AuditFrom = null;
                }
                if (AuditTo != "")
                {
                    mEntry.AuditTo = Convert.ToDateTime(AuditTo);
                }
                else
                {
                    mEntry.AuditTo = null;
                }

                mEntry.CaseType = "C";
                mEntry.CaseDescription = CaseDescriptions;
                mEntry.CrimeLocation = Convert.ToInt32(string.IsNullOrEmpty(allCrimeLocations.First().ToString()) ? "0" : allCrimeLocations.First().ToString());
                mEntry.DealOfficerId = Convert.ToInt64(string.IsNullOrEmpty(allDealingOfficerIds.First()) ? "0" : allDealingOfficerIds.First());
                mEntry.EnqueryOfficerId = 0;
                mEntry.IsActive = true;
                mEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                mEntry.CreateDate = DateTime.Now;
                var mStatus = discCaseMasterService.Create(mEntry);

                if (allDealingOfficerIds != null)
                {

                    foreach (var id in allDealingOfficerIds)
                    {
                        DiscCaseDealingOfficer dofficer = new DiscCaseDealingOfficer();

                        dofficer.CaseMasterId = mStatus.CaseMasterId;
                        dofficer.EmployeeId = Convert.ToInt64(id);
                        dofficer.IsActive = true;
                        dofficer.InActiveDate = DateTime.Now;
                        dofficer.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dofficer.CreateDate = DateTime.Now;
                        discCaseDealingOfficerService.Create(dofficer);
                    }
                }

                if (mStatus.CaseMasterId > 0)
                {
                    var CrimeSl = allCrimeIds.Zip(allNewSls, (c, s) => new { allCrimeIds = c, NewSl = s });
                    var CrimeSlType = allCrimeType.Zip(CrimeSl, (t, c) => new { allCrimeType = t, CrimeSl = c });
                    var CrimeSlTypeAnnex = allTotalAnnexationAmts.Zip(CrimeSlType, (a, t) => new { allTotalAnnexationAmts = a, CrimeSlType = t });
                    var CrimeSlTypeAnnexReturn = allTotReturnAmount.Zip(CrimeSlTypeAnnex, (r, a) => new { allTotReturnAmount = r, allTotalAnnexationAmts = a });
                    var CrimeSlTypeAnnexReturnNoticeDate = allReturnNoticeDates.Zip(CrimeSlTypeAnnexReturn, (a, t) => new { allReturnNoticeDates = a, CrimeSlTypeAnnexReturn = t });

                    //var CrimeSlTypeAnnexReturn = allTotReturnAmount.Zip(CrimeSlTypeReDate, (r, a) => new { allTotReturnAmount = r, CrimeSlTypeReDate = a });

                    long annexStatus = 0;

                    foreach (var crime in allCrimeIds.Values.Distinct())
                    {
                        var crimeType = CrimeSlTypeAnnexReturnNoticeDate.Where(w => w.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.CrimeSl.allCrimeIds.Value == crime).FirstOrDefault();

                        if (crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.allCrimeType.Value == "1") // Attosat Type
                        {
                            //var tCrimeId = Convert.ToInt32(crimeType.allTotalAnnexationAmts.CrimeSlType.CrimeSl.allCrimeIds.Value);
                            //var tTotAnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString()) ? "0" : crimeType.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString());
                            //var tTotReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.allTotReturnAmount.Value.ToString()) ? "0" : crimeType.allTotReturnAmount.Value.ToString());

                            DiscCaseAnnexation aEntry = new DiscCaseAnnexation();
                            aEntry.CaseMasterId = mStatus.CaseMasterId;
                            aEntry.CrimeId = Convert.ToInt32(crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.CrimeSlType.CrimeSl.allCrimeIds.Value);
                            aEntry.TotAnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString()) ? "0" : crimeType.CrimeSlTypeAnnexReturn.allTotalAnnexationAmts.allTotalAnnexationAmts.Value.ToString());
                            aEntry.TotReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(crimeType.CrimeSlTypeAnnexReturn.allTotReturnAmount.Value.ToString()) ? "0" : crimeType.CrimeSlTypeAnnexReturn.allTotReturnAmount.Value.ToString());
                            if (crimeType.allReturnNoticeDates.Value != "")
                            {
                                aEntry.ReturnNoticeDate = Convert.ToDateTime(crimeType.allReturnNoticeDates.Value);
                            }
                            else
                            {
                                aEntry.ReturnNoticeDate = null;
                            }
                            aEntry.IsActive = true;
                            aEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                            aEntry.CreateDate = DateTime.Now;
                            var aStatus = discCaseAnnexationService.Create(aEntry);
                            annexStatus = aStatus.AnnexationId;
                            counter = counter + 1;
                        }
                    }

                    var allCrimeIdAndSl = allCrimeIds.Zip(allNewSls, (c, s) => new { CrimeId = c, NewSl = s });
                    var allCrimeIdAndSlAndCrimeDate = allCrimeDates.Zip(allCrimeIdAndSl, (d, cs) => new { CrimeDate = d, CrimeIdAndSl = cs });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeId = allEmoloyeeIds.Zip(allCrimeIdAndSlAndCrimeDate, (e, csd) => new { EmployeeId = e, CrimeIdAndSlAndCrimeDate = csd });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt = allAnnexationAmts.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeId, (a, csde) => new { AnnexationAmt = a, CrimeIdAndSlAndCrimeDateAndEmployeeId = csde });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt = allTotalAnnexationAmts.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt, (ta, csdea) => new { TotalAnnexationAmt = ta, CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt = csdea });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount = allTotReturnAmount.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt, (ra, csdeat) => new { allTotReturnAmount = ra, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt = csdeat });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo = allDispatchNos.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount, (d, csdeatr) => new { allDispatchNos = d, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount = csdeatr });
                    var allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks = allDesRemarks.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo, (r, csdeatrre) => new { allDesRemarks = r, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo = csdeatrre });
                    var allCrimeDetails1 = allCrimeDatesTo.Zip(allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks, (d, c) => new { allCrimeDatesTo = d, allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks = c });
                    var allCrimeDetails2 = allIndiReturnAmounts.Zip(allCrimeDetails1, (r, c) => new { allIndiReturnAmounts = r, allCrimeDetails1 = c });
                    var allCrimeDetails3 = allIndiReturnNoticeDates.Zip(allCrimeDetails2, (r, c) => new { alltxtIndiReturnNoticeDates = r, allCrimeDetails2 = c });
                    //  var allIndiReturnAmounts

                    foreach (var csdata in allCrimeDetails3)
                    {

                        //DiscCaseAnnexation aEntry = new DiscCaseAnnexation();
                        //aEntry.CaseMasterId = mStatus.CaseMasterId;
                        //aEntry.CrimeId = Convert.ToInt32(csdata.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeIdAndSl.CrimeId.Value);
                        //aEntry.TotAnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(csdata.TotalAnnexationAmt.Value.ToString()) ? "0" : csdata.TotalAnnexationAmt.Value.ToString());
                        //aEntry.IsActive = true;
                        //aEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                        //aEntry.CreateDate = DateTime.Now;
                        //var aStatus = discCaseAnnexationService.Create(aEntry);

                        //if (aStatus.AnnexationId > 0)
                        //{




                        //DiscCaseDetail dEntry = new DiscCaseDetail();
                        //dEntry.CaseMasterId = mStatus.CaseMasterId;
                        //if (annexStatus > 0)
                        //    dEntry.AnnexationId = annexStatus;
                        //dEntry.EmployeeId = Convert.ToInt64(csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.EmployeeId.Value);
                        //dEntry.CrimeId = Convert.ToInt32(csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeIdAndSl.CrimeId.Value);
                        //dEntry.CrimeDateFrom = Convert.ToDateTime(csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeDate.Value);
                        //dEntry.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString()) ? "0" : csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString());
                        //if (dEntry.DispatchNo != "")
                        //    dEntry.DispatchNo = csdata.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allDispatchNos.Value;
                        //if (dEntry.Remarks != "")
                        //    dEntry.Remarks = csdata.allDesRemarks.Value;
                        //dEntry.IsActive = true;
                        //dEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                        //dEntry.CreateDate = DateTime.Now;
                        //var dStatus = discCaseDetailService.Create(dEntry);


                        DiscCaseDetail dEntry = new DiscCaseDetail();
                        dEntry.CaseMasterId = mStatus.CaseMasterId;
                        if (annexStatus > 0)
                            dEntry.AnnexationId = annexStatus;
                        dEntry.EmployeeId = Convert.ToInt64(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.EmployeeId.Value);
                        dEntry.CrimeId = Convert.ToInt32(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeIdAndSl.CrimeId.Value);
                        if (csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeDate.Value != "")
                        {
                            dEntry.CrimeDateFrom = Convert.ToDateTime(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeId.CrimeIdAndSlAndCrimeDate.CrimeDate.Value);
                        }
                        else
                        {
                            dEntry.CrimeDateFrom = null;
                        }
                        if (csdata.allCrimeDetails2.allCrimeDetails1.allCrimeDatesTo.Value != "")
                        {
                            dEntry.CrimeDateTo = Convert.ToDateTime(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeDatesTo.Value);
                        }
                        else
                        {
                            dEntry.CrimeDateTo = null;
                        }

                        dEntry.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString()) ? "0" : csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmount.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmt.CrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmt.AnnexationAmt.Value.ToString());
                        //dEntry.DispatchNo = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allDispatchNos.Value + "-" + DateTime.Now.Year.ToString();
                        dEntry.DispatchNo = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNo.allDispatchNos.Value;

                        dEntry.Remarks = csdata.allCrimeDetails2.allCrimeDetails1.allCrimeIdAndSlAndCrimeDateAndEmployeeIdAndAnnexationAmtAndTotAnnexationAmtAndReturnedAmountAndDispatchNoAndRemarks.allDesRemarks.Value;
                        if (csdata.allCrimeDetails2.allIndiReturnAmounts.Value != "")
                        {
                            dEntry.ReturnAmount = Convert.ToDecimal(csdata.allCrimeDetails2.allIndiReturnAmounts.Value);
                        }
                        else
                        {
                            dEntry.ReturnAmount = null;
                        }
                        if (csdata.alltxtIndiReturnNoticeDates.Value != "")
                        {
                            dEntry.ReturnNoticeDate = Convert.ToDateTime(csdata.alltxtIndiReturnNoticeDates.Value);
                        }
                        else
                        {
                            dEntry.ReturnNoticeDate = null;
                        }
                        dEntry.IsActive = true;
                        dEntry.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dEntry.CreateDate = DateTime.Now;
                        var dStatus = discCaseDetailService.Create(dEntry);
                        counter2 = counter2 + 1;
                        //Crime Location
                        //var CrimeLocationId = 0;
                        //if (allCrimeLocations.ContainsKey(counter2.ToString()))
                        //    int.TryParse(allCrimeLocations[counter2.ToString()], out CrimeLocationId);
                        //if (CrimeLocationId != 0)
                        //{
                        //    DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                        //    Crime_Location.CaseMasterId = mStatus.CaseMasterId;
                        //    Crime_Location.OfficeId = CrimeLocationId;
                        //    Crime_Location.IsActive = true;
                        //    Crime_Location.CreateDate = DateTime.Now;
                        //    Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                        //    discCaseCrimeLocationService.Create(Crime_Location);
                        //}
                    }

                    if (counter > 0)
                    {
                        for (int i = 1; i <= counter2; i++)
                        {
                            string r = i.ToString();
                            var Ind_AnnexAmReturn = 0;
                            var Emoloyee_Id = 0;
                            var Crime_Id = 0;
                            var Despach_No = "";
                            var ReturnAmount = 0;

                            if (allIndiReturnAmounts.ContainsKey(r))
                                int.TryParse(allIndiReturnAmounts[r], out Ind_AnnexAmReturn);

                            if (allEmoloyeeIds.ContainsKey(r))
                                int.TryParse(allEmoloyeeIds[r], out Emoloyee_Id);

                            if (allCrimeIds.ContainsKey(r))
                                int.TryParse(allCrimeIds[r], out Crime_Id);

                            if (allTotReturnAmount.ContainsKey(r))
                                int.TryParse(allTotReturnAmount[r], out ReturnAmount);


                            if (allDispatchNos.ContainsKey(r))
                                Despach_No = allDispatchNos[r];

                            DiscCaseDespatchNo discCaseDespatchNo = new DiscCaseDespatchNo();

                            discCaseDespatchNo.CaseMasterId = mStatus.CaseMasterId;
                            discCaseDespatchNo.EmployeeId = Emoloyee_Id;
                            discCaseDespatchNo.CrimeId = Crime_Id;
                            //discCaseDespatchNo.DespatchNo = Despach_No + "-" + DateTime.Now.Year.ToString();
                            discCaseDespatchNo.DespatchNo = Despach_No;

                            //discCaseDespatchNo.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAmReturn.ToString())?"0": Ind_AnnexAmReturn.ToString());
                            //discCaseDespatchNo.TotalReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(ReturnAmount.ToString()) ? "0" : ReturnAmount.ToString()); ;
                            discCaseDespatchNo.IsActive = true;
                            discCaseDespatchNo.CreateDate = DateTime.Now;
                            discCaseDespatchNo.CreateUser = SessionHelper.LoggedInEmployeeID;

                            discCaseDespatchNoService.Create(discCaseDespatchNo);
                        }
                    }
                    if (allCrimeLocations != null)
                    {
                        foreach (var L in allCrimeLocations)
                        {
                            DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                            Crime_Location.CaseMasterId = mStatus.CaseMasterId;
                            Crime_Location.OfficeId = Convert.ToInt32(L);
                            Crime_Location.IsActive = true;
                            Crime_Location.CreateDate = DateTime.Now;
                            Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                            discCaseCrimeLocationService.Create(Crime_Location);
                        }
                    }

                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PunishmentUpdate(Dictionary<string, string> allCollections, Dictionary<string, string> allCrimes, List<string> allSnos, Dictionary<string, string> allEmployees, Dictionary<string, string> allMasterIds, Dictionary<string, string> allCaseDetails, string PunishmentDates, string Punishments, string EmployeeCodes, string DespatchNos, string DaysLose = "", string ActivatedDt = "", string FirstIncSuspendDt = "", string SecondIncSuspendDt = "", string ThirdIncSuspendDt = "", string FourthIncSuspendDt = "")// List<string> allCaseDetailIds,
        {
            try
            {
                // var trxId = 1;
                //var empIds = allEmployees.Where(w => int.TryParse(w, out trxId));
                int Punishment = 0;
                // var PunishMaster = 0;

                if (Punishments != "0" && Punishments != "")
                {
                    var PunishMaster = 0;
                    Punishment = Convert.ToInt32(Punishments);


                    var punishmentMaster = new DiscCasePunishmentMaster();
                    punishmentMaster.EmployeeId = employeeService.GetByCode(EmployeeCodes).EmployeeId;
                    punishmentMaster.PunishmentId = Punishment;
                    if (PunishmentDates != "")
                    {
                        punishmentMaster.PunishmentDate = Convert.ToDateTime(PunishmentDates);
                    }
                    // punishmentMaster.DespatchNo = DespatchNos + "-" + DateTime.Now.Year.ToString();
                    punishmentMaster.DespatchNo = DespatchNos;

                    punishmentMaster.IsActive = true;
                    punishmentMaster.CreateDate = DateTime.Now;
                    punishmentMaster.CreateUser = SessionHelper.LoggedInEmployeeID;


                    if (ActivatedDt != "")
                    {
                        punishmentMaster.ActivatedDt = Convert.ToDateTime(ActivatedDt);
                    }

                    if (FirstIncSuspendDt != "")
                    {
                        punishmentMaster.FirstIncSuspendDt = Convert.ToDateTime(FirstIncSuspendDt);
                    }
                    if (SecondIncSuspendDt != "")
                    {
                        punishmentMaster.SecondIncSuspendDt = Convert.ToDateTime(SecondIncSuspendDt);
                    }
                    if (ThirdIncSuspendDt != "")
                    {
                        punishmentMaster.ThirdIncSuspendDt = Convert.ToDateTime(ThirdIncSuspendDt);
                    }
                    if (FourthIncSuspendDt != "")
                    {
                        punishmentMaster.FourthIncSuspendDt = Convert.ToDateTime(FourthIncSuspendDt);

                    }

                    if (DaysLose != "")
                    {
                        punishmentMaster.DaysLose = Convert.ToInt32(DaysLose);
                    }


                    PunishMaster = discCasePunishmentMasterService.Create(punishmentMaster).PunishmentMasterId;

                    var emp = employeeService.GetByCode(EmployeeCodes);
                    emp.SeniorityLoss = (emp.SeniorityLoss == null ? 0 : emp.SeniorityLoss) + discPunishmentService.GetById(Punishment).SeniorityLossDay;
                    employeeService.Update(emp);

                    if (allSnos != null)
                    {
                        foreach (var app in allSnos)
                        {
                            //discCasePunishmentDetailService  allCrimes 
                            var crimeId = 0;
                            var masterId = 0;
                            var DetailId = 0;
                            var Crime_Id = "txtCrimeId" + app;
                            if (allCrimes.ContainsKey(Crime_Id))
                                int.TryParse(allCrimes[Crime_Id], out crimeId);

                            var Master_Id = "txtCaseMasterId" + app;
                            if (allMasterIds.ContainsKey(Master_Id))
                                int.TryParse(allMasterIds[Master_Id], out masterId);

                            var Detail_Id = "txtCaseDetailsId" + app;
                            if (allCaseDetails.ContainsKey(Detail_Id))
                                int.TryParse(allCaseDetails[Detail_Id], out DetailId);

                            var PunishmentDetails = new DiscCasePunishmentDetail();
                            PunishmentDetails.PunishmentMasterId = PunishMaster;
                            PunishmentDetails.CaseMasterId = Convert.ToInt32(masterId);
                            PunishmentDetails.CaseDetailId = DetailId;
                            PunishmentDetails.CrimeId = crimeId;
                            PunishmentDetails.IsActive = true;
                            PunishmentDetails.CreateUser = SessionHelper.LoggedInEmployeeID;
                            PunishmentDetails.CreateDate = DateTime.Now;


                            discCasePunishmentDetailService.Create(PunishmentDetails);

                        }
                    }// END of allSnos null check

                }
                else
                {
                    var PunishMaster = 0;
                    foreach (var r in allSnos)
                    {
                        var Emp_id = 0;
                        var PunishmentId = "NewPunishmentID" + r;
                        if (allCollections.ContainsKey(PunishmentId))
                            int.TryParse(allCollections[PunishmentId], out Punishment);


                        var Emp_Id = "txtEmployeeId" + r;
                        if (allEmployees.ContainsKey(Emp_Id))
                            int.TryParse(allEmployees[Emp_Id], out Emp_id);

                        if (Punishment != 0)
                        {
                            var punishmentMaster = new DiscCasePunishmentMaster();
                            punishmentMaster.EmployeeId = Convert.ToInt64(Emp_id);
                            punishmentMaster.PunishmentId = Punishment;
                            if (PunishmentDates != "")
                            {
                                punishmentMaster.PunishmentDate = Convert.ToDateTime(PunishmentDates);
                            }
                            punishmentMaster.DespatchNo = DespatchNos;
                            punishmentMaster.IsActive = true;
                            punishmentMaster.CreateDate = DateTime.Now;
                            punishmentMaster.CreateUser = SessionHelper.LoggedInEmployeeID;
                            PunishMaster = discCasePunishmentMasterService.Create(punishmentMaster).PunishmentMasterId;

                            var emp = employeeService.GetByEmpId(Convert.ToInt64(Emp_id));
                            emp.SeniorityLoss = (emp.SeniorityLoss == null ? 0 : emp.SeniorityLoss) + discPunishmentService.GetById(Punishment).SeniorityLossDay;
                            employeeService.Update(emp);

                            var crimeId = 0;
                            var masterId = 0;
                            var DetailId = 0;
                            var Crime_Id = "txtCrimeId" + r;
                            if (allCrimes.ContainsKey(Crime_Id))
                                int.TryParse(allCrimes[Crime_Id], out crimeId);

                            var Master_Id = "txtCaseMasterId" + r;
                            if (allMasterIds.ContainsKey(Master_Id))
                                int.TryParse(allMasterIds[Master_Id], out masterId);

                            var Detail_Id = "txtCaseDetailsId" + r;
                            if (allCaseDetails.ContainsKey(Detail_Id))
                                int.TryParse(allCaseDetails[Detail_Id], out DetailId);

                            var PunishmentDetails = new DiscCasePunishmentDetail();
                            PunishmentDetails.PunishmentMasterId = PunishMaster;
                            PunishmentDetails.CaseMasterId = Convert.ToInt32(masterId);// app;
                            PunishmentDetails.CaseDetailId = DetailId;
                            PunishmentDetails.CrimeId = crimeId;
                            PunishmentDetails.IsActive = true;
                            PunishmentDetails.CreateUser = SessionHelper.LoggedInEmployeeID;
                            PunishmentDetails.CreateDate = DateTime.Now;

                            discCasePunishmentDetailService.Create(PunishmentDetails);
                        }
                    }
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Create(CaseEntryViewModel model)
        {

            var entity = Mapper.Map<CaseEntryViewModel, DiscCaseMaster>(model);
            try
            {

                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChargeSheetCreate(CaseEntryViewModel model)
        {

            var entity = Mapper.Map<CaseEntryViewModel, DiscCaseMaster>(model);
            try
            {
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditChargeSheet(string masterId, string CaseDates, string AuditFrom, string AuditTo, string CaseDescriptions, Dictionary<string, string> SaveModes, List<string> SNo, Dictionary<string, string> CaseDetailIds, Dictionary<string, string> AllAnnexationIds, Dictionary<string, string> allEmoloyeeIds, List<string> allCrimeLocations, Dictionary<string, string> allCrimeDates, Dictionary<string, string> allCrimeDatesTo, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allAnnexationAmts, Dictionary<string, string> allIndiReturnAmounts, Dictionary<string, string> alltxtIndiReturnNoticeDates, Dictionary<string, string> allTotalAnnexationAmts, Dictionary<string, string> allTotReturnAmount, Dictionary<string, string> allReturnNoticeDates, Dictionary<string, string> allNewSls, string CaseNos, Dictionary<string, string> allDispatchNos, Dictionary<string, string> allDesRemarks, List<string> allDealingOfficerIds)
        {
            try
            {
                var Master_Id = Convert.ToInt32(masterId);
                //long annexId = 0;
                var master = discCaseMasterService.GetById(Master_Id);
                if (CaseDates != "")
                {
                    master.CaseDateFrom = Convert.ToDateTime(CaseDates);
                }

                if (AuditFrom != "")
                {
                    master.AuditFrom = Convert.ToDateTime(AuditFrom);
                }
                if (AuditTo != "")
                {
                    master.AuditTo = Convert.ToDateTime(AuditTo);
                }

                master.CaseDescription = CaseDescriptions;
                master.CrimeLocation = Convert.ToInt32(string.IsNullOrEmpty(allCrimeLocations.First().ToString()) ? "0" : allCrimeLocations.First().ToString());//Convert.ToInt32(string.IsNullOrEmpty(CrimeLocations) ? "0" : CrimeLocations);
                master.UpdateUser = SessionHelper.LoggedInEmployeeID;
                master.UpdateDate = DateTime.Now;
                if (allDealingOfficerIds != null)
                {
                    master.DealOfficerId = Convert.ToInt64(string.IsNullOrEmpty(allDealingOfficerIds.First()) ? "0" : allDealingOfficerIds.First());
                }

                discCaseMasterService.Update(master);
                var annexId = discCaseAnnexationService.GetAll().Where(x => x.CaseMasterId == Master_Id && x.IsActive == true).First().AnnexationId;

                if (allDealingOfficerIds != null)
                {
                    foreach (var id in allDealingOfficerIds) //Dealing Officer Create
                    {
                        DiscCaseDealingOfficer dofficer = new DiscCaseDealingOfficer();

                        dofficer.CaseMasterId = Master_Id;
                        dofficer.EmployeeId = Convert.ToInt64(id);
                        dofficer.IsActive = true;
                        dofficer.InActiveDate = DateTime.Now;
                        dofficer.CreateUser = SessionHelper.LoggedInEmployeeID;
                        dofficer.CreateDate = DateTime.Now;
                        discCaseDealingOfficerService.Create(dofficer);
                    }
                }

                foreach (var r in SNo)
                {


                    var deatail_Id = 0;
                    var annexation_Id = 0;
                    var employee_Id = 0;
                    var crime_Id = 0;
                    string crimeDate_From = string.Empty;
                    string crimeDate_to = string.Empty;
                    var annexation_Am = 0;
                    var return_Am = 0;
                    string return_Date = string.Empty;
                    string ditch_No = string.Empty;
                    string remarktxt = string.Empty;
                    string deMode = string.Empty;


                    var Ind_AnnexAm = 0;
                    var Ind_AnnexAmReturn = 0;
                    string Ind_ReturnNoticeDate = string.Empty;

                    if (CaseDetailIds.ContainsKey(r))
                        int.TryParse(CaseDetailIds[r], out deatail_Id);

                    if (AllAnnexationIds.ContainsKey(r))
                        int.TryParse(AllAnnexationIds[r], out annexation_Id);

                    if (allEmoloyeeIds.ContainsKey(r))
                        int.TryParse(allEmoloyeeIds[r], out employee_Id);

                    if (allCrimeIds.ContainsKey(r))
                        int.TryParse(allCrimeIds[r], out crime_Id);

                    if (allCrimeDates.ContainsKey(r))
                        crimeDate_From = allCrimeDates[r];

                    if (allCrimeDatesTo.ContainsKey(r))
                        crimeDate_to = allCrimeDatesTo[r];


                    if (allTotalAnnexationAmts.ContainsKey(r))
                        int.TryParse(allTotalAnnexationAmts[r], out annexation_Am);

                    if (allTotReturnAmount.ContainsKey(r))
                        int.TryParse(allTotReturnAmount[r], out return_Am);

                    if (allReturnNoticeDates.ContainsKey(r))
                        return_Date = allReturnNoticeDates[r];

                    if (allDispatchNos.ContainsKey(r))
                        ditch_No = allDispatchNos[r];

                    if (allDesRemarks.ContainsKey(r))
                        remarktxt = allDesRemarks[r];


                    if (SaveModes.ContainsKey(r))
                        deMode = SaveModes[r];


                    if (allAnnexationAmts.ContainsKey(r))
                        int.TryParse(allAnnexationAmts[r], out Ind_AnnexAm);

                    if (allIndiReturnAmounts.ContainsKey(r))
                        int.TryParse(allIndiReturnAmounts[r], out Ind_AnnexAmReturn);

                    if (alltxtIndiReturnNoticeDates.ContainsKey(r))
                        Ind_ReturnNoticeDate = alltxtIndiReturnNoticeDates[r];

                    if (deMode == "S")
                    {
                        if (employee_Id != 0 || crime_Id != 0)
                        {
                            DiscCaseDetail dEntry = new DiscCaseDetail();

                            dEntry.CaseMasterId = Master_Id;
                            dEntry.AnnexationId = Convert.ToInt64(annexId);
                            dEntry.EmployeeId = Convert.ToInt64(employee_Id);
                            dEntry.CrimeId = Convert.ToInt32(crime_Id);
                            if (crimeDate_From != "")
                            {
                                dEntry.CrimeDateFrom = Convert.ToDateTime(crimeDate_From);
                            }
                            else
                            {
                                dEntry.CrimeDateFrom = null;
                            }
                            if (crimeDate_to != "")
                            {
                                dEntry.CrimeDateTo = Convert.ToDateTime(crimeDate_to);
                            }
                            else
                            {
                                dEntry.CrimeDateTo = null;
                            }
                            dEntry.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAm.ToString()) ? "0" : Ind_AnnexAm.ToString());
                            dEntry.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAmReturn.ToString()) ? "0" : Ind_AnnexAmReturn.ToString());
                            if (return_Date != "")
                            {
                                dEntry.ReturnNoticeDate = Convert.ToDateTime(return_Date);
                            }

                            // dEntry.DispatchNo = ditch_No + "-" + DateTime.Now.Year.ToString();
                            dEntry.DispatchNo = ditch_No;

                            dEntry.Remarks = remarktxt;
                            dEntry.IsActive = true;
                            dEntry.CreateDate = DateTime.Now;

                            discCaseDetailService.Create(dEntry);


                            //Crime Location
                            //var CrimeLocationId = 0;
                            //if (allCrimeLocations.ContainsKey(r))
                            //    int.TryParse(allCrimeLocations[r], out CrimeLocationId);
                            //if (CrimeLocationId != 0)
                            //{
                            //    DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                            //    Crime_Location.CaseMasterId = Master_Id;
                            //    Crime_Location.OfficeId = CrimeLocationId;
                            //    Crime_Location.IsActive = true;
                            //    Crime_Location.CreateDate = DateTime.Now;
                            //    Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                            //    discCaseCrimeLocationService.Create(Crime_Location);
                            //}

                            DiscCaseDespatchNo discCaseDespatchNo = new DiscCaseDespatchNo();

                            discCaseDespatchNo.CaseMasterId = Master_Id;
                            discCaseDespatchNo.EmployeeId = Convert.ToInt64(employee_Id);
                            discCaseDespatchNo.CrimeId = Convert.ToInt32(crime_Id);
                            //discCaseDespatchNo.DespatchNo = ditch_No + "-" + DateTime.Now.Year.ToString();
                            discCaseDespatchNo.DespatchNo = ditch_No;

                            //discCaseDespatchNo.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(Ind_AnnexAmReturn.ToString()) ? "0" : Ind_AnnexAmReturn.ToString());
                            //discCaseDespatchNo.TotalReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(return_Am.ToString()) ? "0" : return_Am.ToString());
                            discCaseDespatchNo.IsActive = true;
                            discCaseDespatchNo.CreateDate = DateTime.Now;
                            discCaseDespatchNo.CreateUser = SessionHelper.LoggedInEmployeeID;

                            discCaseDespatchNoService.Create(discCaseDespatchNo);
                        }
                    }

                }
                if (allCrimeLocations != null)
                {
                    foreach (var L in allCrimeLocations)
                    {
                        DiscCaseCrimeLocation Crime_Location = new DiscCaseCrimeLocation();
                        Crime_Location.CaseMasterId = Master_Id;
                        Crime_Location.OfficeId = Convert.ToInt32(L);
                        Crime_Location.IsActive = true;
                        Crime_Location.CreateDate = DateTime.Now;
                        Crime_Location.CreateUser = SessionHelper.LoginUserEmployeeId;
                        discCaseCrimeLocationService.Create(Crime_Location);
                    }
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        #region HttpRequests 

        public JsonResult CheckDespatchValidation(string DespatchNo)
        {
            var Despatch = discCaseDetailService.GetAll().Where(d => d.IsActive == true && d.DispatchNo == DespatchNo);
            var Result = "";
            if (Despatch.Count() >= 1)
            {
                Result = "error";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Result = "ok";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CasePunishmentSave(Dictionary<string, string> allCaseTypes, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allCrimeDtFroms, Dictionary<string, string> allCrimeDtTos, Dictionary<string, string> allDespatchs, List<string> allCrimeOffices, List<string> allSnos, string EmployeeIds, string PunishmentDespatchNos, string PunishmentDts, string CommonPunishmentIds, string AnnexationAmounts, string CaseDateFromMsgs, string DaysLose, string ActivatedDt, string FirstIncSuspendDt, string SecondIncSuspendDt, string ThirdIncSuspendDt, string FourthIncSuspendDt)
        {
            try
            {
                // int[] masterId;
                // int[] detailId;
                var MasterId = 0;
                List<int> masterIdList = new List<int>();
                List<int> detailIdList = new List<int>();
                foreach (var r in allSnos)
                {
                    ///////////////////// DiscCaseMaster Table ////////////

                    var CaseType = "";
                    var CaseDateFrom = "";
                    var CaseDateTo = "";

                    var Case_Type = "txtCaseTypeValue" + r;
                    var CaseDate_From = "txtCrimeDateFrom" + r;
                    var CaseDate_To = "txtCrimeDateTo" + r;

                    if (allCaseTypes.ContainsKey(Case_Type))
                        CaseType = allCaseTypes[Case_Type];

                    if (allCrimeDtFroms.ContainsKey(CaseDate_From))
                        CaseDateFrom = allCrimeDtFroms[CaseDate_From];

                    if (allCrimeDtTos.ContainsKey(CaseDate_To))
                        CaseDateTo = allCrimeDtTos[CaseDate_To];

                    DiscCaseMaster CaseMaster = new DiscCaseMaster();
                    CaseMaster.CaseType = CaseType;
                    if (CaseDateFromMsgs != "")
                    {
                        CaseMaster.CaseDateFrom = Convert.ToDateTime(CaseDateFromMsgs);
                    }
                    CaseMaster.CrimeLocation = Convert.ToInt32(string.IsNullOrEmpty(allCrimeOffices.First()) ? "0" : allCrimeOffices.First());
                    CaseMaster.IsActive = true;
                    CaseMaster.CreateDate = DateTime.Now;
                    CaseMaster.CreateUser = SessionHelper.LoggedInEmployeeID;
                    MasterId = discCaseMasterService.Create(CaseMaster).CaseMasterId;

                    masterIdList.Add(MasterId);


                    ////////////////// DiscCaseMaster Table end //////////////////

                    ////////////////// DiscCaseDetail Table  //////////////////


                    var CrimeId = 0;
                    var Despatch = "";

                    var Crime_Id = "txtCrimeId" + r;
                    var Despatch_No = "txtDespatchNo" + r;

                    if (allCrimeIds.ContainsKey(Crime_Id))
                        int.TryParse(allCrimeIds[Crime_Id], out CrimeId);

                    if (allDespatchs.ContainsKey(Despatch_No))
                        Despatch = allDespatchs[Despatch_No];

                    DiscCaseDetail CaseDetail = new DiscCaseDetail();
                    CaseDetail.CaseMasterId = Convert.ToInt32(MasterId);
                    CaseDetail.EmployeeId = Convert.ToInt64(EmployeeIds);
                    CaseDetail.CrimeId = CrimeId;
                    if (CaseDateFrom != "")
                    {
                        CaseDetail.CrimeDateFrom = Convert.ToDateTime(CaseDateFrom);
                    }
                    if (CaseDateTo != "")
                    {
                        CaseDetail.CrimeDateTo = Convert.ToDateTime(CaseDateTo);
                    }
                    if (AnnexationAmounts != "")
                    {
                        CaseDetail.AnnexationAmount = Convert.ToDecimal(AnnexationAmounts);
                    }
                    CaseDetail.DispatchNo = Despatch;// +"-" + DateTime.Now.Year.ToString();
                    CaseDetail.IsActive = true;
                    CaseDetail.CreateDate = DateTime.Now;
                    CaseDetail.CreateUser = SessionHelper.LoggedInEmployeeID;

                    var CaseDetailId = discCaseDetailService.Create(CaseDetail).CaseDetailsId;
                    detailIdList.Add(CaseDetailId);
                    ////////////////// DiscCaseDetail Table end ////////////////// AnnexationAmounts

                    if (AnnexationAmounts != "")
                    {
                        DiscCaseAnnexation Annexation = new DiscCaseAnnexation();
                        Annexation.CaseMasterId = Convert.ToInt32(MasterId);
                        Annexation.CrimeId = CrimeId;
                        Annexation.TotAnnexationAmount = Convert.ToDecimal(AnnexationAmounts);
                        Annexation.IsActive = true;
                        Annexation.CreateDate = DateTime.Now;
                        Annexation.CreateUser = SessionHelper.LoggedInEmployeeID;

                        discCaseAnnexationService.Create(Annexation);
                    }


                    DiscCaseDespatchNo Disc_Descpatch = new DiscCaseDespatchNo();
                    Disc_Descpatch.CaseMasterId = Convert.ToInt32(MasterId);
                    Disc_Descpatch.CrimeId = CrimeId;
                    Disc_Descpatch.DespatchNo = Despatch;
                    Disc_Descpatch.IsActive = true;
                    Disc_Descpatch.EmployeeId = Convert.ToInt64(EmployeeIds);
                    Disc_Descpatch.CreateDate = DateTime.Now;
                    Disc_Descpatch.CreateUser = SessionHelper.LoggedInEmployeeID;
                    discCaseDespatchNoService.Create(Disc_Descpatch);

                    if (allCrimeOffices.Count != 0)
                    {
                        foreach (var L in allCrimeOffices)
                        {
                            DiscCaseCrimeLocation Location = new DiscCaseCrimeLocation();
                            Location.CaseMasterId = Convert.ToInt32(MasterId);
                            Location.OfficeId = Convert.ToInt32(L);
                            Location.CreateDate = DateTime.Now;
                            Location.CreateUser = SessionHelper.LoggedInEmployeeID;
                            Location.IsActive = true;
                            discCaseCrimeLocationService.Create(Location);
                        }
                    }
                }
                var punishmentmasterId = 0;
                if (CommonPunishmentIds != "0")
                {
                    DiscCasePunishmentMaster PunishmentMaster = new DiscCasePunishmentMaster();
                    PunishmentMaster.EmployeeId = Convert.ToInt64(EmployeeIds);
                    PunishmentMaster.PunishmentId = Convert.ToInt32(CommonPunishmentIds);
                    if (PunishmentDts != "")
                    {
                        PunishmentMaster.PunishmentDate = Convert.ToDateTime(PunishmentDts);
                    }
                    if (ActivatedDt != "")
                    {
                        PunishmentMaster.ActivatedDt = Convert.ToDateTime(ActivatedDt);
                    }

                    if (FirstIncSuspendDt != "")
                    {
                        PunishmentMaster.FirstIncSuspendDt = Convert.ToDateTime(FirstIncSuspendDt);
                    }
                    if (SecondIncSuspendDt != "")
                    {
                        PunishmentMaster.SecondIncSuspendDt = Convert.ToDateTime(SecondIncSuspendDt);
                    }
                    if (ThirdIncSuspendDt != "")
                    {
                        PunishmentMaster.ThirdIncSuspendDt = Convert.ToDateTime(ThirdIncSuspendDt);
                    }
                    if (FourthIncSuspendDt != "")
                    {
                        PunishmentMaster.FourthIncSuspendDt = Convert.ToDateTime(FourthIncSuspendDt);

                    }

                    if (DaysLose != "")
                    {
                        PunishmentMaster.DaysLose = Convert.ToInt32(DaysLose);
                    }
                    PunishmentMaster.DespatchNo = PunishmentDespatchNos; // +"-" + DateTime.Now.Year.ToString();
                    PunishmentMaster.IsActive = true;
                    PunishmentMaster.CreateDate = DateTime.Now;
                    PunishmentMaster.CreateUser = SessionHelper.LoggedInEmployeeID;
                    punishmentmasterId = discCasePunishmentMasterService.Create(PunishmentMaster).PunishmentMasterId;
                }

                if (detailIdList.Count != 0)
                {
                    foreach (var app in detailIdList)
                    {
                        DiscCasePunishmentDetail PunishmentDetail = new DiscCasePunishmentDetail();

                        var CrimeDetailId = discCaseDetailService.GetById(app).CrimeId;
                        var CaseMasterId = discCaseDetailService.GetById(app).CaseMasterId;

                        PunishmentDetail.PunishmentMasterId = punishmentmasterId;
                        PunishmentDetail.CaseMasterId = CaseMasterId;
                        PunishmentDetail.CaseDetailId = Convert.ToInt32(app);
                        PunishmentDetail.CrimeId = CrimeDetailId;
                        PunishmentDetail.IsActive = true;
                        PunishmentDetail.CreateDate = DateTime.Now;
                        PunishmentDetail.CreateUser = SessionHelper.LoggedInEmployeeID;

                        discCasePunishmentDetailService.Create(PunishmentDetail);
                    }
                }


                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCrimeLocationByCode(string office_Id)
        {

            try
            {
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var employeeModel = new EmployeeViewModel();
                //var param = new { OfficeId = Convert.ToInt32(office_Id) };
                var officeId = Convert.ToInt32(office_Id);
                var officeDetail = officeService.GetMany(p => p.OfficeId == officeId && p.IsActive==true).FirstOrDefault();
                if(officeDetail!=null)
                {
                    employeeModel.OfficeId= officeDetail.OfficeId;
                    employeeModel.OfficeCode = officeDetail.OfficeCode;
                    employeeModel.OfficeName = officeDetail.OfficeName;
                    List_EmployeeViewModel.Add(employeeModel);
                }

               // var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "disc.SP_Get_OfficeLocationByCode");
               // List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               //.Select(row => new EmployeeViewModel
               //{
               //    OfficeId = row.Field<int>("OfficeId"),
               //    OfficeCode = row.Field<string>("OfficeCode"),
               //    OfficeName = row.Field<string>("OfficeName")
               //}).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DespatchNoSave(string CaseMasterId, string DespatchType, string DespatchDate, string DespatchNo)
        {
            try
            {
                var result = 1;

                DiscCaseDespatchNo despatch = new DiscCaseDespatchNo();
                despatch.CaseMasterId = Convert.ToInt32(CaseMasterId);
                despatch.DespatchType = DespatchType;
                despatch.DespatchDate = Convert.ToDateTime(DespatchDate);
                //despatch.DespatchNo = DespatchNo + "-" + DateTime.Now.Year.ToString();
                despatch.DespatchNo = DespatchNo;

                discCaseDespatchNoService.Create(despatch);



                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ExportException ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditReturnAmount(string CaseDetailsId, string indReturn, string totReturn, string reDespatchNo)
        {
            var result = 1;
            try
            {
                var crimeDetail = discCaseDetailService.GetById(Convert.ToInt32(CaseDetailsId));

                var masterId = crimeDetail.CaseMasterId;
                var EmpId = crimeDetail.EmployeeId;
                var crimeId = crimeDetail.CrimeId;

                //discCaseDespatchNoService
                DiscCaseDespatchNo discCaseDespatchNo = new DiscCaseDespatchNo();

                discCaseDespatchNo.CaseMasterId = masterId;
                discCaseDespatchNo.EmployeeId = EmpId;
                discCaseDespatchNo.CrimeId = crimeId;
                //discCaseDespatchNo.DespatchNo = reDespatchNo + "-" + DateTime.Now.Year.ToString();
                discCaseDespatchNo.DespatchNo = reDespatchNo;

                //discCaseDespatchNo.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(indReturn.ToString()) ? "0" : indReturn.ToString());//Convert.ToDecimal(indReturn);
                //discCaseDespatchNo.TotalReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(totReturn.ToString()) ? "0" : totReturn.ToString());// Convert.ToDecimal(totReturn);
                discCaseDespatchNo.IsActive = true;
                discCaseDespatchNo.CreateDate = DateTime.Now;
                discCaseDespatchNo.CreateUser = SessionHelper.LoggedInEmployeeID;

                discCaseDespatchNoService.Create(discCaseDespatchNo);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ExportException ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditCaseDetails(string CaseDetailsId, string AnnexationId, string TotAnnexationAmount, string TotReturnAmount, string IndReturnNoticeDateMsg, string AnnexationAmount, string ReturnAmount, string ReturnNoticeDateMsg, string CrimeDateFrom, string CrimeDateTo)
        {
            var result = 1;

            try
            {
                var caseDetail = discCaseDetailService.GetById(Convert.ToInt32(CaseDetailsId));

                var caseMaster = discCaseDetailService.GetById(Convert.ToInt32(CaseDetailsId)).CaseMasterId;

                caseDetail.AnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(TotAnnexationAmount.ToString()) ? "0" : TotAnnexationAmount.ToString());// Convert.ToDecimal(TotAnnexationAmount);//AnnexationAmount
                caseDetail.ReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(TotReturnAmount.ToString()) ? "0" : TotReturnAmount.ToString());// Convert.ToDecimal(TotReturnAmount);//ReturnAmount
                if (IndReturnNoticeDateMsg == null || IndReturnNoticeDateMsg == "")//ReturnNoticeDateMsg
                {
                    caseDetail.ReturnNoticeDate = null;
                }
                else
                {
                    caseDetail.ReturnNoticeDate = Convert.ToDateTime(IndReturnNoticeDateMsg);
                }

                if (CrimeDateFrom != "")
                {
                    caseDetail.CrimeDateFrom = Convert.ToDateTime(CrimeDateFrom);
                }
                if (CrimeDateTo != "")
                {
                    caseDetail.CrimeDateTo = Convert.ToDateTime(CrimeDateTo);
                }
                discCaseDetailService.Update(caseDetail);

                if (CrimeDateFrom != "" && CrimeDateTo != "")
                {
                    var CaseDetail = discCaseDetailService.GetAll().Where(x => x.CaseMasterId == caseMaster && x.IsActive == true);

                    foreach (var r in CaseDetail)
                    {
                        r.CrimeDateFrom = Convert.ToDateTime(CrimeDateFrom);
                        r.CrimeDateTo = Convert.ToDateTime(CrimeDateTo);
                        discCaseDetailService.Update(r);
                    }
                }
                var Annex = discCaseAnnexationService.GetById(Convert.ToInt32(AnnexationId));

                Annex.TotAnnexationAmount = Convert.ToDecimal(string.IsNullOrEmpty(AnnexationAmount.ToString()) ? "0" : AnnexationAmount.ToString());//Convert.ToDecimal(AnnexationAmount);
                Annex.TotReturnAmount = Convert.ToDecimal(string.IsNullOrEmpty(ReturnAmount.ToString()) ? "0" : ReturnAmount.ToString());// Convert.ToDecimal(ReturnAmount);
                if (ReturnNoticeDateMsg == null || ReturnNoticeDateMsg == "")
                {
                    Annex.ReturnNoticeDate = null;
                }
                else
                    Annex.ReturnNoticeDate = Convert.ToDateTime(ReturnNoticeDateMsg);

                discCaseAnnexationService.Update(Annex);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ExportException ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EnquiryoffEdit(string CaseEnquiryOfficerId, string EnquiryOfficerAssignedDt, string InvestigationDtMsg, string ReportReceivedDtMsg, string EnquiryRemarks, string DespatchNo)
        {
            var result = 1;

            try
            {
                var enquiry = discCaseEnquiryOfficerService.GetById(Convert.ToInt32(CaseEnquiryOfficerId));

                enquiry.EnquiryOfficerAssignedDt = Convert.ToDateTime(EnquiryOfficerAssignedDt);
                enquiry.InvestigationDt = Convert.ToDateTime(InvestigationDtMsg);
                enquiry.ReportReceivedDt = Convert.ToDateTime(ReportReceivedDtMsg);
                enquiry.EnquiryRemarks = EnquiryRemarks;
                // enquiry.DespatchNo = DespatchNo + "-" + DateTime.Now.Year.ToString();
                enquiry.DespatchNo = DespatchNo;

                discCaseEnquiryOfficerService.Update(enquiry);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ExportException ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEnquiryOfficer(string MasterId)
        {
            try
            {
                List<CaseEntryViewModel> DisEnquiryOfficer = new List<CaseEntryViewModel>();
                var param = new { MasterId = Convert.ToInt32(MasterId) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_Load_EnquiryOfficer");

                DisEnquiryOfficer = empList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    CaseEnquiryOfficerId = row.Field<int>("CaseEnquiryOfficerId"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    EmployeeId = row.Field<long>("EnqueryOfficerId"),
                    DespatchNo = row.Field<string>("DespatchNo"),
                    EnqueryOfficerName = row.Field<string>("EnqueryOfficerName"),
                    EnquiryOfficerAssignedDtMsg = row.Field<string>("EnquiryOfficerAssignedDtMsg"),
                    //CrimeFindOutFromMsg = row.Field<string>("CrimeFindOutFromMsg"),
                    // CrimeFindOutToMsg = row.Field<string>("CrimeFindOutToMsg"),
                    InvestigationDtMsg = row.Field<string>("InvestigationDtMsg"),
                    ReportReceivedDtMsg = row.Field<string>("ReportReceivedDtMsg"),
                    EnquiryRemarks = row.Field<string>("EnquiryRemarks"),
                    Mode = row.Field<string>("Mode"),
                    SlNo = row.Field<string>("SlNo")

                }).ToList();

                return Json(DisEnquiryOfficer.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteInqueryOfficer(string CaseEnquiryOfficerId)
        {
            var result = 1;
            var enquiryoffi = discCaseEnquiryOfficerService.GetById(Convert.ToInt32(CaseEnquiryOfficerId));
            enquiryoffi.IsActive = false;
            discCaseEnquiryOfficerService.Update(enquiryoffi);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CaseDetailDelete(string CaseDetailsId, string AnnexationId)
        {
            var result = 0;
            var CaseDetails = discCaseDetailService.GetById(Convert.ToInt32(CaseDetailsId));
            CaseDetails.IsActive = false;
            discCaseDetailService.Update(CaseDetails);
            result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCrimeDetails(string CaseMasterId)
        {
            try
            {
                List<CaseEntryViewModel> DisCaseDetail = new List<CaseEntryViewModel>();
                var param = new { CaseMasterId = Convert.ToInt32(CaseMasterId) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetCrimeDetails");

                DisCaseDetail = empList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    CaseNo = row.Field<string>("CaseNo"),
                    DispatchNo = row.Field<string>("DispatchNo"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFromMsg"),
                    CrimeDateToMsg = row.Field<string>("CrimeDateToMsg"),
                    CrimeId = row.Field<int>("CrimeId"),
                    CrimeName = row.Field<string>("CrimeName"),
                    TotAnnexationAmount = row.Field<decimal>("TotAnnexationAmount"),
                    TotReturnAmount = row.Field<decimal>("TotReturnAmount"),
                    IndReturnNoticeDateMsg = row.Field<string>("IndReturnNoticeDateMsg"),
                    AnnexationAmount = row.Field<decimal>("AnnexationAmount"),
                    ReturnAmount = row.Field<decimal>("ReturnAmount"),
                    ReturnNoticeDateMsg = row.Field<string>("ReturnNoticeDateMsg"),
                    AnnexationId = row.Field<long?>("AnnexationId"),
                    Remarks = row.Field<string>("Remarks"),
                    CaseDetailsId = row.Field<int>("CaseDetailsId")

                }).ToList();

                return Json(DisCaseDetail.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOfficeForEdit(string OfficeId)
        {
            try
            {
                List<CaseEntryViewModel> DisCrimeLocation = new List<CaseEntryViewModel>();
                var param = new { OfficeId = Convert.ToInt32(OfficeId) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetOfficeForCrimeEdit");

                DisCrimeLocation = empList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId"),
                    OfficeLevel = row.Field<int>("OfficeLevel"),
                    FirstLevelId = row.Field<int>("FirstLevelId"),
                    SecondLevelId = row.Field<int>("SecondLevelId"),
                    ThirdLevelId = row.Field<int>("ThirdLevelId"),
                    FourthLevelId = row.Field<int>("FourthLevelId")

                }).ToList();

                return Json(DisCrimeLocation.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult deleteDeOfficer(string CaseDealingOfficerId)
        {
            var result = 1;
            var dealoffi = discDealingOfficerService.GetById(Convert.ToInt32(CaseDealingOfficerId));
            dealoffi.IsActive = false;
            discDealingOfficerService.Update(dealoffi);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteLocation(string DiscCaseCrimeLocationId)
        {
            var result = 1;
            var dealoffi = discCaseCrimeLocationService.GetById(Convert.ToInt32(DiscCaseCrimeLocationId));
            dealoffi.IsActive = false;
            discCaseCrimeLocationService.Update(dealoffi);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDealingOfficer(string MasterId)
        {
            try
            {
                List<CaseEntryViewModel> DisDealingOfficer = new List<CaseEntryViewModel>();
                var param = new { CaseMasterId = Convert.ToInt32(MasterId) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_LoadDealingOfficer");

                DisDealingOfficer = empList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    EmployeeId = row.Field<long>("DealofficerId"),
                    CaseDealingOfficerId = row.Field<int>("CaseDealingOfficerId"),
                    EmployeeName = row.Field<string>("DealOfficerName"),

                }).ToList();

                return Json(DisDealingOfficer.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCrimeLocation(string MasterId)
        {
            try
            {
                List<DiscCaseCrimeLocationViewModel> List_DiscCaseCrimeLocationViewModel = new List<DiscCaseCrimeLocationViewModel>();
                var param = new { CaseMasterId = Convert.ToInt32(MasterId) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.Sp_GetCrimeLocation");//DiscCaseCrimeLocationId CaseMasterId OfficeId 

                List_DiscCaseCrimeLocationViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DiscCaseCrimeLocationViewModel
                {
                    rowSl = row.Field<long>("rowSl"),
                    DiscCaseCrimeLocationId = row.Field<int>("DiscCaseCrimeLocationId"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),

                }).ToList();

                return Json(List_DiscCaseCrimeLocationViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                List<EmployeePostingHistoryViewModel> List_EmployeeViewModel = new List<EmployeePostingHistoryViewModel>();
                var param = new { EmployeeId = Convert.ToInt64(employee_code) };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetEmployeeDetailsWithSalary");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeePostingHistoryViewModel
                {
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    EmployeeRank = row.Field<string>("EmployeeRank"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    DesignationId = row.Field<int?>("DesignationId"),
                    PromotionType = row.Field<string>("PromotionType"),
                    Pay = row.Field<decimal?>("Pay")

                }).ToList();

                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEnquiryOfficerList()
        {
            //var EnOfficerList = discCaseEnquiryOfficerService.GetAll().Where(x=>x.IsActive == true)
            //var viewEnOfficerList = EnOfficerList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.EmployeeId.ToString(),
            //    Text = string.Format("{0} - {1}", x.e, x.OfficeName)
            //});
            //var aoOffice_items = new List<SelectListItem>();
            //if (viewAOOffice.ToList().Count > 0)
            //{
            //    aoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            //aoOffice_items.AddRange(viewAOOffice);

            var enqueryList = discEnqueryOfficerService.GetEmployeeByOfficeId(Convert.ToInt32(LoggedInOfficeID));
            var enqueryorDetails = enqueryList.Select(m => new SelectListItem() { Text = string.Format("{0}-{1}", m.EmployeeCode, m.EmployeeName), Value = m.EmployeeId.ToString() });
            var discEnqueryLists = new List<SelectListItem>();
            discEnqueryLists.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            discEnqueryLists.AddRange(enqueryorDetails);
            // model.EnqueryOfficerList = discEnqueryLists;

            return Json(discEnqueryLists, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCrimeList()
        {
            var crimeList = discCrimeService.GetAll();
            var viewCrime = crimeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CrimeId.ToString(),
                Text = string.Format("{0} - {1}", x.CrimeCode, x.CrimeName)
            });
            var crime_items = new List<SelectListItem>();
            if (viewCrime.ToList().Count > 0)
            {
                crime_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            crime_items.AddRange(viewCrime);
            return Json(crime_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStatusList()
        {
            var statusList = discStatusService.GetAll();
            var viewStatus = statusList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StatusId.ToString(),
                Text = string.Format(x.StatusMsg)
            });
            var status_items = new List<SelectListItem>();
            if (viewStatus.ToList().Count > 0)
            {
                status_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            status_items.AddRange(viewStatus);
            return Json(status_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCrimeType(string crimeId)
        {
            var crimeType = discCrimeService.GetById(Convert.ToInt32(crimeId)).CrimeType;
            return Json(crimeType, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CaseDelete(string CaseMasterId)
        {
            var master = discCaseMasterService.GetById(Convert.ToInt32(CaseMasterId));
            var annexation = discCaseAnnexationService.GetAllByCaseMasterId(Convert.ToInt32(CaseMasterId));
            var details = discCaseDetailService.GetAllByCaseMasterId(Convert.ToInt32(CaseMasterId));
            var punishment = discCasePunishmentDetailService.GetAll().Where(x => x.CaseMasterId == Convert.ToInt32(CaseMasterId));

            var punishMast = 0;
            string Result = "OK";
            if (ModelState.IsValid)
            {
                master.IsActive = false;
                master.InActiveDate = DateTime.Now;
                master.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                master.UpdateDate = DateTime.Now;
                discCaseMasterService.Update(master);

                foreach (var item in annexation)
                {
                    item.IsActive = false;
                    item.InActiveDate = DateTime.Now;
                    item.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    item.UpdateDate = DateTime.Now;
                    discCaseAnnexationService.Update(item);
                }

                foreach (var item1 in details)
                {
                    item1.IsActive = false;
                    item1.InActiveDate = DateTime.Now;
                    item1.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    item1.UpdateDate = DateTime.Now;
                    discCaseDetailService.Update(item1);
                }
                foreach (var p in punishment)
                {
                    punishMast = punishMast + 1;
                    p.IsActive = false;
                    p.UpdateDate = DateTime.Now;
                    p.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    discCasePunishmentDetailService.Update(p);

                    ////  if the punishment Master is punishment type 24( Zone Wise ) Then Delete From  DiscRestrictedZone

                    //DiscRestrictedZone


                    var param = new
                    {
                        PunishmentMasterId = p.PunishmentMasterId

                    };
                    var empList = employeeSPService.GetDataWithParameter(param, "disc.DeleteRestrictedZone");

                    /////
                    //if (punishMast ==1)
                    //{
                    //    var PunMast = discCasePunishmentMasterService.GetById(p.PunishmentMasterId);
                    //    PunMast.IsActive = false;
                    //    PunMast.UpdateDate = DateTime.Now;
                    //    PunMast.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    //    discCasePunishmentMasterService.Update(PunMast);
                    //}

                }

            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertRestrictedZone(string EmployeeId, string hdnRestrictZoneId)
        {
            var Result = "Restricted Zone Could Not Save.";

            var param = new
            {
                EmployeeId = EmployeeId,
                RestrictedZoneId = hdnRestrictZoneId,
                CreateUser = (int)LoggedInOfficeID

            };
            var empList = employeeSPService.GetDataWithParameter(param, "disc.InsertRestrictedZone");



            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckRestrictedZone(string EmployeeId, string hdnRestrictZoneId)
        {
            var param = new
            {
                EmployeeId = EmployeeId,
                RestrictedZoneId = hdnRestrictZoneId,

            };
            var empList = employeeSPService.GetDataWithParameter(param, "disc.GetRestrictedZone");

            List<EmployeePostingHistoryViewModel> List_ViewModel = new List<EmployeePostingHistoryViewModel>();

            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new EmployeePostingHistoryViewModel
               {
                   ZoneId = row.Field<int>("ZoneId")

               }).ToList();
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCaseList(int jtStartIndex, int jtPageSize, string jtSorting, string zoOfc, string EmployeeID, string StatusId, string CaseNo, string CaseDate, string DespatchNo, string filterType)
        {
            StringBuilder sb = new StringBuilder();

            int? MasterId = 0;
            if (DespatchNo != "" && DespatchNo != null)
            {
                var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == DespatchNo && x.IsActive == true);

                foreach (var r in master)
                {
                    MasterId = Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()); ;//Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId) ? "0" : r.CaseMasterId);
                }
            }
            if (EmployeeID != null)
            {
                if (EmployeeID != "")
                {
                    sb.Append(" AND CD.EmployeeId = " + EmployeeID); //DE.EmployeeId
                }
            }
            if (filterType == "Explanation")
            {
                sb.Append(" AND CM.CaseType = 'D'");
            }
            if (filterType == "ChargeSheet")
            {
                sb.Append(" AND CM.CaseType = 'C'");
            }
            if (zoOfc != null && zoOfc != "" && zoOfc != "0")
                sb.Append(" AND Z.ZoneId = " + Convert.ToInt32(zoOfc));
            if (StatusId != null && StatusId != "" && StatusId != "0")
                sb.Append(" AND S.StatusId =" + Convert.ToInt32(StatusId));
            if (CaseNo != null && CaseNo != "")
                sb.Append(" AND CM.CaseNo = '" + CaseNo + "'");
            if (CaseDate != null && CaseDate != "")
                sb.Append(" AND CM.CaseDateFrom = '" + CaseDate + "'");
            if (MasterId != 0)
                sb.Append(" AND CM.CaseMasterId = " + MasterId);


            try
            {
                List<CaseEntryViewModel> List_DiscCaseMasterViewModel = new List<CaseEntryViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var caseList = employeeSPService.GetDataWithParameter(param, "disc.SP_Get_Case_List");

                List_DiscCaseMasterViewModel = caseList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseDetailsId = row.Field<int>("CaseDetailsId"),
                    ZoneName = row.Field<string>("ZoneName"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseDateToMsg = row.Field<string>("CaseDateTo"),
                    CaseNo = row.Field<string>("CaseNo"),
                    CaseType = row.Field<string>("CaseType"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),
                    TotBalanceMsg = row.Field<string>("TotBalanceMsg"),
                    OfficeName = row.Field<string>("OfficeName"),
                    Crimes = row.Field<string>("Crimes"),
                    Employees = row.Field<string>("Employess"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    StatusDtMsg = row.Field<string>("StatusDt"),
                    StatusMsg = row.Field<string>("StatusMsg"),
                    DispatchNo = row.Field<string>("DispatchNo"),


                }).ToList();

                var currentPageRecords = List_DiscCaseMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_DiscCaseMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetZoneName()//Return Zone List Without "Zonal Office" Words //kk
        {
            List<OfficeViewModel> List_ViewModel = new List<OfficeViewModel>();
            //var param = new { AndCondition = "" };

            var List = employeeSPService.GetDataWithoutParameter("disc.sp_GetZoneList");

            DataTable dt = new DataTable();
            dt = List.Tables[0];

            if (LoggedInOfficeType == 2)
            {
                var rows = from row in List.Tables[0].AsEnumerable()
                           where row.Field<int>("OfficeId") == LoggedInOfficeID
                           select row;

                if (rows.Count() > 0)
                {
                    dt = rows.CopyToDataTable();
                }
            }


            List_ViewModel = dt.AsEnumerable()
            .Select(row => new OfficeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("Zone")

            }).ToList();



            var Zones = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0}", x.OfficeName)
            });

            var Zones_items = new List<SelectListItem>();
            if (Zones.ToList().Count > 0)
            {
                Zones_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Zones_items.AddRange(Zones);
            return Json(Zones_items, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllOffice()//Return ALL Officce List Without  kk
        {
            List<OfficeViewModel> List_ViewModel = new List<OfficeViewModel>();
            //var param = new { AndCondition = "" };

            var List = employeeSPService.GetDataWithoutParameter("disc.sp_GetALLOfficeList");

            DataTable dt = new DataTable();
            dt = List.Tables[0];

            if (LoggedInOfficeType == 2)
            {
                var rows = from row in List.Tables[0].AsEnumerable()
                           where row.Field<int>("OfficeId") == LoggedInOfficeID
                           select row;

                if (rows.Count() > 0)
                {
                    dt = rows.CopyToDataTable();
                }
            }


            List_ViewModel = dt.AsEnumerable()
            .Select(row => new OfficeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("Zone")

            }).ToList();



            var Zones = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0}", x.OfficeName)
            });

            var Zones_items = new List<SelectListItem>();
            if (Zones.ToList().Count > 0)
            {
                Zones_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Zones_items.AddRange(Zones);
            return Json(Zones_items, JsonRequestBehavior.AllowGet);

        }




        public JsonResult GetCasePunishmentList(int jtStartIndex, int jtPageSize, string jtSorting, string FilterType, string FilterValue)
        {
            StringBuilder sb = new StringBuilder();

            int? MasterId = 0;
            var PDespatchNo = "";
            var CaseNo = "";
            var EmployeeCode = "";
            if (FilterType == "C") //case Despatch
            {
                if (FilterValue != "" && FilterValue != null)
                {
                    var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == FilterValue && x.IsActive == true);

                    foreach (var r in master)
                    {
                        MasterId = Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()); ;//Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId) ? "0" : r.CaseMasterId);
                    }
                }
            }
            else if (FilterType == "P") //punishment Despatch
            {
                PDespatchNo = FilterValue;
            }
            else if (FilterType == "CaseNo")
            {
                CaseNo = FilterValue;
            }
            else if (FilterType == "E") //Employee
            {
                EmployeeCode = FilterValue;
            }

            if (CaseNo != "" && CaseNo != null)
            {
                sb.Append(" AND M.CaseNo = '" + CaseNo + "'");
            }

            if (MasterId != 0)
            {
                sb.Append(" AND M.CaseMasterId =" + MasterId);
            }
            if (PDespatchNo != "")
            {
                sb.Append(" AND PM.DespatchNo = '" + PDespatchNo + "'");
            }
            if (EmployeeCode != "")
            {
                sb.Append(" AND PM.EmployeeId = (SELECT EmployeeId FROM Employee WHERE EmployeeCode = '" + EmployeeCode + "')");
            }
            if (FilterType == "ViewAll")
            {
                sb.Append(" ");
            }

            var office = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));

            
            try
            {

                List<CaseEntryViewModel> List_DiscCaseMasterViewModel = new List<CaseEntryViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var caseList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetCasePunishmentList2"); //SP_GetCasePunishmentList

                List_DiscCaseMasterViewModel = caseList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseNo = row.Field<string>("CaseNo"),
                    PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    Crimes = row.Field<string>("CrimeName"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    ZoneName = row.Field<string>("ZoneName"),
                    PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                    CaseType = row.Field<string>("CaseType"),
                    PunishmentName = row.Field<string>("PunishmentNameWithDate"),
                    CaseDesPatchNo = row.Field<string>("caseDespatchNo"),
                    EmployeeCode = row.Field<string>("EmployeeCode")
                }).ToList();

                var currentPageRecords = List_DiscCaseMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_DiscCaseMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetCasePunishmentListForLetter(int jtStartIndex, int jtPageSize, string jtSorting, string FilterType, string FilterValue)
        {
            StringBuilder sb = new StringBuilder();

            int? MasterId = 0;
            var PDespatchNo = "";
            var CaseNo = "";
            var EmployeeCode = "";
            if (FilterType == "C") //case Despatch
            {
                if (FilterValue != "" && FilterValue != null)
                {
                    var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == FilterValue && x.IsActive == true);

                    foreach (var r in master)
                    {
                        MasterId = Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString()); ;//Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId) ? "0" : r.CaseMasterId);
                    }
                }
            }
            else if (FilterType == "P") //punishment Despatch
            {
                PDespatchNo = FilterValue;
            }
            else if (FilterType == "CaseNo")
            {
                CaseNo = FilterValue;
            }
            else if (FilterType == "E") //Employee
            {
                EmployeeCode = FilterValue;
            }

            if (CaseNo != "" && CaseNo != null)
            {
                sb.Append(" AND vw.CaseNo = '" + CaseNo + "'");
            }

            if (MasterId != 0)
            {
                sb.Append(" AND vw.CaseMasterId =" + MasterId);
            }
            if (PDespatchNo != "")
            {
                sb.Append(" AND vw.DispatchNo = '" + PDespatchNo + "'");
            }
            if (EmployeeCode != "")
            {
                sb.Append(" AND vw.EmployeeId = (SELECT EmployeeId FROM Employee WHERE EmployeeCode = '" + EmployeeCode + "')");
            }
            if (FilterType == "ViewAll")
            {
                sb.Append(" ");
            }
            try
            {

                List<CaseEntryViewModel> List_DiscCaseMasterViewModel = new List<CaseEntryViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var caseList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetCasePunishmentList3"); //SP_GetCasePunishmentList2

                List_DiscCaseMasterViewModel = caseList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseNo = row.Field<string>("CaseNo"),
                    PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    Crimes = row.Field<string>("CrimeName"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    ZoneName = row.Field<string>("ZoneName"),
                    PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                    CaseType = row.Field<string>("CaseType"),
                    PunishmentName = row.Field<string>("PunishmentName")


                }).ToList();

                var currentPageRecords = List_DiscCaseMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_DiscCaseMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult GetPunishmentData(string CaseMasterId)//GetEmpInfoForEdit
        {

            try
            {
                List<CaseEntryViewModel> List_EmployeeViewModel = new List<CaseEntryViewModel>();
                var param = new { CaseMasterId = CaseMasterId };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "disc.SP_GetPunishmentwiseEmployeeListForLetter");
                List_EmployeeViewModel = OverdueMls.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {

                   CaseMasterId = row.Field<int>("CaseMasterId"),
                   CaseNo = row.Field<string>("CaseNo"),
                   PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeName = row.Field<string>("EmployeeNameBng"),
                   Crimes = row.Field<string>("Crimes"),
                   CrimeLocationName = row.Field<string>("CrimeLocationName"),
                   CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                   ZoneName = row.Field<string>("ZoneName"),
                   PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                   CaseType = row.Field<string>("CaseType"),
                   PunishmentName = row.Field<string>("PunishmentName")

               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }





        // SAVE Manual Data and Anulipi
        [HttpGet]
        public JsonResult SaveManualData(string CaseMasterId, string Manual1, string Manual2, string Manual3, string Manual4, string AnulipiDetailText)
        {
            var result = "OK";
            //@CaseMasterId AS bigint, @Manual1 AS nvarchar(50) ,@Manual12 AS nvarchar(50) ,@Manual13 AS nvarchar(50),@Manual14 AS nvarchar(50), @AnulipiDetailText AS nvarchar(500))
            try
            {
                var param = new
                {
                    @CaseMasterId = CaseMasterId,
                    @Manual1 = Manual1,
                    @Manual2 = Manual2,
                    @Manual3 = Manual3,
                    @Manual4 = Manual4,
                    @AnulipiDetailText = AnulipiDetailText
                };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.InsertDisciplineAunilipi");

            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }//



        public JsonResult GetEmployeeWiseCaseList(int jtStartIndex, int jtPageSize, string jtSorting, string EmployeeId)
        {
            try
            {
                long EmpId = 0;
                if (EmployeeId != "")
                {
                    EmpId = Convert.ToInt64(EmployeeId);
                }

                List<CaseEntryViewModel> List_DiscCaseMasterViewModel = new List<CaseEntryViewModel>();
                var param = new { EmployeeId = EmpId };
                var caseList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetEmployeeWiseCaseList");

                List_DiscCaseMasterViewModel = caseList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    ZoneName = row.Field<string>("ZoneName"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseDateToMsg = row.Field<string>("CaseDateTo"),
                    CaseNo = row.Field<string>("CaseNo"),
                    CaseType = row.Field<string>("CaseType"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    PunishmentName = row.Field<string>("PunishmentName"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),
                    TotBalanceMsg = row.Field<string>("TotBalanceMsg"),
                    OfficeName = row.Field<string>("OfficeName"),
                    Crimes = row.Field<string>("Crimes"),
                    Employees = row.Field<string>("Employess"),
                    PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                    PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                    ActivatedFromMsg = row.Field<string>("ActivatedDt"),
                    DaysLose = row.Field<int>("DaysLose")

                }).ToList();

                var currentPageRecords = List_DiscCaseMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_DiscCaseMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }




        public JsonResult GetCaseInfoByCaseNoPunishment(string case_no, string Despatch)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var CaseNo = "";
                List<CaseEntryViewModel> List_CaseViewModel = new List<CaseEntryViewModel>();

                List<CaseEntryViewModel> List_CaseViewModel2 = new List<CaseEntryViewModel>();


                if (case_no != "0")
                {

                    sb.Append("AND CM.CaseNo =" + case_no);

                }
                else
                {
                    var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == Despatch && x.IsActive == true);

                    foreach (var r in master)
                    {
                        CaseNo = string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString();//Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId) ? "0" : r.CaseMasterId);

                    }
                    sb.Append("AND CM.CaseNo= =" + CaseNo);
                }

                var param = new { AndCondition = sb.ToString() };
                var crimeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GET_Disc_CaseInfoByCaseNo_Punishment");




                List_CaseViewModel = crimeList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseNo = row.Field<string>("CaseNo"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseType = row.Field<string>("CaseType"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),

                    CrimeName = row.Field<string>("CrimeName"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFrom"),
                    AnnexationAmountMsg = row.Field<string>("AnnexationAmount")
                }).ToList();

                return Json(List_CaseViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCasewiseCrimeListPunishment(int jtStartIndex, int jtPageSize, string jtSorting, string case_no, string despatchNo, string employeeCode, string CaseDetailsIds = "")
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                long employeeId = 0;
                //DateTime? Embezzle_Date = null;
                if (despatchNo == "0")
                {
                    if (case_no != "" && case_no != "0" && case_no != null)
                    {
                        sb.Append(" AND CM.CaseNo = '" + case_no + "'");
                    }
                    if (employeeCode != "" && employeeCode != null)// AND F.UploadDate = ''09/05/2016'
                    {
                        var employee = employeeService.GetByCode(employeeCode);

                        if (employee != null)
                        {
                            employeeId = employee.EmployeeId;
                        }
                        //.EmployeeId;
                        //var Embezzle_Date = Convert.ToDateTime(EmbezzleDate);
                        sb.Append(" AND CD.EmployeeId =  " + employeeId);
                    }
                }
                else
                {
                    var CaseNo = "";
                    var master = discCaseDespatchNoService.GetAll().Where(x => x.DespatchNo == despatchNo && x.IsActive == true);

                    foreach (var r in master)
                    {
                        CaseNo = string.IsNullOrEmpty(r.CaseMasterId.ToString()) ? "0" : r.CaseMasterId.ToString();//Convert.ToInt32(string.IsNullOrEmpty(r.CaseMasterId) ? "0" : r.CaseMasterId);

                    }
                    sb.Append(" AND CM.CaseNo= '" + CaseNo + "'");
                }

                if (CaseDetailsIds != "") // New added: Khalid: Filter selected case.
                {

                    sb.Append(" AND CD.CaseDetailsId IN (" + CaseDetailsIds + ")");

                }

                List<CaseEntryViewModel> List_CaseViewModel = new List<CaseEntryViewModel>();
                List<CaseEntryViewModel> List_CaseViewModel2 = new List<CaseEntryViewModel>();

                var param = new { AndCondition = sb.ToString() };
                var crimeList = employeeSPService.GetDataWithParameter(param, "disc.SP_GET_Disc_CaseInfoByCaseNo_Punishment");




                List_CaseViewModel = crimeList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),//
                    CaseNo = row.Field<string>("CaseNo"),
                    CaseDetailsId = row.Field<int>("CaseDetailsId"),
                    CrimeId = row.Field<int>("CrimeId"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseType = row.Field<string>("CaseType"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),

                    CrimeName = row.Field<string>("CrimeName"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFrom"),
                    DispatchNo = row.Field<string>("DispatchNo"),
                    AnnexationAmountMsg = row.Field<string>("AnnexationAmount")
                }).ToList();

                /// NEW ADD Suspended List
                /// 

                var param2 = new { @EmployeeId = employeeId };

                var SuspendedList = employeeSPService.GetDataWithParameter(param2, "disc.SP_GET_Disc_CaseInfoByCaseNo_PunishmentOnlyTemporarySuspended");


                List_CaseViewModel2 = SuspendedList.Tables[0].AsEnumerable()
                .Select(row => new CaseEntryViewModel
                {
                    SlNo = row.Field<string>("SlNo"),
                    CaseMasterId = row.Field<int>("CaseMasterId"),
                    CaseNo = row.Field<string>("CaseNo"),
                    CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                    CaseType = row.Field<string>("CaseType"),
                    CrimeLocationName = row.Field<string>("CrimeLocationName"),
                    CaseDescription = row.Field<string>("CaseDescription"),
                    DealerName = row.Field<string>("DealerName"),
                    EnquiryName = row.Field<string>("EnquiryName"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    TotalAnnexationAmountMsg = row.Field<string>("TotAnnexationAmount"),
                    TotReturnAmountMsg = row.Field<string>("TotReturnAmount"),

                    CrimeName = row.Field<string>("CrimeName"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CrimeDateFromMsg = row.Field<string>("CrimeDateFrom"),
                    DispatchNo = row.Field<string>("DispatchNo"),
                    AnnexationAmountMsg = row.Field<string>("AnnexationAmount")
                }).ToList();


                List_CaseViewModel.AddRange(List_CaseViewModel2);


                var currentPageRecords = List_CaseViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_CaseViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetPunishmentList()
        {
            try
            {
                var offices = discPunishmentService.GetAll().Where(w => w.IsActive == true).Select(c => new { DisplayText = c.PunishmentName, Value = c.PunishmentId });
                return Json(new { Result = "OK", Options = offices });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




        public JsonResult GetDataPopupCaseEdit(string MasterId, string CaseDetailsId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                List<CaseEntryViewModel> List_ViewModel = new List<CaseEntryViewModel>();
                var param = new { masterId = MasterId, CaseDetailsId = CaseDetailsId };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.GetCasePopupCaseDataEdit"); // in punishment popup: GetCasePopupDataEdit

                List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {
                   CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                   CrimeDateFromMsg = row.Field<string>("CaseDateTo"),

                   ////NEW FIELDS
                   EmployeeId = row.Field<Int64>("EmployeeId"),
                   CrimeLocationId = row.Field<Int32>("CrimeLocationId"),
                   CrimeLocationBng = row.Field<string>("CrimeLocationBng"),
                   DealOfficerId = row.Field<Int64>("DealOfficerId"),

                   CaseDesPatchNo = row.Field<string>("CaseDesPatchNo"),
                   CrimeName = row.Field<string>("CrimeName"),
                   CrimeId = row.Field<int>("CrimeId")

                   //// END NEW FIELDS


               }).ToList();

                //if (WorkAreaId != null)
                //{
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}


                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                // return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        public JsonResult GetDataPopup(string MasterId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                List<CaseEntryViewModel> List_ViewModel = new List<CaseEntryViewModel>();
                var param = new { masterId = MasterId };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.GetCasePopupData");

                List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {
                   CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                   CrimeDateFromMsg = row.Field<string>("CaseDateTo"),
                   PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                   PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                   FirstIncSuspendDtmsg = row.Field<string>("FirstIncSuspendDt"),
                   SecondIncSuspendDtmsg = row.Field<string>("SecondIncSuspendDt"),
                   ThirdIncSuspendDtmsg = row.Field<string>("ThirdIncSuspendDt"),
                   FourthIncSuspendDtmsg = row.Field<string>("FourthIncSuspendDt"),
                   DaysLose = row.Field<Int32>("DaysLoss")

               }).ToList();

                //if (WorkAreaId != null)
                //{
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}


                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                // return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        public JsonResult UpdatePOPUPCaseDataEdit(string masterId, string CaseDateFrom, string CrimeLocationId = null, string CaseDesPatchNo = null, string CaseDetailsId = null, string CrimeId = null, string EmployeeId = null)
        {
            var officeId = Convert.ToInt32(LoggedInOfficeID.ToString());

            if (officeId != 2664 && officeId != 2673)
            {
                Response.StatusCode = 403;

                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;


                var param = new
                {
                    masterId = masterId,

                    CaseDateFrom = CaseDateFrom,

                    CrimeLocationId = CrimeLocationId,

                    CaseDesPatchNo = CaseDesPatchNo,
                    CaseDetailsId = CaseDetailsId,
                    CrimeId = CrimeId,
                    EmployeeId = EmployeeId

                };
                var val = employeeSPService.GetDataWithParameter(param, "disc.UpdatePoppupCaseDataEDIT");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataPopupEdit(string MasterId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                List<CaseEntryViewModel> List_ViewModel = new List<CaseEntryViewModel>();
                var param = new { masterId = MasterId };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.GetCasePopupDataEdit");

                List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {
                   CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                   CrimeDateFromMsg = row.Field<string>("CaseDateTo"),
                   PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                   PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                   FirstIncSuspendDtmsg = row.Field<string>("FirstIncSuspendDt"),
                   SecondIncSuspendDtmsg = row.Field<string>("SecondIncSuspendDt"),
                   ThirdIncSuspendDtmsg = row.Field<string>("ThirdIncSuspendDt"),
                   FourthIncSuspendDtmsg = row.Field<string>("FourthIncSuspendDt"),
                   DaysLose = row.Field<Int32>("DaysLoss"),

                   ////NEW FIELDS
                   CrimeLocationId = row.Field<Int32>("CrimeLocationId"),
                   CrimeLocationBng = row.Field<string>("CrimeLocationBng"),
                   DealOfficerId = row.Field<Int64>("DealOfficerId"),
                   PunishmentId = row.Field<Int32>("PunishmentId"),
                   PunishmentName = row.Field<string>("PunishmentName"),
                   CaseDesPatchNo = row.Field<string>("CaseDesPatchNo"),
                   //// END NEW FIELDS

                   AnnexationAmount = row.Field<decimal>("AnnexationAmount"),
                   ReturnAmount = row.Field<decimal>("ReturnAmount")


               }).ToList();

                //if (WorkAreaId != null)
                //{
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}


                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                // return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        public JsonResult GetPunishmentNameByID(string punishmentId)
        {

            try
            {
                List<DiscPunishmentViewModel> List_EmployeeViewModel = new List<DiscPunishmentViewModel>();
                var param = new { @PunishmentId = punishmentId };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "disc.GetPunishmentNameById");
                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new DiscPunishmentViewModel
               {
                   PunishmentName = row.Field<string>("PunishmentName")

               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCrimeLocationNameByID(string CrimeLocationId)
        {

            try
            {
                List<CaseEntryViewModel> List_EmployeeViewModel = new List<CaseEntryViewModel>();
                var param = new { @OfficeId = CrimeLocationId };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "disc.GetOfficeNameById");
                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {

                   CrimeLocationBng = row.Field<string>("CrimeLocationBng")

               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCrimeNameByID(string CrimeId)
        {

            try
            {
                List<CaseEntryViewModel> List_EmployeeViewModel = new List<CaseEntryViewModel>();
                var param = new { @CrimeId = CrimeId };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "disc.GetCrimeNameById");
                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {

                   CrimeName = row.Field<string>("CrimeName")

               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        ///UpdatePoppupData(@masterId INT, @PunishmentDespatchNo nvarchar(1000), 
        //          @CaseDateFrom date, @CaseDateTo date, @PunishmentDate date, 
        //          @FirstIncSuspendDt Date, @SecondIncSuspendDt date, @ThirdIncSuspendDt date, @FourthIncSuspendDt date

        public JsonResult UpdatePOPUPData(string masterId, string PunishmentDespatchNo, string CaseDateFrom, string CaseDateTo, string PunishmentDate, int daysLoss = 0, string FirstIncSuspendDt = null, string SecondIncSuspendDt = null, string ThirdIncSuspendDt = null, string FourthIncSuspendDt = null)
        {
            var officeId = Convert.ToInt32(LoggedInOfficeID.ToString());
            
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;

                if (FirstIncSuspendDt == "")
                {
                    FirstIncSuspendDt = null;
                }

                if (SecondIncSuspendDt == "")
                {
                    SecondIncSuspendDt = null;
                }
                if (ThirdIncSuspendDt == "")
                {
                    ThirdIncSuspendDt = null;

                }
                if (FourthIncSuspendDt == "")
                {
                    FourthIncSuspendDt = null;
                }
                if (CaseDateTo == "")
                {
                    CaseDateTo = null;
                }


                var param = new
                {
                    masterId = masterId,
                    PunishmentDespatchNo = PunishmentDespatchNo,
                    CaseDateFrom = CaseDateFrom,
                    CaseDateTo = CaseDateTo,
                    PunishmentDate = PunishmentDate,
                    daysLoss = daysLoss,
                    FirstIncSuspendDt = FirstIncSuspendDt,
                    SecondIncSuspendDt = SecondIncSuspendDt,
                    ThirdIncSuspendDt = ThirdIncSuspendDt,
                    FourthIncSuspendDt = FourthIncSuspendDt

                };
                var val = employeeSPService.GetDataWithParameter(param, "disc.UpdatePoppupData");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePOPUPDataEdit(string masterId, string PunishmentDespatchNo, string CaseDateFrom, string CaseDateTo, string PunishmentDate, int daysLoss = 0, string FirstIncSuspendDt = null, string SecondIncSuspendDt = null, string ThirdIncSuspendDt = null, string FourthIncSuspendDt = null, string CrimeLocationId = null, string PunishmentId = null, string CaseDesPatchNo = null,
       string txtAnnexationAmount = "0",
       string txtTotalAnnexationAmount = "0",
       string txtIndiReturnAmountMsg = "0",
       string txtTotReturnAmount = "0"

            )
            {

            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;

                if (FirstIncSuspendDt == "")
                {
                    FirstIncSuspendDt = null;
                }

                if (SecondIncSuspendDt == "")
                {
                    SecondIncSuspendDt = null;
                }
                if (ThirdIncSuspendDt == "")
                {
                    ThirdIncSuspendDt = null;

                }
                if (FourthIncSuspendDt == "")
                {
                    FourthIncSuspendDt = null;
                }
                if (CaseDateTo == "")
                {
                    CaseDateTo = null;
                }


                if (txtAnnexationAmount == "") { txtAnnexationAmount = "0"; }
                if (txtTotalAnnexationAmount == "") { txtTotalAnnexationAmount = "0"; }
                if (txtIndiReturnAmountMsg == "") { txtIndiReturnAmountMsg = "0"; }
                if (txtTotReturnAmount == "") { txtTotReturnAmount = "0"; }


                int AnnexationAmount = Convert.ToInt32(txtAnnexationAmount) + Convert.ToInt32(txtTotalAnnexationAmount);
                int ReturnAmount = Convert.ToInt32(txtIndiReturnAmountMsg) + Convert.ToInt32(txtTotReturnAmount);


                var param = new
                {
                    masterId = masterId,
                    PunishmentDespatchNo = PunishmentDespatchNo,
                    CaseDateFrom = CaseDateFrom,
                    CaseDateTo = CaseDateTo,
                    PunishmentDate = PunishmentDate,
                    daysLoss = daysLoss,
                    FirstIncSuspendDt = FirstIncSuspendDt,
                    SecondIncSuspendDt = SecondIncSuspendDt,
                    ThirdIncSuspendDt = ThirdIncSuspendDt,
                    FourthIncSuspendDt = FourthIncSuspendDt,
                    CrimeLocationId = CrimeLocationId,
                    PunishmentId = PunishmentId,
                    CaseDesPatchNo = CaseDesPatchNo,
                    AnnexationAmount = AnnexationAmount,
                    ReturnAmount = ReturnAmount


                };
                var val = employeeSPService.GetDataWithParameter(param, "disc.UpdatePoppupDataEDIT");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDataPopupPunishmentEntryForm(string MasterId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                List<CaseEntryViewModel> List_ViewModel = new List<CaseEntryViewModel>();
                var param = new { masterId = MasterId };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.GetCasePopupDataPunishmentEntryForm");

                List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {
                   CaseDateFromMsg = row.Field<string>("CaseDateFrom"),
                   CrimeDateFromMsg = row.Field<string>("CaseDateTo"),
                   PunishmentDateMsg = row.Field<string>("PunishmentDate"),
                   PunishmentDespatchNo = row.Field<string>("PunishmentDespatchNo"),
                   FirstIncSuspendDtmsg = row.Field<string>("FirstIncSuspendDt"),
                   SecondIncSuspendDtmsg = row.Field<string>("SecondIncSuspendDt"),
                   ThirdIncSuspendDtmsg = row.Field<string>("ThirdIncSuspendDt"),
                   FourthIncSuspendDtmsg = row.Field<string>("FourthIncSuspendDt"),
                   DaysLose = row.Field<Int32>("DaysLoss")

               }).ToList();

                //if (WorkAreaId != null)
                //{
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}


                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                // return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        public JsonResult UpdatePOPUPDataPunishmentEntryForm(string masterId, string PunishmentDespatchNo, string CaseDateFrom, string CaseDateTo, string PunishmentDate, int daysLoss = 0, string FirstIncSuspendDt = null, string SecondIncSuspendDt = null, string ThirdIncSuspendDt = null, string FourthIncSuspendDt = null)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;

                if (FirstIncSuspendDt == "")
                {
                    FirstIncSuspendDt = null;
                }
                if (SecondIncSuspendDt == "")
                {
                    SecondIncSuspendDt = null;
                }
                if (ThirdIncSuspendDt == "")
                {
                    ThirdIncSuspendDt = null;
                }
                if (FourthIncSuspendDt == "")
                {
                    FourthIncSuspendDt = null;
                }
                if (CaseDateTo == "")
                {
                    CaseDateTo = null;
                }

                var param = new
                {
                    masterId = masterId,
                    PunishmentDespatchNo = PunishmentDespatchNo,
                    CaseDateFrom = CaseDateFrom,
                    CaseDateTo = CaseDateTo,
                    PunishmentDate = PunishmentDate,
                    daysLoss = daysLoss,
                    FirstIncSuspendDt = FirstIncSuspendDt,
                    SecondIncSuspendDt = SecondIncSuspendDt,
                    ThirdIncSuspendDt = ThirdIncSuspendDt,
                    FourthIncSuspendDt = FourthIncSuspendDt

                };
                var val = employeeSPService.GetDataWithParameter(param, "disc.UpdatePoppupDataPunishmentEntryForm");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAnulipiText()
        {
            try
            {
                List<CaseEntryViewModel> Result = new List<CaseEntryViewModel>();
                var empAnulipiList = employeeSPService.GetDataWithoutParameter("disc.SP_GetAnulipiText");
                Result = empAnulipiList.Tables[0].AsEnumerable()
               .Select(row => new CaseEntryViewModel
               {
                   AnulipiId = row.Field<int>("AnulipiId"),
                   AnulipiText = row.Field<string>("AnulipiTextBn")

               }).ToList();


                var Anulipi = Result.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.AnulipiId.ToString(),
                    Text = string.Format("{0}", x.AnulipiText)
                });

                var Anulipi_items = new List<SelectListItem>();
                if (Anulipi.ToList().Count > 0)
                {
                    Anulipi_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                Anulipi_items.AddRange(Anulipi);


                return Json(Anulipi_items, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion






    }
}
