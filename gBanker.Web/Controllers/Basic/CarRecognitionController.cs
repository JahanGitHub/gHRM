using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using AutoMapper;
using Elmah;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.DropDownService;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Web.ViewModels.Basic;

namespace gHRM.Web.Controllers
{
    public class CarRecognitionController : BaseController
    {
        #region Varibles
        private readonly ICarRecognitionService carRecognitionService;
        private readonly ICarRecognitionApprovalService carRecognitionApprovalService;
        private readonly ICarRecognitionApprovedHistoryService carRecognitionApprovedHistoryService;
        private readonly IApprovalNotificationService approvalNotificationService;
        private readonly IEmployeeSPService employeeSPService;
        public CarRecognitionController(
            ICarRecognitionService carRecognitionService,
            ICarRecognitionApprovalService carRecognitionApprovalService,
            ICarRecognitionApprovedHistoryService carRecognitionApprovedHistoryService,
            IApprovalNotificationService approvalNotificationService,
            IEmployeeSPService employeeSPService
        )
        {
            this.carRecognitionService = carRecognitionService;
            this.carRecognitionApprovalService = carRecognitionApprovalService;
            this.carRecognitionApprovedHistoryService = carRecognitionApprovedHistoryService;
            this.approvalNotificationService = approvalNotificationService;
            this.employeeSPService = employeeSPService;
        }

        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Approval()
        {
            var model = new CarRecognitionViewModel();
            model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            return View(model);
        }
        #endregion

