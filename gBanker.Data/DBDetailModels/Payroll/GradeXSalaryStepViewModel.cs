using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Data.DBDetailModels.Payroll
{
    public class GradeXSalaryStepViewModel
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int StepFrom { get; set; }
        public int StepTo { get; set; }
        public string RatioOn { get; set; }
        public int AmountOrPercent { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string GradeName { get; set; }
    }
}