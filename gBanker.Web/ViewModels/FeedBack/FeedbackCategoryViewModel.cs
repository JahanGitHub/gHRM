using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.FeedBack
{
    public class FeedbackCategoryViewModel : BaseModel
    {
        public int FeedbackCategoryID { get; set; }

        [Column("FeedbackCategory")]
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