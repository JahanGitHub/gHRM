using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Models;
using gHRM.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.Core.Extensions;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;

namespace gHRM.Web.Controllers
{
    public class EmployeeOfficeDesignationController : BaseController
    {

        #region Variables
        private readonly IEmployeeOfficeDesignationService employeeOfficeDesignationService;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeDesignationService OfficeDesignationService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeSPService employeeSPService;
        public EmployeeOfficeDesignationController(IEmployeeOfficeDesignationService employeeOfficeDesignationService, IEmployeeService employeeService, IOfficeDesignationService OfficeDesignationService, IOfficeService officeService, IEmployeeSPService employeeSPService)// OfficeService officeService
        {
            this.employeeOfficeDesignationService = employeeOfficeDesignationService;
            this.employeeService = employeeService;
            this.OfficeDesignationService = OfficeDesignationService;
            this.officeService = officeService;
            this.employeeSPService = employeeSPService;
        }

        #endregion


        #region Methods
        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                List<EmployeeOfficeDesignationViewModel> List_EmployeeViewModel = new List<EmployeeOfficeDesignationViewModel>();
                var Emp = employeeService.GetByCode(employee_code);
                var param = new { EmployeeId = Emp.EmployeeId };
                var empList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeDetails_ByEmployeeId");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeOfficeDesignationViewModel
                {
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    OfficeDesignationId = row.Field<int>("OfficeDesignationId")
                    //OfficeDesignationId = Convert.ToInt32(string.IsNullOrEmpty(row.Field<string>("OfficeDesignationId")) ? "0" : row.Field<string>("OfficeDesignationId"))                   
                }).ToList();

                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmployeeOfficeDesignationList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                long TotCount;
                var EmployeeOfficeDesignationDetail = employeeOfficeDesignationService.GetDBEmployeeOfficeDesignationDetails(jtStartIndex, jtSorting, jtPageSize, out TotCount);

