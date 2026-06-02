#region Usings

using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFProfitDistributionProcessController : BaseController
    {
        #region Usings

        private readonly IProcessLogService processLogService;
        private readonly IYearEndProcessLogService yearEndProcessLogService;
        private readonly IProfitDistProcessLogService profitDistProcessLogService;
        private readonly IOrganizationSetupService orgSetupService;
        private readonly IProfitDeclarationService profitDeclarationService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly int transactionCategoryId = 3; //Contribution Interest
        #endregion

        #region Ctor
        public PFProfitDistributionProcessController(IProcessLogService processLogService, IYearEndProcessLogService yearEndProcessLogService, IProfitDistProcessLogService profitDistProcessLogService, IOrganizationSetupService orgSetupService, IProfitDeclarationService profitDeclarationService, IEmployeeSPService employeeSPService)
        {
            this.processLogService = processLogService;
            this.yearEndProcessLogService = yearEndProcessLogService;
            this.profitDistProcessLogService = profitDistProcessLogService;
            this.orgSetupService = orgSetupService;
            this.profitDeclarationService = profitDeclarationService;
            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Process

        //Addition: Add another option- Already distributed
        public ActionResult Create()
        {
            var model = new ProfitDistributionProcessViewModel();

            var lst = profitDeclarationService.GetMany(x => x.DeclarationStatus == ProfitDeclarationConstants.Approved || x.DeclarationStatus == ProfitDeclarationConstants.Entry);
            if (lst.Any())
            {
                if (lst.Count() > 1)
                    model.Message = "Profit Declaration Multiple found";
                else
                {
                    model.DeclararionId = lst.First().DeclararionId;

                    model.Distribution = ((lst.First().DistribursAmount ?? 0) == 0 && lst.First().InduceRate > 0 ? ((lst.First().InduceRate / 100) * lst.First().Profit) : lst.First().DistribursAmount ?? 0);
                    model.DistributionYear = lst.First().YearStartDate.ToString("MMM-yyyy", CultureInfo.InvariantCulture) + " to " + lst.First().YearEndDate.ToString("MMM-yyyy", CultureInfo.InvariantCulture);
                    model.YearStartDateStr = lst.First().YearStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    model.YearEndDateStr = lst.First().YearEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    model.TransactionDate = DateTime.Now.Date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    model.ProfitRate = Math.Round(lst.First().InduceRate, 2).ToString();
                }
            }
            else
                model.Message = "Profit declaration not found";

            return View(model);
        }
        [HttpGet]
        public JsonResult GetEmployeeWisePFDistribution(int declarationId)
        {
            try
            {
                var dec_obj = profitDeclarationService.GetById(declarationId);
                var lst_contributionRegister = new List<ProfitDistributionViewModel>();
                using (gHRMDBContext db = new gHRMDBContext())
                {
                    if (dec_obj.CalculationWithProfit)
                    {
                        lst_contributionRegister = (from c in db.ContributionRegisters
                                                    join e in db.Employees on c.EmployeeId equals e.EmployeeId
                                                    where !(c.IsDeleted ?? false) && c.TransactionDate.Value <= dec_obj.YearEndDate
                                                    && (c.TransactionType == PFTransactionTypeConstants.Contribution || c.TransactionType == PFTransactionTypeConstants.Profit)
                                                    group new { c, e } by new { e.EmployeeId, e.EmployeeCode, e.EmployeeName } into g
                                                    select new ProfitDistributionViewModel
                                                    {
                                                        EmployeeId = g.Key.EmployeeId,
                                                        EmployeeCode = g.Key.EmployeeCode,
                                                        EmployeeName = g.Key.EmployeeName,
                                                        OrgContribution = g.Sum(p => p.c.OrgContribution),
                                                        SelfContribution = g.Sum(p => p.c.SelfContribution)
                                                    }).OrderBy(x => x.EmployeeCode).ToList();
                    }
                    else
                    {
                        lst_contributionRegister = (from c in db.ContributionRegisters
                                                    join e in db.Employees on c.EmployeeId equals e.EmployeeId
                                                    where !(c.IsDeleted ?? false) && c.TransactionDate.Value <= dec_obj.YearEndDate
                                                    && c.TransactionType == PFTransactionTypeConstants.Contribution
                                                    group new { c, e } by new { e.EmployeeId, e.EmployeeCode, e.EmployeeName } into g
                                                    select new ProfitDistributionViewModel
                                                    {
                                                        EmployeeId = g.Key.EmployeeId,
                                                        EmployeeCode = g.Key.EmployeeCode,
                                                        EmployeeName = g.Key.EmployeeName,
                                                        OrgContribution = g.Sum(p => p.c.OrgContribution),
                                                        SelfContribution = g.Sum(p => p.c.SelfContribution)
                                                    }).OrderBy(x => x.EmployeeCode).ToList();
                    }
                }


                if (lst_contributionRegister.Any())
                {
                    decimal disburseAmt = dec_obj.DistribursAmount == 0 && dec_obj.InduceRate > 0 ? ((dec_obj.InduceRate / 100) * dec_obj.Profit) : dec_obj.DistribursAmount ?? 0;

                    decimal contribution_amt = (disburseAmt / lst_contributionRegister.Sum(x => x.OrgContribution + x.SelfContribution));
                    lst_contributionRegister.ForEach(x => { x.TotalContribution = x.SelfContribution + x.OrgContribution; x.ProfitContribution = decimal.Round(((x.SelfContribution + x.OrgContribution) * contribution_amt), 6, MidpointRounding.AwayFromZero); });
                    return Json(new { obj = lst_contributionRegister, msg = "" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { msg = "PF data not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult PostEmployeeDistributionData(List<ProfitDistributionViewModel> model, string transactionDate, int? declarationId)
        {
            string msg = ""; int status = 0;
            if (model == null) msg = "data is not correct format";
            else if ((declarationId ?? 0) <= 0) msg = "Declaration data is not found";
            else if (string.IsNullOrEmpty(transactionDate)) msg = "Transaction date is not found";
            else if (!model.Any()) msg = "data is not found";
            else if (model.Where(x => x.EmployeeId == 0).Any()) msg = "Employee is not found";
            else
            {
                var dec_obj = profitDeclarationService.GetById(declarationId??0);
                if(dec_obj.DeclarationStatus!= ProfitDeclarationConstants.Approved) msg = "Declaration data is not found";
                else
                {
                    try
                    {
                        DateTime tran_dt = DateTime.Now;
                        DateTime.TryParse(transactionDate, out tran_dt);
                        if (DateTime.MinValue.Equals(tran_dt)) msg = "Transaction date is not correct format";
                        else
                        {
                            List<ContributionRegister> lst = new List<ContributionRegister>();

                            foreach (var p in model)
                            {
                                decimal _con = decimal.Round(p.ProfitContribution / 2, 6, MidpointRounding.AwayFromZero);
                                var obj = new ContributionRegister()
                                {
                                    Comments = (dec_obj.YearStartDate.ToString("MMM-yyyy", CultureInfo.InvariantCulture) + " to "+ dec_obj.YearEndDate.ToString("MMM-yyyy", CultureInfo.InvariantCulture))+" Profit contribution",
                                    CreateDate = DateTime.Now,
                                    CreateUser = LoggedInEmployeeId ?? 0,
                                    EmployeeId = p.EmployeeId,
                                    IsDeleted = false,
                                    SelfContribution = _con,
                                    OrgContribution = p.ProfitContribution - _con,
                                    TransactionDate = tran_dt,
                                    TransactionType= PFTransactionTypeConstants.Profit,
                                };
                                lst.Add(obj);
                            }
                            using(gHRMDBContext db=new gHRMDBContext())
                            {
                                db.ContributionRegisters.AddRange(lst);
                                db.SaveChanges();
                            }
                            dec_obj.DeclarationStatus = ProfitDeclarationConstants.Close;
                            dec_obj.UpdateDate = DateTime.UtcNow;
                            dec_obj.UpdateUser = LoggedInEmployeeId;
                            profitDeclarationService.Update(dec_obj);
                            status = 1;
                            msg = "Successfully Distribution complete";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                    
                   
                }
            }
            return Json(new { Message = msg, Status = status });
        }
        public JsonResult ProcessProfitDistribution(string distributionYear, string transDate)
        {
            bool isProcessed = false;

            var model = new ProfitDistributionProcessViewModel();
            try
            {
                DateTime yearStarDate = Convert.ToDateTime(distributionYear.Split(new string[] { "to" }, StringSplitOptions.None)[0].Trim());
                DateTime yearEndDate = Convert.ToDateTime(distributionYear.Split(new string[] { "to" }, StringSplitOptions.None)[1].Trim());

                //Checking Day Ending Status from [gcpf.ProcessLog]
                var processLog = processLogService.GetLastProcessLog();

                if (processLog == null)
                    return Json(new { message = "Please Check Process Log" }, JsonRequestBehavior.AllowGet);

                if (!processLog.IsOpen)
                    return Json(new { message = "Day Closed, please Open day and then start Profit distribution " }, JsonRequestBehavior.AllowGet);

                //Checking Profit DIstribution Status
                string message = string.Empty;

                bool isValid = profitDistProcessLogService.IsValidYearForprofitDist(yearStarDate, yearEndDate, out message);
                if (!isValid)
                    return Json(new { message = message }, JsonRequestBehavior.AllowGet);

                model.TransCategoryId = transactionCategoryId;
                model.TransactionDate = processLog.StartDate.ToString();
                model.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                model.CreateDate = DateTime.Now;

                DistributionProfit(model);

                var objProDisProcessLog = profitDistProcessLogService.GetMany(x => x.YearStartDate.Date == yearStarDate.Date && x.YearEndDate.Date == yearEndDate.Date && x.IsDeleted == false).OrderByDescending(x => x.YearStartDate).FirstOrDefault();
                if (objProDisProcessLog != null)
                    isProcessed = objProDisProcessLog.IsProcessed;
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later", status = "nok", IsProcessed = isProcessed }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Processed Successfully", status = "ok", IsProcessed = isProcessed }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Methods

        private void DistributionProfit(ProfitDistributionProcessViewModel model)
        {
            var param = new
            {
                TransCategoryId = model.TransCategoryId,
                TransactionDate = Convert.ToDateTime(model.TransactionDate),
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate
            };
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_YS_ProcessProfitDistribution");
        }

        #endregion
    }
}
