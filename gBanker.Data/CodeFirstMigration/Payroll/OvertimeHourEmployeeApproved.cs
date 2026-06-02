using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.OvertimeHourEmployeeApproved")]
    public class OvertimeHourEmployeeApproved
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal? TotalWorkHour { get; set; }
        public decimal? TotalOTHour { get; set; }
        public decimal? TotalOTAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPaid { get; set; }
        //public bool? IsReject { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

