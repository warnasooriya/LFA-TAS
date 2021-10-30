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
    public class TASSystemUserEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<SystemUser> GetUsers()
        {
            List<SystemUser> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<SystemUser> userData = session.Query<SystemUser>();

            // TODO: get skip/take values as params
            int recordsTotal = userData.Count();
            userData = userData.Skip(0).Take(10);
            entities = userData.ToList();
            return entities;
        }

        //public SystemUserResponseDto GetUserById(string UserId)
        //{
        //    ISession session = EntitySessionManager.GetSession();

        //    SystemUserResponseDto pDto = new SystemUserResponseDto();

        //    var query =
        //        from User in session.Query<SystemUser>()
        //        where User.Id == UserId
        //        select new { User = User };

        //    var result = query.ToList();


        //    if (result != null && result.Count > 0)
        //    {
        //        pDto.Id = result.First().User.Id;
        //        pDto.Address1 = result.First().User.Address1;
        //        pDto.Address2 = result.First().User.Address2;
        //        pDto.Address3 = result.First().User.Address3;
        //        pDto.Address4 = result.First().User.Address4;
        //        pDto.CountryId = result.First().User.CountryId;
        //        pDto.DateOfBirth = result.First().User.DateOfBirth;
        //        pDto.DLIssueDate = result.First().User.DLIssueDate;
        //        pDto.Email = result.First().User.Email;
        //        pDto.FirstName = result.First().User.FirstName;
        //        pDto.Gender = result.First().User.Gender;
        //        pDto.IDNo = result.First().User.IDNo;
        //        pDto.IDTypeId = result.First().User.IDTypeId;
        //        pDto.InternalExtension = result.First().User.InternalExtension;
        //        pDto.IsActive = result.First().User.IsActive;
        //        pDto.LastName = result.First().User.LastName;
        //        pDto.MobileNo = result.First().User.MobileNo;
        //        pDto.NationalityId = result.First().User.NationalityId;
        //        pDto.OtherTelNo = result.First().User.OtherTelNo;
        //        pDto.Password = result.First().User.Password;
        //        pDto.ProfilePicture = result.First().User.ProfilePicture;
        //        pDto.UserName = result.First().User.UserName;
        //        pDto.EntryDate = result.First().User.EntryDate;
        //        pDto.EntryBy = result.First().User.EntryBy;

        //        pDto.IsUserExists = true;
        //        return pDto;
        //    }
        //    else
        //    {
        //        pDto.IsUserExists = false;
        //        return null;
        //    }
        //}



        //internal bool AddUser(SystemUserRequestDto SystemUser)
        //{
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        SystemUser su = new Entities.SystemUser();
        //        su.DateOfBirth = SystemUser.DateOfBirth;
        //        su.EntryBy = "ranga";
        //        su.EntryDate = DateTime.Now;
        //        su.FirstName = SystemUser.FirstName;
        //        su.Id = Guid.NewGuid().ToString();
        //        su.IsActive = SystemUser.IsActive;
        //        su.LastName = SystemUser.LastName;
        //        su.Password = SystemUser.Password;
        //        su.RecordVersion = 1;
        //        su.SequanceNumber = session.QueryOver<SystemUser>().Select(Projections.Max<SystemUser>(x => x.SequanceNumber))
        //            .SingleOrDefault<int>();
        //        su.Email = SystemUser.Email;
        //        su.UserName = SystemUser.UserName;
        //        su.NationalityId = SystemUser.NationalityId;
        //        su.CountryId = SystemUser.CountryId;
        //        su.MobileNo = SystemUser.MobileNo;
        //        su.OtherTelNo = SystemUser.OtherTelNo;
        //        su.InternalExtension = SystemUser.InternalExtension;
        //        su.Gender = SystemUser.Gender;
        //        su.Address1 = SystemUser.Address1;
        //        su.Address2 = SystemUser.Address2;
        //        su.Address3 = SystemUser.Address3;
        //        su.Address4 = SystemUser.Address4;
        //        su.IDNo = SystemUser.IDNo;
        //        su.IDTypeId = SystemUser.IDTypeId;
        //        su.DLIssueDate = SystemUser.DLIssueDate;
        //        using (ITransaction transaction = session.BeginTransaction())  
        //        {  
        //            session.SaveOrUpdate(su);  
        //            transaction.Commit();  
        //        }  
                
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //internal bool UpdateUser(SystemUserRequestDto User)
        //{
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        SystemUser pr = new Entities.SystemUser();

        //        pr.Id = User.Id;
        //        pr.FirstName = User.FirstName;
        //        pr.LastName = User.LastName;
        //        pr.UserName = User.UserName;
        //        pr.Password = User.Password;
        //        pr.NationalityId = User.NationalityId;
        //        pr.CountryId = User.CountryId;
        //        pr.DateOfBirth = User.DateOfBirth;
        //        pr.MobileNo = User.MobileNo;
        //        pr.OtherTelNo = User.OtherTelNo;
        //        pr.InternalExtension = User.InternalExtension;
        //        pr.Gender = User.Gender;
        //        pr.Address1 = User.Address1;
        //        pr.Address2 = User.Address2;
        //        pr.Address3 = User.Address3;
        //        pr.Address4 = User.Address4;
        //        pr.IDNo = User.IDNo;
        //        pr.IDTypeId = User.IDTypeId;
        //        pr.DLIssueDate = User.DLIssueDate;
        //        pr.Email = User.Email;
        //        pr.IsActive = User.IsActive;
        //        pr.EntryDate = User.EntryDate;
        //        pr.EntryBy = User.EntryBy;

        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            session.Update(pr);

        //            transaction.Commit();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        internal LoginResponseDto LoginAuth(LoginRequestDto LoginRequest)
        {
            try
            {
                LoginResponseDto LoginResponse = new LoginResponseDto();
                ISession session = TASEntitySessionManager.GetSession();
                TASSystemUser TASSystemUser = new TASSystemUser();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    TASSystemUser = session.QueryOver<TASSystemUser>()
                        .Where(x => x.UserName == LoginRequest.UserName && x.Password == LoginRequest.Password)
                        .SingleOrDefault();
                }

                if (TASSystemUser != null)
                {
                    TASSystemUser.UserName = TASSystemUser.UserName == null ? "" :TASSystemUser.UserName;

                    TASJWTHelper JWTHelper = new Common.TASJWTHelper(TASSystemUser,"TAS");
                    LoginResponse.JsonWebToken = JWTHelper.CreateAuthenticationToken(TASSystemUser.Id.ToString(),TASSystemUser.UserName);
                    LoginResponse.IsValid = true;
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
