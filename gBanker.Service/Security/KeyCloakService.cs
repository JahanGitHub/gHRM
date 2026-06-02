using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Data.DBDetailModels.Offices;
using gHRM.Data.DBDetailModels.Apply;
using gHRM.Data.DBDetailModels.Security;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{

    public interface IKeyCloakService
    {
        Task<KeyCloakTokenResponse> GetAccessToken();
        Task<IEnumerable<SSORoleDetailModel>> GetSSORoles(string accessToken);
        Task<EmployeeAddOrEditApiModel> GetAuthEmployeeyId(string accessToken, int employeeId);
        Task<OfficeAddOrEditApiModel> GetOfficeById(string accessToken, int officeId);
        Task<SSORoleDetailModel> GetSSORole(SSOFetchRoleRequestModel model);
        Task<SSOAuthUserDetailModel> GetUserByUsername(string username, string accessToken);
        Task<KeyCloakResponse> SyncOffice(OfficeAddOrEditApiModel model, string accessToken);

        Task<KeyCloakResponse> SyncApplicant(AddorEditApplicantMasterInfo model, string accessToken);
        Task<KeyCloakResponse> SyncEmployee(EmployeeAddOrEditApiModel model, string accessToken);
        Task<KeyCloakResponse> CreateNewUser(SSORegisterModel model, string accessToken);
        Task<KeyCloakResponse> InactiveAuthUser(InactiveAuthUserModel model, string accessToken);
        Task<KeyCloakResponse> ChangePasswordToAuthUser(AuthUserChangePasswordModel model, string accessToken);
        Task<KeyCloakResponse> CreateNewRole(SSORoleRequestModel request);
        Task<GlobalResponse<SSOLoginUserModel>> ValidateUserToken(HttpRequestHeaders headers);
        Task<KeyCloakResponse> MapRoleWithAuthUser(AuthRoleMappingRequestModel request);
    }
    public class KeyCloakService : IKeyCloakService
    {
        #region Ctor
        public KeyCloakService()
        {

        }
        #endregion

        #region Public Methods

        public async Task<KeyCloakTokenResponse> GetAccessToken()
        {
            try
            {
                WebClient webClient = new WebClient();

                var uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/realms/master/protocol/openid-connect/token");
                webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                //for realm admin user
                var reqparm = new System.Collections.Specialized.NameValueCollection();

                //for normal user
                reqparm.Add("grant_type", "password");
                reqparm.Add("client_id", AuthServerConstants.CLIENT_ID.ToAppSettingValue());
                reqparm.Add("username", AuthServerConstants.USERNAME.ToAppSettingValue());
                reqparm.Add("password", AuthServerConstants.PASSWORD.ToAppSettingValue());


                byte[] responsebytes = await webClient.UploadValuesTaskAsync(uri, "POST", reqparm);

                string responsebody = Encoding.UTF8.GetString(responsebytes);

                var keyCloakTokenResponse = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<KeyCloakTokenResponse>(responsebody);
                keyCloakTokenResponse.IsError = keyCloakTokenResponse.error == KeyCloakErrorConstants.Error;
                keyCloakTokenResponse.Message = keyCloakTokenResponse.error_description;

                return keyCloakTokenResponse;
            }
            catch (Exception ex)
            {
                return new KeyCloakTokenResponse { IsError = true, Message = $"{ex.Message}" };
            }
        }

        public async Task<SSORoleDetailModel> GetSSORole(SSOFetchRoleRequestModel model)
        {
            try
            {
                string responseResult;
                var hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/roles/{model.RoleName}";

                if (model.IsClientRole)
                    hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/clients/{model.AppClientId.ToAppSettingValue()}/roles/{model.RoleName}";

                //hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/clients/{AuthServerConstants.ID_CLIENT_HR_APP_DEMO_ASP_MVC_APP.ToAppSettingValue()}/roles/{roleName}";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {model.AccessToken}";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = httpWebResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseResult = await reader.ReadToEndAsync();
                    reader.Close();
                    dataStream.Close();
                }

                var role = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<SSORoleDetailModel>(responseResult);

                return role;
            }
            catch (Exception ex)
            {
                return new SSORoleDetailModel();
            }
        }

        public async Task<SSOAuthUserDetailModel> GetUserByUsername(string username, string accessToken)
        {
            try
            {
                string responseResult;
                var hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users?username={username}";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {accessToken}";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = httpWebResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseResult = await reader.ReadToEndAsync();
                    reader.Close();
                    dataStream.Close();
                }

                var userList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<SSOAuthUserDetailModel>>(responseResult);

                var userInfo = new SSOAuthUserDetailModel();
                if (userList.Any())
                {
                    userInfo = userList.FirstOrDefault();
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<SSORoleDetailModel>> GetSSORoles(string accessToken)
        {
            try
            {
                string responseResult;
                var hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/clients/{AuthServerConstants.ID_CLIENT_HR_APP_DEMO_ASP_MVC_APP.ToAppSettingValue()}/roles";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {accessToken}";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = httpWebResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseResult = await reader.ReadToEndAsync();
                    reader.Close();
                    dataStream.Close();
                }

                var roles = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<SSORoleDetailModel>>(responseResult);

                return roles;
            }
            catch (Exception ex)
            {
                return new List<SSORoleDetailModel>();
            }
        }

        public async Task<OfficeAddOrEditApiModel> GetOfficeById(string accessToken,int officeId)
        {
            try
            {
                string responseResult;
                var hostURI = $"{AuthServerConstants.SYNC_API_PATH.ToAppSettingValue()}/v1/health-center/api-id/{officeId}";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {accessToken}";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = httpWebResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseResult = await reader.ReadToEndAsync();
                    reader.Close();
                    dataStream.Close();
                }

                responseResult = responseResult.Replace("{\"status\":200,\"object\":", "").Replace("}}","}");
                var office = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OfficeAddOrEditApiModel> (responseResult);

                return office;
            }
            catch (Exception ex)
            {
                return new OfficeAddOrEditApiModel();
            }
        }

        public async Task<EmployeeAddOrEditApiModel> GetAuthEmployeeyId(string accessToken, int employeeId)
        {
            try
            {
                string responseResult;
                var hostURI = $"{AuthServerConstants.SYNC_API_PATH.ToAppSettingValue()}/v1/employee/api-id/{employeeId}";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {accessToken}";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = httpWebResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseResult = await reader.ReadToEndAsync();
                    reader.Close();
                    dataStream.Close();
                }

                responseResult = responseResult.Replace("{\"status\":200,\"object\":", "").Replace("}}", "}");
                var employee = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<EmployeeAddOrEditApiModel>(responseResult);

                return employee;
            }
            catch (Exception ex)
            {
                return new EmployeeAddOrEditApiModel();
            }
        }

        public async Task<GlobalResponse<SSOLoginUserModel>> ValidateUserToken(HttpRequestHeaders headers)
        {
            var response = new GlobalResponse<SSOLoginUserModel> { };
            try
            {
                if (!headers.Contains("Authorization"))
                {
                    response = new GlobalResponse<SSOLoginUserModel> { IsSuccess = false, Message = "Warning, Token not exist. Please try again!" };
                    return response;
                }

                string accessToken = headers.GetValues("Authorization").FirstOrDefault();

                if (!(!string.IsNullOrWhiteSpace(accessToken) && accessToken.StartsWith("Bearer ")))
                {
                    response = new GlobalResponse<SSOLoginUserModel> { IsSuccess = false, Message = "Warning, Invalid Token!" };
                    return response;
                }

                //Get user by access token
                var responseBody = await GetUserByAccessToken(accessToken);

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    response = new GlobalResponse<SSOLoginUserModel> { IsSuccess = false, Message = "Warning, User not exist. Try again!" };
                    return response;
                }

                //deserialize data
                var userInfoResponse = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<SSOLoginUserModel>(responseBody);

                if (string.IsNullOrWhiteSpace(userInfoResponse.preferred_username))
                {
                    response = new GlobalResponse<SSOLoginUserModel> { IsSuccess = false, Message = "Warning, User not exist. Try again!" };
                    return response;
                }

                response = new GlobalResponse<SSOLoginUserModel> { IsSuccess = true, Result = userInfoResponse, Message = "Success, User Found!" };
                return response;
            }
            catch (Exception ex)
            {
                return new GlobalResponse<SSOLoginUserModel> { IsSuccess = false, Message = $"{ex.Message}" };
            }
        }

        public async Task<KeyCloakResponse> SyncOffice(OfficeAddOrEditApiModel model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();

                Uri uri = new Uri($"{AuthServerConstants.SYNC_API_PATH.ToAppSettingValue()}/v1/health-center/add");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> SyncApplicant(AddorEditApplicantMasterInfo model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();

                Uri uri = new Uri($"{AuthServerConstants.SYNC_API_PATH.ToAppSettingValue()}/v1/health-center/add");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }
        public async Task<KeyCloakResponse> SyncEmployee(EmployeeAddOrEditApiModel model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();



                Uri uri = new Uri($"{AuthServerConstants.SYNC_API_PATH.ToAppSettingValue()}/v1/employee/add");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> CreateNewUser(SSORegisterModel model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();

                Uri uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> InactiveAuthUser(InactiveAuthUserModel model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();

                Uri uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users/{model.id}");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "PUT", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> ChangePasswordToAuthUser(AuthUserChangePasswordModel model, string accessToken)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();

                Uri uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users/{model.id}");
                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {accessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                await wcNewUser.UploadStringTaskAsync(uri, "PUT", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> CreateNewRole(SSORoleRequestModel request)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();
                var idOfClient = AuthServerConstants.ID_CLIENT_HR_APP_DEMO_ASP_MVC_APP.ToAppSettingValue();

                wcNewUser.Headers["Content-Type"] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {request.AccessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                //Get role creation uri list
                var uriList = GetRoleCreationUri(request);

                foreach (var uri in uriList)
                {
                    var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request.SSORole);
                    await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);
                }

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }

        public async Task<KeyCloakResponse> MapRoleWithAuthUser(AuthRoleMappingRequestModel request)
        {
            var response = new KeyCloakResponse { };
            try
            {
                var wcNewUser = new WebClient();
                wcNewUser.Headers[HttpRequestHeader.ContentType] = "application/json";
                wcNewUser.Headers["Authorization"] = $"Bearer {request.AccessToken}";
                wcNewUser.Encoding = Encoding.UTF8;

                //Get role creation uri list
                var uri = GetAuthRoleMappingUri(request);
                if (uri == null)
                    return new KeyCloakResponse { IsError = true, Message = "Mapping Uri not found." };

                var requestData = Newtonsoft.Json.JsonConvert.SerializeObject(request.AuthRoles);
                //new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request.AuthRoles);
                await wcNewUser.UploadStringTaskAsync(uri, "POST", requestData);

                return new KeyCloakResponse { IsError = false, Message = "Successfull" };
            }
            catch (Exception ex)
            {
                return new KeyCloakResponse { IsError = true, Message = ex.Message };
            }
        }


        #endregion

        #region Private Methods

        private Uri GetAuthRoleMappingUri(AuthRoleMappingRequestModel request)
        {
            try
            {
                Uri uri = null;
                if (!request.ClientRole) // if realm role
                {
                    uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users/{request.IdOfUser}/role-mappings/realm");
                    return uri;
                }

                //if client role                
                uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/users/{request.IdOfUser}/role-mappings/clients/{request.IdOfClient}");
                return uri;
            }
            catch
            {
                return null;
            }
        }

        private List<Uri> GetRoleCreationUri(SSORoleRequestModel request)
        {
            try
            {
                List<Uri> uriList = new List<Uri>();
                if (!request.SSORole.clientRole) // if realm role
                {
                    var uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/roles");
                    uriList.Add(uri);

                    return uriList;
                }

                foreach (var idofClient in request.AppClientIDs) //if client role
                {
                    var idofAppClient = idofClient.ToAppSettingValue();
                    var uri = new Uri($"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/admin/realms/GK_HEALTH/clients/{idofAppClient}/roles");
                    uriList.Add(uri);
                }

                return uriList;
            }
            catch
            {
                return new List<Uri>();
            }
        }

        private async Task<string> GetUserByAccessToken(string accessToken)
        {
            string responseResult;
            var hostURI = $"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/realms/GK_HEALTH/protocol/openid-connect/userinfo";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["Authorization"] = accessToken;

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = httpWebResponse.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseResult = await reader.ReadToEndAsync();
                reader.Close();
                dataStream.Close();
            }

            return responseResult;
        }

        #endregion
    }
}
