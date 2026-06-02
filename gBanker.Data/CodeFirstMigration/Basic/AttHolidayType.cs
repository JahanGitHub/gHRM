using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("AttHolidayType")]
    public class AttHolidayType
    {
        [Key]
        public int AttHolidayTypeId { get; set; }
        public string HolidayTypeShortName { get; set; }
        public string HolidayTypeFullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
