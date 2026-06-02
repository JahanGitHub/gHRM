using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace gHRM.Web.Helpers
{
    public class EmailHelper
    {
        /* Global instance of the scopes required by this quickstart.
         If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = { GmailService.Scope.GmailSend };
        static string ApplicationName = "Gmail API for gHRMPLus";
        GmailService _GmailService;

        public EmailHelper(HttpContextBase Context)
        {
            UserCredential credential;
            // Load client secrets.
            using (var stream =
                   new FileStream(Context.Server.MapPath("~") + "/App_Data/google-credentials.json", FileMode.Open, FileAccess.Read))
            {
                /* The file token.json stores the user's access and refresh tokens, and is created
                 automatically when the authorization flow completes for the first time. */
                string credPath = Context.Server.MapPath("~") + "/App_Data/google-token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Gmail API service.
            _GmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public EmailHelper()
        {
            UserCredential credential;
            // Load client secrets.
            using (var stream =
                   new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~") + "/App_Data/google-credentials.json", FileMode.Open, FileAccess.Read))
            {
                /* The file token.json stores the user's access and refresh tokens, and is created
                 automatically when the authorization flow completes for the first time. */
                string credPath = System.Web.Hosting.HostingEnvironment.MapPath("~") + "/App_Data/google-token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Gmail API service.
            _GmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
        public bool SendMailUsingCompanyMail(string Subject, string Body, string To, string cc, out string Message)
        {
            Message = "";
            try
            {
                MailMessage mail = new MailMessage();
                var from = WebConfigurationManager.AppSettings["messageFrom"];
                mail.From = new MailAddress(from, SessionHelper.CompanyName);
                if (To != null)
                    mail.To.Add(new MailAddress(To, ""));
                if (cc != null)
                    mail.CC.Add(cc);

                mail.Subject = Subject;
                mail.Body = Body;

                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                var smtpHost = WebConfigurationManager.AppSettings["smtpHost"];
                if (string.IsNullOrEmpty(smtpHost))
                    smtpHost = "smtp.gmail.com";
                smtp.Host = smtpHost;
                smtp.UseDefaultCredentials = false;
                var uid = WebConfigurationManager.AppSettings["credentialID"];
                var pass = WebConfigurationManager.AppSettings["credentialPassword"];
                smtp.Credentials = new System.Net.NetworkCredential(uid, pass);

                smtp.Port = 587;
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
        public bool SendMail(string Subject, string Body, string To, out string Message)
        {
            Message = "";
            try
            {
                string msgStr = "" +
                    "To:" + To + "\r\n" +
                    "Subject:" + Subject + "\r\n" +
                    "Content-Type: text/html; charset=us-ascii" + "\r\n\r\n" +
                    Body;
                var result = _GmailService.Users.Messages.Send(new Message
                {
                    Raw = Base64UrlEncode(msgStr)
                }, "me").Execute();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool SendMail(string Subject, string Body, string To, string cc, out string Message)
        {
            Message = "";
            try
            {
                //var mailMessage = new System.Net.Mail.MailMessage();
                //mailMessage.To.Add(To);
                //mailMessage.CC.Add(cc);
                //mailMessage.Subject = Subject;
                //mailMessage.Body = Body;
                //mailMessage.IsBodyHtml = true;
                string msgStr = "" +
                    "To:" + To + "\r\n";
                if (!string.IsNullOrEmpty(cc))
                    msgStr += "CC:" + cc + "\r\n";
                msgStr += "Subject:" + Subject + "\r\n" +
                    "Content-Type: text/html; charset=us-ascii" + "\r\n\r\n" +
                    Body;

                var result = _GmailService.Users.Messages.Send(new Message
                {
                    Raw = Base64UrlEncode(msgStr)
                }, "me").Execute();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        private string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}