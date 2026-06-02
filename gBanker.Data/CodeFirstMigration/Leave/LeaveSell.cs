using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveSell")]
    public partial class LeaveSell
    {
        public int LeaveSellId { get; set; }

        public string LeaveSellNo { get; set; }

        public long EmployeeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleDate { get; set; }

        public int TotalDays { get; set; }

        public decimal EncashedAmount { get; set; }

        public bool IsAuthorized { get; set; }

        public bool IsApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string AnulipiTxt { get; set; }

        public int? OrderCreateOfficeId { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public bool IsAmountPaid { get; set; }

        public bool? IsPaidWithSalary { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string LeaveHeader { get; set; }

        public string LeaveFooter { get; set; }

        public string Remark { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public bool IsBulkEncashed { get; set; }
        public bool IsManualLeaveSellForInactive { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
