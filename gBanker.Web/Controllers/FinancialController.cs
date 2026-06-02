using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using gHRM.Web.ViewModels.Financial;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
 


namespace gHRM.Web.Controllers
{
    public class FinancialController : BaseController
    {

        #region Variables


        private readonly IEmployeeSPService employeeSPService;
        //private readonly IOfficeTypeService officeTypeService;
        //private readonly IOfficeService officeService;

        public FinancialController(IEmployeeSPService employeeSPService)
        {
            this.employeeSPService = employeeSPService;
            
        }


        #endregion Variables



        // GET: Financial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            //  ViewData["ZoOfficeList"] = items;
            ViewData["ReasonList"] = items;
            ViewData["OfficeDesigList"] = items;
            ViewData["DistrictList"] = items;



            // FOR Office DropDown
            //ViewData["AddressTypeList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;

            //var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["LoggedInOfficeName"] = ""; //offc.OfficeName;

            ViewData["SecondLevel"] = "2601";  //offc.SecondLevel;
            ViewData["SecondLevelId"] = 2577; // officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = "2601"; //offc.ThirdLevel;
            ViewData["ThirdLevelId"] = 2577; // officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = "2601";  //offc.FourthLevel;
            ViewData["FourthLevelId"] = 2577;  //officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;

            // End Office DropDown

            return View();
        }

        public ActionResult ViewIncrementReport()
        {

            //IEnumerable<SelectListItem> items = new SelectList(" ");
            ////ViewData["AddressTypeList"] = items;
            //ViewData["OfficeList"] = items;
            //ViewData["HOOfficeList"] = items;
            //ViewData["ZOOfficeList"] = items;
            //ViewData["AOOfficeList"] = items;
            //ViewData["BOOfficeList"] = items;
            //ViewData["ZAOOfficeList"] = items;
            //ViewData["OfficeType"] = LoggedInOfficeType;
            //ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            //var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            //ViewData["SecondLevel"] = offc.SecondLevel;
            //ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            //ViewData["ThirdLevel"] = offc.ThirdLevel;
            //ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            //ViewData["FourthLevel"] = offc.FourthLevel;
            //ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            //var model = new EmployeeViewModel();
            // MapDropDownList(model);
            // return View(model);

            return View();
        }

        public ActionResult ViewPromotionReport()
        {
            //model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();//desig_items;
            return View();
        }

