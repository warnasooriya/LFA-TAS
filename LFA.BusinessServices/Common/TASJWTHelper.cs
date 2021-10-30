using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TAS.Services.Entities.Management;


namespace TAS.Services.Common
{
    public class TASJWTHelper
    {
        internal int clientId { get; set; }
        internal string sharedKey { get; set; }
        internal string username { get; set; }
        internal string token { get; set; }
        internal string database { get; set; }

        public TASJWTHelper(TASSystemUser su, string dbname)
        {
            clientId = 1;
            sharedKey = "abce";
            username = su.Email;
            database = dbname;
        }


        public TASJWTHelper(TAS.DataTransfer.Common.SecurityContext sc)
        {
            sharedKey = "abce";
            token = sc.Token;
        }

        public string CreateAuthenticationToken(string systemUserID, string userName)
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int timestamp = (int)t.TotalSeconds;

            var payload = new Dictionary<string, object>
            {
                { "iat", timestamp },
                { "jti", Guid.NewGuid().ToString() },
                { "name", this.username },
                { "clientid", this.clientId },
                { "dbName", this.database },
                { "sharedkey", this.sharedKey }
            };

            string token = JWT.JsonWebToken.Encode(payload, this.sharedKey, JWT.JwtHashAlgorithm.HS256);

            TASUserLoginEntityManager userLoginEntityManager = new TASUserLoginEntityManager();
            if (userLoginEntityManager.issueUserLogin(systemUserID, token, userName) == "OK")
            {
                return token;
            }
            else
                return "";
        }

        public string CreateCookieToken(string userName, string password, string tpaID)
        {
            var payload = new Dictionary<string, string>();
            payload.Add("jti",Guid.NewGuid().ToString());
            payload.Add("userName",userName);
            payload.Add("password",password);
            payload.Add("tpaID",tpaID);

            string token = JWT.JsonWebToken.Encode(payload, this.sharedKey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }

        public Dictionary<string, object> DecodeCookieToken(String cookieToken)
        {
            string decoded = JWT.JsonWebToken.Decode(cookieToken, this.sharedKey, true);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Dictionary<string, object> payload = (Dictionary<string, object>)js.DeserializeObject(decoded);
           // string json = @"{UserName: '"+payload["userName"]+"',Password:'"+ payload["password"]+"', tpaID:'"+  payload["tpaID"]+"'}";
            return payload;
        }
        public Dictionary<string, object> DecodeAuthenticationToken()
        {
            string decoded = JWT.JsonWebToken.Decode(token, this.sharedKey, true);
            JavaScriptSerializer js = new JavaScriptSerializer();

            Dictionary<string, object> payload = (Dictionary<string, object>)js.DeserializeObject(decoded);

            return payload;
        }

        public bool checkTokenValidity(int idleTimeSec)
        {

            TASUserLoginEntityManager userLoginEntityManager = new TASUserLoginEntityManager();
            return userLoginEntityManager.CheckAndUpdateLogin(this.token, idleTimeSec);

        }
    }
}
