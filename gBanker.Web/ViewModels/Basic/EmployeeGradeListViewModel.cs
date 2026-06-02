using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class EmployeeGradeListViewModel
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string GradeDescription { get; set; }
        public decimal InitialAmount { get; set; }
        public string RatioOn { get; set; }
        public decimal Percentage { get; set; }
        public decimal AmountPerIncrement { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public string ComponentName { get; set; }

    }
}