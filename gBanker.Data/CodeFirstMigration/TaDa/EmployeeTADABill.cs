using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.TaDa
{
    [Table("tada.EmployeeTADABill")]
    public class EmployeeTADABill
    {
        [Key]
        public int TADABillId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int MemoNo { get; set; }
        public DateTime TravelDate { get; set; }
        public string TravelPlace { get; set; }
        public string TravelPurpose { get; set; }
        public DateTime ApproveDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal ApproveAmount { get; set; }
        public bool? IsAmountPaid { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
