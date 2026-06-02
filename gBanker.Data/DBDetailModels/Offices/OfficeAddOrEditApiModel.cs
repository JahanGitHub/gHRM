using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Offices
{
    public class OfficeAddOrEditApiModel
    {
        public int id { get; set; }
        public int apiOfficeId { get; set; }
        public string name { get; set; }
        public string centerCode { get; set; }
        public string address { get; set; }
        public string firstLevel { get; set; }
        public string secondLevel { get; set; }
        public string thirdLevel { get; set; }
        public string fourthLevel { get; set; }
        public int? officeTypeId { get; set; }
        public int? officeLevel { get; set; }

        public Boolean? active { get; set; }
    }
}
