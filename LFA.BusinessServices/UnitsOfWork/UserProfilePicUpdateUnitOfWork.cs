using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Caching;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class UserProfilePicUpdateUnitOfWork:UnitOfWork
    {
        Guid _userId, _imageId;
        public ImageResponseDto Result
        {
            get;
            private set;
        }
        public UserProfilePicUpdateUnitOfWork(Guid userId, Guid imageId)
        {
            _userId = userId;
            _imageId = imageId;
        }
        private string UniqueDbName = string.Empty;
        public override bool PreExecute()
        {
             try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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

        public override void Execute()
        {
            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**


                //EntitySessionManager.OpenSession();
                UserEntityManager UserEntityManager = new UserEntityManager();
                ImageResponseDto imageEntity = UserEntityManager.UpdateUserProfilePic(_userId, _imageId);
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_DealerUserType_" + _userId.ToString().ToLower();
                cache.Remove(uniqueCacheKey);
                this.Result = imageEntity;

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
