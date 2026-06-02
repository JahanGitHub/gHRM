using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class ApprovalNotificationViewModel :BaseModel
    {
        public long NotificationId { get; set; }
        public string ModuleName { get; set; } // Module Name (Leave Module Approval)
        public int ApprovalMasterId { get; set; }
        public int ApprovalDetailId { get; set; }
        public bool? IsChecked { get; set; }
        public string CheckedStatus { get; set; }
        public DateTime? CheckedDate { get; set; }
        public bool? IsBackForward { get; set; }  //  If Forward Previous Step (any cnage.)
        public string Remarks { get; set; }
    }
}