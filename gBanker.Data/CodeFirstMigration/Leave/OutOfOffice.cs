using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.OutOfOffice")]
    public partial class OutOfOffice
    {
        [Key]
        public int OutofOfficeId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public long CreateUser { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }


    }
}
