using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.Helpers;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using gHRM.Core.Utilities;

namespace gHRM.Web.Controllers
{
    public class EmployeeImportController : BaseController
    {
        #region Private Variables

        private readonly IEmployeeService employeeService;
        private readonly ICountryService countryService;
        private readonly IDistrictService districtService;
        private readonly ILgThanaService thanaService;
        private readonly IUnionService unionService;
        private readonly IStateOrProvinceService sateOrProvinceService;
        private readonly IApplicationLogService applicationLogService;
        #endregion

        #region Ctor
        public EmployeeImportController(
             IEmployeeService employeeService,
             ICountryService countryService,
        IDistrictService districtService,
        ILgThanaService thanaService,
        IUnionService unionService,
        IStateOrProvinceService sateOrProvinceService,
            IApplicationLogService applicationLogService
            )
        {
            this.employeeService = employeeService;
            this.countryService = countryService;
            this.districtService = districtService;
            this.thanaService = thanaService;
            this.unionService = unionService;
            this.sateOrProvinceService = sateOrProvinceService;
            this.applicationLogService = applicationLogService;
        }

        #endregion

        #region Import Existing Employee

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExistingEmployee()
        {
            try
            
            {
                string validationMessage;
                var importEmployeeErrorList = "";

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

                var employeeModelList = new List<Employee>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateEmployeeList(employeeModelList, createdBy, ds);

                if (employeeModelList.Count == 0 &&
                    !string.IsNullOrWhiteSpace(validationMessage))
                {
                    importEmployeeErrorList = GetImportEmployeeErrorList(validationMessage);

                    return Json(new
                    {
                        type = "warning",
                        errorLisings = true,
                        importContactErrorList = importEmployeeErrorList,
                        message = "Error occurred. Please seee details in validation message section."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (employeeModelList.Count == 0)
                    return Json(new { type = "warning", errorLisings = false, message = "No employees were found to import." },
                              JsonRequestBehavior.AllowGet);

                var isAdded = employeeService.BulkEmployeesAdd(employeeModelList);
                if (!isAdded)
                    return Json(new { type = "warning", message = "There was an error while adding Import Existing employee. Please try with valid excel data!" },
                             JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = "Import Existing employee successfull!." },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Import Employee Confirmation

        [HttpGet]
        public ActionResult ImportConfirmation()
        {
            return View();
        }

        #endregion

        #region Private Methods

        public string GetImportEmployeeErrorList(string validationMessage)
        {
            var validationErrorList = validationMessage.Split(new string[] { "Error:" }, StringSplitOptions.None);
            var htmlContent = $@" 
                   
                    <div class='row'>
                        <div class='col-md-12'>
                            <div class='panel panel-primary'>
                                <div class='panel-body'>
                                    <div class='lead'>Import Validation Message Summary <small>Partially Imported. Please see details below...</small> </div>
                                    <hr />
                                    <ul class='list-group'>";
            int index = 1;
            foreach (var error in validationErrorList)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    htmlContent += $@" <li class='list-group-item'>{index}. {error}</li>";
                    index++;
                }
            }

            htmlContent += $@"</ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    ";

            return htmlContent;
        }

        /// <summary>
        /// Validates rows and generate list to add
        /// </summary>
        /// <param name="employeeList"></param>
        /// <param name="createdBy"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GenerateEmployeeList(ICollection<Employee> employeeList,
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
                var newEmployee = new Employee
                {
                    EmployeeFamilyInfoes = new List<EmployeeFamilyInfo>(),
                    EmployeeAddresses = new List<EmployeeAddress>()
                };

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(employeeCode))
                    newEmployee.EmployeeCode =GetFormattedEmployeeCode(employeeCode);
                else
                    errorMessage += " Error: Employee Code not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //employee name
                var employeeName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(employeeName))
                    newEmployee.EmployeeName = employeeName;
                else
                    errorMessage += " Error: Employee Name not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //employee name in bangla
                var employeeNameInBangla = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(employeeNameInBangla))
                    newEmployee.EmployeeNameBng = employeeNameInBangla;

