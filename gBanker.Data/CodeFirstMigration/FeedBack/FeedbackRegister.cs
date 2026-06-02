//Created by Mansur 14-11-2016 for Entry HRM Feedback Register as per Ataur Bhai's Reuirment with Morshed Bhai
namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedbackRegister")]
    public partial class FeedbackRegister
    {
        public long FeedbackRegisterID { get; set; }

        public int OfficeId { get; set; }

        public long EmployeeId { get; set; }

        public int FeedbackCategoryID { get; set; }

        [Required]
        [StringLength(255)]
        public string FeedbackDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime FeedbackDate { get; set; }

        public bool IsChecked { get; set; }
        public bool IsSolved { get; set; }

        [StringLength(50)]
        public string SolvedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SolvedDate { get; set; }

        public bool IsActive { get; set; }

        public string FileLocation { get; set; }

        public string FileLocationReply { get; set; }

        public string Remarks { get; set; }
        //public byte[] File_Attachment { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public int? UnitId { get; set; }
    }
}
