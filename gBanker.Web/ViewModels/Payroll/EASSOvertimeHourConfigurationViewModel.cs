using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class EASSOvertimeHourConfigurationViewModel
    {
        public int OTCalcId { get; set; }
        public int EASSDesignationId { get; set; }
        public int EASSCompanyId { get; set; }
        public bool IsOvertimeApplicable { get; set; }
        public decimal MaxOTHourPerMonth { get; set; }
        public decimal RateForOvertimeHour { get; set; }
        public decimal OvertimeHour { get; set; }
        public int CalculationRank { get; set; }
        public IEnumerable<SelectListItem> EASSDesignationNameList { get; set; }
        public IEnumerable<SelectListItem> EASSCompanyNameList { get; set; }
    }
}