using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Data.DBDetailModels.Security
{
    public class SSOLoginUserModel
    {
        public string sub { get; set; }
        public string preferred_username { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public bool email_verified { get; set; }

    }

    public class SSOAuthUserDetailModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool enabled { get; set; }
        public bool emailVerified { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<SSOAuthUserAccessDetailModel> access { get; set; }

    }

    public class SSOAuthUserAccessDetailModel
    {
        public bool manageGroupMembership { get; set; }
        public bool view { get; set; }
        public bool impersonate { get; set; }
        public bool mapRoles { get; set; }
        public bool manage { get; set; }

    }
}