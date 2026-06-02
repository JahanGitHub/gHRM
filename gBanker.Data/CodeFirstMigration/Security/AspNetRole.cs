namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleModules = new HashSet<AspNetRoleModule>();
            AspNetUsers = new HashSet<AspNetUser>();
        }
        
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(500)]
        public string DefaultLinkURL { get; set; }

        #region new Changes

        public bool? IsActive { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        #endregion

        public virtual ICollection<AspNetRoleModule> AspNetRoleModules { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
