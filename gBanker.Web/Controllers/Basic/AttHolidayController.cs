#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

#endregion

namespace gHRM.Web.Controllers
{
    public class AttHolidayController : BaseController
    {
        #region Private Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IAttHolidayDeclarationService attHolidayDeclarationService;
        #endregion

        #region Ctor
        public AttHolidayController(
                IEmployeeSPService employeeSPService,
                IOfficeTypeService officeTypeService,
                IOfficeService officeService,
                IAttHolidayDeclarationService attHolidayDeclarationService
               )
        {

            this.employeeSPService = employeeSPService;
            this.attHolidayDeclarationService = attHolidayDeclarationService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
        }
        #endregion

        #region Listings

        public ActionResult Index()
        {
            var model = new AttHolidayDeclarationViewModel();
            ViewData["Years"] = Years();
            ViewData["AttHolidayType"] = AttHolidayType();
            MapHolidayDayList(model);
            MapOfficeNevigationDropDown(model);
            CheckIfDataExists(model);
            return View(model);
        }
        public ActionResult GetHolidayList([DataSourceRequest] DataSourceRequest request, int HolidayYear)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (HolidayYear > 0)
                {
                    sb.Append(" AND hd.HolidayYear=" + Convert.ToString(HolidayYear));
                }
                else
                {
                    sb.Append(" AND hd.HolidayYear=" + Convert.ToString(DateTime.Now.Year));
                }

