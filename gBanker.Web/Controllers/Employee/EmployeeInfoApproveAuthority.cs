using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Service;
using gHRM.Web.DropDownService;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
   
    public class EmployeeInfoApproveAuthorityController : BaseController
    {
        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeInformationApprovalService employeeInformationApprovalService;

        public EmployeeInfoApproveAuthorityController(
              IEmployeeService employeeService
            , IEmployeeInformationApprovalService employeeInformationApprovalService
            , IAspNetUserService aspNetUserService
            )
        {
            this.employeeService = employeeService;
            this.employeeInformationApprovalService = employeeInformationApprovalService;
        }

        #endregion


        #region Events

        public ActionResult index()
        {
            return View();
        }


        #endregion

        #region HttpRequests

        public JsonResult GetApprovalAuthorityInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var authorityInfo = employeeInformationApprovalService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewAuthorityInfo = authorityInfo.AsEnumerable().Select(p => new EmployeeInformationApprovalViewModel()
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                EmployeeCode = p.EmployeeCode
            }).ToList();
            var currentPageRecords = viewAuthorityInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewAuthorityInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult SaveApprovalAuthoriy(EmployeeInformationApprovalViewModel obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicate = employeeInformationApprovalService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == obj.EmployeeCode).ToList();

                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Already this employee configured, Save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var employeeId = employeeService.GetByCode(obj.EmployeeCode).EmployeeId;

                    var model = new EmployeeInformationApproval();
                    model.EmployeeCode = obj.EmployeeCode;
                    model.EmployeeId = employeeId;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeInformationApprovalService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                }
            }
            catch (Exception ex)
            {
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteApprovalAuthoriy(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeInformationApprovalService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeInformationApprovalService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception ex)
            {
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
