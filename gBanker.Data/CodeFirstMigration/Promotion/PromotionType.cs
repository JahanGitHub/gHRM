namespace gHRM.Data.CodeFirstMigration.Promotion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("promo.PromotionType")]
    public partial class PromotionType
    {
        [Key]
        [Column(Order = 0)]
        public int PromotionTypeId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ViewOrder { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string PromotionTypeName { get; set; }

        [StringLength(5)]
        public string PromotionTypeValue { get; set; }

        [Key]
        [Column(Order = 3)]
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
