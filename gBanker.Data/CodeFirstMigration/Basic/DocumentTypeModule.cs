using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("DocumentTypeModule")]
    public class DocumentTypeModule
    {
        public int DocumentTypeModuleId { get; set; }
        public string DocumentTypeModuleName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long CreateBy { get; set; }

        public long UpdateBy { get; set; }
    }
}
