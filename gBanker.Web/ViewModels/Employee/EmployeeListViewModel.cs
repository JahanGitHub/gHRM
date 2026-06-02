using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeListViewModel 
    {
        public IEnumerable<EmployeeDetailApiModel> Employees { get; set; }
        public BaseResponse Response { get; set; }
    }
}