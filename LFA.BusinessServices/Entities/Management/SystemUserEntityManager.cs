using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using NLog;
using System.Reflection;


namespace TAS.Services.Entities.Management
{
    public class SystemUserEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<SystemUser> GetUsers()
        {
            List<SystemUser> entities = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<SystemUser> userData = session.Query<SystemUser>();

                // TODO: get skip/take values as params
                int recordsTotal = userData.Count();
                userData = userData.Skip(0).Take(10);
                entities = userData.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return entities;
        }

        public SystemUserResponseDto GetUserById(string UserId)
        {
            try
            {
                SystemUserResponseDto pDto = new SystemUserResponseDto();
                if (String.IsNullOrEmpty(UserId) || Guid.Parse(UserId) == Guid.Empty)
                {
                    pDto.IsUserExists = false;
                    return null;
                }
                ISession session = EntitySessionManager.GetSession();
                var query =
                    from User in session.Query<SystemUser>()
                    where User.LoginMapId == Guid.Parse(UserId)
                    select new { User = User };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().User.Id;
                    pDto.Email = result.First().User.UserName;
                    pDto.IsActive = result.First().User.IsActive;
                    pDto.Password = result.First().User.Password;
                    pDto.EntryDate = result.First().User.EntryDate;
                    pDto.EntryBy = result.First().User.EntryBy;
                    pDto.EntryBy = result.First().User.EntryBy;
                    pDto.EntryBy = result.First().User.EntryBy;
                    pDto.UserTypeId = result.First().User.UserTypeId;
                    pDto.LoginMapId = result.First().User.LoginMapId;
                    pDto.LanguageId = result.First().User.LanguageId;
                    pDto.IsUserExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsUserExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }



        internal bool AddUser(SystemUserRequestDto SystemUser)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                SystemUser su = new Entities.SystemUser();

                su.EntryBy = "ranga";
                su.EntryDate = DateTime.Now;
                su.Id = Guid.NewGuid().ToString();
                su.IsActive = SystemUser.IsActive;
                su.Password = SystemUser.Password;
                su.RecordVersion = 1;
                su.SequanceNumber = session.QueryOver<SystemUser>().Select(Projections.Max<SystemUser>(x => x.SequanceNumber)).SingleOrDefault<int>();
                su.UserName = SystemUser.UserName;
                su.UserTypeId = SystemUser.UserTypeId;
                su.LoginMapId = SystemUser.LoginMapId;
                su.LanguageId = SystemUser.LanguageId;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(su);
                    SystemUser.Id = su.Id;
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }


        internal bool AddTempUser(SystemUserRequestDto SystemUser)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                SystemUser su = new Entities.SystemUser();
                su.EntryBy = "chathura";
                su.EntryDate = DateTime.Now;
                su.Id = Guid.NewGuid().ToString();
                su.IsActive = true;
                su.Password = SystemUser.Password;
                su.RecordVersion = 1;
                su.SequanceNumber = session.QueryOver<SystemUser>().Select(Projections.Max<SystemUser>(x => x.SequanceNumber)).SingleOrDefault<int>();
                su.UserName = SystemUser.UserName;
                su.UserTypeId = SystemUser.UserTypeId;
                su.LoginMapId = SystemUser.LoginMapId;
                su.LanguageId = SystemUser.LanguageId;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(su);
                    SystemUser.Id = su.Id;
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateUser(SystemUserRequestDto User)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();


                SystemUser systemUser = session.Query<SystemUser>().Where(a => a.LoginMapId == Guid.Parse(User.Id)).FirstOrDefault();
                SystemUser pr = new Entities.SystemUser();

