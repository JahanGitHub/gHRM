using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Filters.TimeKeepings
{
    public class TimeKeepingExceptionSearchFilter : BaseSearchFilter
    {
        public int AttendenceTypeId { get; set; }
        public DateTime? AttenDanceDate { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string Justification { get; set; }
        public int? CreateUser { get; set; }
    }
}
