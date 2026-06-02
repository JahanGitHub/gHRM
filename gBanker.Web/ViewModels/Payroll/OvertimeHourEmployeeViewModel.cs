using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels.Payroll
{
    public class OvertimeHourEmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public decimal TotalWorkHour { get; set; }
        public decimal TotalOTHour { get; set; }
        public decimal TotalOTAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsSendForApproval { get; set; }

        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> YearList { get; set; }
    }
}