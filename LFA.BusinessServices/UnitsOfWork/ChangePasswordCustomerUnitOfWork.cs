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
    internal sealed class ChangePasswordCustomerUnitOfWork : UnitOfWork
    {
        string OldPassword,NewPassword=String.Empty;
        Guid CustomerId;
        public bool Result = false;
        public ChangePasswordCustomerUnitOfWork(ChangePasswordCustomerRequestDto changePasswordRequestDto)
        {
            OldPassword = changePasswordRequestDto.oldPassword;
            NewPassword = changePasswordRequestDto.newPassword;
            CustomerId = changePasswordRequestDto.userId;

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
                            if (!String.IsNullOrEmpty(OldPassword) && !String.IsNullOrEmpty(NewPassword) && CustomerId != null)
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
                CustomerEntityManager customerEntityManager = new CustomerEntityManager();
                bool result = customerEntityManager.ChangePassword(OldPassword, NewPassword, CustomerId);
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
