using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Core.Extensions;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.IncomeTax;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace gHRM.Web.Controllers
{
    public class IncomeTaxController : BaseController
    {
        #region Variables

        private readonly IIncomeTaxService incomeTaxService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IAspNetRoleService aspNetRoleService;
        public IncomeTaxController(
            IIncomeTaxService incomeTaxService,
            IEmployeeSPService employeeSPService,
            IEmployeeService employeeService,
            IAspNetUserService aspNetUserService,
            IAspNetRoleService aspNetRoleService
            )
        {
            this.incomeTaxService = incomeTaxService;
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.aspNetUserService = aspNetUserService;
            this.aspNetRoleService = aspNetRoleService;
        }

        #endregion

        #region Methods


        [HttpPost]
        public JsonResult GetIncomeTaxList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                // ✅ Pass empty object instead of null to avoid reflection on null


                var result = employeeSPService.GetDataWithParameter(new { LoginEmployeeId = SessionHelper.LoggedInEmployeeID }, "SP_GET_IncomeTaxList");

                var list = result.Tables[0].AsEnumerable().Select(row => new IncomeTaxViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeID = row.Field<long>("EmployeeID"),
                    OfficeID = row.Field<int>("OfficeID"),
                    OfficeName = row.Field<string>("OfficeName"),
                    NationalID = row.Field<string>("NationalID"),
                    TIN = row.Field<string>("TIN"),
                    ReturnRegisterSlNo = row.Field<string>("ReturnRegisterSlNo"),
                    ReturnRegisterVolNo = row.Field<string>("ReturnRegisterVolNo"),
                    ReturnFillingDate = row.Field<string>("ReturnFillingDate"),
                    FiscalYear = row.Field<string>("FiscalYear"),
                    Circle = row.Field<string>("Circle"),
                    TaxArea = row.Field<string>("TaxArea"),
                    TotalIncome = row.Field<string>("TotalIncome"),
                    TotalTaxPaid = row.Field<string>("TotalTaxPaid"),
                    FileLocation = row.Field<string>("FileLocation"),
                    Id = row.Field<int>("Id"),
                    EmployeeName = row.Field<String>("EmployeeName"),
                }).ToList();

                // ✅ Optional: Dynamic sorting
                if (!string.IsNullOrEmpty(jtSorting))
                {
                    var sortParts = jtSorting.Split(' ');
                    var sortColumn = sortParts[0];
                    var sortDirection = sortParts.Length > 1 ? sortParts[1] : "ASC";

                    var propInfo = typeof(IncomeTaxViewModel).GetProperty(sortColumn);
                    if (propInfo != null)
                    {
                        list = sortDirection == "ASC"
                            ? list.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                            : list.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                    }
                }

                var currentPageRecords = list.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = list.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        [HttpPost]
        public JsonResult GetIncomeTaxList2(int jtStartIndex, int jtPageSize, string jtSorting, int EmployeeID)
        {
            try
            {
                // ✅ Pass empty object instead of null to avoid reflection on null

      
                var result = employeeSPService.GetDataWithParameter(new { LoginEmployeeId = EmployeeID }, "SP_GET_IncomeTaxList");

                var list = result.Tables[0].AsEnumerable().Select(row => new IncomeTaxViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeID = row.Field<long>("EmployeeID"),
                    OfficeID = row.Field<int>("OfficeID"),
                    OfficeName = row.Field<string>("OfficeName"),
                    NationalID = row.Field<string>("NationalID"),
                    TIN = row.Field<string>("TIN"),
                    ReturnRegisterSlNo = row.Field<string>("ReturnRegisterSlNo"),
                    ReturnRegisterVolNo = row.Field<string>("ReturnRegisterVolNo"),
                    ReturnFillingDate = row.Field<string>("ReturnFillingDate"),
                    FiscalYear = row.Field<string>("FiscalYear"),
                    Circle = row.Field<string>("Circle"),
                    TaxArea = row.Field<string>("TaxArea"),
                    TotalIncome = row.Field<string>("TotalIncome"),
                    TotalTaxPaid = row.Field<string>("TotalTaxPaid"),
                    FileLocation = row.Field<string>("FileLocation"),
                    Id = row.Field<int>("Id"),
                    EmployeeName = row.Field<String>("EmployeeName"),
                }).ToList();

                // ✅ Optional: Dynamic sorting
                if (!string.IsNullOrEmpty(jtSorting))
                {
                    var sortParts = jtSorting.Split(' ');
                    var sortColumn = sortParts[0];
                    var sortDirection = sortParts.Length > 1 ? sortParts[1] : "ASC";

                    var propInfo = typeof(IncomeTaxViewModel).GetProperty(sortColumn);
                    if (propInfo != null)
                    {
                        list = sortDirection == "ASC"
                            ? list.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                            : list.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();
                    }
                }

                var currentPageRecords = list.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = list.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




        public JsonResult GetEmployeeData(string EmpId)
        {
            List<IncomeTaxViewModel> List_Employee = new List<IncomeTaxViewModel>();
            var param = new { EmpId = EmpId };
            var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_EmpData");

            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
                    .Select(row => new IncomeTaxViewModel
                    {
                        EmployeeID = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        OfficeID = row.Field<int>("OfficeId"),
                    }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }

        private void MapDropDownList(IncomeTaxViewModel model)
        {
            // Add any dropdown mapping logic if needed (Fiscal Year, etc.)
        }

        #endregion

        #region Events
        [AllowAnonymous] // এই লাইন যোগ করুন
        public ActionResult Index()
        {
            return View();
        }

        // GET: IncomeTax/Create
        public ActionResult Create()
        {
            IncomeTaxViewModel model = new IncomeTaxViewModel();
            MapDropDownList(model);
            model.EmployeeID = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;

            var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
            var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == model.EmployeeID).RoleId;

            if (superAdminRoleId == loginRoleId)
            {
                ViewData["UserRole"] = "Super Admin";
            }
            string UserRoleName = aspNetRoleService.GetNameById(SessionHelper.LoggedInRoleId.ToString());
            string EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE = AppSetting.Get(AppSetting.EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE, HttpContext);
            ViewBag.IsEmployeeCodeEditAllowed = !string.IsNullOrEmpty(UserRoleName) && UserRoleName == EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE;
            return View(model);
        }

        public ActionResult Create2()
        {
            IncomeTaxViewModel model = new IncomeTaxViewModel();
            MapDropDownList(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(IncomeTaxViewModel model)
        {
            IncomeTax entity = null;
            
            try
            {
                // Try mapping ViewModel to Entity
                try
                {
                    entity = Mapper.Map<IncomeTaxViewModel, IncomeTax>(model);
                    int id = (int)entity.EmployeeID;
                }
                catch (AutoMapperMappingException ex)
                {
                    var inner = ex.InnerException?.Message;
                    throw new Exception($"AutoMapper Mapping Error: {ex.Message}. Inner: {inner}");
                }

                // Handle file upload
                if (model.FileLocationU != null)
                {
                    DateTime dt = DateTime.Now;
                    string uploadDay = $"IncomeTax_{dt:dd-MM-yyyy}";

                    var fileName = Path.GetFileName(model.FileLocationU.FileName);
                    var path = Path.Combine(@"F:\IIS\ghrm\YPSA\UploadIncomeTaxAttachment", uploadDay + "_" + fileName);

                    model.FileLocationU.SaveAs(path);
                    entity.FileLocation = path;
                }

                entity.CreateDate = DateTime.Now;
                entity.isActive = true;

                // Save to database
                incomeTaxService.Create(entity);

                TempData["Success"] = "Successfully saved.";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error occurred while saving. {ex.Message}";
                return RedirectToAction("Create");
            }
        }


        [HttpPost]
        public ActionResult Create2(IncomeTaxViewModel model)
        {
            IncomeTax entity = null;

            try
            {
                // Try mapping ViewModel to Entity
                try
                {
                    entity = Mapper.Map<IncomeTaxViewModel, IncomeTax>(model);
                    int id = (int)entity.EmployeeID;
                }
                catch (AutoMapperMappingException ex)
                {
                    var inner = ex.InnerException?.Message;
                    throw new Exception($"AutoMapper Mapping Error: {ex.Message}. Inner: {inner}");
                }

                // Handle file upload
                if (model.FileLocationU != null)
                {
                    DateTime dt = DateTime.Now;
                    string uploadDay = $"IncomeTax_{dt:dd-MM-yyyy}";

                    var fileName = Path.GetFileName(model.FileLocationU.FileName);
                    var path = Path.Combine(@"F:\IIS\ghrm\YPSA\UploadIncomeTaxAttachment", uploadDay + "_" + fileName);

                    model.FileLocationU.SaveAs(path);
                    entity.FileLocation = path;
                }

                entity.CreateDate = DateTime.Now;
                entity.isActive = true;

                // Save to database
                incomeTaxService.Create(entity);

                TempData["Success"] = "Successfully saved.";
                return RedirectToAction("Create2");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error occurred while saving. {ex.Message}";
                return RedirectToAction("Create2");
            }
        }


        // POST: IncomeTax/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncomeTaxViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = incomeTaxService.GetById((int)model.Id); // Replace with actual primary key ID if different
                    if (entity == null)
                        return HttpNotFound();

                    // Update values
                    entity.OfficeID = model.OfficeID;
                    entity.NationalID = model.NationalID;
                    entity.TIN = model.TIN;
                    entity.ReturnRegisterSlNo = model.ReturnRegisterSlNo;
                    entity.ReturnRegisterVolNo = model.ReturnRegisterVolNo;
                    entity.ReturnFillingDate = model.ReturnFillingDate;
                    entity.FiscalYear = model.FiscalYear;
                    entity.Circle = model.Circle;
                    entity.TaxArea = model.TaxArea;
                    entity.TotalIncome = model.TotalIncome;
                    entity.TotalTaxPaid = model.TotalTaxPaid;
                    //entity.FileLocation = model.FileLocation;

                    // Preserve the old file path if no new file is uploaded
                    if (model.FileLocationU != null)
                    {
                        DateTime dt = DateTime.Now;
                        string uploadDay = $"IncomeTax_{dt:dd-MM-yyyy}";
                        var fileName = Path.GetFileName(model.FileLocationU.FileName);
                        var path = Path.Combine(@"F:\IIS\ghrm\YPSA\UploadIncomeTaxAttachment", uploadDay + "_" + fileName);

                        model.FileLocationU.SaveAs(path);
                        entity.FileLocation = path; // Save new file path
                    }
                    else
                    {
                        // Retain existing file path
                        entity.FileLocation = model.FileLocation;
                    }

                    entity.CreateDate = model.CreateDate ?? DateTime.Now;

                    incomeTaxService.Update(entity); // Or unitOfWork.Commit() depending on your pattern

                    TempData["Success"] = "Updated successfully!";
                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Update failed: " + ex.Message;
                }
            }

            // Repopulate dropdowns
            //model.FiscalYearList = Common.PopulateFiscalYearList();
            return View(model);
        }


        // POST: IncomeTax/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(IncomeTaxViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = incomeTaxService.GetById((int)model.Id); // Replace with actual primary key ID if different
                    if (entity == null)
                        return HttpNotFound();

                    // Update values
                    entity.OfficeID = model.OfficeID;
                    entity.NationalID = model.NationalID;
                    entity.TIN = model.TIN;
                    entity.ReturnRegisterSlNo = model.ReturnRegisterSlNo;
                    entity.ReturnRegisterVolNo = model.ReturnRegisterVolNo;
                    entity.ReturnFillingDate = model.ReturnFillingDate;
                    entity.FiscalYear = model.FiscalYear;
                    entity.Circle = model.Circle;
                    entity.TaxArea = model.TaxArea;
                    entity.TotalIncome = model.TotalIncome;
                    entity.TotalTaxPaid = model.TotalTaxPaid;
                    //entity.FileLocation = model.FileLocation;

                    // Preserve the old file path if no new file is uploaded
                    if (model.FileLocationU != null)
                    {
                        DateTime dt = DateTime.Now;
                        string uploadDay = $"IncomeTax_{dt:dd-MM-yyyy}";
                        var fileName = Path.GetFileName(model.FileLocationU.FileName);
                        var path = Path.Combine(@"F:\IIS\ghrm\YPSA\UploadIncomeTaxAttachment", uploadDay + "_" + fileName);

                        model.FileLocationU.SaveAs(path);
                        entity.FileLocation = path; // Save new file path
                    }
                    else
                    {
                        // Retain existing file path
                        entity.FileLocation = model.FileLocation;
                    }

                    entity.CreateDate = model.CreateDate ?? DateTime.Now;

                    incomeTaxService.Update(entity); // Or unitOfWork.Commit() depending on your pattern

                    TempData["Success"] = "Updated successfully!";
                    return RedirectToAction("Create2");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Update failed: " + ex.Message;
                }
            }

            // Repopulate dropdowns
            //model.FiscalYearList = Common.PopulateFiscalYearList();
            return View(model);
        }

        // Download attachment
        public ActionResult DownloadFile(int id)
        {
            var getData = incomeTaxService.GetById(Convert.ToInt32(id));

            if (getData == null)
            {
                return Content("File not found or record doesn't exist.");
            }

            var location = getData.FileLocation;

            if (string.IsNullOrEmpty(location))
            {
                return Content("No file uploaded for this record.");
            }

            /*string fullPath = Server.MapPath(location);*/
            //string fullPath = Path.IsPathRooted(location) ? location : Server.MapPath(location);
            //string fileName = Path.GetFileName(fullPath);

            //if (!System.IO.File.Exists(fullPath))
            //{
            //    return Content("File not found on server.");
            //}

            var fileName = Path.GetFileName(location);

            return File(location, "application/octet-stream", fileName);
        }



        // GET: IncomeTax/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var entity = incomeTaxService.GetById(id.Value); // Safely use id.Value here

            if (entity == null)
                return HttpNotFound();

            var model = new IncomeTaxViewModel
            {
                EmployeeID = entity.EmployeeID,
                OfficeID = entity.OfficeID,
                NationalID = entity.NationalID,
                TIN = entity.TIN,
                ReturnRegisterSlNo = entity.ReturnRegisterSlNo,
                ReturnRegisterVolNo = entity.ReturnRegisterVolNo,
                ReturnFillingDate = entity.ReturnFillingDate,
                FiscalYear = entity.FiscalYear,
                Circle = entity.Circle,
                TaxArea = entity.TaxArea,
                TotalIncome = entity.TotalIncome,
                TotalTaxPaid = entity.TotalTaxPaid,
                FileLocation = entity.FileLocation,
                CreateDate = entity.CreateDate,
                //FiscalYearList = Common.PopulateFiscalYearList()
            };

            var emp = employeeService.GetById((int)entity.EmployeeID);
            if (emp != null)
            {
                model.EmployeeName = emp.EmployeeName;
                model.EmployeeCode = emp.EmployeeCode;
            }

            return View(model);
        }


        // GET: IncomeTax/Edit/5
        public ActionResult Edit2(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var entity = incomeTaxService.GetById(id.Value); // Safely use id.Value here

            if (entity == null)
                return HttpNotFound();

            var model = new IncomeTaxViewModel
            {
                EmployeeID = entity.EmployeeID,
                OfficeID = entity.OfficeID,
                NationalID = entity.NationalID,
                TIN = entity.TIN,
                ReturnRegisterSlNo = entity.ReturnRegisterSlNo,
                ReturnRegisterVolNo = entity.ReturnRegisterVolNo,
                ReturnFillingDate = entity.ReturnFillingDate,
                FiscalYear = entity.FiscalYear,
                Circle = entity.Circle,
                TaxArea = entity.TaxArea,
                TotalIncome = entity.TotalIncome,
                TotalTaxPaid = entity.TotalTaxPaid,
                FileLocation = entity.FileLocation,
                CreateDate = entity.CreateDate,
                //FiscalYearList = Common.PopulateFiscalYearList()
            };

            var emp = employeeService.GetById((int)entity.EmployeeID);
            if (emp != null)
            {
                model.EmployeeName = emp.EmployeeName;
                model.EmployeeCode = emp.EmployeeCode;
            }

            return View(model);
        }




        public ActionResult Details(int id)
        {
            //var entity = incomeTaxService.GetById(id); // or use your repo pattern
            //if (entity == null)
            //{
            //    return HttpNotFound();
            //}

            //var model = Mapper.Map<IncomeTax, IncomeTaxViewModel>(entity);
            //// If you have any dropdowns (e.g., fiscal year), load them here
            //// MapDropDownList(model);

            //return View(model);

            var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });
            //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });

           // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
           // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });


            PrintSSRSReport("/gHRMPlus_Reports/IncomeTaxDetails", paramValues.ToArray());  /// 31


            return Content(string.Empty);
        }

        public ActionResult Report22(string fiscalyear)
        {
            //var entity = incomeTaxService.GetById(id); // or use your repo pattern
            //if (entity == null)
            //{
            //    return HttpNotFound();
            //}

            //var model = Mapper.Map<IncomeTax, IncomeTaxViewModel>(entity);
            //// If you have any dropdowns (e.g., fiscal year), load them here
            //// MapDropDownList(model);

            //return View(model);

            var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = fiscalyear });
            //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });

            // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
            // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });


            PrintSSRSReport("/gHRMPlus_Reports/IncomeTaxDetails2", paramValues.ToArray());  /// 31


            return Content(string.Empty);
        }


        


        // Soft Delete
        public ActionResult Delete(int id)
        {
            string result = "OK";

            var record = incomeTaxService.GetById(id);
            if (record != null)
            {
                record.isActive = false;
                record.UpdateDate = DateTime.Now;
                record.UpdateUser = Convert.ToInt64(LoggedInEmployeeId); // if available
                incomeTaxService.Update(record);
            }
            else
            {
                result = "Not Found";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
