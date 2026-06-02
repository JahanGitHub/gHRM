namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SSORoleMapping")]
    public partial class SSORoleMapping
    {
       [Key]
        public int Id { get; set; }

        [Display(Name = "Role Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int RoleId { get; set; }

        [Display(Name = "S S O Role Id")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string SSORoleId { get; set; }

        [Display(Name = "S S O Idof Client")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string SSOIdofClient { get; set; }

        [Display(Name = "S S O Client Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string SSOClientName { get; set; }

        [Display(Name = "S S O Role Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string SSORoleName { get; set; }

        [Display(Name = "Client Role")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool ClientRole { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
