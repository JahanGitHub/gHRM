namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccChart")]
    public partial class AccChart
    {
        [Display(Name = "Acc Chart I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 AccChartID { get; set; }

        [Display(Name = "Account Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(25, ErrorMessage = "Maximum length is {1}")]
        public string AccountCode { get; set; }

        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string AccountName { get; set; }
    }
}
