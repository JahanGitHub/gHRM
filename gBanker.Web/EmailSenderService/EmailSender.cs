using System;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using gHRM.Service.StoreProcedure;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.Helpers;

namespace gHRM.Web.EmailSenderService
{
    public class EmailSender
    {
        private EmployeeSPService spService = new EmployeeSPService();
        public EmailHelper _Helper;

        public string generateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public int SendNotificatinEmail
            (long employeeId, long applicantEmployeeId, string LeaveStartDate, string LeaveEndDate
            , string destinationUrl, string mailType, string reason, string companycode)
        {
            string Message = "";
            // mailType for application, confirmation,replacement and rejection mail
            int is_mail_sent = 0;
            try
            {
                //get approver employee info
                var applicant_param = new { EmpId = applicantEmployeeId };
                var applicant = spService.GetDataWithParameter(applicant_param, "emp.SP_Get_Employee_ByEmployeeId");

                //get applicant employee info
                var employee_param = new { EmpId = employeeId };
                var employee = spService.GetDataWithParameter(employee_param, "emp.SP_Get_Employee_ByEmployeeId");

                if (!(applicant.Tables[0].Rows.Count > 0 && employee.Tables[0].Rows.Count > 0))
                {
                    is_mail_sent = 0;
                    return is_mail_sent;
                }

                var applicantInfo = applicant.Tables[0].Rows[0];
                var employeeInfo = employee.Tables[0].Rows[0];// approver// replacement employee

                string eamilAddress = string.Empty;
                string officialEamilAddress = string.Empty;

                //if application or replacement then send email notifiation to approver
                if (mailType == EmailNotificationTypeConstants.Application ||
                    mailType == EmailNotificationTypeConstants.Replacement)
                {
                    eamilAddress = Convert.ToString(employeeInfo["Email"]);
                    officialEamilAddress = Convert.ToString(employeeInfo["OfficialEmail"]);
                }

                //if Rejected or Approved then send email notifiation to applicant
                if (mailType == EmailNotificationTypeConstants.Rejected ||
                    mailType == EmailNotificationTypeConstants.Approved)
                {
                    eamilAddress = Convert.ToString(applicantInfo["Email"]);
                    officialEamilAddress = Convert.ToString(applicantInfo["OfficialEmail"]);
                }

                if (String.IsNullOrEmpty(officialEamilAddress) && String.IsNullOrEmpty(eamilAddress))
                {
                    is_mail_sent = 0;
                    return is_mail_sent;
                }

                string employeeName = Convert.ToString(employeeInfo["EmployeeName"]);
                string employeeGender = Convert.ToString(employeeInfo["Gender"]);
                var empGender = employeeGender == "M" ? " Sir" : " Madam";
                var empMrMs = employeeGender == "M" ? "Mr. " : "Ms. ";
                var empDesignation = Convert.ToString(employeeInfo["DesignationName"]);
                var empSignatureName = Convert.ToString(employeeInfo["SignatureName"]);

                if (empSignatureName == "")
                    empSignatureName = empDesignation;

                string empDepartment = Convert.ToString(employeeInfo["DepartmentName"]);
                string applicantName = Convert.ToString(applicantInfo["EmployeeName"]);
                string applicantCode = Convert.ToString(applicantInfo["EmployeeCode"]);
                string applicantDesignation = Convert.ToString(applicantInfo["DesignationName"]);
                string applicantDepartment = Convert.ToString(applicantInfo["DepartmentName"]);
                string applicantGender = Convert.ToString(applicantInfo["Gender"]);
                var appliGender = applicantGender == "M" ? "his" : "her";

                var guid = generateGUID();
                string header = "";
                var emailBody = "";
                if (mailType == EmailNotificationTypeConstants.Application)
                {
                    var link = destinationUrl.Replace("GID", guid.ToString());
                    header = "Leave Approval Request";
                    emailBody = string.Format("Dear " + employeeName + empGender + ","
                        + "<br/><br/>" + applicantName + ", ID-" + applicantCode + ", Department-" + applicantDepartment + ", has applied for a leave."
                        + "<br/><br/> <br/> Leave Start Date: " + LeaveStartDate + "<br/>" + "Leave End Date: " + LeaveEndDate
                        + "<br/><a href=\"{0}\"> To approve or reject this leave please click this link.</a>"
                        + "<br/><br/><b>On behalf of HR & Admin Team</b><br />"//<img src="+ companyLogoLink + " alt=\"Company Logo\"width=\"500\" height=\"600\">
                        + "<br/><br/><br/> Note: Please do not reply to this system generated email. For any queries please contact concern person.", link);
                }
                if (mailType == EmailNotificationTypeConstants.Replacement)
                {
                    header = "Leave Replacement Employee";
                    emailBody = string.Format("Dear " + employeeName + "," + "<br/><br/>" + applicantName + ", ID-" + applicantCode + ", Department-" + applicantDepartment + ", has applied for a leave assigning you as " + appliGender + " replacement employee.<br/> <br/> <br/> Leave Start Date: " + LeaveStartDate + "<br/>" + "Leave End Date: " + LeaveEndDate
                        + "<br/><br/><b>On behalf of HR & Admin Team</b><br />"
                        + "<br /><br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");
                }
                if (mailType == EmailNotificationTypeConstants.Rejected)
                {
                    header = "Leave Application Rejection";
                    emailBody = string.Format("Dear " + applicantName + "," + "<br/><br/>Your applied leave from " + LeaveStartDate + " to " + LeaveEndDate + "  is rejected by " + empMrMs + employeeName + ", " + empSignatureName + ", " + empDepartment
                        + "<br/><br/>The reason of rejection is \"" + reason + '"'
                        + ".<br/><br/><b>On behalf of HR & Admin Team</b><br />"
                         + "<br /><br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");
                }
                if (mailType == EmailNotificationTypeConstants.Approved)
                {
                    header = "Leave Application Approval";

                    emailBody = string.Format("Dear " + applicantName + "," + "<br/><br/>" + " Your applied Leave form: " + LeaveStartDate + "  To: " + LeaveEndDate + " is approved by concern authority."
                          + "<br/><br/><b>On behalf of HR & Admin Team</b><br />"
                         + "<br /><br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");
                }

                //let's send email notification
                if (!String.IsNullOrEmpty(officialEamilAddress))
                {
                    string emailAddress = officialEamilAddress;
                    _Helper = new EmailHelper();
                    if (companycode == GHRMPlusCompanyConstants.GrameenCommunications.ToLower())
                        //is_mail_sent = _Helper.SendMailUsingCompanyMail(header, emailBody, emailAddress,null, out Message) ? 1 : 0;
                        is_mail_sent = _Helper.SendMail(header, emailBody, emailAddress, out Message) ? 1 : 0;
                    else
                        is_mail_sent = _Helper.SendMail(header, emailBody, emailAddress, out Message) ? 1 : 0;
                    //var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, header);
                }
                else if (!String.IsNullOrEmpty(eamilAddress))
                {
                    string emailAddress = eamilAddress;
                    _Helper = new EmailHelper();
                    is_mail_sent = _Helper.SendMail(header, emailBody, emailAddress, out Message) ? 1 : 0;
                    //var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, header);
                }
                //is_mail_sent = 1;
            }
            catch (Exception ex)
            {
                is_mail_sent = 0;
                //throw;
                return is_mail_sent;
            }
            return is_mail_sent;
        }

