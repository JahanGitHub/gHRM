using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public  class DBStateOrProvinceOrDivisionDetailModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryShortCode { get; set; }
        public int StateOrProvinceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
