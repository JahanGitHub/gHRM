using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Financial
{
    public class FinancialViewModel : BaseModel
    {
        /*
        public string PresentOffice { get; set; }
        public string CurrentWorkPlaceDuration { get; set; }
        public int district_id { get; set; }
        public string district_name_bng { get; set; }
        public long OfficeNoteNo { get; set; }
        public string ZoneName { get; set; }
        public long rowSl { get; set; }
        */

        public long Id { get; set; }
        public long rowSl { get; set; }
        public int OfficeID { get; set; }

        public int OrgId { get; set; }
        public string OrgName { get; set; }


        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string ZoneCode { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? DepositDraftAmount { get; set; }
        public string SlipDraftNo { get; set; }
        public string ReferenceNo { get; set; }
        public string SendedBy { get; set; }
        public string DepositSendDate { get; set; }
        public string Remarks { get; set; }

        public decimal? DeductionOthers { get; set; }

        public decimal? LBUNetTaka { get; set; }
        


    }// END Class
}// END Namespace