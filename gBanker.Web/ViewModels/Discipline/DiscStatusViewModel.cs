using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscStatusViewModel : BaseModel
    {
        public int StatusId { get; set; }

        public int StatusType { get; set; }

        public string StatusMsg { get; set; }
    }
}