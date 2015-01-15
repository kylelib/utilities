using Common.Tools.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools.Mail
{
    public static class MailHelper
    {
        public static SmtpClient CreateSmtpClient()
        {
            SmtpClient emailClient = new SmtpClient();

            emailClient.Host = ConfigSettings.Smtp.Host;
            emailClient.Port = ConfigSettings.Smtp.Port;
            emailClient.EnableSsl = ConfigSettings.Smtp.EnableSsl;

            if (!string.IsNullOrEmpty(ConfigSettings.Smtp.UserName))
            {
                emailClient.Credentials = new System.Net.NetworkCredential(ConfigSettings.Smtp.UserName, ConfigSettings.Smtp.Password);
            }

            return emailClient;
        }

        public static void SendEmail(MailMessage mail)
        {
            //Prepend environment to Subject (except in PROD)
            if (ConfigSettings.Environment.Abbreviation != "PRD" && !string.IsNullOrEmpty(ConfigSettings.Environment.Abbreviation))
                mail.Subject = ConfigSettings.Environment.Abbreviation + " - " + mail.Subject; ;

            if (ConfigSettings.Smtp.DeliveryEnabled)
            {
                var emailClient = CreateSmtpClient();
                emailClient.Send(mail);
            }
        }


        /// <summary>
        /// method will send email with body pulled from template location and templateVariables will be replaced by name matching.
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="templatePath"></param>
        /// <param name="templateVariables"></param>
        public static void SendEmailFromTemplate(MailMessage mail, string templatePath, Dictionary<string, string> templateVariables)
        {
            //Prepend environment to Subject (except in PROD)
            if (ConfigSettings.Environment.Abbreviation != "PRD" && !string.IsNullOrEmpty(ConfigSettings.Environment.Abbreviation))
                mail.Subject = ConfigSettings.Environment.Abbreviation + " - " + mail.Subject; ;

            mail.Body = BuildBodyFromTemplate(templatePath, templateVariables);

            if (ConfigSettings.Smtp.DeliveryEnabled)
            {
                var emailClient = CreateSmtpClient();
                emailClient.Send(mail);
            }
        }

        /// <summary>        
        /// Builds, asssigns, and returns the email message body.
        /// </summary>
        /// <param name="emailTokens"></param>
        /// <returns></returns>
        private static string BuildBodyFromTemplate(string templatePath, Dictionary<string, string> templateVariables)
        {
            string emailBody = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath));

            //Replace tokens
            foreach (string prop in templateVariables.Keys)
                emailBody = emailBody.Replace("%%" + prop + "%%", templateVariables[prop]);

            return emailBody;
        }
    }
}
