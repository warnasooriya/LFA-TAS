using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TAS.Services.Common;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class TASUserLoginEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TASUserLogin _UL { get; set; }

        internal static List<TASUserLogin> GetAllLoginsByUserId(string SystemUserId)
        {
            List<TASUserLogin> entities = null;
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASUserLogin> UserLogins = session.Query<TASUserLogin>();
            entities = UserLogins.ToList();
            return entities;
        }

        internal static List<TASUserLogin> GetAllLoginsByDateRange(DateTime from, DateTime to)
        {
            List<TASUserLogin> entities = null;
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASUserLogin> UserLogins = session.Query<TASUserLogin>().Where(a => a.IssuedDateTime >= from && a.IssuedDateTime <= to);
            entities = UserLogins.ToList();
            return entities;
        }

        internal  string issueUserLogin(string systemUserId, string jwt,string username )
        {
            try
            {
                List<TASUserLogin> entities = null;
                ISession session = TASEntitySessionManager.GetSession();
               // IQueryable<TASUserLogin> userLogins = session.Query<TASUserLogin>().Where(w => w.SystemUserID == systemUserId && w.IsExpired == false);

               // if (userLogins != null)
               // {

                   
                //if (userLogins.Count() > 0)
                //{
                //    using (ITransaction transaction = session.BeginTransaction())
                //    {
                //        foreach (var userLogin in userLogins)
                //        {
                //            userLogin.IsExpired = true;
                //            session.Update(userLogin);
                //        }
                //        transaction.Commit();
                //    }
                //}


               // if (userLogins.Count() == 0)
               // {
                    TASUserLogin UserLogin = new Entities.TASUserLogin();
                    UserLogin.ID = Guid.NewGuid();
                    UserLogin.SystemUserID = systemUserId;
                    UserLogin.JwtToken = jwt;
                    UserLogin.UserName = username;
                    UserLogin.IssuedDateTime = DateTime.Now;
                    UserLogin.LastRequestDateTime = UserLogin.IssuedDateTime;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(UserLogin);
                        transaction.Commit();
                    }
               // }

               // }
                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return ex.Message;
            }
        }

        internal bool CheckAndUpdateLogin(string jwt, int requestTimeout)
        {
            try
            {
                List<TASUserLogin> entities = null;
                ISession session = TASEntitySessionManager.GetSession();
                TASUserLogin userLogin = session.Query<TASUserLogin>().Where(w => w.JwtToken == jwt && w.IsExpired == false).FirstOrDefault();
                if (userLogin != null)
                {
                    if ((DateTime.Now - userLogin.LastRequestDateTime).TotalSeconds < requestTimeout)
                    {
                        userLogin.LastRequestDateTime = DateTime.Now;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(userLogin);
                            transaction.Commit();
                        }
                        return true;
                    }
                    else
                    {
                        userLogin.LastRequestDateTime = DateTime.Now;
                        userLogin.IsExpired = true;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(userLogin);
                            transaction.Commit();
                        }
                        return false;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool Logout(string jwt)
        {
            try
            {
                List<TASUserLogin> entities = null;
                ISession session = TASEntitySessionManager.GetSession();
                TASUserLogin userLogin = session.Query<TASUserLogin>().Where(w => w.JwtToken == jwt && w.IsExpired == false).FirstOrDefault();

                if (userLogin != null)
                {

                        userLogin.LastRequestDateTime = DateTime.Now;
                        userLogin.IsExpired = true;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(userLogin);
                            transaction.Commit();
                        }
                        return true;

                }
                return false;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }
    }
}
