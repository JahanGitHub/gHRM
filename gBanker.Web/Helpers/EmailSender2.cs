using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using gHRM.Service.StoreProcedure;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.Helpers;

namespace gHRM.Web.EmailSenderService
{
    public class EmailSender2
    {
        private EmployeeSPService spService = new EmployeeSPService();

        public string generateGUID()
        {
            return Guid.NewGuid().ToString();
        }

        public int SendNotificatinEmail(
            long employeeId,
            long applicantEmployeeId,
            string LeaveStartDate,
            string LeaveEndDate,
            string destinationUrl,
            string mailType,
            string reason,
            string company
            )
        {
            try
            {
                // get applicant
                var applicant_param = new { EmpId = applicantEmployeeId };
                var applicant = spService.GetDataWithParameter(applicant_param, "emp.SP_Get_Employee_ByEmployeeId");

                // get approver
                var employee_param = new { EmpId = employeeId };
                var employee = spService.GetDataWithParameter(employee_param, "emp.SP_Get_Employee_ByEmployeeId");

                if (!(applicant.Tables[0].Rows.Count > 0 && employee.Tables[0].Rows.Count > 0))
                    return 0;

                var applicantInfo = applicant.Tables[0].Rows[0];
                var employeeInfo = employee.Tables[0].Rows[0];

                string eamilAddress = "";
                string officialEamilAddress = "";

                if (mailType == EmailNotificationTypeConstants.Application ||
                    mailType == EmailNotificationTypeConstants.Replacement)
                {
                    eamilAddress = Convert.ToString(employeeInfo["Email"]);
                    officialEamilAddress = Convert.ToString(employeeInfo["OfficialEmail"]);
                }
                else if (mailType == EmailNotificationTypeConstants.Rejected ||
                         mailType == EmailNotificationTypeConstants.Approved)
                {
                    eamilAddress = Convert.ToString(applicantInfo["Email"]);
                    officialEamilAddress = Convert.ToString(applicantInfo["OfficialEmail"]);
                }

                if (string.IsNullOrEmpty(officialEamilAddress) && string.IsNullOrEmpty(eamilAddress))
                    return 0;

                // employee & applicant info
                string employeeName = Convert.ToString(employeeInfo["EmployeeName"]);
                string employeeGender = Convert.ToString(employeeInfo["Gender"]);
                var empGender = employeeGender == "M" ? " Sir" : " Madam";
                var empMrMs = employeeGender == "M" ? "Mr. " : "Ms. ";
                var empDesignation = Convert.ToString(employeeInfo["DesignationName"]);
                var empSignatureName = Convert.ToString(employeeInfo["SignatureName"]);
                if (string.IsNullOrEmpty(empSignatureName)) empSignatureName = empDesignation;
                string empDepartment = Convert.ToString(employeeInfo["DepartmentName"]);

                string applicantName = Convert.ToString(applicantInfo["EmployeeName"]);
                string applicantCode = Convert.ToString(applicantInfo["EmployeeCode"]);
                string applicantDepartment = Convert.ToString(applicantInfo["DepartmentName"]);
                string applicantGender = Convert.ToString(applicantInfo["Gender"]);
                var appliGender = applicantGender == "M" ? "his" : "her";

                var guid = generateGUID();
                string header = "";
                string emailBody = "";

                // ================= EMAIL BODY GENERATION =================
                if (mailType == EmailNotificationTypeConstants.Application)
                {
                    var link = destinationUrl.Replace("GID", guid);
                    header = "Leave Approval Request";
                    emailBody = $"Dear {employeeName}{empGender},<br/><br/>" +
                                $"{applicantName}, ID-{applicantCode}, Department-{applicantDepartment}, has applied for a leave." +
                                $"<br/><br/>Leave Start Date: {LeaveStartDate}<br/>Leave End Date: {LeaveEndDate}" +
                                $"<br/><a href=\"{link}\">To approve or reject this leave please click this link.</a>" +
                                "<br/><br/><b>On behalf of HR & Admin Team</b><br />" +
                                "<br/><br/><br/> Note: Please do not reply to this system generated email.";
                }
                else if (mailType == EmailNotificationTypeConstants.Replacement)
                {
                    header = "Leave Replacement Employee";
                    emailBody = $"Dear {employeeName},<br/><br/>{applicantName}, ID-{applicantCode}, Department-{applicantDepartment}, " +
                                $"has applied for a leave assigning you as {appliGender} replacement employee." +
                                $"<br/><br/>Leave Start Date: {LeaveStartDate}<br/>Leave End Date: {LeaveEndDate}" +
                                "<br/><br/><b>On behalf of HR & Admin Team</b><br />" +
                                "<br/><br/><br/> Note: Please do not reply to this system generated email.";
                }
                else if (mailType == EmailNotificationTypeConstants.Rejected)
                {
                    header = "Leave Application Rejection";
                    emailBody = $"Dear {applicantName},<br/><br/>Your applied leave from {LeaveStartDate} to {LeaveEndDate} " +
                                $"is rejected by {empMrMs}{employeeName}, {empSignatureName}, {empDepartment}." +
                                $"<br/><br/>The reason of rejection is \"{reason}\"." +
                                "<br/><br/><b>On behalf of HR & Admin Team</b><br />" +
                                "<br/><br/><br/> Note: Please do not reply to this system generated email.";
                }
                else if (mailType == EmailNotificationTypeConstants.Approved)
                {
                    header = "Leave Application Approval";
                    emailBody = $"Dear {applicantName},<br/><br/>Your applied leave from {LeaveStartDate} To {LeaveEndDate} is approved by concern authority." +
                                "<br/><br/><b>On behalf of HR & Admin Team</b><br />" +
                                "<br/><br/><br/> Note: Please do not reply to this system generated email.";
                }

                // ================= SEND EMAIL =================
                string finalEmailAddress = !string.IsNullOrEmpty(officialEamilAddress) ? officialEamilAddress : eamilAddress;

                SendMail_Configurable(finalEmailAddress, emailBody, header, company);

                return 1;
            }
            catch (Exception ex)
            {
                string abc = ex.Message;
                return 0;
            }
        }

        private async Task SendMail_Configurable(string toAddress, string mailBody, string subject, string company)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var mailMessage = new MailMessage();

            foreach (var address in toAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(address.Trim());
            }

            const string style = "<p style=\"font-family:Cambria;font-size:11pt\">";
            mailMessage.Body = style + mailBody + "</p>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;

            string provider = System.Configuration.ConfigurationManager.AppSettings["MailProvider"];

            string smtpHost = "";
            int smtpPort = 587;
            string smtpUser = "";
            string smtpPass = "";

            if (provider == "Office365")
            {
                smtpHost = System.Configuration.ConfigurationManager.AppSettings["Office365_Host"];
                smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Office365_Port"]);
                smtpUser = System.Configuration.ConfigurationManager.AppSettings["Office365_User"];
                smtpPass = System.Configuration.ConfigurationManager.AppSettings["Office365_Password"];
            }
            else if (provider == "Gmail")
            {
                smtpHost = System.Configuration.ConfigurationManager.AppSettings["Gmail_Host"];
                smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Gmail_Port"]);
                smtpUser = System.Configuration.ConfigurationManager.AppSettings["Gmail_User"];
                smtpPass = System.Configuration.ConfigurationManager.AppSettings["Gmail_Password"];
            }

            mailMessage.From = new MailAddress(smtpUser, company);

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.Timeout = 10000;
                client.SendMailAsync(mailMessage);
            }
        }
    }
}
