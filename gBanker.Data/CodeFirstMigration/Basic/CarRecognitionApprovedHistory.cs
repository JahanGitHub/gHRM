namespace gHRM.Data.CodeFirstMigration.Basic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarRecognitionApprovedHistory")]
    public partial class CarRecognitionApprovedHistory
    {
        public int CarRecognitionApprovedHistoryId { get; set; }

        public int CarRecognitionId { get; set; }
        public int? ApprovalId { get; set; }

        public int? EmployeeId { get; set; }

        public bool IsApproved { get; set; }

        public bool IsActive { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CheckedStatus { get; set; }
        
        public long NotificationId { get; set; }


    }
}
