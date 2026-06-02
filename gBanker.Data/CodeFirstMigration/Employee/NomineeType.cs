using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("NomineeType")]
    public class NomineeType
    {
        [Key]
        public int NomineeTypeId { get; set; }
        public int ViewOrder { get; set; }
        public string NomineeTypeName { get; set; }

        public string NomineeTypeValue { get; set; }

        public bool IsActive { get; set; }

        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
