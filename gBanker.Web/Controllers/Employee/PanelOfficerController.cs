using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Transactions;
namespace gHRM.Web.Controllers
{
    public class PanelOfficerController : BaseController
    {
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeService employeeService;
        private readonly IPanelOfficerService panelOfficerService;
        private readonly IPanelOfficerHistoryService panelOfficerHistoryService;
        public PanelOfficerController(IEmployeeSPService employeeSPService
            , IOfficeDesignationService officeDesignationService
            , IOfficeService officeService
            , IEmployeeService employeeService
            , IPanelOfficerService panelOfficerService
            , IPanelOfficerHistoryService panelOfficerHistoryService
            )
        {
            this.employeeSPService = employeeSPService;
            this.officeDesignationService = officeDesignationService;
            this.officeService = officeService;
            this.employeeService = employeeService;
            this.panelOfficerService = panelOfficerService;
            this.panelOfficerHistoryService = panelOfficerHistoryService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new PanelOfficerViewModel();
            mapDropdownList(model);
            return View(model);
        }

        private void mapDropdownList(PanelOfficerViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var pleaseSelectList=new List<SelectListItem>();
            pleaseSelectList.Add(pleaseSelect);
            var viewRankList=new List<SelectListItem>();
            var employeeRankList = officeDesignationService.GetMany(o => o.IsActive == true);
            var viewList = employeeRankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text=row.OffcDesignName,
                Value = row.OfficeDesignationId.ToString()
                
            }).ToList();
            viewRankList.Add(pleaseSelect);
            viewRankList.AddRange(viewList);
            model.EmployeeRankList = viewRankList;

            var viewZoneList = new List<SelectListItem>();
            var zoneList = officeService.GetMany(o => o.IsActive == true && o.OfficeTypeId==4);
            var viewZone = zoneList.AsEnumerable().Select(row => new SelectListItem
            {
                Text =row.OfficeName,
                Value = row.OfficeId.ToString()
            }).ToList();
            viewZoneList.Add(pleaseSelect);
            viewZoneList.AddRange(viewZone);
            model.ZoneList = viewZoneList;


