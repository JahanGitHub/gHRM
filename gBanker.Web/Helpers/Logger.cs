using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Helpers
{
    public interface ILogger
    {
        void LogRequest(ApplicationLog applicationLog);
    }
    public class Logger : ILogger
    {
        private readonly IApplicationLogService applictionLogService;
        public Logger(IApplicationLogService applictionLogService)
        {
            this.applictionLogService = applictionLogService;
        }
        public void LogRequest(ApplicationLog applicationLog)
        {
            if (IsLoggingEnabled())
                applictionLogService.Create(applicationLog);
        }
        public static ApplicationLog GetLogObject()
        {
            //string clientIp = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            //string ip = clientIp;
            //string localIP;
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            //ip = localEndPoint.Address.ToString();
            //using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            //{

            //    //socket.Connect("8.8.8.8", 65530);
            //    //IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            //    //ip = endPoint.Address.ToString();
            //}

            var context = HttpContext.Current;
            var ip = context.Request.UserHostAddress;
            if (context.Request.IsLocal)
                ip = "127.0.0.1";
            var url = context.Request.Url;
            var userAgent = context.Request.UserAgent;
            //generate request data:

            var request = "";
            var form = context.Request.Form;
            var user = context.Request.IsAuthenticated ? HttpContext.Current.User.Identity.Name : "";
            if (form != null)
            {
                foreach (string k in form.Keys)
                {
                    if (!k.ToLower().Contains("password") && k != "__RequestVerificationToken") //exclude password to save in log...
                        request = request + k + ": " + form[k] + " ";
                }
            }
            var qs = "";
            if (context.Request.QueryString != null)
            {
                foreach (string k in context.Request.QueryString.Keys)
                {
                    if (!k.ToLower().Contains("password"))
                        qs = qs + k + ": " + context.Request.QueryString[k] + " ";
                }
            }

            var sessionId = HttpContext.Current.Session.SessionID;
            var rd = HttpContext.Current.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            var method = HttpContext.Current.Request.HttpMethod;

            var log = new ApplicationLog() { SessionId = sessionId, ActionName = currentAction, ControllerName = currentController, HttpMethod = method, UserAgent = userAgent, QueryStringParams = qs, RequestDetail = request, Status = "A", RequestUser = user, ActionURL = url.AbsoluteUri, ClientIP = ip, CreateDate = DateTime.Now, LogDate = DateTime.Now };
            return log;
        }


        public static bool IsLoggingEnabled()
        {
            //DisableLogging
            try
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLogging"]);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}