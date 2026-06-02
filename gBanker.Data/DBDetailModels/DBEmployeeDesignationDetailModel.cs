using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public  class DBEmployeeDesignationDetailModel
    {
        public int DesignationId { get; set; }

        public string DesignationCode { get; set; }

        public string DesignationName { get; set; }

        public string DesignationShortName { get; set; }
        public string DesignationType { get; set; }
        public int SalaryScaleId { get; set; }

    }
}
