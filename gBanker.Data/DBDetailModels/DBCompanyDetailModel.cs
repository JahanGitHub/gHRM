using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public  class DBCompanyDetailModel
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyAddress { get; set; }

        public string CompanyEmail { get; set; }      

        public string CompanyPhone { get; set; }

        public string CompanyType { get; set; }      
               
             
    }
}