        #region Methods
        public JsonResult SaveCarRecognition(CarRecognition carRecognition)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    carRecognitionService.GetAll()
                        .Where(
                            p =>
                                p.IsActive == true &&
                                p.Purpose.ToUpper().Trim() == carRecognition.Purpose.ToUpper().Trim())
                        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Organization Name found, Save denied";
                }
                else
                {
                    var entity = new CarRecognition();
                    entity.EmployeeId = carRecognition.EmployeeId;
                    entity.CarNo = carRecognition.CarNo;
                    entity.CarRecognitionDate = carRecognition.CarRecognitionDate;
                    entity.CarRecognitionTimeFrom = carRecognition.CarRecognitionTimeFrom;
                    entity.CarRecognitionTimeTo = carRecognition.CarRecognitionTimeTo;
                    entity.Distance = carRecognition.Distance;
                    entity.Purpose = carRecognition.Purpose;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    carRecognitionService.Create(entity);
                    var CarRecognitionId = entity.CarRecognitionId;
                    result = "Save Successfull";

                    var ApproverId = carRecognitionApprovalService.GetMany(w => w.ApprovalLevel == 1 && w.IsActive == true)
                        .Select(w => w.EmployeeId).SingleOrDefault();

                    var newApprovalNotification = new ApprovalNotification();
                    newApprovalNotification.ApplicationId = CarRecognitionId;
                    newApprovalNotification.ApprovalDetailId = 0;
                    newApprovalNotification.ApprovalMasterId = 0;
                    newApprovalNotification.ApproverId = Convert.ToInt64(ApproverId);
                    newApprovalNotification.IsActive = true;
                    newApprovalNotification.IsChecked = false;
                    newApprovalNotification.ModuleName = "CR";
                    newApprovalNotification.CheckedStatus = "N";
                    newApprovalNotification.CreateDate = DateTime.Now;
                    newApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                    approvalNotificationService.Create(newApprovalNotification);
                    var NotificationId = newApprovalNotification.NotificationId;


                    var allApproverid = carRecognitionApprovalService.GetMany(w => w.IsActive == true).Select(w => w.EmployeeId);
                    foreach (var singleApproverid in allApproverid)
                    {
                        var entitylist = new CarRecognitionApprovedHistory();
                        entitylist.CarRecognitionId = CarRecognitionId;
                        entitylist.ApprovalId = singleApproverid;
                        entitylist.EmployeeId = carRecognition.EmployeeId;
                        entitylist.NotificationId = Convert.ToInt32(NotificationId);
                        entitylist.CheckedStatus = "N";
                        entitylist.IsActive = true;
                        entitylist.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entitylist.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entitylist.CreateDate = DateTime.UtcNow;
                        entitylist.UpdateDate = DateTime.UtcNow;
                        carRecognitionApprovedHistoryService.Create(entitylist);
                    }

                    //var entitylist = new CarRecognitionApprovedHistory();
                    //entitylist.CarRecognitionId = CarRecognitionId;
                    //entitylist.ApprovalId = ApproverId;
                    //entitylist.EmployeeId = carRecognition.EmployeeId;
                    //entitylist.NotificationId = Convert.ToInt32(NotificationId);
                    //entitylist.CheckedStatus = "N";
                    //entitylist.IsActive = true;
                    //entitylist.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    //entitylist.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    //entitylist.CreateDate = DateTime.UtcNow;
                    //entitylist.UpdateDate = DateTime.UtcNow;
                    //carRecognitionApprovedHistoryService.Create(entitylist);
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateCarRecognition(CarRecognition carRecognition)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   carRecognitionService.GetAll()
                       .Where(
                           p =>
                               p.IsActive == true && p.CarRecognitionId != carRecognition.CarRecognitionId &&
                               p.Purpose.ToUpper().Trim() == carRecognition.Purpose.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Organization Name found, Update denied";
                }
                else
                {
                    var entity = carRecognitionService.GetById(carRecognition.CarRecognitionId);
                    entity.CarRecognitionId = carRecognition.CarRecognitionId;
                    entity.EmployeeId = carRecognition.EmployeeId;
                    entity.CarNo = carRecognition.CarNo;
                    entity.CarRecognitionDate = carRecognition.CarRecognitionDate;
                    entity.CarRecognitionTimeFrom = carRecognition.CarRecognitionTimeFrom;
                    entity.CarRecognitionTimeTo = carRecognition.CarRecognitionTimeTo;
                    entity.Distance = carRecognition.Distance;
                    entity.Purpose = carRecognition.Purpose;
                    entity.IsActive = true;
                    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    carRecognitionService.Update(entity);
                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListCarRecognition([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request)
        {
            try
            {
                var carRecognitionList = employeeSPService.GetDataWithoutParameter("dbo.GetCarRecognitionList");
                var approvalShowViewModel = carRecognitionList.Tables[0].AsEnumerable()
                    .Select((row, index) => new CarRecognitionViewModel
                    {
                        RowSl = index + 1,
                        CarRecognitionId = row.Field<int>("CarRecognitionId"),
                        EmployeeId = row.Field<int>("EmployeeId"),
                        CarNo = row.Field<string>("CarNo"),
                        CarRecognitionDate = row.Field<DateTime>("CarRecognitionDate"),
                        CarRecognitionTimeFrom = row.Field<DateTime>("CarRecognitionTimeFrom"),
                        CarRecognitionTimeTo = row.Field<DateTime>("CarRecognitionTimeTo"),
                        Distance = row.Field<decimal>("Distance"),
                        Purpose = row.Field<string>("Purpose"),
                        CRD = row.Field<string>("CRD"),
                        CRTF = row.Field<string>("CRTF"),
                        CRTT = row.Field<string>("CRTT"),
                        ApprovedCarNo = row.Field<string>("ApprovedCarNo"),
                        IsActive = row.Field<bool>("IsActive"),
                        NotificationId = row.Field<long?>("NotificationId"),
                        CheckedStatus = row.Field<string>("CheckedStatus")
                    }).ToList();

                DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }


        //public JsonResult ListCarRecognitionApprover([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string qType, int EmployeeId)
        //{
        //    try
        //    {
        //        var prm = new
        //        {
        //            qType = qType,
        //            EmployeeId = EmployeeId
        //        };
        //        var DetailsInfo = employeeSPService.GetDataWithParameter(prm, "dbo.GetCarRecognitionListbyApprover");
        //        if (DetailsInfo.Tables[0].Rows.Count > 0)
        //        {
        //            if (qType == "N")
        //            {
        //                var carRecognitionList = employeeSPService.GetDataWithoutParameter("dbo.GetCarRecognitionList");
        //                var approvalShowViewModel = carRecognitionList.Tables[0].AsEnumerable()
        //                    .Select((row, index) => new CarRecognitionViewModel
        //                    {
        //                        RowSl = index + 1,
        //                        CarRecognitionId = row.Field<int>("CarRecognitionId"),
        //                        EmployeeId = row.Field<int>("EmployeeId"),
        //                        CarNo = row.Field<string>("CarNo"),
        //                        CarRecognitionDate = row.Field<DateTime>("CarRecognitionDate"),
        //                        CarRecognitionTimeFrom = row.Field<DateTime>("CarRecognitionTimeFrom"),
        //                        CarRecognitionTimeTo = row.Field<DateTime>("CarRecognitionTimeTo"),
        //                        Distance = row.Field<decimal>("Distance"),
        //                        Purpose = row.Field<string>("Purpose"),
        //                        CRD = row.Field<string>("CRD"),
        //                        CRTF = row.Field<string>("CRTF"),
        //                        CRTT = row.Field<string>("CRTT"),
        //                        ApprovedCarNo = row.Field<string>("ApprovedCarNo"),
        //                        IsActive = row.Field<bool>("IsActive"),
        //                        NotificationId = row.Field<long?>("NotificationId"),
        //                        CheckedStatus = row.Field<string>("CheckedStatus")
        //                    }).ToList();

        //                DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
        //                return Json(new
        //                {
        //                    data = result.Data,
        //                    total = result.Total
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //            else if (qType == "A")
        //            {
        //            }
        //            else if (qType == "R")
        //            {

        //            }
        //        }
        //        else
        //        {
        //            if (qType == "N")
        //            {
        //                var carRecognitionList = employeeSPService.GetDataWithoutParameter("dbo.GetCarRecognitionList");
        //                var approvalShowViewModel = carRecognitionList.Tables[0].AsEnumerable()
        //                    .Select((row, index) => new CarRecognitionViewModel
        //                    {
        //                        RowSl = index + 1,
        //                        CarRecognitionId = row.Field<int>("CarRecognitionId"),
        //                        EmployeeId = row.Field<int>("EmployeeId"),
        //                        CarNo = row.Field<string>("CarNo"),
        //                        CarRecognitionDate = row.Field<DateTime>("CarRecognitionDate"),
        //                        CarRecognitionTimeFrom = row.Field<DateTime>("CarRecognitionTimeFrom"),
        //                        CarRecognitionTimeTo = row.Field<DateTime>("CarRecognitionTimeTo"),
        //                        Distance = row.Field<decimal>("Distance"),
        //                        Purpose = row.Field<string>("Purpose"),
        //                        CRD = row.Field<string>("CRD"),
        //                        CRTF = row.Field<string>("CRTF"),
        //                        CRTT = row.Field<string>("CRTT"),
        //                        ApprovedCarNo = row.Field<string>("ApprovedCarNo"),
        //                        IsActive = row.Field<bool>("IsActive"),
        //                        NotificationId = row.Field<long?>("NotificationId"),
        //                        CheckedStatus = row.Field<string>("CheckedStatus")
        //                    }).ToList();

        //                DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
        //                return Json(new
        //                {
        //                    data = result.Data,
        //                    total = result.Total
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //            else if (qType == "A")
        //            {

        //            }
        //            else if (qType == "R")
        //            {
        //                //var param = new { qType = qType };
        //                //var carRecognitionList = employeeSPService.GetDataWithParameter(param, "dbo.GetCarRecognitionList");
        //                //var approvalShowViewModel = carRecognitionList.Tables[0].AsEnumerable()
        //                //    .Select((row, index) => new CarRecognitionViewModel
        //                //    {
        //                //        RowSl = index + 1,
        //                //        CarRecognitionId = row.Field<int>("CarRecognitionId"),
        //                //        EmployeeId = row.Field<int>("EmployeeId"),
        //                //        CarNo = row.Field<string>("CarNo"),
        //                //        CarRecognitionDate = row.Field<DateTime>("CarRecognitionDate"),
        //                //        CarRecognitionTimeFrom = row.Field<DateTime>("CarRecognitionTimeFrom"),
        //                //        CarRecognitionTimeTo = row.Field<DateTime>("CarRecognitionTimeTo"),
        //                //        Distance = row.Field<decimal>("Distance"),
        //                //        Purpose = row.Field<string>("Purpose"),
        //                //        CRD = row.Field<string>("CRD"),
        //                //        CRTF = row.Field<string>("CRTF"),
        //                //        CRTT = row.Field<string>("CRTT"),
        //                //        ApprovedCarNo = row.Field<string>("ApprovedCarNo"),
        //                //        IsActive = row.Field<bool>("IsActive"),
        //                //        NotificationId = row.Field<long?>("NotificationId"),
        //                //        CheckedStatus = row.Field<string>("CheckedStatus")
        //                //    }).ToList();

        //                //DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
        //                //return Json(new
        //                //{
        //                //    data = result.Data,
        //                //    total = result.Total
        //                //}, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        return Json(new { result = "ERR" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "ERROR", Message = ex.Message });
        //    }

        //}



        public JsonResult ListCarRecognitionApprover([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string qType, int EmployeeId)
        {
            try
            {
                var prm = new
                {
                    qType = qType,
                    EmployeeId = EmployeeId
                };
                var carRecognitionList = employeeSPService.GetDataWithParameter(prm,"dbo.GetCarRecognitionListbyApprover");
                if (carRecognitionList.Tables[0].Rows.Count > 0)
                {
                    var approvalShowViewModel = carRecognitionList.Tables[0].AsEnumerable()
                    .Select((row, index) => new CarRecognitionViewModel
                    {
                        RowSl = index + 1,
                        CarRecognitionId = row.Field<int>("CarRecognitionId"),
                        EmployeeId = row.Field<int>("EmployeeId"),
                        CarNo = row.Field<string>("CarNo"),
                        CarRecognitionDate = row.Field<DateTime>("CarRecognitionDate"),
                        CarRecognitionTimeFrom = row.Field<DateTime>("CarRecognitionTimeFrom"),
                        CarRecognitionTimeTo = row.Field<DateTime>("CarRecognitionTimeTo"),
                        Distance = row.Field<decimal>("Distance"),
                        Purpose = row.Field<string>("Purpose"),
                        CRD = row.Field<string>("CRD"),
                        CRTF = row.Field<string>("CRTF"),
                        CRTT = row.Field<string>("CRTT"),
                        ApprovedCarNo = row.Field<string>("ApprovedCarNo"),
                        IsActive = row.Field<bool>("IsActive"),
                        NotificationId = row.Field<long>("NotificationId"),
                        CheckedStatus = row.Field<string>("CheckedStatus")
                    }).ToList();

                    DataSourceResult result = approvalShowViewModel.ToDataSourceResult(request);
                    return Json(new
                    {
                        data = result.Data,
                        total = result.Total
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = "ERROR", Message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult InformationDeleteCarRecognition(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = carRecognitionService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                carRecognitionService.Update(model);
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

        public JsonResult CarRecognitionConfirm(int ApprovalId, int EmployeeId, int NotificationId)
        {
            var ApproverID = carRecognitionApprovalService.GetMany(w => w.EmployeeId == ApprovalId).Select(w => w.ApprovalId).SingleOrDefault();
            var ApproverLevel = carRecognitionApprovalService.GetMany(w => w.EmployeeId == ApprovalId).Select(w => w.ApprovalLevel).SingleOrDefault();
            var CarRecognitionId = carRecognitionService.GetMany(w => w.EmployeeId == EmployeeId).Select(w => w.CarRecognitionId).SingleOrDefault();

            //var any = carRecognitionApprovedHistoryService.GetMany(w=>w.ApprovalId== ApprovalId && w.NotificationId == NotificationId).Any();
            //if (any==true)
            //{
            //    var CarRecognitionApprovedHistoryId = carRecognitionApprovedHistoryService.GetMany(w=>w.NotificationId== NotificationId).Select(w=>w.CarRecognitionApprovedHistoryId).FirstOrDefault();
            //    var entity = carRecognitionApprovedHistoryService.GetById(Convert.ToInt32(CarRecognitionApprovedHistoryId));
            //    entity.CheckedStatus = "A";
            //    entity.IsApproved = true;
            //    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            //    entity.UpdateDate = DateTime.UtcNow;
            //    carRecognitionApprovedHistoryService.Update(entity);
            //}
            //else
            //{
            //    var entity = new CarRecognitionApprovedHistory();
            //    entity.NotificationId = NotificationId;
            //    entity.CarRecognitionId = CarRecognitionId;
            //    entity.ApprovalId = ApprovalId;
            //    entity.EmployeeId = EmployeeId;
            //    entity.CheckedStatus = "A";
            //    entity.IsApproved = true;
            //    entity.IsActive = true;
            //    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            //    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            //    entity.CreateDate = DateTime.UtcNow;
            //    entity.UpdateDate = DateTime.UtcNow;
            //    carRecognitionApprovedHistoryService.Create(entity);
            //}

            var CarRecognitionApprovedHistoryId = carRecognitionApprovedHistoryService.GetMany(w => w.ApprovalId == ApprovalId && w.NotificationId == NotificationId).Select(w => w.CarRecognitionApprovedHistoryId).FirstOrDefault();
            var entity = carRecognitionApprovedHistoryService.GetById(Convert.ToInt32(CarRecognitionApprovedHistoryId));
            entity.CheckedStatus = "A";
            entity.IsApproved = true;
            entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            entity.UpdateDate = DateTime.UtcNow;
            carRecognitionApprovedHistoryService.Update(entity);

            var NextApproverId = carRecognitionApprovalService.GetMany(w => w.ApprovalLevel > ApproverLevel && w.IsActive == true)
                    .Select(w => w.EmployeeId).FirstOrDefault() ?? 0;

            var NextApprovalNotification = approvalNotificationService.GetById(Convert.ToInt32(NotificationId));
            NextApprovalNotification.ApproverId = Convert.ToInt64(NextApproverId);
            NextApprovalNotification.CheckedStatus = "A";
            NextApprovalNotification.UpdateDate = DateTime.Now;
            NextApprovalNotification.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            approvalNotificationService.Update(NextApprovalNotification);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveReject(int ApprovalId, int EmployeeId, int NotificationId)
        {
            var ApproverID = carRecognitionApprovalService.GetMany(w => w.EmployeeId == ApprovalId).Select(w => w.ApprovalId).SingleOrDefault();
            var ApproverLevel = carRecognitionApprovalService.GetMany(w => w.EmployeeId == ApprovalId).Select(w => w.ApprovalLevel).SingleOrDefault();
            var CarRecognitionId = carRecognitionService.GetMany(w => w.EmployeeId == EmployeeId).Select(w => w.CarRecognitionId).SingleOrDefault();

            var CarRecognitionApprovedHistoryId = carRecognitionApprovedHistoryService.GetMany(w => w.ApprovalId == ApprovalId && w.NotificationId == NotificationId).Select(w => w.CarRecognitionApprovedHistoryId).FirstOrDefault();
            var entity = carRecognitionApprovedHistoryService.GetById(Convert.ToInt32(CarRecognitionApprovedHistoryId));
            entity.CheckedStatus = "R";
            entity.IsApproved = false;
            entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            entity.UpdateDate = DateTime.UtcNow;
            carRecognitionApprovedHistoryService.Update(entity);


            //var entitys = carRecognitionApprovedHistoryService.GetAll().Where(w =>w.NotificationId == NotificationId).ToList();
            //foreach (var singleentity in entitys)
            //{
            //    var entity = carRecognitionApprovedHistoryService.GetById(singleentity.CarRecognitionApprovedHistoryId);
            //    entity.CheckedStatus = "R";
            //    entity.IsApproved = false;
            //    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            //    entity.UpdateDate = DateTime.UtcNow;
            //    carRecognitionApprovedHistoryService.Update(entity);
            //}


            var NextApprovalNotification = approvalNotificationService.GetById(Convert.ToInt32(NotificationId));
            NextApprovalNotification.CheckedStatus = "R";
            NextApprovalNotification.IsChecked = true;
            NextApprovalNotification.IsActive = true;
            NextApprovalNotification.UpdateDate = DateTime.Now;
            NextApprovalNotification.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
            approvalNotificationService.Update(NextApprovalNotification);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}