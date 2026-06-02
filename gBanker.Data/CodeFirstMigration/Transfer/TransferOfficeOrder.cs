using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("trns.TransferOfficeOrder")]
    public partial class TransferOfficeOrder
    {
        [Key]
        public int CCForOfficeOrderId { get; set; }

        public string CCForOfficeOrderName { get; set; }
        public string CCForOfficeOrderNameView { get; set; }

        public int? ViewOrder { get; set; }

        public bool IsActive { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public string ReportPlacementType { get; set; }
    }
}
