using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public  class DBEmployeeDepartmentDetailModel
    {
      public int DepartmentId { get; set; }
      public int OfficeTypeId { get; set; }
      public string DepartmentCode { get; set; }
      public string DepartmentName { get; set; }
      public string DepartmentShortName { get; set; }
      public int? CompanyId { get; set; }
      public string OfficeTypeName { get; set; }
    }
}
