using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools.Configuration
{
    public class ConfigSettings : ConfigSettingsBase
    {
        private static bool _AreSettingsInitialized;

        private static EnvironmentSettings _Environment;
        public static EnvironmentSettings Environment
        {
            get
            {
                if (!_AreSettingsInitialized) Init();
                return _Environment;
            }
        }

        private static SmtpSettings _Smtp;
        public static SmtpSettings Smtp
        {
            get
            {
                if (!_AreSettingsInitialized) Init();
                return _Smtp;
            }
        }

        private static E3ConnectSettings _E3Connect;
        public static E3ConnectSettings E3Connect
        {
            get
            {
                if (!_AreSettingsInitialized) Init();
                return _E3Connect;
            }
        }

        //Application specific settings below
        private static E3ImportExportSettings _E3ImportExport;
        public static E3ImportExportSettings E3ImportExport
        {
            get
            {
                if (!_AreSettingsInitialized) Init();
                return _E3ImportExport;
            }
        }

        private static void Init()
        {
            _Environment.Name = GetValue<string>("Environment.Name");
            _Environment.Abbreviation = GetValue<string>("Environment.Abbreviation");

            _Smtp.DeliveryEnabled = GetValue<bool>("Smtp.DeliveryEnabled");
            _Smtp.Host = GetValue<string>("Smtp.Host");
            _Smtp.Port = GetValue<int>("Smtp.Port");
            _Smtp.EnableSsl = GetValue<bool>("Smtp.EnableSsl");
            _Smtp.UserName = GetValue<string>("Smtp.UserName", string.Empty);
            _Smtp.Password = GetValue<string>("Smtp.Password", string.Empty);

            _E3Connect.ServiceUrl = GetValue<string>("E3Connect.ServiceUrl");
            _E3Connect.UserName = GetValue<string>("E3Connect.UserName");
            _E3Connect.Password = GetValue<string>("E3Connect.Password");

            _E3ImportExport.ServiceLoopDelayInSeconds = GetValue<int>("E3ImportExport.ServiceLoopDelayInSeconds");
            _E3ImportExport.ServiceLoopDelayIfExceptionInSeconds = GetValue<int>("E3ImportExport.ServiceLoopDelayIfExceptionInSeconds");
            _E3ImportExport.ServiceExportRequestDate = GetValue<string>("E3ImportExport.ServiceExportRequestDate");
            _E3ImportExport.VerifyLoggingOnStartUp = GetValue<bool>("E3ImportExport.VerifyLoggingOnStartUp");

            _E3ImportExport.NotificationEmailFrom = GetValue<string>("E3ImportExport.NotificationEmailFrom");
            _E3ImportExport.NotificationEmailTo = GetValue<string>("E3ImportExport.NotificationEmailTo");
            _E3ImportExport.NotificationEmailTemplate = GetValue<string>("E3ImportExport.NotificationEmailTemplate");

            _E3ImportExport.PromeritExportLocation = GetValue<string>("E3ImportExport.PromeritExportLocation");
            _E3ImportExport.PromeritExportValidationEnabled = GetValue<bool>("E3ImportExport.PromeritExportValidationEnabled");
            _E3ImportExport.PromeritExportAlertWhenNoLoans = GetValue<bool>("E3ImportExport.PromeritExportAlertWhenNoLoans");
            _E3ImportExport.PromeritExportNotificationEmailTo = GetValue<string>("E3ImportExport.PromeritExportNotificationEmailTo");
            _E3ImportExport.PromeritExportProcessingWindowStart = GetValue<TimeSpan>("E3ImportExport.PromeritExportProcessingWindowStart");
            _E3ImportExport.PromeritExportProcessingWindowEnd = GetValue<TimeSpan>("E3ImportExport.PromeritExportProcessingWindowEnd");

            _E3ImportExport.PromeritImportLocation = GetValue<string>("E3ImportExport.PromeritImportLocation");
            _E3ImportExport.PromeritImportValidationEnabled = GetValue<bool>("E3ImportExport.PromeritImportValidationEnabled");
            _E3ImportExport.PromeritImportAlertWhenNoLoans = GetValue<bool>("E3ImportExport.PromeritImportAlertWhenNoLoans");
            _E3ImportExport.PromeritImportNotificationEmailTo = GetValue<string>("E3ImportExport.PromeritImportNotificationEmailTo");

            _AreSettingsInitialized = true;
        }
    }

    public struct EnvironmentSettings
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public struct SmtpSettings
    {
        public bool DeliveryEnabled { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public struct E3ConnectSettings
    {
        public string ServiceUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    //Application Specific classes below
    public struct E3ImportExportSettings
    {
        public int ServiceLoopDelayInSeconds { get; set; }
        public int ServiceLoopDelayIfExceptionInSeconds { get; set; }
        public string ServiceExportRequestDate { get; set; }
        public bool VerifyLoggingOnStartUp { get; set; }

        public string NotificationEmailFrom { get; set; }
        public string NotificationEmailTo { get; set; }
        public string NotificationEmailTemplate { get; set; }

        public string PromeritExportLocation { get; set; }
        public bool PromeritExportValidationEnabled { get; set; }
        public bool PromeritExportAlertWhenNoLoans { get; set; }
        public string PromeritExportNotificationEmailTo { get; set; }
        public TimeSpan PromeritExportProcessingWindowStart { get; set; }
        public TimeSpan PromeritExportProcessingWindowEnd { get; set; }

        public string PromeritImportLocation { get; set; }
        public bool PromeritImportValidationEnabled { get; set; }
        public bool PromeritImportAlertWhenNoLoans { get; set; }
        public string PromeritImportNotificationEmailTo { get; set; }
    }

}
