using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Company
{
    public class CompanyLogoViewModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public byte[] CompanyLogo { get; set; }
        public string CompanyLogoURI { get; set; }
    }
}
