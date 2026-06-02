#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Text;
using System.Data.Entity.Validation;
using System.Transactions;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using gHRM.Web.Helpers;
using gHRM.Web.Filters;
using gHRM.Web.ViewModels;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Web.CommonDropdown;
using gHRM.Core.Utilities.Constants;
using gHRM.Service.TimeKeeping;
using gHRM.Core.Utilities.Enums;
using gHRM.Service.Payroll;
using System.Threading.Tasks;
using gHRM.Data.DBDetailModels.Employee;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        #region variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeAddressService employeeAddressService;
        private readonly ICountryService countryService;
        private readonly IDistrictService districtService;
        private readonly ILgThanaService thanaService;
        private readonly IUnionService unionService;
        private readonly IEmployeeEducationService employeeEducationService;
        private readonly IStateOrProvinceService sateOrProvinceService;
        private readonly IEmployeeReferenceService employeeReferenceService;
        private readonly IEmployeeFamilyInfoService employeeFamilyInfoService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeTransferService employeeTransferService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeOtherQualificationService employeeOtherQualificationService;
        private readonly IEmployeeEmergencyContactService employeeEmergencyContactService;
        private readonly IEmployeeMedicalInfoService employeeMedicalInfoService;
        private readonly IEducationConcentrationService educationConcentrationService;
        private readonly IEducationDegreeService educationDegreeService;
        private readonly IEmployeeStatusHistoryService employeeStatusHistoryService;
        private readonly IFamilyRelationService familyRelationService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IEmployeeFileAttachemntService employeeFileAttachemntService;
        private readonly IEmployeeGuarantorInformationService employeeGuarantorInformationService;
        private readonly IEmployeeGuarantorTranInformationService employeeGuarantorTranInformationService;
        private readonly IView_EmployeeGuarantorInformationService view_EmployeeGuarantorInformationService;
        private readonly IGuarantorRelationshipService guarantorRelationshipService;
        private readonly IOccupationService occupationService;
        private readonly IReceivedCertificatesService receivedCertificatesService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IView_EmployeeTrainingService view_EmployeeTrainingService;
        private readonly IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService;
        private readonly ILinkWithEmployeeService linkWithEmployeeService;
        private readonly IInternalOrganizationService internalOrganizationService;
        private readonly IWorkExperienceWithInterOrganizationService workExperienceWithInterOrganizationService;
        private readonly IView_TimeKeepingRosterService view_TimeKeepingRosterService;
        private readonly ITimeKeepingRosterService timeKeepingRosterService;
        private readonly IEmployeeRosterScheduleService employeeRosterScheduleService;
        private readonly IEmployeeInformationApprovalService employeeInformationApprovalService;
        private readonly IEmployeeSignatureDesignationService employeeSignatureDesignationService;
        private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeePublicationService employeePublicationService;
        private readonly IEmployeeSupervisorService employeeSupervisorService;
        private readonly ICompanyService companyService;
        private readonly IAspNetRoleService aspNetRoleService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IRoasterEmployeeScheduleService roasterEmployeeScheduleService;
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;
        private readonly IKeyCloakService keyCloakService;
        private readonly IOfficeDesignationService officeDesignationService;

        private readonly IOfficeTypeService officeTypeService;

        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        private static DataSet empList;
        private static IEnumerable<Office> officelist;

        private static IEnumerable<StateOrProvince> stateList;
        private static IEnumerable<District> districtList;
        private static IEnumerable<LgThana> thanaList;
        private static IEnumerable<LgUnion> unionList;

        public EmployeeController(
              IEducationDegreeService educationDegreeService
            , IEducationConcentrationService educationConcentrationService
            , IEmployeeService employeeService
            , IEmployeeDepartmentService employeeDepartmentService
            , IEmployeeAddressService employeeAddressService
            , ICountryService countryService
            , IDistrictService districtService
            , ILgThanaService thanaService
            , IUnionService unionService
            , IEmployeeEducationService employeeEducationService
            , IStateOrProvinceService sateOrProvinceService
            , IEmployeeReferenceService employeeReferenceService
            , IEmployeeFamilyInfoService employeeFamilyInfoService
            , IOfficeService officeService
            , IEmployeeTransferService employeeTransferService
            , IEmployeeSPService employeeSPService
            , IEmployeeOtherQualificationService employeeOtherQualificationService
            , IEmployeeEmergencyContactService employeeEmergencyContactService
            , IEmployeeMedicalInfoService employeeMedicalInfoService
            , IEmployeeStatusHistoryService employeeStatusHistoryService
            , IFamilyRelationService familyRelationService
            , IEmployeeStatusService employeeStatusService
            , IDocumentTypeService documentTypeService
            , IEmployeeFileAttachemntService employeeFileAttachemntService
            , IEmployeeGuarantorInformationService employeeGuarantorInformationService
            , IEmployeeGuarantorTranInformationService employeeGuarantorTranInformationService
            , IGuarantorRelationshipService guarantorRelationshipService
            , IOccupationService occupationService
            , IReceivedCertificatesService receivedCertificatesService
            , IView_EmployeeGuarantorInformationService view_EmployeeGuarantorInformationService
            , IEmployeeTrainingService employeeTrainingService
            , IView_EmployeeTrainingService view_EmployeeTrainingService
            , IEmployeeOfficeVisitInformationService employeeOfficeVisitInformationService
            , ILinkWithEmployeeService linkWithEmployeeService
            , IInternalOrganizationService internalOrganizationService
            , IWorkExperienceWithInterOrganizationService workExperienceWithInterOrganizationService
            , IView_TimeKeepingRosterService view_TimeKeepingRosterService
            , ITimeKeepingRosterService timeKeepingRosterService
            , IEmployeeRosterScheduleService employeeRosterScheduleService
            , IEmployeeInformationApprovalService employeeInformationApprovalService
            , IEmployeeSignatureDesignationService employeeSignatureDesignationService
            , IEmployeeDepartmentSectionService employeeDepartmentSectionService
            , IEmployementTypeService employementTypeService
            , IEmployeePublicationService employeePublicationService
            , IEmployeeSupervisorService employeeSupervisorService
            , ICompanyService companyService
            , IAspNetRoleService aspNetRoleService
            , IAspNetUserService aspNetUserService
            , IRoasterEmployeeScheduleService roasterEmployeeScheduleService
            , ICompanyWisePayrollConfigService companyWisePayrollConfigService
            , IKeyCloakService keyCloakService
            , IOfficeDesignationService officeDesignationService
            , IOfficeTypeService officeTypeService
            )
        {
            this.educationDegreeService = educationDegreeService;
            this.educationConcentrationService = educationConcentrationService;
            this.employeeService = employeeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeAddressService = employeeAddressService;
            this.countryService = countryService;
            this.districtService = districtService;
            this.thanaService = thanaService;
            this.unionService = unionService;
            this.employeeEducationService = employeeEducationService;
            this.sateOrProvinceService = sateOrProvinceService;
            this.employeeReferenceService = employeeReferenceService;
            this.employeeFamilyInfoService = employeeFamilyInfoService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeTransferService = employeeTransferService;
            this.employeeSPService = employeeSPService;
            this.employeeOtherQualificationService = employeeOtherQualificationService;
            this.employeeEmergencyContactService = employeeEmergencyContactService;
            this.employeeMedicalInfoService = employeeMedicalInfoService;
            this.employeeStatusHistoryService = employeeStatusHistoryService;
            this.familyRelationService = familyRelationService;
            this.employeeStatusService = employeeStatusService;
            this.documentTypeService = documentTypeService;
            this.employeeFileAttachemntService = employeeFileAttachemntService;
            this.employeeGuarantorInformationService = employeeGuarantorInformationService;
            this.employeeGuarantorTranInformationService = employeeGuarantorTranInformationService;
            this.guarantorRelationshipService = guarantorRelationshipService;
            this.occupationService = occupationService;
            this.receivedCertificatesService = receivedCertificatesService;
            this.view_EmployeeGuarantorInformationService = view_EmployeeGuarantorInformationService;
            this.employeeTrainingService = employeeTrainingService;
            this.view_EmployeeTrainingService = view_EmployeeTrainingService;
            this.employeeOfficeVisitInformationService = employeeOfficeVisitInformationService;
            this.linkWithEmployeeService = linkWithEmployeeService;
            this.internalOrganizationService = internalOrganizationService;
            this.workExperienceWithInterOrganizationService = workExperienceWithInterOrganizationService;
            this.view_TimeKeepingRosterService = view_TimeKeepingRosterService;
            this.timeKeepingRosterService = timeKeepingRosterService;
            this.employeeRosterScheduleService = employeeRosterScheduleService;
            this.employeeInformationApprovalService = employeeInformationApprovalService;
            this.employeeSignatureDesignationService = employeeSignatureDesignationService;
            this.employeeDepartmentSectionService = employeeDepartmentSectionService;
            this.employementTypeService = employementTypeService;
            this.employeePublicationService = employeePublicationService;
            this.employeeSupervisorService = employeeSupervisorService;
            this.companyService = companyService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserService = aspNetUserService;
            this.roasterEmployeeScheduleService = roasterEmployeeScheduleService;
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;
            this.keyCloakService = keyCloakService;
            this.officeDesignationService = officeDesignationService;


            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion


        #region Office
        //Office Search
        public JsonResult GetHOOfficeList()
        {
            if (LoggedInOfficeType == 1)
            {
                //var HOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);

                if (officelist == null || officelist.Count() == 0)
                    officelist = officeService.GetAll();

                var HOOfficeList = from pa in officelist
                                   where pa.OfficeTypeId == 1
                                   orderby pa.OfficeCode
                                   select pa;

                // var HOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);

                var viewHOOffice = HOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var hoOffice_items = new List<SelectListItem>();
                if (viewHOOffice.ToList().Count > 0)
                {
                    hoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                hoOffice_items.AddRange(viewHOOffice);
                return Json(hoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var hoOffice_items = new List<SelectListItem>();
                return Json(hoOffice_items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHOOfficeListDept()
        {

            //var HOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);
            //var HOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);

            if (officelist == null || officelist.Count() == 0)
                officelist = officeService.GetAll();

            var HOOfficeList = from pa in officelist
                               where pa.OfficeTypeId == 1
                               orderby pa.OfficeCode
                               select pa;

            var viewHOOffice = HOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });
            var hoOffice_items = new List<SelectListItem>();
            if (viewHOOffice.ToList().Count > 0)
            {
                hoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            hoOffice_items.AddRange(viewHOOffice);
            return Json(hoOffice_items, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetDepartmentList()
        {
            if (LoggedInOfficeType == 1)
            {
                //var HOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);
                var HOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 1).OrderBy(x => x.OfficeCode);

                var viewHOOffice = HOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var hoOffice_items = new List<SelectListItem>();
                if (viewHOOffice.ToList().Count > 0)
                {

                    hoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });

                    hoOffice_items.Add(new SelectListItem() { Text = "0001 - MD", Value = "1" });
                    hoOffice_items.Add(new SelectListItem() { Text = "0002 - Acting MD", Value = "2" });
                    hoOffice_items.Add(new SelectListItem() { Text = "0003 - GM", Value = "3" });

                }
                hoOffice_items.AddRange(viewHOOffice);
                return Json(hoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var hoOffice_items = new List<SelectListItem>();
                return Json(hoOffice_items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetZOOfficeList()
        {
            if (LoggedInOfficeType == 1)
            {
                //var ZOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 2).OrderBy(x => x.OfficeCode);

                if (officelist == null || officelist.Count() == 0)
                    officelist = officeService.GetAll();

                var ZOOfficeList = from pa in officelist
                                   where pa.OfficeTypeId == 2
                                   orderby pa.OfficeCode
                                   select pa;


                // var ZOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 2).OrderBy(x => x.OfficeCode);

                var viewZOOffice = ZOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var zoOffice_items = new List<SelectListItem>();
                if (viewZOOffice.ToList().Count > 0)
                {
                    zoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                zoOffice_items.AddRange(viewZOOffice);
                return Json(zoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var ZOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 2);

                // var ZOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 2);

                if (officelist == null || officelist.Count() == 0)
                    officelist = officeService.GetAll();

                var ZOOfficeList = from pa in officelist
                                   where pa.OfficeTypeId == 2
                                   orderby pa.OfficeCode
                                   select pa;


                if (LoggedInOfficeType == 2) // ZONAL OFFICE
                {
                    ZOOfficeList = ZOOfficeList.Where(W => W.OfficeId == LoggedInOfficeID).OrderBy(x => x.OfficeCode);
                }
                var viewZOOffice = ZOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var zoOffice_items = new List<SelectListItem>();
                if (viewZOOffice.ToList().Count > 0)
                {
                    zoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                zoOffice_items.AddRange(viewZOOffice);
                return Json(zoOffice_items, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetZoneName()//Return Zone List Without "Zonal Office" Words //kk
        {
            List<OfficeViewModel> List_ViewModel = new List<OfficeViewModel>();
            //var param = new { AndCondition = "" };

            var List = employeeSPService.GetDataWithoutParameter("sp_GetZoneList");

            DataTable dt = new DataTable();
            dt = List.Tables[0];

            if (LoggedInOfficeType == 2)
            {
                var rows = from row in List.Tables[0].AsEnumerable()
                           where row.Field<int>("OfficeId") == LoggedInOfficeID
                           select row;

                if (rows.Count() > 0)
                {
                    dt = rows.CopyToDataTable();
                }
            }


            List_ViewModel = dt.AsEnumerable()
            .Select(row => new OfficeViewModel
            {
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("Zone")

            }).ToList();



            var Zones = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeCode.ToString(),
                Text = string.Format("{0}", x.OfficeName)
            });

            var Zones_items = new List<SelectListItem>();
            if (Zones.ToList().Count > 0)
            {
                Zones_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Zones_items.AddRange(Zones);
            return Json(Zones_items, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAOOfficeList(string zoCode)
        {
            if (LoggedInOfficeType == 1)
            {
                var AOOfficeList = officeService.GetAOOfc(zoCode.ToString()).OrderBy(x => x.OfficeCode);
                var viewAOOffice = AOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var aoOffice_items = new List<SelectListItem>();
                if (viewAOOffice.ToList().Count > 0)
                {
                    aoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                aoOffice_items.AddRange(viewAOOffice);
                return Json(aoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AOOfficeList = officeService.GetAOOfc(zoCode.ToString()).OrderBy(x => x.OfficeCode);
                // if (LoggedInOfficeType == 4) // AREA
                // {
                //   AOOfficeList = AOOfficeList.Where(W => W.OfficeId == 2699);
                //}
                var viewAOOffice = AOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var aoOffice_items = new List<SelectListItem>();
                if (viewAOOffice.ToList().Count > 0)
                {
                    aoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                aoOffice_items.AddRange(viewAOOffice);
                return Json(aoOffice_items, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetBOOfficeList(string aoCode)
        {
            if (aoCode != "")
            {
                var BOOfficeCode = officeService.GetById(Convert.ToInt32(aoCode)).OfficeCode;
                if (LoggedInOfficeType == 1)
                {

                    var BOOfficeList = officeService.GetBOOfc(aoCode).OrderBy(x => x.OfficeName);
                    var viewBOOffice = BOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.OfficeId.ToString(),
                        Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                    });
                    var boOffice_items = new List<SelectListItem>();
                    if (viewBOOffice.ToList().Count > 0)
                    {
                        boOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                    }
                    boOffice_items.AddRange(viewBOOffice);
                    return Json(boOffice_items, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var BOOfficeList = officeService.GetBOOfc(aoCode);
                    if (LoggedInOfficeType == 5) // BRAANCH
                    {
                        BOOfficeList = BOOfficeList.Where(W => W.OfficeId == LoggedInOfficeID).OrderBy(x => x.OfficeCode);
                    }
                    var viewBOOffice = BOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.OfficeId.ToString(),
                        Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                    });
                    var boOffice_items = new List<SelectListItem>();
                    if (viewBOOffice.ToList().Count > 0)
                    {
                        boOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                    }
                    boOffice_items.AddRange(viewBOOffice);
                    return Json(boOffice_items, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Result = "0" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBOOfficeListByZO(string zoCode)
        {
            if (LoggedInOfficeType == 1)
            {
                var BOOfficeList = officeService.GetBOOfcByZO(zoCode).OrderBy(x => x.OfficeCode);
                var viewBOOffice = BOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var boOffice_items = new List<SelectListItem>();
                if (viewBOOffice.ToList().Count > 0)
                {
                    boOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                boOffice_items.AddRange(viewBOOffice);
                return Json(boOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var BOOfficeList = officeService.GetBOOfcByZO(zoCode);
                if (LoggedInOfficeType == 5) //BRANCH
                {
                    BOOfficeList = BOOfficeList.Where(W => W.OfficeId == LoggedInOfficeID).OrderBy(x => x.OfficeCode);
                }
                var viewBOOffice = BOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var boOffice_items = new List<SelectListItem>();
                if (viewBOOffice.ToList().Count > 0)
                {
                    boOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                boOffice_items.AddRange(viewBOOffice);
                return Json(boOffice_items, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetZAOOfficeList()
        {
            if (LoggedInOfficeType == 1)
            {
                //var ZAOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 3).OrderBy(x => x.OfficeCode);

                //var ZAOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 3).OrderBy(x => x.OfficeCode);

                if (officelist == null || officelist.Count() == 0)
                    officelist = officeService.GetAll();

                var ZAOOfficeList = from pa in officelist
                                    where pa.OfficeTypeId == 3
                                    orderby pa.OfficeCode
                                    select pa;


                var viewZAOOffice = ZAOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var zaoOffice_items = new List<SelectListItem>();
                if (viewZAOOffice.ToList().Count > 0)
                {
                    zaoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                zaoOffice_items.AddRange(viewZAOOffice);
                return Json(zaoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else if (LoggedInOfficeType == 3)
            {
                //var ZAOOfficeList = officeService.GetAll().Where(O => O.OfficeTypeId == 3 && O.OfficeId == LoggedInOfficeID).OrderBy(x => x.OfficeCode);

                //var ZAOOfficeList = officeService.GetMany(O => O.OfficeTypeId == 3 && O.OfficeId == LoggedInOfficeID).OrderBy(x => x.OfficeCode);


                if (officelist == null || officelist.Count() == 0)
                    officelist = officeService.GetAll();

                var ZAOOfficeList = from pa in officelist
                                    where pa.OfficeTypeId == 3 && pa.OfficeId == LoggedInOfficeID
                                    orderby pa.OfficeCode
                                    select pa;



                var viewZAOOffice = ZAOOfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
                });
                var zaoOffice_items = new List<SelectListItem>();
                if (viewZAOOffice.ToList().Count > 0)
                {
                    zaoOffice_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                zaoOffice_items.AddRange(viewZAOOffice);
                return Json(zaoOffice_items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var zaoOffice_items = new List<SelectListItem>();
                return Json(zaoOffice_items, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Events



        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult Index_Branch()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult Index_Addin()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;
            ViewData["EmployeeCode"] = model.EmployeeCode;

            return View(model);
        }

        public ActionResult Index_Address()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();


            return View(model);
        }
        public ActionResult JobConfirmation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            MapDropdownForProbationaryEmployee(model);
            return View(model);
        }


        public void MapDropdownForProbationaryEmployee(EmployeeViewModel model)
        {
            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Probationary", Value = "3" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Extended Probationary", Value = "4" });

            model.BloodPressureTypeList = employeeProfilelist;
        }




        public ActionResult Demo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeSearchingViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }


        public ActionResult IndexChildAction()
        {
            return View();
        }

        public ActionResult EmployeeCreateCommon(long? EmployeeId)
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["UserRole"] = aspNetUserService.GetById(Convert.ToInt32(SessionHelper.LoginUserEmployeeId)).AspNetRoles;


            string EMPLOYEE_ENTRY_HEIGHT_UNIT = GetSetting("EMPLOYEE_ENTRY_HEIGHT_UNIT");
            if (EMPLOYEE_ENTRY_HEIGHT_UNIT == "") EMPLOYEE_ENTRY_HEIGHT_UNIT = "Cm.";
            ViewData["EMPLOYEE_ENTRY_HEIGHT_UNIT"] = EMPLOYEE_ENTRY_HEIGHT_UNIT;
            ViewBag.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED = AppSetting.GetBool(AppSetting.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED, HttpContext);




            if (EmployeeId == null)
            {
                var model = new EmployeeViewModel();
                model.FirstJoiningDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.IsValidEmployeeStatus = true;

                MapDropDownList(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);
                model.EmployeeImageLink = "/Images/blank-headshot.jpg";

                model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;

                model.EmployeeName = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeName;



                string UserRoleName = aspNetRoleService.GetNameById(SessionHelper.LoggedInRoleId.ToString());
                string EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE = AppSetting.Get(AppSetting.EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE, HttpContext);
                ViewBag.IsEmployeeCodeEditAllowed = !string.IsNullOrEmpty(UserRoleName) && UserRoleName == EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE;



                string Rank = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeRank;
                int EmpRank = Convert.ToInt32(Rank);

                gHRMDBContext db = new gHRMDBContext();
                var Designation = db.OfficeDesignations.Where(z => z.OfficeDesignationId == EmpRank).Select(k => k.OffcDesignName).FirstOrDefault();

                model.Designation = Designation.ToString();

                return View(model);
            }
            else
            {
                var entity = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));

                var userLoginId = SessionHelper.LoggedInEmployeeID;
                var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
                var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == userLoginId).RoleId;

                if (superAdminRoleId == loginRoleId)
                {
                    ViewData["UserRole"] = "Super Admin";
                }

                var model = Mapper.Map<Employee, EmployeeViewModel>(entity);
                model.EmployeeStatusId = entity.EmployeeStatusId;



                var empstatus = employeeStatusService.GetById(model.EmployeeStatusId);
                model.IsValidEmployeeStatus = empstatus.IsValid;

                model.EmployeeRank = entity.EmployeeRank == null ? entity.EmployeeRank : entity.EmployeeRank.Trim();
                model.Section = entity.SectionId == null ? "" : Convert.ToString(entity.SectionId);
                model.SignatureDesignation = entity.SignatureDesignationId == null ? "" : Convert.ToString(entity.SignatureDesignationId);
                model.EmploymentType = entity.EmploymentTypeId == null ? "" : Convert.ToString(entity.EmploymentTypeId);
                model.FirstJoiningDateMsg = model.FirstJoiningDate.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = model.StatusDate.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.PermanentDate = model.PermanentDate;

                if (entity.JobExperience != null)
                {
                    string[] experience = entity.JobExperience.Split('-');
                    if (experience.Length > 2)
                    {
                        model.ExperienceYear = experience[0];
                        model.ExperienceMonth = experience[1];
                        model.ExperienceDay = experience[2];
                    }
                }
                else
                {
                    model.ExperienceYear = "0";
                    model.ExperienceMonth = "0";
                    model.ExperienceDay = "0";
                }

                if (entity.OfficeId > 0)
                {
                    var officeTypeId = officeService.GetById(Convert.ToInt32(entity.OfficeId)).OfficeTypeId;
                    if (officeTypeId == 6)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        var thirdLevelOffice = officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault();

                        if (thirdLevelOffice != null)
                            model.AreaId = Convert.ToInt32(thirdLevelOffice.OfficeId);

                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                        model.UnitId = entity.OfficeId;
                    }
                    else if (officeTypeId == 5)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        model.AreaId = entity.OfficeId;
                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                    }
                    else if (officeTypeId == 4)
                    {
                        model.ZoneId = entity.OfficeId;
                    }
                    else if (officeTypeId == 3)
                    {
                        model.ProjectId = entity.OfficeId;
                    }
                    else if (officeTypeId == 1)
                    {
                        model.HeadOfficeId = entity.OfficeId;
                    }
                }

                MapDropDownList(model);
                mapOfficeDropdownEdit(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);



                return View(model);
            }
        }

        public ActionResult EmployeeCreate(long? EmployeeId)
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["UserRole"] = aspNetRoleService.GetNameById(SessionHelper.LoggedInRoleId.ToString());

            string EMPLOYEE_ENTRY_HEIGHT_UNIT = GetSetting("EMPLOYEE_ENTRY_HEIGHT_UNIT");
            if (EMPLOYEE_ENTRY_HEIGHT_UNIT == "") EMPLOYEE_ENTRY_HEIGHT_UNIT = "Cm.";
            ViewData["EMPLOYEE_ENTRY_HEIGHT_UNIT"] = EMPLOYEE_ENTRY_HEIGHT_UNIT;
            ViewBag.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED = AppSetting.GetBool(AppSetting.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED, HttpContext);

            if (EmployeeId == null)
            {
                var model = new EmployeeViewModel();
                model.FirstJoiningDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.IsValidEmployeeStatus = true;


                if(SessionHelper.CompanyInfo.CompanyShortName == "YPSA")
                {

                    var maxEmployeeCode2 = employeeSPService.GetDataWithoutParameter("GetMaxEmployeeCode");

                    var maxEmployeeCode = maxEmployeeCode2.Tables[0].Rows[0]["MaxEmployeeCode"].ToString();

                    int i = Convert.ToInt32(maxEmployeeCode) + 1;
                    string formattedCode = i.ToString("00000");
                    try
                    {
                        model.EmployeeCode = formattedCode;
                    }
                    catch
                    {
                        model.EmployeeCode = "";
                    }
                    
                }


                MapDropDownList(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);
                model.EmployeeImageLink = "/Images/blank-headshot.jpg";
                return View(model);
            }
            else
            {
                var entity = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));

                

                var userLoginId = SessionHelper.LoggedInEmployeeID;
                var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
                var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == userLoginId).RoleId;

                if (superAdminRoleId == loginRoleId)
                {
                    ViewData["UserRole"] = "Super Admin";
                }

                var model = Mapper.Map<Employee, EmployeeViewModel>(entity);
                model.EmployeeStatusId = entity.EmployeeStatusId;
                model.OldEmployeeCode = entity.EmployeeCode;

                var empstatus = employeeStatusService.GetById(model.EmployeeStatusId);
                model.IsValidEmployeeStatus = empstatus.IsValid;

                model.EmployeeRank = entity.EmployeeRank == null ? entity.EmployeeRank : entity.EmployeeRank.Trim();
                model.Section = entity.SectionId == null ? "" : Convert.ToString(entity.SectionId);
                model.SignatureDesignation = entity.SignatureDesignationId == null ? "" : Convert.ToString(entity.SignatureDesignationId);
                model.EmploymentType = entity.EmploymentTypeId == null ? "" : Convert.ToString(entity.EmploymentTypeId);
                model.FirstJoiningDateMsg = model.FirstJoiningDate.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = model.StatusDate.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.PermanentDate = model.PermanentDate;

                if (entity.JobExperience != null)
                {
                    string[] experience = entity.JobExperience.Split('-');
                    if (experience.Length > 2)
                    {
                        model.ExperienceYear = experience[0];
                        model.ExperienceMonth = experience[1];
                        model.ExperienceDay = experience[2];
                    }
                }
                else
                {
                    model.ExperienceYear = "0";
                    model.ExperienceMonth = "0";
                    model.ExperienceDay = "0";
                }

                if (entity.OfficeId > 0)
                {
                    var officeTypeId = officeService.GetById(Convert.ToInt32(entity.OfficeId)).OfficeTypeId;
                    if (officeTypeId == 6)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        var thirdLevelOffice = officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault();

                        if (thirdLevelOffice != null)
                            model.AreaId = Convert.ToInt32(thirdLevelOffice.OfficeId);

                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                        model.UnitId = entity.OfficeId;
                    }
                    else if (officeTypeId == 5)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        model.AreaId = entity.OfficeId;
                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                    }
                    else if (officeTypeId == 4)
                    {
                        model.ZoneId = entity.OfficeId;
                    }
                    else if (officeTypeId == 3)
                    {
                        model.ProjectId = entity.OfficeId;
                    }
                    else if (officeTypeId == 1)
                    {
                        model.HeadOfficeId = entity.OfficeId;
                    }
                }

                MapDropDownList(model);
                mapOfficeDropdownEdit(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);

                return View(model);
            }
        }

        public ActionResult EmployeeCreateAddress(long? EmployeeId)
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["UserRole"] = "";
            string EMPLOYEE_ENTRY_HEIGHT_UNIT = GetSetting("EMPLOYEE_ENTRY_HEIGHT_UNIT");
            if (EMPLOYEE_ENTRY_HEIGHT_UNIT == "") EMPLOYEE_ENTRY_HEIGHT_UNIT = "Cm.";
            ViewData["EMPLOYEE_ENTRY_HEIGHT_UNIT"] = EMPLOYEE_ENTRY_HEIGHT_UNIT;
            ViewBag.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED = AppSetting.GetBool(AppSetting.EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED, HttpContext);

            if (EmployeeId == null)
            {
                var model = new EmployeeViewModel();
                model.FirstJoiningDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = DateTime.Now.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.IsValidEmployeeStatus = true;

                MapDropDownList(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);
                model.EmployeeImageLink = "/Images/blank-headshot.jpg";
                return View(model);
            }
            else
            {
                var entity = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));

                var userLoginId = SessionHelper.LoggedInEmployeeID;
                var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
                var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == userLoginId).RoleId;

                if (superAdminRoleId == loginRoleId)
                {
                    ViewData["UserRole"] = "Super Admin";
                }

                var model = Mapper.Map<Employee, EmployeeViewModel>(entity);
                model.EmployeeStatusId = entity.EmployeeStatusId;



                var empstatus = employeeStatusService.GetById(model.EmployeeStatusId);
                model.IsValidEmployeeStatus = empstatus.IsValid;

                model.EmployeeRank = entity.EmployeeRank == null ? entity.EmployeeRank : entity.EmployeeRank.Trim();
                model.Section = entity.SectionId == null ? "" : Convert.ToString(entity.SectionId);
                model.SignatureDesignation = entity.SignatureDesignationId == null ? "" : Convert.ToString(entity.SignatureDesignationId);
                model.EmploymentType = entity.EmploymentTypeId == null ? "" : Convert.ToString(entity.EmploymentTypeId);
                model.FirstJoiningDateMsg = model.FirstJoiningDate.ToString("dd-MMM-yyyy");
                model.StatusDateMsg = model.StatusDate.ToString("dd-MMM-yyyy");
                model.ServerCurrentDate = DateTime.Now;
                model.PermanentDate = model.PermanentDate;

                if (entity.JobExperience != null)
                {
                    string[] experience = entity.JobExperience.Split('-');
                    if (experience.Length > 2)
                    {
                        model.ExperienceYear = experience[0];
                        model.ExperienceMonth = experience[1];
                        model.ExperienceDay = experience[2];
                    }
                }
                else
                {
                    model.ExperienceYear = "0";
                    model.ExperienceMonth = "0";
                    model.ExperienceDay = "0";
                }

                if (entity.OfficeId > 0)
                {
                    var officeTypeId = officeService.GetById(Convert.ToInt32(entity.OfficeId)).OfficeTypeId;
                    if (officeTypeId == 6)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        var thirdLevelOffice = officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault();

                        if (thirdLevelOffice != null)
                            model.AreaId = Convert.ToInt32(thirdLevelOffice.OfficeId);

                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                        model.UnitId = entity.OfficeId;
                    }
                    else if (officeTypeId == 5)
                    {
                        var office = officeService.GetById(Convert.ToInt32(model.OfficeId));
                        model.AreaId = entity.OfficeId;
                        var secondLevelOffice = officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault();
                        if (secondLevelOffice != null)
                            model.ZoneId = Convert.ToInt32(secondLevelOffice.OfficeId);
                    }
                    else if (officeTypeId == 4)
                    {
                        model.ZoneId = entity.OfficeId;
                    }
                    else if (officeTypeId == 3)
                    {
                        model.ProjectId = entity.OfficeId;
                    }
                    else if (officeTypeId == 1)
                    {
                        model.HeadOfficeId = entity.OfficeId;
                    }
                }

                MapDropDownList(model);
                mapOfficeDropdownEdit(model);
                MapDropdownForEmployeeGuarantorInformation(model);
                MapDropdownForEmployeeTraining(model);
                MapDropdownForAttendance(model);

                return View(model);
            }
        }

        public ActionResult RetrieveGuarantorImage(int id)
        {
            byte[] cover = GetGuarantorImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/*");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
        }

        public ActionResult RetrieveImage(long id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/*");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
        }

        [HttpPost]
        public ActionResult UploadImg(HttpPostedFileBase file, string EmpId)
        {

            var Result = 0;
            var entity = employeeService.GetByEmpId(Convert.ToInt64(EmpId));

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.EmployeeImage = data;
                employeeService.Update(entity);
                Result = 1;
            }
            else
            {
                //entity.EmployeeImage = null;
                //employeeService.Update(entity);
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadGuarantorImage(HttpPostedFileBase file, string GuarantorId)
        {
            var Result = 0;
            var entity = employeeGuarantorInformationService.GetByGurId(Convert.ToInt32(GuarantorId));

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.GuarantorImage = data;
                employeeGuarantorInformationService.Update(entity);
                Result = 1;
            }
            else
            {
                //entity.EmployeeImage = null;
                //employeeService.Update(entity);
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetDeptListByOfficeType(int officeTypeId)
        {
            //var param = new { OfficeTypeId = officeTypeId };
            var departmentList = employeeSPService.GetDataWithoutParameter("basic.SP_GetDepartmentByOfficeType");
            var loadDepartment = departmentList.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Field<string>("DepartmentName"),
                Value = p.Field<int>("DepartmentId").ToString()
            }).ToList();
            return Json(loadDepartment, JsonRequestBehavior.AllowGet);
        }


        public ActionResult JobConfirmationReport(string EmpCode)
        {
            try
            {
                try
                {
                    gHRMDBContext db = new gHRMDBContext();
                    var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmpCode", Value = EmpCode });

                    PrintSSRSReport("/gHRMPlus_Reports/JobConfirmationReport", paramValues.ToArray());
                    return Content(string.Empty);



                }
                catch (Exception ex)
                {
                    return Content("<b>error</b><br />" + ex.Message);
                    // return Json(new { Result = "ERROR", Message = ex.Message });
                }


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult LoadEmployeeList([DataSourceRequest] DataSourceRequest request, string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                        sb.Append(" AND es.StatusId ='" + Status[0] + "'");
                }
                else if (Status != null && Status.Count > 1)
                {
                    string statusList = "";
                    var count = 1;
                    foreach (var status in Status)
                    {
                        if (count < Status.Count)
                            statusList = statusList + "'" + status + "', ";
                        else
                            statusList = statusList + "'" + status + "'";
                        count++;
                    }
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                if (DepartmentId != "")
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                if (Responsibility != "")
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);

                if (Section != "")
                    sb.Append(" AND eed.SectionId =" + Section);

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                int offid = 0;
                if (!string.IsNullOrEmpty(OfficeId))
                    int.TryParse(OfficeId, out offid);
                if (offid > 0 && loggedInOfficeTypeId == 1/*HO*/)
                {
                    var off = officeService.GetById(offid);
                    var officeType = officeTypeService.GetById(off.OfficeTypeId ?? 0);
                    string[] arr = { "ZO", "AR" };
                    if (arr.Contains(officeType.OfficeTypeCode))
                    {
                        if (officeType.OfficeTypeCode == "ZO")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = '" + off.OfficeCode + "')");
                        else if (officeType.OfficeTypeCode == "AR")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = '" + off.OfficeCode + "')");
                    }
                    else
                        sb.Append(" AND E.OfficeId =" + OfficeId);
                }
                else
                {
                    //if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    //{
                    //    sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                    //}
                    //else
                    //{

                    var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                    if (loggedInOfficeTypeId == 4) // ZO
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");

                    if (loggedInOfficeTypeId == 5) // area
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                        }
                        else
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = " + GetOfficeCode + ")");
                    }

                    if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");
                    if (OfficeId != "")
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                            else
                                sb.Append(" And E.OfficeId = 0 ");
                        }
                        else
                            sb.Append(" AND E.OfficeId =" + OfficeId);

                    }
                    //}
                }




                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoard");


                // KHALID
                empList = new DataSet();
                empList = employeeList;



                List_EmployeeViewModel = employeeList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeViewModel()
                {
                    SlNo = row.Field<string>("rowSl"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    DesignationName = row.Field<string>("DesignationName"),
                    OrnamentalDesignationName = row.Field<string>("OrnamentalDesignation"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    Phone = row.Field<string>("PhoneNo"),
                    EmployementTypeName = row.Field<string>("EmployementTypeName"),
                    EmployeeStatus = row.Field<string>("EmployeeStatus"),
                    StatusDateMsg = row.Field<string>("StatusDate"),
                    ImageFilePath = row.Field<string>("EmployeeImageLink"),
                    CompanyName = SessionHelper.CompanyName.ToString(),
                }).ToList();

                DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult LoadEmployeeListReport(string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                    {
                        // Split the numbers by comma
                        string[] values = Status[0].Split(',');

                        // Wrap each value in single quotes
                        string formattedValues = string.Join(", ", values.Select(v => $"'{v}'"));

                        // Construct the new condition
                        string newCondition = $"AND es.StatusId IN ({formattedValues})";

                        sb.Append(newCondition);
                    }
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
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                {
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                }
                if (DepartmentId != "")
                {
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                }
                if (Responsibility != "")
                {
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);
                }

                if (Section != "")
                {
                    sb.Append(" AND eed.SectionId =" + Section);
                }

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                int offid = 0;
                if (!string.IsNullOrEmpty(OfficeId))
                    int.TryParse(OfficeId, out offid);
                if (offid > 0 && loggedInOfficeTypeId == 1/*HO*/)
                {
                    var off = officeService.GetById(offid);
                    var officeType = officeTypeService.GetById(off.OfficeTypeId ?? 0);
                    string[] arr = { "ZO", "AR" };
                    if (arr.Contains(officeType.OfficeTypeCode))
                    {
                        if (officeType.OfficeTypeCode == "ZO")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = '" + off.OfficeCode + "')");
                        else if (officeType.OfficeTypeCode == "AR")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = '" + off.OfficeCode + "')");
                    }
                    else
                        sb.Append(" AND E.OfficeId =" + OfficeId);
                }
                else
                {
                    //if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    //{
                    //    sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                    //}
                    //else
                    //{

                    var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                    if (loggedInOfficeTypeId == 4) // ZO
                    {
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");
                    }

                    if (loggedInOfficeTypeId == 5) // area
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                        }
                        else
                        {
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = " + GetOfficeCode + ")");
                        }
                    }

                    if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                    {
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");
                    }
                    if (OfficeId != "")
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                            else
                                sb.Append(" And E.OfficeId = 0 ");
                        }
                        else
                        {
                            sb.Append(" AND E.OfficeId =" + OfficeId);
                        }

                    }
                    //}
                }




                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoard");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintReport("Employee/rpt_EmployeeListTest.rpt", employeeList.Tables[0], reportParam);
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult LoadEmployeeListReportExcel(string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                    {
                        // Split the numbers by comma
                        string[] values = Status[0].Split(',');

                        // Wrap each value in single quotes
                        string formattedValues = string.Join(", ", values.Select(v => $"'{v}'"));

                        // Construct the new condition
                        string newCondition = $"AND es.StatusId IN ({formattedValues})";

                        sb.Append(newCondition);

                    }

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
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                {
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                }
                if (DepartmentId != "")
                {
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                }
                if (Responsibility != "")
                {
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);
                }

                if (Section != "")
                {
                    sb.Append(" AND eed.SectionId =" + Section);
                }

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                int offid = 0;
                if (!string.IsNullOrEmpty(OfficeId))
                    int.TryParse(OfficeId, out offid);
                if (offid > 0 && loggedInOfficeTypeId == 1/*HO*/)
                {
                    var off = officeService.GetById(offid);
                    var officeType = officeTypeService.GetById(off.OfficeTypeId ?? 0);
                    string[] arr = { "ZO", "AR" };
                    if (arr.Contains(officeType.OfficeTypeCode))
                    {
                        if (officeType.OfficeTypeCode == "ZO")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = '" + off.OfficeCode + "')");
                        else if (officeType.OfficeTypeCode == "AR")
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = '" + off.OfficeCode + "')");
                    }
                    else
                        sb.Append(" AND E.OfficeId =" + OfficeId);
                }
                else
                {
                    //if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    //{
                    //    sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                    //}
                    //else
                    //{

                    var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                    if (loggedInOfficeTypeId == 4) // ZO
                    {
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");
                    }

                    if (loggedInOfficeTypeId == 5) // area
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                        }
                        else
                        {
                            sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.ThirdLevel = " + GetOfficeCode + ")");
                        }
                    }

                    if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                    {
                        sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");
                    }
                    if (OfficeId != "")
                    {
                        if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                        {
                            if (loggedInofficeID == Convert.ToInt32(OfficeId))
                                sb.Append(" AND E.OfficeId =" + loggedInofficeID);
                            else
                                sb.Append(" And E.OfficeId = 0 ");
                        }
                        else
                        {
                            sb.Append(" AND E.OfficeId =" + OfficeId);
                        }

                    }
                    //}
                }




                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoard");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.ExportExcelReport("Employee/rpt_EmployeeList.rpt", employeeList.Tables[0], reportParam);
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult LoadEmployeeListForJobConfirmation([DataSourceRequest] DataSourceRequest request, string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue, string Date, string DateTo)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                        sb.Append(" AND es.StatusId ='" + Status[0] + "'");
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
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                {
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                }
                if (DepartmentId != "")
                {
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                }
                if (Responsibility != "")
                {
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);
                }

                if (Section != "")
                {
                    sb.Append(" AND eed.SectionId =" + Section);
                }

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                if (loggedInOfficeTypeId == 4) // ZO
                {
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");
                }

                if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                {
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");
                }
                if (OfficeId != "")
                {
                    sb.Append(" AND E.OfficeId =" + OfficeId);
                }

                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                if (Date != "" && DateTo != "")
                {
                    sb.Append(" And  ((StatusFromDate between '" + Date + "' and '" + DateTo + "' ) or ( StatusToDate between '" + Date + "'  and '" + DateTo + "' )) ");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoardJobConfirmation");


                // KHALID
                empList = new DataSet();
                empList = employeeList;



                List_EmployeeViewModel = employeeList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeViewModel()
                {
                    SlNo = row.Field<string>("rowSl"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    DesignationName = row.Field<string>("DesignationName"),
                    OrnamentalDesignationName = row.Field<string>("OrnamentalDesignation"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    Phone = row.Field<string>("PhoneNo"),
                    EmployementTypeName = row.Field<string>("EmployementTypeName"),
                    EmployeeStatus = row.Field<string>("EmployeeStatus"),
                    StatusDateMsg = row.Field<string>("StatusDate"),
                    ImageFilePath = row.Field<string>("EmployeeImageLink")
                }).ToList();

                DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmpAddressList(string empMasterId)
        {
            try
            {
                List<EmployeeAddressViewModel> List_EmployeeAddressViewModel = new List<EmployeeAddressViewModel>();
                var addressList = employeeAddressService.GetByEmployeeId(Convert.ToInt64(empMasterId));
                foreach (var address in addressList)
                {
                    var details = new EmployeeAddressViewModel();
                    details.AddressId = address.AddressId;
                    details.EmployeeId = address.EmployeeId;
                    details.AddressType = address.AddressType;
                    details.CountryId = address.CountryId;
                    details.CountryName = countryService.GetById(address.CountryId).CountryName;
                    details.StateOrProvinceId = address.StateOrProvinceId;
                    details.StateOrProvinceName = sateOrProvinceService.GetById(address.StateOrProvinceId).Name;
                    details.AddressDetail = address.AddressDetail;
                    if (address.DistrictId.ToString() != "")
                    {
                        if (address.DistrictId > 0)
                        {
                            details.DistrictId = address.DistrictId;
                            details.DistrictName = districtService.GetById(Convert.ToInt32(string.IsNullOrEmpty(address.DistrictId.ToString()) ? "0" : address.DistrictId.ToString())).district_name_eng;
                        }
                    }
                    if (address.ThanaId.ToString() != "")
                    {
                        if (address.ThanaId > 0)
                        {
                            details.ThanaId = address.ThanaId;
                            details.ThanaName = thanaService.GetById(Convert.ToInt32(string.IsNullOrEmpty(address.ThanaId.ToString()) ? "0" : address.ThanaId.ToString())).thana_name_eng;
                        }
                    }
                    if (address.UnionId.ToString() != "")
                    {
                        if (address.UnionId > 0)
                        {
                            details.UnionId = address.UnionId;
                            details.UnionName = unionService.GetById(Convert.ToInt32(string.IsNullOrEmpty(address.UnionId.ToString()) ? "0" : address.UnionId.ToString())).union_name_eng;
                        }
                    }
                    details.StreetOrHouse = address.StreetOrHouse;
                    details.ZipCode = address.ZipCode;
                    details.PostOffice = address.PostOffice;
                    List_EmployeeAddressViewModel.Add(details);
                }

                return Json(List_EmployeeAddressViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetEmpReferenceList(string empMasterId)
        {
            try
            {
                List<EmployeeReferencesViewModel> List_EmployeeReferenceViewModel = new List<EmployeeReferencesViewModel>();
                var referenceList = employeeReferenceService.GetByEmployeeId(Convert.ToInt64(empMasterId));
                foreach (var reference in referenceList)
                {
                    var details = new EmployeeReferencesViewModel() { ReferenceId = reference.ReferenceId, EmployeeId = reference.EmployeeId, ReferenceName = reference.ReferenceName, ReferenceOccupation = reference.ReferenceOccupation, ReferenceDesignation = reference.ReferenceDesignation, ConnectionWithEmployee = reference.ConnectionWithEmployee, ContactAddress = reference.ContactAddress, Mobile = reference.Mobile, Telephone = reference.Telephone, Fax = reference.Fax, RefEmail = reference.Email, Remarks = reference.Remarks };
                    List_EmployeeReferenceViewModel.Add(details);
                }

                return Json(List_EmployeeReferenceViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult GetGrantorMoneyList(int empMasterId)
        {
            try
            {
                var param = new { EmployeeId = empMasterId };
                var grantorMoneyList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetGuarantorMoneyList");

                var List_GrantorMoney = grantorMoneyList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeGuarantorMoneyViewModel
               {
                   ID = row.Field<long>("ID"),
                   SNO = row.Field<string>("SNO"),
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   deposit = row.Field<double>("GuaranteeMoney"),
                   TransactionType = row.Field<string>("TransactionType"),
                   TransactionDate = row.Field<DateTime>("TransactionDate").ToString("dd-MMM-yyyy"),
                   TransactionAmount = row.Field<decimal>("TransactionAmount"),
                   PaymentType = row.Field<string>("PaymentType"),
                   BankName = row.Field<string>("BankName"),
                   BranchName = row.Field<string>("BranchName"),
                   AccountNo = row.Field<string>("AccountNo"),
                   CheckNo = row.Field<string>("ChequeNo"),

               }).ToList();

                return Json(List_GrantorMoney, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Result = "ERROR", Message = ex.Message });
                //return Json(new { Result = "Please add guarantor money in profile!", Message = "Please add guarantor money in profile!" });

                // ViewBag.err = "Please add guarantor money in profile!";
            }
        }


        public JsonResult GetEmpEducationList(string empMasterId)
        {
            try
            {
                List<EmployeeEducationViewModel> List_EmployeeEducationViewModel = new List<EmployeeEducationViewModel>();
                var educationList = employeeEducationService.GetByEmployeeId(Convert.ToInt64(empMasterId));
                foreach (var education in educationList)
                {
                    var educationDegree = educationDegreeService.GetMany(x => x.DegreeCode == education.DegreeTitle).FirstOrDefault();
                    var educationConcentration = educationConcentrationService.GetMany(x => x.ConcentrationCode == education.Concentration).FirstOrDefault();

                    var details = new EmployeeEducationViewModel()
                    {
                        EducationId = education.EducationId,
                        EmployeeId = education.EmployeeId,
                        InstitutionName = education.InstitutionName,
                        ResultType = education.ResultType,
                        PassingYear = education.PassingYear,
                        Duration = education.Duration,
                        Acheivements = education.Acheivements,
                        CGPA = education.CGPA,
                        CGPAScale = education.CGPAScale,
                        Division = education.Division,
                        MarksPercentage = education.MarksPercentage,
                        DegreeLevelId = educationDegree.DegreeLevelId,
                        DegreeLevel = educationDegree.DegreeLevel,
                        DegreeName = educationDegree.DegreeName,
                        DegreeTitle = educationDegree.DegreeCode,
                        Concentration = educationConcentration != null ? educationConcentration.ConcentrationCode : "",
                        ConcentrationName = educationConcentration != null ? educationConcentration.ConcentrationName : "",
                    };
                    List_EmployeeEducationViewModel.Add(details);
                }
                return Json(List_EmployeeEducationViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetEmpFamilyInfoList(string empMasterId)
        {
            try
            {
                List<EmployeeFamilyInfoViewModel> List_EmployeeFamilyInfoViewModel = new List<EmployeeFamilyInfoViewModel>();
                var familyInfoList = employeeFamilyInfoService.GetByEmployeeId(Convert.ToInt64(empMasterId)).Where(p => p.IsActive == true);
                foreach (var family in familyInfoList)
                {
                    var details = new EmployeeFamilyInfoViewModel()
                    {
                        FamilyInfoId = family.FamilyInfoId,
                        EmployeeId = family.EmployeeId,
                        Name = family.Name,
                        Relation = family.Relation.Trim(),
                        Gender = family.Gender.Trim(),
                        DateOfBirth = family.DateOfBirth.ToString(),
                        EducationalQualification = family.EducationalQualification,
                        Occupation = family.Occupation
                    };
                    List_EmployeeFamilyInfoViewModel.Add(details);
                }

                return Json(List_EmployeeFamilyInfoViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [SessionExpireFilter]
        [DisableCache]
        public JsonResult DepartmentList(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var selectedDepartment = employeeDepartmentService.GetAll().Where(w => w.DepartmentId == Convert.ToInt32(id)).Select(s => new { DisplayText = s.DepartmentName, Value = s.DepartmentName }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = selectedDepartment }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var allRoles = employeeDepartmentService.GetAll().Select(s => new { DisplayText = s.DepartmentName, Value = s.DepartmentId }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = allRoles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult GetPostingDetails(string EmployeeId)
        {
            try
            {
                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();

                StringBuilder sb = new StringBuilder();
                sb.Append("AND vt.EmployeeId=" + EmployeeId);
                var param = new { AndCondition = sb.ToString() };

                var empPostingList = employeeSPService.GetDataWithParameter(param, "trns.SP_GetTransferInformation");

                List_EmployeeViewModel = empPostingList.Tables[0].AsEnumerable()
                 .Select((row, sl) => new EmployeeViewModel
                 {
                     sl = sl + 1,
                     OfficeName = row.Field<string>("OfficeName"),
                     DepartmentName = row.Field<string>("DepartmentName"),
                     DesignationName = row.Field<string>("OfficeDesignationName"),
                     JoiningDate = row.Field<string>("JoiningDateMsg"),
                     DepartureDate = row.Field<string>("DepartureDate")
                 }).ToList();
                return Json(List_EmployeeViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDepartmentWiseSection(int deptId)
        {
            var sectionList = employeeDepartmentSectionService.GetAll().Where(p => p.IsActive == true && p.DepartmentId == deptId).ToList();
            var viewSectionList = sectionList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.SectionName,
                Value = p.SectionId.ToString()
            }).ToList();
            var secList = new List<SelectListItem>();
            secList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            secList.AddRange(viewSectionList);
            return Json(secList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetResignDeathEmployee(string EmployeeCode)
        {
            gHRMDBContext db = new gHRMDBContext();
            var ResignDeath = db.Employees.Where(z => z.EmployeeCode == EmployeeCode && z.IsActive == true).Select(k => k.EmployeeStatusId).FirstOrDefault();

            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { EmployeeCode = EmployeeCode };
            var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_FinalSattlementCheck");


            if (empList.Tables[0].Rows.Count > 0)
            {
                return Json("AllReadyExist", JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (ResignDeath == 12 || ResignDeath == 19)
                {
                    return Json("Valid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NotValid", JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult GetStateList(string country_id)
        {
            var stateList = sateOrProvinceService.GetAll().Where(c => c.CountryId == Convert.ToInt32(country_id));
            var viewState = stateList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(new { Data = state_items }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictList(string state_id)
        {
            if (state_id != "null")
            {
                var districtList = districtService.GetAll().Where(c => c.division_Id == Convert.ToInt32(state_id));
                var viewDistrict = districtList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.district_id.ToString(),
                    Text = x.district_name_eng.ToString()
                });

                var district_items = new List<SelectListItem>();
                district_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                district_items.AddRange(viewDistrict);
                return Json(new { Data = district_items }, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThanaList(string district_id)
        {
            if (district_id != "null")
            {
                var thanaList = thanaService.GetAll().Where(c => c.district_id == Convert.ToInt32(district_id));
                var viewThana = thanaList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.thana_id.ToString(),
                    Text = x.thana_name_eng.ToString()
                });
                var thana_items = new List<SelectListItem>();
                thana_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                thana_items.AddRange(viewThana);
                return Json(thana_items, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnionList(string thana_id)
        {
            if (thana_id != "null")
            {
                var unionList = unionService.GetAll().Where(c => c.thana_id == Convert.ToInt32(thana_id));
                var viewUnion = unionList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.union_id.ToString(),
                    Text = x.union_name_eng.ToString()
                });
                var union_items = new List<SelectListItem>();
                union_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                union_items.AddRange(viewUnion);
                return Json(union_items, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmpAddress(string empAddId, string empMasterId)
        {
            var emp = employeeAddressService.GetByAddressId(Convert.ToInt64(empAddId));
            emp.IsActive = false;
            emp.InActiveDate = DateTime.Now;
            emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            emp.UpdateDate = DateTime.Now;
            employeeAddressService.Update(emp);

            return Json(Convert.ToInt64(empAddId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmergencyContact(int EmergencyContactId)
        {
            var Result = 0;
            try
            {
                var entity = employeeEmergencyContactService.GetById(EmergencyContactId);
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeEmergencyContactService.Update(entity);
                Result = 1;
            }
            catch (Exception)
            {
                Result = 0;
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMedicalInfo(int MedicalInfoId)
        {
            var Result = 0;
            try
            {
                var entity = employeeMedicalInfoService.GetById(MedicalInfoId);
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeMedicalInfoService.Update(entity);
                Result = 1;
            }
            catch (Exception)
            {
                Result = 0;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteLanguageFluency(int QualificationId)
        {
            var Result = 0;
            try
            {
                var entity = employeeOtherQualificationService.GetById(QualificationId);
                entity.IsActive = false;
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;
                employeeOtherQualificationService.Update(entity);
                Result = 1;
            }
            catch (Exception)
            {
                Result = 0;
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmpReference(string refName, string refOccupation, string refDesignation, string refRelation, string refAddress, string refMob, string refTel, string refFax, string refEmail, string refRemarks, string mode, string refEditId, string empMasterId)
        {
            long Emp_Reference_Id = 0;
            if (mode == "S")
            {
                var emp_Reference = new EmployeeReference() { EmployeeId = Convert.ToInt64(empMasterId), ReferenceName = refName, ReferenceOccupation = refOccupation, ReferenceDesignation = refDesignation, ConnectionWithEmployee = refRelation, ContactAddress = refAddress, Mobile = refMob, Telephone = refTel, Fax = refFax, Email = refEmail, Remarks = refRemarks, IsActive = true, CreateUser = Convert.ToInt32(LoggedInEmployeeId), CreateDate = DateTime.Now };
                var referenceSave = employeeReferenceService.Create(emp_Reference);
                if (referenceSave.ReferenceId > 0)
                    Emp_Reference_Id = referenceSave.ReferenceId;
                else
                    Emp_Reference_Id = 0;
            }
            else if (mode == "U")
            {
                //var emp = new EmployeeAddress();
                var emp = employeeReferenceService.GetByReferenceId(Convert.ToInt64(refEditId));
                emp.ReferenceName = refName;
                emp.ReferenceOccupation = refOccupation;
                emp.ReferenceDesignation = refDesignation;
                emp.ConnectionWithEmployee = refRelation;
                emp.ContactAddress = refAddress;
                emp.Mobile = refMob;
                emp.Telephone = refTel;
                emp.Fax = refFax;
                emp.Email = refEmail;
                emp.Remarks = refRemarks;
                emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                emp.UpdateDate = DateTime.Now;
                employeeReferenceService.Update(emp);
                Emp_Reference_Id = Convert.ToInt64(refEditId);
            }

            return Json(Emp_Reference_Id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmpReference(string empRefId, string empMasterId)
        {
            var emp = employeeReferenceService.GetByReferenceId(Convert.ToInt64(empRefId));
            emp.IsActive = false;
            emp.InActiveDate = DateTime.Now;
            emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            emp.UpdateDate = DateTime.Now;
            employeeReferenceService.Update(emp);

            return Json(Convert.ToInt64(empRefId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmpEducation(string empEduId, string empMasterId)
        {
            var emp = employeeEducationService.GetByEducationId(Convert.ToInt64(empEduId));
            emp.IsActive = false;
            emp.InActiveDate = DateTime.Now;
            emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            emp.UpdateDate = DateTime.Now;
            employeeEducationService.Update(emp);

            return Json(Convert.ToInt64(empEduId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmpFamilyInfo(string empFamilyInfoId, string empMasterId)
        {
            var emp = employeeFamilyInfoService.GetByFamilyInfoId(Convert.ToInt64(empFamilyInfoId));
            emp.IsActive = false;
            emp.InActiveDate = DateTime.Now;
            emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            emp.UpdateDate = DateTime.Now;
            employeeFamilyInfoService.Update(emp);

            return Json(Convert.ToInt64(empFamilyInfoId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfficeWiseDepartment(string OfficeTypeId)
        {
            var departmentList = employeeDepartmentService.GetAll().Where(x => x.OfficeTypeId == Convert.ToInt32(OfficeTypeId));
            var viewdepartmentList = departmentList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DepartmentCode, m.DepartmentName), Value = m.DepartmentId.ToString() });

            var department_items = new List<SelectListItem>();
            department_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            department_items.AddRange(viewdepartmentList);

            return Json(department_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeDelete(string employeeId)
        {
            var result = 0;
            var message = "";

            try
            {
                var userLoginId = SessionHelper.LoggedInEmployeeID;
                var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
                var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == userLoginId).RoleId;

                if (superAdminRoleId == loginRoleId)
                {
                    var entity = employeeService.GetByEmpId(Convert.ToInt64(employeeId));

                    if (ModelState.IsValid)
                    {
                        entity.IsActive = false;
                        entity.InActiveDate = DateTime.Now;
                        entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        entity.UpdateDate = DateTime.Now;
                        employeeService.Update(entity);

                        result = 1;
                        message = "Employee deleted successfully";
                    }
                }
                else
                {
                    result = 0;
                    message = "Only Super Admin is authorized to delete employee";
                }
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfficeListByType(string officeType)
        {
            List<OfficeViewModel> List_ViewModel = new List<OfficeViewModel>();
            var param = new { OfficeTypeId = officeType };
            var List = employeeSPService.GetDataWithParameter(param, "basic.SP_Get_OfficeByType");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new OfficeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentByOfficeType(string officeType)
        {
            List<EmployeeDepartmentViewModel> List_ViewModel = new List<EmployeeDepartmentViewModel>();
            var param = new { OfficeTypeId = officeType };
            var List = employeeSPService.GetDataWithParameter(param, "basic.SP_Get_DepartmentByType");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new EmployeeDepartmentViewModel
            {
                DepartmentId = row.Field<int>("DepartmentId"),
                DepartmentCode = row.Field<string>("DepartmentCode"),
                DepartmentName = row.Field<string>("DepartmentName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DepartmentId.ToString(),
                Text = string.Format("{0} - {1}", x.DepartmentCode, x.DepartmentName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveEmployeeGuarantorInformation(EmployeeGuarantorInformation employeeGuarantorInformation)
        {
            int result = 0;
            var message = string.Empty;

            var entity = new EmployeeGuarantorInformation();
            var savedEntity = new EmployeeGuarantorInformation();

            try
            {
                if (employeeGuarantorInformation.GRType == null)
                {
                    message = "Type Guarantor or Reference Required";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (employeeGuarantorInformation.EmployeeId < 1)
                {
                    message = "At first save an employee";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var isDuplicate = employeeGuarantorInformationService.GetMany(p => p.IsActive == true && p.EmployeeId == employeeGuarantorInformation.EmployeeId && p.GuarantorName.ToUpper().Trim() == employeeGuarantorInformation.GuarantorName.ToUpper().Trim());

                if (isDuplicate.Any())
                {
                    message = "Duplicate Employee Guarantor Information, Save denied";
                }
                else
                {
                    if (employeeGuarantorInformation.GRType == "Guarantor")
                    {
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.GuarantorName))
                        {
                            message = "Guarantor Name Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.GuarantorRelationshipId.ToString()))
                        {
                            message = "Relationship Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.OccupationId.ToString()))
                        {
                            message = "Occupation Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.ContactNo))
                        {
                            message = "Contact No Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (employeeGuarantorInformation.GRType == "Reference")
                    {
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.GuarantorName))
                        {
                            message = "Reference Name Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        if (string.IsNullOrEmpty(employeeGuarantorInformation.GuarantorRelationshipId.ToString()))
                        {
                            message = "Relationship Required";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }

                    entity.EmployeeId = employeeGuarantorInformation.EmployeeId;
                    entity.GRType = employeeGuarantorInformation.GRType;
                    entity.GuarantorName = employeeGuarantorInformation.GuarantorName;
                    entity.GuarantorRelationshipId = employeeGuarantorInformation.GuarantorRelationshipId;
                    entity.OccupationId = employeeGuarantorInformation.OccupationId == null ? 0 : employeeGuarantorInformation.OccupationId;
                    entity.ContactNo = employeeGuarantorInformation.ContactNo == null ? "" : employeeGuarantorInformation.ContactNo;
                    entity.NationalID = employeeGuarantorInformation.NationalID == null ? "" : employeeGuarantorInformation.NationalID;
                    entity.ReferenceORGuarantorDetail = employeeGuarantorInformation.ReferenceORGuarantorDetail == null ? "" : employeeGuarantorInformation.ReferenceORGuarantorDetail;
                    entity.GuarantorImage = employeeGuarantorInformation.GuarantorImage;
                    entity.GuaranteeMoney = employeeGuarantorInformation.GuaranteeMoney;

                    entity.PresentCountryId = employeeGuarantorInformation.PresentCountryId == null ? 0 : employeeGuarantorInformation.PresentCountryId;
                    entity.PresentDivisionId = employeeGuarantorInformation.PresentDivisionId == null ? 0 : employeeGuarantorInformation.PresentDivisionId;
                    entity.PresentDistrictId = employeeGuarantorInformation.PresentDistrictId == null ? 0 : employeeGuarantorInformation.PresentDistrictId;
                    entity.PresentThanaId = employeeGuarantorInformation.PresentThanaId == null ? 0 : employeeGuarantorInformation.PresentThanaId;
                    entity.PresentUnionId = employeeGuarantorInformation.PresentUnionId == null ? 0 : employeeGuarantorInformation.PresentUnionId;
                    entity.PresentStreetOrHouse = employeeGuarantorInformation.PresentStreetOrHouse == null ? "" : employeeGuarantorInformation.PresentStreetOrHouse;
                    entity.PresentZipCode = employeeGuarantorInformation.PresentZipCode == null ? "" : employeeGuarantorInformation.PresentZipCode;
                    entity.PresentAddressDetail = employeeGuarantorInformation.PresentAddressDetail == null ? "" : employeeGuarantorInformation.PresentAddressDetail;

                    entity.PermanentCountryId = employeeGuarantorInformation.PermanentCountryId == null ? 0 : employeeGuarantorInformation.PermanentCountryId;
                    entity.PermanentDivisionId = employeeGuarantorInformation.PermanentDivisionId == null ? 0 : employeeGuarantorInformation.PermanentDivisionId;
                    entity.PermanentDistrictId = employeeGuarantorInformation.PermanentDistrictId == null ? 0 : employeeGuarantorInformation.PermanentDistrictId;
                    entity.PermanentThanaId = employeeGuarantorInformation.PermanentThanaId == null ? 0 : employeeGuarantorInformation.PermanentThanaId;
                    entity.PermanentUnionId = employeeGuarantorInformation.PermanentUnionId == null ? 0 : employeeGuarantorInformation.PermanentUnionId;
                    entity.PermanentStreetOrHouse = employeeGuarantorInformation.PermanentStreetOrHouse == null ? "" : employeeGuarantorInformation.PermanentStreetOrHouse;
                    entity.PermenantZipCode = employeeGuarantorInformation.PermenantZipCode == null ? "" : employeeGuarantorInformation.PermenantZipCode;
                    entity.PermanentAddressDetail = employeeGuarantorInformation.PermanentAddressDetail == null ? "" : employeeGuarantorInformation.PermanentAddressDetail;

                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;

                    savedEntity = employeeGuarantorInformationService.Create(entity);
                    message = "Save Successfull";
                    result = savedEntity.GuarantorId;
                }
            }

            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }




        public JsonResult SaveGuarantorMoney(string TransactionType, string TransactionDate, decimal TransactionAmount, string PaymentType, string BankName, string AccountNo, string CheckNo, string EmployeeId, string mode, string eduEditId)
        {
            long Grantor_Id = 0;
            int BalaceCheck = 0;

            try
            {
                if (mode == "S")
                {
                    var param = new { EmployeeId = Convert.ToInt64(EmployeeId), TransactionType = TransactionType, TransactionDate = Convert.ToDateTime(TransactionDate), TransactionAmount = TransactionAmount, PaymentType = PaymentType, BankName = BankName, AccountNo = AccountNo, CheckNo = CheckNo, BalaceCheck = 0, id = 0 };
                    var grantorMoneyList = employeeSPService.GetDataWithParameter(param, "emp.SP_GuarantorMoneyBalanceCheck");
                    var Balance = grantorMoneyList.Tables[0].Rows[0].ItemArray[0].ToString();

                    BalaceCheck = Convert.ToInt32(Balance);

                    if (BalaceCheck == 0)
                    {

                        var emp_Grantor = new EmployeeGuarantorTranInformation() { EmployeeID = Convert.ToInt64(EmployeeId), TransactionType = TransactionType, TransactionDate = Convert.ToDateTime(TransactionDate), TransactionAmount = Convert.ToDecimal(TransactionAmount), PaymentType = PaymentType, BankName = BankName, BranchName = "", AccountNo = AccountNo, ChequeNo = CheckNo, CreatedBy = Convert.ToInt64(LoggedInEmployeeId), CreatedDate = DateTime.Now, UpdateBy = Convert.ToInt64(LoggedInEmployeeId), UpdateDate = DateTime.Now, IsRemoved = false };
                        var GurantorMoneySave = employeeGuarantorTranInformationService.Create(emp_Grantor);

                        if (GurantorMoneySave.ID > 0)
                            // Grantor_Id = GurantorMoneySave.ID;
                            Grantor_Id = 0;
                        else
                            Grantor_Id = 0;
                    }
                    else
                    {
                        Grantor_Id = 2;
                    }

                }
                else if (mode == "U")
                {
                    var id = Convert.ToInt64(eduEditId);

                    var param99 = new { EmployeeId = Convert.ToInt64(EmployeeId), TransactionType = TransactionType, TransactionDate = Convert.ToDateTime(TransactionDate), TransactionAmount = TransactionAmount, PaymentType = PaymentType, BankName = BankName, AccountNo = AccountNo, CheckNo = CheckNo, BalaceCheck = 0, id = id };
                    var grantorMoneyList99 = employeeSPService.GetDataWithParameter(param99, "emp.SP_GuarantorMoneyBalanceCheck");
                    var Balance99 = grantorMoneyList99.Tables[0].Rows[0].ItemArray[0].ToString();

                    BalaceCheck = Convert.ToInt32(Balance99);


                    if (BalaceCheck == 0)
                    {

                        var param2 = new { id = id, TransactionType = TransactionType, TransactionDate = Convert.ToDateTime(TransactionDate), TransactionAmount = TransactionAmount, PaymentType = PaymentType, BankName = BankName, AccountNo = AccountNo, CheckNo = CheckNo };
                        var grantorMoneyList2 = employeeSPService.GetDataWithParameter(param2, "emp.SP_UpdateGuarantorMoney");

                        Grantor_Id = 0;
                    }
                    else
                    {
                        Grantor_Id = 2;
                    }

                }

            }
            catch (Exception ex)
            {

            }

            return Json(Grantor_Id, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteGrantorMoney(string empEduId, string empMasterId)
        {

            var param = new { employeeid = Convert.ToInt64(empMasterId), id = Convert.ToInt64(empEduId) };
            var grantorMoneyList = employeeSPService.GetDataWithParameter(param, "emp.SP_DeleteGuarantorMoney");

            return Json(Convert.ToInt64(empEduId), JsonRequestBehavior.AllowGet);
        }





        public JsonResult UpdateEmployeeGuarantorInformation(EmployeeGuarantorInformation employeeGuarantorInformation)
        {
            int result = 0;
            var message = string.Empty;

            try
            {
                var isDuplicate =
                   employeeGuarantorInformationService.GetAll().Where(
                           p =>
                               p.IsActive == true && p.GuarantorId != employeeGuarantorInformation.GuarantorId &&
                               p.GuarantorName.ToUpper().Trim() == employeeGuarantorInformation.GuarantorName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    message = "Duplicate Employee Guarantor Information, Update denied";
                }
                else
                {
                    var entity = employeeGuarantorInformationService.GetById(employeeGuarantorInformation.GuarantorId);

                    entity.GRType = employeeGuarantorInformation.GRType;
                    entity.GuarantorId = employeeGuarantorInformation.GuarantorId;
                    entity.GuarantorName = employeeGuarantorInformation.GuarantorName;
                    entity.GuarantorRelationshipId = employeeGuarantorInformation.GuarantorRelationshipId;
                    entity.OccupationId = employeeGuarantorInformation.OccupationId;
                    entity.ContactNo = employeeGuarantorInformation.ContactNo;
                    entity.NationalID = employeeGuarantorInformation.NationalID;
                    entity.ReferenceORGuarantorDetail = employeeGuarantorInformation.ReferenceORGuarantorDetail;

                    entity.PresentCountryId = employeeGuarantorInformation.PresentCountryId;
                    entity.PresentDivisionId = employeeGuarantorInformation.PresentDivisionId;
                    entity.PresentDistrictId = employeeGuarantorInformation.PresentDistrictId;
                    entity.PresentThanaId = employeeGuarantorInformation.PresentThanaId;
                    entity.PresentUnionId = employeeGuarantorInformation.PresentUnionId;
                    entity.PresentStreetOrHouse = employeeGuarantorInformation.PresentStreetOrHouse;
                    entity.PresentZipCode = employeeGuarantorInformation.PresentZipCode;
                    entity.PresentAddressDetail = employeeGuarantorInformation.PresentAddressDetail;

                    entity.PermanentCountryId = employeeGuarantorInformation.PermanentCountryId;
                    entity.PermanentDivisionId = employeeGuarantorInformation.PermanentDivisionId;
                    entity.PermanentDistrictId = employeeGuarantorInformation.PermanentDistrictId;
                    entity.PermanentThanaId = employeeGuarantorInformation.PermanentThanaId;
                    entity.PermanentUnionId = employeeGuarantorInformation.PermanentUnionId;
                    entity.PermanentStreetOrHouse = employeeGuarantorInformation.PermanentStreetOrHouse;
                    entity.PermenantZipCode = employeeGuarantorInformation.PermenantZipCode;
                    entity.PermanentAddressDetail = employeeGuarantorInformation.PermanentAddressDetail;

                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeGuarantorInformationService.Update(entity);

                    message = "Update Successfull";
                    result = employeeGuarantorInformation.GuarantorId;
                }
            }
            catch (Exception ex)
            {
                message = "Update Failed";
            }
            return Json(new { message = message, result = result }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListEmployeeGuarantorInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, int EmployeeId)
        {
            var vmcar = view_EmployeeGuarantorInformationService.GetMany(t => t.IsActive == true && t.EmployeeId == EmployeeId);

            var currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult InformationDeleteGuarantorInformation(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeGuarantorInformationService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeGuarantorInformationService.Update(model);
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

        [HttpPost]
        public JsonResult SaveTraining(EmployeeTraining employeeTraining)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    employeeTrainingService.GetAll().Where(p => p.IsActive == true && p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Employee Training InstituteName, Save denied";
                }
                else
                {
                    var entity = employeeTraining;
                    entity.IsApproved = employeeTraining.IsApproved;
                    entity.IsRejected = employeeTraining.IsRejected;
                    entity.approveby = employeeTraining.approveby;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    var savedEntity = employeeTrainingService.Create(entity);
                    result = "Save Successfull";
                }
            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTraining(EmployeeTraining employeeTraining)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   employeeTrainingService.GetAll().Where(p => p.IsActive == true && p.EmployeeTrainingId != employeeTraining.EmployeeTrainingId &&
                               p.InstituteName.ToUpper().Trim() == employeeTraining.InstituteName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Employee Training InstituteName, Update denied";
                }
                else
                {
                    var entity = employeeTrainingService.GetById(employeeTraining.EmployeeTrainingId);
                    entity.EmployeeTrainingId = employeeTraining.EmployeeTrainingId;
                    entity.TrainingTitle = employeeTraining.TrainingTitle;
                    entity.InstituteName = employeeTraining.InstituteName;
                    entity.TrainingCountryId = employeeTraining.TrainingCountryId;
                    entity.TrainingTopics = employeeTraining.TrainingTopics;
                    entity.Result = employeeTraining.Result;
                    entity.TrainingDateFrom = employeeTraining.TrainingDateFrom;
                    entity.TrainingDateTo = employeeTraining.TrainingDateTo;
                    entity.CurrentOfficeTraining = employeeTraining.CurrentOfficeTraining;
                    entity.IsApproved = employeeTraining.IsApproved;
                    entity.IsRejected = employeeTraining.IsRejected;
                    entity.approveby = employeeTraining.approveby;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;

                    employeeTrainingService.Update(entity);
                    result = "Update Successfull";
                }
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListTraning(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, int EmployeeId)
        {
            var vmcar = view_EmployeeTrainingService.GetMany(t => t.IsActive == true && t.EmployeeId == EmployeeId && t.IsApproved == true);

            var currentPageRecords = vmcar.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = vmcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult InformationDeleteTraning(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTrainingService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTrainingService.Update(model);
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

        [HttpPost]
        public JsonResult SaveCertificatesInfo(ReceivedCertificates data)
        {
            var result = 0;
            var message = "";

            try
            {
                var empCode = employeeService.GetByEmpId(data.EmployeeId).EmployeeCode;

                var model = new ReceivedCertificates();
                model.EmployeeId = data.EmployeeId;
                model.EmployeeCode = empCode;
                model.DegreeCode = data.DegreeCode;
                model.Memo = data.Memo;

                model.NoOfCopies = data.NoOfCopies;
                model.EmployeeCertificateStatus = data.EmployeeCertificateStatus;
                model.StatusDate = data.StatusDate;
                model.Comment = data.Comment;
                model.CertificateType = data.CertificateType;
                model.IsActive = true;
                model.CreateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;
                receivedCertificatesService.Create(model);
                result = 1;
                message = "Certificates saved successfully";
            }
            catch (Exception e)
            {
                result = 0;
                message = "Certificates save failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeCertificateInformation(int jtStartIndex, int jtPageSize, string jtSorting, int employeeId)
        {
            var param = new { EmployeeId = employeeId };
            var list = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeCertificateInformation");

            var view_List = list.Tables[0].AsEnumerable().Select(row => new EmployeeViewModel
            {
                CertificateId = row.Field<int>("Id"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                DegreeCode = row.Field<string>("DegreeCode"),
                Memo = Convert.ToString(row.Field<int>("Memo")),
                EmployeeCertificateStatus = row.Field<string>("EmployeeCertificateStatus"),
                StatusDate = row.Field<DateTime>("StatusDate"),
                StatusDateForCertificate = row.Field<string>("StatusDateForCertificate"),
                Comment = row.Field<string>("Comment"),
                CertificateType = row.Field<string>("CertificateType"),
                DegreeLevel = row.Field<string>("DegreeLevel"),

            }).ToList();

            var currentPageRecords = view_List.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_List.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult UpdateCertificatesInfo(ReceivedCertificates data)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = receivedCertificatesService.GetById(data.Id);

                var checkDuplicate = receivedCertificatesService.GetAll().Where(p => p.IsActive == true && p.DegreeCode == data.DegreeCode && p.EmployeeId == model.EmployeeId && p.Id != model.Id).ToList();

                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Already this Certificate exist, Update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.DegreeCode = data.DegreeCode;
                    model.NoOfCopies = data.NoOfCopies;
                    model.EmployeeCertificateStatus = data.EmployeeCertificateStatus;
                    model.StatusDate = data.StatusDate;
                    model.Comment = data.Comment;
                    model.CertificateType = data.CertificateType;
                    model.IsActive = true;
                    model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                    model.UpdateDate = DateTime.UtcNow;
                    receivedCertificatesService.Update(model);
                    result = 1;
                    message = "Certificate updated successfully";
                }
            }
            catch (Exception e)
            {
                result = 0;
                message = "Certificates failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnCertificateInformation(int empid, string CertificateStatusDate)
        {
            var result = 0;
            var message = "";

            try
            {

                // var return_certificates = new List<ReceivedCertificates>();
                var employee_certificates = receivedCertificatesService.GetAll().Where(p => p.IsActive == true && p.EmployeeId == empid).ToList();
                foreach (var certificate in employee_certificates)
                {
                    //var return_certificate = new ReceivedCertificates();
                    var model = receivedCertificatesService.GetById(certificate.Id);
                    model.StatusDate = Convert.ToDateTime(CertificateStatusDate);
                    model.Id = certificate.Id;
                    model.EmployeeCertificateStatus = "Return";
                    model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                    model.UpdateDate = DateTime.UtcNow;

                    receivedCertificatesService.Update(model);
                    //return_certificates.Add(return_certificate);
                }
                result = 1;
                message = "Certificate Return successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Return failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WithdrawCertificateInformation(int empid, string CertificateStatusDate, int id)
        {
            var result = 0;
            var message = "";

            try
            {
                var certificate = receivedCertificatesService.GetAll().Where(p => p.IsActive == true && p.Id == id).FirstOrDefault();

                var model = receivedCertificatesService.GetById(certificate.Id);
                model.StatusDate = Convert.ToDateTime(CertificateStatusDate);
                model.Id = certificate.Id;
                model.EmployeeCertificateStatus = "TemporaryWithdrawal";
                model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;

                receivedCertificatesService.Update(model);

                result = 1;
                message = "Certificate Withdrawn successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Return failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCertificateInformation(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = receivedCertificatesService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;
                receivedCertificatesService.Update(model);
                result = 1;
                message = "Certificate deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeOfficeVisitApprovalList(int jtStartIndex, int jtPageSize, string jtSorting, int EmployeeId)
        {
            var approvalList =
                employeeOfficeVisitInformationService.GetMany(p => p.IsActive == true && p.EmployeeId == EmployeeId && p.IsApproved == true)
                    .ToList();
            var viewApprovalList = approvalList.AsEnumerable().Select(p => new EmployeeTVPROViewModel()
            {
                EmpOfficeVisitId = p.EmpOfficeVisitId,
                EmployeeId = p.EmployeeId,
                EmployeeCode = p.EmployeeCode,
                VisitType = p.VisitType,
                Location = p.Location,
                Reason = p.Reason,
                CurrentOfficeProvidedVal = p.CurrentOfficeProvided,
                CurrentOfficeProvided = p.CurrentOfficeProvided == 1 ? "Yes" : "No",
                IsApproved = p.IsApproved
            }).ToList();
            var currentPageRecords = viewApprovalList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewApprovalList.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult GetEmployeeRelativeInfo(int jtStartIndex, int jtPageSize, string jtSorting, int EmployeeId)
        {
            var relativeInfo = linkWithEmployeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeId == EmployeeId && p.IsApproved == true).ToList();
            var viewRelativeInfo = relativeInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                LinkId = p.LinkId,
                OrganizationCode = p.OrganizationCode,
                RelativeEmployeeCode = p.EmployeeCode,
                RelativeDepartmentName = p.Department,
                RelativeDesignationName = p.Designation,
                RelativeEmployeeName = p.EmployeeName,
                IsApproved = p.IsApproved
            }).ToList();
            var currentPageRecords = viewRelativeInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewRelativeInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult GetWorkExperienceInfo(int jtStartIndex, int jtPageSize, string jtSorting, int EmployeeId)
        {
            var expInfo =
                workExperienceWithInterOrganizationService.GetMany(p => p.IsActive == true && p.EmployeeId == EmployeeId && p.IsApproved == true)
                    .ToList();
            var viewExpInfo = expInfo.AsEnumerable().Select(p => new EmployeeOtherInformationViewModel()
            {
                WorkExpId = p.WorkExpId,
                EmployeeCode = p.EmployeeCode,
                OrganizationCode = p.OrgCode,
                RelativeDepartmentName = p.Department,
                RelativeDesignationName = p.Designation,
                JoiningDateView = Convert.ToDateTime(p.JoiningDate).ToString("dd-MMM-yyyy"),
                ReleaseDateView = Convert.ToDateTime(p.ReleaseDate).ToString("dd-MMM-yyyy")
            }).ToList();
            var currentPageRecords = viewExpInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewExpInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult ListTimeKeepingRoster(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, int employeeId)
        {
            //get employee roaster schedule from [[att].[RoasterSchedule_GetRoasterScheduleByEmployee]]
            var roasterEmployeeSchedules = roasterEmployeeScheduleService
                                                .GetRoasterEmployeeSchedulesByEmployeeId(employeeId);

            var currentPageRecords = roasterEmployeeSchedules
                                        .Skip(jtStartIndex)
                                            .Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = roasterEmployeeSchedules.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateRosterAttendance(int timeKeepingRosterId, int employeeId,
            DateTime roasterEffectiveStartDate, DateTime roasterEffectiveEndDate)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Warning, You mus fill all the required field!", isSuccess = false }, JsonRequestBehavior.AllowGet);

            var result = string.Empty;

            try
            {
                //get roaster by roaster id
                var roaster = timeKeepingRosterService.GetActiveRoasterById(timeKeepingRosterId);
                if (roaster == null)
                {
                    result = "Warning, Roaster not found!";
                    return Json(new { result = result, isSuccess = false }, JsonRequestBehavior.AllowGet);
                }

                var roasterEmployeeScheduleId = 0;

                var roasterEmployeeSchedule = roasterEmployeeScheduleService.GetRoasterEmployeeScheduleByEmployeeAndRosterId(employeeId, timeKeepingRosterId);
                if (roasterEmployeeSchedule != null)
                    roasterEmployeeScheduleId = roasterEmployeeSchedule.Id;

                var isDateRangeValid = timeKeepingRosterService.ValidateEmployeeRoasterByDateRange(employeeId, roasterEmployeeScheduleId, roasterEffectiveStartDate, roasterEffectiveEndDate);
                if (!isDateRangeValid)
                    return Json(new { result = "Warning, Effective Date Range in used!", isSuccess = false }, JsonRequestBehavior.AllowGet);

                if (roasterEmployeeSchedule != null)
                {
                    //populate roaster employee schedule
                    roasterEmployeeSchedule = PopulateRoasterEmployeeSchedule(roasterEmployeeSchedule.Id, timeKeepingRosterId, employeeId, roaster
                        , roasterEffectiveStartDate, roasterEffectiveEndDate);


                    if(SessionHelper.CompanyInfo.CompanyShortName == "VERC")
                    {
                        var response = roasterEmployeeScheduleService.Create(roasterEmployeeSchedule);
                        //let's update employee roaster schedule for [att.RoasterEmployeeSchedule]                    

                        ////////KHALID
                        var roasterEmployeeSchedules = roasterEmployeeScheduleService
                                                   .GetRoasterEmployeeSchedulesByEmployeeId(employeeId).FirstOrDefault();
                        var LIT = roasterEmployeeSchedules.LIT; //LOgin time
                        var LOT = roasterEmployeeSchedules.LOT; // Log Out Time
                        var LLT = roasterEmployeeSchedules.LLT; // Grace TIME / Last Login Time

                        var employee = employeeService.GetByEmpId(employeeId);
                        var blandDate = "1900-01-01 ";
                        var tempLoginTime = (employee.LoginTime != null ? Convert.ToDateTime(employee.LoginTime).ToString("yyyy-MM-dd ") : blandDate) + LIT;
                        var tempLogOutTime = (employee.LogoutTime != null ? Convert.ToDateTime(employee.LogoutTime).ToString("yyyy-MM-dd ") : blandDate) + LOT;
                        var tempGraceTime = (employee.LogoutTime != null ? Convert.ToDateTime(employee.LastLoginTime).ToString("yyyy-MM-dd ") : blandDate) + LLT;

                        employee.LoginTime = Convert.ToDateTime(tempLoginTime);
                        employee.LogoutTime = Convert.ToDateTime(tempLogOutTime);
                        employee.LastLoginTime = Convert.ToDateTime(tempGraceTime);
                        employeeService.Update(employee);

                        return Json(new { result = response.Message, isSuccess = response.IsSuccess }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                    var response = roasterEmployeeScheduleService.Update(roasterEmployeeSchedule);                  
                    //let's update employee roaster schedule for [att.RoasterEmployeeSchedule]                    

                    ////////KHALID
                    var roasterEmployeeSchedules = roasterEmployeeScheduleService
                                               .GetRoasterEmployeeSchedulesByEmployeeId(employeeId).FirstOrDefault();
                    var LIT = roasterEmployeeSchedules.LIT; //LOgin time
                    var LOT = roasterEmployeeSchedules.LOT; // Log Out Time
                    var LLT = roasterEmployeeSchedules.LLT; // Grace TIME / Last Login Time

                    var employee = employeeService.GetByEmpId(employeeId);
                    var blandDate = "1900-01-01 ";
                    var tempLoginTime = (employee.LoginTime != null ? Convert.ToDateTime(employee.LoginTime).ToString("yyyy-MM-dd ") : blandDate) + LIT;
                    var tempLogOutTime = (employee.LogoutTime != null ? Convert.ToDateTime(employee.LogoutTime).ToString("yyyy-MM-dd ") : blandDate) + LOT;
                    var tempGraceTime = (employee.LogoutTime != null ? Convert.ToDateTime(employee.LastLoginTime).ToString("yyyy-MM-dd ") : blandDate) + LLT;

                    employee.LoginTime = Convert.ToDateTime(tempLoginTime);
                    employee.LogoutTime = Convert.ToDateTime(tempLogOutTime);
                    employee.LastLoginTime = Convert.ToDateTime(tempGraceTime);
                    employeeService.Update(employee);

                    return Json(new { result = response.Message, isSuccess = response.IsSuccess }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //populate roaster employee schedule
                    var newRoasterEmployeeSchedule = PopulateRoasterEmployeeSchedule(0, timeKeepingRosterId, employeeId, roaster
                        , roasterEffectiveStartDate, roasterEffectiveEndDate);

                    //let's add into [RoasterEmployeeSchedule] table
                    var response = roasterEmployeeScheduleService.Create(newRoasterEmployeeSchedule);

                    return Json(new { result = response.Message, isSuccess = response.IsSuccess }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result, isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckRoster(int employeeId)
        {
            var result = string.Empty;
            var LoginTime = string.Empty;
            var LastLoginTime = string.Empty;
            var LogoutTime = string.Empty;
            var loginTiming = string.Empty;
            try
            {
                var employee = employeeService.GetByEmpId(Convert.ToInt64(employeeId));
                if (employee != null)
                {
                    if (employee.LoginTime != null)
                    {
                        var tempLoginTime = Convert.ToDateTime(employee.LoginTime);
                        LoginTime = tempLoginTime.ToString("hh:mm tt");
                        var tempLastLogIntime = Convert.ToDateTime(employee.LastLoginTime);
                        LastLoginTime = tempLastLogIntime.ToString("hh:mm tt");
                        var tempLogoutTime = Convert.ToDateTime(employee.LogoutTime);
                        LogoutTime = tempLogoutTime.ToString("hh:mm tt");
                        loginTiming = "LogIn Time: " + LoginTime + " Last LogIn Time: " + LastLoginTime + " LogOut Time: " + LogoutTime;
                    }
                    else
                        loginTiming = "No Schedule Found";
                }
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result, loginTiming = loginTiming }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPublicationInfo(int jtStartIndex, int jtPageSize, string jtSorting, int employeId)
        {
            var publicationInfo =
                employeePublicationService.GetMany(p => p.IsActive == true && p.IsApproved == true && p.EmployeeId == employeId).ToList();
            var currentPageRecords = publicationInfo.Skip(jtStartIndex).Take(jtPageSize);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = publicationInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public async Task<JsonResult> NewEmployeeCreate(EmployeeViewModel model)
        {
            long result = 0;
            string message = string.Empty;

            try
            {
                if (model.EmployeeId == 0 && !string.IsNullOrEmpty(model.EmployeeCode))
                {
                    if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.JagoraniChakraFoundation)
                    {
                        if (model.EmployeeCode.Length != 6)
                            return Json(new { IsSuccess = false, Message = "Employee Code must be 6 Digits" }, JsonRequestBehavior.AllowGet);
                    }

                    if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.PidimFoundation)
                    {
                        var param = new { EmployeeCode = model.EmployeeCode };
                        var duplicateResult = employeeSPService.GetDataWithParameter(param, "emp.SP_GET_DUPLICATE_EMPLOYEECODE");
                        if (duplicateResult.Tables[0].Rows[0]["IsDuplicate"].ToString() == "1")
                            return Json(new { IsSuccess = false, Message = "Duplicate Employee Code" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var empCodeCnt = employeeService.IsValidEmployee(model.EmployeeCode).Count();

                        if (empCodeCnt > 0)
                            return Json(new { IsSuccess = false, Message = "Duplicate Employee Code" }, JsonRequestBehavior.AllowGet);
                    }

                    var entity = GenerateEmployee(model);

                    //Let's create new employee into [dbo.Employee]
                    var obj = employeeService.Create(entity);

                    //let's insert into employee status history [dbo.EmployeeStatusHistory]
                    var response = InsertHistory(obj);

                    result = obj.EmployeeId;
                    model.EmployeeId = obj.EmployeeId;
                }

                if (model.EmployeeId > 0 && !string.IsNullOrEmpty(model.EmployeeCode))
                {
                    var employeePrevData = employeeService.GetByEmpId(Convert.ToInt32(model.EmployeeId));
                    if (employeePrevData.EmployeeCode != model.EmployeeCode)
                    {
                        if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.PidimFoundation)
                        {
                            var param = new { EmployeeCode = model.EmployeeCode };
                            var duplicateResult = employeeSPService.GetDataWithParameter(param, "emp.SP_GET_DUPLICATE_EMPLOYEECODE");
                            if (duplicateResult.Tables[0].Rows[0]["IsDuplicate"].ToString() == "1")
                                return Json(new { IsSuccess = false, Message = "Duplicate Employee Code" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var empCodeCnt = employeeService.IsValidEmployee(model.EmployeeCode).Count();
                            if (empCodeCnt > 0)
                                return Json(new { IsSuccess = false, Message = "Duplicate Employee Code" }, JsonRequestBehavior.AllowGet);
                        }
                    }                   

                    var entity = GenerateEmployee(model);
                    if (ModelState.IsValid)
                    {
                        if (entity.EmployeeStatusId == 22)
                            entity.IsActive = false;
                        else
                            entity.IsActive = true;

                        //let's update employee in [dbo.Employee] 
                        employeeService.Update(entity);

                        //let's insert into employee status history 
                        var response = InsertHistory(entity);

                        // if employee code change then takes a history 
                        if (SessionHelper.CompanyInfo.CompanyShortName == "Pidim")
                        {
                            if (model.EmployeeCode != model.OldEmployeeCode)
                            {
                                var param = new
                                {
                                    CreateBy = LoggedInEmployeeId,
                                    OldEmployeeCode = model.OldEmployeeCode ?? "0",
                                    NewEmployeeCode = model.EmployeeCode,
                                    EmployeeId = model.EmployeeId
                                };

                                var tt = employeeSPService.GetDataWithParameter(param, "emp.SP_INSERT_EMPLOYEECODE_HISTORY");
                            }
                        }
                        result = entity.EmployeeId;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                return Json(new { IsSuccess = false, Message = exceptionMessage }, JsonRequestBehavior.AllowGet);
                // Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return Json(new { IsSuccess = true, Message = "Employee Saved Successfully", EmployeeId = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmpEducation(string degreeTitle, string concentration, string institutionName, string resultTyp, string division, string marksPercentage,
            string cgpa, string cgpaScale, string passingYear, string duration, string acheivements, string mode, string eduEditId, string empMasterId)
        {
            long Emp_Education_Id = 0;
            if (mode == "S")
            {
                var emp_Education = new EmployeeEducation() { EmployeeId = Convert.ToInt64(empMasterId), DegreeTitle = degreeTitle, Concentration = concentration, InstitutionName = institutionName, ResultType = resultTyp, Division = division, MarksPercentage = marksPercentage, CGPA = cgpa, CGPAScale = cgpaScale, PassingYear = passingYear, Duration = duration, Acheivements = acheivements, IsActive = true, CreateUser = Convert.ToInt64(LoggedInEmployeeId), CreateDate = DateTime.Now };
                var educationSave = employeeEducationService.Create(emp_Education);
                if (educationSave.EducationId > 0)
                    Emp_Education_Id = educationSave.EducationId;
                else
                    Emp_Education_Id = 0;
            }
            else if (mode == "U")
            {
                var emp = employeeEducationService.GetByEducationId(Convert.ToInt64(eduEditId));
                emp.DegreeTitle = degreeTitle;
                emp.Concentration = concentration;
                emp.InstitutionName = institutionName;
                emp.ResultType = resultTyp;
                emp.Division = division;
                emp.MarksPercentage = marksPercentage;
                emp.CGPA = cgpa;
                emp.CGPAScale = cgpaScale;
                emp.PassingYear = passingYear;
                emp.Duration = duration;
                emp.Acheivements = acheivements;
                emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                emp.UpdateDate = DateTime.Now;
                employeeEducationService.Update(emp);
                Emp_Education_Id = Convert.ToInt64(eduEditId);
            }
            return Json(Emp_Education_Id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeBasicInfoCreate(string dateOfBirth, string birthPlace, string nationality, string nationalId, string bloodGroup,
               string gender, string passportNo, string passportIssueDate, string passportExpireDate, string contactNo1, string contactNo2, string email,
               string officialEmail, string presentAddress, string permanentAddress, string maritalStatus, long empId, string religion, string pabxExtension, string eTinNo)
        {
            long Result = 0;
            try
            {
                var entity = employeeService.GetByEmpId(empId);
                entity.DateOfBirth = Convert.ToDateTime(dateOfBirth);
                entity.BirthPlace = birthPlace;
                entity.Nationality = nationality;
                entity.NationalId = nationalId;
                entity.BloodGroup = bloodGroup;
                entity.Gender = gender;
                entity.PassportNo = passportNo;
                if (passportIssueDate != "")
                {
                    entity.PassportIssueDate = Convert.ToDateTime(passportIssueDate);
                }

                if (passportExpireDate != "")
                {
                    entity.PassportExpireDate = Convert.ToDateTime(passportExpireDate);
                }

                entity.ContactNo1 = contactNo1;
                entity.ContactNo2 = contactNo2;
                entity.Email = email;
                entity.OfficialEmail = officialEmail;
                entity.PresentAddress = presentAddress;
                entity.PermanentAddress = permanentAddress;
                entity.MaritalStatus = maritalStatus;
                entity.Religion = religion;
                entity.PABXExtension = pabxExtension;
                entity.ETinNo = eTinNo;
                employeeService.Update(entity);
                Result = 1;

            }
            catch (Exception ex)
            {
                Result = 0;

            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmpAddress(string addType, string country, string state, string dist, string thana, string unionId, string street, string ZipCode,
            string mode, string addressEditId, string empMasterId, string addressDetail, string postOffice)
        {
            var result = 0;
            var message = "";

            try
            {

                var checkDuplicateAddress = employeeAddressService.GetAll().Where(p => p.EmployeeId == Convert.ToInt64(empMasterId) && p.AddressType.Trim().ToUpper() == addType.Trim().ToUpper() && p.IsActive == true && p.AddressId != Convert.ToInt32(addressEditId)).ToList();

                if (checkDuplicateAddress.Any())
                {
                    result = 0;
                    message = "Employee Address Already Exists";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (street != null && street == "")
                    street = "N/A";

                if (ZipCode != null && ZipCode == "")
                    ZipCode = "N/A";

                long Emp_Address_Id = 0;
                if (mode == "S")
                {
                    var emp_Address = new EmployeeAddress()
                    {
                        EmployeeId = Convert.ToInt64(empMasterId),
                        AddressType = addType,
                        CountryId = country == "" ? 0 : Convert.ToInt32(country),
                        StateOrProvinceId = state == "" ? 0 : Convert.ToInt32(state),
                        DistrictId = dist == "" ? 0 : Convert.ToInt32(dist),
                        ThanaId = thana == "" ? 0 : Convert.ToInt32(thana),
                        UnionId = unionId == "" ? 0 : Convert.ToInt32(unionId),
                        StreetOrHouse = street,
                        ZipCode = ZipCode,
                        IsActive = true,
                        CreateUser = Convert.ToInt32(LoggedInEmployeeId),
                        CreateDate = DateTime.Now,
                        AddressDetail = addressDetail,
                        PostOffice = postOffice
                    };
                    var addressSave = employeeAddressService.Create(emp_Address);
                    if (addressSave.AddressId > 0)
                        Emp_Address_Id = addressSave.AddressId;
                    else
                        Emp_Address_Id = 0;
                }
                else if (mode == "U")
                {
                    var emp = employeeAddressService.GetByAddressId(Convert.ToInt64(addressEditId));
                    emp.AddressType = addType;
                    emp.CountryId = Convert.ToInt32(country);
                    emp.StateOrProvinceId = Convert.ToInt32(state);
                    if (dist != "")
                        emp.DistrictId = Convert.ToInt32(dist);
                    if (thana != "")
                        emp.ThanaId = Convert.ToInt32(thana);
                    if (unionId != "")
                        emp.UnionId = Convert.ToInt32(unionId);
                    emp.StreetOrHouse = street;
                    emp.ZipCode = ZipCode;
                    emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    emp.UpdateDate = DateTime.Now;
                    Emp_Address_Id = Convert.ToInt64(addressEditId);
                    emp.AddressDetail = addressDetail;
                    emp.PostOffice = postOffice;
                    employeeAddressService.Update(emp);

                }

                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.ToString();
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveEmpAddressBothPermanentPresent(string addType, string country, string state, string dist, string thana, string unionId, string street, string ZipCode,
           string mode, string addressEditId, string empMasterId, string addressDetail, string postOffice)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicateAddress = employeeAddressService.GetAll().Where(p => p.EmployeeId == Convert.ToInt64(empMasterId) && p.AddressType.Trim().ToUpper() == addType.Trim().ToUpper() && p.IsActive == true && p.AddressId != Convert.ToInt32(addressEditId)).ToList();

                if (checkDuplicateAddress.Any())
                {
                    result = 0;
                    message = "Employee Address Already Exists";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (street != null && street == "")
                    street = "N/A";

                if (ZipCode != null && ZipCode == "")
                    ZipCode = "N/A";

                if (mode == "S")
                {
                    int Rows = 2;
                    for (var iterator = 1; iterator <= Rows; iterator++)
                    {
                        if (iterator == 1)
                            addType = "Pr";
                        else
                            addType = "Pe";

                        var emp_Address = new EmployeeAddress()
                        {
                            EmployeeId = Convert.ToInt64(empMasterId),
                            AddressType = addType,
                            CountryId = country == "" ? 0 : Convert.ToInt32(country),
                            StateOrProvinceId = state == "" ? 0 : Convert.ToInt32(state),
                            DistrictId = dist == "" ? 0 : Convert.ToInt32(dist),
                            ThanaId = thana == "" ? 0 : Convert.ToInt32(thana),
                            UnionId = unionId == "" ? 0 : Convert.ToInt32(unionId),
                            StreetOrHouse = street,
                            ZipCode = ZipCode,
                            IsActive = true,
                            CreateUser = Convert.ToInt32(LoggedInEmployeeId),
                            CreateDate = DateTime.Now,
                            AddressDetail = addressDetail,
                            PostOffice = postOffice
                        };
                        var addressSave = employeeAddressService.Create(emp_Address);


                    }

                }

                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.ToString();
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveEmployeeSameAddress(string addType, string country, string state, string dist, string thana, string unionId, string street,
            string ZipCode, string mode, string addressEditId, string empMasterId, string addressDetail)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkAddressExist =
                   employeeAddressService.GetAll().Where(p => p.EmployeeId == Convert.ToInt64(empMasterId) && p.IsActive == true).ToList();

                if (checkAddressExist.Count == 0)
                {
                    result = 0;
                    message = "Please Generate Employee Address First";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (addType == "Pr")
                    addType = "Pe";
                else if (addType == "Pe")
                    addType = "Pr";

                var checkDuplicateAddress = employeeAddressService.GetAll().Where(p =>
                               p.EmployeeId == Convert.ToInt64(empMasterId) &&
                               p.AddressType.Trim().ToUpper() == addType.Trim().ToUpper() && p.IsActive == true)
                       .ToList();
                if (checkDuplicateAddress.Any())
                {
                    result = 0;
                    message = "Employee Address Already Exists";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (street != null && street == "")
                {
                    street = "N/A";
                }
                if (ZipCode != null && ZipCode == "")
                {
                    ZipCode = "N/A";
                }
                long Emp_Address_Id = 0;

                var emp_Address = new EmployeeAddress()
                {
                    EmployeeId = Convert.ToInt64(empMasterId),
                    AddressType = addType,
                    CountryId = country == "" ? 0 : Convert.ToInt32(country),
                    StateOrProvinceId = state == "" ? 0 : Convert.ToInt32(state),
                    DistrictId = dist == "" ? 0 : Convert.ToInt32(dist),
                    ThanaId = thana == "" ? 0 : Convert.ToInt32(thana),
                    UnionId = unionId == "" ? 0 : Convert.ToInt32(unionId),
                    StreetOrHouse = street,
                    ZipCode = ZipCode,
                    IsActive = true,
                    CreateUser = Convert.ToInt32(LoggedInEmployeeId),
                    CreateDate = DateTime.Now,
                    AddressDetail = addressDetail,
                };
                var addressSave = employeeAddressService.Create(emp_Address);
                if (addressSave.AddressId > 0)
                {
                    Emp_Address_Id = addressSave.AddressId;
                }
                else
                {
                    Emp_Address_Id = 0;
                }
                result = 1;
                message = "Saved successfully";

                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.ToString();
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveEmpFamilyInfo(string refName, string refRelation, string refGender, string refDOB, string refOccupation,
            string educationalQualification, string mode, string familyInfoEditId, string empMasterId)
        {
            long Emp_FamilyInfo_Id = 0;
            var logInUserId = SessionHelper.LoggedInEmployeeID;
            try
            {
                if (mode == "S")
                {
                    var loginId =
                        employeeInformationApprovalService.GetAll()
                            .Where(p => p.IsActive == true && p.EmployeeId == logInUserId)
                            .FirstOrDefault();

                    if (loginId != null)
                    {
                        var emp_FamilyInfo = new EmployeeFamilyInfo();
                        emp_FamilyInfo.EmployeeId = Convert.ToInt64(empMasterId);
                        emp_FamilyInfo.Name = refName;
                        emp_FamilyInfo.Relation = refRelation.Trim();
                        emp_FamilyInfo.Gender = refGender.Trim();

                        if (refDOB != "")
                        {
                            refDOB = refDOB.Replace("/", "-");
                            emp_FamilyInfo.DateOfBirth = Convert.ToDateTime(refDOB);
                        }

                        emp_FamilyInfo.Occupation = refOccupation;
                        emp_FamilyInfo.EducationalQualification = educationalQualification;
                        emp_FamilyInfo.IsActive = true;
                        emp_FamilyInfo.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                        emp_FamilyInfo.CreateDate = DateTime.Now;
                        emp_FamilyInfo.IsApproved = true;
                        emp_FamilyInfo.IsRejected = false;
                        emp_FamilyInfo.ApprovedOrRejectedBy = 0;
                        emp_FamilyInfo.ApprovalOrRejectDate = DateTime.UtcNow;
                        var familyInfoSave = employeeFamilyInfoService.Create(emp_FamilyInfo);
                        if (familyInfoSave.FamilyInfoId > 0)
                            Emp_FamilyInfo_Id = familyInfoSave.FamilyInfoId;
                        else
                            Emp_FamilyInfo_Id = 0;
                    }
                    else
                    {
                        var emp_FamilyInfo = new EmployeeFamilyInfo();
                        emp_FamilyInfo.EmployeeId = Convert.ToInt64(empMasterId);
                        emp_FamilyInfo.Name = refName;
                        emp_FamilyInfo.Relation = refRelation.Trim();
                        emp_FamilyInfo.Gender = refGender.Trim();
                        if (refDOB != "")
                        {
                            refDOB = refDOB.Replace("/", "-");
                            emp_FamilyInfo.DateOfBirth = Convert.ToDateTime(refDOB);
                        }
                        emp_FamilyInfo.Occupation = refOccupation;
                        emp_FamilyInfo.EducationalQualification = educationalQualification;
                        emp_FamilyInfo.IsActive = true;
                        emp_FamilyInfo.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                        emp_FamilyInfo.CreateDate = DateTime.Now;
                        emp_FamilyInfo.IsApproved = false;
                        emp_FamilyInfo.IsRejected = false;
                        emp_FamilyInfo.ApprovedOrRejectedBy = 0;
                        emp_FamilyInfo.ApprovalOrRejectDate = DateTime.UtcNow;
                        var familyInfoSave = employeeFamilyInfoService.Create(emp_FamilyInfo);
                        if (familyInfoSave.FamilyInfoId > 0)
                            Emp_FamilyInfo_Id = familyInfoSave.FamilyInfoId;
                        else
                            Emp_FamilyInfo_Id = 0;
                    }
                }
                else if (mode == "U")
                {
                    var emp = employeeFamilyInfoService.GetByFamilyInfoId(Convert.ToInt64(familyInfoEditId));
                    emp.Name = refName;
                    emp.Relation = refRelation;
                    emp.Gender = refGender;

                    if (refDOB != "")
                        emp.DateOfBirth = Convert.ToDateTime(refDOB);

                    emp.Occupation = refOccupation;
                    emp.EducationalQualification = educationalQualification;
                    emp.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    emp.UpdateDate = DateTime.Now;
                    employeeFamilyInfoService.Update(emp);
                    Emp_FamilyInfo_Id = Convert.ToInt64(familyInfoEditId);
                }

                return Json(Emp_FamilyInfo_Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(Emp_FamilyInfo_Id, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetDateForJobConfirmation(int EmployeeStatusId, int AgreementPeriodInMonth, string FirstJoiningDateMsg)
        {
            var Result = 0;
            object AddMonth = null;
            try
            {
                if (EmployeeStatusId == 3)
                {

                    DateTime NewDate = Convert.ToDateTime(FirstJoiningDateMsg);
                    AddMonth = NewDate.AddMonths(AgreementPeriodInMonth).ToString("dd-MMM-yyyy");

                }
            }
            catch (Exception)
            {
                Result = 0;
                throw;
            }


            var data = new { FirstJoiningDateMsg = FirstJoiningDateMsg, AddMonth = AddMonth };


            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveLanguageFluency(string qid, long EmployeeId, string languageName, string efficiency)
        {
            var Result = 0;
            try
            {
                if (EmployeeId != 0)
                {

                    if (qid == "" && languageName != "" && efficiency != "")
                    {

                        var entity = new EmployeeOtherQualification();
                        entity.EmployeeId = EmployeeId;
                        entity.Language = languageName;
                        entity.FluencyLevel = efficiency;
                        entity.IsActive = true;
                        entity.CreateUser = LoggedInEmployeeId;
                        entity.CreateDate = DateTime.Now;
                        employeeOtherQualificationService.Create(entity);
                        Result = 1;

                    }
                    else if (qid != "" && languageName != "" && efficiency != "")
                    {

                        var entity = employeeOtherQualificationService.GetById(Convert.ToInt32(qid));
                        entity.Language = languageName;
                        entity.FluencyLevel = efficiency;
                        entity.UpdateDate = DateTime.Now;
                        entity.UpdateUser = LoggedInEmployeeId;

                        employeeOtherQualificationService.Update(entity);
                        Result = 2;
                    }
                }
            }
            catch (Exception)
            {
                Result = 0;
                throw;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLanguageFluencyList(string EmployeeId)
        {
            var Result = 0;
            try
            {
                long EmpId = Convert.ToInt64(EmployeeId);
                var list = employeeOtherQualificationService.GetByEmployeeId(EmpId);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Result = 0;
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMedicalInfoList(string EmployeeId)
        {
            var employeeId = Convert.ToInt64(EmployeeId);
            var Result = 0;
            var message = "";
            object data = "";
            try
            {
                long EmpId = Convert.ToInt64(EmployeeId);
                var MedicalInfoList =
                    employeeMedicalInfoService.GetMany(p => p.EmployeeId == employeeId).ToList(); //GetByEmployeeId(EmpId);

                return Json(MedicalInfoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Result = 0;
                //throw;
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetEmergencyContactList(string EmployeeId)
        {
            var result = 0;
            var message = "";
            object data = "";
            try
            {
                long EmpId = Convert.ToInt64(EmployeeId);
                var EmergencyList = employeeEmergencyContactService.GetByEmployeeId(EmpId);
                data = EmergencyList;
                //return Json(EmergencyList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result = 0;
                message = "Error Occurde";
            }
            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmpOtherQualification(long EmployeeId, string computerEfficiency)
        {
            var Result = 0;
            try
            {

                if (EmployeeId != 0)
                {
                    if (computerEfficiency != "")
                    {
                        var entity = employeeService.GetByEmpId(EmployeeId);
                        entity.ComputerEfficiency = computerEfficiency;
                        employeeService.Update(entity);
                        Result = 1;
                    }
                }
            }
            catch (Exception)
            {
                Result = 0;
                throw;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveEmpEmergencyContact(string emerId, long EmployeeId, string EmergencyContactName, string EmergencyRelation, string EmergencyMobile,
            string EmergencyTelephone, string EmergencyOwnEmail, string EmergencyOfficialEmail, string EmergencyAddress)
        {

            var Result = 0;
            try
            {

                if (EmployeeId != 0 && emerId == "")
                {
                    var entity = new EmployeeEmergencyContact();

                    entity.EmployeeId = EmployeeId;
                    entity.ContactName = EmergencyContactName;
                    entity.Relation = EmergencyRelation;
                    entity.Mobile = EmergencyMobile;
                    entity.Telephone = EmergencyTelephone;
                    entity.OwnEmail = EmergencyOwnEmail;
                    entity.OfficialEmail = EmergencyOfficialEmail;
                    entity.Address = EmergencyAddress;
                    entity.IsActive = true;
                    entity.CreateUser = LoggedInEmployeeId;
                    entity.CreateDate = DateTime.Now;

                    employeeEmergencyContactService.Create(entity);
                    Result = 1;
                }
                else if (EmployeeId != 0 && emerId != "")
                {
                    var entity = employeeEmergencyContactService.GetById(Convert.ToInt32(emerId));

                    //entity.EmployeeId = EmployeeId;
                    entity.ContactName = EmergencyContactName;
                    entity.Relation = EmergencyRelation;
                    entity.Mobile = EmergencyMobile;
                    entity.Telephone = EmergencyTelephone;
                    entity.OwnEmail = EmergencyOwnEmail;
                    entity.OfficialEmail = EmergencyOfficialEmail;
                    entity.Address = EmergencyAddress;
                    //entity.IsActive = true;
                    entity.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    entity.UpdateDate = DateTime.Now;

                    employeeEmergencyContactService.Update(entity);
                    Result = 2;
                }
            }
            catch (Exception ex)
            {
                Result = 0;
                throw;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmpMedicalInfo(string MedicalInfoId, long EmployeeId, string MedicalInfoOf, string PersonBloodGroup, bool HasBloodPressure,
            string BloodPressureType, bool HasDiabetics, bool HasHeartDisease, bool HasAlergy, bool HasOtherDisease, bool XRayChest, bool VDRL,
            bool HBsAgE, bool VisionTest, string Weight, string Height, string MedicalRemarks)
        {

            var Result = 0;
            try
            {

                if (EmployeeId != 0 && MedicalInfoId == "")
                {
                    var entity = new EmployeeMedicalInfo();
                    entity.EmployeeId = EmployeeId;
                    entity.MedicalInfoOf = MedicalInfoOf;
                    entity.PersonBloodGroup = PersonBloodGroup;
                    entity.HasBloodPressure = HasBloodPressure;
                    entity.BloodPressureType = BloodPressureType;
                    entity.HasDiabetics = HasDiabetics;
                    entity.HasHeartDisease = HasHeartDisease;
                    entity.HasAlergy = HasAlergy;
                    entity.HasOtherDisease = HasOtherDisease;
                    entity.XRayChest = XRayChest;
                    entity.VDRL = VDRL;
                    entity.HBsAgE = HBsAgE;
                    entity.VisionTest = VisionTest;
                    entity.Weight = Weight;
                    entity.Height = Height;
                    entity.MedicalRemarks = MedicalRemarks;
                    entity.IsActive = true;
                    entity.CreateUser = LoggedInEmployeeId;
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateUser = LoggedInEmployeeId;
                    entity.UpdateDate = DateTime.UtcNow;

                    employeeMedicalInfoService.Create(entity);
                    Result = 1;
                }
                else if (EmployeeId != 0 && MedicalInfoId != "")
                {
                    var entity = employeeMedicalInfoService.GetById(Convert.ToInt32(MedicalInfoId));

                    //entity.EmployeeId = EmployeeId;
                    entity.MedicalInfoOf = MedicalInfoOf;
                    entity.PersonBloodGroup = PersonBloodGroup;
                    entity.HasBloodPressure = HasBloodPressure;
                    entity.BloodPressureType = BloodPressureType;
                    entity.HasDiabetics = HasDiabetics;
                    entity.HasHeartDisease = HasHeartDisease;
                    entity.HasAlergy = HasAlergy;
                    entity.XRayChest = XRayChest;
                    entity.VDRL = VDRL;
                    entity.HBsAgE = HBsAgE;
                    entity.VisionTest = VisionTest;
                    entity.Weight = Weight;
                    entity.Height = Height;
                    entity.HasOtherDisease = HasOtherDisease;
                    entity.MedicalRemarks = MedicalRemarks;
                    //entity.IsActive = true;
                    entity.UpdateUser = LoggedInEmployeeId;
                    entity.UpdateDate = DateTime.Now;

                    employeeMedicalInfoService.Update(entity);
                    Result = 2;
                }
            }
            catch (Exception ex)
            {
                Result = 0;
                throw;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFileAttachment(TempAttachment fileAttachment, EmployeeFileAttachemnt RelatedInfo)
        {
            var result = string.Empty;
            try
            {
                var isConditionOk = true;
                if (RelatedInfo.EmployeeId == null || RelatedInfo.DocumentTypeId == null)
                {
                    result = "Employee Id And Document Type is required for Saving Attachment";
                    isConditionOk = false;
                }
                if (isConditionOk == true)
                {
                    string fileConstraint = System.Guid.NewGuid().ToString();
                    var filePath = SaveFiletoFileSystem(fileAttachment.AttachmentContent, fileAttachment.ContentFileName, fileConstraint);

                    var entity = new EmployeeFileAttachemnt();
                    entity.EmployeeId = RelatedInfo.EmployeeId;
                    entity.DocumentTypeId = RelatedInfo.DocumentTypeId;
                    entity.FileName = fileAttachment.ContentFileName;
                    entity.FileLocation = filePath;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    //entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    //entity.UpdateDate = DateTime.UtcNow;

                    employeeFileAttachemntService.Create(entity);
                    result = "Saved Successfully";
                }

            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEmployeeImagetoFileSystem(TempAttachment fileAttachment, EmployeeFileAttachemnt RelatedInfo)
        {
            var result = 0;
            var message = "";
            try
            {
                var company = companyService.GetAll().FirstOrDefault(x => x.IsActive == true);
                var companyShortName = company.CompanyShortName;
                if (companyShortName != "")
                {
                    string imgFolder = companyShortName + "_EmployeeProfileImage";

                    bool exists = System.IO.Directory.Exists(Server.MapPath("~//" + imgFolder));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~//" + imgFolder));
                    }

                    var employee = employeeService.GetByEmpId(RelatedInfo.EmployeeId);
                    var employeeCode = employee.EmployeeCode.Trim();
                    var filePath = Server.MapPath("~//" + imgFolder + "/") + "_" + employeeCode.Trim() + "_" + fileAttachment.ContentFileName.Trim();
                    System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(fileAttachment.AttachmentContent));
                    var imgUrl = "/" + imgFolder + "/" + "_" + employeeCode + "_" + fileAttachment.ContentFileName.Trim();

                    employee.EmployeeImageLink = imgUrl;
                    employeeService.Update(employee);
                    result = 1;
                }
            }
            catch (Exception e)
            {
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttachmentList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, long employeeId)
        {
            try
            {
                var empList = employeeFileAttachemntService.GetMany(x => x.IsActive == true && x.EmployeeId == employeeId).ToList();
                var docType = documentTypeService.GetMany(d => d.IsActive == true);

                var viewList = (from eList in empList
                                join dList in docType on eList.DocumentTypeId equals dList.DocumentTypeId
                                select new EmployeeFileAttachemntViewModel
                                {
                                    AttachmentId = eList.AttachmentId,
                                    DocumentType = dList.TypeName,
                                    FileName = eList.FileName,
                                    FileLocation = eList.FileLocation,
                                }).ToList();

                var currentPageRecords = viewList.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewList.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteFile(int Id)
        {
            var message = string.Empty;
            try
            {
                var file = employeeFileAttachemntService.GetById(Id);
                file.IsActive = false;
                file.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                file.UpdateDate = DateTime.UtcNow;
                employeeFileAttachemntService.Update(file);
                message = "File Deleted Successfully";
            }
            catch (Exception e)
            {
                message = "File Deletion Failed";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmployeeSupervisor(long EmployeeId, long SupervisorId)
        {
            var result = 0;
            var message = "";
            if (EmployeeId > 0 && SupervisorId > 0)
            {

                try
                {
                    var checkDuplicateSupervisor = employeeSupervisorService.GetAll().Where(p => p.IsActive == true && p.EmployeeId == EmployeeId && p.SupervisorId == SupervisorId).ToList();
                    if (checkDuplicateSupervisor.Any())
                    {
                        return Json(new { result = 0, message = "Duplicate Supervisor Found for the Employee, Save Denied" }, JsonRequestBehavior.AllowGet);
                    }
                    var entity = new EmployeeSupervisor();
                    entity.EmployeeId = EmployeeId;
                    entity.SupervisorId = SupervisorId;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.UtcNow;

                    employeeSupervisorService.Create(entity);
                    result = 1;
                    message = "Employee Supervisor Saved Successfully";
                }
                catch (Exception e)
                {
                    result = 0;
                    message = e.Message;
                }
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmployeeSupervisorInfo(int jtStartIndex, int jtPageSize, string jtSorting, int EmployeeId)
        {
            var param = new { EmployeeId = EmployeeId };
            var supervisorList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetSupervisorList");
            var view_List = supervisorList.Tables[0].AsEnumerable()
           .Select(row => new EmployeeSupervisorViewModel
           {
               rowSl = row.Field<string>("rowSl"),
               Id = row.Field<int>("Id"),
               EmployeeId = row.Field<long>("EmployeeId"),
               SupervisorId = row.Field<long>("SupervisorId"),
               SupervisorName = row.Field<string>("SupervisorName"),

           }).ToList();
            var currentPageRecords = view_List.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_List.LongCount() }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteSupervisorInfo(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeSupervisorService.GetById(Id);
                model.IsActive = false;
                model.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                model.UpdateDate = DateTime.UtcNow;
                employeeSupervisorService.Update(model);
                result = 1;
                message = "Supervisor deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployeeRoasterSchedule(int id)
        {
            var result = 0;
            var message = "";

            try
            {
                var deletedRoasterSchedule = roasterEmployeeScheduleService.GetById(id);
                deletedRoasterSchedule.IsActive = false;
                deletedRoasterSchedule.UpdateBy = Convert.ToInt64(LoggedInEmployeeId);
                deletedRoasterSchedule.UpdateDate = DateTime.UtcNow;

                var response = roasterEmployeeScheduleService.Delete(deletedRoasterSchedule);

                result = 1;
                if (!response.IsSuccess)
                    result = 0;

                message = response.Message;
            }
            catch (Exception)
            {
                result = 0;
                message = "Warning, Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupervisorListByDepartment(int OfficeTypeId, string HOId, string PRId, string ZoneId, string AreaId, string UnitId,
            int SupervisorDepartmentId, long CurrentEmployeeId)
        {
            var viewList = new List<SelectListItem>();
            viewList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            int officeId = 0;
            try
            {
                if (OfficeTypeId > 0)
                {
                    if (OfficeTypeId == 1)
                    {
                        officeId = Convert.ToInt32(HOId);
                    }
                    else if (OfficeTypeId == 3)
                    {
                        officeId = Convert.ToInt32(PRId);
                    }
                    else if (OfficeTypeId == 4)
                    {
                        officeId = Convert.ToInt32(ZoneId);
                    }
                    else if (OfficeTypeId == 5)
                    {
                        officeId = Convert.ToInt32(AreaId);
                    }
                    else if (OfficeTypeId == 6)
                    {
                        officeId = Convert.ToInt32(UnitId);
                    }
                }
                var param = new { OfficeId = officeId, DepartmentId = SupervisorDepartmentId, CurrentEmployeeId = CurrentEmployeeId };
                var supervisorList = employeeSPService.GetDataWithParameter(param, "emp.SP_GET_AllEmployeeByDepartment_Office");

                var empList = supervisorList.Tables[0].AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.Field<string>("EmployeeName"),
                    Value = row.Field<long>("EmployeeId").ToString()
                }).ToList();

                viewList.AddRange(empList);
            }
            catch (Exception e)
            {
                throw;
            }
            return Json(viewList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupervisorList(int OfficeTypeId, string HOId, string PRId, string ZoneId, string AreaId, string UnitId,
            string EmployeeRank, int SupervisorDepartmentId, long CurrentEmployeeId)
        {
            var viewList = new List<SelectListItem>();
            viewList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            int officeId = 0;
            try
            {
                if (OfficeTypeId > 0)
                {
                    if (OfficeTypeId == 1)
                    {
                        officeId = Convert.ToInt32(HOId);
                    }
                    else if (OfficeTypeId == 3)
                    {
                        officeId = Convert.ToInt32(PRId);
                    }
                    else if (OfficeTypeId == 4)
                    {
                        officeId = Convert.ToInt32(ZoneId);
                    }
                    else if (OfficeTypeId == 5)
                    {
                        officeId = Convert.ToInt32(AreaId);
                    }
                    else if (OfficeTypeId == 6)
                    {
                        officeId = Convert.ToInt32(UnitId);
                    }
                }
                var param = new { OfficeId = officeId, DepartmentId = SupervisorDepartmentId, EmployeeRank = EmployeeRank.Trim(), CurrentEmployeeId = CurrentEmployeeId };
                var supervisorList = employeeSPService.GetDataWithParameter(param, "emp.SP_GET_AllEmployeeByDepartment_Office_OrnamentalDesignation");
                //var list = employeeService.GetAll().Where(x => x.IsActive == true && x.OfficeId == officeId && x.DepartmentId == SupervisorDepartmentId && x.EmployeeRank.Trim() == EmployeeRank.Trim()).ToList();
                var empList = supervisorList.Tables[0].AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.Field<string>("EmployeeName"),
                    Value = row.Field<long>("EmployeeId").ToString()
                }).ToList();

                viewList.AddRange(empList);
            }
            catch (Exception e)
            {
                throw;
            }
            return Json(viewList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetApprovedCurrentOfficeEmployeeRelation(int jtStartIndex, int jtPageSize, string jtSorting, int empId)
        {
            var param = new { EmployeeId = empId };
            var currentOfficeEmployeeRelationInfo = employeeSPService.GetDataWithParameter(param, "emp.SP_GetApprovedCurrentOfficeEmployeeRelationInfo");

            var viewCurrentOfficeEmployeeRelationInfo =
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
            var currentPageRecords = viewCurrentOfficeEmployeeRelationInfo.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewCurrentOfficeEmployeeRelationInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult GetApprovedInterOrgEmployeeRelation(int jtStartIndex, int jtPageSize, string jtSorting, int empId)
        {
            var interOrgEmployee =
                linkWithEmployeeService.GetAll()
                    .Where(p => p.EmployeeId == empId && p.IsActive == true && p.IsApproved == true)
                    .ToList();
            var currentPageRecords = interOrgEmployee.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = interOrgEmployee.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult SaveReceivedCertificateLists(List<EmployeeViewModel> ProposalList)
        {
            var data = "";
            var message = "";
            var duplicate_message = "";

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (var proposal in ProposalList)
                    {
                        var empCode = employeeService.GetByEmpId(proposal.EmployeeId).EmployeeCode;
                        var model = new ReceivedCertificates();

                        var isDuplicate = receivedCertificatesService.GetMany(p => p.EmployeeId == proposal.EmployeeId && p.IsActive == true && p.DegreeCode.ToUpper().Trim() == proposal.DegreeCode.ToUpper().Trim() && p.EmployeeCertificateStatus != "Return");

                        if (isDuplicate.Any())
                            duplicate_message = "Certificate Saved Successfully Except Duplicate";
                        else
                        {
                            model.EmployeeId = proposal.EmployeeId;
                            model.EmployeeCode = empCode;
                            model.Memo = Convert.ToInt32(proposal.Memo);
                            model.DegreeCode = proposal.DegreeCode;
                            model.CertificateType = proposal.CertificateType;
                            model.EmployeeCertificateStatus = proposal.EmployeeCertificateStatus;
                            model.Comment = proposal.Comment;
                            model.StatusDate = Convert.ToDateTime(proposal.StatusDate);
                            model.IsActive = true;
                            model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            model.CreateDate = DateTime.UtcNow;
                            model.UpdateDate = DateTime.UtcNow;
                            receivedCertificatesService.Create(model);
                        }
                    }

                    data = "OK";
                    message = "Certificate Save Successfully";
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    data = "Error";
                    message = "Certificate Save Denied";
                    scope.Dispose();
                }

                if (!String.IsNullOrEmpty(duplicate_message))
                {
                    message = duplicate_message;
                }
            }

            return Json(new { data = data, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaxMemoNumber(int employeId)
        {

            int nomemo = 0;
            bool certificate_exist = false;

            var chekk_memo = receivedCertificatesService.GetAll().Where(x => x.IsActive == true && x.EmployeeId == employeId).ToList();
            if (chekk_memo.Any())
            {
                nomemo = chekk_memo.FirstOrDefault().Memo;
                certificate_exist = true;
            }

            else
            {
                int maxMemoNo = 0;
                var certificates = receivedCertificatesService.GetAll().Where(x => x.IsActive == true);
                if (certificates.Any())
                {
                    maxMemoNo = certificates.Max(p => p.Memo);
                }
                nomemo = maxMemoNo + 1;
            }

            return Json(new { nomemo = nomemo, certificate_exist = certificate_exist }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformationStatus(bool stsvalue)
        {
            var statusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: stsvalue);
            return Json(new { Data = statusList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeInfoByEmployeeCode(string employeeCode)
        {
            try
            {
                var withResignEmployee = false;

                //get employee information
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim(), withResignEmployee);

                if (employeeInfo == null)
                    return Json(new { type = "warning", message = "Employee not exist. Please try again!" },
                           JsonRequestBehavior.AllowGet);

                var employeeRelatedInfo = new
                {
                    EmployeeId = employeeInfo.EmployeeId,
                    EmployeeName = employeeInfo.EmployeeName,
                    EmployeeEmployeeStatus = EmployeeStatusConstants.GetText(employeeInfo.EmployeeStatusId.ToString()),
                    EmployeeDesignationStatus = employeeInfo.EmployeeDesignation.DesignationName,
                    EmployeeDepartment = employeeInfo.EmployeeDepartment.DepartmentName,
                };

                return Json(new { type = "success", employeeInfo = employeeRelatedInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "Employee not exist. Please try again!" },
                          JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Methods

        private void MapDropDownList(EmployeeViewModel model)
        {
            model.EmployeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList(IsValid: model.IsValidEmployeeStatus);//empStatus;
            model.AgreementPeriodInMonthList = commonStaticDropDown.GetPeriodInMonthsList();//periodInMonth;
            model.StatusPeriodInMonthList = commonStaticDropDown.GetPeriodInMonthsList(); //statusperiodInMonth;
            if (SessionHelper.CompanyInfo.CompanyShortName == "SNG")
            {
                model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList_SP();//desig_items;
                model.RankList = commonDynamicDropDown.GetAllOfficeDesignationList_SP();//officeOrnamentDesign_items;
            }
            else
            {
                model.RankList = commonDynamicDropDown.GetAllOfficeDesignationList();//officeOrnamentDesign_items;
                model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();//desig_items;
            }


            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();//officeDesign_items;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();//officeType_items;
            model.HOList = commonDynamicDropDown.GetHeadOfficeList();//viewHOList;
            model.ProjectList = commonDynamicDropDown.GetProjectOfficeList();//viewProjectList;
            model.OfficeList = commonStaticDropDown.ddlInitial();//ofc_items;

            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();//depList;

            model.SameAddressList = commonStaticDropDown.GetYesNoList();//sameAddressList;
            model.ReligionList = commonStaticDropDown.GetReligionsList();//empReligion;
            model.GenderList = commonStaticDropDown.GetGendersList(); //empGender;
            model.MaritalList = commonStaticDropDown.GetMaritalStatusList();//empMarital;
            model.CertificateTypeList = commonStaticDropDown.GetEducationCertificateTypeList();//CertificateType;

            model.SupervisorList = commonDynamicDropDown.GetHeadOfficeList();//supervisor_items;
            model.SupervisorDeptList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.SupervisorOrnamentalDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();//viewList;

            var CertificateempStatuss = new List<SelectListItem>();
            CertificateempStatuss.Add(new SelectListItem() { Text = "Receive", Value = "Receive", Selected = true });
            CertificateempStatuss.Add(new SelectListItem() { Text = "Temporary Withdrawal", Value = "TemporaryWithdrawal" });
            CertificateempStatuss.Add(new SelectListItem() { Text = "Return", Value = "Return" });
            model.EmployeeCertificateStatusList = CertificateempStatuss;

            var empSalaryType = new List<SelectListItem>();
            empSalaryType.Add(new SelectListItem { Text = "Please Select", Value = "" });
            empSalaryType.Add(new SelectListItem() { Text = "PayScale", Value = "1", Selected = true });
            empSalaryType.Add(new SelectListItem() { Text = "Non PayScale", Value = "2" });
            model.EmployeeSalaryType = empSalaryType;

            //Employee Education
            //ResultType
            var resultStatus = new List<SelectListItem>();
            resultStatus.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            resultStatus.Add(new SelectListItem() { Text = "Grade", Value = "G" });
            resultStatus.Add(new SelectListItem() { Text = "Division", Value = "D" });
            resultStatus.Add(new SelectListItem() { Text = "Appeared", Value = "A" });
            resultStatus.Add(new SelectListItem() { Text = "Enrolled", Value = "E" });
            resultStatus.Add(new SelectListItem() { Text = "Passed", Value = "P" });
            resultStatus.Add(new SelectListItem() { Text = "Class", Value = "C" });

            model.ResultTypeList = resultStatus;

            //Employee Address

            //Address Type Dropdown
            var addressTyp = new List<SelectListItem>();
            addressTyp.Add(new SelectListItem() { Text = "Present Address", Value = "Pr", Selected = true });
            addressTyp.Add(new SelectListItem() { Text = "Permanent Address", Value = "Pe" });
            //addressTyp.Add(new SelectListItem() { Text = "Emergency Contact", Value = "Ec" });
            model.AddressTypeList = addressTyp;

            //Country Dropdown
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

            model.StateOrProvinceList = commonStaticDropDown.ddlInitial();//stateList;
            model.DistrictList = commonStaticDropDown.ddlInitial();// districtList;
            model.ThanaList = commonStaticDropDown.ddlInitial();//thanaList;
            model.UnionList = commonStaticDropDown.ddlInitial();//unionList;

            model.AreaList = commonStaticDropDown.ddlInitial();//area_items;
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();//zone_items;
            model.UnitList = commonStaticDropDown.ddlInitial();//unit_items;

            // Relation List
            model.relationWithEmployeeList = commonStaticDropDown.GetRelationTypeList();

            var empRetiredCause = new List<SelectListItem>();
            empRetiredCause.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            empRetiredCause.Add(new SelectListItem() { Text = "Due to rules violation", Value = "Due to rules violation" });

            model.RetiredCauseList = empRetiredCause;

            model.TerminationCauseList = commonStaticDropDown.ddlInitial();

            var languageList = new List<SelectListItem>();
            languageList.Add(new SelectListItem() { Text = "Bangla", Value = "Bangla" });
            languageList.Add(new SelectListItem() { Text = "English", Value = "English" });
            languageList.Add(new SelectListItem() { Text = "Hindi", Value = "Hindi" });

            model.languageList = languageList;

            var efficiencyList = new List<SelectListItem>();
            efficiencyList.Add(new SelectListItem() { Text = "High", Value = "High" });
            efficiencyList.Add(new SelectListItem() { Text = "Medium", Value = "Medium" });
            efficiencyList.Add(new SelectListItem() { Text = "Low", Value = "Low" });

            model.efficiencyList = efficiencyList;
            var divisionList = new List<SelectListItem>();
            divisionList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            divisionList.Add(new SelectListItem() { Text = "First Division", Value = "First Division" });
            divisionList.Add(new SelectListItem() { Text = "Second Division", Value = "Second Division" });
            divisionList.Add(new SelectListItem() { Text = "Third Division", Value = "Third Division" });
            model.ResultDivisionList = divisionList;


            var classList = new List<SelectListItem>();
            classList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            classList.Add(new SelectListItem() { Text = "First class", Value = "First class" });
            classList.Add(new SelectListItem() { Text = "Second class", Value = "Second class" });
            classList.Add(new SelectListItem() { Text = "Third class", Value = "Third class" });
            model.ResultclassList = classList;

            //MedicalPersonList
            var medicalPersonList = new List<SelectListItem>();
            medicalPersonList.Add(new SelectListItem() { Text = "Own", Value = "Own" });
            medicalPersonList.Add(new SelectListItem() { Text = "Father", Value = "Father" });
            medicalPersonList.Add(new SelectListItem() { Text = "Mother", Value = "Mother" });
            medicalPersonList.Add(new SelectListItem() { Text = "Spouse", Value = "Spouse" });
            medicalPersonList.Add(new SelectListItem() { Text = "Child-1", Value = "Child-1" });
            medicalPersonList.Add(new SelectListItem() { Text = "Child-2", Value = "Child-2" });
            medicalPersonList.Add(new SelectListItem() { Text = "Child-3", Value = "Child-3" });
            medicalPersonList.Add(new SelectListItem() { Text = "Child-4", Value = "Child-4" });
            medicalPersonList.Add(new SelectListItem() { Text = "Child-5", Value = "Child-5" });

            model.MedicalPersonList = medicalPersonList;


            model.BloodGroupList = commonStaticDropDown.GetAllBloodGroupTypeList(); //bloodGroupList;

            //BloodPressureTypeList
            var bloodPressureTypeList = new List<SelectListItem>();
            bloodPressureTypeList.Add(new SelectListItem() { Text = "Low", Value = "Low" });
            bloodPressureTypeList.Add(new SelectListItem() { Text = "High", Value = "High" });

            model.BloodPressureTypeList = bloodPressureTypeList;



            var degreeLevelList = educationDegreeService.GetMany(w => w.CompanyId == 1).DistinctBy(w => new { w.DegreeLevelId, w.DegreeLevel }).ToList();

            var viewdegreeList = degreeLevelList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DegreeLevelId.ToString(),
                Text = x.DegreeLevel.ToString()
            });

            var degree_items = new List<SelectListItem>();
            degree_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            degree_items.AddRange(viewdegreeList);
            model.DegreeLevelList = degree_items;

            //concentration
            //var concentrationList = educationConcentrationService.GetMany(w => w.CompanyId == 1);


            var DegreeList = new List<SelectListItem>();
            DegreeList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.DegreeList = DegreeList;

            model.ConcentrationList = commonDynamicDropDown.ddlInitial();//concentration_items;

            var probationPeriod = new List<SelectListItem>();
            probationPeriod.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            probationPeriod.Add(new SelectListItem() { Text = "3 Months", Value = "3" });
            probationPeriod.Add(new SelectListItem() { Text = "4 Months", Value = "4" });
            probationPeriod.Add(new SelectListItem() { Text = "6 Months", Value = "6" });
            probationPeriod.Add(new SelectListItem() { Text = "1 Year", Value = "12" });
            model.ProbationPeriodList = probationPeriod;


            var emergencyContactList = familyRelationService.GetAll();
            var viewEmergencyContactList = emergencyContactList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.RelationName.ToString(),
                Text = x.RelationName.ToString()
            });
            var contact_items = new List<SelectListItem>();
            contact_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            contact_items.AddRange(viewEmergencyContactList);
            model.EmergencyContactList = contact_items;


            var documenttypelist = documentTypeService.GetMany(p => p.IsActive == true && p.DocumentTypeModuleName == "EP");
            var viewdocumenttypelist = documenttypelist.Select(a => new SelectListItem()
            {
                Value = a.DocumentTypeId.ToString(),
                Text = a.TypeName
            });
            var documenttypelists = new List<SelectListItem>();
            documenttypelists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            documenttypelists.AddRange(viewdocumenttypelist);
            model.DocumentTypeNameList = documenttypelists;


            var certificateStatusList = new List<SelectListItem>();
            certificateStatusList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            certificateStatusList.Add(new SelectListItem() { Text = "Received", Value = "Received" });
            certificateStatusList.Add(new SelectListItem() { Text = "Returned", Value = "Returned" });
            model.StatusList = certificateStatusList;

            var signatureList = employeeSignatureDesignationService.GetMany(p => p.IsActive == true);
            var viewSignaturelist = signatureList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.SignatureName,
                Value = p.SignatureId.ToString()
            }).ToList();
            var signatureDesignationList = new List<SelectListItem>();
            signatureDesignationList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            signatureDesignationList.AddRange(viewSignaturelist);
            model.SignatureDesignationList = signatureDesignationList;

            //var sectionList = new List<SelectListItem>();
            //sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //sectionList.AddRange(viewSection);
            model.SectionList = commonStaticDropDown.ddlInitial();//sectionList;

            var empType = employementTypeService.GetMany(p => p.IsActive == true).OrderBy(p => p.ViewOrder).ToList();
            var viewEmpType = empType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmployementTypeName,
                Value = p.EmployementTypeId.ToString()
            }).ToList();
            var typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            typeList.AddRange(viewEmpType);
            model.EmploymentTypeList = typeList;
        }

        public byte[] GetGuarantorImageFromDataBase(int Id)
        {
            var GuarantorDetail = employeeGuarantorInformationService.GetByGurId(Id);
            var img = GuarantorDetail.GuarantorImage;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;
        }

        public byte[] GetImageFromDataBase(long Id)
        {
            var employeeDetail = employeeService.GetByEmpId(Id);
            var img = employeeDetail.EmployeeImage;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;
        }

        private string GetNewEmployeeCode()
        {
            string new_code = "";

            var empLastId = employeeService.GetAll().Select(d => d.EmployeeId).Max();
            //var empLastId = Use store procedure

            if (empLastId == 0)
            {
                new_code = "00001";
            }
            else
            {
                long last_code = empLastId + 1;
                new_code = last_code.ToString();//.PadLeft(7, '0');               
            }

            return new_code;
        }

        private string SaveFiletoFileSystem(string base64, string fileName, string fileConstraint)
        {
            string docFolder = "UploadedEmployeeDocuments";

            bool exists = System.IO.Directory.Exists(Server.MapPath("~//" + docFolder));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~//" + docFolder));
            }
            var filePath = Server.MapPath("~//" + docFolder + "/") + "_" + fileConstraint.Trim() + "_" + fileName.Trim();

            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
            var hostAddress = Request.Url.OriginalString.Replace(Request.Url.LocalPath, "");

            return "/UploadedEmployeeDocuments/" + "_" + fileConstraint + "_" + fileName;
        }

        private Employee GenerateEmployee(EmployeeViewModel model)
        {
            var entity = new Employee();

            if (model.EmployeeId > 0)
            {
                entity = employeeService.GetByEmpId(model.EmployeeId);
            }

            entity.EmployeeCode = model.EmployeeCode;
            entity.EmployeeName = model.EmployeeName;
            entity.EmployeeNameBng = model.EmployeeNameBng;

            entity.CompanyId = CompanyID;// from Session
            entity.OfficeId = model.OfficeId;
            entity.DepartmentId = model.DepartmentId;
            entity.SectionId = model.SectionId;
            entity.EmployeeRank = model.EmployeeRank;
            entity.DesignationId = model.DesignationId;
            entity.SignatureDesignationId = model.SignatureDesignationId;
            entity.EmploymentTypeId = model.EmploymentTypeId;
            entity.EmployeeStatusId = model.EmployeeStatusId;

            //entity.EmployeeStatus = status.Trim();
            entity.StatusPeriodInMonth = model.StatusPeriodInMonth;
            entity.StatusChangeComment = model.StatusChangeComment;

            //if (!string.IsNullOrEmpty(model.StatusDate))
            //{
            entity.StatusDate = model.StatusDate;
            //}

            //if (!string.IsNullOrEmpty(model.StatusFromDate))
            //{
            entity.StatusFromDate = model.StatusFromDate;
            // }

            //if (!string.IsNullOrEmpty(model.StatusToDate))
            //{
            entity.StatusToDate = model.StatusToDate;
            // }

            //if (!string.IsNullOrEmpty(model.FirstJoiningDate))
            //{
            entity.FirstJoiningDate = model.FirstJoiningDate;
            //}

            //if (!string.IsNullOrEmpty(model.ConfirmationDate))
            //{
            entity.ConfirmationDate = model.ConfirmationDate;
            //}

            //entity.ConfirmationDate = confirmDate ?? Convert.ToDateTime(confirmDate);

            entity.AgreementPeriodInMonth = model.AgreementPeriodInMonth;

            //if (!string.IsNullOrEmpty(model.AgreementFromDate))
            //{
            entity.AgreementFromDate = model.AgreementFromDate;
            //}

            //if (!string.IsNullOrEmpty(model.AgreementToDate))
            //{
            // entity.AgreementToDate = model.AgreementToDate;
            //}

            //entity.GrossSalary = grossSalary;
            entity.PermanentDate = model.PermanentDate;
            entity.JobExperience = model.JobExperience;
            if (entity.EmployeeStatusId == 22)
            {
                entity.IsActive = false;
            }
            else
            {
                entity.IsActive = true;
            }

            entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            entity.UpdateDate = DateTime.Now;

            return entity;
        }

        private bool InsertHistory(Employee model)
        {
            bool insertSuccess = true;
            try
            {
                // make previous history inactive
                var previousHistory = employeeStatusHistoryService.GetByEmployeeId(model.EmployeeId);
                if (previousHistory != null)
                {
                    previousHistory.IsActive = false;
                    employeeStatusHistoryService.Update(previousHistory);
                }

                // insert new history
                var newHistory = new EmployeeStatusHistory();
                newHistory.EmployeeId = model.EmployeeId;
                //newHistory.Status = statusId.ToString();
                newHistory.StatusId = model.EmployeeStatusId;
                //if (!string.IsNullOrEmpty(model.StatusToDate))
                //{
                newHistory.StartDate = model.StatusToDate;
                //}

                //if (!string.IsNullOrEmpty(model.ConfirmationDate))
                //{
                newHistory.ConfirmationDate = model.ConfirmationDate;
                //}

                newHistory.IsActive = true;
                newHistory.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                newHistory.CreateDate = DateTime.Now;

                //let's create employee history [dbo.EmployeeStatusHistory]
                employeeStatusHistoryService.Create(newHistory);
            }
            catch (Exception ex)
            {
                insertSuccess = false;
            }
            return insertSuccess;
        }

        private void mapOfficeDropdownEdit(EmployeeViewModel model)
        {
            int? officeTypeId = null;

            if (model.OfficeId > 0)
                officeTypeId = officeService.GetById(Convert.ToInt32(model.OfficeId)).OfficeTypeId;

            model.OfficeTypeId = officeTypeId;

            var ofcList = officeService.GetAll().Where(w => (officeTypeId == null || w.OfficeTypeId == officeTypeId));
            var viewOfcList = ofcList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOfcList);
            model.OfficeList = ofc_items;

            var section = employeeDepartmentSectionService.GetAll().Where(p => p.IsActive == true && p.DepartmentId == model.DepartmentId).ToList();
            var viewSection = section.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.SectionName,
                Value = p.SectionId.ToString()
            }).ToList();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            sectionList.AddRange(viewSection);
            model.SectionList = sectionList;
        }

        private void MapDropdownForEmployeeTraining(EmployeeViewModel model)
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
            isapproved.Add(new SelectListItem { Text = "NonApproved", Value = "0" });
            model.isapprovedList = isapproved;

            var isrejected = new List<SelectListItem>();
            isrejected.Add(new SelectListItem { Text = "Please Select", Value = "" });
            isrejected.Add(new SelectListItem { Text = "Rejected", Value = "1" });
            isrejected.Add(new SelectListItem { Text = "NonRejected", Value = "0" });
            model.isrejectedList = isrejected;


        }

        private void MapDropdownForEmployeeGuarantorInformation(EmployeeViewModel model)
        {
            var relationshipnamelist = guarantorRelationshipService.GetAll().Where(p => p.IsActive == true);
            var viewrelationshipnamelist = relationshipnamelist.Select(a => new SelectListItem()
            {
                Value = a.GuarantorRelationshipId.ToString(),
                Text = a.GuarantorRelationshipName
            });
            var listofrelationshipname = new List<SelectListItem>();
            listofrelationshipname.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listofrelationshipname.AddRange(viewrelationshipnamelist);
            model.RelationshipNameList = listofrelationshipname;

            var occupationnamelist = occupationService.GetAll().Where(p => p.IsActive == true);
            var viewoccupationnamelist = occupationnamelist.Select(a => new SelectListItem()
            {
                Value = a.OccupationId.ToString(),
                Text = a.OccupationName
            });
            var listoccupationname = new List<SelectListItem>();
            listoccupationname.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listoccupationname.AddRange(viewoccupationnamelist);
            model.OccupationNameList = listoccupationname;

            var GRType = new List<SelectListItem>();
            GRType.Add(new SelectListItem { Text = "Please Select", Value = "" });
            GRType.Add(new SelectListItem { Text = "Reference", Value = "Reference" });
            GRType.Add(new SelectListItem { Text = "Guarantor", Value = "Guarantor" });
            model.GRTypeList = GRType;


            //Address Type Dropdown
            var addressTyp = new List<SelectListItem>();
            addressTyp.Add(new SelectListItem() { Text = "Present Address", Value = "Pr", Selected = true });
            addressTyp.Add(new SelectListItem() { Text = "Permanent Address", Value = "Pe" });
            //addressTyp.Add(new SelectListItem() { Text = "Emergency Contact", Value = "Ec" });
            model.AddressTypeList = addressTyp;
            //Country Dropdown
            var countryList = countryService.GetAll().Where(w => w.CountryId == CountryID);
            var viewCountryList = countryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString()
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            country_items.AddRange(viewCountryList);
            model.CountryList = country_items;


            //State/Province/Division Dropdown
            //var stateList = sateOrProvinceService.GetAll();
            //var viewStateList = stateList.Select(m => new SelectListItem() { Text =  m.Name, Value = m.StateOrProvinceId.ToString() });
            //model.StateOrProvinceList = viewStateList;

            var stateList = new List<SelectListItem>();
            stateList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.StateOrProvinceList = stateList;

            //District Dropdown
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            //Thana Dropdown
            //var thanaList = thanaService.GetAll();
            //var viewThanaList = thanaList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.thana_code, m.thana_name_eng), Value = m.thana_id.ToString() });
            //model.ThanaList = viewThanaList;
            var thanaList = new List<SelectListItem>();
            thanaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.ThanaList = thanaList;

            //Union Dropdown
            //var unionList = unionService.GetAll();
            //var viewUnionList = unionList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.union_code, m.union_name_eng), Value = m.union_id.ToString() });
            //model.UnionList = viewUnionList;
            var unionList = new List<SelectListItem>();
            unionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UnionList = unionList;



            //var unitCommonTransport = new List<SelectListItem>();
            //unitCommonTransport.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //unitCommonTransport.Add(new SelectListItem() { Text = "Yes", Value = "true" });
            //unitCommonTransport.Add(new SelectListItem() { Text = "No", Value = "false" });
            //model.IsCommonTransportList = unitCommonTransport;
        }

        private void MapDropdownForAttendance(EmployeeViewModel model)
        {
            var fromDate = DateTime.Now;
            var timeKeepingRoasterlist = timeKeepingRosterService.GetTimeKeepingRosterByDate(fromDate);

            var viewAttendancelist = timeKeepingRoasterlist.Select(a => new SelectListItem()
            {
                Value = a.TimeKeepingRosterId.ToString(),
                Text = a.RosterName
            });
            var listviewAttendancelist = new List<SelectListItem>();
            listviewAttendancelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listviewAttendancelist.AddRange(viewAttendancelist);
            model.AttendanceRosterList = listviewAttendancelist;

        }

        public void ConvertEmployeeExistingImageByteToImageLink()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var company = companyService.GetAll().Where(x => x.IsActive == true).First();
                    var companyShortName = company.CompanyShortName;
                    if (companyShortName != "")
                    {
                        //var filePath = Server.MapPath("~//" + imgFolder + "/") + "_" + fileConstraint.Trim() + "_" + fileName.Trim() + ".png";
                        string imgFolder = companyShortName + "_EmployeeProfileImage"; // your code goes here

                        bool exists = System.IO.Directory.Exists(Server.MapPath("~//" + imgFolder));
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath("~//" + imgFolder));
                        }

                        //var employeeListWithImgByteNotImgLink = employeeService.GetAll().Where(e => e.EmployeeImage != null && e.EmployeeImageLink == null).ToList();
                        var employeeListWithImgByteNotImgLink = employeeService.GetAll().Where(e => e.EmployeeImage != null && e.EmployeeImageLink == null).ToList();
                        var employeeCount = employeeListWithImgByteNotImgLink.Count();
                        var loopCount = 0;
                        if (employeeCount > 0)
                        {
                            foreach (var employee in employeeListWithImgByteNotImgLink)
                            {
                                string base64 = Convert.ToBase64String(employee.EmployeeImage);
                                var fileName = employee.EmployeeName.Trim();
                                var fileConstraint = employee.EmployeeCode.Trim();
                                var imgUrl = SaveImagetoFileSystem(imgFolder, base64, fileName, fileConstraint);
                                employee.EmployeeImageLink = imgUrl;
                                //employeecode = employee.EmployeeCode;
                                employeeService.Update(employee);
                            }
                        }
                    }
                    scope.Complete();
                    RedirectToAction("Index", "Employee");
                }
                catch (Exception e)
                {
                    //var aa = employeecode;
                    scope.Dispose();
                    RedirectToAction("Index", "Employee");
                }
            }
            //  return View();
        }

        private string SaveImagetoFileSystem(string imgFolder, string base64, string fileName, string fileConstraint)
        {
            var filePath = Server.MapPath("~//" + imgFolder + "/") + "_" + fileConstraint.Trim() + "_" + fileName.Trim() + ".png";
            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64));
            var hostAddress = Request.Url.OriginalString.Replace(Request.Url.LocalPath, "");
            return "/" + imgFolder + "/" + "_" + fileConstraint + "_" + fileName + ".png";
        }

        private RoasterEmployeeSchedule PopulateRoasterEmployeeSchedule(int id, int roasterId, int employeeId, TimeKeepingRoster roaster
            , DateTime roasterEffectiveStartDate, DateTime roasterEffectiveEndDate)
        {
            var roasterEmployeeSchedule = new RoasterEmployeeSchedule();
            roasterEmployeeSchedule.Id = id;
            roasterEmployeeSchedule.EmployeeId = Convert.ToInt32(employeeId);
            roasterEmployeeSchedule.RoasterId = roasterId;
            roasterEmployeeSchedule.RoasterName = roaster.RosterName;
            roasterEmployeeSchedule.LoginTime = roaster.LoginTime;
            roasterEmployeeSchedule.LastLoginTime = roaster.LastLoginTime;
            roasterEmployeeSchedule.LogoutTime = roaster.LogoutTime;
            roasterEmployeeSchedule.EffectiveStartDate = roasterEffectiveStartDate;
            roasterEmployeeSchedule.EffectiveEndDate = roasterEffectiveEndDate;

            //for create new
            roasterEmployeeSchedule.IsActive = true;
            roasterEmployeeSchedule.CreateDate = DateTime.UtcNow;
            roasterEmployeeSchedule.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

            //for update
            roasterEmployeeSchedule.UpdateDate = DateTime.UtcNow;
            roasterEmployeeSchedule.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

            return roasterEmployeeSchedule;
        }


        #endregion

        #region PF
        //AS METHOD Was NOT Found from UI Asad bhai source. I added(KHALID/Arefeen)


        public JsonResult GetEmployeeByEmployeeCode(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                {
                    return Json(new
                    {
                        EmployeeName = string.Empty,
                        EmployeeCode = string.Empty,
                        EmployeeId = 0,
                        message = "Employee code is required"
                    }, JsonRequestBehavior.AllowGet);
                }

                var employee = employeeService.GetByCode(employeeCode, false);

                if (employee == null)
                {
                    return Json(new
                    {
                        EmployeeName = string.Empty,
                        EmployeeCode = employeeCode,
                        EmployeeId = 0,
                        message = $"{employeeCode} does not exist"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!employee.IsActive)
                {
                    return Json(new
                    {
                        EmployeeName = employee.EmployeeName,
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeId = employee.EmployeeId,
                        message = $"{employee.EmployeeName} is inactive"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    EmployeeName = employee.EmployeeName,
                    EmployeeCode = employee.EmployeeCode,
                    EmployeeId = employee.EmployeeId,
                    message = ""
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    EmployeeName = string.Empty,
                    EmployeeCode = employeeCode,
                    EmployeeId = 0,
                    message = "Sorry for inconvenience! please try again later"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetEmployeeNameByEmpId(string employeeId)
        {
            string employeeName = string.Empty;
            string message = string.Empty;
            try
            {
                int empId = Convert.ToInt32(employeeId);
                //Getting Employee Name

                Employee objEmployee = new Employee();
                objEmployee = employeeService.GetById(empId);  //empConfigService.GetById(empId);
                if (objEmployee == null)
                {
                    message = employeeId + " does not exist";
                    return Json(new { EmployeeName = string.Empty, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (!objEmployee.IsActive)
                    {
                        message = employeeName + " is inactive";
                        return Json(new { EmployeeName = employeeName, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        employeeName = objEmployee.EmployeeName;
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { EmployeeName = employeeName, message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { EmployeeName = employeeName, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ChildActionOnly
        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult GlobalEmployeeSearch()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            var model = new EmployeeSearchingViewModel();

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return PartialView(model);
        }
        #endregion

        #region Ajax Calls

        public JsonResult GetEmployeeInfo(int employeeId)
        {
            try
            {
                //get employee info
                var employeeInfo = employeeService.GetEmployeeInfo(employeeId);

                return Json(employeeInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeShortInfoByCode(string Code)
        {
            try
            {
                Dictionary<string, object> Info = employeeService.GetEmployeeShortInfoByCode(Code);
                return Json(new { success = true, data = Info });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Private Methods
        //KHALID
        //public ActionResult GenerateReportEmployee(string DownloadExcel)
        //{
        //    try
        //    {
        //        //var param = new { OfficeTypeId = OfficeTypeId };
        //        //var OverdueMls = employeeSPService.GetDataWithParameter(param, "SP_RPT_BranchWiseNoOfEmployee");
        //        var reportParam = new Dictionary<string, object>();

        //        int IsDownloadExcel = Convert.ToInt32(DownloadExcel);
        //        if (IsDownloadExcel == 1)
        //        {
        //            ReportHelper.ExportExcelReport("Employee/rpt_EmployeeList.rpt", empList.Tables[0], reportParam);
        //        }
        //        else
        //        {
        //            //  ReportHelper.PrintReport("Employee/rpt_EmployeeList.rpt", empList.Tables[0], reportParam);
        //            ReportHelper.PrintReport("Employee/rpt_EmployeeListTest.rpt", empList.Tables[0], reportParam);
        //        }

        //        empList.Clear();
        //        return Content(string.Empty);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}

        public ActionResult GenerateReportEmployee([DataSourceRequest] DataSourceRequest request, string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue, string DownloadExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                        sb.Append(" AND es.StatusId ='" + Status[0] + "'");
                }
                else if (Status != null && Status.Count > 1)
                {
                    string statusList = "";
                    var count = 1;
                    foreach (var status in Status)
                    {
                        if (count < Status.Count)
                            statusList = statusList + "'" + status + "', ";
                        else
                            statusList = statusList + "'" + status + "'";
                        count++;
                    }
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                if (DepartmentId != "")
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                if (Responsibility != "")
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);

                if (Section != "")
                    sb.Append(" AND eed.SectionId =" + Section);

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                if (loggedInOfficeTypeId == 4) // ZO
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");

                if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");

                if (OfficeId != "")
                    sb.Append(" AND E.OfficeId =" + OfficeId);

                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoard");


                var reportParam = new Dictionary<string, object>();

                int IsDownloadExcel = Convert.ToInt32(DownloadExcel);
                if (IsDownloadExcel == 1)
                {
                    ReportHelper.ExportExcelReport("Employee/rpt_EmployeeList.rpt", employeeList.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("Employee/rpt_EmployeeList.rpt", employeeList.Tables[0], reportParam);
                }

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }// END Class






        }// END Function


        public ActionResult GenerateReportEmployee2([DataSourceRequest] DataSourceRequest request, string OfficeTypeId, string OfficeId, string DepartmentId, string PayrollDesignation, string Responsibility, string IsValidEmployeeStatus, string Section, List<string> Status, string FilterColumn, string FilterValue, string DownloadExcel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Status != null && Status.Count == 1)
                {
                    if (Status[0] != "")
                        sb.Append(" AND es.StatusId ='" + Status[0] + "'");
                }
                else if (Status != null && Status.Count > 1)
                {
                    string statusList = "";
                    var count = 1;
                    foreach (var status in Status)
                    {
                        if (count < Status.Count)
                            statusList = statusList + "'" + status + "', ";
                        else
                            statusList = statusList + "'" + status + "'";
                        count++;
                    }
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (PayrollDesignation != "")
                    sb.Append(" AND E.DesignationId =" + PayrollDesignation);
                if (DepartmentId != "")
                    sb.Append(" AND E.DepartmentId =" + DepartmentId);
                if (Responsibility != "")
                    sb.Append(" AND E.EmployeeRank =" + Responsibility);

                if (Section != "")
                    sb.Append(" AND eed.SectionId =" + Section);

                var loggedInofficeID = SessionHelper.LoginUserOfficeID;

                var loggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;

                /*
                OfficeTypeId OfficeTypeCode     OfficeTypeName
                        4       ZO              Zonal Office
                        5       AR              Area Office

                */

                var GetOfficeCode = "(SELECT OfficeCode FROM Office WHERE OfficeID=" + loggedInofficeID + ")";

                if (loggedInOfficeTypeId == 4) // ZO
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.SecondLevel = " + GetOfficeCode + ")");

                if (OfficeTypeId != "" && OfficeId == "" && loggedInOfficeTypeId == 1) // AND LoggedIn Office HO
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + OfficeTypeId + ")");

                if (OfficeId != "")
                    sb.Append(" AND E.OfficeId =" + OfficeId);

                if (FilterValue != "")
                {
                    if (FilterColumn == "EmployeeCode")
                        sb.Append(" AND E.EmployeeCode ='" + FilterValue + "'");
                    else if (FilterColumn == "EmployeeName")
                        sb.Append(" AND E.EmployeeName LIKE '%" + FilterValue + "%'");
                    else if (FilterColumn == "Joining")
                        sb.Append(" AND E.FirstJoiningDate ='" + FilterValue + "'");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeListForDashBoard");


                var reportParam = new Dictionary<string, object>();

                int IsDownloadExcel = Convert.ToInt32(DownloadExcel);
                if (IsDownloadExcel == 1)
                {
                    ReportHelper.ExportExcelReport("Employee/rpt_EmployeeList.rpt", employeeList.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("Employee/rpt_EmployeeList.rpt", employeeList.Tables[0], reportParam);
                }

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }// END Class






        }// END Function



        [HttpGet]
        public async Task<ActionResult> GetHealthApi(string phone)
        {
            try
            {
                List<GHealthPatientDetailsBasicCheckUpViewModel> objList = new List<GHealthPatientDetailsBasicCheckUpViewModel>();

                GHealthPatientDetailsBasicCheckUpViewModel obj = new GHealthPatientDetailsBasicCheckUpViewModel();

                using (HttpClient client = new HttpClient())
                {
                    string apiKey = "GHEALTHC023011V120200731"; // Replace with your actual API key
                    string apiUrl = "http://ghealth.gramweb.net/apiv2/ghealth_patient_details"; // Replace with the PHP site's API URL

                    var content = new FormUrlEncodedContent(new[]
                   {
                    new KeyValuePair<string, string>("apikey", apiKey),
                    new KeyValuePair<string, string>("mobile", phone)
                    // Add more key-value pairs as needed
                });


                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    string data = await response.Content.ReadAsStringAsync();

                    List<GHealthPatientDetailsViewModel> person = JsonConvert.DeserializeObject<List<GHealthPatientDetailsViewModel>>(data);

                    if (person.Count > 0)
                    {

                        // Convert string to Base64
                        string originalString = person[0].barcode_id;
                        byte[] bytes = Encoding.UTF8.GetBytes(originalString);
                        string base64String = Convert.ToBase64String(bytes);


                        string apiKey2 = "RUhFQUxUSDAyMzAxMTIwMTUwMzAx"; // Replace with your actual API key
                        string apiUrl2 = "http://ghealth.gramweb.net/api/phc_api/phc_checkup_details_hrm"; // Replace with the PHP site's API URL

                        var content2 = new FormUrlEncodedContent(new[]
                       {
                    new KeyValuePair<string, string>("api_key", apiKey2),
                    new KeyValuePair<string, string>("barcode", base64String)
                    // Add more key-value pairs as needed
                });


                        HttpResponseMessage response2 = await client.PostAsync(apiUrl2, content2);
                        response2.EnsureSuccessStatusCode();

                        string data2 = await response2.Content.ReadAsStringAsync();

                        JToken token = JToken.Parse(data2);

                        if (token.Type == JTokenType.Object)
                        {
                            // JSON is an object
                            //  Console.WriteLine("JSON is an object");
                            var jsonObject = token.ToObject<JObject>();

                            var bids = JArray.Parse(jsonObject["checkup_info"].ToString());

                            foreach (JObject o in bids.Children<JObject>())
                            {
                                foreach (JProperty p in o.Properties())
                                {
                                    string name = p.Name;
                                    string value = (string)p.Value;
                                    //Console.WriteLine(name + " -- " + value);


                                    if (name == "checkup_date")
                                        obj.checkup_date = value;
                                    if (name == "height")
                                        obj.height = value;
                                    if (name == "weight")
                                        obj.weight = value;
                                    if (name == "bmi")
                                        obj.bmi = value;
                                    if (name == "waist")
                                        obj.waist = value;
                                    if (name == "hip")
                                        obj.hip = value;
                                    if (name == "waist_hip_ratio")
                                        obj.waist_hip_ratio = value;
                                    if (name == "temperature")
                                        obj.temperature = value;
                                    if (name == "oxygen_of_blood")
                                        obj.oxygen_of_blood = value;
                                    if (name == "bp_sys")
                                        obj.bp_sys = value;
                                    if (name == "bp_dia")
                                        obj.bp_dia = value;
                                    if (name == "blood_glucose")
                                        obj.blood_glucose = value;
                                    if (name == "blood_glucose_type")
                                        obj.blood_glucose_type = value;
                                    if (name == "blood_hemoglobin")
                                        obj.blood_hemoglobin = value;
                                    if (name == "urinary_glucose")
                                        obj.urinary_glucose = value;
                                    if (name == "urinary_protein")
                                        obj.urinary_protein = value;
                                    if (name == "urinary_urobilinogen")
                                        obj.urinary_urobilinogen = value;

                                    if (name == "urinary_ph")
                                        obj.urinary_ph = value;
                                    if (name == "pulse_rate")
                                        obj.pulse_rate = value;
                                    if (name == "arrhythmia")
                                        obj.arrhythmia = value;
                                    if (name == "cholesterol")
                                        obj.cholesterol = value;
                                    if (name == "uric_acid")
                                        obj.uric_acid = value;
                                    if (name == "hbsag")
                                        obj.hbsag = value;
                                    if (name == "color_status")
                                        obj.color_status = value;

                                }
                                objList.Add(obj);
                                obj = new GHealthPatientDetailsBasicCheckUpViewModel();

                            }



                            // Perform object-specific operations
                        }
                        else if (token.Type == JTokenType.Array)
                        {
                            // JSON is an array
                            // Console.WriteLine("JSON is an array");
                            var jsonArray = token.ToObject<JArray>();
                            // Perform array-specific operations
                        }
                        else
                        {
                            // JSON is of a different type
                            // Console.WriteLine("JSON is of a different type");
                            // Handle other types accordingly
                        }



                        return PartialView("~/Views/Employee/_HealthApiData.cshtml", objList);

                        // return View(obj);
                    }
                }

                return Json("", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("No Data Found!", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
