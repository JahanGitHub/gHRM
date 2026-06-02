using gHRM.Web.Helpers;
using Microsoft.Win32;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Scheduler
{

    public class ScheduleRegister
    {
        public static void DailyLateInScheduleStart()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //// Daily Late In
            IJobDetail job = JobBuilder.Create<DailyLateInSchedule>()
                //.WithIdentity("","")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(s =>
                s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(11, 10))
                /*.InTimeZone(TimeZoneInfo.Utc)*/)
            .Build();
            scheduler.ScheduleJob(job, trigger);

            //// Punch Not Found
            IJobDetail job_PNF = JobBuilder.Create<PunchNotFoundScheduleMail>()
                //.WithIdentity("","")
                .Build();
            ITrigger trigger_PNF = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(s =>
                s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 01))
                /*.InTimeZone(TimeZoneInfo.Utc)*/)
            .Build();
            scheduler.ScheduleJob(job_PNF, trigger_PNF);

            //// Absent But No Apply
            IJobDetail job_Absent = JobBuilder.Create<AbsentButNoApplyScheduleMail>()
                //.WithIdentity("","")
                .Build();
            ITrigger trigger_Absent = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(s =>
                s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 01))
                /*.InTimeZone(TimeZoneInfo.Utc)*/)
            .Build();
            scheduler.ScheduleJob(job_Absent, trigger_Absent);

            /// Monthly
            IJobDetail monthlyJob = JobBuilder.Create<MonthlyCongratulationScheduleMail>().Build();
            ITrigger monthlytrigger = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(s =>
                s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(01, 00)))
            .Build();
            scheduler.ScheduleJob(monthlyJob, monthlytrigger);

            // Office out time add
            IJobDetail out_add_Job = JobBuilder.Create<OfficeOutTimeAddSchedule>().Build();
            ITrigger out_add_trigger = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule(s =>
                s.WithIntervalInHours(24)
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(22, 30)))
            //.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 47)))
            .Build();
            scheduler.ScheduleJob(out_add_Job, out_add_trigger);

        }
    }
}