                pr.Id = systemUser.Id;
                pr.Password = User.Password;
                pr.UserTypeId = User.UserTypeId;
                pr.LoginMapId = Guid.Parse(User.Id);
                pr.UserName = User.UserName;
                pr.IsActive = User.IsActive;
                pr.EntryDate = User.EntryDate;
                pr.EntryBy = User.EntryBy;
                pr.LanguageId = User.LanguageId;
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    systemUser.Id = pr.Id;
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal LoginResponseDto LoginAuth(LoginRequestDto LoginRequest, string dbName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<UserType> entities = null;
                LoginResponseDto LoginResponse = new LoginResponseDto();
                SystemUser systemUser = new SystemUser();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    systemUser = session.QueryOver<SystemUser>()
                        .Where(x => x.UserName == LoginRequest.UserName && x.Password == LoginRequest.Password)
                        .SingleOrDefault();
                }
                if (systemUser != null)
                {


                    JWTHelper JWTHelper = new Common.JWTHelper(systemUser, dbName);
                    systemUser.UserName = systemUser.UserName == null ? "" : systemUser.UserName;
                    string token = JWTHelper.CreateAuthenticationToken(systemUser.Id, systemUser.UserName , systemUser.LoginMapId.ToString());

                    if (token.Length > 0)
                    {
                        LoginResponse.JsonWebToken = token;
                        LoginResponse.IsValid = true;
                        LoginResponse.LoggedInUserId = systemUser.LoginMapId;
                    }
                    else
                    {
                        LoginResponse.JsonWebToken = null;
                    }
                    //if (systemUser.UserTypeId.ToString() == "00000000-0000-0000-0000-000000000000")//Remove later when data is entered
                    //    LoginResponse.UserType = "IU";
                    //else
                    LoginResponse.UserType = session.Query<UserType>().FirstOrDefault(u => u.Id == systemUser.UserTypeId).Code;
                    if (systemUser.LanguageId != null && systemUser.LanguageId != Guid.Empty)
                    {
                        var langauge = session.Query<SystemLanguage>().FirstOrDefault(u => u.Id == systemUser.LanguageId);
                        LoginResponse.LanguageCode = langauge != null ? langauge.LanguageCode : "";
                    }

                }
                else
                {
                    LoginResponse.JsonWebToken = null;
                }
                return LoginResponse;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }

        }


        internal ForgotPasswordRequest RequestChangePassword(string currentSystemUserId)
        {
            ForgotPasswordRequest _Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                var query =
                    from ChangePasswordRequest in session.Query<ForgotPasswordRequest>()
                    where (ChangePasswordRequest.SystemUserId == currentSystemUserId) &&
                    (ChangePasswordRequest.IsUsed == false) && (ChangePasswordRequest.ExpiryTime >= DateTime.Now)
                    select new { ChangePasswordRequest = ChangePasswordRequest };
                //force expriring exiting request records
                var result = query.ToList();
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {

                        item.ChangePasswordRequest.ExpiryTime = DateTime.Now;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(item.ChangePasswordRequest, item.ChangePasswordRequest.Id);
                            transaction.Commit();
                        }
                    }
                }

                //create new record for this request
                ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest()
                {
                    Id = Guid.NewGuid(),
                    ExpiryTime = DateTime.Now.AddHours(ConfigurationData.ForgotPasswordLinkExpiryHours),
                    IsUsed = false,
                    PreviousPassword = null,
                    RequestedTime = DateTime.Now,
                    SystemUserId = currentSystemUserId,
                    TempKey = null
                };
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(forgotPasswordRequest);
                    transaction.Commit();
                }
                _Response = forgotPasswordRequest;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal string ValidateChangePasswordRequestId(Guid RequestId)
        {
            string _Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ForgotPasswordRequest currentRequest = session.QueryOver<ForgotPasswordRequest>()
                    .Where(a => a.Id == RequestId).SingleOrDefault();
                if (currentRequest != null && currentRequest.IsUsed == false && currentRequest.ExpiryTime >= DateTime.Now)
                {
                    _Response = currentRequest.SystemUserId;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal void ChangeForgetPassword(Guid requestId, Guid systemUserId, string password)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ForgotPasswordRequest currentRequest = session.QueryOver<ForgotPasswordRequest>()
                    .Where(a => a.Id == requestId).SingleOrDefault();
                SystemUser sUser = session.QueryOver<SystemUser>()
                    .Where(a => a.LoginMapId == systemUserId).SingleOrDefault();
                InternalUser iUser = session.QueryOver<InternalUser>()
                    .Where(a => a.Id == systemUserId.ToString()).SingleOrDefault();

                if (currentRequest != null && currentRequest.IsUsed == false && currentRequest.ExpiryTime >= DateTime.Now)
                {
                    //update request table
                    currentRequest.IsUsed = true;
                    currentRequest.PreviousPassword = sUser.Password;
                    //update system user
                    sUser.Password = password;
                    //update internal user
                    iUser.Password = password;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(currentRequest);
                        session.Update(sUser);
                        session.Update(iUser);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
        }

        internal CustomerLoginResponseDto CustomerLoginAuth(CustomerLoginRequestDto LoginRequest, string dbName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                string CustumerId;

                List<UserType> entities = null;
                IQueryable<UserType> userData = session.Query<UserType>();
                entities = userData.ToList();

                CustomerLoginResponseDto LoginResponse = new CustomerLoginResponseDto();
                Customer customer = new Customer();
                //SystemUser systemUser = new SystemUser();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    customer = session.QueryOver<Customer>()
                        .Where(x => x.UserName == LoginRequest.UserName && x.Password == LoginRequest.Password).SingleOrDefault();

                    //systemUser = session.QueryOver<SystemUser>()
                    //    .Where(x => x.UserName == LoginRequest.UserName && x.Password == LoginRequest.Password)
                    //    .SingleOrDefault();
                }
                if (customer != null)
                {
                    CustumerId = customer.Id.ToString();

                    JWTHelper JWTHelper = new Common.JWTHelper(customer, dbName);
                    customer.UserName = customer.UserName == null ? "" : customer.UserName;
                    string token = JWTHelper.CreateCustomerAuthenticationToken(CustumerId, customer.UserName);
                    CommonEntityManager CommonEntityManager = new CommonEntityManager();

                    if (token.Length > 0)
                    {
                        LoginResponse.JsonWebToken = token;
                        LoginResponse.IsValid = true;
                        Guid LogId = Guid.Parse(LoginRequest.tpaID);
                        LoginResponse.LoggedInUserId = LogId;
                        LoginResponse.LoggedInCustomerId = Guid.Parse(CustumerId);
                        LoginResponse.LoggedInCustomerName = CommonEntityManager.GetCustomerNameById(Guid.Parse(CustumerId));
                    }
                    else
                    {
                        LoginResponse.JsonWebToken = null;
                    }
                    //if (systemUser.UserTypeId.ToString() == "00000000-0000-0000-0000-000000000000")//Remove later when data is entered
                    //    LoginResponse.UserType = "IU";
                    //else
                    //LoginResponse.UserType = entities.Find(u => u.Id == customer.CustomerTypeId).Code;

                }
                else
                {
                    LoginResponse.JsonWebToken = null;
                }
                return LoginResponse;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
        }
    }
}
