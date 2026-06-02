using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCasePunishmentDetailViewModel : BaseModel
    {
        public int PunishmentDetailId { get; set; }
        public int PunishmentMasterId { get; set; }
        public int CaseMasterId { get; set; }
        public int CrimeId { get; set; }
    }
}