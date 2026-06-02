using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using AutoMapper;
using Elmah;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.DropDownService;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
    public class InternalOrganizationController : BaseController
    {
        #region Varibles
        private readonly IInternalOrganizationService internalOrganizationService;
       
        public InternalOrganizationController(
            IInternalOrganizationService internalOrganizationService
        )
        {
            this.internalOrganizationService = internalOrganizationService;
        }

        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Methods
        public JsonResult SaveInternalOrganization(InternalOrganization InternalOrganization)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    internalOrganizationService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.OrganizationName.ToUpper().Trim() == InternalOrganization.OrganizationName.ToUpper().Trim())
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Organization Name found, Save denied";
                }
                else
                {
                    var entity = new InternalOrganization();
                    entity.OrganizationName = InternalOrganization.OrganizationName;
                    entity.OrganizationCode = InternalOrganization.OrganizationCode;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    internalOrganizationService.Create(entity);
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateInternalOrganization(InternalOrganization InternalOrganization)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   internalOrganizationService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.OrgId != InternalOrganization.OrgId &&
                               p.OrganizationName.ToUpper().Trim() == InternalOrganization.OrganizationName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Organization Name found, Update denied";
                }
                else
                {
                    var entity = internalOrganizationService.GetById(InternalOrganization.OrgId);
                    entity.OrgId = InternalOrganization.OrgId;
                    entity.OrganizationName = InternalOrganization.OrganizationName;
                    entity.OrganizationCode = InternalOrganization.OrganizationCode;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    internalOrganizationService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult ListInternalOrganization(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        //{
        //    var InternalOrganization = internalOrganizationService.GetAll().Where(t => t.IsActive == true);
        //    var listInternalOrganization = InternalOrganization.AsEnumerable().Select(a => new InternalOrganization()
        //    {
        //        OrgId = a.OrgId,
        //        OrganizationName = a.OrganizationName,
        //        OrganizationCode = a.OrganizationCode
        //    }).ToList();
        //    var currentPageRecords = listInternalOrganization.Skip(jtStartIndex).Take(jtPageSize);
        //    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = listInternalOrganization.LongCount(), JsonRequestBehavior.AllowGet });
        //}

        public JsonResult ListInternalOrganization([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request,string OrganizationCode)
        {
            try
            {
                //var approvalList = employeeSPService.GetDataWithoutParameter("leave.SP_GetLeaveApprovalConfigurList");
                //var approvalShowViewModel = approvalList.Tables[0].AsEnumerable()
                //    .Select(row => new ApprovalConfigurationViewModel
                //    {
                //        ConfigMasterId = row.Field<int>("ConfigMasterId"),
                //        ConfigDesignationId = row.Field<int>("ConfigDesignationId"),
                //        DesignationName = row.Field<string>("DesignationName"),
                //        TotalLevel = row.Field<int>("TotalLevel")

                //    }).ToList();

                var InternalOrganization = internalOrganizationService.GetAll().Where(t => t.IsActive == true);
                var listInternalOrganization = InternalOrganization.AsEnumerable().Select(a => new InternalOrganization()
                {
                    OrgId = a.OrgId,
                    OrganizationName = a.OrganizationName,
                    OrganizationCode = a.OrganizationCode
                }).ToList();
                DataSourceResult result = listInternalOrganization.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult InformationDeleteInternalOrganization(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = internalOrganizationService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                internalOrganizationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        #endregion

    }
}