            model.EmployeeList = pleaseSelectList;
        }

        public JsonResult getOfficeEmployeeList(string EmployeeRank)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var EmployeeList = new List<SelectListItem>();
            EmployeeList.Add(pleaseSelect);
            try
            {
                var empList = employeeService.GetMany(x => x.IsActive == true && x.EmployeeRank.Trim() == EmployeeRank.Trim());
                foreach (var item in empList)
                {
                    var listItem = new SelectListItem { Text = item.EmployeeCode + " - " + item.EmployeeName, Value = item.EmployeeId.ToString() };
                    EmployeeList.Add(listItem);
                }
            }
            catch (Exception e)
            {                
                throw;
            }
            return Json(EmployeeList.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneWiseUnitForPanelOfficer(int zoneId)
        {
            var param = new { OfficeId = zoneId };
            var unitList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetAllUnassignedUnitsByZone");
            var untiListForPanelOfficer = unitList.Tables[0].AsEnumerable().Select(row => new PanelOfficerViewModel()
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("OfficeName"),
               
            }).ToList();
            return Json(untiListForPanelOfficer, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AssignUnitListToPanelOfficer(long EmployeeId, string AssignDt, List<PanelOfficer> AssignedUnitList)//,string AssignDt,List<string>UnitIdList
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    var entity = new PanelOfficer();
                    var entityHistory = new PanelOfficerHistory();
                    entity.EmployeeId = EmployeeId;
                    entity.AssignDt = Convert.ToDateTime(AssignDt);
                    entity.CreateBy = Convert.ToInt64(LoggedInEmployeeId);
                    entity.CreatrDate = DateTime.Now;

                    entityHistory.EmployeeId = EmployeeId;
                    var dbList = panelOfficerService.GetAll();
                   
                    foreach (var unitList in AssignedUnitList)
                    {
                        entity.OfficeId = unitList.OfficeId;
                        List<PanelOfficer> IsExistPanelOfficer = new List<PanelOfficer>();



                        IsExistPanelOfficer = dbList.Where(b => b.OfficeId == unitList.OfficeId).ToList();

                        if (IsExistPanelOfficer.Count() == 0)
                        {
                            var pInfo = panelOfficerService.Create(entity);

                            entityHistory.EmployeeId = pInfo.EmployeeId;
                            entityHistory.ID = pInfo.ID;
                            entityHistory.CreateBy = Convert.ToInt64(LoggedInEmployeeId);
                            entityHistory.CreateDate = DateTime.Now;
                            entityHistory.AssignDt = Convert.ToDateTime(AssignDt);
                            entityHistory.OfficeId = unitList.OfficeId;
                            panelOfficerHistoryService.Create(entityHistory);
                        }
                        else
                        {
                            var panelOfficerInfo = panelOfficerService.GetById(IsExistPanelOfficer.FirstOrDefault().ID);

                            var previousOffInfo = panelOfficerInfo; // Previous EMployee.

                            panelOfficerInfo.OfficeId = unitList.OfficeId;
                            panelOfficerInfo.EmployeeId = EmployeeId;
                            panelOfficerInfo.AssignDt = Convert.ToDateTime(AssignDt);
                            panelOfficerInfo.ReleaseDt = null;
                            panelOfficerInfo.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                            panelOfficerInfo.UpdateDate = DateTime.Now;
                            panelOfficerService.Update(panelOfficerInfo);

                            var xList = panelOfficerHistoryService.GetMany(b => b.EmployeeId == previousOffInfo.EmployeeId && b.OfficeId == previousOffInfo.OfficeId && b.ID == previousOffInfo.ID).FirstOrDefault();
                            if (xList != null)
                            {
                                xList.ReleaseDt = Convert.ToDateTime(AssignDt);
                                xList.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                                xList.UpdateDate = DateTime.Now;
                                panelOfficerHistoryService.Update(xList);
                            }
                            entityHistory.ID = panelOfficerInfo.ID;
                            entityHistory.CreateBy = Convert.ToInt64(LoggedInEmployeeId);
                            entityHistory.CreateDate = DateTime.Now;
                            entityHistory.AssignDt = Convert.ToDateTime(AssignDt);
                            entityHistory.OfficeId = unitList.OfficeId;
                            panelOfficerHistoryService.Create(entityHistory);
                        }

                    }
                    ts.Complete();
                    var data = 1;
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    ts.Dispose();
                    var data = 0;
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }//UnassignUnitListToPanelOfficer
       [HttpPost]
        public JsonResult UnassignUnitListToPanelOfficer(long EmployeeId, List<PanelOfficer> UnassignedUnitList)//,string AssignDt,List<string>UnitIdList
        {
            try
            {
                foreach (var unitList in UnassignedUnitList)
                {
                    //var history = getCommonDropDownService.GetUnReleasePanelOfficerList(EmployeeId, unitList.UnitId).FirstOrDefault();
                    var entity = panelOfficerService.Get(p=>p.OfficeId==unitList.OfficeId);
                    entity.ReleaseDt = DateTime.Now;
                    entity.UpdateBy = LoggedInEmployeeId;
                    entity.UpdateDate = DateTime.Now;
                    panelOfficerService.Update(entity);

                    var history = panelOfficerHistoryService.Get(h => h.EmployeeId == EmployeeId && h.OfficeId == unitList.OfficeId && h.ReleaseDt == null);
                    //var xList = panelOfficerHistoryService.GetById(history.HistoryId);
                    if (history != null)
                    {
                        history.ReleaseDt = DateTime.Now;
                        history.UpdateBy = LoggedInEmployeeId;
                        history.UpdateDate = DateTime.Now;
                        panelOfficerHistoryService.Update(history);
                    }
                }
                var data = 1;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var data = 0;
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetAssignedUnitListToPanelOfficer(int EmployeeId)
        {
            var param = new { EmployeeId = EmployeeId };
            var unitList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetAssignedUnitsToPanelOfficer");
            var assignedUntiListForPanelOfficer = unitList.Tables[0].AsEnumerable().Select(row => new PanelOfficerViewModel()
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("OfficeName")                
            }).ToList();
            return Json(assignedUntiListForPanelOfficer, JsonRequestBehavior.AllowGet);
        }//
    }
}