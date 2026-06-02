using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class DocumentTypeModuleViewModel : BaseModel
    {
        public int DocumentTypeModuleId { get; set; }
        public string DocumentTypeModuleName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
    }
}