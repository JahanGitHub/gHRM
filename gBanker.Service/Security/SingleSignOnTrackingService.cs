#region Using

using gHRM.Service;
using System;
using System.Configuration;
using System.Web;

#endregion

namespace gHRM.Service
{
    public class SingleSignOnTrackingService : ISingleSignOnTrackingService
    {
        #region Private Members
      
        private readonly HttpContextBase _httpContext;      
        private const int IdentifierCookieExpirationDays = 7;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext"></param>
        public SingleSignOnTrackingService(HttpContextBase httpContext)
        {            
            _httpContext = httpContext;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get single sign on identifier
        /// </summary>
        /// <returns></returns>
        public string GetSingleSignOnIdentifier()
        {
            var identityLoginIdentifier = "";

            try
            {
                var trackingCookieName = ConfigurationManager.AppSettings["gHRM.Cookie.SingleSignOn.Tracking"];

                var identityLoginIdentifierCookie = _httpContext.Request.Cookies.Get(trackingCookieName);

                if (identityLoginIdentifierCookie == null)
                    return "";

                identityLoginIdentifier = identityLoginIdentifierCookie.Expires>=DateTime.Now && !string.IsNullOrWhiteSpace(identityLoginIdentifierCookie?.Value)? identityLoginIdentifierCookie?.Value:string.Empty;
            }
            catch (Exception ex)
            {
                identityLoginIdentifier = "";
            }

            return identityLoginIdentifier;
        }


        /// <summary>
        /// Track single sign on
        /// </summary>
        /// <param name="loginIdentifier"></param>
        public void TrackSingleSignOn(string loginIdentifier)
        {
            try
            {
                var trackingCookieName = ConfigurationManager.AppSettings["gHRM.Cookie.SingleSignOn.Tracking"];

                var identityLoginIdentifierCookie = _httpContext.Request.Cookies.Get(trackingCookieName) ??
                                           new HttpCookie(trackingCookieName)
                                           {
                                               HttpOnly = false,
                                               Secure = false
                                           };

                identityLoginIdentifierCookie.Values.Clear();
                identityLoginIdentifierCookie.Value = loginIdentifier;
                identityLoginIdentifierCookie.Expires = DateTime.Now.AddDays(IdentifierCookieExpirationDays);               
                _httpContext.Response.Cookies.Set(identityLoginIdentifierCookie);
            }
            catch (Exception ex)
            {  
                              
            }
        }
        
        /// <summary>
        /// Remove single sign on identifier
        /// </summary>       
        public void RemoveSingleSignOnIdentifier()
        {
            try
            {
                var trackingCookieName = ConfigurationManager.AppSettings["gHRM.Cookie.SingleSignOn.Tracking"];

                var identityLoginIdentifierCookie = _httpContext.Request.Cookies.Get(trackingCookieName) ??
                                           new HttpCookie(trackingCookieName)
                                           {
                                               HttpOnly = false,
                                               Secure = false
                                           };

                identityLoginIdentifierCookie.Values.Clear();
                identityLoginIdentifierCookie.Value = null;
                identityLoginIdentifierCookie.Expires = DateTime.Now.AddDays(-IdentifierCookieExpirationDays);
                _httpContext.Response.Cookies.Set(identityLoginIdentifierCookie);

                _httpContext.Response.Cookies.Set(identityLoginIdentifierCookie);
            }
            catch (Exception ex)
            {   
                            
            }
        }       

        #endregion       
    }
}
