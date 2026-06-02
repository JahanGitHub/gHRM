using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class LeaveSellAdviseInfoModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBng { get; set; }
        public string Zone { get; set; }
        public string DMC { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
        public string LeaveSellNo { get; set; }
        public string EncashedAmount { get; set; }
        public int TatalDays { get; set; }
        public string SaleDate { get; set; }
        public string RequestDate { get; set; }
        public string ApprovedDate { get; set; }
        public string Remarks { get; set; }

    }
}
