using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gHRM.Data.CodeFirstMigration.EmployeePromotion
{
    [Table("promo.EmployeePromotion")]
    public partial  class EmployeePromotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PromotionId { get; set; }
        
        public long EmployeeId { get; set; }

        public int DesignationId { get; set; }

        public int PromotionTypeId { get; set; }

        public DateTime? PromotionDate { get; set; }

        public DateTime? NextReviewDate { get; set; }

        public string Remarks { get; set; }


        
        public bool IsReviewed { get; set; }

        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string PromotionStatus { get; set; }
        public DateTime? PromotionEffectDate { get; set; }
        public int? AssessmentYear { get; set; }
        public int? Score { get; set; }
        

    }// End Class
}// End Namespace
