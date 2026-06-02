using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using System.Transactions;
using gHRM.Service.StoreProcedure;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Web.CommonDropdown;
using System.Text;
using Microsoft.Ajax.Utilities;
using gHRM.Web.Helpers.Transfer;

namespace gHRM.Web.Controllers
{
    public class EmployeeTransferNewController : BaseController
    {
        #region variables
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeOfficeDesignationService employeeOfficeDesignationService;
        private readonly IEmployeeTransferService employeeTransferService;
        private readonly IOfficeDesignationService officeDesignationService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;

        public EmployeeTransferNewController(
              IEmployeeSPService employeeSPService
            , IEmployeeService employeeService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IEmployeeTransferService employeeTransferService
            , IEmployeeDepartmentService employeeDepartmentService
            , IEmployeeOfficeDesignationService employeeOfficeDesignationService
            , IOfficeDesignationService officeDesignationService
            , IEmployeeDepartmentSectionService employeeDepartmentSectionService
            )
        {
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeeTransferService = employeeTransferService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeOfficeDesignationService = employeeOfficeDesignationService;
            this.officeDesignationService = officeDesignationService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeDepartmentSectionService = employeeDepartmentSectionService;
        }

        #endregion

        #region Events 

        public ActionResult TransferBacklogIndex()
        {
            return View();
        }

        public ActionResult TransferBacklogEntry(string EmployeeCode)
        {
            var model = new EmployeeTransferViewModel();
            MapOfficeNevigationDropDown(model);
            return View(model);
        }


        public ActionResult TransferPlanningIndex()
        {
            return View();
        }

        public ActionResult TransferPlanningApproveIndex()
        {
            return View();
        }

        public ActionResult TransferPlanningEntry(int? Id, string EmployeeCode, string EntryType)
        {
            var model = new EmployeeTransferViewModel();
            MapOfficeNevigationDropDown(model);
            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            var notifyList = new List<SelectListItem>();
            notifyList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.NotificationList1 = notifyList;
            model.NotificationList2 = notifyList;


            if (EmployeeCode != "" && EmployeeCode != null && Id.HasValue)
            {
                var employeeInfo = SP_GetEmployeeInfo_ByEmployeeCode(EmployeeCode);
                model.Id = Convert.ToInt32(Id);
                model.EntryType = EntryType;

                model.EmployeeCode = EmployeeCode;
                model.EmployeeId = employeeInfo.EmployeeId;
                model.EmployeeName = employeeInfo.EmployeeName;
                model.CurrentOfficeType = employeeInfo.CurrentOfficeType;
                model.EmployeeCurrentOfficeId = employeeInfo.EmployeeCurrentOfficeId;
                model.EmployeeCurrentOfficeName = employeeInfo.EmployeeCurrentOfficeName;
                model.EmployeeCurrentDepartmentName = employeeInfo.EmployeeCurrentDepartmentName;
                model.EmployeeCurrentDesignation = employeeInfo.EmployeeCurrentDesignation;

                var planInfo = employeeTransferService.Get(x => x.IsActive == true && x.Id == Id);
                model.OfficeId = planInfo.OfficeId;
                model.ChangingStatus = planInfo.ChangingStatus;
                GetOfficeInfo(model);

                var edlist = employeeDepartmentSectionService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.SectionId);
                var viewedlist = edlist.Select(a => new SelectListItem()
                {
                    Value = a.SectionId.ToString(),
                    Text = a.SectionName
                });
                var listSectionList = new List<SelectListItem>();
                listSectionList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                listSectionList.AddRange(viewedlist);
                model.SectionList = sectionList;

                model.DepartmentId = planInfo.DepartmentId;
                model.OfficeDesignationId = planInfo.OfficeDesignationId;
                model.OrderNo = planInfo.OrderNo;
                model.SectionId = planInfo.SectionId;

                model.OrderDate = planInfo.OrderDate;
                model.PlannedReleaseDate = planInfo.PlannedReleaseDate;
                model.PlannedJoiningDate = planInfo.PlannedJoiningDate;
                model.IsMutual = planInfo.IsMutual;
                model.IsTADAApplicable = planInfo.IsTADAApplicable;
            }

            return View(model);
        }

        #endregion

