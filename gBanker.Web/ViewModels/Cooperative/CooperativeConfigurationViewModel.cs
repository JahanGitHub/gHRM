using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes;

namespace gHRM.Web.ViewModels.Cooperative
{
    public class CooperativeConfigurationViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Emp. Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Emp. Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Salary Component")]
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        [Display(Name = "Installment Amt.")]
        public int MonthlyInstallment { get; set; }

        
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        
        public List<SelectListItem>  ComponentLst { get; set; }

    }
}