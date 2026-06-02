using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class EmployeeStatusController : BaseController
    {
        #region Variables
        //private readonly IVMCarTypeService vmCarTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        public EmployeeStatusController(
            //IVMCarTypeService vmCarTypeService,
            IEmployeeStatusService employeeStatusService
        )
        {
            //this.vmCarTypeService = vmCarTypeService;
            this.employeeStatusService = employeeStatusService;
        }

        #endregion

        #region MapDropDown
        //private void MapDropdownForEmployeeStatusbyId(EmployeeStatusViewModel model)
        //{
        //    var empStatus = new List<SelectListItem>();
        //    empStatus.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
        //    var statusList = employeeStatusService.GetMany(x => x.IsActive == true && x.IsValid == true).OrderBy(p => p.ViewOrder);
        //    var getEmpStatus = statusList.AsEnumerable().Select(row => new SelectListItem
        //    {
        //        Text = row.StatusName,
        //        Value = row.StatusValue

        //    }).ToList();
        //    empStatus.AddRange(getEmpStatus);
        //    model.EmployeeStatusList = empStatus;
        //}
        #endregion

        #region Methods

        public JsonResult SaveEmployeeStatus(EmployeeStatus employeeStatus)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    employeeStatusService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.StatusName.ToUpper().Trim() == employeeStatus.StatusName.ToUpper().Trim())
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Status Name found, Save denied";
                }
                else
                {
                    var entity = new EmployeeStatus();
                    entity.StatusName = employeeStatus.StatusName;
                    entity.StatusValue = employeeStatus.StatusValue;
                    entity.ViewOrder = employeeStatus.ViewOrder;
                    entity.IsActive = true;
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeStatusService.Create(entity);
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateEmployeeStatus(EmployeeStatus employeeStatus)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   employeeStatusService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.StatusId != employeeStatus.StatusId &&
                               p.StatusName.ToUpper().Trim() == employeeStatus.StatusName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Status Name found, Save denied";
                }
                else
                {
                    var entity = employeeStatusService.GetById(employeeStatus.StatusId);
                    entity.StatusId = employeeStatus.StatusId;
                    entity.StatusName = employeeStatus.StatusName;
                    entity.StatusValue = employeeStatus.StatusValue;
                    entity.ViewOrder = employeeStatus.ViewOrder;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    employeeStatusService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        //public JsonResult ListEmployeeStatus(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        //{
        //    var vmcar = employeeStatusService.GetAll().Where(t => t.IsActive == true);
        //    var listVMcartype = vmcar.AsEnumerable().Select(a => new EmployeeStatus()
        //    {
        //        StatusId = a.StatusId,
        //        StatusName = a.StatusName,
        //        StatusValue = a.StatusValue,
        //        ViewOrder = a.ViewOrder
        //    }).ToList();

        //    var currentPageRecords = listVMcartype.Skip(jtStartIndex).Take(jtPageSize);

        //    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = listVMcartype.LongCount(), JsonRequestBehavior.AllowGet });
        //}

        public ActionResult ListEmployeeStatus([DataSourceRequest]DataSourceRequest request)
        {

            var EmployeeStatusList = employeeStatusService.GetMany(p => p.IsActive == true).ToList();
            var viewList = EmployeeStatusList.AsEnumerable().Select((p, sl) => new EmployeeStatusViewModel()
            {
                StatusId = p.StatusId,
                StatusName = p.StatusName,
                StatusValue = p.StatusValue,
                ViewOrder = p.ViewOrder
            }).ToList();
            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult InformationDeleteEmployeeStatus(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeStatusService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeStatusService.Update(model);
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
        #endregion

        #region Actions
        public ActionResult Index()
        {
            //var model = new VMCarConfigurationViewModel();
            //MapDropdownForEmployeeStatusbyId(model);
            return View();
        }
        #endregion
    }
}