using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCaseCrimeLocationViewModel : BaseModel
    {
        public int DiscCaseCrimeLocationId { get; set; }//OfficeCode OfficeName rowSl
        public int CaseMasterId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public long rowSl { get; set; }

    }
}