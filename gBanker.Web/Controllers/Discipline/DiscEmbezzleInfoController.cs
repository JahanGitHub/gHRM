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
using System.Text;
using BasicDataAccess;
using gHRM.Service.Discipline;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Web.Controllers
{
    public class DiscEmbezzleInfoController : Controller
    {

        #region Variables

        private readonly IDiscEmbezzleService discEmbezzleService;
        private readonly IDiscCaseMasterService discCaseMasterService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IDiscEmbezzleEmpInfoService discEmbezzleEmpInfoService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;


        public DiscEmbezzleInfoController(IDiscEmbezzleService discEmbezzleService, IDiscCaseMasterService discCaseMasterService, IEmployeeSPService employeeSPService, IDiscEmbezzleEmpInfoService discEmbezzleEmpInfoService, IOfficeService officeService,IOfficeTypeService officeTypeService)
        {
            this.discEmbezzleService = discEmbezzleService;
            this.discCaseMasterService = discCaseMasterService;
            this.employeeSPService = employeeSPService;
            this.discEmbezzleEmpInfoService = discEmbezzleEmpInfoService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
        }

        #endregion

        #region Methods

        public JsonResult EmbezzleInfoDelete(string EmbezzleId)
        {
            var entity = discEmbezzleService.GetById(Convert.ToInt32(EmbezzleId));
            string Result = "OK";
            if (ModelState.IsValid)
            {
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateDate = DateTime.Now;
                discEmbezzleService.Update(entity);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZOOfficeListForEmbezzle()//SP_GetOfficeNameForEmbezzle
        {
            List<OfficeViewModel> List_OfficeViewModel = new List<OfficeViewModel>();
            var empList = employeeSPService.GetDataWithoutParameter("SP_GetOfficeNameForEmbezzle");

            List_OfficeViewModel = empList.Tables[0].AsEnumerable()
            .Select(row => new OfficeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("OfficeName"),
            }).ToList();
            return Json(List_OfficeViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AccusedDelete(string EmbezzleEmpId)
        {
            var result = 1;
            var Embe = discEmbezzleEmpInfoService.GetById(Convert.ToInt32(EmbezzleEmpId));
            Embe.IsActive = false;
            Embe.UpdateDate = DateTime.Now;
            Embe.UpdateUser = SessionHelper.LoggedInEmployeeID;
            discEmbezzleEmpInfoService.Update(Embe);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmbezzleEmplyeeList(string EmbezzleId)
        {

            try
            {
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { EmbezzleId = EmbezzleId };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "disc.SP_GetEmbezzleEmplyeeList");
                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeViewModel
               {
                   EmbezzleEmpId = row.Field<int>("EmbezzleEmpId"),
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   OfficeName = row.Field<string>("OfficeName"),
                   DesignationName = row.Field<string>("DesignationName"),

               }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AccusedListSave(string EmbezzleId, string EmployeeId)
        {
            try
            {
                var result = 0;
                DiscEmbezzleEmpInfo EmbezzleEmpInfo = new DiscEmbezzleEmpInfo();
                EmbezzleEmpInfo.EmployeeId = Convert.ToInt64(EmployeeId);
                EmbezzleEmpInfo.EmbezzleId = Convert.ToInt32(EmbezzleId);
                EmbezzleEmpInfo.IsActive = true;
                EmbezzleEmpInfo.CreateDate = DateTime.Now;
                EmbezzleEmpInfo.CreateUser = SessionHelper.LoggedInEmployeeID;
                discEmbezzleEmpInfoService.Create(EmbezzleEmpInfo);
                result = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetEmbezzleListInfo(int jtStartIndex, int jtPageSize, string jtSorting, string OfficeId, string EmbezzleDatefrm, string EmbezzleDateto, string EmbezzleOrderBy)
        {
            try
            {  //File Type= Upload type
                StringBuilder sb = new StringBuilder();


                //DateTime? Embezzle_Date = null;



                if (OfficeId != "" && OfficeId != "0" && OfficeId != null)
                {

                    //New Add
                    //if Zone Get All OfficeId under it
                    var myOffice = officeService.GetById(Convert.ToInt32(OfficeId));
                    string query = "";

                    if (myOffice.OfficeTypeId == 2) //Zone
                    {
                        query = "Select OfficeId From Office Where SecondLevel =" + myOffice.OfficeCode;
                    }
                    else if (myOffice.OfficeTypeId == 4) //Area
                    {
                        query = "Select OfficeId From Office Where ThirdLevel =" + myOffice.OfficeCode;
                    }
                    else if (myOffice.OfficeTypeId == 5) //Branch
                    {
                        query = "Select OfficeId From Office Where FourthLevel =" + myOffice.OfficeCode;
                    }


                    //New Add


                    //sb.Append("AND EM.OfficeId =" + OfficeId);
                    sb.Append("AND EM.OfficeId IN (" + query + ")");
                }
                if (EmbezzleDatefrm != "" && EmbezzleDatefrm != null && EmbezzleDateto != "" && EmbezzleDateto != null)// AND F.UploadDate = ''09/05/2016'
                {
                    var Embezzle_Datefrm = Convert.ToDateTime(EmbezzleDatefrm);
                    sb.Append("AND EM.EmbezzleRcvDt BETWEEN '" + Embezzle_Datefrm + "' AND '" + EmbezzleDateto + "'  ");
                    // sb.Append("AND EM.EmbezzleRcvDt = '" + Embezzle_Datefrm + "'");

                }

                if (EmbezzleOrderBy != "" && EmbezzleOrderBy != "0" && EmbezzleOrderBy != null)
                {
                    //EM.TotEmbezzledAmount
                    if (EmbezzleOrderBy == "1")
                    {
                        sb.Append(" ORDER BY EM.TotEmbezzledAmount ASC");
                    }
                    else if (EmbezzleOrderBy == "2")
                    {
                        sb.Append(" ORDER BY EM.TotEmbezzledAmount DESC");
                    }



                }
                else
                {
                    sb.Append(" ORDER BY EM.EmbezzleId DESC");
                }


                List<DiscEmbezzleInfoViewModel> List_DiscEmbezzleInfoViewModel = new List<DiscEmbezzleInfoViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "disc.SP_Get_EmbezzleInfo");

                List_DiscEmbezzleInfoViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DiscEmbezzleInfoViewModel
                {
                    rowSl = row.Field<long>("rowSl"),
                    EmbezzleId = row.Field<int>("EmbezzleId"),
                    EmbezzleRcvDtMsg = row.Field<string>("EmbezzleRcvDtMsg"),
                    AuditDateFromMsg = row.Field<string>("AuditDateFromMsg"),
                    OfficeName = row.Field<string>("OfficeName"),
                    AuditDateToMsg = row.Field<string>("AuditDateToMsg"),

                    BranchAuditNo = row.Field<string>("BranchAuditNo"),
                    NoOfBMAccused = row.Field<int?>("NoOfBMAccused"),
                    NoOfSignatoryAccussed = row.Field<int?>("NoOfSignatoryAccussed"),
                    TotEmbezzledAmount = row.Field<decimal?>("TotEmbezzledAmount"),
                    NoOfCMAccussed = row.Field<int?>("NoOfCMAccussed"),
                    TotReturnAmount = row.Field<decimal?>("TotReturnAmount"),
                    Balance = row.Field<decimal?>("Balance"),
                    Remarks = row.Field<string>("Remarks")

                }).ToList();

                var currentPageRecords = List_DiscEmbezzleInfoViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_DiscEmbezzleInfoViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        #endregion

        #region Events

        //
        // GET: /DiscEmbezzleInfo/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            return View();
        }

        //
        // GET: /DiscEmbezzleInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public void MapDropDown(DiscEmbezzleInfoViewModel model)
        {
            var officeType = officeTypeService.GetAll().Where(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            model.OfficeList = ofc_items;

            var ZoneList = officeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;
        }

        //
        // GET: /DiscEmbezzleInfo/Create
        public ActionResult Create()
        {
            //var OfficeName = "";
            //var Embleze = discEmbezzleService.GetAll().Where(e => e.IsActive == true && e.CaseMasterId == id);

            //var OfficeId = discCaseMasterService.GetById(id).CrimeLocation;

            //var param = new { OfficeId = OfficeId };
            //var Office_Name = employeeSPService.GetDataWithParameter(param, "SP_GetOffice");
            //OfficeName = Office_Name.Tables[0].Rows[0][0].ToString();

            //if (Embleze.Count()>=1)
            //{
            //    var EmblezzId = 0;


            //    foreach (var r in Embleze)
            //    {
            //        EmblezzId = r.EmbezzleId;
            //        //OfficeId = string.IsNullOrEmpty(r.OfficeId)? 0: r.OfficeId;
            //    }



            //    var Embleze_Case = discEmbezzleService.GetById(EmblezzId);
            //    DiscEmbezzleInfoViewModel model = new DiscEmbezzleInfoViewModel();


            //    model.EmbMode = "U";//entity.AuditFromMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditFrom);
            //    model.AuditDateFromMsg = String.Format("{0:dd-MMM-yyyy}", Embleze_Case.AuditDateFrom);
            //    model.AuditDateToMsg =  String.Format("{0:dd-MMM-yyyy}",Embleze_Case.AuditDateTo);
            //    model.BranchAuditNo = Embleze_Case.BranchAuditNo;
            //    model.EmbezzleId = Embleze_Case.EmbezzleId;
            //    model.EmbezzleRcvDtMsg =  String.Format("{0:dd-MMM-yyyy}",Embleze_Case.EmbezzleRcvDt);
            //    model.ExplonatoryNo = discCaseMasterService.GetById(id).CaseNo;
            //    model.NoOfBMAccused = Embleze_Case.NoOfBMAccused;
            //    model.NoOfCMAccussed = Embleze_Case.NoOfCMAccussed;
            //    model.NoOfSignatoryAccussed = Embleze_Case.NoOfSignatoryAccussed;
            //    model.OfficeId = OfficeId;
            //    model.Remarks = Embleze_Case.Remarks;
            //    model.TotEmbezzledAmount = Embleze_Case.TotEmbezzledAmount;
            //    model.TotReturnAmount = Embleze_Case.TotReturnAmount;
            //    model.OfficeName = OfficeName;
            //    model.CaseMasterId = id;

            //      IEnumerable<SelectListItem> items = new SelectList(" ");
            //      ViewData["OfficeId"] = items;
            //      ViewData["HOList"] = items;
            //      ViewData["ZOOfficeList"] = items;
            //      ViewData["ZAOOfficeList"] = items;
            //      ViewData["AOOfficeList"] = items;
            //      ViewData["BOOfficeList"] = items;
            //      return View(model);
            ////  }
            //  else
            //  {
            DiscEmbezzleInfoViewModel model = new DiscEmbezzleInfoViewModel();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeId"] = items;
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            MapDropDown(model);
            return View(model);

        }

        //
        // POST: /DiscEmbezzleInfo/Create
        [HttpPost]
        public ActionResult Create(int CrimeLocation, DiscEmbezzleInfoViewModel model)
        {
            try
            {
                var entity = Mapper.Map<DiscEmbezzleInfoViewModel, DiscEmbezzleInfo>(model);

                entity.CaseMasterId = model.CaseMasterId;
                entity.IsActive = true;
                entity.CreateUser = SessionHelper.LoginUserEmployeeId;
                entity.OfficeId = CrimeLocation;
                entity.CreateDate = DateTime.Now;
                entity.AuditDateFrom = Convert.ToDateTime(model.AuditDateFromMsg);
                entity.AuditDateTo = Convert.ToDateTime(model.AuditDateToMsg);
                entity.EmbezzleRcvDt = Convert.ToDateTime(model.EmbezzleRcvDtMsg);

                var Embezzle = discEmbezzleService.Create(entity);
                var EmbezzleId = Embezzle.EmbezzleId;
                model.EmbezzleId = EmbezzleId;

                return Json(model, JsonRequestBehavior.AllowGet);//EmbezzleId
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            var Emb = discEmbezzleService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<DiscEmbezzleInfo, DiscEmbezzleInfoViewModel>(Emb);

            // var Embleze_Case = discEmbezzleService.GetById(EmblezzId);
            DiscEmbezzleInfoViewModel model = new DiscEmbezzleInfoViewModel();


            //  model.EmbMode = "U";//entity.AuditFromMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditFrom);
            model.AuditDateFromMsg = String.Format("{0:dd-MMM-yyyy}", Emb.AuditDateFrom);
            model.AuditDateToMsg = String.Format("{0:dd-MMM-yyyy}", Emb.AuditDateTo);
            model.BranchAuditNo = Emb.BranchAuditNo;
            model.EmbezzleId = Emb.EmbezzleId;
            model.EmbezzleRcvDtMsg = String.Format("{0:dd-MMM-yyyy}", Emb.EmbezzleRcvDt);
            //model.ExplonatoryNo = discCaseMasterService.GetById(id).CaseNo;
            model.NoOfBMAccused = Emb.NoOfBMAccused;
            model.NoOfCMAccussed = Emb.NoOfCMAccussed;
            model.NoOfSignatoryAccussed = Emb.NoOfSignatoryAccussed;
            model.OfficeId = Emb.OfficeId;
            model.Remarks = Emb.Remarks;
            model.TotEmbezzledAmount = Emb.TotEmbezzledAmount;
            model.TotReturnAmount = Emb.TotReturnAmount;
            //model.OfficeName = OfficeName;

            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["OfficeId"] = items;
            //ViewData["HOList"] = items;
            //ViewData["ZOOfficeList"] = items;
            //ViewData["ZAOOfficeList"] = items;
            //ViewData["AOOfficeList"] = items;
            //ViewData["BOOfficeList"] = items;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, int CrimeLocation, DiscEmbezzleInfoViewModel model)
        {
            try
            {
                var entity = Mapper.Map<DiscEmbezzleInfoViewModel, DiscEmbezzleInfo>(model);

                // entity.CaseMasterId = model.CaseMasterId;
                entity.OfficeId = CrimeLocation;
                entity.IsActive = true;
                entity.CreateUser = SessionHelper.LoginUserEmployeeId;
                entity.CreateDate = DateTime.Now;
                entity.AuditDateFrom = Convert.ToDateTime(model.AuditDateFromMsg);
                entity.AuditDateTo = Convert.ToDateTime(model.AuditDateToMsg);
                entity.EmbezzleRcvDt = Convert.ToDateTime(model.EmbezzleRcvDtMsg);

                discEmbezzleService.Update(entity);
                var EmbezzleId = model.EmbezzleId;
                return Json(EmbezzleId, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DiscEmbezzleInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DiscEmbezzleInfo/Delete/5
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
    }
    #endregion
}
