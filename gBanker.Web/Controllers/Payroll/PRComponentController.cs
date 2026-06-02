
#region Usings

using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using gHRM.Service.StoreProcedure;
using gHRM.Service.Payroll;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Web.CommonDropdown;
using gHRM.Data.CodeFirstMigration.Payroll;
using System.Data.Entity.Validation;
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Utilities;
using System.Transactions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;

#endregion

namespace gHRM.Web.Controllers
{
    public class PRComponentController : BaseController
    {
        #region  Private Variables

        private readonly IPRComponentService prComponentService;
       // private readonly IPRComponentService_designation prComponentServicedesignation;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IComponentPayrollService componentPayrollService;
        private readonly IPRSalaryConfigurationService pRSalaryConfigurationService;
        private readonly IView_EmployeeTypeWiseComponentConfigurationService empWiseComponentConfiguration;
        private readonly IView_PRComponentConfigurationService view_PRComponentConfigurationService;
        private readonly IPRComponentGroupService prComponentGroupService;
        private readonly IComponentPayrollService iComponentPayrollService;
        List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        #endregion

        #region  Ctor
        public PRComponentController(
            IPRComponentService prComponentService,            
            //IPRComponentService_designation prComponentServicedesignation,
            IEmployeeSPService employeeSPService,
            IComponentPayrollService componentPayrollService,
            IPRSalaryConfigurationService pRSalaryConfigurationService,
            IPRComponentGroupService prComponentGroupService,
            IView_EmployeeTypeWiseComponentConfigurationService empWiseComponentConfiguration,
            IView_PRComponentConfigurationService view_PRComponentConfigurationService,
            IComponentPayrollService iComponentPayrollService
           )
        {
            this.prComponentService = prComponentService;
            //this.prComponentServicedesignation = prComponentServicedesignation;
            this.employeeSPService = employeeSPService;
            this.componentPayrollService = componentPayrollService;
            this.pRSalaryConfigurationService = pRSalaryConfigurationService;
            this.empWiseComponentConfiguration = empWiseComponentConfiguration;
            this.view_PRComponentConfigurationService = view_PRComponentConfigurationService;
            this.prComponentGroupService = prComponentGroupService;            
            this.iComponentPayrollService = iComponentPayrollService;
            var List = employeeSPService.GetDataWithoutParameter("prl.SP_PR_GetAllComponentGroup");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new PRComponentViewModel
            {
                PRComponentGroupID = row.Field<int>("PRComponentGroupID"),
                ComponentGroupName = row.Field<string>("ComponentGroupName")

            }).ToList();

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Listing

        public ActionResult Index()
        {
            var model = new PRComponentViewModel();
            MapDropDownList(model);
            return View(model);
        }

        public ActionResult Index_designation()
        {
            var model = new PRComponentViewModel_designation();
            MapDropDownList_designation(model);
            return View(model);
        }


        #endregion

        #region Create

        public ActionResult Create(int? id)
        {
            var model = new PRComponentViewModel();

            if (!id.HasValue)
            {
                MapDropDownList(model);
                model.EffectiveStartDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");              
                // Account Code
                var acc = new gHRMDBContext().Database.SqlQuery<AccChart>("SELECT TOP(1)* FROM AccChart WHERE AccountCode='0000'");
                if (acc.Any())
                {
                    model.SalaryAccCode = acc.First().AccountCode;
                    model.AccountName = acc.First().AccountName;
                }
                return View(model);
            }

            var data = empWiseComponentConfiguration.GetAll().Where(p => p.PRComponentId == id).FirstOrDefault();
            var entity = Mapper.Map<View_EmployeeTypeWiseComponentConfiguration, PRComponentViewModel>(data);

            if (entity == null)
            {
                MapDropDownList(model);
                return View(model);
            }

            MapDropDownList(entity);

            entity.EffectiveStartDateMsg = entity.EffectiveStartDate.ToString("dd-MMM-yyyy");
            entity.EffectiveEndDateMsg = entity.EffectiveEndDate.ToString("dd-MMM-yyyy");
            var duration = entity.EffectiveEndDate.Year - entity.EffectiveStartDate.Year;

            if (duration == 0)
                entity.ValidateDurtion = Convert.ToString(Math.Abs((entity.EffectiveStartDate.Month - entity.EffectiveEndDate.Month) + 12 * (entity.EffectiveStartDate.Year - entity.EffectiveEndDate.Year)));
            else if (duration > 0)
                entity.ValidateDurtion = Convert.ToString(entity.EffectiveEndDate.Year - entity.EffectiveStartDate.Year);
            else
                entity.ValidateDurtion = "Other";

            model = entity;
           

            return View(model);
        }

        public ActionResult Create_designation(int? id)
        {
            var model = new PRComponentViewModel();

            if (!id.HasValue)
            {
                MapDropDownList(model);
                model.EffectiveStartDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                // Account Code
                var acc = new gHRMDBContext().Database.SqlQuery<AccChart>("SELECT TOP(1)* FROM AccChart WHERE AccountCode='0000'");
                if (acc.Any())
                {
                    model.SalaryAccCode = acc.First().AccountCode;
                    model.AccountName = acc.First().AccountName;
                }
                return View(model);
            }

            var data = empWiseComponentConfiguration.GetAll().Where(p => p.PRComponentId == id).FirstOrDefault();
            var entity = Mapper.Map<View_EmployeeTypeWiseComponentConfiguration, PRComponentViewModel>(data);

            if (entity == null)
            {
                MapDropDownList(model);
                return View(model);
            }

            MapDropDownList(entity);

            entity.EffectiveStartDateMsg = entity.EffectiveStartDate.ToString("dd-MMM-yyyy");
            entity.EffectiveEndDateMsg = entity.EffectiveEndDate.ToString("dd-MMM-yyyy");
            var duration = entity.EffectiveEndDate.Year - entity.EffectiveStartDate.Year;

            if (duration == 0)
                entity.ValidateDurtion = Convert.ToString(Math.Abs((entity.EffectiveStartDate.Month - entity.EffectiveEndDate.Month) + 12 * (entity.EffectiveStartDate.Year - entity.EffectiveEndDate.Year)));
            else if (duration > 0)
                entity.ValidateDurtion = Convert.ToString(entity.EffectiveEndDate.Year - entity.EffectiveStartDate.Year);
            else
                entity.ValidateDurtion = "Other";

            model = entity;


            return View(model);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int PRComponentID)
        {
            var model = new PRComponentViewModel();
            var componentEdit = view_PRComponentConfigurationService.GetAll().Where(p => p.IsActive == true && p.PRComponentID == PRComponentID).FirstOrDefault();
            if (componentEdit != null)
            {
                model.PRComponentID = componentEdit.PRComponentID;
                model.ComponentName = componentEdit.ComponentName;
                model.ComponentPayrollId = componentEdit.ComponentPayrollId;
                model.ComponentType = componentEdit.ComponentType;
                model.ComponentAmount = componentEdit.ComponentAmount;
                model.TransactionType = componentEdit.TransactionType;
                model.PRComponentGroupID = componentEdit.PRComponentGroupID;
                model.ComponentGroupName = componentEdit.ComponentGroupName;
                model.ComponentCategory = componentEdit.ComponentCategory;
                model.SalaryAccCode = componentEdit.SalaryAccCode;
                model.AccountName = componentEdit.AccountName;
                model.EffectiveStartDate = componentEdit.EffectiveStartDate;
                model.EffectiveEndDate = componentEdit.EffectiveEndDate;
                model.EffectiveStartDateMsg = componentEdit.EffectiveStartDate.ToString("dd-MMM-yyyy");
                model.EffectiveEndDateMsg = componentEdit.EffectiveEndDate.ToString("dd-MMM-yyyy");

                model.IsProductDependent = Convert.ToBoolean(componentEdit.IsProductDependent);
                model.MaximumLimit = Convert.ToDecimal(componentEdit.MaximumLimit);
                model.MinimumLimit = Convert.ToDecimal(componentEdit.MinimumLimit);
                model.EmployeeTypeId = componentEdit.EmployeeTypeId;
                model.EmployeeStatusId = componentEdit.EmployeeStatusId;
                model.RatioBasedOn = componentEdit.RatioBasedOn;
                model.SalaryEffect = componentEdit.SalaryEffect == true ? true : false;

                model.MinDuration = componentEdit.MinDuration;
                model.MaxDuration = componentEdit.MaxDuration;
                model.InterestRate = componentEdit.InterestRate;
                model.OfficeLocationId = componentEdit.OfficeLocationId;
                model.IsAdjustable = componentEdit.IsAdjustable;
                model.LoanCalculationId = componentEdit.LoanCalculationId;
                model.OfficeLocationName = componentEdit.OfficeLocationName;
                model.SalaryChangesByComponent = componentEdit.SalaryChangesByComponent;
                model.PRComponentGroupID = componentEdit.PRComponentGroupID;
                model.SalaryRoundType = componentEdit.SalaryRoundType;
                model.IsProvidentFundComponent = componentEdit.IsProvidentFundComponent;
                model.LoanCalculationId = componentEdit.LoanCalculationId;
                model.IsSalaryImpactProhibited = componentEdit.IsSalaryImpactProhibited;
                model.PFTypeId = componentEdit.PFTypeId;
            }

            MapDropDownList(model);
            return View(model);
        }


