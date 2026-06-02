using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Discipline
{
    public class View_PunishmentDetail
    {
        public int? RowSl { get; set; }
        public int PunishmentMasterId { get; set; }
        public long EmployeeId { get; set; }
        public int PunishmentId { get; set; }

        [Display(Name = "Punishment Date")]
        public DateTime? PunishmentDate { get; set; }

        [Display(Name = "Punishment Dispatch Number")]
        public string PunishmentDispatchNumber { get; set; }
        public int DaysLose { get; set; }
        public int CaseMasterId { get; set; }

        [Display(Name = "Case No")]
        public string CaseNo { get; set; }

        [Display(Name = "Case Date From")]
        public string CaseDateFrom { get; set; }

        [Display(Name = "Case Date To")]
        public DateTime? CaseDateTo { get; set; }
        public DateTime? AuditFrom { get; set; }
        public DateTime? AuditTo { get; set; }
        public string CaseType { get; set; }
        public string CaseTypeName { get; set; }
        public string CaseDescription { get; set; }
        public string CaseMasterRemarks { get; set; }
        public string CaseDispatchNumber { get; set; }
        public DateTime? CrimeDateFrom { get; set; }
        public DateTime? CrimeDateTo { get; set; }

        [Display(Name = "Annexation Amount")]
        public decimal? AnnexationAmount { get; set; }

        [Display(Name = "Return Amount")]
        public decimal ReturnAmount { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CrimeCode { get; set; }
        public string CrimeName { get; set; }
       
    }    
}
