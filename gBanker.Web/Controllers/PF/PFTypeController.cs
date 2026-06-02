
#region Usings


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.ViewModels.PF;
using gHRM.Data.CodeFirstMigration;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFTypeController : BaseController
    {
        #region Variables
        private readonly IPFTypeService pFTypeService;

        #endregion

        #region Ctor

        public PFTypeController(IPFTypeService pFTypeService)
        {
            this.pFTypeService = pFTypeService;
        }

        #endregion

        #region Listings

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPfTypeList(string PFTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                List<PFType> List_PFType = new List<PFType>();
                var pfTypeList = pFTypeService.GetAll().Where(x => x.IsDeleted == false);

                var List_ViewModel = pfTypeList.AsEnumerable()
               .Select(row => new PFTypeViewModel
               {
                   PFTypeId = row.PFTypeId.ToString(),
                   ShortName = row.ShortName,
                   FullName = row.FullName,
                   HasSelfContribution = row.HasSelfContribution,
                   SelfContributionRate = Math.Round(row.SelfContributionRate, 2).ToString(),
                   HasAddSelfContribution = row.HasAddSelfContribution,
                   HasOrgContribution = row.HasOrgContribution,
                   OrgContributionRate = Math.Round(row.OrgContributionRate, 2).ToString()

               }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            var model = new PFTypeViewModel();
            MapDropDownList(model);
            return View(model);
        }

        public JsonResult SavePFType(string pfTypeId, string shortName, string fullName, bool hasSelfContribution, bool hasOrgContribution, bool hasAddSelfContribution, decimal? selfContributionRate, decimal? orgContributionRate)
        {
            string msg = "Unable to Save";
            PFType objPFType = new PFType();

            try
            {
                int pfId = Convert.ToInt32(pfTypeId);
                var obj = pFTypeService.GetById(pfId);

                if (obj != null)
                    return Json(new { message = "Already exist" }, JsonRequestBehavior.AllowGet);

                if (hasSelfContribution && selfContributionRate <= 0)
                    return Json(new { message = "Self Contribution Rate is Required" }, JsonRequestBehavior.AllowGet);

                if (hasOrgContribution && orgContributionRate <= 0)
                    return Json(new { message = "Organization Contribution Rate is Required" }, JsonRequestBehavior.AllowGet);

                objPFType.PFTypeId = pfId;
                objPFType.ShortName = shortName;
                objPFType.FullName = fullName;

                objPFType.HasSelfContribution = hasSelfContribution;
                objPFType.HasOrgContribution = hasOrgContribution;
                objPFType.HasAddSelfContribution = hasAddSelfContribution;

                objPFType.SelfContributionRate = hasSelfContribution ? (decimal)selfContributionRate : 0;
                objPFType.OrgContributionRate = hasOrgContribution ? (decimal)orgContributionRate : 0;

                objPFType.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objPFType.CreateDate = DateTime.Now;

                //let's insert into [gcpf.PFType]
                var pfType = pFTypeService.Create(objPFType);

                obj = null;
                obj = pFTypeService.GetById(pfId);

                if (obj != null)
                    msg = "Added Successfully";
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! Please try again later" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = msg }, JsonRequestBehavior.AllowGet);
        }

        #region Collection Configuration
        public ActionResult CollectionConfiguration()
        {
            CollectionTypeConfigurationViewModel viewmodel = new CollectionTypeConfigurationViewModel();
            List<SelectListItem> lst = new List<SelectListItem>() {
                new SelectListItem() {Text="Principal",Value="Principal" },
                //new SelectListItem() {Text="Interest",Value="Interest" },
                new SelectListItem() {Text="Both(Principal & Interest)",Value="Partial" }
            };
            viewmodel.CollectionTypeLst = lst;


            return View(viewmodel);
        }

        public JsonResult SaveCollectionConfiguration(CollectionTypeConfiguration obj)
        {
            var lst = new gHRMDBContext().CollectionTypeConfigurations.Where(x => x.IsActive == true).ToList();
            if (lst.Any())
                new gHRMDBContext().Database.ExecuteSqlCommand("UPDATE gcpf.CollectionTypeConfiguration SET IsActive=0");
            obj.CreateBy = LoggedInEmployeeId.ToString();
            obj.CreateDate = DateTime.Now;
            using (gHRMDBContext db = new gHRMDBContext())
            {
                db.CollectionTypeConfigurations.Add(obj);
                db.SaveChanges();
            }
            return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllCollectionConfig()
        {
            var lst = new gHRMDBContext().CollectionTypeConfigurations.Where(x=>x.IsActive==true).ToList();
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        #endregion  Collection Configuration
        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            var model = new PFTypeViewModel();

            var pfType = pFTypeService.GetById(id);
            model.PFTypeId = pfType.PFTypeId.ToString();
            model.FullName = pfType.FullName;
            model.HasSelfContribution = pfType.HasSelfContribution;
            model.HasOrgContribution = pfType.HasOrgContribution;
            model.HasAddSelfContribution = pfType.HasAddSelfContribution;
            model.SelfContributionRate = Math.Round(pfType.SelfContributionRate, 2).ToString();
            model.OrgContributionRate = Math.Round(pfType.OrgContributionRate, 2).ToString();
            MapDropDownList(model);

            return View(model);
        }

        public JsonResult UpdatePFType(string pfTypeId, string fullName, bool hasSelfContribution, bool hasOrgContribution, bool hasAddSelfContribution, decimal? selfContributionRate, decimal? orgContributionRate)
        {
            try
            {
                int pfId = Convert.ToInt32(pfTypeId);
                PFType objPFType = pFTypeService.GetById(pfId);

                if (string.IsNullOrEmpty(objPFType.PFTypeId.ToString()))
                    return Json(new { message = "Record does not exist" }, JsonRequestBehavior.AllowGet);

                if (hasSelfContribution && selfContributionRate <= 0)
                    return Json(new { message = "Self Contribution Rate is Required" }, JsonRequestBehavior.AllowGet);

                if (hasOrgContribution && orgContributionRate <= 0)
                    return Json(new { message = "Organization Contribution Rate is Required" }, JsonRequestBehavior.AllowGet);

                objPFType.FullName = fullName;
                //new fields
                objPFType.HasSelfContribution = hasSelfContribution;
                objPFType.HasOrgContribution = hasOrgContribution;
                objPFType.HasAddSelfContribution = hasAddSelfContribution;

                objPFType.SelfContributionRate = hasSelfContribution ? (decimal)selfContributionRate : 0;
                objPFType.OrgContributionRate = hasOrgContribution ? (decimal)orgContributionRate : 0;

                objPFType.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objPFType.UpdateDate = DateTime.Now;

                //let's update on [gcpf.PFType]
                pFTypeService.Update(objPFType);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Others Methods

        public JsonResult GetPfType(string PFTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(PFTypeId))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                List<PFType> List_PFType = new List<PFType>();
                List<PFType> pfTypeList = pFTypeService.GetAll().Where(x => x.PFTypeId == Convert.ToInt32(PFTypeId) && x.IsDeleted == false).ToList();

                var List_ViewModel = pfTypeList.AsEnumerable()
               .Select(row => new PFTypeViewModel
               {
                   PFTypeId = row.PFTypeId.ToString(),
                   ShortName = row.ShortName,
                   FullName = row.FullName,
                   HasSelfContribution = row.HasSelfContribution,
                   SelfContributionRate = Math.Round(row.SelfContributionRate, 2).ToString(),
                   HasAddSelfContribution = row.HasAddSelfContribution,
                   HasOrgContribution = row.HasOrgContribution,
                   OrgContributionRate = Math.Round(row.OrgContributionRate, 2).ToString()

               }).ToList();

                //if (PFTypeId != null)
                //{
                //    return Json(pfTypeList.ToList(), JsonRequestBehavior.AllowGet);
                //}
                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        #endregion

        #region Private Methods

        private void MapDropDownList(PFTypeViewModel model)
        {
            var pfType = new List<SelectListItem>();
            pfType.Add(new SelectListItem() { Text = "Select One", Value = "", Selected = true });
            pfType.Add(new SelectListItem() { Text = "CPF[Contributional Provident Fund]", Value = "1" });
            pfType.Add(new SelectListItem() { Text = "GPF[General Provident Fund]", Value = "2" });
            model.PFTypeList = pfType;
        }

        #endregion
    }
}
