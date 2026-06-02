using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Text;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Web.CommonDropdown;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Web.Helpers;
using System.Transactions;

namespace gHRM.Web.Controllers
{
    public class EmployeePromotionConfigurationController : BaseController
    {
        #region variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeePromotionService employeePromotionService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IPromotionTypeService promotionTypeService;
        public EmployeePromotionConfigurationController(
              IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IEmployeePromotionService employeePromotionService
            , IOfficeDesignationService officeDesignationService
            , IPromotionTypeService promotionTypeService
            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeePromotionService = employeePromotionService;
            this.officeDesignationService = officeDesignationService;
            this.promotionTypeService = promotionTypeService;
        }

        #endregion

        #region Actions
        public ActionResult PromotionBacklogIndex()
        {
            var model = new EmployeeViewModel();

            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();

            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult PromotionBacklogEntry(string EmployeeCode)
        {
            var model = new EmployeePromotionViewModel();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.PromotionTypeList = commonDynamicDropDown.PromotionTypeList();

            model.StatusPeriodInMonthList = commonStaticDropDown.GetPeriodInMonthsList();
            model.YesNoList = commonStaticDropDown.YesNoDropDown_Int();

            //var promotionType = promotionTypeService.GetMany(p => p.IsActive == true).ToList();
            //var viewpromotionType = promotionType.Select(p => new SelectListItem()
            //{
            //    Text = p.PromotionTypeName,
            //    Value = p.PromotionTypeId.ToString()
            //});
            //var promotionTypelist = new List<SelectListItem>();
            //promotionTypelist.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            //promotionTypelist.AddRange(viewpromotionType);

            return View(model);
        }

        #endregion

        #region HttpRequests

        //public JsonResult loadPromotionBacklogList([DataSourceRequest]DataSourceRequest request)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("AND vp.IsActive=1");

        //    var PromotInfoList = GetPromotionInformation(sb.ToString());
        //    DataSourceResult result = PromotInfoList.ToDataSourceResult(request);
        //    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetEmpInfoByCodeEmp(string employee_code)
        {
            var result = 0;
            try
            {
                //var employeeInfo = SP_GetEmployeeInfo_ByEmployeeCode(employee_code);

                var param = new { EmployeeCode = employee_code };
                var empList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                var List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeePromotionViewModel
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

        public JsonResult LoadPreviousDataList([DataSourceRequest]DataSourceRequest request, long EmployeeId)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                int _EmployeeId = Convert.ToInt32(EmployeeId);
                sb.Append(" AND vp.EmployeeId=" + _EmployeeId);


                var PromotInfoList = GetPromotionInformation(sb.ToString());

                if (!PromotInfoList.Any())
                {
                    var empInfo = employeeService.Get(x => x.EmployeeId == EmployeeId && x.IsActive == true);
                    var entity = new EmployeePromotion();
                    entity.EmployeeId = EmployeeId;
                    entity.PromotionTypeId = 0;
                    entity.DesignationId = Convert.ToInt32(empInfo.DesignationId);
                    entity.PromotionDate = empInfo.FirstJoiningDate;
                    entity.IsReviewed = false;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    employeePromotionService.Create(entity);
                    PromotInfoList = GetPromotionInformation(sb.ToString());
                }

                DataSourceResult result = PromotInfoList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult SavePromotionBackLog(int PromotionId, int EmployeeId, int DesignationId, 
            DateTime? PromotionDate, DateTime? NextReviewDate, bool IsReviewed, int PromotionTypeId)
        {
            int result = 0;
            string message = string.Empty;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var entity = new EmployeePromotion();                   

                    if (PromotionId > 0)
                    {
                        if (IsReviewed == false)
                        {
                            var isDuplicate = employeePromotionService.GetMany(p => p.EmployeeId == EmployeeId && p.PromotionId != PromotionId && p.IsActive == true && p.IsReviewed == false).ToList();
                            if (isDuplicate.Any())
                            {
                                scope.Dispose();
                                message = "Another review is pending, Save denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }

                            var emp_entity = employeeService.GetByEmpId(EmployeeId);
                            if (emp_entity.DesignationId != DesignationId)
                            {
                                emp_entity.DesignationId = DesignationId;
                                employeeService.Update(emp_entity);
                            }
                        }

                        entity = employeePromotionService.GetById(PromotionId);
                        entity.PromotionTypeId = PromotionTypeId;
                        entity.DesignationId = DesignationId;
                        entity.PromotionDate = PromotionDate;
                        entity.NextReviewDate = NextReviewDate;
                        entity.IsReviewed = IsReviewed;
                        entity.IsActive = true;
                        entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateDate = DateTime.UtcNow;

                        //let's update for [promo.EmployeePromotion]
                        employeePromotionService.Update(entity);
                    
                        message = "Update Successfully";
                    }
                    else
                    {
                        if (IsReviewed == false)
                        {
                            var isDuplicate = employeePromotionService.GetMany(p => p.EmployeeId == EmployeeId &&  p.IsActive == true && p.IsReviewed == false).ToList();
                            if (isDuplicate.Any())
                            {
                                scope.Dispose();
                                message = "Another review is pending, Save denied";
                                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        
                        entity.EmployeeId = EmployeeId;
                        entity.PromotionTypeId = PromotionTypeId;
                        entity.DesignationId = DesignationId;
                        entity.PromotionDate = PromotionDate;
                        entity.NextReviewDate = NextReviewDate;
                        entity.IsReviewed = IsReviewed;
                        entity.IsActive = true;
                        entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.UtcNow;

                        //let's add into [promo.EmployeePromotion]
                        employeePromotionService.Create(entity);
                        message = "Save Successfully";
                    }

                    //if (PromotionTypeId == 7) // KHALID Added: If Promotion then update EmployeeTable DesignationId
                    //{

                    //    var promotion = employeePromotionService.GetById(EmployeeId);
                    //    var employee = employeeService.GetById(EmployeeId);
                    //    if (promotion == null)
                    //    {

                    //        promotion.EmployeeId = EmployeeId;
                    //        promotion.PromotionTypeId = PromotionTypeId;
                    //        promotion.DesignationId =(employee.DesignationId??0);
                    //        promotion.PromotionDate = employee.ConfirmationDate;
                    //        promotion.NextReviewDate = PromotionDate;
                    //        promotion.IsReviewed = false;
                    //        promotion.Remarks = "Initial data not promoted";
                    //        promotion.IsActive = true;
                    //        promotion.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    //        promotion.CreateDate = DateTime.UtcNow;

                    //        //let's add into [promo.EmployeePromotion]
                    //        employeePromotionService.Create(promotion);

                    //    }
                        
                    //    //employee.DesignationId = DesignationId;
                    //    //employeeService.Update(employee);
                    
                    //}

                    if(PromotionTypeId == 8)
                    {
                        var param = new { EmployeeId = EmployeeId, DesignationId = DesignationId };
                        var empList = employeeSPService.GetDataWithParameter(param, "promo.SP_Update_Designation_Promotion");
                    }

                    result = 1;                    
                    scope.Complete();
                    scope.Dispose();
                }
                catch (Exception ex)
                {
                    message = "Save Denied";
                    scope.Dispose();
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeePromotion(int PromotionId, long EmployeeId)
        {
            int result = 0;
            string message = "";
            using (TransactionScope scope = new TransactionScope()) // KHALID As two Table will be Update
            {
                try
                {
                    var param = new { PromotionId = PromotionId, EmployeeId = EmployeeId };
                    var employeeList = employeeSPService.GetDataWithParameter(param, "promo.SP_UpdateSalary");

                    message = "Promotion Information deleted succesfully";
                    scope.Complete();
                    scope.Dispose();

                }
                catch (Exception e)
                {
                    scope.Dispose();
                    result = 0;
                    message = "Failed to delete Promotion Information";
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeListForPromotion([DataSourceRequest]DataSourceRequest request, string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue, string status_Promotion)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                        sb.Append(" AND e.StatusId ='" + Status[0] + "'");
                }
                else if (Status != null && Status.Count > 1)
                {
                    string statusList = "";
                    var count = 1;
                    foreach (var status in Status)
                    {
                        if (count < Status.Count)
                        {
                            statusList = statusList + "'" + status + "', ";
                        }
                        else
                        {
                            statusList = statusList + "'" + status + "'";
                        }
                        count++;
                    }
                    sb.Append(" AND e.EmployeeStatusId in(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                {
                    sb.Append(" AND e.DesignationId =" + PayrollDesignation);
                }
                if (DepartmentId != "")
                {
                    sb.Append(" AND e.DepartmentId =" + DepartmentId);
                }
                if (Responsibility != "")
                {
                    sb.Append(" AND e.EmployeeRank =" + Responsibility);
                }

                if (Section != "")
                {
                    sb.Append(" AND e.SectionId =" + Section);
                }

                if (OfficeTypeId != "" && OfficeId == "")
                {
                    sb.Append(" AND e.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");
                }
                if (OfficeId != "")
                {
                    sb.Append(" AND e.OfficeId =" + OfficeId);
                }

                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND e.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND e.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND e.FirstJoiningDate ='" + FilterValue + "'");
                }
                else if (status_Promotion != "")
                {
                    if (status_Promotion == "ActivePromotion")
                    {
                        sb.Append(" AND ep.NextReviewDate is not NULL");
                    }
                    else if (status_Promotion == "InActivePromotion")
                    {
                        sb.Append(" AND ep.NextReviewDate is NULL");
                    }
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var employeeList = employeeSPService.GetDataWithParameter(param, "promo.SP_GetEmployeeListForPromotion");

                List_EmployeeViewModel = employeeList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeViewModel()
                {
                    SlNo = row.Field<string>("rowSl"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    DesignationName = row.Field<string>("DesignationName"),
                    OrnamentalDesignationName = row.Field<string>("OffcDesignName"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    EmployementTypeName = row.Field<string>("EmployementTypeName"),
                    EmployeeStatus = row.Field<string>("StatusName"),
                    NextReviewDate = row.Field<DateTime?>("NextReviewDate"),

                }).ToList();

                DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Methods

        private List<EmployeePromotionViewModel> GetPromotionInformation(string andCondition)
        {
            var param = new { AndCondition = andCondition };
            var List = employeeSPService.GetDataWithParameter(param, "promo.SP_GetPromotionInformation");
            var viewList = List.Tables[0].AsEnumerable()
                .Select((row, sl) => new EmployeePromotionViewModel
                {
                    RowSl = sl + 1,
                    EmployeeId              = row.Field<long>("EmployeeId"),
                    EmployeeName            = row.Field<string>("EmployeeName"),
                    PromotionDateMsg        = row.Field<string>("PromotionDateMsg"),
                    NextReviewDateMsg       = row.Field<string>("NextReviewDateMsg"),
                    DesignationId           = row.Field<int>("DesignationId"),
                    DesignationName         = row.Field<string>("DesignationName"),
                    PromotionId             = row.Field<long>("PromotionId"),
                    IsReviewed              = row.Field<int>("IsReviewed"),
                    IsReviewedString        = row.Field<string>("IsReviewedString"),
                    PromotionTypeId         = row.Field<int>("PromotionTypeId"),
                    PromotionTypeName       = row.Field<string>("PromotionTypeName"),

                    BasicSalary             = row.Field<decimal>("BasicSalary"),
                    HouseRent               = row.Field<decimal>("HouseRent"),
                    Medical                 = row.Field<decimal>("Medical"),
                    BonusAmount             = row.Field<decimal>("BonusAmount"),
                    GrossSalary             = row.Field<decimal>("GrossSalary"),

                }).ToList();

            return viewList;
        }

        public JsonResult UpdateSalary( string EmplyoeeID,
                                        string PromotionId, 
                                        string BasicSalary,
                                        string HouseRent  ,
                                        string Medical    ,
                                        string BonusAmount,
                                        string GrossAmount
            )
        {
            string result = string.Empty;
            try
            {
                if(PromotionId == "" || PromotionId =="0")
                    return Json("Warning! No Data Found to Update", JsonRequestBehavior.AllowGet);


                var param = new {
                                    @promotionId        = PromotionId    ,
                                    @EmployeeId         = EmplyoeeID     ,
                                    @BasicSalary        = BasicSalary    ,
                                    @HouseRent          = HouseRent      ,
                                    @Medical            = Medical        ,
                                    @Conveyance         = BonusAmount    ,
                                    @GrossAmount        = GrossAmount
                };
                var List = employeeSPService.GetDataWithParameter(param, "promo.UpdateSalaryConfiguration");
                


                result = "Salary Data Updated Successfully.";
            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion

    }// End Class
}// End NameSpace
