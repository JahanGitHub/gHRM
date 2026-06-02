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
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{
    public class EmployeeOtherInformationController : BaseController
    {
        #region Varibles

        private readonly IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly ILinkWithEmployeeService linkWithEmployeeService;
        private readonly IInternalOrganizationService internalOrganizationService;
        private readonly IWorkExperienceWithInterOrganizationService workExperienceWithInterOrganizationService;
        private readonly ICountryService countryService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IView_EmployeeTrainingService view_EmployeeTrainingService;
        private readonly IEmployeeFamilyInfoApprovalProcessService employeeFamilyInfoApprovalProcessService;
        private readonly IEmployeeMaritalStatusApprovalService employeeMaritalStatusApprovalService;
        private readonly IEmployeeFamilyInfoService employeeFamilyInfoService;
        private readonly IEmployeePreviousWorkExperienceService employeePreviousWorkExperienceService;
        private readonly IEmployeeInformationApprovalService employeeInformationApprovalService;
        private readonly IEmployeePublicationService employeePublicationService;
        private readonly IFamilyRelationService familyRelationService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly ICurrentOrganizationRelationshipService currentOrganizationRelationshipService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeTranningDropDownService employeeTranningDropDownService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public EmployeeOtherInformationController(
            IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService,
            IEmployeeService employeeService, IEmployeeSPService employeeSpService,
            ILinkWithEmployeeService linkWithEmployeeService,
            IInternalOrganizationService internalOrganizationService,
            IWorkExperienceWithInterOrganizationService workExperienceWithInterOrganizationService,
            ICountryService countryService,
            IEmployeeTrainingService employeeTrainingService,
            IView_EmployeeTrainingService view_EmployeeTrainingService,
            IEmployeeFamilyInfoApprovalProcessService employeeFamilyInfoApprovalProcessService,
            IEmployeeMaritalStatusApprovalService employeeMaritalStatusApprovalService,
            IEmployeeFamilyInfoService employeeFamilyInfoService,
            IEmployeePreviousWorkExperienceService employeePreviousWorkExperienceService,
            IEmployeeInformationApprovalService employeeInformationApprovalService,
            IEmployeePublicationService employeePublicationService,
            IFamilyRelationService familyRelationService,
            IOfficeTypeService officeTypeService,
            IOfficeService officeService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeDesignationService employeeDesignationService,
            ICurrentOrganizationRelationshipService currentOrganizationRelationshipService,
            IEmployementTypeService employementTypeService,
            IEmployeeTranningDropDownService employeeTranningDropDownService
        )
        {
            this.employeeOfficeVisitInformationService = employeeOfficeVisitInformationService;
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.linkWithEmployeeService = linkWithEmployeeService;
            this.internalOrganizationService = internalOrganizationService;
            this.workExperienceWithInterOrganizationService = workExperienceWithInterOrganizationService;
            this.countryService = countryService;
            this.employeeTrainingService = employeeTrainingService;
            this.view_EmployeeTrainingService = view_EmployeeTrainingService;
            this.employeeFamilyInfoApprovalProcessService = employeeFamilyInfoApprovalProcessService;
            this.employeeMaritalStatusApprovalService = employeeMaritalStatusApprovalService;
            this.employeeFamilyInfoService = employeeFamilyInfoService;
            this.employeePreviousWorkExperienceService = employeePreviousWorkExperienceService;
            this.employeeInformationApprovalService = employeeInformationApprovalService;
            this.employeePublicationService = employeePublicationService;
            this.familyRelationService = familyRelationService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.currentOrganizationRelationshipService = currentOrganizationRelationshipService;
            this.employementTypeService = employementTypeService;
            this.employeeTranningDropDownService = employeeTranningDropDownService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion


        #region Events


        public ActionResult EmployeeGuarantorMoney(string Code)
        {
            var model = new EmployeeGuarantorMoneyViewModel();
            MapDropdownForTransaction(model);
            MapDropdownForPayment(model);
            ViewBag.CompanyCode = SessionHelper.CompanyCode;
            ViewBag.err = "";
            return View(model);
        }




        public ActionResult EligibleEmployeeConfirmation(string Code)
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            ViewData["DesignationList"] = items2;

            var model = new EmployeePromotionViewModel();
           // MapDropDown(model);

            ViewData["Months"] = commonStaticDropDown.MonthList();
            ViewData["Years"] = commonStaticDropDown.YearList(10, 20);

            return View(model);
        }


        public ActionResult EmployeeOtherInfo(string Code)
        {
            var model = new EmployeeOtherInformationViewModel();
            model.EmployeeCode = Code;

            MapDropDownListForEmployeeInfo(model);
            MapDropDownListFortraninginfo(model);
            return View(model);
        }
        public ActionResult EmployeeOtherInfoApproval()
        {
            var model = new EmployeeOtherInformationViewModel();
            MapDropDownListForEmployeeInfo(model);
            MapDropDownListFortraninginfo(model);
            return View(model);
        }
        public ActionResult EmployeeTrainingInfoApproval()
        {
            var model = new EmployeeOtherInformationViewModel();
            MapDropDownListForEmployeeInfo(model);
            MapDropDownListFortraninginfo(model);
            return View(model);
        }

        public ActionResult EmploymentType()
        {
            return View();
        }

        #endregion

        #region Ajax Methods        

        public JsonResult SaveEmployementType(string EmployementTypeName)
        {
            var result = 0;
            var message = "";
            try
            {
                var checkDuplicate =
                    employementTypeService.GetAll()
                        .Where(
                            p => p.IsActive == true && p.EmployementTypeName.ToUpper().Trim() == EmployementTypeName.ToUpper().Trim())
                        .ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "This Employment type already exists, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = new EmployementType();
                    model.EmployementTypeName = EmployementTypeName;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employementTypeService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployementTypeList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var typeList = employementTypeService.GetMany(p => p.IsActive == true).ToList();
            var currentPageRecords = typeList.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = typeList.LongCount(), JsonRequestBehavior.AllowGet });
        }


        public JsonResult UpdateEmployementType(string EmployementTypeName, int EmployementTypeId)
        {
            var result = 0;
            var message = "";
            try
            {
                var checkDuplicate =
                    employementTypeService.GetAll()
                        .Where(
                            p => p.IsActive == true && p.EmployementTypeId != EmployementTypeId && p.EmployementTypeName.ToUpper().Trim() == EmployementTypeName.ToUpper().Trim())
                        .ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "This Employment type already exists, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = employementTypeService.GetById(EmployementTypeId);
                    model.EmployementTypeName = EmployementTypeName;
                    model.IsActive = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employementTypeService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployementType(int id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employementTypeService.GetById(id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employementTypeService.Update(model);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add employee training dropdown title
        /// </summary>
        /// <param name="employeeTraining"></param>
        /// 


        public void MapDropdownForTransaction(EmployeeGuarantorMoneyViewModel model)
        {
            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Deposit", Value = "Deposit" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
        
            model.TransactionTypeList = employeeProfilelist;
        }


        public void MapDropdownForPayment(EmployeeGuarantorMoneyViewModel model)
        {
            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });

            model.PaymentTypeList = employeeProfilelist;
        }




        private void AddEmployeeTrainingDropDownTitle(EmployeeTraining employeeTraining)
        {
            var entitys = new EmployeeTranningDropDown
            {
                EmployeeTrainingDropDownName = employeeTraining.TrainingTitle,
                IsActive = true,
                CreateBy = SessionHelper.LoggedInEmployeeID
            };

            //let's add into EmployeeTranningDropDown table
            employeeTranningDropDownService.Create(entitys);
        }
        private void MapDropDownListForEmployeeInfo(EmployeeOtherInformationViewModel model)
        {
            var visitType = new List<SelectListItem>();
            visitType.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            visitType.Add(new SelectListItem() { Text = "Local", Value = "L" });
            visitType.Add(new SelectListItem() { Text = "International", Value = "IN" });
            model.VisitTypeList = visitType;

            var offProvided = new List<SelectListItem>();
            offProvided.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            offProvided.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            offProvided.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.OfficeProvidedList = offProvided;

            var organizationList = internalOrganizationService.GetMany(p => p.IsActive == true).ToList();
            var viewOrgList = organizationList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.OrganizationName,
                Value = p.OrganizationCode
            }).ToList();
            var orgList = new List<SelectListItem>();
            orgList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            orgList.AddRange(viewOrgList);
            model.OrganizationList = orgList;

            model.EmployeeList = commonStaticDropDown.ddlInitial();
            model.relationWithEmployeeList = commonStaticDropDown.GetRelationTypeList();
            model.GenderList = commonStaticDropDown.GetGendersList();
            model.MaritalList = commonStaticDropDown.GetMaritalStatusList();


            var isapproved = new List<SelectListItem>();
            isapproved.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isapproved.Add(new SelectListItem { Text = "Approved", Value = "1" });
            model.IsApprovedList = isapproved;

            var isrejected = new List<SelectListItem>();
            isrejected.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isrejected.Add(new SelectListItem { Text = "Rejected", Value = "1" });
            model.IsRejectedList = isrejected;

            var relationList = familyRelationService.GetMany(p => p.IsActive == true).ToList();
            var viewRelationList = relationList.Select(p => new SelectListItem()
            {
                Text = p.RelationName,
                Value = p.RelaitonId.ToString()
            }).ToList();
            var relation = new List<SelectListItem>();
            relation.Add(new SelectListItem { Text = "Please Select", Value = "" });
            relation.AddRange(viewRelationList);
            model.RelationshipList = relation;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.OfficeList = commonDynamicDropDown.ddlInitial();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
        }

        private void MapDropDownListFortraninginfo(EmployeeOtherInformationViewModel model)
        {
            var countryList = countryService.GetMany(w => w.CountryId == CountryID);
            var viewCountryList = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountryList);
            model.CountryList = country_items;

            var CurrentOfficeTraining = new List<SelectListItem>();
            CurrentOfficeTraining.Add(new SelectListItem { Text = "Please Select", Value = "" });
            CurrentOfficeTraining.Add(new SelectListItem { Text = "Yes", Value = "1" });
            CurrentOfficeTraining.Add(new SelectListItem { Text = "No", Value = "0" });
            model.CurrentOfficeTrainingList = CurrentOfficeTraining;

            var isapproved = new List<SelectListItem>();
            isapproved.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isapproved.Add(new SelectListItem { Text = "Approved", Value = "1" });
            model.IsApprovedList = isapproved;

            var isrejected = new List<SelectListItem>();
            isrejected.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isrejected.Add(new SelectListItem { Text = "Rejected", Value = "1" });
            model.IsRejectedList = isrejected;

            var employeeTranningDropDown = employeeTranningDropDownService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewemployeeTranningDropDown = employeeTranningDropDown.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeTrainingDropDownId.ToString(),
                Text = x.EmployeeTrainingDropDownName.ToString()
            });
            var employeeTranningDropDowns = new List<SelectListItem>();
            employeeTranningDropDowns.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            employeeTranningDropDowns.AddRange(viewemployeeTranningDropDown);
            model.EmployeeTranningDropDownList = employeeTranningDropDowns;


            var instituteNameDropDowns = new List<SelectListItem>();
            instituteNameDropDowns.Add(new SelectListItem { Text = "Please Select", Value = "" });
            instituteNameDropDowns.Add(new SelectListItem { Text = "InM", Value = "InM" });
            instituteNameDropDowns.Add(new SelectListItem { Text = "PKSF", Value = "PKSF" });
            instituteNameDropDowns.Add(new SelectListItem { Text = "JCF", Value = "JCF" });
            model.InstituteNameDropDownList = instituteNameDropDowns;

        }


        [HttpPost]
        public JsonResult SaveOfficeVisitInfo(EmployeeOfficeVisitInformation obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = new EmployeeOfficeVisitInformation();
                var employeeId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;



                model.EmployeeId = employeeId;
                model.EmployeeCode = obj.EmployeeCode;
                model.VisitType = obj.VisitType;
                model.Location = obj.Location;
                model.Reason = obj.Reason;
                model.CurrentOfficeProvided = obj.CurrentOfficeProvided;
                model.IsActive = true;
                model.IsRejected = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;

                var loginId = employeeInformationApprovalService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode).FirstOrDefault();
                if (loginId != null)
                {
                    model.IsApproved = true;
                }
                else
                {
                    model.IsApproved = false;
                }
                employeeOfficeVisitInformationService.Create(model);
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Save failed, Because Employee Status Not Active";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeOfficeVisitInformation(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var list = employeeOfficeVisitInformationService.GetMany(p => p.IsActive == true && p.EmployeeCode == empCode && (p.IsApproved == false || p.IsRejected == false)).ToList();
            var view_List = list.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                EmpOfficeVisitId = p.EmpOfficeVisitId,
                EmployeeId = p.EmployeeId,
                EmployeeCode = p.EmployeeCode,
                VisitType = p.VisitType == "L" ? "Local" : "International",
                Location = p.Location,
                Reason = p.Reason,
                CurrentOfficeProvidedVal = p.CurrentOfficeProvided,
                CurrentOfficeProvided = p.CurrentOfficeProvided == 1 ? "Yes" : "No",
                IsApproved = p.IsApproved,
                IsRejected = p.IsRejected,
            }).ToList();

            foreach (var item in view_List)
            {
                if (item.IsApproved == true)
                {
                    item.VisitStatus = "Approved";
                }

                if (item.IsRejected == true)
                {
                    item.VisitStatus = "Rejected";
                }

                if (item.IsRejected == false && item.IsApproved == false)
                {
                    item.VisitStatus = "Pending";
                }
            }
            var currentPageRecords = view_List.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_List.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateOfficeVisitInfo(EmployeeOfficeVisitInformation obj)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeOfficeVisitInformationService.GetById(obj.EmpOfficeVisitId);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Visit status pending is applicable for edit, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();

                    if (loginId != null)
                    {
                        model.VisitType = obj.VisitType;
                        model.Location = obj.Location;
                        model.Reason = obj.Reason;
                        model.CurrentOfficeProvided = obj.CurrentOfficeProvided;
                        model.IsActive = true;
                        model.IsApproved = true;
                        model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        model.UpdateDate = DateTime.UtcNow;
                        employeeOfficeVisitInformationService.Update(model);
                    }
                    else
                    {
                        model.VisitType = obj.VisitType;
                        model.Location = obj.Location;
                        model.Reason = obj.Reason;
                        model.CurrentOfficeProvided = obj.CurrentOfficeProvided;
                        model.IsActive = true;
                        model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        model.UpdateDate = DateTime.UtcNow;
                        employeeOfficeVisitInformationService.Update(model);
                    }
                    result = 1;
                    message = "Updated successfully";
                }

            }
            catch (Exception)
            {
                result = 0;
                message = "Update failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfficeVisitApproval(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var currentPageRecords = new List<EmployeeOfficeVisitInformation>();
            var vmcar = new List<EmployeeOfficeVisitInformation>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var list = employeeOfficeVisitInformationService.GetMany(t => t.IsActive == true && t.IsApproved == false && t.IsRejected == false).ToList();
                vmcar = list.AsEnumerable().Select(row => new EmployeeOfficeVisitInformation
                {
                    EmpOfficeVisitId = row.EmpOfficeVisitId,
                    EmployeeId = row.EmployeeId,
                    EmployeeCode = row.EmployeeCode,
                    VisitType = row.VisitType,
                    Location = row.Location,
                    Reason = row.Reason,
                    CurrentOfficeProvided = row.CurrentOfficeProvided
                }).ToList();
                currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult InformationOfficeVisitApproved(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsApproved = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Approved Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult InformationOfficeVisitRejected(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsRejected = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteOfficeVisitInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Visit status pending is applicable for delete, Delete Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeInfoByCode(string empCode)
        {
            var result = string.Empty;
            var param = new { EmployeeCode = empCode };
            var empInfo = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");
            var viewEmpInfo =
                empInfo.Tables[0].AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    EmployeeId = p.Field<long>("EmployeeId"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    Department = p.Field<string>("DepartmentName"),
                    Designation = p.Field<string>("DesignationName"),
                    EmployeeName = p.Field<string>("EmployeeName"),

                    ZoneOfficeName = p.Field<string>("ZoneOfficeName"),
                    AreaOfficeName = p.Field<string>("AreaOfficeName"),
                    UnitOfficeName = p.Field<string>("UnitOfficeName"),

                }).ToList();

            if (viewEmpInfo.Any())
            {
                result = "OK";
            }
            else
            {
                result = "Not OK";
            }

            return Json(new { Result = result, dataList = viewEmpInfo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeOfficeVisitApprove(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsApproved = true;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Approved Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Approval denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeOfficeVisitReject(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeOfficeVisitInformationService.GetById(Id);
                model.IsRejected = true;
                employeeOfficeVisitInformationService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Rejection denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveEmployeeLink(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var empId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;
                var isExists = linkWithEmployeeService.GetMany(x => x.IsActive == true && x.EmployeeId == empId && x.Department == obj.RelativeDepartmentName && x.Designation == obj.RelativeDesignationName && x.EmployeeName == obj.RelativeEmployeeName);
                if (isExists.Count() == 0)
                {
                    var loginId = employeeInformationApprovalService.GetAll().Where(p => p.IsActive == true && p.EmployeeId == SessionHelper.LoggedInEmployeeID).FirstOrDefault();

                    var model = new LinkWithEmployee();
                    model.OrganizationCode = obj.OrganizationCode;
                    model.EmployeeCode = obj.EmployeeCode;
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.EmployeeName = obj.RelativeEmployeeName;
                    model.EmployeeId = empId;
                    model.Relation = obj.Relation;
                    model.IsRejected = false;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;

                    if (loginId != null)
                    {
                        model.IsApproved = true;
                    }
                    else
                    {
                        model.IsApproved = false;
                    }
                    linkWithEmployeeService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                }
                else {
                    result = 0;
                    message = "Duplicate Employee, Save denied";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeRelativeInfo(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var relativeInfo = linkWithEmployeeService.GetMany(p => p.IsActive == true && p.EmployeeCode == empCode).ToList();
            var viewRelativeInfo = relativeInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                LinkId = p.LinkId,
                OrganizationCode = p.OrganizationCode,
                RelativeEmployeeCode = p.EmployeeCode,
                RelativeDepartmentName = p.Department,
                RelativeDesignationName = p.Designation,
                RelativeEmployeeName = p.EmployeeName,
                Relation = p.Relation,
                IsApproved = p.IsApproved,
                IsRejected = p.IsRejected
            }).ToList();
            foreach (var item in viewRelativeInfo)
            {
                if (item.IsApproved == true)
                {
                    item.RelativeStatus = "Approved";
                }

                if (item.IsRejected == true)
                {
                    item.RelativeStatus = "Rejected";
                }

                if (item.IsRejected == false && item.IsApproved == false)
                {
                    item.RelativeStatus = "Pending";
                }
            }
            var currentPageRecords = viewRelativeInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewRelativeInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }


        [HttpPost]
        public JsonResult UpdateEmployeeLink(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = linkWithEmployeeService.GetById(obj.LinkId);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Relative status pending is applicable for edit, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();
                if (loginId != null)
                {
                    model.OrganizationCode = obj.OrganizationCode;
                    model.EmployeeCode = obj.RelativeEmployeeCode;
                    model.EmployeeName = obj.RelativeEmployeeName;
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.EmployeeName = obj.RelativeEmployeeName;
                    model.Relation = obj.Relation;
                    model.IsApproved = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    linkWithEmployeeService.Update(model);
                }
                else
                {
                    model.OrganizationCode = obj.OrganizationCode;
                    model.EmployeeCode = obj.RelativeEmployeeCode;
                    model.EmployeeName = obj.RelativeEmployeeName;
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.EmployeeName = obj.RelativeEmployeeName;
                    model.Relation = obj.Relation;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    linkWithEmployeeService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateCurrentOfficeEmployeeRelation(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {

                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeId == SessionHelper.LoggedInEmployeeID)
                        .FirstOrDefault();
                var isDuplicate = currentOrganizationRelationshipService.GetMany(x => x.IsActive == true && x.OfficeId == obj.OfficeId && x.DepartmentId == obj.COEDepartmentId && x.DesignationId == obj.COEDesignationId && x.EmployeeName == obj.COEmployeeName);

                if (isDuplicate.Count() == 0)
                {
                    var model = currentOrganizationRelationshipService.GetById(obj.SelfOrgRelationId);
                    model.OfficeId = obj.OfficeId;
                    model.EmployeeName = obj.COEmployeeName;
                    model.RelationId = obj.COERelationId;
                    model.DepartmentId = obj.COEDepartmentId;

                    model.DesignationId = obj.COEDesignationId;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    if (loginId != null)
                    {
                        model.IsApproved = true;
                    }
                    currentOrganizationRelationshipService.Update(model);

                    result = 1;
                    message = "Updated successfully";
                }
                else {
                    result = 0;
                    message = "Duplicate Employee, Update Failed";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListEmployeeLinkApproval(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewRelativeInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var relativeInfo = linkWithEmployeeService.GetMany(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false).ToList();
                viewRelativeInfo = relativeInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    LinkId = p.LinkId,
                    OrganizationCode = p.OrganizationCode,
                    RelativeEmployeeCode = p.EmployeeCode,
                    RelativeDepartmentName = p.Department,
                    RelativeDesignationName = p.Designation,
                    RelativeEmployeeName = p.EmployeeName,
                    IsApproved = p.IsApproved,
                    IsRejected = p.IsRejected,
                }).ToList();
                currentPageRecords = viewRelativeInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewRelativeInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult InformationEmployeeLinkApproved(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = linkWithEmployeeService.GetById(Id);
                model.IsApproved = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Update(model);
                result = 1;
                message = "Approved Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult InformationEmployeeLinkRejected(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = linkWithEmployeeService.GetById(Id);
                model.IsRejected = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteEmployeeRelativeInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = linkWithEmployeeService.GetById(Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Relative status pending is applicable for delete, Delete Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                linkWithEmployeeService.Update(model);
                result = 1;
                message = "Deleted successfully";

            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveWorkExperience(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();

                if (loginId != null)
                {
                    var employeeId = loginId.EmployeeId;
                    var employeeCode = loginId.EmployeeCode;

                    var model = new WorkExperienceWithInterOrganization();
                    model.OrgCode = obj.OrganizationCode;
                    model.EmployeeCode = employeeCode;
                    model.EmployeeId = employeeId;
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.JoiningDate = obj.JoiningDate;
                    model.ReleaseDate = obj.ReleaseDate;
                    model.IsApproved = true;
                    model.IsRejected = false;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    workExperienceWithInterOrganizationService.Create(model);
                }
                else
                {
                    var model = new WorkExperienceWithInterOrganization();
                    model.OrgCode = obj.OrganizationCode;
                    model.EmployeeCode = obj.EmployeeCode;
                    var employeeId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;
                    model.EmployeeId = employeeId;
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.JoiningDate = obj.JoiningDate;
                    model.ReleaseDate = obj.ReleaseDate;
                    model.IsApproved = false;
                    model.IsRejected = false;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    workExperienceWithInterOrganizationService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkExperienceInfo(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {

            var expInfo =
            workExperienceWithInterOrganizationService.GetAll()
                .Where(p => p.IsActive == true && p.EmployeeCode == empCode)
                .ToList();
            var viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                WorkExpId = p.WorkExpId,
                EmployeeCode = p.EmployeeCode,
                OrganizationCode = p.OrgCode,
                RelativeDepartmentName = p.Department,
                RelativeDesignationName = p.Designation,
                JoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                ReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy"),
                IsApproved = p.IsApproved,
                IsRejected = p.IsRejected
            }).ToList();
            foreach (var item in viewExpInfo)
            {
                if (item.IsApproved == true)
                {
                    item.ExperienceStatus = "Approved";
                }
                if (item.IsRejected == true)
                {
                    item.ExperienceStatus = "Rejected";
                }
                if (item.IsApproved == false && item.IsRejected == false)
                {
                    item.ExperienceStatus = "Pending";
                }
            }
            var currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewExpInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }


        [HttpPost]
        public JsonResult UpdateWorkExperience(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = workExperienceWithInterOrganizationService.GetById(obj.WorkExpId);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Visit status pending is applicable for edit, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();
                if (loginId != null)
                {
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.JoiningDate = obj.JoiningDate;
                    model.ReleaseDate = obj.ReleaseDate;
                    model.IsApproved = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    workExperienceWithInterOrganizationService.Update(model);
                }
                else
                {
                    model.Department = obj.RelativeDepartmentName;
                    model.Designation = obj.RelativeDesignationName;
                    model.JoiningDate = obj.JoiningDate;
                    model.ReleaseDate = obj.ReleaseDate;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    workExperienceWithInterOrganizationService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListEmployeeWorkExperience(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewExpInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var expInfo =
                workExperienceWithInterOrganizationService.GetAll()
                    .Where(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false)
                    .ToList();
                viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    WorkExpId = p.WorkExpId,
                    EmployeeCode = p.EmployeeCode,
                    OrganizationCode = p.OrgCode,
                    RelativeDepartmentName = p.Department,
                    RelativeDesignationName = p.Designation,
                    IsApproved = p.IsApproved,
                    IsRejected = p.IsRejected,
                    JoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                    ReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy")
                }).ToList();
                currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewExpInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult InformationEmployeeWorkExperienceIsApproved(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = workExperienceWithInterOrganizationService.GetById(Id);
                model.IsApproved = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                workExperienceWithInterOrganizationService.Update(model);
                result = 1;
                message = "Approved Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult InformationEmployeeWorkExperienceIsRejected(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = workExperienceWithInterOrganizationService.GetById(Id);
                model.IsRejected = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                workExperienceWithInterOrganizationService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteWorkExperienceInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = workExperienceWithInterOrganizationService.GetById(Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Experince status pending is applicable for delete, Delete Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                workExperienceWithInterOrganizationService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        // employee training start
        [HttpPost]
        public JsonResult SaveTraining(EmployeeTraining employeeTraining)
        {
            var result = 0;
            var message = "";
            try
            {
                var employeeId = employeeService.GetByCode(employeeTraining.EmployeeCode).EmployeeId;

                var isDuplicate =
                    employeeTrainingService.GetAll()
                        .Any(
                            p =>
                                p.IsActive == true && p.TrainingTitle.ToUpper().Trim() == employeeTraining.TrainingTitle.ToUpper().Trim()
                                //&& p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim()
                                && p.EmployeeId == employeeId);
                if (isDuplicate)
                {
                    result = 0;
                    message = "Duplicate Employee Training Title & Institute Name found, save denied";
                    return Json(new { message = message, result = result }, JsonRequestBehavior.AllowGet);
                }

                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == employeeTraining.EmployeeCode)
                        .FirstOrDefault();

                if (loginId != null)
                {
                    employeeId = loginId.EmployeeId;
                    var employeeCode = loginId.EmployeeCode;

                    var entity = employeeTraining;
                    entity.EmployeeId = employeeId;
                    entity.EmployeeCode = employeeCode;
                    entity.IsApproved = true;
                    entity.IsRejected = false;
                    entity.OrganisedBy = employeeTraining.OrganisedBy;
                    entity.SupportedBy = employeeTraining.SupportedBy;
                    entity.approveby = employeeTraining.approveby;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.ApproveAndRejectionDate = DateTime.UtcNow;
                    var savedEntity = employeeTrainingService.Create(entity);
                }
                else
                {
                    var entity = employeeTraining;                    
                    entity.EmployeeId = employeeId;
                    entity.IsApproved = false;
                    entity.IsRejected = false;
                    entity.OrganisedBy = employeeTraining.OrganisedBy;
                    entity.SupportedBy = employeeTraining.SupportedBy;
                    entity.approveby = employeeTraining.approveby;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.ApproveAndRejectionDate = DateTime.UtcNow;

                    //let's add into EmployeeTraining table
                    employeeTrainingService.Create(entity);

                    //check Employee Training Dropdown is exist or not
                    var isExistEmployeeTrainingDropdown = employeeTranningDropDownService
                                                                .IsExistEmployeeTrainingDropDownByTitle(employeeTraining.TrainingTitle);

                    if (!isExistEmployeeTrainingDropdown)
                    {
                        //let's add into EmployeeTranningDropDown table
                        AddEmployeeTrainingDropDownTitle(employeeTraining);
                    }
                }
                result = 1;
                message = "Saved Successfully";
            }
            catch (Exception ex)
            {
                message = ex.InnerException.Message.ToString();
            }
            return Json(new { message = message, result = result }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateTraining(EmployeeTraining employeeTraining)
        {
            var result = 0;
            var message = "";
            try
            {
                var isExist= employeeTrainingService.IsExistEmployeeTraining(employeeTraining);

                if (isExist)               
                    return Json(new { message = "This is training already exist! Please try again.", result = result }, JsonRequestBehavior.AllowGet);
                
                var entity = employeeTrainingService.GetById(employeeTraining.EmployeeTrainingId);

                //if (entity.IsApproved == true || entity.IsRejected == true)               
                //    return Json(new { message = "Only Visit status pending is applicable for edit, Update Denied", result = result }, JsonRequestBehavior.AllowGet);
                
                var loginId =
                employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeCode == employeeTraining.EmployeeCode)
                    .FirstOrDefault();

                if (loginId != null)
                {
                    entity.TrainingTitle = employeeTraining.TrainingTitle;
                    entity.InstituteName = employeeTraining.InstituteName;
                    entity.TrainingCountryId = employeeTraining.TrainingCountryId;
                    entity.TrainingTopics = employeeTraining.TrainingTopics;
                    entity.OrganisedBy = employeeTraining.OrganisedBy;
                    entity.SupportedBy = employeeTraining.SupportedBy;
                    entity.Result = employeeTraining.Result;
                    entity.TrainingDateFrom = employeeTraining.TrainingDateFrom;
                    entity.TrainingDateTo = employeeTraining.TrainingDateTo;
                    entity.CurrentOfficeTraining = employeeTraining.CurrentOfficeTraining;
                    entity.IsApproved = true;
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeTrainingService.Update(entity);
                }
                else
                {
                    entity.TrainingTitle = employeeTraining.TrainingTitle;
                    entity.InstituteName = employeeTraining.InstituteName;
                    entity.TrainingCountryId = employeeTraining.TrainingCountryId;
                    entity.TrainingTopics = employeeTraining.TrainingTopics;
                    entity.OrganisedBy = employeeTraining.OrganisedBy;
                    entity.SupportedBy = employeeTraining.SupportedBy;
                    entity.Result = employeeTraining.Result;
                    entity.TrainingDateFrom = employeeTraining.TrainingDateFrom;
                    entity.TrainingDateTo = employeeTraining.TrainingDateTo;
                    entity.CurrentOfficeTraining = employeeTraining.CurrentOfficeTraining;
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateDate = DateTime.UtcNow;

                    employeeTrainingService.Update(entity);

                    //check Employee Training Dropdown is exist or not
                    var isExistEmployeeTrainingDropdown = employeeTranningDropDownService
                                                                .IsExistEmployeeTrainingDropDownByTitle(employeeTraining.TrainingTitle);

                    if (!isExistEmployeeTrainingDropdown)
                    {
                        //let's add into EmployeeTranningDropDown table
                        AddEmployeeTrainingDropDownTitle(employeeTraining);
                    }
                }
                message = "Update Successfull";
                result = 1;
            }
            catch (Exception ex)
            {
                message = ex.InnerException.Message.ToString();
                result = 0;
            }

            return Json(new { message = message, result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListTraning(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string empCode)
        {
            var vmcar = view_EmployeeTrainingService.GetMany(t => t.IsActive == true && t.EmployeeCode == empCode).ToList();

            var currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult ListTraningApproval(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var currentPageRecords = new List<View_EmployeeTraining>();
            var vmcar = new List<View_EmployeeTraining>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                vmcar = view_EmployeeTrainingService.GetMany(t => t.IsActive == true && t.IsApproved == false && t.IsRejected == false).ToList();
                currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult InformationTraningisapproved(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTrainingService.GetById(Id);
                model.IsApproved = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTrainingService.Update(model);
                result = 1;
                message = "Approved Successfully";

            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult InformationTraningisrejected(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTrainingService.GetById(Id);
                model.IsRejected = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTrainingService.Update(model);
                result = 1;
                message = "Rejected Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteTraning(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTrainingService.GetById(Id);
                //if (model.IsApproved == true || model.IsRejected == true)
                //{
                //    result = 0;
                //    message = "Only training status pending is applicable for delete, Delete Denied";
                //    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                //}
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTrainingService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.Message.ToString();

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GETTraningTimeTrainingDateTo(DateTime TrainingDateFrom, DateTime TrainingDateTo)
        {
            var result = 0;
            try
            {
                if (TrainingDateFrom < TrainingDateTo)
                {
                    result = 1;
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = 0;
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }

        }
        // employee training end

        [HttpPost]
        public JsonResult SaveEmployeeFamilyInfo(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();

                if (loginId != null)
                {
                    var employeeId = loginId.EmployeeId;
                    //var employeeCode = loginId.EmployeeCode;

                    var model = new EmployeeFamilyInfoApprovalProcess();
                    model.EmployeeId = employeeId;
                    model.Name = obj.FamilyMemberName;
                    model.Relation = obj.RelationWithFamilyMember;
                    model.Gender = obj.FamilyMemberGender;
                    model.DateOfBirth = obj.FamilyMemberDateOfBirth;
                    model.EducationalQualification = obj.EducationalQualification;
                    model.Occupation = obj.FamilyMemberOccupation;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.IsApproved = true;
                    model.IsRejected = false;
                    model.ApprovedOrRejectedBy = 0;
                    model.ApprovalOrRejectDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeFamilyInfoApprovalProcessService.Create(model);
                }
                else
                {
                    var employeeId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;
                    var model = new EmployeeFamilyInfoApprovalProcess();
                    model.EmployeeId = employeeId;
                    model.Name = obj.FamilyMemberName;
                    model.Relation = obj.RelationWithFamilyMember;
                    model.Gender = obj.FamilyMemberGender;
                    model.DateOfBirth = obj.FamilyMemberDateOfBirth;
                    model.EducationalQualification = obj.EducationalQualification;
                    model.Occupation = obj.FamilyMemberOccupation;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.IsApproved = false;
                    model.IsRejected = false;
                    model.ApprovedOrRejectedBy = 0;
                    model.ApprovalOrRejectDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeFamilyInfoApprovalProcessService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeFamilyInfo(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var employeeId = 0;
            var employeeCode = employeeService.GetMany(p => p.IsActive == true && p.EmployeeCode == empCode).FirstOrDefault();
            if (employeeCode != null)
            {
                employeeId = Convert.ToInt32(employeeCode.EmployeeId);
            }

            var familyInfo = employeeFamilyInfoApprovalProcessService.GetMany(p => p.IsActive == true && p.EmployeeId == employeeId).ToList();
            var viewFamilyInfo = familyInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                FamilyMemberName = p.Name,
                RelationWithFamilyMember = p.Relation.Trim(),
                FamilyMemberGender = p.Gender.Trim(),
                FamilyMemberDateOfBirthShow = Convert.ToDateTime(p.DateOfBirth).ToString("dd-MMM-yyyy"),
                EducationalQualification = p.EducationalQualification,
                FamilyMemberOccupation = p.Occupation,
                IsApproved = p.IsApproved,
                IsRejected = p.IsRejected
            }).ToList();
            foreach (var item in viewFamilyInfo)
            {
                if (item.IsApproved == true)
                {
                    item.FamilyInfoStatus = "Approved";
                }

                if (item.IsRejected == true)
                {
                    item.FamilyInfoStatus = "Rejected";
                }

                if (item.IsRejected == false && item.IsApproved == false)
                {
                    item.FamilyInfoStatus = "Pending";
                }
            }
            var currentPageRecords = viewFamilyInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewFamilyInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }


        public JsonResult GetEmployeeFamilyInfoApproval(int jtStartIndex, int jtPageSize, string jtSorting)
        {

            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewFamilyInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                viewFamilyInfo = (from r in employeeFamilyInfoService.GetMany(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false)
                                  join e in employeeService.GetMany(b => b.IsActive == true) on r.EmployeeId equals e.EmployeeId
                                  select new EmployeeOtherInformationViewModel()
                                  {
                                      Id = Convert.ToInt32(r.FamilyInfoId),
                                      EmployeeId = r.EmployeeId,
                                      EmployeeCode = e.EmployeeCode,
                                      FamilyMemberName = r.Name,
                                      RelationWithFamilyMember = r.Relation.Trim(),
                                      FamilyMemberGender = r.Gender.Trim(),
                                      FamilyMemberDateOfBirthShow = Convert.ToDateTime(r.DateOfBirth).ToString("dd-MMM-yyyy"),
                                      EducationalQualification = r.EducationalQualification,
                                      FamilyMemberOccupation = r.Occupation,
                                  }).ToList();
                //return list;
                //var familyInfo = employeeFamilyInfoService.GetMany(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false).ToList();



                //viewFamilyInfo = familyInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                //{
                //    Id = Convert.ToInt32(p.FamilyInfoId),
                //    //EmployeeId = p.EmployeeId,
                //    //EmployeeCode=
                //    FamilyMemberName = p.Name,
                //    RelationWithFamilyMember = p.Relation.Trim(),
                //    FamilyMemberGender = p.Gender.Trim(),
                //    FamilyMemberDateOfBirthShow = Convert.ToDateTime(p.DateOfBirth).ToString("dd-MMM-yyyy"),
                //    EducationalQualification = p.EducationalQualification,
                //    FamilyMemberOccupation = p.Occupation,
                //}).ToList();
                currentPageRecords = viewFamilyInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewFamilyInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }


        [HttpPost]
        public JsonResult UpdateEmployeeFamilyInfo(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeFamilyInfoApprovalProcessService.GetById(obj.Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Family info status pending is applicable for edit, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();
                if (loginId != null)
                {
                    model.Name = obj.FamilyMemberName;
                    model.Relation = obj.RelationWithFamilyMember;
                    model.Gender = obj.FamilyMemberGender;
                    model.DateOfBirth = obj.FamilyMemberDateOfBirth;
                    model.EducationalQualification = obj.EducationalQualification;
                    model.Occupation = obj.FamilyMemberOccupation;
                    model.IsApproved = true;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeFamilyInfoApprovalProcessService.Update(model);
                }
                else
                {
                    model.Name = obj.FamilyMemberName;
                    model.Relation = obj.RelationWithFamilyMember;
                    model.Gender = obj.FamilyMemberGender;
                    model.DateOfBirth = obj.FamilyMemberDateOfBirth;
                    model.EducationalQualification = obj.EducationalQualification;
                    model.Occupation = obj.FamilyMemberOccupation;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeFamilyInfoApprovalProcessService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteEmployeeFamilyInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeFamilyInfoApprovalProcessService.GetById(Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Family info status pending is applicable for delete, Delete Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                model.IsActive = false;
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeFamilyInfoApprovalProcessService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ApproveEmployeeFamilyInfo(int Id)
        {
            var result = 0;
            var message = "";

            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {
                var model = employeeFamilyInfoService.GetById(Id);
                model.IsApproved = true;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeFamilyInfoService.Update(model);
                result = 1;
                message = "Approved successfully";

                //var entity = new EmployeeFamilyInfo();
                //entity.EmployeeId = model.EmployeeId;
                //entity.Name = model.Name;
                //entity.Relation = model.Relation;
                //entity.Gender = model.Gender;
                //entity.DateOfBirth = model.DateOfBirth;
                //entity.EducationalQualification = model.EducationalQualification;
                //entity.Occupation = model.Occupation;
                //entity.IsActive = model.IsActive;
                //entity.DateOfBirth = model.DateOfBirth;
                //entity.IsActive = model.IsActive;
                //entity.InActiveDate = model.InActiveDate;
                //entity.CreateUser = model.CreateUser;
                //entity.CreateDate = model.CreateDate;
                //entity.UpdateUser = model.UpdateUser;
                //entity.UpdateDate = model.UpdateDate;
                //employeeFamilyInfoService.Create(entity);
                //scope.Complete();
            }
            catch (Exception ex)
            {
                //scope.Dispose();
                result = 0;
                message = ex.InnerException.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RejectEmployeeFamilyInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeFamilyInfoService.GetById(Id);
                model.IsRejected = true;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeFamilyInfoService.Update(model);
                result = 1;
                message = "Rejected successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Rejection failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveMaritalStatus(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();

                if (loginId != null)
                {
                    //var employeeId = loginId.EmployeeId;
                    var employeeCode = loginId.EmployeeCode;

                    var model = new EmployeeMaritalStatusApproval();
                    model.EmployeeCode = employeeCode;
                    model.MaritalStatus = obj.MaritalStatus;
                    model.IsActive = true;
                    model.IsApproved = true;
                    model.IsRejected = false;
                    model.ApprovedOrRejectedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.ApprovalOrRejectionDate = DateTime.UtcNow;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeMaritalStatusApprovalService.Create(model);
                }
                else
                {
                    var model = new EmployeeMaritalStatusApproval();
                    model.EmployeeCode = obj.EmployeeCode;
                    model.MaritalStatus = obj.MaritalStatus;
                    model.IsActive = true;
                    model.IsApproved = false;
                    model.IsRejected = false;
                    model.ApprovedOrRejectedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.ApprovalOrRejectionDate = DateTime.UtcNow;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeMaritalStatusApprovalService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeMaritalStatus(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var maritalInfo =
                employeeMaritalStatusApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeCode == empCode)
                    .ToList();
            var viewmaritalInfo = maritalInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                MaritalId = p.MaritalId,
                EmployeeCode = p.EmployeeCode,
                MaritalStatus = p.MaritalStatus,
                IsApproved = p.IsApproved,
                IsRejected = p.IsRejected
            }).ToList();
            foreach (var item in viewmaritalInfo)
            {
                if (item.IsApproved == true)
                {
                    item.StatusApprovalOrRejection = "Approved";
                }

                if (item.IsRejected == true)
                {
                    item.StatusApprovalOrRejection = "Rejected";
                }

                if (item.IsRejected == false && item.IsApproved == false)
                {
                    item.StatusApprovalOrRejection = "Pending";
                }
            }
            var currentPageRecords = viewmaritalInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewmaritalInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }


        public JsonResult GetEmployeeMaritalStatusApproval(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewmaritalInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var maritalInfo =
                employeeMaritalStatusApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false)
                    .ToList();
                viewmaritalInfo = maritalInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    MaritalId = p.MaritalId,
                    EmployeeCode = p.EmployeeCode,
                    MaritalStatus = p.MaritalStatus,
                }).ToList();
                currentPageRecords = viewmaritalInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewmaritalInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }


        [HttpPost]
        public JsonResult UpdateMaritalStatus(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeMaritalStatusApprovalService.GetById(obj.MaritalId);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only Marital status pending is applicable for edit, Update Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();
                if (loginId != null)
                {
                    model.MaritalStatus = obj.MaritalStatus;
                    model.IsApproved = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeMaritalStatusApprovalService.Update(model);
                }
                else
                {
                    model.MaritalStatus = obj.MaritalStatus;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeMaritalStatusApprovalService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Update failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeeMaritalStatus(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeMaritalStatusApprovalService.GetById(Id);
                if (model.IsApproved == true || model.IsRejected == true)
                {
                    result = 0;
                    message = "Only marital status pending is applicable for delete, Delete Denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeMaritalStatusApprovalService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ApproveEmployeeMaritalStatus(int Id)
        {
            var result = 0;
            var message = "";
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var model = employeeMaritalStatusApprovalService.GetById(Id);
                    model.IsApproved = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeMaritalStatusApprovalService.Update(model);

                    var empCode = model.EmployeeCode;

                    var changeMaritalStatusByEmpCode = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                    if (changeMaritalStatusByEmpCode != null)
                    {
                        changeMaritalStatusByEmpCode.MaritalStatus = model.MaritalStatus;
                        employeeService.Update(changeMaritalStatusByEmpCode);
                    }
                    result = 1;
                    message = "Approved successfully";
                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    result = 0;
                    message = "Approval failed";
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RejectEmployeeMaritalStatus(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeMaritalStatusApprovalService.GetById(Id);
                model.IsRejected = true;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeMaritalStatusApprovalService.Update(model);
                result = 1;
                message = "Rejected successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Rejection failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePrevWorkExperience(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();

                if (loginId != null)
                {
                    //var employeeId = loginId.EmployeeId;
                    var employeeCode = loginId.EmployeeCode;

                    var model = new EmployeePreviousWorkExperience();
                    model.EmployeeCode = employeeCode;
                    model.OrganizationName = obj.OrganizationName;
                    model.Department = obj.PreviousDepartment;
                    model.Designation = obj.PreviousDesignation;
                    model.JoiningDate = obj.PrevJoiningDate;
                    model.ReleaseDate = obj.PrevReleaseDate;
                    model.ExperienceYear = obj.ExperienceYear;
                    model.ExperienceMonth = obj.ExperienceMonth;

                    model.SupervisorName = obj.PreSupervisorName;
                    model.SupervisorMobileNo = obj.PreSupervisorMobileNo;
                    model.LeaveReason = obj.PreLeaveReason;

                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;       
                    


                    employeePreviousWorkExperienceService.Create(model);
                }
                else
                {
                    var model = new EmployeePreviousWorkExperience();
                    model.EmployeeCode = obj.EmployeeCode;
                    model.OrganizationName = obj.OrganizationName;
                    model.Department = obj.PreviousDepartment;
                    model.Designation = obj.PreviousDesignation;
                    model.JoiningDate = obj.PrevJoiningDate;
                    model.ReleaseDate = obj.PrevReleaseDate;
                    model.ExperienceYear = obj.ExperienceYear;
                    model.ExperienceMonth = obj.ExperienceMonth;

                    model.SupervisorName = obj.PreSupervisorName;
                    model.SupervisorMobileNo = obj.PreSupervisorMobileNo;
                    model.LeaveReason = obj.PreLeaveReason;

                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePreviousWorkExperienceService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeePreviousWorkExperience(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var expInfo =
                employeePreviousWorkExperienceService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeCode == empCode)
                    .ToList();
            var viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                OrgId = p.OrgId,
                EmployeeCode = p.EmployeeCode,
                OrganizationName = p.OrganizationName,
                PreviousDepartment = p.Department,
                PreviousDesignation = p.Designation,
                PrevJoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                PrevReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy"),

                PreSupervisorName = p.SupervisorName,
                PreSupervisorMobileNo = p.SupervisorMobileNo,
                PreLeaveReason = p.LeaveReason,

            ExperienceYear = p.ExperienceYear,
                ExperienceMonth = p.ExperienceMonth
            }).ToList();
            var currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewExpInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }

        [HttpPost]
        public JsonResult UpdatePrevWorkExperience(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                    employeeInformationApprovalService.GetAll()
                        .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                        .FirstOrDefault();
                if (loginId != null)
                {
                    var model = employeePreviousWorkExperienceService.GetById(obj.OrgId);
                    model.OrganizationName = obj.OrganizationName;
                    model.Department = obj.PreviousDepartment;
                    model.Designation = obj.PreviousDesignation;
                    model.JoiningDate = obj.PrevJoiningDate;
                    model.ReleaseDate = obj.PrevReleaseDate;
                    model.ExperienceYear = obj.ExperienceYear;
                    model.ExperienceMonth = obj.ExperienceMonth;

                    model.SupervisorName = obj.PreSupervisorName;
                    model.SupervisorMobileNo = obj.PreSupervisorMobileNo;
                    model.LeaveReason = obj.PreLeaveReason;

                    model.IsApproved = true;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePreviousWorkExperienceService.Update(model);
                }
                else
                {
                    var model = employeePreviousWorkExperienceService.GetById(obj.OrgId);
                    model.OrganizationName = obj.OrganizationName;
                    model.Department = obj.PreviousDepartment;
                    model.Designation = obj.PreviousDesignation;
                    model.JoiningDate = obj.PrevJoiningDate;
                    model.ReleaseDate = obj.PrevReleaseDate;
                    model.ExperienceYear = obj.ExperienceYear;
                    model.ExperienceMonth = obj.ExperienceMonth;

                    model.SupervisorName = obj.PreSupervisorName;
                    model.SupervisorMobileNo = obj.PreSupervisorMobileNo;
                    model.LeaveReason = obj.PreLeaveReason;

                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePreviousWorkExperienceService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeePreviousWorkExperience(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePreviousWorkExperienceService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePreviousWorkExperienceService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeePreviousWorkExperienceApproval(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewExpInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var expInfo =
              employeePreviousWorkExperienceService.GetAll()
                  .Where(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false)
                  .ToList();
                viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    OrgId = p.OrgId,
                    EmployeeCode = p.EmployeeCode,
                    OrganizationName = p.OrganizationName,
                    PreviousDepartment = p.Department,
                    PreviousDesignation = p.Designation,
                    PrevJoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                    PrevReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy"),
                    ExperienceYear = p.ExperienceYear,
                    ExperienceMonth = p.ExperienceMonth
                }).ToList();
                currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewExpInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }

        public JsonResult ApproveEmployeeWorkExperience(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePreviousWorkExperienceService.GetById(Id);
                model.IsApproved = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePreviousWorkExperienceService.Update(model);
                result = 1;
                message = "Approved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectEmployeeWorkExperience(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePreviousWorkExperienceService.GetById(Id);
                model.IsRejected = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePreviousWorkExperienceService.Update(model);
                result = 1;
                message = "Rejected successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetApprovedEmployeePreviousWorkExperience(int jtStartIndex, int jtPageSize, string jtSorting, int empId)
        {
            var employee = employeeService.GetMany(p => p.IsActive == true && p.EmployeeId == empId).FirstOrDefault();
            if (employee != null)
            {
                var employeeCode = employee.EmployeeCode;

                var expInfo = employeePreviousWorkExperienceService.GetAll()
                        .Where(p => p.IsActive == true && p.IsApproved == true && p.EmployeeCode == employeeCode)
                        .ToList();
                var viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    OrgId = p.OrgId,
                    EmployeeCode = p.EmployeeCode,
                    OrganizationName = p.OrganizationName,
                    PreviousDepartment = p.Department,
                    PreviousDesignation = p.Designation,
                    PrevJoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                    PrevReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy"),
                    ExperienceYear = p.ExperienceYear,
                    ExperienceMonth = p.ExperienceMonth
                }).ToList();
                var currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewExpInfo.LongCount(), JsonRequestBehavior.AllowGet });
            }
            else
            {
                var viewExpInfo = new List<EmployeeOtherInformationViewModel>();
                var currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewExpInfo.LongCount(), JsonRequestBehavior.AllowGet });
                //return Json(new { Result = "", JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult SavePublication(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                   employeeInformationApprovalService.GetAll()
                       .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                       .FirstOrDefault();

                if (loginId != null)
                {
                    var employeeId = loginId.EmployeeId;
                    var employeeCode = loginId.EmployeeCode;

                    var model = new EmployeePublication();
                    model.EmployeeId = employeeId;
                    model.EmployeeCode = employeeCode;
                    model.PublicationName = obj.PublicationName;
                    model.PublicationDetail = obj.PublicationDetail;
                    model.IsActive = true;
                    model.IsApproved = true;
                    model.IsRejected = false;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePublicationService.Create(model);
                }
                else
                {
                    var empId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;

                    var model = new EmployeePublication();
                    model.EmployeeId = empId;
                    model.EmployeeCode = obj.EmployeeCode;
                    model.PublicationName = obj.PublicationName;
                    model.PublicationDetail = obj.PublicationDetail;
                    model.IsActive = true;
                    model.IsApproved = false;
                    model.IsRejected = false;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePublicationService.Create(model);
                }
                result = 1;
                message = "Saved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeePublicationInfo(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var publicationInfo =
                employeePublicationService.GetMany(p => p.IsActive == true && p.EmployeeCode == empCode).ToList();
            var viewPublicationInfo = publicationInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                PublicationId = p.PublicationId,
                EmployeeId = p.EmployeeId,
                EmployeeCode = p.EmployeeCode,
                PublicationName = p.PublicationName,
                PublicationDetail = p.PublicationDetail,
            }).ToList();
            var currentPageRecords = viewPublicationInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewPublicationInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdatePublication(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var loginId =
                   employeeInformationApprovalService.GetAll()
                       .Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode)
                       .FirstOrDefault();

                if (loginId != null)
                {
                    var model = employeePublicationService.GetById(obj.PublicationId);
                    model.PublicationName = obj.PublicationName;
                    model.PublicationDetail = obj.PublicationDetail;
                    model.IsActive = true;
                    model.IsApproved = true;
                    model.IsRejected = false;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePublicationService.Update(model);
                }
                else
                {
                    var model = employeePublicationService.GetById(obj.PublicationId);
                    model.PublicationName = obj.PublicationName;
                    model.PublicationDetail = obj.PublicationDetail;
                    model.IsActive = true;
                    model.IsApproved = false;
                    model.IsRejected = false;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeePublicationService.Update(model);
                }
                result = 1;
                message = "Updated successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeePublicationInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePublicationService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePublicationService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPublicationApproval(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewPublicationInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var publicationInfo =
               employeePublicationService.GetAll()
                   .Where(p => p.IsActive == true && p.IsApproved == false && p.IsRejected == false)
                   .ToList();
                viewPublicationInfo = publicationInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    PublicationId = p.PublicationId,
                    EmployeeCode = p.EmployeeCode,
                    PublicationName = p.PublicationName,
                    PublicationDetail = p.PublicationDetail
                }).ToList();
                currentPageRecords = viewPublicationInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new
            {
                Result = "OK",
                Records = currentPageRecords,
                TotalRecordCount = viewPublicationInfo.LongCount(),
                JsonRequestBehavior.AllowGet
            });
        }

        public JsonResult ApproveEmployeePublication(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePublicationService.GetById(Id);
                model.IsApproved = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePublicationService.Update(model);
                result = 1;
                message = "Approved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RejectEmployeePublication(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeePublicationService.GetById(Id);
                model.IsRejected = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeePublicationService.Update(model);
                result = 1;
                message = "Rejected successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkingExperience(string joiningDate, string releaseDate)
        {
            var result = 0;
            int years = 0;
            int months = 0;
            try
            {
                DateTime joiningDateNew = Convert.ToDateTime(joiningDate);
                DateTime releaseDateNew = Convert.ToDateTime(releaseDate);

                TimeSpan span = releaseDateNew - joiningDateNew;

                years = span.Days / 365;
                months = (span.Days % 365) / 30;
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
            }

            return Json(new { result = result, Year = years, Month = months }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult SaveCurrentOfficeEmployeeRelation(EmployeeOtherInformationViewModel obj)
        {
            var result = 0;
            var message = "";


            try
            {
                var loginId = employeeInformationApprovalService.GetMany(p => p.IsActive == true && p.EmployeeId == SessionHelper.LoggedInEmployeeID).FirstOrDefault();
                var empId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;

                var isExists = currentOrganizationRelationshipService.GetMany(x => x.IsActive == true && x.EmployeeId == empId && x.DepartmentId == obj.COEDepartmentId && x.DesignationId == obj.COEDesignationId && x.EmployeeName == obj.COEmployeeName);
                if (isExists.Count() == 0)
                {
                    var model = new CurrentOrganizationRelationship();
                    model.OfficeId = obj.OfficeId;
                    model.EmployeeCode = obj.EmployeeCode;
                    model.EmployeeId = empId;
                    model.DepartmentId = obj.COEDepartmentId;
                    model.DesignationId = obj.COEDesignationId;
                    model.EmployeeName = obj.COEmployeeName;
                    model.RelationId = obj.COERelationId;

                    model.IsRejected = false;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;


                    if (loginId != null)
                    {
                        model.IsApproved = true;
                    }
                    else
                    {
                        model.IsApproved = false;
                    }
                    currentOrganizationRelationshipService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                }
                else {
                    result = 0;
                    message = "Duplicate Employee, Save denied";
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetCurrentOfficeEmployeeRelationInfo(int jtStartIndex, int jtPageSize, string jtSorting, string empCode)
        {
            var param = new { EmployeeCode = empCode };
            var currentOfficeEmployeeRelationInfo = employeeSpService.GetDataWithParameter(param, "emp.SP_GetCurrentOfficeEmployeeRelationInfo");

            var viewCurrentOfficeEmployeeRelationInfo =
                currentOfficeEmployeeRelationInfo.Tables[0].AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                {
                    SelfOrgRelationId = p.Field<int>("SelfOrgRelationId"),
                    //OfficeTypeId = p.Field<int>("OfficeTypeId"),
                    OfficeId = p.Field<int>("OfficeId"),
                    OfficeName = p.Field<string>("OfficeName"),
                    EmployeeId = p.Field<long>("EmployeeId"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    COEDepartmentId = p.Field<int>("DepartmentId"),
                    COEDepartmentName = p.Field<string>("DepartmentName"),
                    COEDesignationId = p.Field<int>("DesignationId"),
                    COEDesignationName = p.Field<string>("DesignationName"),
                    EmployeeName = p.Field<string>("EmployeeName"),
                    COERelationId = p.Field<int>("RelationId"),
                    COERelationName = p.Field<string>("RelationName")
                }).ToList();
            var currentPageRecords = viewCurrentOfficeEmployeeRelationInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewCurrentOfficeEmployeeRelationInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult DeleteCurrentOfficeEmployeeRelationInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = currentOrganizationRelationshipService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                currentOrganizationRelationshipService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception ex)
            {

                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetCurrentOfficeEmployeeRelationApproval(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var currentPageRecords = new List<EmployeeOtherInformationViewModel>();
            var viewCurrentOfficeEmployeeRelationInfo = new List<EmployeeOtherInformationViewModel>();
            var validEmpId = SessionHelper.LoggedInEmployeeID;
            var checkValidUserForApproval = employeeInformationApprovalService.GetAll()
                    .Where(p => p.IsActive == true && p.EmployeeId == validEmpId)
                    .FirstOrDefault();
            if (checkValidUserForApproval != null)
            {
                var currentOfficeEmployeeRelationInfo = employeeSpService.GetDataWithoutParameter("emp.SP_GetCurrentOfficeEmployeeRelationInfoForApproval");

                viewCurrentOfficeEmployeeRelationInfo =
                   currentOfficeEmployeeRelationInfo.Tables[0].AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
                   {
                       SelfOrgRelationId = p.Field<int>("SelfOrgRelationId"),
                       OfficeId = p.Field<int>("OfficeId"),
                       OfficeName = p.Field<string>("OfficeName"),
                       EmployeeId = p.Field<long>("EmployeeId"),
                       EmployeeCode = p.Field<string>("EmployeeCode"),
                       COEDepartmentId = p.Field<int>("DepartmentId"),
                       COEDepartmentName = p.Field<string>("DepartmentName"),
                       COEDesignationId = p.Field<int>("DesignationId"),
                       COEDesignationName = p.Field<string>("DesignationName"),
                       EmployeeName = p.Field<string>("EmployeeName"),
                       COERelationId = p.Field<int>("RelationId"),
                       COERelationName = p.Field<string>("RelationName")
                   }).ToList();
                currentPageRecords = viewCurrentOfficeEmployeeRelationInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewCurrentOfficeEmployeeRelationInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult ApproveCurrentOfficeEmployeeRelationInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = currentOrganizationRelationshipService.GetById(Id);
                model.IsApproved = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                currentOrganizationRelationshipService.Update(model);
                result = 1;
                message = "Approved successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RejectCurrentOfficeEmployeeRelationInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = currentOrganizationRelationshipService.GetById(Id);
                model.IsRejected = true;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                currentOrganizationRelationshipService.Update(model);
                result = 1;
                message = "Rejected successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        #endregion






        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            gHRMDBContext entities = new gHRMDBContext();
            var customers = (from Country in entities.Countries
                             where Country.CountryName.StartsWith(prefix)
                             select new
                             {
                                 label = Country.CountryName,
                                 val = Country.CountryId
                             }).ToList();

            return Json(customers);
        }

        [HttpPost]
        public ActionResult EmployeeOtherInfo(string CountryName, string CountryId)
        {
            ViewBag.Message = "CountryName: " + CountryName + " CountryId: " + CountryId;
            return View();
        }

    }
}