using gHRM.Core.Filters.Employee;
using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Api.ViewModels.Employees
{
    public class EmloyeeListViewModel
    {
        public IEnumerable<Employee> Employee { get; set; }
        public EmployeeSearchFilter SearchFilter { get; set; }
    }
}