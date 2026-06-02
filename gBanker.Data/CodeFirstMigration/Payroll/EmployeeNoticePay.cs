using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeNoticePay")]
    public class EmployeeNoticePay
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public int NoticePayConfigId { get; set; }
		public DateTime ProcessDate { get; set; }
		public decimal GeneratedAmount { get; set; }
		public decimal Amount { get; set; }
		public DateTime InformDate { get; set; }
		public DateTime ResignDate { get; set; }
        public int NoticePeriod { get; set; }
        public int NoticeGiven { get; set; }
		public bool IsCalcFromBasic { get; set; }
		public decimal SalaryAmount { get; set; }
        public int SalaryPer { get; set; }
		public bool IsActive { get; set; }
		public bool IsSendForApproval { get; set; }
		public bool IsRejected { get; set; }
		public bool IsApproved { get; set; }
		public long? ApprovedBy { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public long CreateUser { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