                List<AttHolidayDeclarationViewModel> List_ViewModel = new List<AttHolidayDeclarationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "att.SP_AttHolidayDeclaration_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttHolidayDeclarationViewModel
                {
                    AttHolidayDeclarationId = row.Field<long>("AttHolidayDeclarationId"),
                    HolidayYear = row.Field<int>("HolidayYear"),
                    AttHolidayTypeId = row.Field<int>("AttHolidayTypeId"),
                    HolidayDateForView = row.Field<string>("HolidayDate"),
                    HolidayTypeFullName = row.Field<string>("HolidayTypeFullName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DayName = row.Field<string>("DayName")
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Events

        public void CheckIfDataExists(AttHolidayDeclarationViewModel model)
        {
            try
            {
                var holidayList = attHolidayDeclarationService.GetAll().Where(h => h.IsActive == true).ToList();
                if (holidayList.Count > 0)
                {
                    model.IfDataExists = true;
                }
                else
                {
                    model.IfDataExists = false;
                }
            }
            catch (Exception e)
            {
            }
        }

        public void MapHolidayDayList(AttHolidayDeclarationViewModel entity)
        {
            var DaysList = new List<SelectListItem>();
            DaysList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (int i = 0; i < 7; i++)
            {
                var day = new SelectListItem()
                {
                    Text = Enum.GetName(typeof(DayOfWeek), i),
                    Value = i.ToString()
                };
                DaysList.Add(day);
            }
            DaysList.Add(new SelectListItem() { Text = "Friday-Saturday", Value = "7" });
            entity.DayList = DaysList;

            var YearList = new List<SelectListItem>();
            YearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            YearList.Add(new SelectListItem { Text = (DateTime.Now.Year - 1).ToString(), Value = (DateTime.Now.Year - 1).ToString() });
            YearList.Add(new SelectListItem { Text = (DateTime.Now.Year).ToString(), Value = (DateTime.Now.Year).ToString(), Selected = true });
            YearList.Add(new SelectListItem { Text = (DateTime.Now.Year + 1).ToString(), Value = (DateTime.Now.Year + 1).ToString() });
            entity.HolidayYearList = YearList;
        }
        public ActionResult HoliDayEntry()
        {
            var model = new AttHolidayDeclarationViewModel();
            ViewData["Years"] = Years();
            ViewData["AttHolidayType"] = GetHoliDayTypeList();
            MapOfficeNevigationDropDown(model);
            return View(model);
        }

        private void MapOfficeNevigationDropDown(AttHolidayDeclarationViewModel entity)
        {
            var officeType = officeTypeService.GetAll().Where(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            entity.OfficeTypeList = officeType_items;

            var ZoneList = officeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            entity.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            entity.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            entity.UnitList = unit_items;
        }
        public List<DateTime> getWeekdatesandDates(int Year, int option)
        {
            List<DateTime> weekdays = new List<DateTime>();

            for (int Month = 1; Month <= 12; Month++)
            {

                DateTime firstOfMonth = new DateTime(Year, Month, 1);

                DateTime currentDay = firstOfMonth;
                while (firstOfMonth.Month == currentDay.Month)
                {
                    DayOfWeek dayOfWeek = currentDay.DayOfWeek;
                    if (option == 0) // Sunday
                    {
                        if (dayOfWeek == DayOfWeek.Sunday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 1) // Monday
                    {
                        if (dayOfWeek == DayOfWeek.Monday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 2) // Tuesday
                    {
                        if (dayOfWeek == DayOfWeek.Tuesday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 3) // Wednesday
                    {
                        if (dayOfWeek == DayOfWeek.Wednesday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 4) // Thursday
                    {
                        if (dayOfWeek == DayOfWeek.Thursday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 5) // Friday
                    {
                        if (dayOfWeek == DayOfWeek.Friday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 6) // Saterday
                    {
                        if (dayOfWeek == DayOfWeek.Saturday)
                            weekdays.Add(currentDay);
                    }
                    else if (option == 7) // Tuesday
                    {
                        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Friday)
                            weekdays.Add(currentDay);
                    }
                    currentDay = currentDay.AddDays(1);

                } //End of particular Month
            } //End of all Month

            return weekdays;
        }

        public ActionResult LoadAttHolidayList([DataSourceRequest] DataSourceRequest request, string HolidayYear, string AttHolidayTypeId, string HolidayDate, List<string> OfficeIdList, int OfficeTypeId, int radio_button_all)
        {
            try
            {
                AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
                StringBuilder sb = new StringBuilder();
                if (OfficeIdList != null && OfficeIdList.Count == 1)
                {
                    if (OfficeIdList[0] != "")
                        sb.Append(" AND ad.OfficeId ='" + OfficeIdList[0] + "'");
                }
                else if (OfficeIdList != null && OfficeIdList.Count > 1)
                {
                    string OfficeList = "";
                    var count = 1;
                    foreach (var Office in OfficeIdList)
                    {
                        if (count < OfficeIdList.Count)
                        {
                            OfficeList = OfficeList + "'" + Office + "', ";
                        }
                        else
                        {
                            OfficeList = OfficeList + "'" + Office + "'";
                        }
                        count++;
                    }
                    sb.Append(" AND ad.OfficeId In(" + OfficeList + ")");
                }

                if (HolidayYear != "0")
                {
                    sb.Append("AND ad.HolidayYear=" + HolidayYear);
                }
                if (OfficeTypeId != 0)
                {
                    sb.Append(" AND o.OfficeTypeId=" + OfficeTypeId);
                }

                //if (radio_button_all != 0)
                //{
                //    sb.Append(" AND eed.SectionId =" + radio_button_all);
                //}

                List<AttHolidayDeclarationViewModel> List_EmployeeViewModel = new List<AttHolidayDeclarationViewModel>();
                var param = new { AndCondition = sb.ToString() };

                var employeeList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetAttHolidayList");
                List_EmployeeViewModel = employeeList.Tables[0].AsEnumerable()
                .Select(row => new AttHolidayDeclarationViewModel()
                {
                    SlNo = row.Field<string>("rowSl"),
                    AttHolidayDeclarationId = row.Field<long>("AttHolidayDeclarationId"),
                    HolidayYear = row.Field<int>("HolidayYear"),
                    AttHolidayTypeId = row.Field<int>("AttHolidayTypeId"),
                    OfficeId = row.Field<int>("OfficeId"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DayName = row.Field<string>("DayName"),
                    HolidayDateForView = row.Field<string>("HolidayDate"),
                    HolidayTypeFullName = row.Field<string>("HolidayTypeFullName")
                }).ToList();

                DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(string HolidayYear, string AttHolidayTypeId, string HolidayDate, List<string> OfficeIdList, int OfficeTypeId, int radio_button_all)
        {
            string result = "OK";
            try
            {
                var weeklyholidayType = Convert.ToInt32(AttHolidayTypeId);
                AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
                model.HolidayYear = Convert.ToInt32(HolidayYear);
                model.AttHolidayTypeId = Convert.ToInt32(1);
                List<DateTime> WeekDays = getWeekdatesandDates(model.HolidayYear, weeklyholidayType);
                var existingHolidays = attHolidayDeclarationService.GetAll().Where(p => p.HolidayYear == model.HolidayYear).ToList().OrderBy(p => p.HolidayDate);

                if (radio_button_all == 1)
                {
                    var all_office = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == OfficeTypeId).Select(b => b.OfficeId).ToList();
                    foreach (var officeId in all_office)
                    {
                        int office = Convert.ToInt32(officeId);
                        if (office > 0)
                        {
                            var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);
                            var param = new { OfficeId = office, AttHolidayTypeId = model.AttHolidayTypeId, HolidayYear = model.HolidayYear };
                            var deleteExistingData = employeeSPService.GetDataWithParameter(param, "att.SP_DeleteExistingDataForOffice");
                            foreach (var v in WeekDays)
                            {
                                entity.HolidayYear = model.HolidayYear;
                                entity.AttHolidayTypeId = model.AttHolidayTypeId;
                                entity.HolidayDate = v.Date;
                                entity.OfficeId = office;
                                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                                entity.CreateDate = DateTime.Now;
                                entity.IsActive = true;
                                attHolidayDeclarationService.Create(entity);
                            }

                        }
                    }
                }
                else if (radio_button_all == 0)
                {
                    foreach (var officeId in OfficeIdList)
                    {
                        int office = Convert.ToInt32(officeId);
                        if (office > 0)
                        {
                            var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);
                            var param = new { OfficeId = office, AttHolidayTypeId = model.AttHolidayTypeId, HolidayYear = model.HolidayYear };
                            var deleteExistingData = employeeSPService.GetDataWithParameter(param, "att.SP_DeleteExistingDataForOffice");
                            foreach (var v in WeekDays)
                            {
                                entity.HolidayYear = model.HolidayYear;
                                entity.AttHolidayTypeId = model.AttHolidayTypeId;
                                entity.HolidayDate = v.Date;
                                entity.OfficeId = office;
                                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                                entity.CreateDate = DateTime.Now;
                                entity.IsActive = true;
                                attHolidayDeclarationService.Create(entity);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Response.StatusCode = 403;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }// End of Insert

        //public JsonResult CreateH(string HolidayYear, string AttHolidayTypeId, string HolidayDate, List<string> OfficeIdList, int OfficeTypeId, int radio_button_all)
        //{
        //    var result = 0;
        //    var message = "";
        //    try
        //    {

        //                if (radio_button_all == 1)
        //                {
        //                    var all_office = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == OfficeTypeId).Select(b => b.OfficeId).ToList();
        //                    foreach (var officeId in all_office)
        //                    {
        //                        int office = Convert.ToInt32(officeId);
        //                        if (office > 0)
        //                        {
        //                            AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
        //                            model.HolidayYear = Convert.ToInt32(HolidayYear);
        //                            model.AttHolidayTypeId = Convert.ToInt32(AttHolidayTypeId);
        //                            //if(dateDiff > 0)
        //                            model.HolidayDate = Convert.ToDateTime(HolidayDate);
        //                            model.OfficeId = office;
        //                            var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);
        //                            entity.HolidayYear = model.HolidayYear;
        //                            entity.AttHolidayTypeId = model.AttHolidayTypeId;
        //                            entity.HolidayDate = model.HolidayDate;
        //                            entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
        //                            entity.CreateDate = DateTime.Now;
        //                            entity.IsActive = true;

        //                            attHolidayDeclarationService.Create(entity);
        //                            result = 1;
        //                            message = "Saved successfully";

        //                        }
        //                    }
        //                }

        //                else if (radio_button_all == 0)
        //                {
        //                    foreach (var officeId in OfficeIdList)
        //                    {
        //                        int office = Convert.ToInt32(officeId);
        //                        if (office > 0)
        //                        {
        //                            //var param = new { OfficeId = office, AttHolidayTypeId = AttHolidayTypeId, HolidayYear = HolidayYear };
        //                            //var deleteExistingData = employeeSPService.GetDataWithParameter(param, "att.SP_DeleteExistingDataForOffice");
        //                            AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
        //                            model.HolidayYear = Convert.ToInt32(HolidayYear);
        //                            model.AttHolidayTypeId = Convert.ToInt32(AttHolidayTypeId);
        //                            model.HolidayDate = Convert.ToDateTime(HolidayDate);
        //                            model.OfficeId = office;
        //                            var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);
        //                            entity.HolidayYear = model.HolidayYear;
        //                            entity.AttHolidayTypeId = model.AttHolidayTypeId;
        //                            entity.HolidayDate = model.HolidayDate;
        //                            entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
        //                            entity.CreateDate = DateTime.Now;
        //                            entity.IsActive = true;
        //                            attHolidayDeclarationService.Create(entity);
        //                            result = 1;
        //                            message = "Saved successfully";

        //                        }
        //                    }
        //                }
        //          
        //        }

        //    }

        //    catch (Exception ex)
        //    {

        //        Response.StatusCode = 403;
        //    }
        //    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        //}// End of Insert


        public JsonResult CreateH(string HolidayYear, string AttHolidayTypeId, string HolidayDate, string HolidayDateTo, List<string> OfficeIdList, int OfficeTypeId, int radio_button_all)
        {
            var result = 0;
            var message = "";
            try
            {
                DateTime holidayDateFrom = Convert.ToDateTime(HolidayDate).Date;
                DateTime holidayDateTo = Convert.ToDateTime(HolidayDateTo).Date;

                var dateDiff = (holidayDateTo - holidayDateFrom).Days + 1;

                for (int i = 1; i <= dateDiff; i++)
                {
                    List<int> all_office;
                    if (radio_button_all == 1)
                        all_office = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == OfficeTypeId).Select(b => b.OfficeId).ToList();
                    else all_office = OfficeIdList.Select(x => Convert.ToInt32(x)).ToList();
                    foreach (var officeId in all_office)
                    {
                        int office = Convert.ToInt32(officeId);
                        if (office > 0)
                        {
                            AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
                            model.HolidayYear = Convert.ToInt32(HolidayYear);
                            model.AttHolidayTypeId = Convert.ToInt32(AttHolidayTypeId);
                            if (dateDiff > 1)
                                model.HolidayDate = holidayDateFrom.AddDays(i).AddDays(-1);
                            else
                                model.HolidayDate = holidayDateFrom;
                            model.OfficeId = office;
                            var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);
                            entity.HolidayYear = model.HolidayYear;
                            entity.AttHolidayTypeId = model.AttHolidayTypeId;
                            entity.HolidayDate = model.HolidayDate;
                            entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                            entity.CreateDate = DateTime.Now;
                            entity.IsActive = true;

                            attHolidayDeclarationService.Create(entity);
                            result = 1;
                            message = "Saved successfully";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }// End of Insert



        //// Update
        public JsonResult Update(string AttHolidayDeclarationId = "", string HolidayYear = "", string AttHolidayTypeId = "", string HolidayDate = "")
        {
            var result = 0;
            var message = "";
            try
            {
                var isDuplicate =
                    attHolidayDeclarationService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.AttHolidayDeclarationId != Convert.ToInt64(AttHolidayDeclarationId) &&
                                ((Convert.ToString(p.HolidayDate) ==
                                  HolidayDate) ||
                                 (p.AttHolidayTypeId ==
                                  Convert.ToInt64(AttHolidayTypeId)))).ToList();

                if (isDuplicate.Any())
                {
                    result = 0;
                    message = "Holiday already exists";
                }
                else
                {

                    AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
                    model.HolidayYear = Convert.ToInt32(HolidayYear);
                    model.AttHolidayTypeId = Convert.ToInt32(AttHolidayTypeId);
                    model.HolidayDate = Convert.ToDateTime(HolidayDate);
                    model.AttHolidayDeclarationId = Convert.ToInt32(AttHolidayDeclarationId);


                    var entity = Mapper.Map<AttHolidayDeclarationViewModel, AttHolidayDeclaration>(model);

                    var GetData = attHolidayDeclarationService.GetById(Convert.ToInt32(entity.AttHolidayDeclarationId));

                    GetData.HolidayYear = model.HolidayYear;
                    GetData.AttHolidayTypeId = model.AttHolidayTypeId;
                    GetData.HolidayDate = model.HolidayDate;
                    GetData.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    GetData.UpdateDate = DateTime.Now;
                    GetData.IsActive = true;
                    attHolidayDeclarationService.Update(GetData);
                    result = 1;
                    message = "Updated successfully";

                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }// End of Insert

        public ActionResult GetList([DataSourceRequest] DataSourceRequest request, string Id, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string Ids = Convert.ToString(Id);

                if (Id != null)
                {
                    sb.Append(" AND hd.AttHolidayDeclarationId =" + Ids);
                }
                if (Convert.ToInt32(filterValue) > 0)
                {
                    sb.Append(" AND hd.OfficeId =" + Convert.ToInt32(filterValue));
                }
                List<AttHolidayDeclarationViewModel> List_ViewModel = new List<AttHolidayDeclarationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "att.SP_AttHolidayDeclaration_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttHolidayDeclarationViewModel
                {
                    AttHolidayDeclarationId = row.Field<long>("AttHolidayDeclarationId"),
                    HolidayYear = row.Field<int>("HolidayYear"),
                    AttHolidayTypeId = row.Field<int>("AttHolidayTypeId"),
                    HolidayDateForView = row.Field<string>("HolidayDate"),
                    HolidayTypeFullName = row.Field<string>("HolidayTypeFullName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DayName = row.Field<string>("DayName")
                }).ToList();

                if (Id != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult GetListPublicHoliday([DataSourceRequest] DataSourceRequest request, string HolidayYear, string AttHolidayTypeId, string HolidayDate, List<string> OfficeIdList, int OfficeTypeId, int radio_button_all)
        {
            try
            {
                AttHolidayDeclarationViewModel model = new AttHolidayDeclarationViewModel();
                StringBuilder sb = new StringBuilder();
                if (OfficeIdList != null && OfficeIdList.Count == 1)
                {
                    if (OfficeIdList[0] != "")
                        sb.Append(" AND hd.OfficeId ='" + OfficeIdList[0] + "'");
                }
                else if (OfficeIdList != null && OfficeIdList.Count > 1)
                {
                    string OfficeList = "";
                    var count = 1;
                    foreach (var Office in OfficeIdList)
                    {
                        if (count < OfficeIdList.Count)
                        {
                            OfficeList = OfficeList + "'" + Office + "', ";
                        }
                        else
                        {
                            OfficeList = OfficeList + "'" + Office + "'";
                        }
                        count++;
                    }
                    sb.Append(" AND hd.OfficeId In(" + OfficeList + ")");
                }

                if (HolidayYear != "0")
                {
                    sb.Append("AND hd.HolidayYear=" + HolidayYear);
                }
                if (OfficeTypeId != 0)
                {
                    sb.Append(" AND o.OfficeTypeId=" + OfficeTypeId);
                }
                if (AttHolidayTypeId != "" && AttHolidayTypeId != "0")
                {
                    sb.Append("AND hd.AttHolidayTypeId=" + AttHolidayTypeId);
                }

                List<AttHolidayDeclarationViewModel> List_ViewModel = new List<AttHolidayDeclarationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "att.SP_AttHolidayDeclaration_PublicHoliday_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttHolidayDeclarationViewModel
                {
                    AttHolidayDeclarationId = row.Field<long>("AttHolidayDeclarationId"),
                    HolidayYear = row.Field<int>("HolidayYear"),
                    AttHolidayTypeId = row.Field<int>("AttHolidayTypeId"),
                    HolidayDateForView = row.Field<string>("HolidayDate"),
                    HolidayTypeFullName = row.Field<string>("HolidayTypeFullName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DayName = row.Field<string>("DayName"),
                    OfficeTypeId = row.Field<int>("OfficeTypeId")
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //// Update
        public JsonResult Delete(string AttHolidayDeclarationId)
        {

            string result = "";
            try
            {
                var GetData = attHolidayDeclarationService.GetById(Convert.ToInt32(AttHolidayDeclarationId));
                if (GetData == null)
                {
                    Response.StatusCode = 403;
                }
                GetData.IsActive = false;

                attHolidayDeclarationService.Update(GetData);
                result = "OK";
            }
            catch (Exception ex)
            {
                result = "";
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }// End of Insert

        public List<SelectListItem> GetHoliDayTypeList()
        {
            List<AttHolidayDeclarationViewModel> List_ViewModel = new List<AttHolidayDeclarationViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("att.SP_Att_Get_HolidayType_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new AttHolidayDeclarationViewModel
            {
                AttHolidayTypeId = row.Field<int>("AttHolidayTypeId"),
                HolidayTypeFullName = row.Field<string>("HolidayTypeFullName"),
                HolidayTypeShortName = row.Field<string>("HolidayTypeShortName")

            }).Where(c => c.HolidayTypeShortName != "WH").ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AttHolidayTypeId.ToString(),
                Text = string.Format("{0} - {1}", x.HolidayTypeFullName, x.AttHolidayTypeId)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);

            return Component_items;
        }

        #endregion Events      

        #region Ajax Requests
        public JsonResult GetProjectsByOfficeType()
        {
            var list = new List<SelectListItem>();
            var offList = officeService.GetMany(b => b.IsActive == true && b.OfficeTypeId == (3)).Select(b => new SelectListItem
            {
                Text = b.OfficeName,
                Value = b.OfficeId.ToString()
            });
            //list.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            list.AddRange(offList);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Methods
        private List<SelectListItem> Years()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Text = "Please Select", Value = "0" });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year - 5), Value = Convert.ToString(DateTime.Now.Year - 5) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year - 4), Value = Convert.ToString(DateTime.Now.Year - 4) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year - 3), Value = Convert.ToString(DateTime.Now.Year - 3) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year - 2), Value = Convert.ToString(DateTime.Now.Year - 2) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year - 1), Value = Convert.ToString(DateTime.Now.Year - 1) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year), Value = Convert.ToString(DateTime.Now.Year) });
            items2.Add(new SelectListItem { Text = Convert.ToString(DateTime.Now.Year + 1), Value = Convert.ToString(DateTime.Now.Year + 1) });

            //for (int year = DateTime.Now.Year; year >= 2000; year--)
            //{
            //    items2.Add(new SelectListItem
            //    {
            //        Text = Convert.ToString(year),
            //        Value = Convert.ToString(year)
            //    });
            //}

            return items2;
        }// End of Years

        private List<SelectListItem> AttHolidayType()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            items2.Add(new SelectListItem
            {
                Text = "Friday",
                Value = "1"
            });
            items2.Add(new SelectListItem
            {
                Text = "Friday-Saturday",
                Value = "2"
            });
            //for (int type = 1; type <= 2; type++)
            //{
            //    items2.Add(new SelectListItem
            //    {
            //        Text = Convert.ToString(type),
            //        Value = Convert.ToString(type)
            //    });
            //}

            return items2;
        }// End of Years

        #endregion
    }
}