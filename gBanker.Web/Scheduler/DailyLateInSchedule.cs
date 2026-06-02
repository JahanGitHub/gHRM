using gHRM.Data.CodeFirstMigration;
using gHRM.Web.EmailSenderService;
using gHRM.Web.Helpers;
using Quartz;
using System;
using System.Linq;

namespace gHRM.Web.Scheduler
{
    public class DailyLateInSchedule : IJob
    {
        private readonly EmailSender emailSenderService;
        public DailyLateInSchedule()
        {
            emailSenderService = new EmailSender();
        }
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (AppSetting.GetBool(AppSetting.DailyLateInScheduleEnable, null))
                {

                    DayOfWeek dow = DateTime.Now.DayOfWeek;
                    string[] arr_notcheckEmp2 = { "0001", "0008", "0591", "0621", "2655", "2739", "2759", "2761", "0984", "2444" };
                    string[] stringArray = { "" };

                    using (gHRMDBContext db = new gHRMDBContext())
                    {
                        var qryNotSend = (from outt in db.OutOfOffices
                                          join emp in db.Employees
                                          on outt.EmployeeId equals emp.EmployeeId
                                          where outt.IsActive && outt.FromDate >= DateTime.Today && outt.ToDate <= DateTime.Today
                                          && outt.Category == "Late"
                                          select new
                                          {
                                              emp.EmployeeCode
                                          }).ToList();
                        string[] commaSeparatedArray = new string[qryNotSend.Count];

                        int index = 0;

                        foreach (var NotIn in qryNotSend)
                        {
                            commaSeparatedArray[index] = NotIn.EmployeeCode;
                            index++;
                        }
                        stringArray = commaSeparatedArray.ToArray();
                    }

                    string[] arr_notcheckEmp = arr_notcheckEmp2.Concat(stringArray).ToArray();

                    if (dow.ToString().ToLower() != "friday")
                    {
                        using (gHRMDBContext db = new gHRMDBContext())
                        {
                            var qry = (from att in db.AttAttendances
                                       join emp in db.Employees
                                       on att.EmployeeId equals emp.EmployeeId
                                       join lv in db.LeaveApprovers
                                       on emp.EmployeeId equals lv.EmployeeId
                                       join leave_app in db.Employees
                                       on lv.ApproverEmpId equals leave_app.EmployeeId
                                       join typ in db.AttAttendanceType
                                       on att.AttAttendanceTypeId equals typ.AttAttendanceTypeId
                                       where att.IsActive && att.AttenDate == DateTime.Today && lv.IsActive
                                       && lv.ApprovalLevel == 1
                                       && typ.AttenTypeFullName == "Late"
                                       && !arr_notcheckEmp.Contains(emp.EmployeeCode)
                                       //&& TimeSpan.Parse(att.LateTime) > TimeSpan.FromMinutes(5)
                                       select new
                                       {
                                           emp.EmployeeName,
                                           emp.Gender,
                                           emp.OfficialEmail,
                                           emp.Email,
                                           CCOfficialEmail = leave_app.OfficialEmail,
                                           CCEmail = leave_app.Email,
                                           att.AttenDate,
                                           att.LoginTime,
                                           att.LateTime
                                       }).ToList();

                            foreach (var q in qry)
                            {
                                //if(TimeSpan.Parse(q.LateTime) > TimeSpan.FromMinutes(5))
                                //{
                                string subject = "Late in office";
                                string body = "";
                                string to = "";
                                string cc = "";
                                if (!string.IsNullOrEmpty(q.OfficialEmail))
                                    to = q.OfficialEmail;
                                else if (!string.IsNullOrEmpty(q.Email))
                                    to = q.Email;

                                if (!string.IsNullOrEmpty(q.CCOfficialEmail) && q.CCOfficialEmail != "saleem@grameen.org")
                                    cc = q.CCOfficialEmail;
                                else if (!string.IsNullOrEmpty(q.CCEmail) && q.CCEmail != "saleem@grameen.org")
                                    cc = q.CCEmail;
                                //Colleague
                                body = string.Format("Dear " + (q.Gender == "M" ? "Mr. " : q.Gender == "F" ? "Ms. " : "")
                                + q.EmployeeName + "," + "<br/><br/>" + " Today (" + q.AttenDate.ToString("dd-MMM-yyyy") + ") you have arrived office at " + q.LoginTime.Value.ToString("HH:mm:ss") + ", your late time is " + q.LateTime + ". We will appreciate if you come office on time."
                                      + "<br/><br/>Head of HR & Admin<br />"
                                     + "<br /><br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");
                                int isSend = emailSenderService.SendMailForLateIn(subject, body, to, cc);
                                // }
                            }
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