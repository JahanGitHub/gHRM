using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Infrastucture.Framework
{
    public interface IWorkContext
    {
        /// <summary>
        /// Is Authenticated
        /// </summary>
        bool IsAuthenticated { get; set; }

        /// <summary>
        /// Is Session Exist By Key
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        bool IsSessionExistByKey(string sessionKey);

        /// <summary>
        /// Set Current User Session
        /// </summary>
        bool SetCurrentUserSession();
    }
}