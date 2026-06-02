using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
    public class BulkLeaveEncashmentModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Amt { get; set; }
    }
}
