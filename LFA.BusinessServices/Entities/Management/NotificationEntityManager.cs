using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;

namespace TAS.Services.Entities.Management
{
    public static class NotificationEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string notificationHostUrl = ConfigurationData.NotificationHostUrl;
        private static string privateKey = ConfigurationData.NotificationKey;
        private static readonly HttpClient client = new HttpClient();

        public async static Task PushNotificationSender(PushNotificationsRequestDto notificationData)
        {
            logger.Trace("Push notification recieved at " + DateTime.Now.ToShortDateString() + "as " + JsonConvert.SerializeObject(notificationData));
            try
            {
                var actionUrl = "TasNotification/PushNotifications";
                // client.DefaultRequestHeaders.Add("Authkey", BuildAuthkeyForNotificationSystem());
                var content = new StringContent(JsonConvert.SerializeObject(notificationData).ToString(), Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(notificationHostUrl + actionUrl, content);
                var reponse = await httpResponse.Content.ReadAsStringAsync();
                logger.Trace("Push notification response from notification API " + reponse);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
        }

        #region Utils
        private static string BuildAuthkeyForNotificationSystem()
        {
            string encrypted = "error";
            try
            {
                var authHeader = privateKey + "|" + UnixTimeStampUTC();
                var authCipher = CryptoManager.Encrypt(authHeader);
                return authCipher;
            }
            catch (Exception)
            {
            }
            //format key|time
            return encrypted;
        }

        

        private static Int32 UnixTimeStampUTC()
        {
            Int32 unixTimeStamp;
            DateTime currentTime = DateTime.Now;
            DateTime zuluTime = currentTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        }
        #endregion
    }
}
