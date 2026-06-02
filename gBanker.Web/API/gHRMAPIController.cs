using System;
using System.Collections.Generic;
 
using System.Linq;
 
using System.Data;
using System.Web.Http;
using gHRM.Service.StoredProcedure;
using Newtonsoft.Json;
 
using System.Net.Http;
using System.Net;
using gHRM.Web.API.Model;

namespace gHRM.Web.API
{
    public class gHRMAPIController : ApiController
    {
        #region Variables
        //API Added For gHRM GB .KHALID 26 August, 2020.
        private readonly IAPISPService apiSPService;
        //private readonly IOfficeTypeService officeTypeService;
        //private readonly IOfficeService officeService;

        public gHRMAPIController(IAPISPService apiSPService)
        {
            this.apiSPService = apiSPService;

        }

        public gHRMAPIController()
        {
            this.apiSPService = new APISPService() ;
        }


        #endregion Variables

        [System.Web.Mvc.Route("api/ghrmapi/Get")]
        [System.Web.Mvc.HttpGet]
        public string Get()
        {
            return "Welcome To Web API";
        }

        [System.Web.Mvc.Route("api/ghrmapi/GetEmployeeData")]
        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage GetEmployeeData(int OfficeId)
        {
            try
            {
                //int OfficeId = 2658;
                List<EMPLOYEEAPIModel> ListContractDetails = new List<EMPLOYEEAPIModel>();

                var Param = new { officeId = OfficeId };
                var List = apiSPService.GetDataWithParameter(Param, "GetEmployeeList");

                ListContractDetails = List.Tables[0].AsEnumerable()
                .Select(row => new EMPLOYEEAPIModel
                {

                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    PresentAddress = row.Field<string>("PresentAddress"),

                }).ToList();

                var MFICode = ListContractDetails.Select(l => l.EmployeeCode).FirstOrDefault();

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


        [System.Web.Mvc.Route("api/ghrmapi/employeedatajson")]
        //[System.Web.Mvc.HttpPost]
        [System.Web.Mvc.HttpGet]
        public string EmployeeDataJSON(int OfficeId) // int OfficeId
        {
            try
            {
                //int OfficeId = 2658;
                List<EMPLOYEEAPIModel> ListContractDetails = new List<EMPLOYEEAPIModel>();

                var Param = new { officeId = OfficeId };
                var List = apiSPService.GetDataWithParameter(Param, "GetEmployeeList");

                ListContractDetails = List.Tables[0].AsEnumerable()
                .Select(row => new EMPLOYEEAPIModel
                {

                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    PresentAddress = row.Field<string>("PresentAddress"),

                }).ToList();

                var MFICode = ListContractDetails.Select(l => l.EmployeeCode).FirstOrDefault();

                //var Contract_Details = new
                //{
                //    header = new { DataType = "H", DataDate = DateTime.Now, AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                //    Details = new { DataType = "C", values = ListContractDetails },
                //    footer = new { DataType = "F", ListContractDetails.Count }
                //};

                var Contract_Details = new
                {
                    ListContractDetails 
                };

                 
                //var mraCIBData = new { Contract_Details, Subject };
                //var mraCIBData = new { Contract_Details};
                var mraCIBData = new { ListContractDetails };
                var jsonString = JsonConvert.SerializeObject(mraCIBData);


                return jsonString;

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }

        }


       




    }// END Class
}// End 