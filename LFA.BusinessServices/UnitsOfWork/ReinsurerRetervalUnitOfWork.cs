using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ReinsurerUserRetervalUnitOfWork:UnitOfWork
    {
        private readonly Guid _userId;
        public UserResponseDto Result
        {
            get;
            private set;
        }
        public ReinsurerUserRetervalUnitOfWork(Guid userId)
        {
            _userId = userId;
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
                UserEntityManager UserEntityManager = new UserEntityManager();
                UserResponseDto UserEntity = UserEntityManager.GetUserById(_userId.ToString());
                if (UserEntity == null)
                    UserEntity = new UserResponseDto() { IsUserExists = false };
                if (UserEntity.IsUserExists == null || UserEntity.IsUserExists == false)
                {
                    UserEntity.IsUserExists = false;
                }
                else
                {
                    UserEntity.IsUserExists = true;
                }

                this.Result = UserEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
