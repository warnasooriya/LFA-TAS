using System;
using System.Configuration;

namespace TAS.Services.Common
{
    public static class ConfigurationData
    {
        internal static string NotificationHostUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["NotificationHostUrl"];
            }

        }


        public static string DataMappingFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DataMappingFilePath"];
            }

        }

        public static string DefaultConnectionStringFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultConnectionStringFormat"];
            }
        }

        public static string tasTokenValidTime
        {
            get
            {
                return ConfigurationManager.AppSettings["tasTokenValidTime"];
            }
        }

        public static string SmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServer"];
            }
        }

        public static int Port
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Port"]);
            }
        }

        public static string EmailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFrom"];
            }
        }

        public static string EmailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailPassword"];
            }
        }

        public static bool EnableSsl
        {
            get
            {
                return Boolean.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
            }
        }

        public static string EmailPath
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailPath"];
            }
        }

        public static bool BlockEmailToTPAAdmin
        {
            get
            {
                String restut = ConfigurationManager.AppSettings["BlockEmailToTPAAdmin"];
                if (restut == "TRUE" || restut == "true")
                {
                    return true;
                }
                else {
                    return false;
                }

            }
        }



        public static string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"];
            }
        }

        public static int ForgotPasswordLinkExpiryHours
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["ForgotPasswordLinkExpiryHours"]);
            }
        }

        public static string QueryPath
        {
            get
            {
                return ConfigurationManager.AppSettings["QueryPath"];
            }
        }

        public static string ReportsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportsPath"];
            }
        }





        public static string AWSUniqueDbKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSUniqueDbKey"];
            }
        }

        public static string AWSAccessKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSAccessKey"];
            }
        }

        public static string AWSSecrteKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSSecrteKey"];
            }
        }

        public static string CrowdPdfKey
        {
            get
            {
                return ConfigurationManager.AppSettings["CrowdPdfKey"];
            }
        }

        public static string CrowdPdfUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["CrowdPdfUserName"];
            }
        }

        public static string PdfType
        {
            get
            {
                return ConfigurationManager.AppSettings["PdfType"];
            }
        }

        public static string ClaimNumberFormatPadding
        {
            get
            {
                return ConfigurationManager.AppSettings["ClaimNumberFormatPadding"];
            }
        }

        public static int ClaimProcessingSafeTime
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ClaimProcessingSafeTime"]);
            }
        }

        public static string EmailMask
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailMask"];
            }
        }

        public static string NotificationKey
        {
            get
            {
                return ConfigurationManager.AppSettings["NotificationKey"];
            }
        }

        public static string NotificationEncryptionKey
        {
            get
            {
                return ConfigurationManager.AppSettings["NotificationEncryptionKey"];
            }
        }

        public static int TireInvoiceCodeValidityPeriodDays
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TireInvoiceCodeValidityPeriodDays"]);
            }
        }

        public static string PrintFriendlyApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["PrintFriendlyApiKey"];
            }
        }

        public static string PrintFriendlyBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PrintFriendlyBaseUrl"];
            }
        }

        public static string PrintFriendlyHtmlBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PrintFriendlyHtmlBaseUrl"];
            }
        }

        public static string EmailLogoPath {
            get {
                return ConfigurationManager.AppSettings["EmailLogoPath"];
            }
        }

        public static string BackendEndPointUrl {
            get
            {
                return ConfigurationManager.AppSettings["BackendEndPointUrl"];
            }
        }
    }
}
