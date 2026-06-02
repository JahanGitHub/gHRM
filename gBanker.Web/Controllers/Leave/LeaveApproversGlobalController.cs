using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Models;
using gHRM.Web.ViewModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Transactions;
using Kendo.Mvc.UI;
using DataSourceRequest = Kendo.DynamicLinq.DataSourceRequest;
using Kendo.Mvc.Extensions;
using gHRM.Web.DropDownService;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;

namespace gHRM.Web.Controllers
{
    public class LeaveApproversGlobalController : BaseController
    {
        #region Variable
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IApprovalConfigMasterService approvalConfigMasterService;
        private readonly ILeaveApproversMetadataService leaveApproversMetadataService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public LeaveApproversGlobalController(
              IEmployeeSPService employeeSPService
            , IOfficeService officeService
            , IEmployeeDepartmentService employeeDepartmentService
            , IApprovalConfigMasterService approvalConfigMasterService
            , ILeaveApproversMetadataService leaveApproversMetadataService)
        {
            this.employeeSPService = employeeSPService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.approvalConfigMasterService = approvalConfigMasterService;
            this.leaveApproversMetadataService = leaveApproversMetadataService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Configure Leave Approval

        public ActionResult ConfigureLeaveApproval()
        {
            var model = new ApprovalConfigurationViewModel();
            MapDropdownForLeaveApprovalConfiguration(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveConfigureLeaveApproval(int ConfigDesignationId, List<ApprovalConfigDetailViewModel> ApproverList)
        {
            var result = 0;
            var message = "";
            if (ConfigDesignationId <= 0 || !ApproverList.Any())
            {
                result = 0;
                message = "Please setup all required field";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var configExist = approvalConfigMasterService.GetMany(x => x.IsActive == 1 && x.ConfigDesignation == ConfigDesignationId).Any();

                if (configExist)
                {
                    result = 0;
                    message = "Approvers already configured";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
                {
                    try
                    {
                        var tempLeaveApproversMetadataList = new List<LeaveApproversMetadata>();

                        foreach (var item in ApproverList)
                        {
                            var tempLeaveApproversMetadata = new LeaveApproversMetadata();
                            tempLeaveApproversMetadata.ApproveOfficeId = item.IsApproverInSelfOffice == true ? 0 : item.ApproveOfficeId;
                            tempLeaveApproversMetadata.ApproveDepartmentId = item.ApproveDepartmentId > 0 ? item.ApproveDepartmentId : 0;
                            tempLeaveApproversMetadata.ApprovalLevel = item.ApprovalLevel;
                            tempLeaveApproversMetadata.ApproveDesignationId = item.ApproveDesignationId;
                            tempLeaveApproversMetadata.IsApproverInSelfOffice = item.IsApproverInSelfOffice;
                            tempLeaveApproversMetadata.ConfigDesignation = ConfigDesignationId;
                            tempLeaveApproversMetadata.IsActive = true;
                            tempLeaveApproversMetadata.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            tempLeaveApproversMetadata.CreateDate = DateTime.UtcNow;

                            if(!string.IsNullOrEmpty(item.FromDay))                      
                                tempLeaveApproversMetadata.FromDay = Convert.ToInt32(item.FromDay);       
                          


                            if (!string.IsNullOrEmpty(item.ToDay))                      
                                tempLeaveApproversMetadata.ToDay = Convert.ToInt32(item.ToDay);
                           

                            tempLeaveApproversMetadataList.Add(tempLeaveApproversMetadata);
                        }

                        //let's insert into [leave.LeaveApproversMetadata]
                        leaveApproversMetadataService.AddLeaveApproversMetadataList(tempLeaveApproversMetadataList);

                        var param = new { desinationID = ConfigDesignationId };

                        //let's insert into [leave.ApprovalConfigMaster]  Note: OfficeXDept for this "ConfigDesignationId"
                        var approversMaster = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveApproversMaster");

                        //let's insert into [leave.ApprovalConfigDetail]
                        var approversDetail = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveApproversDetail");

                        //let's insert into [leave.LeaveApprovers]
                        var leaveApprovers = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveApprovers"); // Need to update

                        scope.Complete();
                        scope.Dispose();

                        result = 1;
                        message = "Approvers configured successfully";
                    }
                    catch (Exception ex)
                    {
                        result = 0;
                        message = "Approvers configuration failed";
                        scope.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Approvers configuration failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Event

        public ActionResult LeaveApprovalIndex()
        {
            var model = new ApprovalConfigurationViewModel();
            return View(model);
        }
        
        public ActionResult ConfigureLeaveApprovalEdit(int ConfigMasterId, int ConfigDesignationId)
        {
            var model = new ApprovalConfigurationViewModel();
            MapDropdownForLeaveApprovalConfiguration(model);
            model.ConfigMasterId = ConfigMasterId;
            model.ConfigDesignationId = ConfigDesignationId;
            return View(model);
        }

        #endregion
        
        #region HttpRequests

        public JsonResult GetLeaveApprovalConfigurList([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request)
        {
            try
            {
                //get listing from [leave.ApprovalConfigMaster]
                var approvalList = employeeSPService.GetDataWithoutParameter("leave.SP_GetLeaveApprovalConfigurList");
                var approvalShowViewModel = approvalList.Tables[0].AsEnumerable()
                    .Select(row => new ApprovalConfigurationViewModel
                    {
                        ConfigMasterId = row.Field<int>("ConfigMasterId"),
                        ConfigDesignationId = row.Field<int>("ConfigDesignationId"),
                        DesignationName = row.Field<string>("DesignationName"),
                        TotalLevel = row.Field<int>("TotalLevel")

                    }).ToList();

                DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult LoadLeaveApprovalConfigurationDetail(int ConfigMasterId)
        {
            List<ApprovalConfigurationViewModel> ApprovalShowViewModel = new List<ApprovalConfigurationViewModel>();
            var param = new { ConfigMasterId = ConfigMasterId };
            //var param = new { ConfigDesignationId = ConfigDesignationId };
            var _approvalList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveApprovalConfigDetail");

            ApprovalShowViewModel = _approvalList.Tables[0].AsEnumerable()
                .Select(row => new ApprovalConfigurationViewModel
                {
                    rowSl = row.Field<string>("rowSl"),
                    ConfigMasterId = row.Field<int>("ConfigMasterId"),
                    ConfigDesignationId = row.Field<int>("ConfigDesignationId"),
                    ApproveDesignationId = row.Field<int>("ApproveDesignationId"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    ApprovalLevel = row.Field<int>("ApprovalLevel"),
                    ApprovalLevelInString = row.Field<int>("ApprovalLevel").ToString(),
                    FromDay = row.Field<string>("FromDay"),
                    ToDay = row.Field<string>("ToDay")
                }).ToList();

            return Json(ApprovalShowViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LeaveApprovalConfigurationDelete(int ConfigDesignationId)
        {
            var result = 0;
            var message = "";

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (ConfigDesignationId > 0)
                    {
                        var param = new { ConfigDesignationId = ConfigDesignationId };
                        employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveApproversGlobalDelete");

                        result = 1;
                        message = "Approval level deleted successfully";
                    }
                    else
                    {
                        result = 0;
                        message = "No Configuration Found to Delete";
                    }
                    scope.Complete();

                }
                catch (Exception e)
                {
                    result = 0;
                    message = "Approval level deletion failed";
                    scope.Dispose();
                }
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfficeTypeWiseDepartment(int officeTypeId)
        {
            var view_List = new List<SelectListItem>();
            var result = 0;

            try
            {

                if (officeTypeId > 0)
                {
                    var deptList = employeeDepartmentService.GetMany(x => x.IsActive == true).ToList();//&& x.OfficeTypeId == officeTypeId

                    var ViewDeptList = deptList.AsEnumerable().Select(row => new SelectListItem()
                    {
                        Value = row.DepartmentId.ToString(),
                        Text = row.DepartmentName
                    }).ToList();

                    // var view_List = new List<SelectListItem>();
                    view_List.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                    view_List.AddRange(ViewDeptList);

                }
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;

            }
            return Json(new { result = result, data = view_List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfficeTypeWiseOffice(int officeTypeId)
        {
            var view_List = new List<SelectListItem>();
            var result = 0;
            try
            {
                var list = officeService.GetOfficeByType(officeTypeId);
                var ViewOfficeList = list.AsEnumerable().Select(row => new SelectListItem()
                {
                    Value = row.OfficeId.ToString(),
                    Text = row.OfficeName
                }).ToList();

                view_List.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                view_List.AddRange(ViewOfficeList);

                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(new { result = result, data = view_List }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Method

        private void MapDropdownForLeaveApprovalConfiguration(ApprovalConfigurationViewModel model)
        {
            model.ApprovalOfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ApproveOfficeList = commonStaticDropDown.ddlInitial();
            model.ApproveDepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.ApplicantDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            model.ApproveDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            model.ApprovalLevelList = commonStaticDropDown.Get1To10NumberList();
        }

        #endregion

    }
}