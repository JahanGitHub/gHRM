using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class DBThanaDetailsModel
    {


        public int thana_id { get; set; }

        public int? district_id { get; set; }

       
        public string thana_code { get; set; }
        public string district_name_eng { get; set; }

   
        public string thana_name_eng { get; set; }

        public string thana_name_bng { get; set; }
      
    }
}
