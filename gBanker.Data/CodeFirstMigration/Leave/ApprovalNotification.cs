using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.ApprovalNotification")]
    public class ApprovalNotification
    {
        [Key]
        public long NotificationId { get; set; }
        public string ModuleName { get; set; } // Module Name (Leave Module Approval)
        public int ApprovalMasterId { get; set; }
        public int ApprovalDetailId { get; set; }
        public long ApproverId { get; set; }
        public long ApplicationId { get; set; }
        
        public bool IsChecked { get; set; }
        public string Remarks { get; set; } // Nvarchar(300)
        public string CheckedStatus { get; set; } //Approvae | Reject
        public DateTime? CheckedDate { get; set; }
        public bool? IsBackForward { get; set; }  //  If Forward Previous Step
   
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