        public ActionResult Edit_designation(int PRComponentID)
        {
            var model = new PRComponentViewModel_designation();
            var componentEdit = view_PRComponentConfigurationService.GetAll().Where(p => p.IsActive == true && p.PRComponentID == PRComponentID).FirstOrDefault();
            if (componentEdit != null)
            {
                model.PRComponentID = componentEdit.PRComponentID;
                model.ComponentName = componentEdit.ComponentName;
                model.ComponentPayrollId = componentEdit.ComponentPayrollId;
                model.ComponentType = componentEdit.ComponentType;
                model.ComponentAmount = componentEdit.ComponentAmount;
                model.TransactionType = componentEdit.TransactionType;
                model.PRComponentGroupID = componentEdit.PRComponentGroupID;
                model.ComponentGroupName = componentEdit.ComponentGroupName;
                model.ComponentCategory = componentEdit.ComponentCategory;
                model.SalaryAccCode = componentEdit.SalaryAccCode;
                model.AccountName = componentEdit.AccountName;
                model.EffectiveStartDate = componentEdit.EffectiveStartDate;
                model.EffectiveEndDate = componentEdit.EffectiveEndDate;
                model.EffectiveStartDateMsg = componentEdit.EffectiveStartDate.ToString("dd-MMM-yyyy");
                model.EffectiveEndDateMsg = componentEdit.EffectiveEndDate.ToString("dd-MMM-yyyy");

                model.IsProductDependent = Convert.ToBoolean(componentEdit.IsProductDependent);
                model.MaximumLimit = Convert.ToDecimal(componentEdit.MaximumLimit);
                model.MinimumLimit = Convert.ToDecimal(componentEdit.MinimumLimit);
                model.EmployeeTypeId = componentEdit.EmployeeTypeId;
                model.EmployeeStatusId = componentEdit.EmployeeStatusId;
                model.RatioBasedOn = componentEdit.RatioBasedOn;
                model.SalaryEffect = componentEdit.SalaryEffect == true ? true : false;

                model.MinDuration = componentEdit.MinDuration;
                model.MaxDuration = componentEdit.MaxDuration;
                model.InterestRate = componentEdit.InterestRate;
                model.OfficeLocationId = componentEdit.OfficeLocationId;
                model.IsAdjustable = componentEdit.IsAdjustable;
                model.LoanCalculationId = componentEdit.LoanCalculationId;
                model.OfficeLocationName = componentEdit.OfficeLocationName;
                model.SalaryChangesByComponent = componentEdit.SalaryChangesByComponent;
                model.PRComponentGroupID = componentEdit.PRComponentGroupID;
                model.SalaryRoundType = componentEdit.SalaryRoundType;
                model.IsProvidentFundComponent = componentEdit.IsProvidentFundComponent;
                model.LoanCalculationId = componentEdit.LoanCalculationId;
                model.IsSalaryImpactProhibited = componentEdit.IsSalaryImpactProhibited;
                model.PFTypeId = componentEdit.PFTypeId;
            }

            MapDropDownList_designation(model);
            return View(model);
        }


        #endregion

        #region HTTPRequest        

