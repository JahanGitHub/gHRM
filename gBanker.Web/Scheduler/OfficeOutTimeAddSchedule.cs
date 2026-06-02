using gHRM.Data.CodeFirstMigration;
using gHRM.Web.EmailSenderService;
using gHRM.Web.Helpers;
using Quartz;
using System;
using System.Linq;

namespace gHRM.Web.Scheduler
{
    public class OfficeOutTimeAddSchedule : IJob
    {
        private readonly EmailSender emailSenderService;
        public OfficeOutTimeAddSchedule()
        {
            emailSenderService = new EmailSender();
        }
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (AppSetting.GetBool(AppSetting.LogoutScheduleEnable, null))
                {
                    DayOfWeek dow = DateTime.Now.DayOfWeek;
                    if (dow.ToString().ToLower() != "friday" || dow.ToString().ToLower() != "saturday")
                    {
                        using (gHRMDBContext db = new gHRMDBContext())
                        {
                            var dd = db.Database.SqlQuery<int>("[att].[sp_TimeKeeping_OfficeOut]");
                        }
                    }
                }
                   
            }
            catch (Exception ex)
            {

            }
        }
    }
}