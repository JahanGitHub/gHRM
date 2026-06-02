using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Service.Loan;
using gHRM.Service.Payroll;
using gHRM.Web.ViewModels.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Loan
{
    public class LoanConfig2Controller : BaseController
    {
        #region Private Variables
        private readonly ICollectionMethodService collectionMethodService;
        private readonly ILoanPurposeService loanPurposeService;
        private readonly ILoanEligibilityService loanEligibilityService;
        private readonly IApprovalMasterService approvalMasterService;
        private readonly IApproveDetailService approveDetailService;
        private readonly IPRComponentService prComponentService;
        #endregion Private Variables
        #region Ctor
        public LoanConfig2Controller
            (ICollectionMethodService collectionMethodService, ILoanPurposeService loanPurposeService, ILoanEligibilityService loanEligibilityService
            , IApprovalMasterService approvalMasterService, IApproveDetailService approveDetailService, IPRComponentService prComponentService)
        {
            this.collectionMethodService = collectionMethodService;
            this.loanPurposeService = loanPurposeService;
            this.loanEligibilityService = loanEligibilityService;
            this.approvalMasterService = approvalMasterService;
            this.approveDetailService = approveDetailService;
            this.prComponentService = prComponentService;
        }
        #endregion Ctor
        #region Action Methods
        [HttpGet]
        public ActionResult CollectionMethod()
        {
            CollectionMethodViewModel viewmodel = new CollectionMethodViewModel();
            viewmodel.CollectionFormatLst = new LoanConfigCommonDropdown().CollectionFormat("");
            viewmodel.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
            viewmodel.MethodTypeLst = new LoanConfigCommonDropdown().MethodType("");
            return View(viewmodel);
        }


        [HttpPost]
        public JsonResult PostCollectionMethod(CollectionMethodViewModel model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else
            {
                try
                {
                    if (model.CollectionFormat == "PI")
                    {
                        model.Principal = 100;
                        model.Interest = 0;
                    }
                    if (model.Principal + model.Interest != 100)
                        msg = "Percentage need to 100";
                    else if (model.Id > 0)
                    {
                        var obj = collectionMethodService.GetById(model.Id);
                        obj.CollectionFormat = model.CollectionFormat;
                        obj.MethodType = model.MethodType;
                        obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                        obj.Principal = model.Principal;
                        obj.Interest = model.Interest;
                        collectionMethodService.Update(obj);
                        result = 1;
                        msg = "Update Successfully";
                    }
                    else
                    {
                        var isExist = collectionMethodService.GetMany(x => x.LoanType == model.LoanType);
                        if (!isExist.Any())
                        {
                            var obj = Mapper.Map<CollectionMethodViewModel, CollectionMethod>(model);
                            obj.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                            collectionMethodService.Create(obj);
                            result = 1;
                            msg = "Save Successfully";
                        }
                        else msg = "Loan Type is already exist";
                    }
                }
                catch (System.Exception ex)
                {
                    msg = ex.Message;
                }
            }

            return Json(new { Result = result, Message = msg });
        }

        [HttpGet]
        public ActionResult LoanPurpose()
        {
            LoanPurposeViewModel viewmodel = new LoanPurposeViewModel();
            viewmodel.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
            viewmodel.MethodTypeLst = new LoanConfigCommonDropdown().MethodType("");
            viewmodel.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult LoanPurpose2()
        {
            LoanPurposeViewModel viewmodel = new LoanPurposeViewModel();
            viewmodel.LoanTypeLst = new LoanConfigCommonDropdown().LoanType2("");
            viewmodel.MethodTypeLst = new LoanConfigCommonDropdown().MethodType("");
            viewmodel.GracePeriodLst = new LoanConfigCommonDropdown().GracePeriod(null);
            return View(viewmodel);
        }

        [HttpPost]
        public JsonResult PostLoanPurpose(LoanPurpose model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else if (string.IsNullOrEmpty(model.PurposeName))
                msg = "Purpose Name is required";
            else if (loanPurposeService.GetMany(x => x.PurposeName == model.PurposeName && x.PurposeId != model.PurposeId && x.IsActive == true).Any())
                msg = "Duplicate Purpose Name is not allow";
            else if (loanPurposeService.GetMany(x => x.LoanType == "PFL" && x.IsActive == true).Any() && model.PurposeId == 0 && model.LoanType == "PFL")
                msg = "Multiple PF head is not allow";
            else
            {
                try
                {
                    if (model.PurposeId > 0)
                    {
                        var obj = loanPurposeService.GetById(model.PurposeId);
                        if (!prComponentService.GetMany(x => x.ComponentName == obj.PurposeName).Any())
                            obj.PurposeName = model.PurposeName;
                        obj.MethodType = model.MethodType;
                        obj.GracePeriod = model.GracePeriod;
                        obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                        obj.UpdateDate = DateTime.Now;
                        loanPurposeService.Update(obj);
                        result = 1;
                        msg = "Update Successfully";
                    }
                    else
                    {
                        model.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                        model.CreateDate = DateTime.Now;
                        loanPurposeService.Create(model);
                        result = 1;
                        msg = "Save Successfully";
                    }
                }
                catch (System.Exception ex)
                {
                    msg = ex.Message;
                }
            }
            return Json(new { Result = result, Message = msg });
        }
        [HttpGet]
        public ActionResult LoanEligibility()
        {
            LoanEligibilityViewModel viewmodel = new LoanEligibilityViewModel();
            viewmodel.LoanTypeLst = new LoanConfigCommonDropdown().LoanType("");
            viewmodel.PFContributionLst = new LoanConfigCommonDropdown().PFContribution("");
            viewmodel.PurposeLst = new LoanConfigCommonDropdown().TypeXPropose("", 0);
            return View(viewmodel);
        }

        [HttpPost]
        public JsonResult PostLoanEligibility(LoanEligibilityViewModel model)
        {
            int result = 0;
            string msg = "";
            if (model == null)
                msg = "Data not found";
            else if (loanEligibilityService.GetMany(x => x.PurposeId == model.PurposeId && x.IsActive == true
             && ((model.MinmumJobAge >= x.MinmumJobAge && model.MinmumJobAge <= x.MaximumJobAge)
             || (model.MaximumJobAge >= x.MinmumJobAge && model.MaximumJobAge <= x.MaximumJobAge)) && x.Id != model.Id).Any())
                msg = "Minimum & maximum job age check";
            else
            {
                try
                {
                    if (model.LoanType == "CL" || model.LoanType == "COL")
                        model.PFContribution = "";
                    if (model.Id > 0)
                    {
                        var obj = loanEligibilityService.GetById(model.Id);
                        obj.PurposeId = model.PurposeId;
                        obj.LoanType = model.LoanType;
                        obj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                        obj.MinmumJobAge = model.MinmumJobAge;
                        obj.MaximumJobAge = model.MaximumJobAge;
                        obj.LoanEligibleInPercent = model.LoanEligibleInPercent;
                        loanEligibilityService.Update(obj);
                        result = 1;
                        msg = "Update Successfully";
                    }
                    else
                    {
                        var obj = Mapper.Map<LoanEligibilityViewModel, LoanEligibility>(model);
                        obj.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                        loanEligibilityService.Create(obj);
                        result = 1;
                        msg = "Save Successfully";
                    }
                }
                catch (System.Exception ex)
                {
                    msg = ex.Message;
                }
            }

            return Json(new { Result = result, Message = msg });
        }

        [HttpGet]
        public ActionResult ApprovalLevel()
        {

            return View();
        }
        [HttpGet]
        public ActionResult ApprovalLevelCreate(int? id)
        {
            LoanApprovalViewModel model = new LoanApprovalViewModel();
            if (id.HasValue)
            {
                var obj = approvalMasterService.GetById(id.Value);
                model.TotalLevel = obj.TotalLevel;
                model.ApprovalMasterId = obj.ApprovalMasterId;
                model.LoanType = obj.LoanType;
                model.FormName = obj.FormName;
            }
            model.FormNameLst = new LoanConfigCommonDropdown().FormName(model.FormName);
            model.LoanTypeLst = new LoanConfigCommonDropdown().LoanType(model.LoanType);

            return View(model);
        }
        [HttpPost]
        public JsonResult PostApprovalLevelCreate(ApprovalMaster master, List<ApproveDetail> details)
        {
            int result = 0; string msg = "";
            if (master == null || details == null)
                msg = "Data not found";
            else if (!details.Any())
                msg = "Data not found";
            else if (details.GroupBy(x => x.EmployeeId).Any(g => g.Count() > 1))
                msg = "Duplicate data is not allow";
            else
            {
                try
                {
                    if (master.ApprovalMasterId > 0)
                    {
                        var masterobj = approvalMasterService.GetById(master.ApprovalMasterId);
                        masterobj.TotalLevel = master.TotalLevel;
                        masterobj.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                        masterobj.CreateDate = DateTime.Now;
                        approvalMasterService.Update(masterobj);

                        var detailobj = approveDetailService.GetMany(x => x.IsActive && x.ApprovalMasterId == master.ApprovalMasterId);
                        foreach (var d in detailobj)
                        {
                            var detail = approveDetailService.GetById(d.ApprovalDetailId);
                            detail.IsActive = false;
                            detail.UpdateBy = (int)(LoggedInEmployeeId ?? 0);
                            detail.UpdateDate = DateTime.Now;
                            approveDetailService.Update(detail);
                        }

                        foreach (var d in details)
                        {
                            if (d.ApprovalDetailId == 0)
                            {
                                d.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                                d.CreateDate = DateTime.Now;
                                d.ApprovalMasterId = master.ApprovalMasterId;
                                approveDetailService.Create(d);
                            }
                            else if (detailobj.Where(x => x.ApprovalDetailId == d.ApprovalDetailId).Any())
                            {
                                var detail = approveDetailService.GetById(d.ApprovalDetailId);
                                detail.IsActive = true;
                                detail.PriorityLevel = d.PriorityLevel;
                                detail.ConditionalAmount = d.ConditionalAmount;
                                detail.ConditionType = d.ConditionType;
                                approveDetailService.Update(detail);
                            }
                        }
                        result = 1;
                        msg = "Update Successfully";
                    }
                    else
                    {
                        if (approvalMasterService.GetMany(x => x.IsActive == true && x.LoanType == master.LoanType && x.FormName == master.FormName).Any())
                            msg = "Multi loan type and form name is not allow";
                        else
                        {
                            master.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                            master.CreateDate = DateTime.Now;
                            approvalMasterService.Create(master);
                            foreach (var d in details)
                            {
                                d.CreateBy = (int)(LoggedInEmployeeId ?? 0);
                                d.CreateDate = DateTime.Now;
                                d.ApprovalMasterId = master.ApprovalMasterId;
                                approveDetailService.Create(d);
                            }
                            result = 1;
                            msg = "Save Successfully";
                        }

                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }

            return Json(new { Result = result, Message = msg });
        }

        [HttpGet]
        public JsonResult GetApprovalDetail(int masterid)
        {
            gHRMDBContext db = new gHRMDBContext();
            var objLst = (from d in db.ApproveDetails
                          join e in db.Employees on d.EmployeeId equals e.EmployeeId
                          where d.IsActive && d.ApprovalMasterId == masterid
                          select new LoanApprovalViewModel
                          {
                              ApprovalDetailId = d.ApprovalDetailId,
                              ApprovalMasterId = d.ApprovalMasterId,
                              ConditionalAmount = d.ConditionalAmount,
                              ConditionType = d.ConditionType,
                              EmployeeId = d.EmployeeId,
                              EmployeeName = "(" + e.EmployeeCode + ") " + e.EmployeeName,
                              PriorityLevel = d.PriorityLevel
                          }).ToList();

            return Json(objLst, JsonRequestBehavior.AllowGet);
        }

        #endregion Action Methods

        #region    Method
        public JsonResult GetProposeXLoanType(string loanType, int id)
        {
            var lst = new LoanConfigCommonDropdown().TypeXPropose(loanType, id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion Method

        #region Grid
        public JsonResult GetAllCollectionMethod()
        {
            var lst = collectionMethodService.GetMany(x => x.IsActive == true);
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLoanPurpose()
        {
            var lst = loanPurposeService.GetMany(x => x.IsActive == true);
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLoanEligibility()
        {
            gHRMDBContext db = new gHRMDBContext();
            var lst = (from le in db.LoanEligibility
                       join lp in db.LoanPurposes on le.PurposeId equals lp.PurposeId into gj
                       from x in gj.DefaultIfEmpty()
                       select new
                       {
                           Id = le.Id,
                           LoanType = le.LoanType,
                           MaximumJobAge = le.MaximumJobAge,
                           MinmumJobAge = le.MinmumJobAge,
                           PFContribution = le.PFContribution,
                           LoanEligibleInPercent = le.LoanEligibleInPercent,
                           PurposeId = le.PurposeId,
                           PurposeName = x.PurposeName
                           //GracePeriod=x.GracePeriod
                       });

            //loanEligibilityService.GetMany(x => x.IsActive == true);
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLoanApproval()
        {
            var lst = approvalMasterService.GetMany(x => x.IsActive);
            return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() });
        }
        #endregion
    }
}