using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class NomineeDetailViewModel : BaseModel
    {
        public long NomineeDetailId { get; set; }

        public int NomineeMasterId { get; set; }

        public string NomineeType { get; set; }

        public string NomineeName { get; set; }

        public string NomineeAddress { get; set; }

        public int? NomineeAge { get; set; }

        public string NomineeRelation { get; set; }

        public decimal? NomineePercentage { get; set; }

        public string NomineeNationalId { get; set; }

        public byte[] NomineeImage { get; set; }

        public string NomineeRemarks { get; set; }
    }
}