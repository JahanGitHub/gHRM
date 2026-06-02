namespace gHRM.Data.CodeFirstMigration.Basic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarRecognitionApproval")]
    public partial class CarRecognitionApproval
    {
        [Key]
        public int ApprovalId { get; set; }

        public int? EmployeeId { get; set; }

        public int ApprovalLevel { get; set; }

        public bool IsActive { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
