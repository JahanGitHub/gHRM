using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
   public class DBTimeScaleDetails
    {
        public long TimeScaleId { get; set; }    
        public long EmployeeId { get; set; }       
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long DesignationId { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string NatureOfTimeScale { get; set; }
        public DateTime TimeScaleDate { get; set; }
        public decimal? FixedPay { get; set; }
        public string OfficeMemo { get; set; }
        public DateTime? MemoDate { get; set; }
    }
}
