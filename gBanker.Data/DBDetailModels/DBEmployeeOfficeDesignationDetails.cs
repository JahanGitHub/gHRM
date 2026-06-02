using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public class DBEmployeeOfficeDesignationDetails
    {
        public long EmpOfficeDesigId { get; set; }

        public long EmployeeId { get; set; }

        public int OfficeDesignationId { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeDesignationName { get; set; }
        public string SartDate { get; set; }

        public string EndDate { get; set; }

        public int? Duration { get; set; }
    }
}
