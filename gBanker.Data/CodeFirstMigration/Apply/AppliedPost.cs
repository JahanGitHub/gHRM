using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Apply
{
    [Table("apply.AppliedPost")]
    public partial class AppliedPost
    {
        public AppliedPost()
        {

        }
        [Key]
        public Int64 AppliedId { get; set; }  
        public Int64? JobId { get; set; }
        public Int64? ApplicantId { get; set; }
        public bool? IsActive { get; set; }
        public int? AlreadyApplied { get; set; }


    }
}
