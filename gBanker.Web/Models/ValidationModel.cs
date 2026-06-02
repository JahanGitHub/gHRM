using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Models
{
    public class ValidationModel
    {
        public bool isValid { get; set; }
        public string message { get; set; }
    }

    public class IsSavedModel
    {
        public bool isSaved { get; set; }
        public string message { get; set; }
    }
}