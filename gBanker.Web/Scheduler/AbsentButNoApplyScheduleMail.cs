using gHRM.Data.CodeFirstMigration;
using gHRM.Web.EmailSenderService;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.TimeKeeping;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Scheduler
{
    public class AbsentButNoApplyScheduleMail : IJob
    {
        private readonly EmailSender emailSenderService;
        public AbsentButNoApplyScheduleMail()
        {
            emailSenderService = new EmailSender();
        }
        // After 5 PM
        public void Execute(IJobExecutionContext context)
        {
            if (AppSetting.GetBool(AppSetting.AbsentScheduleEnable, null))
            {
                DayOfWeek dow = DateTime.Now.DayOfWeek;
                if (dow.ToString().ToLower() != "friday" || dow.ToString().ToLower() != "saturday")
                {
                    using (gHRMDBContext db = new gHRMDBContext())
                    {
                        try
                        {
                            var lstobj = db.Database.SqlQuery<EmployeeEmailViewModel>("sp_DailyAbsentList").ToList();
                            if (lstobj.Any())
                            {
                                foreach (var q in lstobj)
                                {
                                    string subject = "Absent";
                                    string body = "";
                                    string to = "";
                                    string cc = "";
                                    if (!string.IsNullOrEmpty(q.OfficialEmail))
                                        to = q.OfficialEmail;
                                    else if (!string.IsNullOrEmpty(q.Email))
                                        to = q.Email;
                                    if (!string.IsNullOrEmpty(to))
                                    {
                                        cc = "ataur@grameen.technology";
                                        if (!string.IsNullOrEmpty(q.CCOfficialEmail) && q.CCOfficialEmail!="saleem@grameen.org")
                                            cc = "," + q.CCOfficialEmail;
                                        else if (!string.IsNullOrEmpty(q.CCEmail) && q.CCEmail != "saleem@grameen.org")
                                            cc = "," + q.CCEmail;
                                        //Colleague
                                        body = string.Format("Dear " + (q.Gender == "M" ? "Mr. " : q.Gender == "F" ? "Ms. " : "")
                                + q.EmployeeName + "," + "<br/>Greetings<br/>You are absent on " + DateTime.Today.ToString("dd-MMM-yyyy") + ". <br/>Please apply leave application."
                                              + "<br/><br/>Head of HR & Admin<br />"
                                             + "<br /><br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");
                                        try
                                        {
                                            int isSend = emailSenderService.SendMailForLateIn(subject, body, to, cc);
                                        }
                                        catch (Exception) { }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}