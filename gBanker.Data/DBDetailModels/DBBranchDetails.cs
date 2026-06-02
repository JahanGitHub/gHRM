using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels
{
  public class DBBranchDetails
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchEmail { get; set; }
        public string BranchPhone { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

    }
}
