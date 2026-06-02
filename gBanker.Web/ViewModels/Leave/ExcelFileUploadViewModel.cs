
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Leave
{
    public class ExcelFileUploadViewModel : BaseModel
    {
        [Display(Name = "BatchFile")]
        public string BatchFile { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string value4 { get; set; }

    }
}