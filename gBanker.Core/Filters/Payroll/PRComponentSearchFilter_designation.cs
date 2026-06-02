using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Filters.Payroll
{
    public class PRComponentSearchFilter_designation : BaseSearchFilter
    {
        public string AndCondition { get; set; }    
        
        public int? DesignationId { get; set; }
    }
}
