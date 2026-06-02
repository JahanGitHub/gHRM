using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("inv.Tbl_Inv_GenerateNo")]
    public class Tbl_Inv_GenerateNo
    {       
        public int OfficeID { get; set; }
        public int NoOfRequest { get; set; }
        public string Prrefix { get; set; }
        public int GenerateNoOfYear { get; set; }        
    }
}
