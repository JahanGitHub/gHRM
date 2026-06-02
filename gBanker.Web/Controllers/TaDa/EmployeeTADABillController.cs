using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Service.TaDa;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.TaDa;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.TaDa
{
    public class EmployeeTADABillController : BaseController
    {
        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeTADABillService employeeTADABillService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeSalaryIncentiveService employeeSalaryIncentiveService;
        private readonly IPRComponentService pRComponentService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IEmployeeShortInfoService employeeShortInfoService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly ITADAPurposeService tADAPurposeService;
        public EmployeeTADABillController(IEmployeeTADABillService employeeTADABillService,
            IEmployeeSPService employeeSpService,
            IEmployeeSalaryIncentiveService employeeSalaryIncentiveService,
            IEmployeeService employeeService,
            IPRComponentService pRComponentService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IOfficeTypeService officeTypeService
            , IEmployeeSPService employeeSPService
            , IOfficeDesignationService officeDesignationService
            , IEmployeeShortInfoService employeeShortInfoService
            , IOfficeService officeService
            , IEmployeeDepartmentService employeeDepartmentService
            , IEmployeeDesignationService employeeDesignationService
            , ITADAPurposeService tADAPurposeService
            )
        {
            this.employeeTADABillService = employeeTADABillService;
            this.employeeSpService = employeeSpService;
            this.employeeSalaryIncentiveService = employeeSalaryIncentiveService;
            this.employeeService = employeeService;
            this.pRComponentService = pRComponentService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.officeTypeService = officeTypeService;
            this.employeeSPService = employeeSPService;
            this.officeDesignationService = officeDesignationService;
            this.employeeShortInfoService = employeeShortInfoService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.tADAPurposeService = tADAPurposeService;
        }

        #endregion

        #region ActionResult

        //public ActionResult Index()
        //{
        //    return View();
        //}


        public void MapDropdownForTada(EmployeeTADABillViewModel model)
        {
            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            officeType_items.Add(new SelectListItem() { Text = "Multiple Locations", Value = "Multiple Locations"});
            officeType_items.Add(new SelectListItem() { Text = "Courier", Value = "Courier" });
            officeType_items.Add(new SelectListItem() { Text = "Others", Value = "Others" });
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //ofc_items.AddRange(viewOfcList);
            model.OfficeList = ofc_items;

            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
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

            
            var purposeList = tADAPurposeService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewPurposeList = purposeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Purpose,
                Value = p.Purpose
            });
            var travelPurposeName = new List<SelectListItem>();
            travelPurposeName.AddRange(viewPurposeList);
            model.TravelPurposeNameList = travelPurposeName;
        }

        public ActionResult Create()
        {
            var model = new EmployeeTADABillViewModel();
            MapDropdownForTada(model);
            return View(model);
        }
        

        public ActionResult BillSearch()
        {
            return View();
        }

        public ActionResult TADABillWithSalary()
        {
            var entity = new EmployeeTADABillViewModel();
            mapDropDownList(entity);
            return View(entity);
        }
        public ActionResult TADAReport()
        {
            var entity = new EmployeeTADABillViewModel();
            MapDropDownListForTADAReport(entity);
            return View(entity);
        }
        #endregion

        #region Functions
        private void mapDropDownList(EmployeeTADABillViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                //monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            entity.MonthList = monthList;
        }

        private void MapDropDownListForTADAReport(EmployeeTADABillViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                //monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            entity.MonthList = monthList;

            var officeType = officeTypeService.GetMany(p => p.IsActive == true).ToList();
            var viewOfficeType = officeType.Select(p => new SelectListItem()
            {
                Text = p.OfficeTypeName,
                Value = p.OfficeTypeId.ToString()
            });
            var officeTypeList = new List<SelectListItem>();
            officeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            officeTypeList.AddRange(viewOfficeType);
            entity.OfficeTypeList = officeTypeList;

            //var employeeType = new List<SelectListItem>();
            //employeeType.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //employeeType.Add(new SelectListItem() { Text = "Acting Zone Supervisor", Value = "43" });
            //employeeType.Add(new SelectListItem() { Text = "Support Engineer (HDW)", Value = "52" });
            //employeeType.Add(new SelectListItem() { Text = "Others", Value = "100" });
            //employeeType.Add(new SelectListItem() { Text = "Group By All", Value = "1000" });
            //employeeType.Add(new SelectListItem() { Text = "General", Value = "10000" });
            //entity.EmployeeRankList = employeeType;


            var employeeTypes = officeDesignationService.GetMany(p => p.IsActive == true).ToList();
            var viewemployeeType = employeeTypes.Select(p => new SelectListItem()
            {
                Text = p.OffcDesignName,
                Value = p.OfficeDesignationId.ToString()
            });
            var employeeType = new List<SelectListItem>();
            employeeType.Add(new SelectListItem() { Text = "Group By All", Value = "1000" });
            employeeType.AddRange(viewemployeeType);
            entity.EmployeeRankList = employeeType;
            

            var departlist = employeeDepartmentService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.DepartmentId);
            var viewdepartlist = departlist.Select(a => new SelectListItem()
            {
                Value = a.DepartmentId.ToString(),
                Text = a.DepartmentName
            });
            var listviewdepartlist = new List<SelectListItem>();
            listviewdepartlist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listviewdepartlist.AddRange(viewdepartlist);
            entity.DepartmentNameList = listviewdepartlist;

            var designationlist = employeeDesignationService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.DesignationId);
            var viewdesignationlist = designationlist.Select(a => new SelectListItem()
            {
                Value = a.DesignationId.ToString(),
                Text = a.DesignationName
            });
            var listviewdesignationlist = new List<SelectListItem>();
            listviewdesignationlist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listviewdesignationlist.AddRange(viewdesignationlist);
            entity.DesignationNameList = listviewdesignationlist;

            var listOfEmployee = new List<SelectListItem>();
            listOfEmployee.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            entity.EmployeeNameList = listOfEmployee;
        }


        [HttpPost]
        public JsonResult SaveEmployeeTADAData(EmployeeTADABillViewModel CreateObject)
        {
            int result = 0;
            string message = String.Empty;
            try
            {
                if (CreateObject != null)
                {
                    var employee = employeeService.GetByEmpId(CreateObject.EmployeeId);
                    if (employee != null)
                    {
                        if (employeeTADABillService.GetMany(p => p.MemoNo == CreateObject.MemoNo).ToList().Any())
                        {
                            result = 0;
                            return Json(new { Result = result, Message = "Duplicate Memo Number, Save Denied" }, JsonRequestBehavior.AllowGet);
                        }
                        var prcomponent = pRComponentService.GetAll().Where(p => p.ComponentName == "TA/DA Allowance" && p.EmployeeTypeId == employee.EmployeeTypeId && p.IsActive == true && p.EmployeeStatusId == employee.EmployeeStatusId).ToList();
                        if (prcomponent.Count >0)
                        {
                            var entity = new EmployeeTADABill();
                            entity.EmployeeId = CreateObject.EmployeeId;
                            entity.EmployeeCode = CreateObject.EmployeeCode;
                            entity.MemoNo = CreateObject.MemoNo;
                            entity.TravelDate = CreateObject.TravelDate;
                            entity.TravelPlace = CreateObject.TravelPlace;
                            entity.TravelPurpose = CreateObject.TravelPurpose;
                            entity.ApproveDate = CreateObject.ApproveDate;
                            entity.ClaimAmount = CreateObject.ClaimAmount;
                            entity.ApproveAmount = CreateObject.ApproveAmount;
                            entity.IsAmountPaid = false;
                            entity.IsActive = true;
                            entity.CreateDate = DateTime.UtcNow;
                            entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                            employeeTADABillService.Create(entity);
                            result = 1;
                            message = "TA/DA Save Successfull";
                        }
                        else
                        {
                            result = 0;
                            message = "TA/DA Generation Not Valid For the Employee, Please check Component Configuration";
                        }
                    }
                    else
                    {
                        result = 0;
                        message = "No Valid Employee Found, Save Denied";
                    }
                }
            }

            catch (Exception e)
            {
                result = 0;
                message = "Save Denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateEmployeeTADAData(EmployeeTADABillViewModel CreateObject)
        {
            int result = 0;
            string message = String.Empty;

            try
            {
                if (CreateObject.TADABillId > 0)
                {
                    var entity = employeeTADABillService.GetById(CreateObject.TADABillId);
                    entity.MemoNo = CreateObject.MemoNo;
                    entity.TravelDate = CreateObject.TravelDate;
                    entity.TravelPlace = CreateObject.TravelPlace;
                    entity.TravelPurpose = CreateObject.TravelPurpose;
                    entity.ApproveDate = CreateObject.ApproveDate;
                    entity.ClaimAmount = CreateObject.ClaimAmount;
                    entity.ApproveAmount = CreateObject.ApproveAmount;
                    entity.IsAmountPaid = false;
                    entity.IsActive = true;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);

                    employeeTADABillService.Update(entity);
                    result = 1;
                    message = "TA/DA Updated Successfull";
                }
                else
                {
                    result = 0;
                    message = "TA/DA Updated Failed";
                }
            }
            catch (Exception e)
            {
                result = 0;
                message = "TA/DA Updated Denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);


        }
        //[HttpPost]
        public JsonResult LoadTADABILLInfo(int jtStartIndex, int jtPageSize,string jtSorting, string employee_code)
        {
            try
            {
                var param = new { EmployeeCode = employee_code.Trim() };
                var getInfo = employeeSpService.GetDataWithParameter(param, "tada.SP_GetAllTADAListByEmployeeCode");
                var view_GetInfo = getInfo.Tables[0].AsEnumerable().Select(a => new EmployeeTADABillViewModel()
                {
                    rowSl = a.Field<string>("rowSl"),
                    TADABillId = a.Field<int>("TADABillId"),
                    EmployeeId = a.Field<long>("EmployeeId"),
                    EmployeeCode = a.Field<string>("EmployeeCode"),
                    EmployeeName = a.Field<string>("EmployeeName"),
                    MemoNo = a.Field<int>("MemoNo"),
                    TravelDateMsg = a.Field<string>("TravelDate"),
                    OfficeTypeId = a.Field<int?>("OfficeTypeId"),
                    HeadOfficeId = a.Field<int?>("HeadOfficeId"),
                    ZoneOfficeId = a.Field<int?>("ZoneOfficeId"),
                    AreaOfficeId = a.Field<int?>("AreaOfficeId"),
                    UnitOfficeId = a.Field<int?>("UnitOfficeId"),
                    TravelPlaceId = a.Field<int?>("TravelPlaceId"),
                    TravelPlace = a.Field<string>("TravelPlace"),
                    TravelPurpose = a.Field<string>("TravelPurpose"),
                    ApproveDateMsg = a.Field<string>("ApproveDate"),
                    ClaimAmount = a.Field<decimal>("ClaimAmount"),
                    ApproveAmount = a.Field<decimal>("ApproveAmount")
                }).ToList();

                var currentPageRecords = view_GetInfo.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new
                {
                    Result = "OK",
                    Records = currentPageRecords,
                    TotalRecordCount = view_GetInfo.LongCount(),
                    JsonRequestBehavior.AllowGet
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    Result = "OK",
                    JsonRequestBehavior.AllowGet
                });
            }
        }


        public JsonResult DeleteTADABill(int TADABillId)
        {
            int result = 0;
            string message = String.Empty;
            try
            {
                var entity = employeeTADABillService.GetById(TADABillId);
                entity.IsActive = false;
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                result = 1;
                employeeTADABillService.Update(entity);
                result = 1;
                message = "TA/DA Bill Deleted Successfull";
            }
            catch (Exception e)
            {
                result = 0;
                message = "TA/DA Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetWaitingTADABillList(int jtStartIndex, int jtPageSize, string jtSorting, string FilterBy, string FilterValue, string FilterValuedate, string ApprovalStatus)
        {
            try
            {
                StringBuilder AndCondition = new StringBuilder();

                if (ApprovalStatus == "A") // Approved
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 1");
                }
                else if (ApprovalStatus == "N") // Not Approve
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 0");
                }
                else if (ApprovalStatus == "E")//Encashment with salary
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    //AndCondition.Append(" AND LS.IsApproved = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 0");
                }


                var param = new { AndCondition = AndCondition.ToString() };
                var tadaBill = employeeSpService.GetDataWithParameter(param, "tada.SP_GetAllTADAList");

                List<EmployeeTADABillViewModel> tadaBillList = new List<EmployeeTADABillViewModel>();

                tadaBillList = tadaBill.Tables[0].AsEnumerable().Select(a => new EmployeeTADABillViewModel()
                {
                    rowSl = a.Field<string>("rowSl"),
                    TADABillId = a.Field<int>("TADABillId"),
                    EmployeeId = a.Field<long>("EmployeeId"),
                    EmployeeCode = a.Field<string>("EmployeeCode"),
                    EmployeeName = a.Field<string>("EmployeeName"),
                    MemoNo = a.Field<int>("MemoNo"),
                    TravelDateMsg = a.Field<string>("TravelDate"),
                    TravelPlace = a.Field<string>("TravelPlace"),
                    TravelPurpose = a.Field<string>("TravelPurpose"),
                    ApproveDateMsg = a.Field<string>("ApproveDate"),
                    ClaimAmount = a.Field<decimal>("ClaimAmount"),
                    ApproveAmount = a.Field<decimal>("ApproveAmount"),
                    IsAmountPaid = a.Field<bool>("IsAmountPaid"),
                    Remark = a.Field<string>("Remark")
                }).ToList();
                var currentPageRecords = tadaBillList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = tadaBillList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetTADABillList([DataSourceRequest]DataSourceRequest request, string FilterBy, string FilterValue, string FilterValuedate, string ApprovalStatus)
        {
            try
            {
                StringBuilder AndCondition = new StringBuilder();

                if (ApprovalStatus == "A") // Approved
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 1");
                }
                else if (ApprovalStatus == "N") // Not Approve
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 0");
                }
                else if (ApprovalStatus == "E")//Encashment with salary
                {
                    //AndCondition.Append(" AND LS.IsActive = 1");
                    //AndCondition.Append(" AND LS.IsApproved = 1");
                    AndCondition.Append(" AND tb.IsAmountPaid = 0");
                }


                var param = new { AndCondition = AndCondition.ToString() };
                var tadaBill = employeeSpService.GetDataWithParameter(param, "tada.SP_GetAllTADAList");

                List<EmployeeTADABillViewModel> tadaBillList = new List<EmployeeTADABillViewModel>();

                tadaBillList = tadaBill.Tables[0].AsEnumerable().Select(a => new EmployeeTADABillViewModel()
                {
                    rowSl = a.Field<string>("rowSl"),
                    TADABillId = a.Field<int>("TADABillId"),
                    EmployeeId = a.Field<long>("EmployeeId"),
                    EmployeeCode = a.Field<string>("EmployeeCode"),
                    EmployeeName = a.Field<string>("EmployeeName"),
                    MemoNo = a.Field<int>("MemoNo"),
                    TravelDateMsg = a.Field<string>("TravelDate"),
                    TravelPlace = a.Field<string>("TravelPlace"),
                    TravelPurpose = a.Field<string>("TravelPurpose"),
                    ApproveDateMsg = a.Field<string>("ApproveDate"),
                    ClaimAmount = a.Field<decimal>("ClaimAmount"),
                    ApproveAmount = a.Field<decimal>("ApproveAmount"),
                    IsAmountPaid = a.Field<bool>("IsAmountPaid"),
                    Remark = a.Field<string>("Remark")
                }).ToList();
                DataSourceResult result = tadaBillList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult ProvideTADABill(int TADABillId, string Remark)
        {
            var result = 0;
            try
            {
                var entity = employeeTADABillService.GetById(TADABillId);
                entity.IsAmountPaid = true;
                entity.UpdateDate = DateTime.UtcNow;
                entity.Remark = Remark;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                result = 1;
                employeeTADABillService.Update(entity);
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveTADABillWithSalary(List<EmployeeTADABillViewModel> BillList)
        {
            var result = 0;
            if (BillList.Count <= 0)
            {
                return Json(new { Result = result, Message = "Nothing found to send salary" }, JsonRequestBehavior.AllowGet);
            }
            if (employeeMonthlySalaryService.GetApprovedSalary(BillList[0].Year, BillList[0].Month).ToList().Any())
            {
                return Json(new { Result = result, Message = "Salary Already Approved for this month, Generation Denied" }, JsonRequestBehavior.AllowGet);
            }
            if (employeeMonthlySalaryService.GetSendForApprovalSalary(BillList[0].Year, BillList[0].Month).ToList().Any())
            {
                return Json(new { Result = result, Message = "Salary Already Send for Approval for this month, Generation Denied" }, JsonRequestBehavior.AllowGet);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (BillList.Count > 0)
                    {
                        foreach (var item in BillList)
                        {
                            var employeeCode = item.EmployeeCode.Trim();
                            var employeeDetail = employeeService.GetByCode(employeeCode);
                            var empTypeId = employeeDetail.EmployeeTypeId;
                            var empStatus = employeeDetail.EmployeeStatusId;

                            var firstDate = new DateTime(item.Year, item.Month, 1);
                            var lastDate = firstDate.AddMonths(1).AddDays(-1);

                            var prComponent = pRComponentService.GetAll().Where(x => x.IsActive == true && x.ComponentName == "TA/DA Allowance" && x.EmployeeTypeId == empTypeId && x.EmployeeStatusId == empStatus).FirstOrDefault();
                            if (prComponent != null)
                            {
                                var prComponentId = prComponent.PRComponentID;
                                if (prComponentId > 0)
                                {
                                    var entity = employeeTADABillService.GetById(item.TADABillId);
                                    entity.IsAmountPaid = true;
                                    entity.UpdateDate = DateTime.UtcNow;
                                    entity.Remark = "With Salary";
                                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                                    employeeTADABillService.Update(entity);

                                    var existingIncentive = employeeSalaryIncentiveService.GetAll().Where(x => x.IsActive == true && x.PRComponentId == prComponentId && x.EmployeeId == employeeDetail.EmployeeId && x.StartDate >= firstDate && x.EndDate <= lastDate).FirstOrDefault();

                                    if (existingIncentive != null)
                                    {
                                        var currentAmt = Convert.ToDecimal(entity.ApproveAmount);
                                        var previousAmt = existingIncentive.PRComponentAmount;

                                        existingIncentive.PRComponentAmount = currentAmt + previousAmt;
                                        existingIncentive.IsActive = true;
                                        existingIncentive.IsApproved = true;
                                        existingIncentive.StartDate = firstDate;
                                        existingIncentive.EndDate = lastDate;

                                        existingIncentive.UpdateDate = DateTime.UtcNow;
                                        existingIncentive.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);

                                        employeeSalaryIncentiveService.Update(existingIncentive);

                                    }
                                    else
                                    {
                                        var salaryIncentive = new EmployeeSalaryIncentive();
                                        salaryIncentive.EmployeeId = employeeDetail.EmployeeId;
                                        salaryIncentive.PRComponentId = prComponentId;
                                        salaryIncentive.ProductId = 0;
                                        salaryIncentive.SerialId = 0;
                                        salaryIncentive.PRComponentAmount = Convert.ToDecimal(entity.ApproveAmount);
                                        salaryIncentive.PRComponentHour = 0;
                                        salaryIncentive.IsActive = true;
                                        salaryIncentive.IsApproved = true;
                                        salaryIncentive.StartDate = firstDate;
                                        salaryIncentive.EndDate = lastDate;
                                        salaryIncentive.CreateDate = DateTime.UtcNow;
                                        salaryIncentive.CreatedBy = Convert.ToInt64(LoggedInEmployeeId);
                                        salaryIncentive.UpdateDate = DateTime.UtcNow;
                                        salaryIncentive.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);

                                        employeeSalaryIncentiveService.Create(salaryIncentive);
                                    }
                                    result = 1;

                                }
                                else
                                {
                                    //scope.Dispose();
                                    result = 0;
                                }
                            }

                        }
                        scope.Complete();
                    }
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    result = 0;
                }
            }
            return Json(new { Result = result, Message = "Save Successfull" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                var resultObj = new EmployeeViewModel();
                var Emp = employeeService.GetByCode(employee_code);
                if (Emp != null)
                {
                    List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();                    
                    var _resultObj = employeeShortInfoService.Get(b => b.EmployeeId == Emp.EmployeeId);
                    resultObj.EmployeeId = _resultObj.EmployeeId;
                    resultObj.EmployeeName = _resultObj.EmployeeName;
                    resultObj.OfficeId = _resultObj.OfficeId;
                    resultObj.OfficeName = _resultObj.OfficeName;
                    resultObj.DepartmentName = _resultObj.DepartmentName;
                    resultObj.DesignationName = _resultObj.DesignationName;
                    resultObj.EmployeeStatusId = _resultObj.EmployeeStatusId.Value;
                    resultObj.EmployeeStatusName = _resultObj.EmployeeStatusName;
                    if (resultObj != null)
                    {
                        return Json(resultObj, JsonRequestBehavior.AllowGet);
                    }
                    return Json(resultObj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(resultObj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLastTADAMemoNo()
        {
            try
            {
                int result = 0;
                var lastMemoNoDataset = employeeSPService.GetDataWithoutParameter("tada.SP_Get_LastTADAMemoNo");
                if (lastMemoNoDataset.Tables.Count > 0)
                {
                    result = Convert.ToInt32(lastMemoNoDataset.Tables[0].Rows[0][0]) + 1;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult LoadTravelPlace(string searchedText)
        {
            try
            {
                int result = 0;

                var param = new { Text = searchedText };
                var OfficeDataset = employeeSpService.GetDataWithParameter(param, "SP_Get_OfficeBySearch");

                if (OfficeDataset.Tables.Count > 0)
                {
                    var officeList = OfficeDataset.Tables[0];
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult TADAReportPrint(int Year, int Month, int OfficeTypeId, int EmployeeRank)
        //{
        //    try
        //    {
        //        var firsDateOfMonth = new DateTime(Year, Month, 1);
        //        DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
        //        var lastDateOfMonth = firstOfNextMonth.AddDays(-1);
        //        var andCondition = "";
        //        if (OfficeTypeId > 0)
        //        {
        //            andCondition = " AND o.OfficeTypeId = " + OfficeTypeId;
        //        }
        //        if (OfficeTypeId == 0 && EmployeeRank == 100)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank  NOT IN(43,52)";
        //        }
        //        if (OfficeTypeId > 0 && EmployeeRank == 100)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank NOT IN(43,52)";
        //        }
        //        if (OfficeTypeId == 0 && EmployeeRank == 43)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
        //        }
        //        if (OfficeTypeId > 0 && EmployeeRank == 43)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
        //        }
        //        if (OfficeTypeId == 0 && EmployeeRank == 52)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
        //        }
        //        if (OfficeTypeId > 0 && EmployeeRank == 52)
        //        {
        //            andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
        //        }
        //        var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, AndCondition = andCondition};
        //        var data = employeeSPService.GetDataWithParameter(param, "SP_RPT_GetTADAReport");
        //        var reportParam = new Dictionary<string, object>();
        //        if (EmployeeRank == 1000)
        //        {
        //            ReportHelper.PrintReport("rpt_TADABillReport_GroupBy.rpt", data.Tables[0], reportParam);
        //        }
        //        else
        //        {
        //            ReportHelper.PrintReport("rpt_TADABillReport.rpt", data.Tables[0], reportParam);
        //        }
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public ActionResult EmployeeInformation()
        //{
        //    var EmpId = 0;
        //    var emp = employeeService.GetAll().Select(p => p.EmployeeId == EmpId);
        //    return View(Emp);
        //}

        public ActionResult TADAReportPrintNew(int Year, int Month, int OfficeTypeId, int EmployeeRank, string EmployeeNameSig, string DepartmentNameSig, string DesignationNameSig)
        {
            try
            {
                var firsDateOfMonth = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDateOfMonth = firstOfNextMonth.AddDays(-1);
                var andCondition = "";
                if (OfficeTypeId > 0)
                {
                    andCondition = " AND o.OfficeTypeId = " + OfficeTypeId;
                }
                if (OfficeTypeId == 0 && EmployeeRank != 0)
                {
                    andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
                }
                if (OfficeTypeId > 0 && EmployeeRank != 0)
                {
                    andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
                }
                if (EmployeeRank == 1000 && OfficeTypeId == 0)
                {
                    andCondition = "";
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig= EmployeeNameSig, DepartmentNameSig= DepartmentNameSig, DesignationNameSig= DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.PrintReport("TaDa/rpt_TADABillReport_GroupByNew.rpt", data.Tables[0], reportParam);
                }
                else if (EmployeeRank == 1000 && OfficeTypeId > 0)
                {
                    andCondition = "";
                    andCondition = " AND o.OfficeTypeId = " + OfficeTypeId;
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.PrintReport("TaDa/rpt_TADABillReport_GroupByNew.rpt", data.Tables[0], reportParam);
                }
                else
                {
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.PrintReport("TaDa/rpt_TADABillReportNew.rpt", data.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult PrintTADABillReportExcelNew(int Year, int Month, int OfficeTypeId, int EmployeeRank, string EmployeeNameSig, string DepartmentNameSig, string DesignationNameSig)
        {
            try
            {
                var firsDateOfMonth = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDateOfMonth = firstOfNextMonth.AddDays(-1);
                var andCondition = "";
                if (OfficeTypeId > 0)
                {
                    andCondition = " AND o.OfficeTypeId = " + OfficeTypeId;
                }
                if (OfficeTypeId == 0 && EmployeeRank != 0)
                {
                    andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
                }
                if (OfficeTypeId > 0 && EmployeeRank != 0)
                {
                    andCondition = andCondition + " AND e.EmployeeRank = " + EmployeeRank;
                }
                if (EmployeeRank == 1000 && OfficeTypeId == 0)
                {
                    andCondition = "";
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.ExportExcelReport("TaDa/rpt_TADABillReport_GroupByNew.rpt", data.Tables[0], reportParam);
                }
                else if (EmployeeRank == 1000 && OfficeTypeId > 0)
                {
                    andCondition = "";
                    andCondition = " AND o.OfficeTypeId = " + OfficeTypeId;
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.ExportExcelReport("TaDa/rpt_TADABillReport_GroupByNew.rpt", data.Tables[0], reportParam);
                }
                else
                {
                    var param = new { FirstDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig, AndCondition = andCondition };
                    var data = employeeSPService.GetDataWithParameter(param, "tada.SP_RPT_GetTADAReportNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                    reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                    reportParam.Add("DesignationNameSig", DesignationNameSig);
                    ReportHelper.ExportExcelReport("TaDa/rpt_TADABillReportNew.rpt", data.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}