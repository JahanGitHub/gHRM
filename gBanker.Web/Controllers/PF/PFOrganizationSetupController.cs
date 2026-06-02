
#region Usings

using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.Payroll;
using gHRM.Service.PF;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFOrganizationSetupController : BaseController
    {
        #region Private Variables
        private readonly IOrganizationSetupService orgSetupService;
        private readonly IPFTypeService pFTypeService;
        private readonly IComponentPayrollService componentPayrollService;
        #endregion

        #region Ctor

        public PFOrganizationSetupController
            (IOrganizationSetupService orgSetupService, IPFTypeService pFTypeService, IComponentPayrollService componentPayrollService)
        {
            this.orgSetupService = orgSetupService;
            this.pFTypeService = pFTypeService;
            this.componentPayrollService = componentPayrollService;
        }

        #endregion

        #region PFOrganizationSetup Listing

        public ActionResult Index()
        {
            var model = new OrganizationSetupViewModel();
            MapDropDownList(model);

            return View(model);
        }
        [HttpPost]
        public JsonResult PFConfigurationSave(OrganizationPFSetup obj)
        {
            string msg = "";
            if (obj == null)
                msg = "Data not found";
            else
            {
                gHRMDBContext db = new gHRMDBContext();
                if (obj.Id > 0)
                {
                    var model = db.OrganizationPFSetups.FirstOrDefault(x => x.Id == obj.Id);
                    model.SelfContribution_ComponentPayrollId = obj.SelfContribution_ComponentPayrollId;
                    model.OfficeContribution_ComponentPayrollId = obj.OfficeContribution_ComponentPayrollId;
                    msg = "Update Successfully";
                }
                else if (obj.Id == 0)
                {
                    obj.IsActive = true;
                    db.OrganizationPFSetups.Add(obj);
                    msg = "Saved Successfully";
                }
                db.SaveChanges();
            }
            
            return Json(msg);
        }
        public JsonResult GetOrganizationSetupList( int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();

                var lst = (from pf in db.OrganizationPFSetups
                           join self in db.ComponentPayroll on pf.SelfContribution_ComponentPayrollId equals self.Id into ps
                           from self in ps.DefaultIfEmpty()
                           join ofc in db.ComponentPayroll on pf.OfficeContribution_ComponentPayrollId equals ofc.Id into po
                           from ofc in po.DefaultIfEmpty()
                           where pf.IsActive
                           select new
                           {
                               Id=pf.Id,
                               Self_ComponentName = self.ComponentName,
                               Office_ComponentName = ofc.ComponentName,
                           }
                ).ToList();
                var currentPageRecords = lst.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = lst.Count(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        #endregion

        #region PFOrganizationSetup Create

        //
        // GET: /PFOrganizationSetup/Create
        public ActionResult Create()
        {
            var model = new OrganizationSetupViewModel();
            var pfType = pFTypeService.GetAll().Where(x => x.IsDeleted == false);
            MapDropDownList(model);
            return View(model);
        }

        public JsonResult SaveOrganizationSetup(string pfTypeId, string yearStartDate, string yearEndDate, bool isActive)
        {
            //string result = "OK";
            OrganizationSetup objOrganizationSetup = new OrganizationSetup();
            try
            {
                var newYearStartDate = Convert.ToDateTime(yearStartDate);
                var newYearEndDate = Convert.ToDateTime(yearEndDate);

                var isValid = orgSetupService.ValidateEmployeeRoasterByDateRange(0, newYearStartDate, newYearEndDate);
                if (!isValid)
                    return Json(new { message = "PF Organization already exist within Year start and end date!" }, JsonRequestBehavior.AllowGet);

                int pfId = Convert.ToInt32(pfTypeId);

                string message = IsValidPFType(pfId);

                if (!string.IsNullOrEmpty(message))
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                objOrganizationSetup.OrgId = (int)SessionHelper.CompanyID;
                objOrganizationSetup.OrgName = SessionHelper.CompanyName;
                objOrganizationSetup.PFTypeId = pfId;
                objOrganizationSetup.YearStartDate = Convert.ToDateTime(yearStartDate);
                objOrganizationSetup.YearEndDate = Convert.ToDateTime(yearEndDate);
                objOrganizationSetup.IsActive = isActive;

                objOrganizationSetup.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objOrganizationSetup.CreateDate = DateTime.Now;

                //let's insert into [gcpf.OrganizationSetup]
                orgSetupService.Create(objOrganizationSetup);

            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience, please try again later." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Added successfully." }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PFOrganizationSetup Edit

        //
        // GET: /PFOrganizationSetup/Edit/5
        public ActionResult Edit(int id)
        {
            OrganizationSetupViewModel model = new OrganizationSetupViewModel();
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                var objOrganizationSetup = orgSetupService.GetOrganizationSetupById(id);

                if (objOrganizationSetup == null || objOrganizationSetup.Id <= 0 || objOrganizationSetup.IsDeleted)
                    return Json(new { Result = "ERROR", Message = "Organization wise PF setup not found!" });

                //model.Id = objOrganizationSetup.Id.ToString();               
                //model.PFTypeId = objOrganizationSetup.PFTypeId;
                //model.PFTypeName = objOrganizationSetup.PFType.FullName;
                //model.IsActive = objOrganizationSetup.IsActive;
                //model.YearStartDate = objOrganizationSetup.YearStartDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
                //model.YearEndDate = objOrganizationSetup.YearEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                //MapDropDownList(model);
                //Checked in
            }
            catch (Exception ex)
            {
                var pFTypeItems = new List<SelectListItem>();
                pFTypeItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            }
            return View(model);
        }

        public JsonResult UpdateOrganizationSetup(string id, string pfTypeId, string yearStartDate, string yearEndDate, bool isActive)
        {

            try
            {
                int pfId = Convert.ToInt32(pfTypeId);
                string message = IsValidPFType(pfId);
                if (!string.IsNullOrEmpty(message))
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                //int oId = Convert.ToInt32(id);
                OrganizationSetup objOrganizationSetup = orgSetupService.GetById(Convert.ToInt32(id));

                if (objOrganizationSetup == null)
                    return Json(new { message = "Does not exist" }, JsonRequestBehavior.AllowGet);

                var newYearStartDate = Convert.ToDateTime(yearStartDate);
                var newYearEndDate = Convert.ToDateTime(yearEndDate);

                var isValid = orgSetupService.ValidateEmployeeRoasterByDateRange(pfId, newYearStartDate, newYearEndDate);
                if (!isValid)
                    return Json(new { message = "PF Organization already exist within Year start and end date!" }, JsonRequestBehavior.AllowGet);

                objOrganizationSetup.PFTypeId = pfId;
                objOrganizationSetup.YearStartDate = newYearStartDate;
                objOrganizationSetup.YearEndDate = newYearEndDate;
                objOrganizationSetup.IsActive = isActive;

                objOrganizationSetup.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                objOrganizationSetup.UpdateDate = DateTime.Now;

                //let's update into [gcpf.OrganizationSetup]
                orgSetupService.Update(objOrganizationSetup);
            }
            catch
            {
                return Json(new { message = "Sorry for inconvenience!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

        }


        #endregion

        public JsonResult GetOrganizationSetup(string OrgId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            List<OrganizationSetup> objOrganizationSetupList = new List<OrganizationSetup>();
            try
            {
                if (string.IsNullOrEmpty(OrgId))
                    return Json(new { Result = "ERROR", Message = "Internal Error" });

                objOrganizationSetupList = orgSetupService.GetAll().Where(x => x.OrgId == Convert.ToInt32(OrgId) && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return Json(objOrganizationSetupList, JsonRequestBehavior.AllowGet);

        }// End Function

        //
        // GET: /PFOrganizationSetup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // POST: /PFOrganizationSetup/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /PFOrganizationSetup/Edit/5
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
        // GET: /PFOrganizationSetup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PFOrganizationSetup/Delete/5
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

        #region Private Methods

        private void MapDropDownList(OrganizationSetupViewModel model)
        {
            var comlst = componentPayrollService.GetMany(x => x.ComponentCategory == "Salary" && x.ComponentName.ToLower().Contains("pf ") && x.ComponentName.ToLower().Contains("deduction")).ToList();
            var Lst = new List<SelectListItem>();
            Lst.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            if (comlst.Any())
                Lst.AddRange(comlst.Select(s => new SelectListItem
                {
                    Text = s.ComponentName,
                    Value = s.Id.ToString()
                }));
            model.ComponentLst = Lst;
        }

        private string IsValidPFType(int pfTypeId)
        {
            string message = string.Empty;
            var pfType = pFTypeService.GetById(pfTypeId);

            if (pfType == null)
                message = "Setup PF definition";


            if (!pfType.HasSelfContribution)
                message = "Allow Self Contribution";

            if (pfType.SelfContributionRate <= 0)
                message = "Set Value for Self Contribution Rate";

            if (pfTypeId == Convert.ToInt32(PFTypeConstants.CPF)) //CPF
            {
                if (!pfType.HasOrgContribution)
                    message = "Allow Org Contribution";

                if (pfType.OrgContributionRate <= 0)
                    message = "Set Value for Org Contribution Rate";
            }

            return message;
        }

        #endregion
    }
}
