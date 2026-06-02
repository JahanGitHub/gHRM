using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
   public  class DBDistrictDetailModel
    {
        public int district_id { get; set; }      
        public string district_code { get; set; }
        public string district_name_eng { get; set; }
        public int StateOrProvinceId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
