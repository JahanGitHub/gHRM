using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("DocumentType")]
    public partial class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string TypeName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
        
        public string DocumentTypeModuleName { get; set; }
    }
}
