using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class DBOfficeDetailModel
    {
        public int SlNo { get; set; }
        public int OfficeID { get; set; }
        public int? OfficeTypeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int OfficeLevel { get; set; }
        public string FirstLevel { get; set; }
        public string SecondLevel { get; set; }
        public string ThirdLevel { get; set; }
        public string FourthLevel { get; set; }
        public System.DateTime OperationStartDate { get; set; }
        public string OfficeAddress { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> GeoLocationID { get; set; }
        public string LocationName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int TotalCount { get; set; }
    }
}
