using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ChangePasswordUnitOfWork:UnitOfWork
    {
        string OldPassword,NewPassword=String.Empty;
        Guid UserId;
        public bool Result = false;
        public ChangePasswordUnitOfWork(ChangePasswordRequestDto changePasswordRequestDto)
        {
            OldPassword = changePasswordRequestDto.oldPassword;
            NewPassword = changePasswordRequestDto.newPassword;
            UserId = changePasswordRequestDto.userId;

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
                            //backend validation
                            if (!String.IsNullOrEmpty(OldPassword) && !String.IsNullOrEmpty(NewPassword) && UserId!=null)
                            {
                                return true;
                            }
                            
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
                UserEntityManager userEntityManager = new UserEntityManager();
                bool result = userEntityManager.ChangePassword(OldPassword,NewPassword,UserId);
                if (result)
                {
                    this.Result = true;
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
