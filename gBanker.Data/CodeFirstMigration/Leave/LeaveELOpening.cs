
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveELOpening")]
    public partial class LeaveELOpening
    {
        [Key]
        public int ELOpeningId { get; set; }

        public long EmployeeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime LeaveStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime LeaveEndDate { get; set; }

        public int ELFull { get; set; }

        public int EnjoyFull { get; set; }

        public int BalanceFull { get; set; }

        public int ELHalf { get; set; }

        public int EnjoyHalf { get; set; }

        public int BalanceHalf { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastSaleDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; } 
    }
}
