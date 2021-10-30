using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class UserUpdationUnitOfWork : UnitOfWork
    {
        public UserRequestDto User;
        public UserResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public UserUpdationUnitOfWork(UserRequestDto User)
        {

            this.User = User;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            //UserEntityManager UserEntityManager = new UserEntityManager();
                            //var ce = UserEntityManager.GetUserById(User.Id);
                            //if (ce.IsUserExists == true)
                            //{
                            //    User.EntryDate = ce.EntryDate;
                            //    User.EntryBy = ce.EntryBy;
                            //    return true;
                            //}
                            //else
                            //{
                            //    return false;
                            //}
                            return true;
                        }
                        EntitySessionManager.CloseSession();

                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;
        }

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        UserEntityManager UserEntityManager = new UserEntityManager();
        //        var ce = UserEntityManager.GetUserById(User.Id);
        //        if (ce.IsUserExists == true)
        //        {
        //            User.EntryDate = ce.EntryDate;
        //            User.EntryBy = ce.EntryBy;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

        public override void Execute()
        {
            try
            {
                ICache cache = CacheFactory.GetCache();
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                UserEntityManager UserEntityManager = new UserEntityManager();
                //var ce = UserEntityManager.GetUserById(User.Id);
                //if (ce.IsUserExists == true)
                //{
                bool result = UserEntityManager.UpdateUser(User);
                this.User.UserInsertion = result;

                    cache.Remove(UniqueDbName + "_Users");
                    string uniqueCacheKey = UniqueDbName + "_DealerUserType_" + User.Id.ToString().ToLower();
                    cache.Remove(uniqueCacheKey);


                //}
                //else
                //{
                //    this.User.UserInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
