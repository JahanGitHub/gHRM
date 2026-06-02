using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Data.DBDetailModels.Security
{
    public class KeyCloakTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int not_before_policy { get; set; }
        public string session_state { get; set; }
        public string scope { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }

    public class KeyCloakResponse
    {       
        public string errorMessage { get; set; } //User exists; http-status: 409
        public string error { get; set; } //un-authorized; http-status code: 401
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}