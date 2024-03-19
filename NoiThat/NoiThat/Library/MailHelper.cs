using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace NoiThat.Library
{
    public class MailHelper
    {
        public void SendMail(string toEmail, string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSs1l = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmail));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSs1l;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);

        }
    }
}