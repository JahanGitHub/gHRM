using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Security;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IUserService
    {
        BaseResponse AddSSOUserInGBanker(SSOUserRegistrationModel model);
        AspDotNetUserModel GetAspNetUserByUsername(string username);
    }

    public class UserService : IUserService
    {
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeFamilyInfoService employeeFamilyInfoService;
        private readonly IEmployeeAddressService employeeAddressService;
        private readonly IEmployeeDesignationService employeeDesignationService;

        public UserService(IEmployeeService employeeService,
            IOfficeService officeService,
            IEmployeeFamilyInfoService employeeFamilyInfoService,
            IEmployeeAddressService employeeAddressService,
            IEmployeeDesignationService employeeDesignationService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.employeeFamilyInfoService = employeeFamilyInfoService;
            this.employeeAddressService = employeeAddressService;
            this.employeeDesignationService = employeeDesignationService;
        }

        public AspDotNetUserModel GetAspNetUserByUsername(string username)
        {
            var single = new AspDotNetUserModel();
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $"SELECT * FROM [dbo].[AspNetUsers] WHERE UserName='{username}'";
                single = db.Database.SqlQuery<AspDotNetUserModel>(sqlCommand).FirstOrDefault();
            }

            return single;
        }

        public BaseResponse AddSSOUserInGBanker(SSOUserRegistrationModel model)
        {
            var response = new BaseResponse();

            try
            {
                var employee = employeeService.GetByEmpId(model.EmployeeID);

                if (employee == null)
                    return response = new BaseResponse { IsSuccess = false, Message = "Employee not found." };

                if (employee.OfficeId == null || employee.OfficeId == 0)
                    return response = new BaseResponse { IsSuccess = false, Message = "Employee Office not found." };

                if (string.IsNullOrWhiteSpace(employee.Gender))
                    return response = new BaseResponse { IsSuccess = false, Message = "Employee Gender not found." };

                //get employee designation
                var employeeDesignation = employeeDesignationService.GetById((int)employee.DesignationId);

                if (employeeDesignation == null)
                    return response = new BaseResponse { IsSuccess = false, Message = "Employee designation not found." };

                //get employee office
                var office = officeService.GetById((int)employee.OfficeId);
                if (office == null)
                    return response = new BaseResponse { IsSuccess = false, Message = "Employee Office not found." };

                //get employee family info for guardian info
                var employeeFamily = employeeFamilyInfoService.GetDefaultEmployeeFamilyInfo(employee.EmployeeId);

                //get employee address info
                var employeeAddress = employeeAddressService.GetDefaultEmployeeAddress(employee.EmployeeId);

                var employeeAddressInfo = "Bangladesh";
                if (employeeAddress != null)
                {
                    var house = string.IsNullOrWhiteSpace(employeeAddress.StreetOrHouse) ? "" : employeeAddress.StreetOrHouse;
                    var country = employeeAddress.Country != null ? employeeAddress.Country.CountryName : "";
                    var division = employeeAddress.StateOrProvince != null ? employeeAddress.StateOrProvince.Name : "";
                    var zipcode = string.IsNullOrWhiteSpace(employeeAddress.ZipCode) ? "" : employeeAddress.ZipCode;

                    employeeAddressInfo = $"{house} {division} {country} {zipcode}";
                }

                //employee info
                model.EmployeeCode = employee.EmployeeCode;
                model.OfficeCode = office.OfficeCode;
                model.EmpName = employee.EmployeeName;
                model.EmpNameBen = string.IsNullOrWhiteSpace(employee.EmployeeNameBng)?"": employee.EmployeeNameBng;
                model.GuardianName = employeeFamily != null ? employeeFamily.Name : "";
                model.EmpAddress = employeeAddressInfo;
                model.PhoneNo = employee.ContactNo1;
                model.Email = string.IsNullOrWhiteSpace(employee.Email) ? "" : employee.Email;
                model.Gender = string.IsNullOrWhiteSpace(employee.Gender) ? "" : employee.Gender;
                model.BirthDate =employee.DateOfBirth!=null ? employee.DateOfBirth : DateTime.Now;
                model.Designation = employeeDesignation.DesignationName;
                model.JoiningDate = employee.FirstJoiningDate;

                var connectionString = ConfigurationManager.AppSettings["gBankerDbConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $@"[dbo].[SSORegistration_RegisterSSOUser] 
                                    @EmployeeCode,
                                    @OfficeCode,
                                    @EmpName,
                                    @EmpNameBen,
                                    @GuardianName,
                                    @EmpAddress,
                                    @PhoneNo,
                                    @Email,
                                    @Gender,
                                    @BirthDate,
                                    @Designation,
                                    @JoiningDate,
                                    @OrganizationId,
                                    @ASPNETUserId,
                                    @UserName,
                                    @RoleName,
                                    @PasswordHash,
                                    @SecurityStamp
                                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                        command.Parameters.AddWithValue("@OfficeCode", model.OfficeCode);
                        command.Parameters.AddWithValue("@EmpName", model.EmpName);
                        command.Parameters.AddWithValue("@EmpNameBen", model.EmpNameBen);
                        command.Parameters.AddWithValue("@GuardianName", model.GuardianName);

                        command.Parameters.AddWithValue("@EmpAddress", model.EmpAddress);
                        command.Parameters.AddWithValue("@PhoneNo", model.PhoneNo);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Gender", model.Gender);

                        command.Parameters.AddWithValue("@BirthDate", Convert.ToDateTime(model.BirthDate).ToString("dd-MMM-yyyy"));
                        command.Parameters.AddWithValue("@Designation", model.Designation);
                        command.Parameters.AddWithValue("@JoiningDate", model.JoiningDate.ToString("dd-MMM-yyyy"));
                        command.Parameters.AddWithValue("@OrganizationId", model.OrganizationId);

                        command.Parameters.AddWithValue("@ASPNETUserId", model.ASPNETUserId);
                        command.Parameters.AddWithValue("@UserName", model.UserName);
                        command.Parameters.AddWithValue("@RoleName", model.RoleName);
                        command.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                        command.Parameters.AddWithValue("@SecurityStamp", model.SecurityStamp);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return response=new BaseResponse { IsSuccess=false,Message="Error on adding user in gBanker" };
            }

            return response = new BaseResponse { IsSuccess = true, Message = "SSO Registration Completed!" };            
        }               
    }
}
