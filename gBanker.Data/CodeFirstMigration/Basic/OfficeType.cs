using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("OfficeType")]
    public partial class OfficeType
    {
        public int OfficeTypeId { get; set; }

        [StringLength(10)]
        public string OfficeTypeCode { get; set; }

        [StringLength(100)]
        public string OfficeTypeName { get; set; }

        [StringLength(50)]
        public string OfficeShortName { get; set; }
        public int? OfficeTypeLevel { get; set; }
        public bool IsActive { get; set; }
        
    }
}
