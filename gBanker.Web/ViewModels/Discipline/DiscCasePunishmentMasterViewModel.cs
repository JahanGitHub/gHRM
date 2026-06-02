using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Discipline
{
    public class DiscCasePunishmentMasterViewModel : BaseModel
    {
        public int PunishmentMasterId { get; set; }
        public long EmployeeId { get; set; }
        public int PunishmentId { get; set; }
        public string Remarks { get; set; }
    }
}