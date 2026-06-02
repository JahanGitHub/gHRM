using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class Att_OfficeMachineViewModel : BaseModel
    {
        public int OfficeMachineId { get; set; }
        public string MachineName { get; set; }
        public int OfficeId { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}