                var detail = EmployeeOfficeDesignationDetail.ToList();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(EmployeeOfficeDesignationViewModel model)
        {
            var offiDetList = OfficeDesignationService.GetMany(w => w.IsActive == true);
            var viewoffiDe = offiDetList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeDesignationId.ToString(),
                Text =  x.OffcDesignName
            });
            var offiDe_items = new List<SelectListItem>();
            offiDe_items.Add(new SelectListItem() { Text = "N/A", Value = "0", Selected = true });
            offiDe_items.AddRange(viewoffiDe);
            model.OfficeDesignationNameList = offiDe_items;
        }

        public JsonResult EmpOfficeDesignationSave(string EmployeeId, string OffDesiId, string startDate)
        {
            long EmpOfficeDesigId = 0;
            if (OffDesiId != "0")
            {
                var employee = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));

                employee.OfficeDesignationId = Convert.ToInt32(OffDesiId);
                employeeService.Update(employee);

                var epmDes = employeeOfficeDesignationService.GetMany(e => e.EmployeeId == Convert.ToInt64(EmployeeId));

                if (epmDes.ToList().Count() >= 1)
                {
                    var lastEmpOfficeDesigId = epmDes.Max(e => e.EmpOfficeDesigId);

                    var EmpOfficeDes = employeeOfficeDesignationService.GetById(Convert.ToInt32(lastEmpOfficeDesigId));
                    DateTime LaststartDate = Convert.ToDateTime(employeeOfficeDesignationService.GetById(Convert.ToInt32(lastEmpOfficeDesigId)).SartDate);
                    DateTime staDate = Convert.ToDateTime(startDate);
                    double Duration = (staDate-LaststartDate).TotalDays;
                    DateTime StDate = staDate.AddDays(-1);
                    EmpOfficeDes.EndDate = Convert.ToDateTime(StDate);
                    EmpOfficeDes.Duration = Convert.ToInt32(Duration);
                    employeeOfficeDesignationService.Update(EmpOfficeDes);
                }

                var OffDesignation = new EmployeeOfficeDesignation() { EmployeeId = Convert.ToInt64(EmployeeId), OfficeDesignationId = Convert.ToInt32(OffDesiId), SartDate = Convert.ToDateTime(startDate),IsActive=true,CreateUser=SessionHelper.LoggedInEmployeeID,CreateDate = DateTime.Now };
                employeeOfficeDesignationService.Create(OffDesignation);
                EmpOfficeDesigId = OffDesignation.EmpOfficeDesigId;
            }
            else
            {
                var employee = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));

                employee.OfficeDesignationId = null;
                employeeService.Update(employee);
                EmpOfficeDesigId = 1;
            }                                 

            return Json(EmpOfficeDesigId, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetOfficeDesignation()
        {
            var OfficeDesignation = OfficeDesignationService.GetAll();

            var viewOfficeDesignationList = OfficeDesignation.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeDesignationId.ToString(),
                Text = x.OffcDesignName.ToString()
            });
            var OfficeDesignation_items = new List<SelectListItem>();
            OfficeDesignation_items.Add(new SelectListItem() { Text = "N/A", Value = "0", Selected = true });
            OfficeDesignation_items.AddRange(viewOfficeDesignationList);
            return Json(OfficeDesignation_items, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Events

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
          var  model =new EmployeeOfficeDesignationViewModel();
            MapDropDownList(model);
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["OfficeDesignationList"] = items;
            
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Create(EmployeeOfficeDesignationViewModel model)
        //{
        //    try
        //    {
        //        MapDropDownList(model);
        //        IEnumerable<SelectListItem> items = new SelectList(" ");
        //       // ViewData["EmployeeList"] = items;
        //        ViewData["OfficeDesignationList"] = items;
        //        ViewData["HOOfficeList"] = items;
        //        ViewData["ZOOfficeList"] = items;
        //        ViewData["AOOfficeList"] = items;
        //        ViewData["BOOfficeList"] = items;
        //        ViewData["ZAOOfficeList"] = items;
        //        return View(model);
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult Edit(int id)
        {
            if (employeeService.IsContinued(id))
            {
                var employeeOfficeDesignation = employeeOfficeDesignationService.GetById(id);
                var entity = Mapper.Map<EmployeeOfficeDesignation, EmployeeOfficeDesignationViewModel>(employeeOfficeDesignation);

                var EmpId = employeeOfficeDesignationService.GetById(id).EmployeeId;
                var EmpName = employeeService.GetByEmpId(Convert.ToInt64(EmpId)).EmployeeName;
                var employeeOfficeId = employeeService.GetByEmpId(Convert.ToInt64(EmpId)).OfficeId;
                var empOffNmae = officeService.GetById(Convert.ToInt32(employeeOfficeId)).OfficeName;
                var EpmOffDesId = employeeOfficeDesignationService.GetById(id).OfficeDesignationId;
                var EpmOffDesName = OfficeDesignationService.GetById(Convert.ToInt32(EpmOffDesId)).OffcDesignName;

                entity.OfficeName = empOffNmae;
                entity.EmployeeName = EmpName;
                entity.OfficeDesignationName = EpmOffDesName;
                entity.SartDate = employeeOfficeDesignation.SartDate;
                MapDropDownList(entity);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued Employee Time Scale, please enter a diferent employee Time Scale id.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EmployeeOfficeDesignationViewModel model)
        {
            try
            {
                var entity = Mapper.Map<EmployeeOfficeDesignationViewModel, EmployeeOfficeDesignation>(model);
                var empOffDetails = employeeOfficeDesignationService.GetById(Convert.ToInt32(entity.EmpOfficeDesigId));

                if (entity.OfficeDesignationId != 0)
                {
                    if (ModelState.IsValid)
                    {
                        //getDistrictDetails.CountrtyId = entity.CountrtyId;
                        empOffDetails.OfficeDesignationId = entity.OfficeDesignationId;
                        empOffDetails.SartDate = entity.SartDate;
                        empOffDetails.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        empOffDetails.UpdateDate = DateTime.Now;

                        employeeOfficeDesignationService.Update(empOffDetails);

                        var EmpId = employeeOfficeDesignationService.GetById(Convert.ToInt32(model.EmpOfficeDesigId)).EmployeeId;

                        var Emp = employeeService.GetByEmpId(Convert.ToInt64(EmpId));
                        Emp.OfficeDesignationId = model.OfficeDesignationId;
                        employeeService.Update(Emp);

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    {
                        //getDistrictDetails.CountrtyId = entity.CountrtyId;
                        empOffDetails.OfficeDesignationId = entity.OfficeDesignationId;
                        empOffDetails.SartDate = entity.SartDate;
                        empOffDetails.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        empOffDetails.UpdateDate = DateTime.Now;

                        employeeOfficeDesignationService.Update(empOffDetails);

                        var EmpId = employeeOfficeDesignationService.GetById(Convert.ToInt32(model.EmpOfficeDesigId)).EmployeeId;

                        var Emp = employeeService.GetByEmpId(Convert.ToInt64(EmpId));
                        Emp.OfficeDesignationId = null;
                        employeeService.Update(Emp);

                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        
        }
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
