using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class DBEmployeeDetailsModel
    {
        public long EmployeeId { get; set; }
        public int? OfficeId { get; set; }
        public int OfficeTypeId { get; set; }

        public string OfficeName { get; set; }
        public string BatchNo { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBng { get; set; }
        public string EmployeeStatus { get; set; }
        public string Gender { get; set; }
        public string ContactNo1 { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string EmployeeRank { get; set; }
        
    }
}
