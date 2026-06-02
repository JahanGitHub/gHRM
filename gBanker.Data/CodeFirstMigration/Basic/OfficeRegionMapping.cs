using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("OfficeRegionMapping")]
    public class OfficeRegionMapping
    {
        [Key]
        public int Id { get; set; }
        public int RegionId { get; set; }
        public int OfficeId { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
