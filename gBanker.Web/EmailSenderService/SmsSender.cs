using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace gHRM.Web.EmailSenderService
{
    public class SmsSender
    {
        public void SendSMS()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.192.192.233:8082");
                var result = client.PostAsync("/api/SMSCore/SendSMS", new
                {
                    EndUserId = "576833958096399122",
                    Reply = "Test Api"
                }, new JsonMediaTypeFormatter()).Result;
                if (result.IsSuccessStatusCode)
                {
                    var msg = "Performance instance successfully sent to the API";
                    //Console.writeLine("Performance instance successfully sent to the API");
                }
                else
                {
                    string content = result.Content.ReadAsStringAsync().Result;
                   // Console.WriteLine("oops, an error occurred, here's the raw response: {0}", content);
                }
            }
        }

    }
}
