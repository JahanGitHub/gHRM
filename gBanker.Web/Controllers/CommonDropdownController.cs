using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using System.Text;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class CommonDropdownController : BaseController
    {
        #region Variables

        StringBuilder sb = new StringBuilder();
        private readonly IEmployeeSPService employeeSPService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public CommonDropdownController(IEmployeeSPService employeeSPService)
        {
            this.employeeSPService = employeeSPService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        public JsonResult GetHeadOfficeByOfficeType()
        {
            var offList = commonDynamicDropDown.GetHeadOfficeList();
            return Json(new { Data = offList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectsByOfficeType()
        {
            var list = commonDynamicDropDown.GetProjectOfficeList();
            return Json(new { Data = list }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeptListByOfficeType(int OfficeTypeId)
        {
            var deptList = commonDynamicDropDown.GetDepartmentByOfficeTypeId(OfficeTypeId);
            return Json(new { Data = deptList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllActiveDeptList()
        {
            var deptList = commonDynamicDropDown.GetAllActiveDepartmentList();
            return Json(new { Data = deptList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeTypeWiseOfficeList(int OfficeTypeId)
        {
            var officeList = commonDynamicDropDown.GetOfficeTypeWiseOfficeList(OfficeTypeId);
            return Json(new { Data = officeList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeListByOffice_Department_OfficeDesignation(int OfficeId, int DepartmentId, int OfficeDesignationId)
        {
            var officeList = commonDynamicDropDown.GetEmployeeListByOffice_Department_OfficeDesignation(OfficeId, DepartmentId, OfficeDesignationId);
            return Json(new { Data = officeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveTypeListByEmployeeStatusAndGender(string EmployeeGender, int EmployeeStatusId)
        {
            var leaveTypeList = commonDynamicDropDown.GetLeaveTypeListByEmployeeStatusAndGender(EmployeeGender.Trim(), EmployeeStatusId);
            return Json(new { Data = leaveTypeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOfficeAndDepartmentWiseEmployeeList(int OfficeId, int DepartmentId)
        {
            var replacementEmployeeList = commonDynamicDropDown.GetOfficeAndDepartmentWiseEmployeeList(OfficeId, DepartmentId);
            return Json(new { Data = replacementEmployeeList }, JsonRequestBehavior.AllowGet);
        }
    }
}