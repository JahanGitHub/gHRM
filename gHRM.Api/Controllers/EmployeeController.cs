#region Usings

using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace gHRM.Api.Controllers
{
    public class EmployeeController : ApiController
    {
        #region Private Members

        private readonly IEmployeeService empoyeeService;

        #endregion

        #region Ctor

        public EmployeeController(
            IEmployeeService empoyeeService
            )
        {
            this.empoyeeService = empoyeeService;
        }

        #endregion
        
    }
}
