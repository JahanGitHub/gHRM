using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.NobinAccMapper;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.VoucherinNobinEP
{
    public class SalaryVoucherPostController : BaseController
    {
        #region Private Variables
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeSPService employeeService;

        #endregion Private Variables
        #region Ctor
        public SalaryVoucherPostController()
        {
            this.commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = new EmployeeSPService();
        }
        #endregion Ctor

        // GET: SalaryVoucherPost
        #region Action
        public ActionResult Index()
        {
            try
            {
                var compXAccLst = new gHRMDBContext().Database.SqlQuery<AccountMappingWithNobinViewModel>("sp_AccountMappingWithNobin").ToList();
                if (compXAccLst.Any())
                    return View(compXAccLst);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(new List<AccountMappingWithNobinViewModel>());
            }
        }

        public ActionResult ReverseVoucherMapping()
        {
            try
            {
                var compXAccLst = new gHRMDBContext().Database.SqlQuery<AccountMappingWithNobinViewModel>("sp_OfficeXNobinAccountMapping").ToList();
                if (compXAccLst.Any())
                    return View(compXAccLst);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(new List<AccountMappingWithNobinViewModel>());
            }
        }
        public ActionResult SalarySendInNobin()
        {
            var lst = new gHRMDBContext().EmployeeMonthlySalaryApproved.Where(x => x.IsActive && x.IsApproved)
                .Select(x => new { x.SalaryYear, x.SalaryMonth }).Distinct().ToList();
            ViewBag.SalaryYear = lst.Select(s => s.SalaryYear).Distinct().OrderByDescending(x => x).ToList();
            ViewBag.SalaryMonth = lst.Select(s => s.SalaryMonth).Distinct().OrderBy(x => x).ToList();
            ViewBag.CompanyBank = commonDynamicDropDown.GetCompanyBankListForNobin().ToList();
            return View();
        }
        #endregion Action
        #region Ajax
        [HttpPost]
        public JsonResult NobinAccountHead(string acccode)
        {
            try
            {
                string acc = "";
                var compXAccLst = new gHRMDBContext().Database.SqlQuery<string>("sp_AccountHeadFromNobin '" + acccode + "'").ToList();
                if (compXAccLst.Any())
                    acc = compXAccLst.First();
                return Json(acc);
            }
            catch (Exception ex)
            {
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult PostNobinAccountHead(List<AccountMappingWithNobin> obj)
        {
            try
            {
                if (obj.Any())
                {
                    new gHRMDBContext().Database.ExecuteSqlCommand("update AccountMappingWithNobin SET IsActive=0 ,UpdateBy="+SessionHelper.LoggedInEmployeeID+ ",UpdateDate=getdate() WHERE IsActive=1");
                    foreach (var o in obj)
                    {
                        string sql = $@"INSERT INTO AccountMappingWithNobin(ComponentName,NobinAccCode,VoucherNaration,IsActive,CreateBy,CreateDate)
                        values('" + o.ComponentName + "','" + o.NobinAccCode + "','" + o.VoucherNaration + "',1," + SessionHelper.LoggedInEmployeeID+ ",getdate())";
                        new gHRMDBContext().Database.ExecuteSqlCommand(sql);
                    }
                    return Json("Save Successfully");
                }
                else return Json("data not found");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult PostReverseVoucherMapping(List<OfficeXReverseAccountMappingWithhNobin> obj)
        {
            try
            {
                if (obj.Any())
                {
                    new gHRMDBContext().Database.ExecuteSqlCommand("update OfficeXReverseAccountMappingWithhNobin SET IsActive=0 WHERE IsActive=1");
                    foreach (var o in obj)
                    {
                        string sql = $@"INSERT INTO OfficeXReverseAccountMappingWithhNobin(OfficeCode,ReverseNobinAccCodeForDR,ReverseNobinAccCodeForCR,ReverseVoucherNaration,IsActive,CreateBy,CreateDate)
                        values('" + o.OfficeCode + "','" + o.ReverseNobinAccCodeForDR + "','" + o.ReverseNobinAccCodeForCR + "','" + o.ReverseVoucherNaration + "',1," + LoginUserOfficeID + ",getdate())";
                        new gHRMDBContext().Database.ExecuteSqlCommand(sql);
                    }
                    return Json("Save Successfully");
                }
                else return Json("data not found");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult SendSalaryVoucherData(int salaryMonth, int salaryYear, string voucherDate,int CompanyBankId,string refferenceNo)
        {
            int statuscode = 0; string msg = "";

            DateTime vdate = DateTime.Now;
            if (!string.IsNullOrEmpty(voucherDate))
                DateTime.TryParse(voucherDate, out vdate);

            if (DateTime.MinValue.Equals(vdate))
                msg = "Voucher date is not correct format.";
            else if (salaryYear >= vdate.Year && salaryMonth > vdate.Month)
                msg = "Salary date cannot be greater than voucher date.";
            else if(CompanyBankId==0)
                msg = "Bank is required.";
            else
            {
                using (gHRMDBContext db = new gHRMDBContext())
                {
                    try
                    {
                        List<SqlParameter> parmLst = new List<SqlParameter>();
                        parmLst.Add(new SqlParameter("@salaryMonth", salaryMonth));
                        parmLst.Add(new SqlParameter("@salaryYear", salaryYear));
                        parmLst.Add(new SqlParameter("@voucherdate", voucherDate));
                        parmLst.Add(new SqlParameter("@companyBankId", CompanyBankId));
                        parmLst.Add(new SqlParameter("@createBy", SessionHelper.LoggedInEmployeeID));
                        parmLst.Add(new SqlParameter("@refferenceNo", refferenceNo));
                        db.Database.ExecuteSqlCommand("EXEC sp_SendSalaryinNEP @salaryMonth,@salaryYear,@voucherdate,@companyBankId,@createBy,@refferenceNo", parmLst.ToArray());
                        statuscode = 200;
                        msg = "Voucher Successfully Send.";
                    }
                    catch (SqlException ex)
                    {
                        statuscode = ex.State /*ex.Number*/;
                        msg = "Error line:" + ex.LineNumber + "</br>" + ex.Message; ;
                    }
                    finally
                    {
                        db.Dispose();
                    }
                }
            }
            return Json(new { statuscode = statuscode, Message = msg });
        }
        #endregion Ajax
        #region NEP Report
        public ActionResult OfficeXSalaryData(int salaryMonth, int salaryYear)
        {
            var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = salaryMonth.ToString() });
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = salaryYear });
            PrintSSRSReport("/gHRMPlus_Reports/OfficeWiseSalaryForNEP", paramValues.ToArray());
            return Content(string.Empty);
        }

        public ActionResult OfficeXBonusData(int bonusMonth, int bonusYear)
        {
            var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = bonusMonth.ToString() });
            paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = bonusYear });
            PrintSSRSReport("/gHRMPlus_Reports/OfficeWiseBonusForNEP", paramValues.ToArray());
            return Content(string.Empty);
        }
        #endregion NEP Report


        [HttpGet]
        public JsonResult GetBonusMonth(int BonusYear)
        {
            List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
            var param = new { @bonusYear = BonusYear };
            var List = employeeService.GetDataWithParameter(param, "GetBonusMonthByYear");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new PRComponentViewModel
            {
                PRComponentID = row.Field<int>("PRComponentID"),
                ComponentName = row.Field<string>("ComponentName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PRComponentID.ToString(),
                Text = string.Format("{0}", x.ComponentName)
                // Text = string.Format("{0} - {1}", x.ComponentName, x.PRComponentID)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }// Populate ddl


        [HttpGet]
        public JsonResult GetSalaryMonth(int SalaryYear)
        {
            List<PRComponentViewModel> List_ViewModel = new List<PRComponentViewModel>();
            var param = new { @salaryYear = SalaryYear };
            var List = employeeService.GetDataWithParameter(param, "GetSalaryMonthByYear");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new PRComponentViewModel
            {
                PRComponentID = row.Field<int>("PRComponentID"),
                ComponentName = row.Field<string>("ComponentName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PRComponentID.ToString(),
                Text = string.Format("{0}", x.ComponentName)
                // Text = string.Format("{0} - {1}", x.ComponentName, x.PRComponentID)
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
        public JsonResult SendBonusVoucherData(int bonusMonth, int bonusYear, string voucherDate, int CompanyBankId, string refferenceNo)
        {
            int statuscode = 0; string msg = "";

            DateTime vdate = DateTime.Now;
            if (!string.IsNullOrEmpty(voucherDate))
                DateTime.TryParse(voucherDate, out vdate);

            if (DateTime.MinValue.Equals(vdate))
                msg = "Voucher date is not correct format.";
            else if (bonusYear >= vdate.Year && bonusMonth > vdate.Month)
                msg = "Bonus date cannot be greater than voucher date.";
            else if (CompanyBankId == 0)
                msg = "Bank is required.";
            else
            {
                using (gHRMDBContext db = new gHRMDBContext())
                {
                    try
                    {
                        List<SqlParameter> parmLst = new List<SqlParameter>();
                        parmLst.Add(new SqlParameter("@bonusMonth", bonusMonth));
                        parmLst.Add(new SqlParameter("@bonusYear", bonusYear));
                        parmLst.Add(new SqlParameter("@voucherdate", voucherDate));
                        parmLst.Add(new SqlParameter("@companyBankId", CompanyBankId));
                        parmLst.Add(new SqlParameter("@createBy", SessionHelper.LoggedInEmployeeID));
                        parmLst.Add(new SqlParameter("@refferenceNo", refferenceNo));
                        db.Database.ExecuteSqlCommand("EXEC sp_SendBonusinNEP @bonusMonth,@bonusYear,@voucherdate,@companyBankId,@createBy,@refferenceNo", parmLst.ToArray());
                        statuscode = 200;
                        msg = "Voucher Successfully Send.";
                    }
                    catch (SqlException ex)
                    {
                        statuscode = ex.State /*ex.Number*/;
                        msg = "Error line:" + ex.LineNumber + "</br>" + ex.Message; ;
                    }
                    finally
                    {
                        db.Dispose();
                    }
                }
            }
            return Json(new { statuscode = statuscode, Message = msg });
        }


    }// End Class
}// END Namespace