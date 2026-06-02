using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Security
{
    public class SSORegisterModel
    {
        public string firstName { get; set; }

        public string lastName { get; set; }
        public bool enabled { get; set; }
        public string email { get; set; }
        public string username { get; set; }

        public List<CredentialModel> credentials { get; set; }
        public string[] clientRoles { get; set; }
        public string[] realmRoles { get; set; }
    }

    public class InactiveAuthUserModel
    {
        public string id { get; set; }
        public bool enabled { get; set; }
        public string username { get; set; }
    }
    
    public class AuthUserChangePasswordModel
    {
        public string id { get; set; }       
        public string username { get; set; }
        public List<CredentialModel> credentials { get; set; }
    }

    public class CredentialModel
    {
        public string value { get; set; }
        public bool temporary { get; set; }
    }

    public class AuthRoleMappingRequestModel
    {
        public bool ClientRole { get; set; }
        public string IdOfUser { get; set; }
        public string IdOfClient { get; set; }
        public List<AuthRoleMappingModel> AuthRoles { get; set; }
        public string AccessToken { get; set; }
    }

    public class AuthRoleMappingModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
