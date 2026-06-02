using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Cooperative
{
    public class DBEmployeeWiseCooperativeConfigurationDetailModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PhoneNo { get; set; }
        public decimal? GrossSalary { get; set; }
        public int CooperativeConfigurationId { get; set; }
        public decimal CollectionAmount { get; set; }
        public int? CollectionYear { get; set; }
        public int? CollectionMonth { get; set; }

        public bool? IsActive { get; set; }
    }
}
