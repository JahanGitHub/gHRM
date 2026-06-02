using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class InternalOrganizationViewModel : BaseModel
    {
        public int OrgId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationCode { get; set; }
        public bool IsActive { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}