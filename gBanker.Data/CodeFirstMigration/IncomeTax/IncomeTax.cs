namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IncomeTax")]
    public partial class IncomeTax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public long EmployeeID { get; set; }

        [Required]
        public int OfficeID { get; set; }

        [StringLength(50)]
        public string NationalID { get; set; }

        [StringLength(50)]
        public string TIN { get; set; }

        [StringLength(100)]
        public string ReturnRegisterSlNo { get; set; }

        [StringLength(100)]
        public string ReturnRegisterVolNo { get; set; }

        [StringLength(100)]
        public string ReturnFillingDate { get; set; }

        [Required]
        [StringLength(100)]
        public string FiscalYear { get; set; }

        [StringLength(100)]
        public string Circle { get; set; }

        [StringLength(100)]
        public string TaxArea { get; set; }

        [StringLength(100)]
        public string TotalIncome { get; set; }

        [StringLength(100)]
        public string TotalTaxPaid { get; set; }

        [StringLength(150)]
        public string FileLocation { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        // ✅ Soft delete flag
        [Required]
        public bool isActive { get; set; }

        // ✅ Add these properties for update tracking
        public DateTime? UpdateDate { get; set; }

        public long? UpdateUser { get; set; }
    }
}
