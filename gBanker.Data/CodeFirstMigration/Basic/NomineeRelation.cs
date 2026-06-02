using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("NomineeRelation")]
    public class NomineeRelation
    {
        [Key]
        public int RelaitonId { get; set; }
        public string RelationName { get; set; }
        public string RelationNameOther { get; set; }
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
