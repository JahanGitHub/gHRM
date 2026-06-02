using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeSignatureDesignationViewModel
    {
        public int SignatureId { get; set; }
        public string SignatureCode { get; set; }
        public string SignatureName { get; set; }
        public int rowSl { get; set; }
    }
}