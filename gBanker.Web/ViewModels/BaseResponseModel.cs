using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class BaseResponseModel
    {
        public BaseResponseModel()
        {
            message = "";
        }
        public bool success { get; set; }
        public string message { get; set; }
    }
}