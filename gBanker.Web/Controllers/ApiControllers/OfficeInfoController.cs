using gHRM.Core.Filters.Employee;
using gHRM.Core.Filters.Offices;
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
    public class OfficeInfoController : ApiController
    {
        #region Private Members

        private readonly IOfficeService officeService;
        private readonly IKeyCloakService keyCloakService;

        #endregion

        #region Ctor

        public OfficeInfoController(
            IOfficeService officeService,
            IKeyCloakService keyCloakService
            )
        {
            this.officeService = officeService;
            this.keyCloakService = keyCloakService;
        }

        #endregion  

        #region Get Office Detail

        [HttpPost]
        public async Task<IHttpActionResult> Details([FromBody] OfficeSearchFilter filter)
        {
            var response = new GlobalResponse<Office> { };
            
            try
            {
                var headers = Request.Headers;

                //validate user token
                var responseTokenValidate = await keyCloakService.ValidateUserToken(headers);
                if (!responseTokenValidate.IsSuccess)
                {
                    response = new GlobalResponse<Office> { IsSuccess = false, Message = responseTokenValidate.Message };
                    return Ok(response);
                }

                var officeInfo = await officeService.GetOfficeByFilter(filter);

                response.Result = officeInfo;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Warning, There was an error while getting data or check internet connection!";
            }

            return Ok(response);
        }

        #endregion

        #region Get Offices By Filter

        [HttpPost]
        public async Task<IHttpActionResult> GetOfficesByFilter([FromBody] OfficeSearchFilter filter)
        {
            var model = new OfficeListViewModel
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
                    filter.SortColumn = "OfficeName";

                if (string.IsNullOrWhiteSpace(filter.SortDirection))
                    filter.SortDirection = "ASC";

                var filteredList = await officeService.GetOfficeListByFilter(filter);
                model.Offices = filteredList;
                model.Filter = filter;
            }
            catch (Exception ex)
            {
                model.Response = new BaseResponse { IsSuccess = false, Message = "Warning, There was an error while getting data or check internet connection!" };
                return Ok(model);
            }

            return Ok(model);
        }

        #endregion

        #region Private Methods
       
        #endregion
    }
}