        public ActionResult GenerateIncrementListReport(string OfficeTypeId = "0", string SelectedDate = "0", string DownloadExcel = "0", string SoftwareName = "ghrmplus")
        {
            try
            {
                DataSet OverdueMls;
                if ("paperless" == SoftwareName)
                {
                    var param = new { @DateFrom = SelectedDate, @cOfficeCode = OfficeTypeId };// SP_Rpt_Multiple_Leave
                    OverdueMls = employeeSPService.GetDataWithParameterIncrement(param, "List_of_Employee_Who_is_Elegible_for_Increment");
                }
                else
                {
                    var param = new { date = SelectedDate, OfficeTypeId = OfficeTypeId };
                    OverdueMls = employeeSPService.GetDataWithParameter(param, "[promo].[SP_ListOfEmployeeWhoiseligibleForIncrement]");
                }
                var reportParam = new Dictionary<string, object>();

                if (DownloadExcel == "1")
                {
                    ReportHelper.ExportExcelReport("increment.rpt", OverdueMls.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("increment.rpt", OverdueMls.Tables[0], reportParam);
                }

                

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult IncrementReportExeclPrint(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttenDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ManuallyUpdatedReport");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("TimeKeeping/rpt_TimeKeeping_ManuallyUpdatedReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult GetOrgList()
        {
            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("GetOrgList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OrgId = row.Field<int>("OrgId"),
                OrgName = row.Field<string>("OrgName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OrgId.ToString(),
                Text = string.Format("{0}", x.OrgName)
            });

            var Component_items = new List<SelectListItem>();
            //if (Components.ToList().Count > 0)
            //{
            //    Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }
        //dims

        public JsonResult Create(

             string orgId
            , string OrgName
            , string OfficeCode
            , string OfficeName
            , string ZoneCode
            , string ServiceCharge
            , string Deduction
            , string DepositDraftAmount
            , string SlipDraftNo
            , string ReferenceNo
            , string SendedBy
            , string DepositSendDate
            , string Remarks

            , string DeductionOthers
            , string LBUNetTaka

            ) // End of Parameter KK
        {
            string result = "Data Saved Successfully";
            try
            {
                var param = new
                {

                    orgId = orgId,
                    OrgName = OrgName,
                    OfficeCode = OfficeCode,
                    OfficeName = OfficeName,
                    ZoneCode = ZoneCode,
                    ServiceCharge = ServiceCharge,
                    Deduction = Deduction,
                    DepositDraftAmount = DepositDraftAmount,
                    SlipDraftNo = SlipDraftNo,
                    ReferenceNo = ReferenceNo,
                    SendedBy = SendedBy,
                    DepositSendDate = DepositSendDate,
                    Remarks = Remarks,
                    DeductionOthers = DeductionOthers,
                    LBUNetTaka = LBUNetTaka

                };
                var val = employeeSPService.GetDataWithParameter(param, "SP_PR_SET_FinancialRegister"); // Was this SP_PR_CreateSALoanDisburseTmp

            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End of Function.



        public JsonResult GetRegisterList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string ZoneCode, string SlipDraftNo, string DepositSendDateFrom, string DepositSendDateTo, string ReferenceNo)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (ZoneCode != null)
                {
                    if (ZoneCode != "")
                        sb.Append(" AND  ZoneCode = '" + ZoneCode.Trim() + "'");
                }

                if (SlipDraftNo != null)
                {
                    if (SlipDraftNo != "")
                        sb.Append(" AND SlipDraftNo = '" + SlipDraftNo.Trim() + "'");
                }

                if (DepositSendDateFrom != null && DepositSendDateTo !=null)
                {
                    if (DepositSendDateFrom != "" && DepositSendDateTo != "")
                        sb.Append(" AND DepositSendDate between '" + DepositSendDateFrom.Trim() + "'" + " AND '" + DepositSendDateTo.Trim() + "' " );
                }

                
                     if (ReferenceNo != null)
                {
                    if (ReferenceNo != "")
                        sb.Append(" AND ReferenceNo = '" + ReferenceNo.Trim() + "'");
                }


                /*
                 
                if (ComponentName != null)
                {
                    if (ComponentName != "")
                        sb.Append(" AND ai.AppliedPostId LIKE '" + ComponentName.Trim() + "%'");
                }  

                 */
                List<FinancialViewModel> List_InvMasterViewModel = new List<FinancialViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "SP_Get_ServiceRegister_List");

                List_InvMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FinancialViewModel
                {
                    rowSl = row.Field<long>("rowSl"),
                    Id = row.Field<long>("Id"),
                    OrgId = row.Field<int>("OrgId"),
                    OrgName = row.Field<string>("OrgName"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    ZoneCode = row.Field<string>("ZoneCode"),
                    ServiceCharge = row.Field<decimal?>("ServiceCharge"),
                    Deduction = row.Field<decimal?>("Deduction"),
                    DepositDraftAmount = row.Field<decimal?>("DepositDraftAmount"),
                    SlipDraftNo = row.Field<string>("SlipDraftNo"),
                    ReferenceNo = row.Field<string>("ReferenceNo"),
                    SendedBy = row.Field<string>("SendedBy"),
                    DepositSendDate = row.Field<string>("DepositSendDate"),
                    Remarks = row.Field<string>("Remarks"),
                    DeductionOthers = row.Field<decimal?>("DeductionOthers"),
                    LBUNetTaka = row.Field<decimal?>("LBUNetTaka"),



                }).ToList();

                var currentPageRecords = List_InvMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_InvMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function


        public JsonResult Delete(int Id)
        {
            string result = "Data Deleted Successfully";
            try
            {
                var param2 = new { Id = Id };
                var val = employeeSPService.GetDataWithParameter(param2, "SP_DeleteServiceRegister");
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetHOOfficeList()
        {
            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("GetHOList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }//End Function



        public JsonResult GetZOOfficeList()
        {
            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("GetZonalOfficeList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }//End Function


        public JsonResult GetAOOfficeList(string zoCode)
        {

            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var param = new { @ZoneCode = zoCode };
            var List = employeeSPService.GetDataWithParameter(param, "GetAreaOfficeList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);


        }// END Function

        public JsonResult GetBOOfficeList(string aoCode)
        {

            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var param = new { @AreaCode = aoCode };
            var List = employeeSPService.GetDataWithParameter(param, "GetBranchOfficeList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }// END Function



        public JsonResult GetBOOfficeListByZO(string zoCode)
        {
            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var param = new { @ZoneCode = zoCode };
            var List = employeeSPService.GetDataWithParameter(param, "GetBranchOfficeListByZone");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }// END Function


        public JsonResult GetZAOOfficeList()
        {
            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("GetZonalAuditOfficeList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);


        }// END Function


        public JsonResult GetBranchOfficeList(string areaOffice_val)
        {


            List<FinancialViewModel> List_ViewModel = new List<FinancialViewModel>();

            var param = new { areaOffice_val = areaOffice_val };
            var List = employeeSPService.GetDataWithParameter(param, "GetAreaOfficeListByCode");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new FinancialViewModel
            {
                OfficeID = row.Field<int>("OfficeId"),
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeCode, x.OfficeName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);


        }// END Function










    }// END Class
}// ENd Namespace