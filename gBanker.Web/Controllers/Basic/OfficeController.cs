#region Usings

using AutoMapper;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.Offices;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class OfficeController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService offTypeiceService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IKeyCloakService keyCloakService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown commonDynamicDropDown;

        public OfficeController(
            IOfficeService officeService,
            IOfficeTypeService offTypeiceService,
            IEmployeeService employeeService,
            IKeyCloakService keyCloakService,
            IEmployeeSPService employeeSPService)
        {
            this.officeService = officeService;
            this.offTypeiceService = offTypeiceService;
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.keyCloakService = keyCloakService;

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }
        #endregion

        #region Events

        public ActionResult OfficeReport(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });
              
                PrintSSRSReport("/gHRMPlus_Reports/OfficeReport", paramValues.ToArray());

                
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult Index()
        {
            var model = new OfficeViewModel();
            model.OfficeTypeList = OfficeTypeList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(int? OfficeId)
        
        {
            var model = new OfficeViewModel();
            bool OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED = true;

            if (OfficeId.HasValue && OfficeId > 0)
            {
                int _officeId = Convert.ToInt32(OfficeId);
                var _model = officeService.GetById(_officeId);
                model = Mapper.Map<Office, OfficeViewModel>(_model);
                OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED = AppSetting.GetBool(AppSetting.OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED, HttpContext);

                if (model.OfficeTypeId == 6)
                {
                    var office = officeService.GetById(Convert.ToInt32(model.OfficeId));

                    var officeArea = officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault();
                    if (officeArea != null)
                        model.AreaCode = officeArea.OfficeCode;

                    var officeZoneCode = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                    if (officeZoneCode != null)
                        model.ZoneCode = officeZoneCode.OfficeCode;

                }
                else if (model.OfficeTypeId == 5)
                {
                    var office = officeService.GetById(Convert.ToInt32(model.OfficeId));

                    var officeInfo = officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault();
                    if (officeInfo != null)
                        model.ZoneCode = officeInfo.OfficeCode;
                }
                model.OperationStartDateMsg = model.OperationStartDate.ToString("dd-MMM-yyyy");
            }
            MapDropDownList(model);
            model.OfficeTypeList = OfficeTypeListWithOutHeadOffice();
            model.ZoneList = getAllZone();
            List<SelectListItem> _AreaList = new List<SelectListItem>();
            _AreaList.Add(new SelectListItem() { Value = "", Text = "Please Select" });
            model.AreaList = _AreaList;
            ViewBag.OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED = OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED;
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(OfficeViewModel model, FormCollection collection)
        {
            try
            {
                var entity = Mapper.Map<OfficeViewModel, Office>(model);

                if (ModelState.IsValid)
                {
                    var officeList = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == model.OfficeTypeId);
                    var LatestOfficeCode = officeList.Count() == 0 ? 2000 : Convert.ToInt32(officeList.Max(b => b.OfficeCode)) + 1;
                    entity.OfficeCode = LatestOfficeCode.ToString();
                    entity.OfficeLevel = 4;
                    entity.FirstLevel = "1000";
                    entity.SecondLevel = "1000";
                    entity.ThirdLevel = Convert.ToInt32(model.OfficeTypeId) == 1 ? "2000" : Convert.ToInt32(model.OfficeTypeId) == 2 ? "3000" : Convert.ToInt32(model.OfficeTypeId) == 3 ? "4000" : "Other";
                    entity.FourthLevel = LatestOfficeCode.ToString();
                    entity.IsActive = true;
                    entity.CreateDate = DateTime.Now;
                    entity.CompanyId = CompanyID.HasValue ? CompanyID : 1;
                    entity.CreateUser = LoggedInEmployeeId;
                    officeService.Create(entity);

                    return GetSuccessMessageResult();
                }
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public ActionResult Edit(int id)
        {
            var offc = officeService.GetById(id);
            var offcModel = Mapper.Map<Office, OfficeViewModel>(offc);
            MapDropDownList(offcModel);
            return View(offcModel);
        }


        [HttpPost]
        public JsonResult Edit(OfficeViewModel model)
        {
            try
            {
                var entity = officeService.GetById(model.OfficeId);
                if (ModelState.IsValid)
                {
                    entity.OfficeName = model.OfficeName;
                    entity.OfficeAddress = model.OfficeAddress;
                    entity.OperationStartDate = model.OperationStartDate;
                    entity.PostCode = model.PostCode;
                    //entity.GeoLocationID = model.GeoLocationID;
                    entity.Email = model.Email;
                    entity.Phone = model.Phone;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    officeService.Update(entity);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
                //return Json(new { Result = "OK" });

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View("Index");
        }


        [HttpPost]
        public ActionResult Delete(OfficeViewModel model)
        {
            try
            {
                var entity = Mapper.Map<OfficeViewModel, Office>(model);
                entity.IsActive = false;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.InActiveDate = DateTime.Now;
                officeService.Update(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult getOfficeDashboard([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var pram = new { AndCondition = sb.ToString() };
               
                var officeList = employeeSPService.GetDataWithParameter(pram, "basic.SP_GetOfficeInfo");

                var officeViewList = officeList.Tables[0].AsEnumerable()
                .Select(row => new OfficeViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    OfficeNameBn = row.Field<string>("OfficeNameBn"),
                    OfficeAddress = row.Field<string>("OfficeAddress"),
                    Phone = row.Field<string>("Phone"),
                    OperationStartDateMsg = row.Field<string>("OperationStartDate")

                }).ToList();

                DataSourceResult result = officeViewList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region HttpRequests

        public JsonResult GetOfficeInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                List<OfficeViewModel> List_ViewModel = new List<OfficeViewModel>();
                StringBuilder sb = new StringBuilder();
                if (filterColumn != null)
                {
                    if (filterColumn == "OfficeType")
                    {
                        sb.Append(" AND o.OfficeTypeId=" + filterValue);
                    }
                    else if (filterColumn == "OfficeName")
                    {
                        sb.Append(" AND o.OfficeName LIKE('" + filterValue + "%')");
                    }
                    else if (filterColumn == "OfficeCode")
                    {
                        sb.Append(" AND o.OfficeCode='" + filterValue + "'");
                    }

                }

                var pram = new { AndCondition = sb.ToString() };
                var officeList = employeeSPService.GetDataWithParameter(pram, "basic.SP_GetOfficeInfo");
                List_ViewModel = officeList.Tables[0].AsEnumerable()
                .Select(row => new OfficeViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId"),
                    OfficeTypeName = row.Field<string>("OfficeTypeName"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    OfficeNameBn = row.Field<string>("OfficeNameBn"),
                    OfficeAddress = row.Field<string>("OfficeAddress"),
                    Phone = row.Field<string>("Phone"),
                    OperationStartDateMsg = row.Field<string>("OperationStartDate")

                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult GetParentList(string OfficeCode)
        {
            var offc = officeService.GetMany(m => m.OfficeLevel != 4).ToList();
            var officeList = new List<Office>();
            officeList = offc;
            var offce = officeList.Where(m => string.Format("{0} - {1}", m.OfficeCode, m.OfficeName).ToLower().Contains(OfficeCode.ToLower())).Select(m1 => new { m1.OfficeId, OfficeFullName = string.Format("{0} - {1}", m1.OfficeCode, m1.OfficeName), m1.FirstLevel, m1.SecondLevel, m1.ThirdLevel }).ToList();
            return Json(offce, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getFirstLevelOffice()
        {
            int result = 0;
            string message = ""; object data = "";
            try
            {
                var offc = officeService.GetAll().Where(m => m.OfficeTypeId == 1 && m.IsActive == true).ToList();
                var officeList = new List<Office>();
                officeList = offc;
                var offce = offc.Select(b => new
                {
                    value = b.OfficeCode,
                    text = b.OfficeName
                });
                result = 1;
                data = offce;

            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;
            }

            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFirstLevelWiseSecondLevelOffice(string FirstLevel)
        {
            int result = 0;
            string message = ""; object data = "";
            try
            {

                var offc = officeService.GetMany(m => m.IsActive == true && m.OfficeTypeId == 2 && m.FirstLevel == FirstLevel).ToList();
                var officeList = new List<Office>();
                officeList = offc;
                var offce = offc.Select(b => new
                {
                    value = b.OfficeCode,
                    text = b.OfficeName
                });
                result = 1;
                data = offce;
            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;
            }

            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSecondLevelWiseThirdLevelOffice(string SecondLevel)
        {
            int result = 0;
            string message = ""; object data = "";
            try
            {
                var offc = officeService.GetMany(m => m.IsActive == true && m.OfficeTypeId == 3 && m.SecondLevel == SecondLevel).ToList();
                var officeList = new List<Office>();
                officeList = offc;
                var offce = offc.Select(b => new
                {
                    value = b.OfficeCode,
                    text = b.OfficeName
                });
                data = offce;
            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;
            }

            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParentCodeDetail(string ParentId)
        {
            if (ParentId != "")
            {
                var offc = officeService.GetByOfficeCode(ParentId);
                var result = new { FirstLevel = offc.FirstLevel, SecondLevel = offc.SecondLevel, ThirdLevel = offc.ThirdLevel };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult OfficeDelete(string officeId)
        {
            int result = 0;
            string message = "";
            int officeCountUnderThisOffice = 0;

            int office_id = Convert.ToInt32(officeId);
            var office = officeService.GetById(office_id);

            if (office.OfficeTypeId == 4) //Zonal Office
            {
                officeCountUnderThisOffice = officeService.GetAll().Count(o => o.SecondLevel == office.OfficeCode && o.IsActive == true);
            }

            else if (office.OfficeTypeId == 5) //Area Office
            {
                officeCountUnderThisOffice = officeService.GetAll().Count(o => o.ThirdLevel == office.OfficeCode && o.IsActive == true);
            }

            if (officeCountUnderThisOffice > 0)
            {
                message = "Sorry there are offices under this office";
            }

            var employeeCountInOffice = employeeService.GetMany(e => e.OfficeId == office_id).ToList();

            if (employeeCountInOffice.Any())
            {
                message = "Sorry there are employees in this office";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var entity = officeService.GetById(Convert.ToInt32(officeId));
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.InActiveDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    officeService.Update(entity);
                }
                result = 1;
                message = "Office Deleted Succesfully";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult OfficeTypeList_JS()
        {
            var list = new List<SelectListItem>();
            var offList = offTypeiceService.GetMany(b => b.IsActive == true).Select(b => new SelectListItem
            {
                Text = b.OfficeTypeName,
                Value = b.OfficeTypeId.ToString()
            });
            list.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHeadOfficeByOfficeType()
        {
            var offList = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == 1).Select(b => new SelectListItem
            {
                Text = b.OfficeName,
                Value = b.OfficeId.ToString()
            });
            return Json(offList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectsByOfficeType()
        {
            var list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == (3)).Select(b => new SelectListItem
            {
                Text = b.OfficeName,
                Value = b.OfficeId.ToString()
            });
            list.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllZone_js()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 4 && b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeCode
                });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllZoneId()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 4 && b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeId.ToString()
                });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllAreaByZoneCode(string ZoneCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 5 && b.IsActive == true && b.SecondLevel == ZoneCode)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeCode
                });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllAreaIdByZoneCode(int ZoneId)
        {
            var ZoneCode = officeService.GetById(ZoneId).OfficeCode;
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 5 && b.IsActive == true && b.SecondLevel == ZoneCode).OrderBy(x => x.OfficeName)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeId.ToString()
                });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getAllAreaIdByZoneCodeEmpList(int ZoneId)
        {
            var ZoneCode = officeService.GetById(ZoneId).OfficeCode;
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 5 && b.IsActive == true && b.SecondLevel == ZoneCode).OrderBy(x => x.OfficeName)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeId.ToString()
                });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllUnitByAreaCode(string AreaCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 6 && b.IsActive == true && b.ThirdLevel == AreaCode)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeCode
                });

            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllUnitIdByAreaCode(int AreaId)
        {
            var AreaCode = officeService.GetById(AreaId).OfficeCode;
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 6 && b.IsActive == true && b.ThirdLevel == AreaCode)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeName,
                    Value = b.OfficeId.ToString()
                });

            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult isDuplicateOfficeCode_JS(string OfficeCode, int? OfficeId)
        {
            int result = 0;
            string message = "";
            try
            {
                var List = officeService.GetMany(b => b.OfficeCode == OfficeCode && b.IsActive == true);
                if (OfficeId.HasValue && OfficeId > 0)
                {
                    var _officeId = Convert.ToInt32(OfficeId);
                    List = List.Where(b => b.OfficeId != _officeId);
                }
                if (List.Count() > 0)
                {
                    result = 1;
                    message = OfficeCode + " already exists.";
                }
                else
                {
                    result = 0;
                    message = "";
                }
            }
            catch (Exception e)
            {
                result = 0;
                message = "";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SaveOffice(int officeId, int officeLocationId, int officeTypeId, string zoneId, string areaId, string officeCode,
            string officeName, string officeNameBn, string operationStartDateMsg, string officeAddress, string phone, string email,string officeTypeName)
        {
            int result = 0;
            string message = "";

            try
            {
                var model = new Office();


                var isDuplicateOfficeCode = this.isDuplicateOfficeCode(officeCode, officeId);
                if (isDuplicateOfficeCode == true)
                {
                    message = officeCode + " already exists";
                }
                else
                {
                    var headOfficeCode =
                           officeService.GetMany(o => o.OfficeTypeId == 1).FirstOrDefault().OfficeCode;

                    if (officeId > 0)
                    {
                        model = officeService.GetById(officeId);
                    }
                    model.FirstLevel = headOfficeCode;

                    model.OfficeTypeId = officeTypeId;
                    if (officeTypeId <= 4)//for zone
                    {
                        model.SecondLevel = officeCode;
                        model.ThirdLevel = officeCode;
                        model.FourthLevel = officeCode;
                    }
                    else if (officeTypeId == 5)//for area
                    {
                        model.SecondLevel = zoneId;
                        model.ThirdLevel = officeCode;
                        model.FourthLevel = officeCode;
                    }
                    else if (officeTypeId == 6)//for Unit
                    {
                        model.SecondLevel = zoneId;
                        model.ThirdLevel = areaId;
                        model.FourthLevel = officeCode;
                    }
                    else
                    {
                        model.SecondLevel = officeCode;
                        model.ThirdLevel = officeCode;
                        model.FourthLevel = officeCode;
                    }

                    model.OfficeLocationId = officeLocationId;
                    model.OfficeCode = officeCode;
                    model.OfficeName = officeName;
                    model.OfficeNameBn = officeNameBn;
                    model.OperationStartDate = Convert.ToDateTime(operationStartDateMsg);
                    model.OfficeAddress = officeAddress;
                    model.Phone = phone;
                    model.Email = email;
                    model.IsActive = true;

                    if (officeId > 0)
                    {
                        model.UpdateUser = LoggedInEmployeeId;
                        model.UpdateDate = DateTime.Now;
                        officeService.Update(model);
                        message = "Updated Successfully";
                    }
                    else
                    {
                        model.CreateUser = LoggedInEmployeeId;
                        model.CreateDate = DateTime.Now;
                        model.OfficeLevel = 4;
                        model.CompanyId = SessionHelper.CompanyID;
                        officeService.Create(model);
                        message = "Saved Successfully";
                    }

                    if (SessionHelper.EnabledSSOLogin)
                    {
                        //sync office at health center db
                        var token = HttpContext.Request.Cookies.Get(CookieConstants.CURRENT_LOGGED_IN_ACCESSTOKEN);
                        var acccessToken = token != null ? token.Value.ToString() : "";

                        var healthCenterOfficeId = await keyCloakService.GetOfficeById(acccessToken, model.OfficeId);

                        if (!string.IsNullOrWhiteSpace(acccessToken))
                        {
                            var newOffice = new OfficeAddOrEditApiModel
                            {
                                id= healthCenterOfficeId.id,
                                apiOfficeId = model.OfficeId,
                                name = GetSSOOfficeName(model),
                                centerCode = model.OfficeCode,
                                address = model.OfficeAddress,
                                firstLevel = model.FirstLevel,
                                secondLevel = model.SecondLevel,
                                thirdLevel = model.ThirdLevel,
                                fourthLevel = model.FourthLevel,
                                officeTypeId =(int)model.OfficeTypeId,
                                officeLevel = model.OfficeLevel,

                                active = true,
                            };

                            //sync office
                            await keyCloakService.SyncOffice(newOffice, acccessToken);
                        }
                    }

                    result = 1;
                }
            }
            //catch (DbEntityValidationException ex)
            //{
            //    // Iterate through each validation error
            //    foreach (var entityValidationErrors in ex.EntityValidationErrors)
            //    {
            //        foreach (var validationError in entityValidationErrors.ValidationErrors)
            //        {
            //            // Output the validation error message
            //            Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
            //        }
            //    }
            //}
            catch (Exception ex)
            {
                message = "Error Occured";
                result = 0;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        private void MapDropDownList(OfficeViewModel model)
        {
            model.OfficeLocationList = commonDynamicDropDown.OfficeLocationList(false);

        }

        public IEnumerable<SelectListItem> OfficeTypeList()
        {
            var offList = offTypeiceService.GetMany(b => b.IsActive == true).Select(b => new SelectListItem
            {
                Text = b.OfficeTypeName,
                Value = b.OfficeTypeId.ToString()
            });
            return offList;
        }

        public IEnumerable<SelectListItem> getAllZone()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.OfficeTypeId == 4 && b.IsActive == true)
                 .Select(b => new SelectListItem
                 {
                     Text = b.OfficeName,
                     Value = b.OfficeCode
                 });
            var pleaseSelect = new SelectListItem() { Text = "Please Select", Value = "" };
            list.Add(pleaseSelect);
            list.AddRange(offList);
            return list;
        }

        public bool isDuplicateOfficeCode(string OfficeCode, int? OfficeId)
        {
            var List = officeService.GetMany(b => b.OfficeCode == OfficeCode && b.IsActive == true);
            if (OfficeId.HasValue && OfficeId > 0)
            {
                var _officeId = Convert.ToInt32(OfficeId);
                List = List.Where(b => b.OfficeId != _officeId);
            }
            if (List.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<SelectListItem> OfficeTypeListWithOutHeadOffice()
        {
            var offList = offTypeiceService.GetMany(b => b.OfficeTypeId != 1 && b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Text = b.OfficeTypeName,
                    Value = b.OfficeTypeId.ToString()
                });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(offList);
            return officeType_items;
        }



        #endregion

        #region for Acounts
        public JsonResult getOfficeTypeWiseOfficeList(int OfficeTypeId)
        {
            int result = 0; string message = "";
            object Data = "";
            try
            {
                Data = GetOfficeTypeWiseOfficeList(OfficeTypeId);

            }
            catch (Exception e)
            {
                Data = "";
                result = 0;
                message = e.Message;

            }

            return Json(new { result = result, message = message, Data = Data }, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<SelectListItem> GetOfficeTypeWiseOfficeList(int OfficeTypeId)
        {

            List<SelectListItem> dropDownFirstElement = new List<SelectListItem>();
            dropDownFirstElement.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            var list = officeService.getOfficeTypeWiseOfficeList(OfficeTypeId)
                .Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                });
            dropDownFirstElement.AddRange(list);
            return dropDownFirstElement;
        }

        #endregion

        #region Private Methods

        private string GetSSOOfficeName(Office office)
        {
            var officeNameWithShortCode = "";
            switch (office.OfficeLevel)
            {
                case 1:
                    officeNameWithShortCode = $"{office.OfficeName} (HO)";
                    break;

                case 2:
                    officeNameWithShortCode = $"{office.OfficeName} (ZO)";
                    break;

                case 3:
                    officeNameWithShortCode = $"{office.OfficeName} (AR)";
                    break;

                default:
                    officeNameWithShortCode = $"{office.OfficeName} (HC)";
                    break;
            }

            return officeNameWithShortCode;
        }

        #endregion
    }
}


