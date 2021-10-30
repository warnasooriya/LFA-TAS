using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TAS.Services.Entities.Management;
using TAS.DataTransfer.Exceptions;
using TAS.DataTransfer.Enum;
using System.Web.Http;
using System.Diagnostics;
using System.Reflection;
using TAS.Services.Entities.Persistence;


namespace TAS.Services.Common
{
    public class AuthorizeUOW
    {
        internal TAS.DataTransfer.Common.SecurityContext secCon { get; set; }
        internal string unitOfWorkName { get; set; }
        internal string userName { get; set; }
        internal string ConnectionString { get; set; }

        public AuthorizeUOW(TAS.DataTransfer.Common.SecurityContext sc, string Username, string UnitOfWorkName, string ConString)
        {
            secCon = sc;
            var stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            //string methodName = method.Name;
            //string className = method.ReflectedType.Name;

            unitOfWorkName = UnitOfWorkName;//method.ReflectedType.Name;
            userName = Username;
            ConnectionString = ConString;
            //currentUserId = Guid.Empty;
        }
       

        public bool checkAuthorization()
        {
            EntitySessionManager.OpenSession(ConnectionString);
            bool b;
            try
            {
                UserLoginEntityManager userLoginEntityManager = new UserLoginEntityManager();

                if (userName != "" && unitOfWorkName != "" && ConnectionString != "")
                {

                    if (userLoginEntityManager.UserMenuAuthorize(secCon.UserId, secCon.MenuName, unitOfWorkName))
                    {
                        b = true;
                    }
                }
                b = false;  // set false after working properly
            }
            catch (Exception ex)
            {
                b = false;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return b;
        }
    }
}