                // father's name 
                var fathersName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(fathersName))
                {
                    var newEmployeeFamilyInfo = new EmployeeFamilyInfo
                    {
                        Name = fathersName,
                        Relation = EmployeeFamilyRelationConstants.Father,
                        Gender = GenderConstants.Male,
                        FamilyInfoType = FamilyInfoTypeConstants.FatherInfo,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUser = createdBy
                    };

                    newEmployee.EmployeeFamilyInfoes.Add(newEmployeeFamilyInfo);
                }

                // mother's name  
                var mothersName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(mothersName))
                {
                    var newEmployeeFamilyInfo = new EmployeeFamilyInfo
                    {
                        Name = mothersName,
                        Relation = EmployeeFamilyRelationConstants.Mother,
                        Gender = GenderConstants.Female,
                        FamilyInfoType = FamilyInfoTypeConstants.MotherInfo,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUser = createdBy
                    };

                    newEmployee.EmployeeFamilyInfoes.Add(newEmployeeFamilyInfo);
                }

                //designation  
                //var degignation = ds.Tables[0].Rows[i][j++].ToString();
                //if (!string.IsNullOrWhiteSpace(degignation))
                //{
                //    newEmployee.PayrollDesignation = degignation;
                //}

                //email
                var email = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    newEmployee.Email = email;
                }

                // gender  
                var gender = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrEmpty(gender))
                {
                    var matchedGender = "";
                    //get gender info
                    matchedGender = GetGendarInfo(gender, matchedGender);
                    newEmployee.Gender = matchedGender;
                }

                //blood group
                var bloodGroup = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(bloodGroup))
                {
                    var matchedBloodGroup = "";

                    //get matched bloodgroup
                    matchedBloodGroup = GetMatchedBloodGroup(bloodGroup, matchedBloodGroup);

                    newEmployee.BloodGroup = matchedBloodGroup;
                }

                //religion
                var religion = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(religion))
                {
                    var matchedReligion = "";

                    //get matched religion
                    matchedReligion = GetMatchedReligion(religion);

                    newEmployee.Religion = matchedReligion;
                }

                //national ID
                newEmployee.NationalId = ds.Tables[0].Rows[i][j++].ToString();

                //present address
                j = PopulatePresentAddress(createdBy, ds, i, j, newEmployee);

                //permanent address
                j = PopulatePermanentAddress(createdBy, ds, i, j, newEmployee);

                //contact no
                newEmployee.ContactNo1 = ds.Tables[0].Rows[i][j++].ToString();

                //joining date
                var joiningDate = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(joiningDate))
                {
                    //populate joining date
                    var validJoiningDate = PopulateValidDate(joiningDate);

                    if (validJoiningDate != null)
                        newEmployee.FirstJoiningDate = (DateTime)validJoiningDate;
                }

                //date of birth
                var dateOfBirth = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(dateOfBirth))
                {
                    //populate date of birth
                    var validDateOfBirth = PopulateValidDate(dateOfBirth);

                    if (validDateOfBirth != null)
                        newEmployee.DateOfBirth = (DateTime)validDateOfBirth;
                }

                //date of confirmation
                var dateOfConfirmation = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(dateOfConfirmation))
                {
                    //populate date of confirmation
                    var validDateOfConfirmation = PopulateValidDate(dateOfConfirmation);

                    if (validDateOfConfirmation != null)
                        newEmployee.ConfirmationDate = (DateTime)validDateOfConfirmation;
                }

                //existance
                var existance = ds.Tables[0].Rows[i][j++].ToString();

                var validExistance = false;
                if (!string.IsNullOrWhiteSpace(existance))
                {
                    switch (existance.ToUpper())
                    {
                        case "ACTIVE":
                            validExistance = true;
                            break;

                        default:
                            validExistance = false;
                            break;
                    }
                }

                newEmployee.IsActive = true;

                //employee status
                newEmployee.EmployeeStatusId = validExistance ? EmployeeStatusConstants.Regular : EmployeeStatusConstants.Resign;

                newEmployee.CreateUser = createdBy;
                newEmployee.CreateDate = DateTime.Now;
                newEmployee.CompanyId = CompanyConstants.DefaultCompany;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    employeeList.Add(newEmployee);
                }
                else
                {
                    validationMessage += errorMessage;
                }
            }

            return validationMessage;
        }

        private int PopulatePermanentAddress(long createdBy, DataSet ds, int i, int j, Employee newEmployee)
        {
            var permanentStreetOrHouse = ds.Tables[0].Rows[i][j++].ToString();
            var permanentCountry = ds.Tables[0].Rows[i][j++].ToString();
            var permanentDivision = ds.Tables[0].Rows[i][j++].ToString();
            var permanentDistrict = ds.Tables[0].Rows[i][j++].ToString();
            var permanentThana = ds.Tables[0].Rows[i][j++].ToString();
            var permanentUnion = ds.Tables[0].Rows[i][j++].ToString();

            if (!string.IsNullOrWhiteSpace(permanentCountry) && !string.IsNullOrWhiteSpace(permanentDivision))
            {
                var permanentCountryInfo = countryService.GetByName(permanentCountry);
                var permanentDivisionInfo = sateOrProvinceService.GetByName(permanentDivision);

                if (permanentCountryInfo != null && permanentDivisionInfo != null)
                {
                    //district
                    var permanentDistictInfo = districtService.GetByName(permanentDistrict);
                    int? permanentDistictId = null;
                    if (permanentDistictInfo != null)
                        permanentDistictId = permanentDistictInfo.district_id;

                    //thana
                    var permanentThanaInfo = thanaService.GetByName(permanentThana);
                    int? permanentThanaId = null;
                    if (permanentThanaInfo != null)
                        permanentThanaId = permanentThanaInfo.thana_id;

                    //union
                    var permanentUnionInfo = unionService.GetByName(permanentUnion);
                    int? permanentUnionId = null;
                    if (permanentUnionInfo != null)
                        permanentUnionId = permanentUnionInfo.union_id;

                    var newEmployeeAddress = new EmployeeAddress
                    {
                        AddressType = AddressTypeConstants.PermanentAddress,
                        CountryId = permanentCountryInfo.CountryId,
                        StateOrProvinceId = permanentDivisionInfo.StateOrProvinceId,
                        DistrictId = permanentDistictId,
                        ThanaId = permanentThanaId,
                        UnionId = permanentUnionId,
                        StreetOrHouse = permanentStreetOrHouse,
                        IsActive = true,
                        CreateUser = createdBy,
                        CreateDate = DateTime.Now,
                        AddressDetail = permanentStreetOrHouse,
                    };

                    newEmployee.EmployeeAddresses.Add(newEmployeeAddress);
                }
            }

            return j;
        }

        private int PopulatePresentAddress(long createdBy, DataSet ds, int i, int j, Employee newEmployee)
        {
            var presentStreetOrHouse = ds.Tables[0].Rows[i][j++].ToString();
            var presentCountry = ds.Tables[0].Rows[i][j++].ToString();
            var presentDivision = ds.Tables[0].Rows[i][j++].ToString();
            var presentDistrict = ds.Tables[0].Rows[i][j++].ToString();
            var presentThana = ds.Tables[0].Rows[i][j++].ToString();
            var presentUnion = ds.Tables[0].Rows[i][j++].ToString();

            if (!string.IsNullOrWhiteSpace(presentCountry) && !string.IsNullOrWhiteSpace(presentDivision))
            {
                var presentCountryInfo = countryService.GetByName(presentCountry);
                var presentDivisionInfo = sateOrProvinceService.GetByName(presentDivision);

                if (presentCountryInfo != null && presentDivisionInfo != null)
                {
                    //district
                    var presentDistictInfo = districtService.GetByName(presentDistrict);
                    int? presentDistictId = null;
                    if (presentDistictInfo != null)
                        presentDistictId = presentDistictInfo.district_id;

                    //thana
                    var presentThanaInfo = thanaService.GetByName(presentThana);
                    int? presentThanaId = null;
                    if (presentThanaInfo != null)
                        presentThanaId = presentThanaInfo.thana_id;

                    //union
                    var presentUnionInfo = unionService.GetByName(presentUnion);
                    int? presentUnionId = null;
                    if (presentUnionInfo != null)
                        presentUnionId = presentUnionInfo.union_id;

                    var newEmployeeAddress = new EmployeeAddress
                    {
                        AddressType = AddressTypeConstants.PresentAddress,
                        CountryId = presentCountryInfo.CountryId,
                        StateOrProvinceId = presentDivisionInfo.StateOrProvinceId,
                        DistrictId = presentDistictId,
                        ThanaId = presentThanaId,
                        UnionId = presentUnionId,
                        StreetOrHouse = presentStreetOrHouse,
                        IsActive = true,
                        CreateUser = createdBy,
                        CreateDate = DateTime.Now,
                        AddressDetail = presentStreetOrHouse,
                    };

                    newEmployee.EmployeeAddresses.Add(newEmployeeAddress);
                }
            }

            return j;
        }

        private static string GetMatchedReligion(string religion)
        {
            string matchedReligion;
            //get matched religion
            switch (religion.ToUpper())
            {
                case "ISLAM":
                    matchedReligion = ReligionConstants.Islam;
                    break;

                case "HINDU":
                    matchedReligion = ReligionConstants.Hindu;
                    break;

                case "CHRISTAN":
                    matchedReligion = ReligionConstants.Christan;
                    break;

                case "BUDDISH":
                    matchedReligion = ReligionConstants.Buddish;
                    break;

                default:
                    matchedReligion = ReligionConstants.Buddish;
                    break;
            }

            return matchedReligion;
        }


        /// <summary>
        /// get gender info
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="matchedGender"></param>
        /// <returns></returns>
        private string GetGendarInfo(string gender, string matchedGender)
        {
            switch (gender.ToUpper())
            {
                case "MALE":
                    matchedGender = GenderConstants.Male;
                    break;

                case "FEMALE":
                    matchedGender = GenderConstants.Female;
                    break;

                default:
                    matchedGender = GenderConstants.Common;
                    break;
            }

            return matchedGender;
        }


        /// <summary>
        /// Populate Joining Date
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <param name="joiningDate"></param>
        private DateTime? PopulateValidDate(string joiningDate)
        {
            DateTime? validJoiningDate = null;
            var fragmentedDate = joiningDate.Split(' ');
            joiningDate = fragmentedDate[0];
            try
            {
                var dateTime = DateTime.ParseExact(joiningDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                validJoiningDate = dateTime;
            }
            catch
            {
                validJoiningDate = null;
            }

            if (validJoiningDate == null)
            {
                try
                {
                    var dateTime = DateTime.ParseExact(joiningDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                    validJoiningDate = dateTime;
                }
                catch
                {
                    validJoiningDate = null;
                }
            }

            return validJoiningDate;
        }


        /// <summary>
        /// Get matched bloodgroup
        /// </summary>
        /// <param name="bloodGroup"></param>
        /// <param name="matchedBloodGroup"></param>
        /// <returns></returns>
        private string GetMatchedBloodGroup(string bloodGroup, string matchedBloodGroup)
        {
            switch (bloodGroup.ToUpper())
            {
                case BloodGroupConstants.APlus:
                    matchedBloodGroup = BloodGroupConstants.APlus;
                    break;

                case BloodGroupConstants.ANegative:
                    matchedBloodGroup = BloodGroupConstants.ANegative;
                    break;

                case BloodGroupConstants.BPlus:
                    matchedBloodGroup = BloodGroupConstants.BPlus;
                    break;

                case BloodGroupConstants.BNegative:
                    matchedBloodGroup = BloodGroupConstants.BNegative;
                    break;

                case BloodGroupConstants.ABPlus:
                    matchedBloodGroup = BloodGroupConstants.ABPlus;
                    break;

                case BloodGroupConstants.ABNegative:
                    matchedBloodGroup = BloodGroupConstants.ABNegative;
                    break;

                case BloodGroupConstants.OPlus:
                    matchedBloodGroup = BloodGroupConstants.OPlus;
                    break;

                case BloodGroupConstants.ONegative:
                    matchedBloodGroup = BloodGroupConstants.ONegative;
                    break;

                default:
                    matchedBloodGroup = BloodGroupConstants.Unknown;
                    break;
            }

            return matchedBloodGroup;
        }


        /// <summary>
        /// Processes the uploaded employee import file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="validationMessage"></param>
        /// <returns></returns>
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

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/EmployeeImport/");
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


                    var query = string.Format("Select * from [{0}]", "EmployeeImport$");// excelSheets[0]);

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


        private string GetFormattedEmployeeCode(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return employeeCode;

            if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GT)
            {
                return employeeCode;
            }

            employeeCode =CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);

            return employeeCode;
        }

        #endregion
    }
}