        [HttpPost]
        public ActionResult Create(PRComponentViewModel model)
        {
            var result = string.Empty;
            var isOperationSuccess = true;

            if (model == null)
            {
                result = "You must fill all the required fields!";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            var entityMap = Mapper.Map<PRComponentViewModel, PRComponent>(model);
            
            var effectiveEndDateMsg = Convert.ToDateTime(model.EffectiveEndDateMsg);
            var endDateLastDate = new DateTime(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month, DateTime.DaysInMonth(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month));

            // Comment for Test:  Mizan
            //if (endDateLastDate > Convert.ToDateTime(model.EffectiveEndDateMsg))
            //{
            //    result = "End Date Should be last date of the month";
            //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            //}

            if (model.EffectiveEndDate < model.EffectiveStartDate)
            {
                return GetErrorMessageResult();
            }

            if (model.RatioBasedOn == null)
            {
                result = "Ratio Based on Information Required";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.IsProvidentFundComponent == true && model.PFTypeId == 0)
            {
                result = "Provident Fund Type Required";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.IsProvidentFundComponent == false && model.PFTypeId != 0)
            {
                result = "Please select provident fund integration required Yes";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.EmployeeStatusIdList == null)
            {
                result = "Please select at least one employee status";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.OffLocationList == null)
            {
                result = "Please select at least one office loaction";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            var maxLim = Convert.ToDecimal(model.MaximumLimit);
            var minLim = Convert.ToDecimal(model.MinimumLimit);
            var limit = maxLim - minLim;

            if (!(limit >= 0))
            {
                result = "Max and Min limit is not valid";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    foreach (var officelocation in model.OffLocationList)
                    {
                        foreach (var status in model.EmployeeStatusIdList)
                        {
                            var validationFor = $@"for Component: {model.ComponentName}";

                            var newPRComponent = PopulatePRComponent(model, entityMap, officelocation, status);

                            //update PR component info if exist
                            if (entityMap.PRComponentID > 0)
                            {
                                //check duplicate 
                                var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent(newPRComponent);

                                if (cheackDuplicateComonent)
                                {
                                    result = "This Component configuration already exist. Please try another!";
                                    isOperationSuccess = true;
                                    break;
                                }

                                var entityUpdate = prComponentService.GetById(entityMap.PRComponentID);
                                if (entityUpdate == null)
                                {
                                    result = $"Existing Component not found {validationFor} with Id {entityMap.PRComponentID}";
                                    isOperationSuccess = true;
                                    break;
                                }

                                //let's update PR Component
                                var response = UpdatePRComponent(entityUpdate, model, status, officelocation);
                                if (!response.IsSuccess)
                                {
                                    isOperationSuccess = false;
                                    result = response.Message;
                                    break;
                                }
                            }
                            else
                            {
                                //check duplicate 
                                var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent(newPRComponent);

                                if (cheackDuplicateComonent)
                                {
                                    result = "This Component configuration already exist. Please try another!";
                                    isOperationSuccess = true;
                                    break;
                                }

                                //let's add new PR Component
                                var response = SavePRComponent(model, status, officelocation);
                                if (!response.IsSuccess)
                                {
                                    isOperationSuccess = false;
                                    result = response.Message;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    result = exceptionMessage;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                {
                    result = "Success, Component configured!";
                    ts.Complete();
                }

                ts.Dispose();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create_designation(PRComponentViewModel model)
        {
            var result = string.Empty;
            var isOperationSuccess = true;

            if (model == null)
            {
                result = "You must fill all the required fields!";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            var entityMap = Mapper.Map<PRComponentViewModel, PRComponent>(model);

            var effectiveEndDateMsg = Convert.ToDateTime(model.EffectiveEndDateMsg);
            var endDateLastDate = new DateTime(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month, DateTime.DaysInMonth(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month));

            // Comment for Test:  Mizan
            //if (endDateLastDate > Convert.ToDateTime(model.EffectiveEndDateMsg))
            //{
            //    result = "End Date Should be last date of the month";
            //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            //}

            if (model.EffectiveEndDate < model.EffectiveStartDate)
            {
                return GetErrorMessageResult();
            }

            if (model.RatioBasedOn == null)
            {
                result = "Ratio Based on Information Required";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.IsProvidentFundComponent == true && model.PFTypeId == 0)
            {
                result = "Provident Fund Type Required";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.IsProvidentFundComponent == false && model.PFTypeId != 0)
            {
                result = "Please select provident fund integration required Yes";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.EmployeeStatusIdList == null)
            {
                result = "Please select at least one employee status";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            if (model.OffLocationList == null)
            {
                result = "Please select at least one office loaction";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            var maxLim = Convert.ToDecimal(model.MaximumLimit);
            var minLim = Convert.ToDecimal(model.MinimumLimit);
            var limit = maxLim - minLim;

            if (!(limit >= 0))
            {
                result = "Max and Min limit is not valid";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    foreach (var officelocation in model.OffLocationList)
                    {
                        foreach (var status in model.EmployeeStatusIdList)
                        {
                            foreach (var desg in model.EmpDesignationIdList)
                            {

                                var validationFor = $@"for Component: {model.ComponentName}";

                                var newPRComponent = PopulatePRComponent_desig(model, entityMap, officelocation, status, desg);

                                //update PR component info if exist
                                if (entityMap.PRComponentID > 0)
                                {
                                    //check duplicate 
                                    var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent_designation(newPRComponent);

                                    if (cheackDuplicateComonent)
                                    {
                                        result = "This Component configuration already exist. Please try another!";
                                        isOperationSuccess = true;
                                        break;
                                    }

                                    var entityUpdate = prComponentService.GetById(entityMap.PRComponentID);
                                    if (entityUpdate == null)
                                    {
                                        result = $"Existing Component not found {validationFor} with Id {entityMap.PRComponentID}";
                                        isOperationSuccess = true;
                                        break;
                                    }

                                    //let's update PR Component
                                    var response = UpdatePRComponent_designation(entityUpdate, model, status, officelocation);
                                    if (!response.IsSuccess)
                                    {
                                        isOperationSuccess = false;
                                        result = response.Message;
                                        break;
                                    }
                                }
                                else
                                {
                                    //check duplicate 
                                    var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent_designation(newPRComponent);

                                    if (cheackDuplicateComonent)
                                    {
                                        result = "This Component configuration already exist. Please try another!";
                                        isOperationSuccess = true;
                                        break;
                                    }

                                    //let's add new PR Component
                                    var response = SavePRComponent_designation(model, status, officelocation, desg);
                                    if (!response.IsSuccess)
                                    {
                                        isOperationSuccess = false;
                                        result = response.Message;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    result = exceptionMessage;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                {
                    result = "Success, Component configured!";
                    ts.Complete();
                }

                ts.Dispose();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdatePRComponent(PRComponentViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var endDateGeneration = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1);
                DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1).AddMonths(1);
                var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

                if (endDateLastDate > Convert.ToDateTime(obj.EffectiveEndDateMsg))
                    return GetErrorMessageResult("End Date Should be last date of the month");

                if (Convert.ToDateTime(obj.EffectiveEndDateMsg) < Convert.ToDateTime(obj.EffectiveStartDateMsg))

                    return GetErrorMessageResult("Please check effective start and end date");

                if (obj.TransactionType == "")
                    return GetErrorMessageResult("Please Check Transaction Type");

                if (obj.ComponentGroupName == "")
                    return GetErrorMessageResult("Please Check Component Group");

                if (obj.ComponentType == "")
                    return GetErrorMessageResult("Please Check Component Type");

                if (obj.EmployeeTypeId == 0)
                    return GetErrorMessageResult("Please Check Employee Type");

                if (obj.EmployeeStatusId == 0)
                    return GetErrorMessageResult("Please Check Employee Status");

                if (obj.IsProductDependent == null)
                    return GetErrorMessageResult("Please Check Product Dependecny");

                if (obj.IsProvidentFundComponent == null)
                    return GetErrorMessageResult("Please Check Provident Fund Integration Required");

                if (obj.OfficeLocationId == 0)
                    return GetErrorMessageResult("Please Check Office Location");

                if (obj.IsAdjustable == null)
                    return GetErrorMessageResult("Please Check Loan Configuration Changable");

                if (obj.LoanCalculationId == null)
                    return GetErrorMessageResult("Please Check Loan Calculation Type");

                var validatePRComponent = new  PRComponent
                {
                    ComponentName = obj.ComponentName,
                    EmployeeStatusId = obj.EmployeeStatusId,
                    EmployeeTypeId = obj.EmployeeTypeId,
                    OfficeLocationId = obj.OfficeLocationId,
                    IsProvidentFundComponent = obj.IsProvidentFundComponent,
                    PFTypeId = obj.PFTypeId,
                    PRComponentID = obj.PRComponentID
                };

                //check duplicate item
                var cheackDuplicate = prComponentService.CheckDuplicateComponent(validatePRComponent);
                if (cheackDuplicate)
                {
                    message = "Already this component is configured, Update Denied";
                    result = 0;                   
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }                

                var updatePrComponent = prComponentService.GetById(obj.PRComponentID);
                if (updatePrComponent == null)
                {
                    result = 0;
                    message = "Component not found";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }

                //let's insert history
                InsertComponentHistory(updatePrComponent);

                updatePrComponent.ComponentAmount = obj.ComponentAmount;
                updatePrComponent.ComponentType = obj.ComponentType;
                updatePrComponent.RatioBasedOn = obj.RatioBasedOn;
                updatePrComponent.TransactionType = obj.TransactionType;
                updatePrComponent.PRComponentGroupID = obj.PRComponentGroupID;
                updatePrComponent.EffectiveStartDate = Convert.ToDateTime(obj.EffectiveStartDateMsg);
                updatePrComponent.EffectiveEndDate = Convert.ToDateTime(obj.EffectiveEndDateMsg);
                updatePrComponent.EmployeeTypeId = obj.EmployeeTypeId;
                updatePrComponent.EmployeeStatusId = obj.EmployeeStatusId;
                updatePrComponent.OfficeLocationId = obj.OfficeLocationId;
                updatePrComponent.SalaryRoundType = obj.SalaryRoundType;
                updatePrComponent.SalaryChangesByComponent = obj.SalaryChangesByComponent;
                updatePrComponent.SalaryEffect = (obj.SalaryChangesByComponent == "N/A") ? false : true;

                updatePrComponent.IsSalaryImpactProhibited = obj.IsSalaryImpactProhibited;
                updatePrComponent.IsProvidentFundComponent = obj.IsProvidentFundComponent;
                updatePrComponent.IsProductDependent = obj.IsProductDependent;
                updatePrComponent.MaximumLimit = obj.MaximumLimit;
                updatePrComponent.MinimumLimit = obj.MinimumLimit;
                updatePrComponent.SalaryAccCode = obj.SalaryAccCode;
                updatePrComponent.LoanCalculationId = obj.LoanCalculationId;

                updatePrComponent.InterestRate = Convert.ToDecimal(obj.InterestRate);
                updatePrComponent.MinDuration = obj.MinDuration;
                updatePrComponent.MaxDuration = obj.MaxDuration;
                updatePrComponent.IsAdjustable = obj.IsAdjustable;

                updatePrComponent.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                prComponentService.Update(updatePrComponent);

                result = 1;
                message = "Component Updated Successfully";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                result = 0;
                message = "Error";
            }

            return Json(new { result = result, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePRComponent_designation(PRComponent entityUpdate, PRComponentViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var endDateGeneration = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1);
                DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1).AddMonths(1);
                var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

                if (endDateLastDate > Convert.ToDateTime(obj.EffectiveEndDateMsg))
                    return GetErrorMessageResult("End Date Should be last date of the month");

                if (Convert.ToDateTime(obj.EffectiveEndDateMsg) < Convert.ToDateTime(obj.EffectiveStartDateMsg))

                    return GetErrorMessageResult("Please check effective start and end date");

                if (obj.TransactionType == "")
                    return GetErrorMessageResult("Please Check Transaction Type");

                if (obj.ComponentGroupName == "")
                    return GetErrorMessageResult("Please Check Component Group");

                if (obj.ComponentType == "")
                    return GetErrorMessageResult("Please Check Component Type");

                if (obj.EmployeeTypeId == 0)
                    return GetErrorMessageResult("Please Check Employee Type");

                if (obj.EmployeeStatusId == 0)
                    return GetErrorMessageResult("Please Check Employee Status");

                if (obj.IsProductDependent == null)
                    return GetErrorMessageResult("Please Check Product Dependecny");

                if (obj.IsProvidentFundComponent == null)
                    return GetErrorMessageResult("Please Check Provident Fund Integration Required");

                if (obj.OfficeLocationId == 0)
                    return GetErrorMessageResult("Please Check Office Location");

                if (obj.IsAdjustable == null)
                    return GetErrorMessageResult("Please Check Loan Configuration Changable");

                if (obj.LoanCalculationId == null)
                    return GetErrorMessageResult("Please Check Loan Calculation Type");

                var validatePRComponent = new PRComponent
                {
                    ComponentName = obj.ComponentName,
                    EmployeeStatusId = obj.EmployeeStatusId,
                    EmployeeTypeId = obj.EmployeeTypeId,
                    OfficeLocationId = obj.OfficeLocationId,
                    IsProvidentFundComponent = obj.IsProvidentFundComponent,
                    PFTypeId = obj.PFTypeId,
                    PRComponentID = obj.PRComponentID,
                    DesignationId = obj.DesignationId,
                };

                //check duplicate item
                var cheackDuplicate = prComponentService.CheckDuplicateComponent_designation(validatePRComponent);
                if (cheackDuplicate)
                {
                    message = "Already this component is configured, Update Denied";
                    result = 0;
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }

                var updatePrComponent = prComponentService.GetById(obj.PRComponentID);
                if (updatePrComponent == null)
                {
                    result = 0;
                    message = "Component not found";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }

                //let's insert history
                InsertComponentHistory(updatePrComponent);

                updatePrComponent.ComponentAmount = obj.ComponentAmount;
                updatePrComponent.ComponentType = obj.ComponentType;
                updatePrComponent.RatioBasedOn = obj.RatioBasedOn;
                updatePrComponent.TransactionType = obj.TransactionType;
                updatePrComponent.PRComponentGroupID = obj.PRComponentGroupID;
                updatePrComponent.EffectiveStartDate = Convert.ToDateTime(obj.EffectiveStartDateMsg);
                updatePrComponent.EffectiveEndDate = Convert.ToDateTime(obj.EffectiveEndDateMsg);
                updatePrComponent.EmployeeTypeId = obj.EmployeeTypeId;
                updatePrComponent.EmployeeStatusId = obj.EmployeeStatusId;
                updatePrComponent.OfficeLocationId = obj.OfficeLocationId;
                updatePrComponent.SalaryRoundType = obj.SalaryRoundType;
                updatePrComponent.SalaryChangesByComponent = obj.SalaryChangesByComponent;
                updatePrComponent.SalaryEffect = (obj.SalaryChangesByComponent == "N/A") ? false : true;

                updatePrComponent.IsSalaryImpactProhibited = obj.IsSalaryImpactProhibited;
                updatePrComponent.IsProvidentFundComponent = obj.IsProvidentFundComponent;
                updatePrComponent.IsProductDependent = obj.IsProductDependent;
                updatePrComponent.MaximumLimit = obj.MaximumLimit;
                updatePrComponent.MinimumLimit = obj.MinimumLimit;
                updatePrComponent.SalaryAccCode = obj.SalaryAccCode;
                updatePrComponent.LoanCalculationId = obj.LoanCalculationId;

                updatePrComponent.InterestRate = Convert.ToDecimal(obj.InterestRate);
                updatePrComponent.MinDuration = obj.MinDuration;
                updatePrComponent.MaxDuration = obj.MaxDuration;
                updatePrComponent.IsAdjustable = obj.IsAdjustable;

                updatePrComponent.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                updatePrComponent.DesignationId = obj.DesignationId;
                prComponentService.Update(updatePrComponent);

                result = 1;
                message = "Component Updated Successfully";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                result = 0;
                message = "Error";
            }

            return Json(new { result = result, Message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Edit(PRComponentViewModel model)
        {
            string result = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Result = "Error", Message = "You must fill all the required fields!" }, JsonRequestBehavior.AllowGet);

                if (model.EffectiveEndDate < model.EffectiveStartDate)
                    return GetErrorMessageResult();

                var entity = Mapper.Map<PRComponentViewModel, PRComponent>(model);
                var updatePRComponent = prComponentService.GetById(Convert.ToInt32(entity.PRComponentID));
                if (updatePRComponent == null)
                {
                    result = "This component not found, Update Denied";
                    return Json(new { Result = "Error", Message = result }, JsonRequestBehavior.AllowGet);
                }

                var validatePRComponent = PopulateToValidatePRComponent(model, entity);
                //check duplicate item
                var cheackDuplicate = prComponentService.CheckDuplicateComponent(validatePRComponent);
                if (cheackDuplicate)
                {
                    result = "Already this component is configured, Update Denied";
                    return Json(new { Result = "Error", Message = result }, JsonRequestBehavior.AllowGet);
                }

                //populate to update prcomponent
                PopulateToUpdatePRComponent(model, updatePRComponent);

                //let's update prcomponent
                prComponentService.Update(updatePRComponent);

                result = "Component Update Successfull";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = ex.InnerException.InnerException.ToString();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        //public ActionResult Create_designation(PRComponentViewModel model)
        //{
        //    var result = string.Empty;
        //    var isOperationSuccess = true;

        //    if (model == null)
        //    {
        //        result = "You must fill all the required fields!";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    var entityMap = Mapper.Map<PRComponentViewModel, PRComponent>(model);

        //    var effectiveEndDateMsg = Convert.ToDateTime(model.EffectiveEndDateMsg);
        //    var endDateLastDate = new DateTime(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month, DateTime.DaysInMonth(effectiveEndDateMsg.Year, effectiveEndDateMsg.Month));

        //    // Comment for Test:  Mizan
        //    //if (endDateLastDate > Convert.ToDateTime(model.EffectiveEndDateMsg))
        //    //{
        //    //    result = "End Date Should be last date of the month";
        //    //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    //}

        //    if (model.EffectiveEndDate < model.EffectiveStartDate)
        //    {
        //        return GetErrorMessageResult();
        //    }

        //    if (model.RatioBasedOn == null)
        //    {
        //        result = "Ratio Based on Information Required";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.IsProvidentFundComponent == true && model.PFTypeId == 0)
        //    {
        //        result = "Provident Fund Type Required";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.IsProvidentFundComponent == false && model.PFTypeId != 0)
        //    {
        //        result = "Please select provident fund integration required Yes";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.EmployeeStatusIdList == null)
        //    {
        //        result = "Please select at least one employee status";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.OffLocationList == null)
        //    {
        //        result = "Please select at least one office loaction";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    var maxLim = Convert.ToDecimal(model.MaximumLimit);
        //    var minLim = Convert.ToDecimal(model.MinimumLimit);
        //    var limit = maxLim - minLim;

        //    if (!(limit >= 0))
        //    {
        //        result = "Max and Min limit is not valid";
        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //    using (var ts = new TransactionScope())
        //    {
        //        try
        //        {
        //            foreach (var officelocation in model.OffLocationList)
        //            {
        //                foreach (var status in model.EmployeeStatusIdList)
        //                {
        //                    var validationFor = $@"for Component: {model.ComponentName}";

        //                    var newPRComponent = PopulatePRComponent(model, entityMap, officelocation, status);

        //                    //update PR component info if exist
        //                    if (entityMap.PRComponentID > 0)
        //                    {
        //                        //check duplicate 
        //                        var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent(newPRComponent);

        //                        if (cheackDuplicateComonent)
        //                        {
        //                            result = "This Component configuration already exist. Please try another!";
        //                            isOperationSuccess = true;
        //                            break;
        //                        }

        //                        var entityUpdate = prComponentService.GetById(entityMap.PRComponentID);
        //                        if (entityUpdate == null)
        //                        {
        //                            result = $"Existing Component not found {validationFor} with Id {entityMap.PRComponentID}";
        //                            isOperationSuccess = true;
        //                            break;
        //                        }

        //                        //let's update PR Component
        //                        var response = UpdatePRComponent(entityUpdate, model, status, officelocation);
        //                        if (!response.IsSuccess)
        //                        {
        //                            isOperationSuccess = false;
        //                            result = response.Message;
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //check duplicate 
        //                        var cheackDuplicateComonent = prComponentService.CheckDuplicateComponent(newPRComponent);

        //                        if (cheackDuplicateComonent)
        //                        {
        //                            result = "This Component configuration already exist. Please try another!";
        //                            isOperationSuccess = true;
        //                            break;
        //                        }

        //                        //let's add new PR Component
        //                        var response = SavePRComponent(model, status, officelocation);
        //                        if (!response.IsSuccess)
        //                        {
        //                            isOperationSuccess = false;
        //                            result = response.Message;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            var errorMessages = ex.EntityValidationErrors
        //                    .SelectMany(x => x.ValidationErrors)
        //                    .Select(x => x.ErrorMessage);

        //            var fullErrorMessage = string.Join("; ", errorMessages);
        //            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //            result = exceptionMessage;
        //            isOperationSuccess = false;
        //        }

        //        if (isOperationSuccess)
        //        {
        //            result = "Success, Component configured!";
        //            ts.Complete();
        //        }

        //        ts.Dispose();
        //    }

        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult UpdatePRComponent_designation(PRComponent entityUpdate, PRComponentViewModel obj)
        //{
        //    var result = 0;
        //    var message = "";

        //    try
        //    {
        //        var endDateGeneration = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1);
        //        DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(obj.EffectiveEndDateMsg).Year, Convert.ToDateTime(obj.EffectiveEndDateMsg).Month, 1).AddMonths(1);
        //        var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

        //        if (endDateLastDate > Convert.ToDateTime(obj.EffectiveEndDateMsg))
        //            return GetErrorMessageResult("End Date Should be last date of the month");

        //        if (Convert.ToDateTime(obj.EffectiveEndDateMsg) < Convert.ToDateTime(obj.EffectiveStartDateMsg))

        //            return GetErrorMessageResult("Please check effective start and end date");

        //        if (obj.TransactionType == "")
        //            return GetErrorMessageResult("Please Check Transaction Type");

        //        if (obj.ComponentGroupName == "")
        //            return GetErrorMessageResult("Please Check Component Group");

        //        if (obj.ComponentType == "")
        //            return GetErrorMessageResult("Please Check Component Type");

        //        if (obj.EmployeeTypeId == 0)
        //            return GetErrorMessageResult("Please Check Employee Type");

        //        if (obj.EmployeeStatusId == 0)
        //            return GetErrorMessageResult("Please Check Employee Status");

        //        if (obj.IsProductDependent == null)
        //            return GetErrorMessageResult("Please Check Product Dependecny");

        //        if (obj.IsProvidentFundComponent == null)
        //            return GetErrorMessageResult("Please Check Provident Fund Integration Required");

        //        if (obj.OfficeLocationId == 0)
        //            return GetErrorMessageResult("Please Check Office Location");

        //        if (obj.IsAdjustable == null)
        //            return GetErrorMessageResult("Please Check Loan Configuration Changable");

        //        if (obj.LoanCalculationId == null)
        //            return GetErrorMessageResult("Please Check Loan Calculation Type");

        //        var validatePRComponent = new PRComponent
        //        {
        //            ComponentName = obj.ComponentName,
        //            EmployeeStatusId = obj.EmployeeStatusId,
        //            EmployeeTypeId = obj.EmployeeTypeId,
        //            OfficeLocationId = obj.OfficeLocationId,
        //            IsProvidentFundComponent = obj.IsProvidentFundComponent,
        //            PFTypeId = obj.PFTypeId,
        //            PRComponentID = obj.PRComponentID
        //        };

        //        //check duplicate item
        //        var cheackDuplicate = prComponentService.CheckDuplicateComponent(validatePRComponent);
        //        if (cheackDuplicate)
        //        {
        //            message = "Already this component is configured, Update Denied";
        //            result = 0;
        //            return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
        //        }

        //        var updatePrComponent = prComponentService.GetById(obj.PRComponentID);
        //        if (updatePrComponent == null)
        //        {
        //            result = 0;
        //            message = "Component not found";
        //            return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
        //        }

        //        //let's insert history
        //        InsertComponentHistory(updatePrComponent);

        //        updatePrComponent.ComponentAmount = obj.ComponentAmount;
        //        updatePrComponent.ComponentType = obj.ComponentType;
        //        updatePrComponent.RatioBasedOn = obj.RatioBasedOn;
        //        updatePrComponent.TransactionType = obj.TransactionType;
        //        updatePrComponent.PRComponentGroupID = obj.PRComponentGroupID;
        //        updatePrComponent.EffectiveStartDate = Convert.ToDateTime(obj.EffectiveStartDateMsg);
        //        updatePrComponent.EffectiveEndDate = Convert.ToDateTime(obj.EffectiveEndDateMsg);
        //        updatePrComponent.EmployeeTypeId = obj.EmployeeTypeId;
        //        updatePrComponent.EmployeeStatusId = obj.EmployeeStatusId;
        //        updatePrComponent.OfficeLocationId = obj.OfficeLocationId;
        //        updatePrComponent.SalaryRoundType = obj.SalaryRoundType;
        //        updatePrComponent.SalaryChangesByComponent = obj.SalaryChangesByComponent;
        //        updatePrComponent.SalaryEffect = (obj.SalaryChangesByComponent == "N/A") ? false : true;

        //        updatePrComponent.IsSalaryImpactProhibited = obj.IsSalaryImpactProhibited;
        //        updatePrComponent.IsProvidentFundComponent = obj.IsProvidentFundComponent;
        //        updatePrComponent.IsProductDependent = obj.IsProductDependent;
        //        updatePrComponent.MaximumLimit = obj.MaximumLimit;
        //        updatePrComponent.MinimumLimit = obj.MinimumLimit;
        //        updatePrComponent.SalaryAccCode = obj.SalaryAccCode;
        //        updatePrComponent.LoanCalculationId = obj.LoanCalculationId;

        //        updatePrComponent.InterestRate = Convert.ToDecimal(obj.InterestRate);
        //        updatePrComponent.MinDuration = obj.MinDuration;
        //        updatePrComponent.MaxDuration = obj.MaxDuration;
        //        updatePrComponent.IsAdjustable = obj.IsAdjustable;

        //        updatePrComponent.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
        //        prComponentService.Update(updatePrComponent);

        //        result = 1;
        //        message = "Component Updated Successfully";
        //    }
        //    catch (Exception e)
        //    {
        //        string msg = e.Message;
        //        result = 0;
        //        message = "Error";
        //    }

        //    return Json(new { result = result, Message = message }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult Edit_designation(PRComponentViewModel_designation model)
        {
            string result = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Result = "Error", Message = "You must fill all the required fields!" }, JsonRequestBehavior.AllowGet);

                if (model.EffectiveEndDate < model.EffectiveStartDate)
                    return GetErrorMessageResult();

                var entity = Mapper.Map<PRComponentViewModel_designation, PRComponent>(model);
                var updatePRComponent = prComponentService.GetById(Convert.ToInt32(entity.PRComponentID));
                if (updatePRComponent == null)
                {
                    result = "This component not found, Update Denied";
                    return Json(new { Result = "Error", Message = result }, JsonRequestBehavior.AllowGet);
                }

                var validatePRComponent = PopulateToValidatePRComponent_designation(model, entity);
                //check duplicate item
                var cheackDuplicate = prComponentService.CheckDuplicateComponent(validatePRComponent);
                if (cheackDuplicate)
                {
                    result = "Already this component is configured, Update Denied";
                    return Json(new { Result = "Error", Message = result }, JsonRequestBehavior.AllowGet);
                }

                //populate to update prcomponent
                PopulateToUpdatePRComponent_designation(model, updatePRComponent);

                //let's update prcomponent
                prComponentService.Update(updatePRComponent);

                result = "Component Update Successfull";
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = ex.InnerException.InnerException.ToString();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult AutoCompleteGroupName(string term, string comCategory)
        {

            var result = (from r in List_ViewModel
                          where r.ComponentGroupName.ToLower().Contains(term.ToLower())
                          select new { r.ComponentGroupName, r.PRComponentGroupID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetComponentList_designation([DataSourceRequest] DataSourceRequest request,
    long? employeeTypeId, int? employeeStatusId, int designationId)
        {
            try
            {
                var filter = new PRComponentSearchFilter_designation
                {
                    EmployeeTypeId = employeeTypeId,
                    EmployeeStatusId = employeeStatusId,
                    DesignationId = designationId,
                };

                var prComponentList = employeeSPService.GetListingByFilter_designation(filter);

                DataSourceResult result = prComponentList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetComponentList([DataSourceRequest]DataSourceRequest request,
            long? employeeTypeId, int? employeeStatusId)
        {
            try
            { 
                var filter = new PRComponentSearchFilter
                {
                    EmployeeTypeId= employeeTypeId,
                    EmployeeStatusId= employeeStatusId
                };

                var prComponentList = employeeSPService.GetListingByFilter(filter);

                DataSourceResult result = prComponentList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult Delete(string Id)
        {
            var entity = prComponentService.GetById(Convert.ToInt32(Id.Trim()));
            string Result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (Id != null)
                    {
                        var prComponentId = Convert.ToInt32(Id);
                        var checkExistance = pRSalaryConfigurationService.GetMany(p => p.PRComponentID == prComponentId && p.IsActive == true).ToList();
                        if (checkExistance.Any())
                        {
                            Result = "This component is currently configured for salary generation, Delete Denied";
                        }
                        else
                        {
                            entity.IsActive = false;
                            entity.InActiveDate = DateTime.Now;
                            entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                            entity.UpdateDate = DateTime.Now;
                            prComponentService.Update(entity);
                            Result = "Deleted Successfully";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                throw;
            }

            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAccountData(string AccCode)
        {
            List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
            var param = new { AccCode = AccCode };
            var empList = employeeSPService.GetDataWithParameter(param, "SP_PR_Get_AccData");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new PRComponentViewModel
               {
                   SalaryAccCode = row.Field<string>("AccountCode"),
                   AccountName = row.Field<string>("AccountName")

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPRComponentList()
        {
            List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
            var param = new { AndCondition = "" };
            var List = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_Component_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new PRComponentViewModel
            {
                PRComponentID = row.Field<int>("PRComponentID"),
                ComponentName = row.Field<string>("ComponentName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PRComponentID.ToString(),
                Text = string.Format("{0} - {1}", x.ComponentName, x.PRComponentID)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetPRComponentListLoan()
        {
            List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
            var param = new { AndCondition = "" };
            var List = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_Component_ListLoan");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new PRComponentViewModel
            {
                PRComponentID = row.Field<int>("PRComponentID"),
                ComponentName = row.Field<string>("ComponentName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PRComponentID.ToString(),
                Text = string.Format("{0} - {1}", x.ComponentName, x.PRComponentID)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComponentNameWiseCategory(int componentId)
        {
            var component = iComponentPayrollService.GetById(componentId);
            // var ComponentCategoryList = component.Select(p => p.ComponentCategory).FirstOrDefault();
            return Json(component.ComponentCategory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComponentNamebyCategory(string categoryName)
        {
            var component = commonDynamicDropDown.PayrollComponentName(categoryName);
            return Json(component, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetInterestRateForLoan(int PurposeId,int empid)
        {
            decimal interestRate = 0;
            gHRMDBContext db = new gHRMDBContext();
            var obj = (from e in db.Employees
                       join o in db.Offices on e.OfficeId equals o.OfficeId
                       where e.EmployeeId == empid
                       select new { e.EmployeeStatusId, o.OfficeLocationId }).ToList();
            if (obj.Any())
            {
                int employeeStatusId = obj[0].EmployeeStatusId, officeLocationId= obj[0].OfficeLocationId??0;
                var component = prComponentService.GetMany(x => x.ComponentCategory == "Loan" && x.ComponentPayrollId == PurposeId && (x.EmployeeStatusId ?? employeeStatusId) == employeeStatusId && x.OfficeLocationId == officeLocationId);
                if (component.Any())
                    interestRate = component.First().InterestRate;
            }
            
            return Json(interestRate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetComponentByStatusAndType(int? statusid,int? typeid)
        {
            List<SelectListItem> lstObj = commonDynamicDropDown.CommoninitialOption().ToList();
            var lst = prComponentService.GetMany(x => x.EmployeeStatusId == statusid && x.EmployeeTypeId == typeid).ToList()
                .Select(s => new SelectListItem { Text = s.ComponentName, Value = s.PRComponentID.ToString() });
            lstObj.AddRange(lst);
            return Json(lstObj,JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Private Methods

        private void PopulateToUpdatePRComponent_designation(PRComponentViewModel_designation model, PRComponent updatePRComponent)
        {
            updatePRComponent.ComponentAmount = model.ComponentAmount;
            updatePRComponent.ComponentType = model.ComponentType;
            updatePRComponent.RatioBasedOn = model.RatioBasedOn;
            updatePRComponent.TransactionType = model.TransactionType;
            updatePRComponent.PRComponentGroupID = model.PRComponentGroupID;
            updatePRComponent.EffectiveStartDate = model.EffectiveStartDate;
            updatePRComponent.EffectiveEndDate = model.EffectiveEndDate;
            updatePRComponent.EmployeeTypeId = model.EmployeeTypeId;
            updatePRComponent.EmployeeStatusId = model.EmployeeStatusId;
            updatePRComponent.OfficeLocationId = model.OfficeLocationId;
            updatePRComponent.SalaryRoundType = model.SalaryRoundType;
            updatePRComponent.SalaryChangesByComponent = model.SalaryChangesByComponent;

            if (model.SalaryChangesByComponent == "N/A")
                updatePRComponent.SalaryEffect = false;
            else
                updatePRComponent.SalaryEffect = true;

            updatePRComponent.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
            updatePRComponent.IsProvidentFundComponent = model.IsProvidentFundComponent;
            updatePRComponent.IsProductDependent = model.IsProductDependent;
            updatePRComponent.MaximumLimit = model.MaximumLimit;
            updatePRComponent.MinimumLimit = model.MinimumLimit;
            updatePRComponent.SalaryAccCode = model.SalaryAccCode;
            updatePRComponent.LoanCalculationId = model.LoanCalculationId;

            updatePRComponent.InterestRate = Convert.ToDecimal(model.InterestRate);
            updatePRComponent.MinDuration = model.MinDuration;
            updatePRComponent.MaxDuration = model.MaxDuration;
            updatePRComponent.IsAdjustable = model.IsAdjustable;

            updatePRComponent.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            updatePRComponent.UpdateDate = DateTime.UtcNow;
        }

        private void PopulateToUpdatePRComponent(PRComponentViewModel model, PRComponent updatePRComponent)
        {
            updatePRComponent.ComponentAmount = model.ComponentAmount;
            updatePRComponent.ComponentType = model.ComponentType;
            updatePRComponent.RatioBasedOn = model.RatioBasedOn;
            updatePRComponent.TransactionType = model.TransactionType;
            updatePRComponent.PRComponentGroupID = model.PRComponentGroupID;
            updatePRComponent.EffectiveStartDate = model.EffectiveStartDate;
            updatePRComponent.EffectiveEndDate = model.EffectiveEndDate;
            updatePRComponent.EmployeeTypeId = model.EmployeeTypeId;
            updatePRComponent.EmployeeStatusId = model.EmployeeStatusId;
            updatePRComponent.OfficeLocationId = model.OfficeLocationId;
            updatePRComponent.SalaryRoundType = model.SalaryRoundType;
            updatePRComponent.SalaryChangesByComponent = model.SalaryChangesByComponent;

            if (model.SalaryChangesByComponent == "N/A")
                updatePRComponent.SalaryEffect = false;
            else
                updatePRComponent.SalaryEffect = true;

            updatePRComponent.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
            updatePRComponent.IsProvidentFundComponent = model.IsProvidentFundComponent;
            updatePRComponent.IsProductDependent = model.IsProductDependent;
            updatePRComponent.MaximumLimit = model.MaximumLimit;
            updatePRComponent.MinimumLimit = model.MinimumLimit;
            updatePRComponent.SalaryAccCode = model.SalaryAccCode;
            updatePRComponent.LoanCalculationId = model.LoanCalculationId;

            updatePRComponent.InterestRate = Convert.ToDecimal(model.InterestRate);
            updatePRComponent.MinDuration = model.MinDuration;
            updatePRComponent.MaxDuration = model.MaxDuration;
            updatePRComponent.IsAdjustable = model.IsAdjustable;

            updatePRComponent.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            updatePRComponent.UpdateDate = DateTime.UtcNow;
        }

        private PRComponent PopulateToValidatePRComponent_designation(PRComponentViewModel_designation model, PRComponent entity)
        {
            return new PRComponent
            {
                ComponentName = model.ComponentName,
                EmployeeStatusId = model.EmployeeStatusId,
                EmployeeTypeId = entity.EmployeeTypeId,
                OfficeLocationId = entity.OfficeLocationId,
                IsProvidentFundComponent = model.IsProvidentFundComponent,
                PFTypeId = model.PFTypeId,
                PRComponentID = model.PRComponentID
            };
        }

        private PRComponent PopulateToValidatePRComponent(PRComponentViewModel model, PRComponent entity)
        {
            return new PRComponent
            {
                ComponentName = model.ComponentName,
                EmployeeStatusId = model.EmployeeStatusId,
                EmployeeTypeId = entity.EmployeeTypeId,
                OfficeLocationId = entity.OfficeLocationId,
                IsProvidentFundComponent = model.IsProvidentFundComponent,
                PFTypeId = model.PFTypeId,
                PRComponentID = model.PRComponentID
            };
        }


        private PRComponent PopulatePRComponent(PRComponentViewModel model, PRComponent entityMap, int officelocation, int status)
        {
            return new PRComponent
            {
                PRComponentID = model.PRComponentID,
                ComponentName = model.ComponentName.Trim().ToUpper(),
                EmployeeStatusId = status,
                EmployeeTypeId = entityMap.EmployeeTypeId,
                OfficeLocationId = officelocation,
                IsProvidentFundComponent = entityMap.IsProvidentFundComponent,
                PFTypeId = entityMap.PFTypeId,
                ComponentType = entityMap.ComponentType,
                DesignationId = entityMap.DesignationId,
            };
        }

        private PRComponent PopulatePRComponent_desig(PRComponentViewModel model, PRComponent entityMap, int officelocation, int status, int designation )
        {
            return new PRComponent
            {
                PRComponentID = model.PRComponentID,
                ComponentName = model.ComponentName.Trim().ToUpper(),
                EmployeeStatusId = status,
                EmployeeTypeId = entityMap.EmployeeTypeId,
                OfficeLocationId = officelocation,
                IsProvidentFundComponent = entityMap.IsProvidentFundComponent,
                PFTypeId = entityMap.PFTypeId,
                ComponentType = entityMap.ComponentType,
                DesignationId = designation,
            };
        }


        private PRComponent_designation PopulatePRComponent_designation(PRComponentViewModel_designation model, PRComponent_designation entityMap, int officelocation, int status)
        {
            return new PRComponent_designation
            {
                PRComponentID = model.PRComponentID,
                ComponentName = model.ComponentName.Trim().ToUpper(),
                EmployeeStatusId = status,
                EmployeeTypeId = entityMap.EmployeeTypeId,
                OfficeLocationId = officelocation,
                IsProvidentFundComponent = entityMap.IsProvidentFundComponent,
                PFTypeId = entityMap.PFTypeId,
                ComponentType = entityMap.ComponentType,
                DesignationId = entityMap.DesignationId

            };
        }


        private BaseResponse SavePRComponent(PRComponentViewModel model, int status, int officelocation)
        {
            var response = new BaseResponse();
            try
            {
                var entity = new PRComponent();
                entity.ComponentPayrollId = model.ComponentPayrollId;
                entity.ComponentCategory = model.ComponentCategory;

                entity.ComponentName = model.ComponentName;
                entity.ComponentAmount = model.ComponentAmount;

                entity.ComponentType = model.ComponentType;
                entity.RatioBasedOn = model.RatioBasedOn;
                entity.TransactionType = model.TransactionType;
                entity.PRComponentGroupID = model.PRComponentGroupID;

                int currentYear = DateTime.Now.Year;

                // First day of the current year
                DateTime firstDayOfCurrentYear = new DateTime(currentYear, 1, 1);
                //Console.WriteLine("First day of the current year: " + firstDayOfCurrentYear.ToString("d"));

                // Last day of the year after 100 years
                int yearAfter100Years = currentYear + 100;
                DateTime lastDayOfYearAfter100Years = new DateTime(yearAfter100Years, 12, 31);
                //Console.WriteLine("Last day of the year after 100 years: " + lastDayOfYearAfter100Years.ToString("d"));

                entity.EffectiveStartDate = firstDayOfCurrentYear; // Convert.ToDateTime(model.EffectiveStartDateMsg);
                entity.EffectiveEndDate = lastDayOfYearAfter100Years; //Convert.ToDateTime(model.EffectiveEndDateMsg);
                entity.EmployeeTypeId = model.EmployeeTypeId;
                entity.EmployeeStatusId = status;
                entity.OfficeLocationId = officelocation;

                entity.SalaryRoundType = model.SalaryRoundType;
                entity.SalaryChangesByComponent = model.SalaryChangesByComponent;

                entity.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
                entity.IsProvidentFundComponent = model.IsProvidentFundComponent;
                entity.IsProductDependent = model.IsProductDependent;

                entity.MaximumLimit = Convert.ToDecimal(model.MaximumLimit);
                entity.MinimumLimit = Convert.ToDecimal(model.MinimumLimit);
                entity.SalaryAccCode = model.SalaryAccCode;

                entity.IsProductDependent = model.IsProductDependent;

                entity.SalaryEffect = model.SalaryEffect;
                entity.SalaryEffect = (model.SalaryChangesByComponent == "N/A") ? false : true;

                entity.LoanCalculationId = model.LoanCalculationId;
                entity.InterestRate = Convert.ToInt32(model.InterestRate);

                entity.MinDuration = model.MinDuration == null ? 0 : model.MinDuration;
                entity.MaxDuration = model.MaxDuration == null ? 0 : model.MaxDuration;
                entity.IsAdjustable = model.IsAdjustable;

                entity.IsActive = true;
                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.CreateDate = DateTime.UtcNow;

                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.UtcNow;

                entity.PFTypeId = model.PFTypeId;
                prComponentService.Create(entity);

                response.Message = "Component Configuration Successfull!";
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Error on Configure!";
                return response;
            }
        }

        private BaseResponse SavePRComponent_designation(PRComponentViewModel model, int status, int officelocation, int designation )
        {
            var response = new BaseResponse();
            try
            {
                var entity = new PRComponent();
                entity.ComponentPayrollId = model.ComponentPayrollId;
                entity.ComponentCategory = model.ComponentCategory;

                entity.ComponentName = model.ComponentName;
                entity.ComponentAmount = model.ComponentAmount;

                entity.ComponentType = model.ComponentType;
                entity.RatioBasedOn = model.RatioBasedOn;
                entity.TransactionType = model.TransactionType;
                entity.PRComponentGroupID = model.PRComponentGroupID;

                int currentYear = DateTime.Now.Year;

                // First day of the current year
                DateTime firstDayOfCurrentYear = new DateTime(currentYear, 1, 1);
                //Console.WriteLine("First day of the current year: " + firstDayOfCurrentYear.ToString("d"));

                // Last day of the year after 100 years
                int yearAfter100Years = currentYear + 100;
                DateTime lastDayOfYearAfter100Years = new DateTime(yearAfter100Years, 12, 31);
                //Console.WriteLine("Last day of the year after 100 years: " + lastDayOfYearAfter100Years.ToString("d"));

                entity.EffectiveStartDate = firstDayOfCurrentYear; // Convert.ToDateTime(model.EffectiveStartDateMsg);
                entity.EffectiveEndDate = lastDayOfYearAfter100Years; //Convert.ToDateTime(model.EffectiveEndDateMsg);
                entity.EmployeeTypeId = model.EmployeeTypeId;
                entity.EmployeeStatusId = status;
                entity.OfficeLocationId = officelocation;

                entity.SalaryRoundType = model.SalaryRoundType;
                entity.SalaryChangesByComponent = model.SalaryChangesByComponent;

                entity.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
                entity.IsProvidentFundComponent = model.IsProvidentFundComponent;
                entity.IsProductDependent = model.IsProductDependent;

                entity.MaximumLimit = Convert.ToDecimal(model.MaximumLimit);
                entity.MinimumLimit = Convert.ToDecimal(model.MinimumLimit);
                entity.SalaryAccCode = model.SalaryAccCode;

                entity.IsProductDependent = model.IsProductDependent;

                entity.SalaryEffect = model.SalaryEffect;
                entity.SalaryEffect = (model.SalaryChangesByComponent == "N/A") ? false : true;

                entity.LoanCalculationId = model.LoanCalculationId;
                entity.InterestRate = Convert.ToInt32(model.InterestRate);

                entity.MinDuration = model.MinDuration == null ? 0 : model.MinDuration;
                entity.MaxDuration = model.MaxDuration == null ? 0 : model.MaxDuration;
                entity.IsAdjustable = model.IsAdjustable;

                entity.IsActive = true;
                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.CreateDate = DateTime.UtcNow;

                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.UtcNow;

                entity.PFTypeId = model.PFTypeId;
                entity.DesignationId = designation; 

                prComponentService.Create(entity);

                response.Message = "Component Configuration Successfull!";
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Error on Configure!";
                return response;
            }
        }

        private BaseResponse UpdatePRComponent(PRComponent entityUpdate, PRComponentViewModel model, int status, int officelocation)
        {
            var response = new BaseResponse();
            try
            {
                entityUpdate.ComponentPayrollId = model.ComponentPayrollId;
                entityUpdate.ComponentCategory = model.ComponentCategory;

                entityUpdate.ComponentName = model.ComponentName;
                entityUpdate.ComponentAmount = model.ComponentAmount;

                entityUpdate.ComponentType = model.ComponentType;
                entityUpdate.RatioBasedOn = model.RatioBasedOn;
                entityUpdate.TransactionType = model.TransactionType;
                entityUpdate.PRComponentGroupID = model.PRComponentGroupID;
                entityUpdate.EffectiveStartDate = Convert.ToDateTime(model.EffectiveStartDateMsg);
                entityUpdate.EffectiveEndDate = Convert.ToDateTime(model.EffectiveEndDateMsg);
                entityUpdate.EmployeeTypeId = model.EmployeeTypeId;
                entityUpdate.EmployeeStatusId = status;
                entityUpdate.OfficeLocationId = officelocation;

                entityUpdate.SalaryRoundType = model.SalaryRoundType;
                entityUpdate.SalaryChangesByComponent = model.SalaryChangesByComponent;

                entityUpdate.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
                entityUpdate.IsProvidentFundComponent = model.IsProvidentFundComponent;
                entityUpdate.IsProductDependent = model.IsProductDependent;

                entityUpdate.MaximumLimit = Convert.ToDecimal(model.MaximumLimit);
                entityUpdate.MinimumLimit = Convert.ToDecimal(model.MinimumLimit);
                entityUpdate.SalaryAccCode = model.SalaryAccCode;


                entityUpdate.IsProductDependent = model.IsProductDependent;

                entityUpdate.SalaryEffect = model.SalaryEffect;
                entityUpdate.SalaryEffect = (model.SalaryChangesByComponent == "") ? false : true;

                entityUpdate.LoanCalculationId = model.LoanCalculationId;
                entityUpdate.InterestRate = Convert.ToInt32(model.InterestRate);

                entityUpdate.MinDuration = model.MinDuration;
                entityUpdate.MaxDuration = model.MaxDuration;
                entityUpdate.IsAdjustable = model.IsAdjustable;

                entityUpdate.IsActive = true;

                entityUpdate.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entityUpdate.UpdateDate = DateTime.UtcNow;

                entityUpdate.PFTypeId = model.PFTypeId;

                prComponentService.Update(entityUpdate);

                response.Message = "Updated Successfull!";
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Error on Update!";
                return response;
            }
        }

        private BaseResponse UpdatePRComponent_designation(PRComponent entityUpdate, PRComponentViewModel model, int status, int officelocation)
        {
            var response = new BaseResponse();
            try
            {
                entityUpdate.ComponentPayrollId = model.ComponentPayrollId;
                entityUpdate.ComponentCategory = model.ComponentCategory;

                entityUpdate.ComponentName = model.ComponentName;
                entityUpdate.ComponentAmount = model.ComponentAmount;

                entityUpdate.ComponentType = model.ComponentType;
                entityUpdate.RatioBasedOn = model.RatioBasedOn;
                entityUpdate.TransactionType = model.TransactionType;
                entityUpdate.PRComponentGroupID = model.PRComponentGroupID;
                entityUpdate.EffectiveStartDate = Convert.ToDateTime(model.EffectiveStartDateMsg);
                entityUpdate.EffectiveEndDate = Convert.ToDateTime(model.EffectiveEndDateMsg);
                entityUpdate.EmployeeTypeId = model.EmployeeTypeId;
                entityUpdate.EmployeeStatusId = status;
                entityUpdate.OfficeLocationId = officelocation;

                entityUpdate.SalaryRoundType = model.SalaryRoundType;
                entityUpdate.SalaryChangesByComponent = model.SalaryChangesByComponent;

                entityUpdate.IsSalaryImpactProhibited = model.IsSalaryImpactProhibited;
                entityUpdate.IsProvidentFundComponent = model.IsProvidentFundComponent;
                entityUpdate.IsProductDependent = model.IsProductDependent;

                entityUpdate.MaximumLimit = Convert.ToDecimal(model.MaximumLimit);
                entityUpdate.MinimumLimit = Convert.ToDecimal(model.MinimumLimit);
                entityUpdate.SalaryAccCode = model.SalaryAccCode;


                entityUpdate.IsProductDependent = model.IsProductDependent;

                entityUpdate.SalaryEffect = model.SalaryEffect;
                entityUpdate.SalaryEffect = (model.SalaryChangesByComponent == "") ? false : true;

                entityUpdate.LoanCalculationId = model.LoanCalculationId;
                entityUpdate.InterestRate = Convert.ToInt32(model.InterestRate);

                entityUpdate.MinDuration = model.MinDuration;
                entityUpdate.MaxDuration = model.MaxDuration;
                entityUpdate.IsAdjustable = model.IsAdjustable;

                entityUpdate.IsActive = true;

                entityUpdate.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entityUpdate.UpdateDate = DateTime.UtcNow;

                entityUpdate.PFTypeId = model.PFTypeId;
                entityUpdate.DesignationId = model.DesignationId;

                prComponentService.Update(entityUpdate);

                response.Message = "Updated Successfull!";
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Error on Update!";
                return response;
            }
        }


        private void InsertComponentHistory(PRComponent entityUpdate)
        {
            var param = new
            {
                PRComponentID = entityUpdate.PRComponentID,
                CreateUser = entityUpdate.CreateUser
            };
            var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_InsertComponentHistory");
        }

        public void MapDropDownList_designation(PRComponentViewModel_designation model)
        {
            model.SalaryChangesByComponentList = commonStaticDropDown.SalaryChangesByComponentList();
            model.RatioBasedList = commonStaticDropDown.ddlSalaryRatio();
            model.DurationList = commonStaticDropDown.YearDurationList(5);
            model.ComponentTypeList = commonStaticDropDown.SalaryCalculationType();
            model.TransactionTypeList = commonStaticDropDown.SalaryAccountTransactionType("0");
            model.ComponentCategoryList = commonStaticDropDown.SalaryComponentCategory();
            model.ProductdependentList = commonStaticDropDown.YesNoDropDown_bool();
            model.SalaryEffectList = commonStaticDropDown.YesNoDropDown_bool();
            model.IsAdjustableList = commonStaticDropDown.YesNoDropDown_bool();
            model.ProvidentFundComponentList = commonStaticDropDown.YesNoDropDown_bool();
            model.SalaryImpactProhibitedList = commonStaticDropDown.YesNoDropDown_bool();

            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: true);
            model.LoneCalculationList = commonDynamicDropDown.loanCalculationList();
            model.OfficeLocationList = commonDynamicDropDown.OfficeLocationList();
            model.EmployeeTypeList = commonDynamicDropDown.ddlEmployeeType();
            model.ComponentGroupList = commonDynamicDropDown.PRComponentGroup_Only_SalaryOrDeduction();

            model.PFTypeList = commonDynamicDropDown.ProvidentFundType();

            model.SalaryRoundTypeList = commonDynamicDropDown.PRSalaryRoundType();

            List<string> ignorList = new List<string>();
            ignorList.Add("Deposit");
            model.ComponentList = commonDynamicDropDown.ddlInitial(true, "");
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();//payroll designation
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();//office designation
            model.RankList = commonDynamicDropDown.GetAllOfficeDesignationList();//officeOrnamentDesign_items;//office designation 

            //commonDynamicDropDown.PayrollComponentIgnoreByCategory(ignorList);
        }
        public void MapDropDownList(PRComponentViewModel model)
        {
            model.SalaryChangesByComponentList = commonStaticDropDown.SalaryChangesByComponentList();
            model.RatioBasedList = commonStaticDropDown.ddlSalaryRatio();
            model.DurationList = commonStaticDropDown.YearDurationList(5);
            model.ComponentTypeList = commonStaticDropDown.SalaryCalculationType();
            model.TransactionTypeList = commonStaticDropDown.SalaryAccountTransactionType("0");
            model.ComponentCategoryList = commonStaticDropDown.SalaryComponentCategory();
            model.ProductdependentList = commonStaticDropDown.YesNoDropDown_bool();
            model.SalaryEffectList = commonStaticDropDown.YesNoDropDown_bool();
            model.IsAdjustableList = commonStaticDropDown.YesNoDropDown_bool();
            model.ProvidentFundComponentList = commonStaticDropDown.YesNoDropDown_bool();
            model.SalaryImpactProhibitedList = commonStaticDropDown.YesNoDropDown_bool();
            
            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: true);
            model.LoneCalculationList = commonDynamicDropDown.loanCalculationList();
            model.OfficeLocationList = commonDynamicDropDown.OfficeLocationList();
            model.EmployeeTypeList = commonDynamicDropDown.ddlEmployeeType();
            model.ComponentGroupList = commonDynamicDropDown.PRComponentGroup_Only_SalaryOrDeduction();

            model.PFTypeList = commonDynamicDropDown.ProvidentFundType();

            model.SalaryRoundTypeList = commonDynamicDropDown.PRSalaryRoundType();

            List<string> ignorList = new List<string>();
            ignorList.Add("Deposit");
            model.ComponentList = commonDynamicDropDown.ddlInitial(true, "");

            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();//payroll 

            //commonDynamicDropDown.PayrollComponentIgnoreByCategory(ignorList);
        }
        #endregion
    }
}