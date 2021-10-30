using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;

namespace TAS.Services.Common
{
    public class JWTHelper
    {
        internal int clientId { get; set; }
        internal string sharedKey { get; set; }
        internal string username { get; set; }
        internal string token { get; set; }
        internal string database { get; set; }

        public JWTHelper(SystemUser su, string dbname)
        {
            clientId = 1;
            sharedKey = "abce";
            username = su.UserName;
            database = dbname;
            //currentUserId = Guid.Empty;
        }

        public JWTHelper(Customer su, string dbname)
        {
            clientId = 1;
            sharedKey = "abce";
            username = su.UserName;
            database = dbname;
            //currentUserId = Guid.Empty;
        }
        public JWTHelper(TAS.DataTransfer.Common.SecurityContext sc)
        {
            sharedKey = "abce";
            token = sc.Token;
        }

        public JWTHelper(string jwt)
        {
            sharedKey = "abce";
            token = jwt;
        }

        public string CreateAuthenticationToken(string systemUserID, string userName , string LoginMapId)
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
                { "sharedkey", this.sharedKey },
                { "userid",LoginMapId}
            };

            string token = JWT.JsonWebToken.Encode(payload, this.sharedKey, JWT.JwtHashAlgorithm.HS256);

            UserLoginEntityManager userLoginEntityManager = new UserLoginEntityManager();
            if (userLoginEntityManager.issueUserLogin(systemUserID, token, userName) == "OK")
            {
                return token;
            }
            else
                return "";

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
            UserLoginEntityManager userLoginEntityManager = new UserLoginEntityManager();
            Guid userId = Guid.Empty;
            if (userLoginEntityManager.CheckAndUpdateLogin(this.token, idleTimeSec,out userId))
            {
                return true;
            }
            else
            {
                //throw new CustomException("Invalid Token",(int)ErrorCode.TokenExpired);
                //throw new AccessViolationException();
                //this might need to be changed, coz in debug mode this causes an exception if the token is expired.
                //but in release mode it is returning a 403 http status code and angular identifies it as session expired
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }

        }

        public string CreateCustomerAuthenticationToken(string CustumerId, string userName)
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

            UserLoginEntityManager userLoginEntityManager = new UserLoginEntityManager();
            if (userLoginEntityManager.issueCustomerLogin(CustumerId, token, userName) == "OK")
            {
                return token;
            }
            else
                return "";
        }
    }
}
