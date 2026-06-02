using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class OfficeListViewModel : BaseModel
    {
        public IEnumerable<DBOfficeDetailModel> Offices { get; set; }
        public OfficeSearchFilter Filter { get; set; }
        public BaseResponse Response { get; set; }
    }
}