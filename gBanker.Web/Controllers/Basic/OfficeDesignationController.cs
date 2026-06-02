using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
    public class OfficeDesignationController : BaseController
    {
        #region Variables

        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeService employeeService;

        public OfficeDesignationController(IOfficeDesignationService officeDesignationService,
            IOfficeTypeService officeTypeService,
            IEmployeeSPService employeeSpService,
            IEmployeeService employeeService)
        {
            this.officeDesignationService = officeDesignationService;
            this.officeTypeService = officeTypeService;
            this.employeeSpService = employeeSpService;
            this.employeeService = employeeService;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var model = new OfficeDesignationViewModel();
            MapDropdownForOfficetype(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult SaveDesignation(OfficeDesignation obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var isDuplicateName =
                    officeDesignationService.GetMany(
                            p => p.IsActive == true && p.OffcType.ToUpper().Trim() == obj.OffcType.ToUpper().Trim() && p.OffcDesignName.ToUpper().Trim() == obj.OffcDesignName.ToUpper().Trim()).ToList();
                if (isDuplicateName.Any())
                {
                    result = 0;
                    message = "This Designation already exists, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                var isDuplicateOrder = officeDesignationService
                    .GetMany(p => p.IsActive == true && p.DesignationOrder == obj.DesignationOrder).ToList();

                if (isDuplicateOrder.Any())
                {
                    result = 0;
                    message = "Designation order already exists";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                var model = new OfficeDesignation();
                model.OffcType = obj.OffcType;
                model.OffcDesignName = obj.OffcDesignName.Trim();
                model.OffcDesignNameBn = obj.OffcDesignNameBn.Trim();
                model.DesignationOrder = obj.DesignationOrder;
                model.IsSectionDependent = obj.IsSectionDependent;
                model.IsActive = true;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                officeDesignationService.Create(model);
                result = 1;
                message = "Saved successfully";

                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DesignationList([DataSourceRequest] DataSourceRequest request)
        {
            var list = employeeSpService.GetDataWithoutParameter("basic.SP_GetOfficeTypeWiseDesignation");
            var officeTypeDesignationList = list.Tables[0].AsEnumerable().Select((row, sl) => new OfficeDesignationViewModel()
            {
                rowSl = sl + 1,
                OfficeDesignationId = row.Field<int>("OfficeDesignationId"),
                OffcDesignName = row.Field<string>("OffcDesignName"),
                OffcDesignNameBn = row.Field<string>("OffcDesignNameBn").Trim(),
                DesignationOrder = row.Field<int>("DesignationOrder"),
                OffcType = row.Field<string>("OffcType"),
                OfficeTypeName = row.Field<string>("OfficeTypeName"),
                IsSectionDependent = row.Field<bool?>("IsSectionDependent")
            }).ToList();
            DataSourceResult result = officeTypeDesignationList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDesignation(OfficeDesignation obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var isDuplicateName =
                    officeDesignationService.GetAll()
                        .Where(
                            p => p.IsActive == true && p.OffcType == obj.OffcType && p.OffcDesignName == obj.OffcDesignName && p.OfficeDesignationId != obj.OfficeDesignationId).ToList();


                var isDuplicateOrder =
                   officeDesignationService.GetAll()
                       .Where(
                           p => p.IsActive == true && p.DesignationOrder == obj.DesignationOrder && p.OfficeDesignationId != obj.OfficeDesignationId).ToList();

                if (isDuplicateName.Any())
                {
                    result = 0;
                    message = "This Designation name already exists, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (isDuplicateOrder.Any())
                {
                    result = 0;
                    message = "Designation Order already exists, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var model = officeDesignationService.GetById(obj.OfficeDesignationId);
                    model.OffcDesignName = obj.OffcDesignName;
                    model.OffcDesignNameBn = obj.OffcDesignNameBn;
                    model.DesignationOrder = obj.DesignationOrder;
                    model.IsSectionDependent = obj.IsSectionDependent;
                    model.IsActive = true;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    officeDesignationService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteDesignation(int OfficeDesignationId)
        {
            var result = 0;
            var message = "";
            try
            {
                var ifOfficeDesignationExists = employeeService.GetAll().Where(p => p.IsActive == true && Convert.ToInt32(p.EmployeeRank) == OfficeDesignationId).ToList();
                if (ifOfficeDesignationExists.Any())
                {
                    result = 0;
                    message = "This Designation depends on Employee Rank, Delete denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = officeDesignationService.GetMany(p => p.OfficeDesignationId == OfficeDesignationId && p.IsActive == true).FirstOrDefault();
                    //officeDesignationService.GetById(OfficeDesignationId);
                    if (model != null)
                    {
                        model.IsActive = false;
                        model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        model.UpdateDate = DateTime.UtcNow;
                        officeDesignationService.Update(model);
                        result = 1;
                        message = "Deleted successfully";
                        result = 1;
                        message = "Deleted successfully";
                    }

                }

            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods
        private void MapDropdownForOfficetype(OfficeDesignationViewModel model)
        {
            var officeTypeList = new List<SelectListItem>();
            officeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

            var offType = officeTypeService.GetMany(p => p.IsActive == true).FirstOrDefault(); //Head Office 
            //var officeType = officeTypeService.GetMany(p => p.IsActive == true).ToList();

            if (offType != null)
            {
                var viewOfficeType = new SelectListItem()
                {
                    Text = offType.OfficeTypeName,
                    Value = offType.OfficeTypeId.ToString(),
                    Selected = true
                };

                //var viewOfficeType = officeType.AsEnumerable().Select(p => new SelectListItem()
                //{
                //    Text = p.OfficeTypeName,
                //    Value = p.OfficeTypeId.ToString()
                //}).ToList();


                officeTypeList.Add(viewOfficeType);
                //officeTypeList.AddRange(viewOfficeType);
            }

            model.OfficeTypeList = officeTypeList;

            var rankList = new List<SelectListItem>();
            rankList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            for (var i = 1; i <= 200; i++)
            {
                rankList.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            model.RankList = rankList;

            var isDependentList = new List<SelectListItem>();
            isDependentList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            isDependentList.Add(new SelectListItem() { Text = "True", Value = "true" });
            isDependentList.Add(new SelectListItem() { Text = "False", Value = "false" });
            model.SectionDependentList = isDependentList;
        }

        #endregion

    }
}