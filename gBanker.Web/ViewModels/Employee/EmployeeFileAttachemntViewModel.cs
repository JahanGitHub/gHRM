using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeFileAttachemntViewModel
    {
        public long AttachmentId { get; set; }
        public long EmployeeId { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public bool IsActive { get; set; }
        public string DocumentType { get; set; }
        //public string DocumentType { get; set; }
    }
}