        public int SendMailForLateIn(string subject, string emailBody, string to, string cc)
        {
            string msg = "";
            _Helper = new EmailHelper();
            
            //return _Helper.SendMailUsingCompanyMail(subject, emailBody, to, cc, out msg) ? 1 : 0;
            return _Helper.SendMail(subject, emailBody, to, cc, out msg) ? 1 : 0;
        }
        //public int SendNotificationEmailToReplacemnetEmployee(long ReplacementEmployee, long employeeId, string LeaveStartDate, string LeaveEndDate, string destinationUrl)
        //{
        //    int is_mail_sent = 0;
        //    try
        //    {
        //        var employee_param = new { EmpId = employeeId };
        //        var employees = spService.GetDataWithParameter(employee_param, "SP_Get_Employee_ByEmployeeId");

        //        var replacement_param = new { EmpId = ReplacementEmployee };
        //        var replacement = spService.GetDataWithParameter(replacement_param, "SP_Get_Employee_ByEmployeeId");

        //        if (employees.Tables[0].Rows.Count > 0 && replacement.Tables[0].Rows.Count > 0)
        //        {
        //            var employeeInfo = employees.Tables[0].Rows[0];
        //            var replacementInfo = replacement.Tables[0].Rows[0];

        //            //string eamilAddress = Convert.ToString(replacementInfo["Email"]);
        //            //string officialEamilAddress = Convert.ToString(replacementInfo["OfficialEmail"]);
        //            string eamilAddress = "jobayershoaib@gmail.com";
        //            string officialEamilAddress = "jobayershoaib@gmail.com";

        //            if (!String.IsNullOrEmpty(officialEamilAddress) || !String.IsNullOrEmpty(eamilAddress))
        //            {
        //                string replacementName = Convert.ToString(replacementInfo["EmployeeName"]);
        //                string designationName = Convert.ToString(replacementInfo["DesignationName"]);
        //                string employeeName = Convert.ToString(employeeInfo["EmployeeName"]);
        //                //string designationName = Convert.ToString(employeeInfo["DesignationName"]);
        //                //string departmentName = Convert.ToString(employeeInfo["DepartmentName"]);

        //                string vendor = "gHRMPLUS";
        //                //const string guId = "GID";
        //                //var destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, IRequest.Url.Scheme);

