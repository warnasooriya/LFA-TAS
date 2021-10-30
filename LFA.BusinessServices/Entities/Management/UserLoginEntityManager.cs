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
using System.Reflection;
using NLog;
using TAS.Services.Common;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class UserLoginEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static UserLogin _UL { get; set; }

        internal static List<UserLogin> GetAllLoginsByUserId(string SystemUserId)
        {
            List<UserLogin> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<UserLogin> UserLogins = session.Query<UserLogin>();
            entities = UserLogins.ToList();
            return entities;
        }

        internal static List<UserLogin> GetAllLoginsByDateRange(DateTime from, DateTime to)
        {
            List<UserLogin> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<UserLogin> UserLogins = session.Query<UserLogin>().Where(a => a.IssuedDateTime >= from && a.IssuedDateTime <= to);
            entities = UserLogins.ToList();
            return entities;
        }

        internal  string issueUserLogin(string systemUserId, string jwt,string username )
        {
            try
            {
               // List<UserLogin> entities = null;
                ISession session = EntitySessionManager.GetSession();
               // IQueryable<UserLogin> userLogins = session.Query<UserLogin>().Where(w => w.SystemUserID == systemUserId && w.IsExpired == false );

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


              //  if (userLogins.Count() == 0)
              //  {
                    UserLogin UserLogin = new Entities.UserLogin();
                    UserLogin.ID = Guid.NewGuid();
                    UserLogin.SystemUserID = systemUserId;
                    UserLogin.JwtToken = jwt;
                    UserLogin.UserName = username;
                    UserLogin.IssuedDateTime = DateTime.Now;
                    UserLogin.LastRequestDateTime = UserLogin.IssuedDateTime;
                    UserLogin.IsExpired = false;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(UserLogin);
                        transaction.Commit();
                    }
              //  }

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return ex.Message;
            }
        }

        internal bool CheckAndUpdateLogin(string jwt, int requestTimeout, out Guid userId)
        {
            try
            {
                List<UserLogin> entities = null;
                ISession session = EntitySessionManager.GetSession();
                UserLogin userLogin = session.Query<UserLogin>().Where(w => w.JwtToken == jwt && w.IsExpired == false).FirstOrDefault();
                userId = Guid.Empty;
                if (userLogin != null)
                {
                    if ((DateTime.Now - userLogin.LastRequestDateTime).TotalSeconds < requestTimeout){
                        userLogin.LastRequestDateTime = DateTime.Now;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(userLogin);
                            transaction.Commit();
                        }
                        userId = Guid.Parse(userLogin.SystemUserID);
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
                userId = Guid.Empty;
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal string Logout(string jwt)
        {
            try
            {
                List<UserLogin> entities = null;
                ISession session = EntitySessionManager.GetSession();
                UserLogin userLogin = session.Query<UserLogin>().Where(w => w.JwtToken == jwt && w.IsExpired == false).FirstOrDefault();

                if (userLogin != null)
                {
                    userLogin.LastRequestDateTime = DateTime.Now;
                    userLogin.IsExpired = true;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(userLogin);
                        transaction.Commit();
                    }
                    return "ok";
                }else
                {
                    return "error";
                }
               
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                              ex.InnerException);
                return "error";
            }
        }

        internal bool UserMenuAuthorize(string userId,string menuName, string unitOfWorkName)
        {
                Guid userid;
                try
                {
                    if (Guid.TryParse(userId, out userid) && menuName != null && unitOfWorkName != null)
                    {
                        ISession session = EntitySessionManager.GetSession();
                        //Guid UserId = session.Query<SystemUser>().Where(a => a.UserName == userid).First().LoginMapId;

                        //var x = session.Query<UnitOfWorks>().ToList();

                        var ss = (from RoleMenu in session.Query<RoleMenuMapping>()
                                  join RoleUserMapping in session.Query<UserRoleMapping>() on RoleMenu.RoleId equals RoleUserMapping.RoleId
                                  join menu in session.Query<Menu>() on RoleMenu.MenuId equals menu.Id
                                  join unitOfWork in session.Query<Entities.UnitOfWorks>() on RoleMenu.LevelId equals unitOfWork.PriviledgeLevelId
                                  where RoleUserMapping.UserId == userid && unitOfWork.UnitOfWorkName.Contains(unitOfWorkName) && menu.LinkURL.Contains(menuName)
                                  select RoleMenu).ToList();


                        if (ss.Count > 0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }
            
            return false;
        }

        internal string issueCustomerLogin(string CustumerId, string token, string userName)
        {
            try
            {               
                ISession session = EntitySessionManager.GetSession();

                CustomerLogin CustomerLogin = new Entities.CustomerLogin();
                CustomerLogin.Id = Guid.NewGuid();
                CustomerLogin.CustomerUserID = CustumerId;
                CustomerLogin.JwtToken = token;
                CustomerLogin.CustomerName = userName;
                CustomerLogin.IssuedDateTime = DateTime.Now;
                CustomerLogin.LastRequestDateTime = DateTime.Now;
                CustomerLogin.IsExpired = false;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(CustomerLogin);
                    transaction.Commit();
                }                

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return ex.Message;
            }
        }
    }
}
