using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.TaDa
{
    [Table("tada.TADAPurpose")]
    public class TADAPurpose
    {
        [Key]
        public int Id { get; set; }       
        public string Purpose { get; set; }      
        public string Remarks { get; set; }
        public bool IsActive { get; set; }        
        public DateTime? InActiveDate { get; set; }
        public long CreateUser { get; set; }        
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }        
        public DateTime? UpdateDate { get; set; }        
    }
}
