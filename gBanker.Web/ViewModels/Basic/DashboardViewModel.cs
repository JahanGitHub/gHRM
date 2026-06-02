using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class HRDashboardViewModel
    {
        public int TotalBranches { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalEmployeesHO { get; set; }
        public int TotalEmployeesFO { get; set; }

        public int TotalEmployeesProbation { get; set; }
        public int TotalEmployeesHOProbation { get; set; }
        public int TotalEmployeesFOProbation { get; set; }

        public int TotalJoinedThisMonth { get; set; }
        public int TotalJoinedThisMonthHO { get; set; }
        public int TotalJoinedThisMonthFO { get; set; }
        public int TotalLeftThisMonth { get; set; }
        public int TotalLeftThisMonthHO { get; set; }
        public int TotalLeftThisMonthFO { get; set; }

        public int Present { get; set; }
        public int Leave { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }

        public int Tour { get; set; }


        public DateTime LastUpdated { get; set; }
        public Dictionary<string, int> GenderCountList { get; set; }
        public Dictionary<int, int> Leave6MonthsCountList { get; set; }

        public Dictionary<string, int> AttCountList { get; set; }
    }
}