//Created by Mansur 14-11-2016 for Entry HRM Feedback Register as per Ataur Bhai's Reuirment with Morshed Bhai
namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedbackCategory")]
    public partial class FeedbackCategory
    {
        public int FeedbackCategoryID { get; set; }

        [Required]
        [StringLength(255)]
        public string FeedbackCategoryName { get; set; }

        [Required]
        [StringLength(50)]
        public string FeedbackCategoryType { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}