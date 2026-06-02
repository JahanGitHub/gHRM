using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class TempAttachment
    {
        public string AttachmentContent { get; set; }
        public string ContentFileName { get; set; }
    }
}