        //                var guid = generateGUID();
        //                var link = destinationUrl.Replace("GID", guid.ToString());

        //                // var emailBody = string.Format("Dear" + approverName + "," + "<br/><br/>  The following " + vendor + " time sheet has been submitted through ____ for your approval.<br/> <br/> <br/> Name: " + employeeName + "<br/>" + "Designation: " + designationName + "<br/>" + "Department: " + departmentName + "<br/>" + "<a href=\"{0}\"> Please click here to APPROVE or DECLINE the timesheet.</a>" + "<br /><br /> <br /><br />  Note: Please do not reply to this system generated email. For any queries please contact ", link);
        //                var emailBody = string.Format("Dear " + replacementName + "," + "<br/><br/>" + employeeName + " has applied for a leave assigning you as his/her replacement employee.<br/> <br/> <br/> Leave Start Date: " + LeaveStartDate + "<br/>" + "Leave End Date: " + LeaveEndDate + "<br /><br /> <br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");


        //                if (!String.IsNullOrEmpty(officialEamilAddress))
        //                {
        //                    string emailAddress = officialEamilAddress;
        //                    var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, "Leave Replacement Employee");
        //                }

        //                else if (!String.IsNullOrEmpty(eamilAddress))
        //                {
        //                    string emailAddress = eamilAddress;
        //                    var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, "Leave Replacement Employee");
        //                }
        //            }
        //        }
        //        is_mail_sent = 1;
        //    }

        //    catch (Exception ex)
        //    {
        //        is_mail_sent = 0;
        //        return is_mail_sent;
        //        //throw new System.Exception("Logfile cannot be read-only");
        //    }
        //    return is_mail_sent;
        //}


        //public int SendConfirmationEmail(long employeeId, string LeaveStartDate, string LeaveEndDate, string info)
        //{
        //    int is_mail_sent = 0;
        //    try
        //    {
        //        var employee_param = new { EmpId = employeeId };
        //        var employees = spService.GetDataWithParameter(employee_param, "SP_Get_Employee_ByEmployeeId");

        //        if (employees.Tables[0].Rows.Count > 0)
        //        {
        //            var employeeInfo = employees.Tables[0].Rows[0];

        //            string eamilAddress = Convert.ToString(employeeInfo["Email"]);
        //            string officialEamilAddress = Convert.ToString(employeeInfo["OfficialEmail"]);

        //            if (!String.IsNullOrEmpty(officialEamilAddress) || !String.IsNullOrEmpty(eamilAddress))
        //            {
        //                string employeeName = Convert.ToString(employeeInfo["EmployeeName"]);
        //                string header = string.Format("{0} {1} {2}", "Leave", info, "Mail");
        //                var emailBody = string.Format("Dear " + employeeName + "," + "<br/><br/>" + " Your applied Leave form: " + LeaveStartDate + "  To: " + LeaveEndDate + " is " + info + "by concern authority." + "<br /><br /> Note: Please do not reply to this system generated email. For any queries please contact concern person.");


        //                if (!String.IsNullOrEmpty(officialEamilAddress))
        //                {
        //                    string emailAddress = officialEamilAddress;
        //                    var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, header);
        //                }

        //                else if (!String.IsNullOrEmpty(eamilAddress))
        //                {
        //                    string emailAddress = eamilAddress;
        //                    var reponse = new EmailSender().SendMailAsync(emailAddress, emailBody, header);
        //                }
        //            }
        //        }
        //        is_mail_sent = 1;
        //    }

        //    catch (Exception ex)
        //    {
        //        is_mail_sent = 0;
        //        return is_mail_sent;
        //        //throw new System.Exception("Logfile cannot be read-only");
        //    }
        //    return is_mail_sent;
        //}

        private void SendMail(string toAddress, string mailBody, string subject)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(toAddress);
            const string style = "<p style=" + "font-family:Cambria;font-size:11pt" + ">";//Calibri
            mailMessage.Body = style + mailBody + "</p>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            var fromMessage = ConfigurationManager.AppSettings["messageFrom"];
            mailMessage.From = new MailAddress(fromMessage);
            var client = new SmtpClient();
            var smtpHost = ConfigurationManager.AppSettings["smtpHost"];
            client.Host = smtpHost; //Set your smtp host address

            var portId = ConfigurationManager.AppSettings["portID"];

            client.Port = Convert.ToInt32(portId); // Set your smtp port address
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            var credentialId = ConfigurationManager.AppSettings["credentialID"];
            var credentialPassword = ConfigurationManager.AppSettings["credentialPassword"];

            client.Credentials = new NetworkCredential(credentialId, credentialPassword); //account name and password

            client.Send(mailMessage);
        }


        public Task SendMailAsync(string toAddress, string mailBody, string subject)
        {
            string Message = "";
            return Task.Run(() => SendMail(toAddress, mailBody, subject));
        }
    }
}
