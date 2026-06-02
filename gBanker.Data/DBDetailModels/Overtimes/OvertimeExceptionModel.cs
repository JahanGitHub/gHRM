using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Overtimes
{
   public  class OvertimeExceptionModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }        
        public string ExceptionType { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }       
        public DateTime? InActiveDate { get; set; }
        public long CreateUser { get; set; }        
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }     
        public DateTime? UpdateDate { get; set; }

        //additional
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
       
        //public string EffectiveStartDateInString => EffectiveStartDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        //public string EffectiveEndDateInString => EffectiveEndDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

    }
}
