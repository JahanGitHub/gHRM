using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Service;
using gHRM.Service.Discipline;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class DiscMemorendumEntryController : BaseController
    {
        #region Variables
        private readonly IDisCrimeService discCrimeService;
        private readonly IDiscMemorendumDetailsService discMemorendumDetailsService;
        private readonly IDiscMemorendumMasterService discMemorendumMasterService;        
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDesignationService employeeDesignationService;        
        private readonly IEmployeeSPService employeeSPService;
        private readonly IDiscPunishmentService discPunishmentService;

        public DiscMemorendumEntryController(IDisCrimeService discCrimeService, IDiscMemorendumDetailsService discMemorendumDetailsService, IDiscMemorendumMasterService discMemorendumMasterService, IEmployeeService employeeService, IEmployeeDesignationService employeeDesignationService, IEmployeeSPService employeeSPService, IDiscPunishmentService discPunishmentService)
        {
            this.discCrimeService = discCrimeService;
            this.discMemorendumDetailsService = discMemorendumDetailsService;
            this.discMemorendumMasterService = discMemorendumMasterService;
            this.employeeService = employeeService;
            this.employeeDesignationService = employeeDesignationService;            
            this.employeeSPService = employeeSPService;            
            this.discPunishmentService = discPunishmentService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(DiscMemorendumMasterViewModel model)
        {
            var crimeList = discCrimeService.GetAll();
            var crimeListDetails = crimeList.Select(m => new SelectListItem() { Text = string.Format("{0}  {1}", m.CrimeCode, m.CrimeName), Value = m.CrimeId.ToString() });
            var dicsCrimeList = new List<SelectListItem>();
            dicsCrimeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            dicsCrimeList.AddRange(crimeListDetails);
            model.CrimeList = dicsCrimeList;

            var punishmentList = discPunishmentService.GetAll();
            var punishmentListDetails = punishmentList.Select(m => new SelectListItem() { Text = string.Format("{0}  {1}", m.PunishmentCode, m.PunishmentName), Value = m.PunishmentId.ToString() });
            var dicspunishmentList = new List<SelectListItem>();
            dicspunishmentList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            dicspunishmentList.AddRange(punishmentListDetails);
            model.PunishmentList = dicspunishmentList;
        }
        public ActionResult SaveMemorendum(Dictionary<string, string> allEmoloyeeIds, Dictionary<string, string> allDispatchNos, Dictionary<string, string> allCrimeIds, Dictionary<string, string> allPunishmentIds, Dictionary<string, string> allRemarks, string MemorendumNo, string MemorendumDate)
        {
            try
            {
                var EmployeeIdAndDispatchNo = allEmoloyeeIds.Zip(allDispatchNos, (e, d) => new { allEmoloyeeIds = e, allDispatchNos = d });
                var EmployeeIdAndDispatchNoAndPunishmentId = allPunishmentIds.Zip(EmployeeIdAndDispatchNo, (p, ed) => new { allPunishmentIds = p, EmployeeIdAndDispatchNo = ed });
                var EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId = allCrimeIds.Zip(EmployeeIdAndDispatchNoAndPunishmentId, (c, edp) => new { allCrimeIds = c, EmployeeIdAndDispatchNoAndPunishmentId = edp });
                var EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeIdAndRemarks = allRemarks.Zip(EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId, (r, edpc) => new { allRemarks = r, EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId = edpc });

                foreach (var emp in allEmoloyeeIds.Values.Distinct())
                {
                    var EmpDetail = EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeIdAndRemarks.Where(w => w.EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId.EmployeeIdAndDispatchNoAndPunishmentId.EmployeeIdAndDispatchNo.allEmoloyeeIds.Value == emp).FirstOrDefault();

                    DiscMemorendumMaster mEntry = new DiscMemorendumMaster();
                    mEntry.MemorendumNo = MemorendumNo;
                    mEntry.MemorendumDate = Convert.ToDateTime(MemorendumDate);
                    mEntry.EmployeeId = Convert.ToInt64(EmpDetail.EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId.EmployeeIdAndDispatchNoAndPunishmentId.EmployeeIdAndDispatchNo.allEmoloyeeIds.Value);
                    mEntry.DispatchNo = EmpDetail.EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId.EmployeeIdAndDispatchNoAndPunishmentId.EmployeeIdAndDispatchNo.allDispatchNos.Value;
                    mEntry.PunishmentId = Convert.ToInt32(EmpDetail.EmployeeIdAndDispatchNoAndPunishmentIdAndCrimeId.EmployeeIdAndDispatchNoAndPunishmentId.allPunishmentIds.Value);
                    mEntry.IsPunishmentRunning = true;
                    mEntry.IsActive = true;
                    mEntry.CreateUser = LoggedInEmployeeId;
                    mEntry.CreateDate = DateTime.Now;
                    var discMemorendumMasterId = discMemorendumMasterService.Create(mEntry);

                    if (Convert.ToInt32(discMemorendumMasterId) > 0) // DiscMamoMasterSave
                    {
                        var EmployeeIdAndCrimeId = allEmoloyeeIds.Zip(allCrimeIds, (e, c) => new { allEmoloyeeIds = e, allCrimeIds = c });
                        var EmployeeIdAndCrimeIdAndRemarks = allRemarks.Zip(EmployeeIdAndCrimeId, (r, ec) => new { allRemarks = r, EmployeeIdAndCrimeId = ec });
                        var EmpCrime = EmployeeIdAndCrimeIdAndRemarks.Where(w => w.EmployeeIdAndCrimeId.allEmoloyeeIds.Value == emp);

                        foreach (var ed in EmpCrime)
                        {
                            DiscMemorendumDetail dEntry = new DiscMemorendumDetail();
                            dEntry.MemorendumMasterId = Convert.ToInt32(discMemorendumMasterId);
                            dEntry.CrimeId = Convert.ToInt32(ed.EmployeeIdAndCrimeId.allCrimeIds.Value);
                            dEntry.Remarks = ed.allRemarks.Value;
                            dEntry.IsActive = true;
                            dEntry.CreateUser = LoggedInEmployeeId;
                            dEntry.CreateDate = DateTime.Now;
                            discMemorendumDetailsService.Create(dEntry);                            
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
            DiscMemorendumMasterViewModel model = new DiscMemorendumMasterViewModel();
            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
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
