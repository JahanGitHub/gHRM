using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace gHRM.Data.CodeFirstMigration.Basic
{
    [Table("CarRecognition")]
    public partial class CarRecognition
    {
        [Key]
        public int CarRecognitionId { get; set; }
        public int EmployeeId { get; set; }

        [StringLength(50)]
        public string CarNo { get; set; }

        public DateTime? CarRecognitionDate { get; set; }

        public DateTime? CarRecognitionTimeFrom { get; set; }

        public DateTime? CarRecognitionTimeTo { get; set; }

        public decimal? Distance { get; set; }

        public string Purpose { get; set; }
        public string ApprovedCarNo { get; set; }
        public int? ApprovedDriverId { get; set; }
        public bool IsActive { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