        #region HttpRequests Common

        public ActionResult loadTransferBacklogList([DataSourceRequest]DataSourceRequest request)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("AND vt.IsApproved=1");

            var transferInfoList = SP_GetTransferInformation(sb.ToString());

            DataSourceResult result = transferInfoList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadTransferPlanList([DataSourceRequest]DataSourceRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AND vt.IsPlanned=1 AND IsApproved=0");

            var transferInfoList = SP_GetTransferInformation(sb.ToString());
            DataSourceResult result = transferInfoList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTransferInformation(int Id)
        {
            int result = 0;
            string message = "";
            try
            {
                var entity = employeeTransferService.GetById(Id);
                entity.IsActive = false;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeTransferService.Update(entity);
                result = 1;
                message = "Transfer Information deleted succesfully";
            }
            catch (Exception e)
            {
                result = 0;
                message = "Failed to delete transfer Information";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            var result = 0;
            try
            {
                //var employeeInfo = SP_GetEmployeeInfo_ByEmployeeCode(employee_code);

                var param = new { EmployeeCode = employee_code };
                var empList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                var List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeTransferViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                        EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                        EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                        EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                        EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                    }).ToList();


                result = 1;
                return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region HttpRequests Backlog

        public ActionResult LoadPreviousDataList([DataSourceRequest]DataSourceRequest request, long EmployeeId)
        {
            try
            {
                List<EmployeeTransferViewModel> List_PreviousData = new List<EmployeeTransferViewModel>();
                var empInfo = employeeService.Get(x => x.EmployeeId == EmployeeId && x.IsActive == true);
                var currentOfficeId = empInfo.OfficeId;
                var currentOfficeDesignation = Convert.ToInt32(empInfo.EmployeeRank);
                var currentOfficeEntry = employeeTransferService.GetMany(p => p.EmployeeId == EmployeeId
                                                                            && p.OfficeId == currentOfficeId
                                                                            && p.DepartmentId == empInfo.DepartmentId
                                                                            && p.OfficeDesignationId == currentOfficeDesignation
                                                                            && p.IsActive == true
                                                                            && p.IsApproved == true
                                                                            && p.OrderNo != 999999
                                                                            ).ToList().OrderByDescending(x => x.JoiningDate).FirstOrDefault();
                var condiitonForEntity = employeeTransferService.GetMany(p => p.EmployeeId == EmployeeId
                                                                            && (p.OrderNo == 999999 || p.OrderNo == 99999)
                                                                            && p.IsActive == true).FirstOrDefault();
                if (currentOfficeEntry == null && condiitonForEntity == null)
                {
                    var model = new EmployeeTransfer();
                    model.EmployeeId = empInfo.EmployeeId;
                    model.OfficeId = empInfo.OfficeId.Value;
                    model.DepartmentId = empInfo.DepartmentId.Value;
                    if (empInfo.SectionId > 0)
                    {
                        model.SectionId = empInfo.SectionId.Value;
                    }

                    model.OfficeDesignationId = Convert.ToInt32(empInfo.EmployeeRank);
                    model.OrderNo = 999999;
                    model.OrderDate = empInfo.FirstJoiningDate;
                    model.IsMutual = false;
                    model.IsTADAApplicable = false;
                    model.IsPlanned = false;
                    model.IsApproved = true;
                    model.IsActive = true;
                    model.JoiningDate = empInfo.FirstJoiningDate;
                    model.CreateUser = LoggedInEmployeeId.Value;
                    model.CreateDate = DateTime.Now;
                    employeeTransferService.Create(model);
                }

                var param = new { EmployeeId = EmployeeId };
                var employeeList = employeeSPService.GetDataWithParameter(param, "trns.SP_GetPreviousOfficeDatabyId");

                List_PreviousData = employeeList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeTransferViewModel()
                {
                    RowSl = Convert.ToInt32(row.Field<long>("rowSl")),
                    Id = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId"),
                    PreviousOfficeType = row.Field<string>("OfficeTypeName"),
                    OfficeId = row.Field<int>("OfficeId"),
                    HeadOfficeId = row.Field<int>("HeadOfficeId"),
                    ZoneId = row.Field<int>("ZoneOfficeId"),
                    AreaId = row.Field<int>("AreaOfficeId"),
                    UnitId = row.Field<int>("UnitOfficeId"),
                    EmployeePreviousOfficeName = row.Field<string>("OfficeName"),
                    DepartmentId = row.Field<int>("DepartmentId"),
                    EmployeePreviousDepartmentName = row.Field<string>("DepartmentName"),
                    SectionId = row.Field<int>("SectionId"),
                    EmployeePreviousSectionName = row.Field<string>("SectionName"),
                    OfficeDesignationId = row.Field<int>("OfficeDesignationId"),
                    EmployeePreviousDesignation = row.Field<string>("OffcDesignName"),
                    OrderNo = row.Field<long>("OrderNo"),
                    OrderDate = row.Field<DateTime>("OrderDate"),
                    JoiningDate = row.Field<DateTime?>("JoiningDate"),
                    ReleaseDate = row.Field<DateTime?>("ReleaseDate"),
                    //EmployeePreviousOfficeJoiningDate = row.Field<string>("JoiningDate"),
                    //EmployeePreviousOfficeReleaseDate = row.Field<string>("ReleaseDate"),
                    IsMutual = row.Field<bool>("IsMutual"),
                    IsTADAApplicable = row.Field<bool>("IsTADAApplicable")
                }).ToList();
                DataSourceResult result = List_PreviousData.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult SaveTransferBacklog(EmployeeTransferViewModel model)
        {
            try
            {
                var message = "";
                TransferBacklogHelper Helper = new TransferBacklogHelper();
                Helper.LoggedInEmployeeId = LoggedInEmployeeId ?? 0;
                Helper._EmployeeTransferService = employeeTransferService;
                var entity = new EmployeeTransfer();
                entity.EmployeeId = model.EmployeeId;
                entity.OfficeDesignationId = model.OfficeDesignationId;
                entity.OfficeId = model.OfficeId;
                entity.DepartmentId = model.DepartmentId;
                entity.SectionId = model.SectionId;
                entity.OrderNo = model.OrderNo;
                entity.OrderDate = Convert.ToDateTime(model.OrderDate);
                entity.IsTADAApplicable = model.IsTADAApplicable;
                entity.IsMutual = model.IsMutual;
                entity.JoiningDate = model.JoiningDate;
                entity.ReleaseDate = model.ReleaseDate;
                bool IsSuccess = Helper.Save(entity, out message);
                return Json(new { result = IsSuccess ? 1 : 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveTransferBacklog_OLD(EmployeeTransferViewModel model)
        {
            try
            {
                var message = "";
                var operationMood = "Create";
                var checkDuplicateEntry = new List<EmployeeTransfer>();

                var employeeId = Convert.ToInt64(model.EmployeeId);
                if (employeeId == 0)
                {
                    return Json(new { result = 0, message = "Invalid Employee Save Failed" }, JsonRequestBehavior.AllowGet);
                }
                if (model.OfficeId <= 0)
                {
                    return Json(new { result = 1, message = "Employee Office Required" }, JsonRequestBehavior.AllowGet);
                }
                if (model.DepartmentId <= 0)
                {
                    return Json(new { result = 0, message = "Employee Department Required" }, JsonRequestBehavior.AllowGet);
                }
                if (model.OfficeDesignationId <= 0)
                {
                    return Json(new { result = 0, message = "Employee Designation Required" }, JsonRequestBehavior.AllowGet);
                }

                if (model.Id == 0)
                {
                    checkDuplicateEntry = employeeTransferService.GetMany(p =>
                                              p.EmployeeId == employeeId && p.OfficeId == model.OfficeId
                                             && p.OrderNo == model.OrderNo).ToList();
                }
                else if (model.Id > 0)
                {
                    checkDuplicateEntry = employeeTransferService.GetMany(p => p.Id != model.Id &&
                                              p.EmployeeId == employeeId && p.OfficeId == model.OfficeId
                                             && p.OrderNo == model.OrderNo).ToList();
                }
                if (checkDuplicateEntry.Any())
                {
                    return Json(new { result = 0, message = "This Order No already exists" }, JsonRequestBehavior.AllowGet);
                }

                var entity = new EmployeeTransfer();
                if (model.Id > 0)
                {
                    entity = employeeTransferService.Get(x => x.IsActive == true && x.IsApproved == true && x.Id == model.Id);
                    entity.EmployeeId = employeeId;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.UtcNow;
                    message = "Update Successfull";

                    operationMood = "Edit";
                }

                //get last employee transfer
                var getLastTransfer = employeeTransferService.GetLastTranserByEmployeeId(employeeId);

                entity.EmployeeId = employeeId;
                entity.OfficeDesignationId = model.OfficeDesignationId;
                entity.OfficeId = model.OfficeId;
                entity.DepartmentId = model.DepartmentId;
                entity.SectionId = model.SectionId;
                entity.OrderNo = model.OrderNo;
                entity.OrderDate = Convert.ToDateTime(model.OrderDate);
                entity.IsTADAApplicable = model.IsTADAApplicable;
                entity.IsMutual = model.IsMutual;
                entity.JoiningDate = model.JoiningDate;
                entity.ReleaseDate = model.ReleaseDate;

                if (model.Id == 0)
                {
                    entity.IsActive = true;
                    entity.IsPlanned = false;
                    entity.IsApproved = true;
                    entity.ReleaseDate = null;
                    entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.CreateDate = DateTime.UtcNow;
                    message = "Save Successfull";
                    employeeTransferService.Create(entity);
                }

                if (model.Id > 0)
                    employeeTransferService.Update(entity);

                //let's update last transter release date of this employee
                if (getLastTransfer != null && getLastTransfer.Id > 0 && operationMood == "Create")
                {
                    getLastTransfer.ReleaseDate = model.ReleaseDate != null ? model.ReleaseDate : ((DateTime)model.JoiningDate).AddDays(-1);
                    getLastTransfer.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    getLastTransfer.UpdateDate = DateTime.UtcNow;
                    employeeTransferService.Update(getLastTransfer);
                }

                return Json(new { result = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region HttpRequests Planning

        public JsonResult SaveTransferPlanning(EmployeeTransferViewModel model)
        {
            int result = 0;           
            string message = string.Empty;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (model.PlannedJoiningDate < model.PlannedReleaseDate)
                        return Json(new { result = 0, message = "Joining Date must be greater than Release Date" }, JsonRequestBehavior.AllowGet);

                    if (model.OfficeId <= 0)
                        return Json(new { result = 0, message = "Employee Office Required" }, JsonRequestBehavior.AllowGet);

                    if (model.DepartmentId <= 0)
                        return Json(new { result = 0, message = "Employee Department Required" }, JsonRequestBehavior.AllowGet);


                    if (model.OfficeDesignationId <= 0)
                        return Json(new { result = 0, message = "Employee Designation Required" }, JsonRequestBehavior.AllowGet);


                    if (model.EntryType == "Approve")
                    {
                        if (String.IsNullOrEmpty(model.JoiningDate.ToString()))
                            return Json(new { result = 0, message = "Joining Date is required" }, JsonRequestBehavior.AllowGet);

                        if (String.IsNullOrEmpty(model.ReleaseDate.ToString()))
                            return Json(new { result = 0, message = "Release Date is required" }, JsonRequestBehavior.AllowGet);

                        if (Convert.ToDateTime(model.JoiningDate) < Convert.ToDateTime(model.ReleaseDate))
                            return Json(new { result = 0, message = "Joining Date must be greater then Release Date" }, JsonRequestBehavior.AllowGet);
                    }

                    var checkEntry = new List<EmployeeTransfer>();

                    if (model.Id == 0)                    
                        checkEntry = employeeTransferService.GetMany(p => p.EmployeeId == model.EmployeeId
                                                                            && p.IsPlanned == true
                                                                            && p.IsActive == true
                                                                            && p.IsApproved == false
                                                                            && p.HasJoined == true).ToList();                    
                    else                    
                        checkEntry = employeeTransferService.GetMany(p => p.Id != model.Id
                                                                            && p.EmployeeId == model.EmployeeId
                                                                            && p.IsPlanned == true
                                                                            && p.IsActive == true
                                                                            && p.IsApproved == false
                                                                            && p.HasJoined == true).ToList();
                    
                    if (checkEntry.Any())
                        return Json(new { result = 0, message = "Previous plan not approved yet, new plan save denied." }, JsonRequestBehavior.AllowGet);

                    var empInfo = employeeService.Get(x => x.EmployeeId == model.EmployeeId && x.IsActive == true);
                    var currentOfficeId = empInfo.OfficeId;
                    var currentOfficeEntry = employeeTransferService.GetMany(p => p.EmployeeId == model.EmployeeId
                                                                                && p.OfficeId == currentOfficeId
                                                                                && p.IsActive == true
                                                                                && p.IsApproved == true
                                                                          //      && p.HasJoined == true
                                                                                ).ToList().OrderByDescending(x => x.JoiningDate).FirstOrDefault();
                    if (currentOfficeEntry == null)
                        return Json(new { result = 0, message = "Current Office Entry Not Found, Insert it first." }, JsonRequestBehavior.AllowGet);

                    var entity = new EmployeeTransfer();

                    if (model.Id > 0)
                        entity = employeeTransferService.Get(x => x.IsActive == true && x.Id == model.Id && x.EmployeeId == model.EmployeeId);

                    entity.OfficeId = model.OfficeId;
                    entity.DepartmentId = model.DepartmentId;
                    entity.OfficeDesignationId = model.OfficeDesignationId;
                    entity.SectionId = model.SectionId;
                    entity.IsActive = true;
                    entity.IsPlanned = true;
                    entity.IsApproved = false;
                    entity.OrderNo = model.OrderNo;
                    entity.OrderDate = Convert.ToDateTime(model.OrderDate);
                    entity.IsTADAApplicable = model.IsTADAApplicable;
                    entity.IsMutual = model.IsMutual;
                    entity.PlannedJoiningDate = model.PlannedJoiningDate;
                    entity.PlannedReleaseDate = model.PlannedReleaseDate;
                    entity.ChangingStatus = model.ChangingStatus;

                    entity.HasJoined = model.HasJoined;
                    
                    if (model.Id == 0)
                    {
                        entity.EmployeeId = model.EmployeeId;
                        entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                        entity.CreateDate = DateTime.UtcNow;

                        //let's update [trns.EmployeeTransfer]
                        employeeTransferService.Create(entity);
                        message = "Saved Successfull";
                        scope.Complete();
                    }
                    else if (model.Id > 0)
                    {
                        ////get last employee transfer
                        var getLastTransfer = employeeTransferService.GetLastTranserByEmployeeId(model.EmployeeId);

                        entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        entity.UpdateDate = DateTime.UtcNow;
                        message = "Updated Successfull";

                        if (model.EntryType == "Approve")
                        {
                            entity.IsApproved = true;
                            entity.JoiningDate = Convert.ToDateTime(model.JoiningDate);
                            entity.ReleaseDate = Convert.ToDateTime(model.ReleaseDate);// it was commented: I(Khalid) have uncomment it.

                            entity.HasJoined = true;

                            int Old_OfficeID = 0;

                            var empEntity = employeeService.Get(x => x.EmployeeId == model.EmployeeId && x.IsActive == true);

                            Old_OfficeID =(int)empEntity.OfficeId;

                            empEntity.OfficeId = entity.OfficeId;
                            empEntity.DepartmentId = entity.DepartmentId;
                            empEntity.SectionId = entity.SectionId;
                            empEntity.EmployeeRank = entity.OfficeDesignationId.ToString();
                            empEntity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                            empEntity.UpdateDate = DateTime.UtcNow;
                             
                            //let's update [dbo.Employee]
                            //employeeService.Update(empEntity);

                             // NOTE: While Update Employee Table:
                             //Trigger is Called: trg_UpdateEmployeeSalary
                            try
                            {
                                employeeService.Update(empEntity);
                            }
                            catch (Exception ex)
                            {
                                var messagess = ex.InnerException?.InnerException?.Message
                                              ?? ex.InnerException?.Message
                                              ?? ex.Message;

                                throw new Exception(message);
                            }
                             
                            message = "Approved Successfull";
                        }

                        

                       employeeTransferService.Update(entity);



                        //let's update last transter release date of this employee on [trns.EmployeeTransfer]
                        if (getLastTransfer != null && getLastTransfer.Id > 0 && model.EntryType == "Approve")
                        {
                            getLastTransfer.ReleaseDate = model.ReleaseDate != null ? model.ReleaseDate : ((DateTime)model.JoiningDate).AddDays(-1);
                            getLastTransfer.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                            getLastTransfer.UpdateDate = DateTime.UtcNow;
                            employeeTransferService.UpdateEmployeeTransferReleaseDate(getLastTransfer);
                        }

                        scope.Complete();
                    }

                    result = 1;
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    scope.Dispose();                    
                    return Json(new { result = 0, message = "Save Denied" }, JsonRequestBehavior.AllowGet);
                }
            }            
        }

        #endregion

        #region Methods

        private void MapOfficeNevigationDropDown(EmployeeTransferViewModel entity)
        {
            entity.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            entity.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            entity.AreaList = commonDynamicDropDown.ddlInitial();
            entity.UnitList = commonDynamicDropDown.ddlInitial();
            entity.DepartmentNameList = commonDynamicDropDown.GetAllActiveDepartmentList();
            entity.SectionList = commonDynamicDropDown.ddlInitial();
            entity.RankList = commonDynamicDropDown.GetAllOfficeDesignationList();
            entity.YesNoList = commonStaticDropDown.GetYesNoList();
        }

        private void GetOfficeInfo(EmployeeTransferViewModel entity)
        {
            var office = officeService.GetById(Convert.ToInt32(entity.OfficeId));
            entity.OfficeTypeId = Convert.ToInt32(office.OfficeTypeId);

            if (office.OfficeTypeId == 6)
            {
                entity.AreaId = Convert.ToInt32(officeService.GetAll().Where(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault().OfficeId);
                entity.ZoneId = Convert.ToInt32(officeService.GetAll().Where(o => o.OfficeCode == office.SecondLevel).FirstOrDefault().OfficeId);
                entity.UnitId = office.OfficeId;
            }
            else if (office.OfficeTypeId == 5)
            {
                entity.ZoneId = Convert.ToInt32(officeService.GetAll().Where(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault().OfficeId);
                entity.AreaId = office.OfficeId;
            }
            else if (office.OfficeTypeId == 4)
            {
                entity.ZoneId = office.OfficeId;
            }
            else if (office.OfficeTypeId == 3)
            {
                entity.ProjectId = office.OfficeId;
            }
            else if (office.OfficeTypeId == 1)
            {
                entity.HeadOfficeId = office.OfficeId;
            }
        }

        private EmployeeTransferViewModel SP_GetEmployeeInfo_ByEmployeeCode(string EmployeeCode)
        {
            var param = new { EmployeeCode = EmployeeCode };
            var dataList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

            var viewList = dataList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeTransferViewModel
                {
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                    EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                    EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                    EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                    EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                }).ToList();
            return viewList[0];
        }

        private List<EmployeeTransferViewModel> SP_GetTransferInformation(string andCondition)
        {
            var param = new { AndCondition = andCondition };

            //get list from [trns.EmployeeTransfer]
            var List = employeeSPService.GetDataWithParameter(param, "trns.SP_GetTransferInformation");
            var viewList = List.Tables[0].AsEnumerable()
                .Select((row, sl) => new EmployeeTransferViewModel
                {
                    RowSl = sl + 1,
                    Id = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DepartmentId = row.Field<int>("DepartmentId"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    OfficeDesignationId = row.Field<int>("OfficeDesignationId"),
                    OfficeDesignationName = row.Field<string>("OfficeDesignationName"),
                    OrderNo = row.Field<long>("OrderNo"),

                    OrderDate = row.Field<DateTime?>("OrderDate"),
                    JoiningDate = row.Field<DateTime?>("JoiningDate"),
                    ReleaseDate = row.Field<DateTime?>("ReleaseDate"),
                    PlannedJoiningDate = row.Field<DateTime?>("PlannedJoiningDate"),
                    PlannedReleaseDate = row.Field<DateTime?>("PlannedReleaseDate"),
                    IsTADAApplicable = row.Field<bool>("IsTADAApplicable"),
                    IsMutual = row.Field<bool>("IsMutual"),
                    //HasJoined = row.Field<bool>("HasJoined")
                }).ToList();

            return viewList;
        }

        #endregion
    }
}