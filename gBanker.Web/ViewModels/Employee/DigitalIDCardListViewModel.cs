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
    public class DigitalIDCardListViewModel
    {
        public IEnumerable<EmployeeDigitalIDModel> DigitalIDCardInfos { get; set; }
        public string BaseUrl { get; set; }
    }
}