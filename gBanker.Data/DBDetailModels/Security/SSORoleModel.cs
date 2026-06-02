using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Data.DBDetailModels.Security
{
    public class SSORoleModel
    {
        public bool clientRole { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }

    public class SSOFetchRoleRequestModel
    {
        public bool IsClientRole { get; set; }
        public string RoleName { get; set; }
        public string AppClientId { get; set; }
        public string AccessToken { get; set; }

    }

    public class SSORoleRequestModel
    {
        public SSORoleModel SSORole { get; set; }
        public List<string> AppClientIDs { get; set; }
        public string AccessToken { get; set; }

    }
    public class SSORoleDetailModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public string AuthIdOfClient { get; set; }
        public string AuthClientId { get; set; }
    }

}