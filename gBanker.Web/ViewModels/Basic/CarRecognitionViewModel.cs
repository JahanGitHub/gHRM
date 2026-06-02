using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Basic
{
    public class CarRecognitionViewModel : BaseModel
    {
        public int CarRecognitionId { get; set; }
        public int EmployeeId { get; set; }
        public string CarNo { get; set; }
        public DateTime? CarRecognitionDate { get; set; }
        public DateTime? CarRecognitionTimeFrom { get; set; }
        public DateTime? CarRecognitionTimeTo { get; set; }
        public decimal? Distance { get; set; }
        public string ApprovedCarNo { get; set; }
        public int? ApprovedDriverId { get; set; }
        public string Purpose { get; set; }
        public string CRD { get; set; }
        public string CRTF { get; set; }
        public string CRTT { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
        public int RowSl { get; set; }

        // history
        public int CarRecognitionApprovedHistoryId { get; set; }
        public int ApprovalId { get; set; }
        public bool IsApproved { get; set; }
        // notification
        public long? NotificationId { get; set; }
        public string CheckedStatus { get; set; }




    }
}