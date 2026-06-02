using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Basic
{
    public class ReportSignatureController : BaseController
    {
        public ReportSignatureController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Id = 0;
            return View();
        }

        public ActionResult Edit(int Id)
        {
            using (var DB = new gHRMDBContext())
            {
                var Obj = (from S in DB.ReportSignatures
                            join E_A in DB.Employees on S.ASignatureId equals E_A.EmployeeId
                            join E_B in DB.Employees on S.BSignatureId equals E_B.EmployeeId into c_cd_E_B
                            from E_B in c_cd_E_B.DefaultIfEmpty()
                            join E_C in DB.Employees on S.CSignatureId equals E_C.EmployeeId into c_cd_E_C
                            from E_C in c_cd_E_C.DefaultIfEmpty()
                            join E_D in DB.Employees on S.DSignatureId equals E_D.EmployeeId into c_cd_E_D
                            from E_D in c_cd_E_D.DefaultIfEmpty()
                            join E_E in DB.Employees on S.ESignatureId equals E_E.EmployeeId into c_cd_E_E
                            from E_E in c_cd_E_E.DefaultIfEmpty()
                            join E_F in DB.Employees on S.FSignatureId equals E_F.EmployeeId into c_cd_E_F
                            from E_F in c_cd_E_F.DefaultIfEmpty()
                            orderby S.Description
                            select new
                            {
                                S.Code,
                                Signature1 = E_A.EmployeeCode,
                                Signature2 = E_B.EmployeeCode,
                                Signature3 = E_C.EmployeeCode,
                                Signature4 = E_D.EmployeeCode,
                                Signature5 = E_E.EmployeeCode,
                                Signature6 = E_F.EmployeeCode,
                            }).FirstOrDefault();
                if (Obj == null) return Redirect("/ReportSignature/Create");
                ViewBag.Id = Id;
                ViewData["ReportName"] = Obj.Code;
                ViewData["SignEmp_Id1"] = Obj.Signature1;
                ViewData["SignEmp_Id2"] = Obj.Signature2 ?? "";
                ViewData["SignEmp_Id3"] = Obj.Signature3 ?? "";
                ViewData["SignEmp_Id4"] = Obj.Signature4 ?? "";
                ViewData["SignEmp_Id5"] = Obj.Signature5 ?? "";
                ViewData["SignEmp_Id6"] = Obj.Signature6 ?? "";
            }
            return View("Create");
        }

        [HttpPost]
        public JsonResult Save(int Id, string ReportCode, string ReportDes, long SignEmp_Id1, long SignEmp_Id2, long SignEmp_Id3, long SignEmp_Id4, long SignEmp_Id5, long SignEmp_Id6)
        {
            try
            {
                string message = "";
                if (!IsValid(ReportCode, ReportDes, SignEmp_Id1, out message)) return GetErrorMessageResult(message);
                using (var DB = new gHRMDBContext())
                {
                    ReportSignature Obj = Id > 0 ? DB.ReportSignatures.Find(Id) : new ReportSignature();
                    Obj.Code = ReportCode;
                    Obj.Description = ReportDes;
                    Obj.ASignatureId = SignEmp_Id1;

                    if (Id == 0)
                    {
                        Obj.IsActive = true;
                        Obj.CreateDate = DateTime.Now;
                        Obj.CreateUser = LoggedInEmployeeId ?? 0;
                    }
                    else
                    {
                        Obj.UpdateDate = DateTime.Now;
                        Obj.UpdateUser = LoggedInEmployeeId ?? 0;
                    }
                    if (SignEmp_Id2 > 0) Obj.BSignatureId = SignEmp_Id2;
                    else Obj.BSignatureId = null;
                    if (SignEmp_Id3 > 0) Obj.CSignatureId = SignEmp_Id3;
                    else Obj.CSignatureId = null;
                    if (SignEmp_Id4 > 0) Obj.DSignatureId = SignEmp_Id4;
                    else Obj.DSignatureId = null;
                    if (SignEmp_Id5 > 0) Obj.ESignatureId = SignEmp_Id5;
                    else Obj.ESignatureId = null;
                    if (SignEmp_Id6 > 0) Obj.FSignatureId = SignEmp_Id6;
                    else Obj.FSignatureId = null;
                    if (Id == 0) DB.ReportSignatures.Add(Obj);
                    DB.SaveChanges();
                }
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                string message = Funct.GetError(ex);
                if (message.Contains("UK_ReportSignature_Code")) message = "Duplicate Report Code was found";
                if (message.Contains("UK_ReportSignature_Name")) message = "Duplicate Report Name was found";
                return GetErrorMessageResult(message);
            }
        }

        private bool IsValid(string ReportCode, string ReportDes, long SignEmp_Id1, out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(ReportCode) || string.IsNullOrEmpty(ReportDes))
            {
                message = "Report is required";
                return false;
            }
            if (0 == SignEmp_Id1)
            {
                message = "Signature 1 Employee Code is required";
                return false;
            }
            return true;
        }

        public JsonResult LoadReportSignatureList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = (from S in DB.ReportSignatures
                                 join E_A in DB.Employees on S.ASignatureId equals E_A.EmployeeId
                                 join E_B in DB.Employees on S.BSignatureId equals E_B.EmployeeId into c_cd_E_B
                                 from E_B in c_cd_E_B.DefaultIfEmpty()
                                 join E_C in DB.Employees on S.CSignatureId equals E_C.EmployeeId into c_cd_E_C
                                 from E_C in c_cd_E_C.DefaultIfEmpty()
                                 join E_D in DB.Employees on S.DSignatureId equals E_D.EmployeeId into c_cd_E_D
                                 from E_D in c_cd_E_D.DefaultIfEmpty()
                                 join E_E in DB.Employees on S.ESignatureId equals E_E.EmployeeId into c_cd_E_E
                                 from E_E in c_cd_E_E.DefaultIfEmpty()
                                 join E_F in DB.Employees on S.FSignatureId equals E_F.EmployeeId into c_cd_E_F
                                 from E_F in c_cd_E_F.DefaultIfEmpty()
                                 orderby S.Description
                                 select new
                                 {
                                     Id = S.Id,
                                     S.Description,
                                     Signature1 = E_A.EmployeeName + " (" + E_A.EmployeeCode + ")",
                                     Signature2 = E_B == null ? "" : E_B.EmployeeName + " (" + E_B.EmployeeCode + ")",
                                     Signature3 = E_C == null ? "" : E_C.EmployeeName + " (" + E_C.EmployeeCode + ")",
                                     Signature4 = E_D == null ? "" : E_D.EmployeeName + " (" + E_D.EmployeeCode + ")",
                                     Signature5 = E_E == null ? "" : E_E.EmployeeName + " (" + E_E.EmployeeCode + ")",
                                     Signature6 = E_F == null ? "" : E_F.EmployeeName + " (" + E_F.EmployeeCode + ")"
                                 }).ToList();
                    DataSourceResult result = DataList.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException });
            }
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    ReportSignature _ReportSignature = DB.ReportSignatures.Find(Id);
                    if (_ReportSignature != null)
                    {
                        DB.ReportSignatures.Remove(_ReportSignature);
                        DB.SaveChanges();
                    }
                }
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}