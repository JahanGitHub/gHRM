namespace gHRM.Data.CodeFirstMigration.Promotion
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("promo.EmployeePromotionFail")]
    public partial class EmployeePromotionFail
    {
        [Key]
        public int Id { get; set; }        
        public string FailReason { get; set; }    
        public string SheetCreatedBy { get; set; }   
        public bool IsActive { get; set; }        
        public Int64 CreateUser { get; set; }        
        public DateTime CreateDate { get; set; }
    }
}
