
#region Usings

using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Promotion;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeTransferImportController : BaseController
    {
        #region Private Members

        private readonly IEmployeeTransferService employeeTransferService;

        #endregion

        #region Ctor
        public EmployeeTransferImportController(IEmployeeTransferService employeeTransferService)
        {
            this.employeeTransferService = employeeTransferService;
        }
        #endregion

        #region Import Backlog

        [HttpGet]
        public ActionResult Import()
        {
            return Redirect("/ExcelImport/TransferBacklog");
        }

        [HttpPost]
        public ActionResult ImportBacklog()
        {
            try
            {
                string validationMessage;               

                var isAjax = Request.IsAjaxRequest();

                if (!ModelState.IsValid)
                    return Json(new { type = "warning", errorLisings = false, message = "Error on file, Please try again" },
                               JsonRequestBehavior.AllowGet);

                if (Request.Files.Count <= 0)
                    return Json(new { type = "warning", errorLisings = false, message = "File not found. Please try again." },
                             JsonRequestBehavior.AllowGet);

                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile(file, out validationMessage);

                if (ds == null)
                {
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);
                }

                var transferBackLogImportModelList = new List<TransferBackLogImportModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateTransferBackLogList(transferBackLogImportModelList, createdBy, ds);

                if (transferBackLogImportModelList.Count == 0 &&
                    !string.IsNullOrWhiteSpace(validationMessage))
                {
                    return Json(new
                    {
                        type = "warning",
                        errorLisings = true,
                        message = "Error occurred. Please seee details in validation message section."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (transferBackLogImportModelList.Count == 0)
                    return Json(new { type = "warning", errorLisings = false, message = "No promotion records were found to import." },
                              JsonRequestBehavior.AllowGet);

                var isAdded = employeeTransferService.BulkPromotionBackLogAdd(transferBackLogImportModelList);
                if (!isAdded)
                    return Json(new { type = "warning", message = "There was an error while adding import existing promotion. Please try with valid excel data!" },
                             JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = "Import existing promotion successfull!." },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "There was an error while adding import existing promotion. Please try with valid excel data!" },
                            JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region Import Confirmation

        [HttpGet]
        public ActionResult ImportConfirmation()
        {
            return View();
        }

        #endregion

        #region Private Methods

        private DataSet GetMemberDatasetFromFile(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();

            validationMessage = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var ticks = DateTime.Now.Ticks;

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/TransferBacklog/");
                    var fileLocation = $"{serverMappedPath}{ticks}/{file.FileName}";
                    var directory = $"{serverMappedPath}{ticks}";

                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        file.SaveAs(fileLocation);
                    }
                    catch
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                        + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();

                    var dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dt == null)
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelSheets = new string[dt.Rows.Count];
                    var t = 0;

                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);


                    var query = string.Format("Select * from [{0}]", "Transfer$");

                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                }
                else
                {
                    validationMessage = "Error! Please import an correct file. You can download the sample file & try again.";
                    return null;
                }
            }
            else
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            return ds;
        }

        private string GenerateTransferBackLogList(ICollection<TransferBackLogImportModel> promotionList,
                                                    long createdBy,
                                                    DataSet ds)
        {
            var validationMessage = "";

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
            {
                return "There is an issue reading data from this file. Please try again.";
            }

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
                var newTransferBackLogImportModel = new TransferBackLogImportModel();

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();

                if (string.IsNullOrWhiteSpace(employeeCode))
                    continue;               

                employeeCode = employeeCode.Replace("\"", "");
                newTransferBackLogImportModel.EmployeeCode = CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);

                //for test
                if (employeeCode == "0921")
                    employeeCode = employeeCode;

                //zone
                var zone = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(zone))
                {
                    newTransferBackLogImportModel.OfficeZone = zone;
                }

                //area
                var area = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(area))
                {
                    newTransferBackLogImportModel.OfficeArea = area;
                }

                // release date 
                var releaseDate = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(releaseDate))
                {
                    releaseDate = releaseDate.Split(' ')[0];
                    try
                    {
                        //try
                        //{
                        //    newTransferBackLogImportModel.ReleaseDate = DateTime.ParseExact(releaseDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        //}
                        //catch
                        //{
                        //    newTransferBackLogImportModel.ReleaseDate = DateTime.ParseExact(releaseDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                        //}

                        newTransferBackLogImportModel.ReleaseDate = DateTime.ParseExact(releaseDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    { 
                    
                    }
                }

                //joining date
                var joiningDate = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(joiningDate))
                {
                    joiningDate = joiningDate.Split(' ')[0];
                    try
                    {
                        //try
                        //{
                        //    newTransferBackLogImportModel.JoiningDate = DateTime.ParseExact(joiningDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        //}
                        //catch
                        //{
                        //    newTransferBackLogImportModel.JoiningDate = DateTime.ParseExact(joiningDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                        //}
                        newTransferBackLogImportModel.JoiningDate = DateTime.ParseExact(joiningDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {

                    }
                }


                //office order
                var officeOrder = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(officeOrder))                
                    newTransferBackLogImportModel.OrderNo = officeOrder;                
                //else                
                //    errorMessage += " Error: Office Order not found in the file. " +
                //                         "Row is " + (1 + i) + " and column is " + j;
                

                //order date
                var orderDate = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(orderDate))
                {
                    orderDate = orderDate.Split(' ')[0];
                    try
                    {
                        //try
                        //{
                        //    newTransferBackLogImportModel.OrderDate = DateTime.ParseExact(orderDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        //}
                        //catch
                        //{
                        //    newTransferBackLogImportModel.OrderDate = DateTime.ParseExact(orderDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                        //}
                        newTransferBackLogImportModel.OrderDate = DateTime.ParseExact(orderDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    newTransferBackLogImportModel.OrderDate = newTransferBackLogImportModel.ReleaseDate;
                }

                // Office Designation
                var officeDesignation = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(officeDesignation))                
                    newTransferBackLogImportModel.OfficeDesignation = officeDesignation;                

                newTransferBackLogImportModel.IsActive = true;
                newTransferBackLogImportModel.CreateUser = createdBy;
                newTransferBackLogImportModel.CreateDate = DateTime.Now;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    promotionList.Add(newTransferBackLogImportModel);
                }
                else
                {
                    var newEmployeeTransferImportFail = new EmployeePromotionFail
                    {
                        FailReason = errorMessage,
                        IsActive = true,
                        CreateUser = createdBy,
                        CreateDate = DateTime.Now
                    };

                    using (var db = new gHRMDBContext())
                    {
                        db.EmployeePromotionFails.Add(newEmployeeTransferImportFail);
                        db.SaveChanges();
                    }

                    validationMessage += errorMessage;
                }
            }

            return validationMessage;
        }       

        #endregion
    }
}
