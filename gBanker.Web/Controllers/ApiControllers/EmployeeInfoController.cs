using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Service;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace gHRM.Web.Controllers.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeInfoController : ApiController
    {
        #region Private Members

        private readonly IEmployeeService empoyeeService;
        private readonly IKeyCloakService keyCloakService;

        #endregion

        #region Ctor

        public EmployeeInfoController(
            IEmployeeService empoyeeService,
            IKeyCloakService keyCloakService
            )
        {
            this.empoyeeService = empoyeeService;
            this.keyCloakService = keyCloakService;
        }

        #endregion

        #region Get Current Employee Info

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentEmployeeInfo()
        {
            var response = new GlobalResponse<Employee> { };

            try
            {
                var headers = Request.Headers;
                //validate user token
                var responseTokenValidate = await keyCloakService.ValidateUserToken(headers);
                if (!responseTokenValidate.IsSuccess)
                {
                    response = new GlobalResponse<Employee> { IsSuccess = false, Message = responseTokenValidate.Message };
                    return Ok(response);
                }
                
                var employeeInfo = await empoyeeService.GetEmployeeInfoByUsername(responseTokenValidate.Result.preferred_username);
                                
                response.Result = employeeInfo;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Warning, There was an error while getting data or check internet connection!";
            }

            return Ok(response);
        }


        #endregion

        #region Get Employees By Filter

        [HttpPost]
        public async Task<IHttpActionResult> GetEmployeesByFilter([FromBody] EmployeeSearchFilter filter)
        {            
            var model = new EmployeeListViewModel
            {
                Response = new BaseResponse{ IsSuccess = true, Message = "Success" }
            };

            try
            {
                var headers = Request.Headers;

                //validate user token
                var responseTokenValidate = await keyCloakService.ValidateUserToken(headers);
                if (!responseTokenValidate.IsSuccess)
                {
                    model.Response = new BaseResponse { IsSuccess = false, Message = responseTokenValidate.Message };
                    return Ok(model);
                }

                if (filter.PageNumber <= 0)
                    filter.PageNumber = 1;

                if (filter.PageSize <= 0)
                    filter.PageSize = 20;

                if (string.IsNullOrWhiteSpace(filter.SortColumn))
                    filter.SortColumn = "EmployeeName";

                if (string.IsNullOrWhiteSpace(filter.SortDirection))
                    filter.SortDirection = "ASC";

                //get employee list by filter
                var filteredList = await empoyeeService.GetEmployeeListByFilter(filter);

                model.Employees = filteredList;
                //model.Filter = filter;
            }
            catch (Exception ex)
            {
                model.Response = new BaseResponse { IsSuccess = false, Message = "Warning, There was an error while getting data or check internet connection!" };                
            }

            return Ok(model);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
