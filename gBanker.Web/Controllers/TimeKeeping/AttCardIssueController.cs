
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using AutoMapper;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace gHRM.Web.Controllers
{
    public class AttCardIssueController : BaseController
    {

        #region Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IAttCardIssueService attCardIssueService;

        public AttCardIssueController(IEmployeeSPService employeeSPService, IAttCardIssueService attCardIssueService)
        {

            this.employeeSPService = employeeSPService;
            this.attCardIssueService = attCardIssueService;

        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Create(string EmployeeId = "", string CardNo = "", string CardIssueDateView = "", string Remarks = "")
        {
            string result = "OK";
            try
            {
                AttCardIssueViewModel model = new AttCardIssueViewModel();
                model.EmployeeId = Convert.ToInt64(EmployeeId);
                model.CardNo = CardNo;
                model.CardIssueDate = Convert.ToDateTime(CardIssueDateView);
                model.Remarks = Remarks;

                var entity = Mapper.Map<AttCardIssueViewModel, AttCardIssue>(model);

                entity.EmployeeId = model.EmployeeId;
                entity.CardNo = model.CardNo;
                entity.CardIssueDate = model.CardIssueDate;
                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.CreateDate = DateTime.Now;
                entity.IsActive = true;

                attCardIssueService.Create(entity);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Update(string AttCardIssueId = "", string EmployeeId = "", string CardNo = "", string CardIssueDateView = "", string Remarks = "")
        {
            string result = "OK";
            try
            {
                //End of Check
                AttCardIssueViewModel model = new AttCardIssueViewModel();
                model.AttCardIssueId = Convert.ToInt64(AttCardIssueId);
                model.EmployeeId = Convert.ToInt64(EmployeeId);
                model.CardNo = CardNo;
                model.CardIssueDate = Convert.ToDateTime(CardIssueDateView);
                model.Remarks = Remarks;

                var entity = Mapper.Map<AttCardIssueViewModel, AttCardIssue>(model);

                var GetData = attCardIssueService.GetById(Convert.ToInt32(entity.AttCardIssueId));

                GetData.Remarks = model.Remarks;
                GetData.CardNo = model.CardNo;
                GetData.CardIssueDate = model.CardIssueDate;
                GetData.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                GetData.UpdateDate = DateTime.Now;
                GetData.IsActive = true;

                attCardIssueService.Update(GetData);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(string AttCardIssueId = "")
        {
            string result = "OK";
            try
            {
                var GetData = attCardIssueService.GetById(Convert.ToInt32(AttCardIssueId));
                if (GetData == null)
                {
                    Response.StatusCode = 403;
                }
                GetData.IsActive = false;

                attCardIssueService.Update(GetData);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList([DataSourceRequest]DataSourceRequest request, string IssueId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string IssueIds = Convert.ToString(IssueId);

                if (IssueId != null)
                    sb.Append(" AND ci.AttCardIssueId =" + IssueIds);

                List<AttCardIssueViewModel> List_ViewModel = new List<AttCardIssueViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "att.SP_AttCardIssueList");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttCardIssueViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    AttCardIssueId = row.Field<long>("AttCardIssueId"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    CardNo = row.Field<string>("CardNo"),
                    CardIssueDateView = row.Field<string>("CardIssueDate"),
                    Remarks = row.Field<string>("Remarks"),

                }).ToList();

                if (IssueId != null)
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

    }// End of Class
}// ENd of namespace