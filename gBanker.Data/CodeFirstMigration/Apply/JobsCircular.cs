using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Apply
{
    [Table("apply.JobsCircular")]
    public partial class  JobsCircular
    {
        public JobsCircular()
        {

        }
        [Key]
        public Int64 JobId { get; set; }

        [Display(Name = "Post Name")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PostName { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Post Description")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PostDescription { get; set; }

        [Display(Name = "Pdf Byte")]
        public byte[] PdfByte { get; set; }

        public Int64? CreatedBy { get; set; }


    }
}
