using System;
using System.Collections.Generic;
 
using System.Linq;
 
using System.Data;
using System.Web.Http;
 
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using gHRM.Service.StoreProcedure;

using Microsoft.AspNet.Identity;
using gHRM.Data;
using gHRM.Web.API.Model;
using gHRM.Service;
using gHRM.Data.CodeFirstMigration;
using System.Drawing;
using System.IO;
using System.Web;

namespace gHRM.Web.API
{
    public class gHRMPlusAPIController : ApiController
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: gHRMPlusAPI
        #region Variables
        //API Added For gHRM GB .KHALID 26 August, 2020.
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;

        //private readonly IOfficeTypeService officeTypeService;
        //private readonly IOfficeService officeService;

        //public gHRMPlusAPIController(IEmployeeService employeeServiceA, IEmployeeSPService employeeSPService)
        //{
        //    this.employeeService = employeeServiceA;
        //    this.employeeSPService = employeeSPService;

        //}

        public gHRMPlusAPIController()
        {
            this.employeeSPService = new EmployeeSPService();

        }


        #endregion Variables


        [System.Web.Mvc.Route("api/ghrmplusapi/GetEmployeeProfileInfo")]
        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage  GetEmployeeProfileInfo(string EmployeeCode, string Password)
        {
            try
            {

                List<EMPLOYEEAPIModel> ListContractDetails = new List<EMPLOYEEAPIModel>();
                

                ///Check Password and Employee logins
                var Param = new { EmployeeCode = EmployeeCode, Password = Password };
                var List = employeeSPService.GetDataWithParameter(Param, "GetEmployeeProfileInfo");

                ListContractDetails = List.Tables[0].AsEnumerable()
                .Select(row => new EMPLOYEEAPIModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    PresentAddress = row.Field<string>("PresentAddress"),
                    PasswordHash = row.Field<string>("PasswordHash"),
                    SecurityStamp = row.Field<string>("SecurityStamp"),
                }).ToList();

                var v = ListContractDetails.Select(x => x.EmployeeName).SingleOrDefault();

                if (v != "Not Found")
                {
                    var single = new Employee();
                    string saveDir = "";
                    using (var db = new gHRMDBContext())
                    {
                        single = db.Employees.FirstOrDefault(p => p.EmployeeCode == EmployeeCode && p.IsActive);
                    }
                    // Convert image and save in a folder
                    string SavePath = "";
                    if (single.EmployeeImage != null)
                    {
                        Image image;
                        using (MemoryStream ms = new MemoryStream(single.EmployeeImage))
                        {
                            image = Image.FromStream(ms);
                            string uFile = EmployeeCode + ".png";
                            saveDir = HttpContext.Current.Server.MapPath(@"~/API/Image/");
                            SavePath = saveDir + uFile;

                            // Delete File If Exist
                            if (File.Exists(SavePath))
                            {
                                try
                                {
                                    File.Delete(SavePath);
                                }
                                catch (Exception ex)
                                {
                                    //Do something
                                }
                            }
                            // END File Delete
                            image.Save(SavePath);
                        }// End Using Image
                    }
                    // END
                    EMPLOYEEAPIModel objEMPLOYEEAPIModel = new EMPLOYEEAPIModel();

                    objEMPLOYEEAPIModel.EmployeeCode = single.EmployeeCode;
                    objEMPLOYEEAPIModel.EmployeeName = single.EmployeeName;
                    objEMPLOYEEAPIModel.CurrentOfficeTypeName = "";
                    objEMPLOYEEAPIModel.OfficeName = "";
                    objEMPLOYEEAPIModel.DepartmentName = "";
                    objEMPLOYEEAPIModel.Responsibility = "";
                    objEMPLOYEEAPIModel.ImageLink = SavePath != "" ? Path.GetFileName(SavePath)  : Path.GetFileName(single.EmployeeImageLink);
                    ListContractDetails.Add(objEMPLOYEEAPIModel);

                }// ENd //Not Found


                if (ListContractDetails.Count() == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //var MFICode = ListContractDetails.Select(l => l.EmployeeCode).FirstOrDefault();

                var Contract_Details = new
                {
                    header = new { DataType = "H", DataDate = DateTime.Now, AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                    Details = new { DataType = "C", values = ListContractDetails },
                    footer = new { DataType = "F", ListContractDetails.Count }
                };

                //var Contract_Details = new
                //{
                //    ListContractDetails
                //};


                //var mraCIBData = new { Contract_Details, Subject };
                //var mraCIBData = new { Contract_Details};
                var mraCIBData = new { Contract_Details };

                var jsonString = JsonConvert.SerializeObject(mraCIBData);

                //return jsonString;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                return response;


            }
            catch (Exception ex)
            {
                //return "Error: " + ex.Message.ToString();
                  return Request.CreateResponse(HttpStatusCode.NotFound);
                
            }

        }// END Function


        
        [System.Web.Mvc.Route("api/ghrmplusapi/GetEmployeeLeaveList")]
        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage GetEmployeeLeaveList(string EmployeeCode)
        {
            try
            {

                //int OfficeId = 2658;
                List<LeaveListViewModel> ListContractDetails = new List<LeaveListViewModel>();

                // GetLeaveTakenList(@EmployeeCode nvarchar(50))

                var Param = new { EmployeeCode = EmployeeCode };
                var List = employeeSPService.GetDataWithParameter(Param, "GetLeaveTakenList");

                ListContractDetails = List.Tables[0].AsEnumerable()
                .Select(row => new LeaveListViewModel
                {

                    LeaveTypeName = row.Field<string>("LeaveTypeName"),
                    LeaveStartDate = row.Field<string>("LeaveStartDate"),
                    LeaveEndDate = row.Field<string>("LeaveEndDate"),
                    TotalDays = row.Field<int>("TotalDays"),
                    LeaveReason = row.Field<string>("LeaveReason"),
                  
                }).ToList();

                if (ListContractDetails.Count() == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //var MFICode = ListContractDetails.Select(l => l.EmployeeCode).FirstOrDefault();

                var Contract_Details = new
                {
                    header = new { DataType = "H", DataDate = DateTime.Now, AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                    Details = new { DataType = "C", values = ListContractDetails },
                    footer = new { DataType = "F", ListContractDetails.Count }
                };

                //var Contract_Details = new
                //{
                //    ListContractDetails
                //};


                //var mraCIBData = new { Contract_Details, Subject };
                //var mraCIBData = new { Contract_Details};
                var mraCIBData = new { Contract_Details };

                var jsonString = JsonConvert.SerializeObject(mraCIBData);

                //return jsonString;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                return response;



            }
            catch (Exception ex)
            {
                //return "Error: " + ex.Message.ToString();
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }

        }// END Function




    }// END CLASS
}// END Namespace