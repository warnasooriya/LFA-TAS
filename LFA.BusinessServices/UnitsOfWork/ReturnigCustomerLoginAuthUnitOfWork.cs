using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ReturnigCustomerLoginAuthUnitOfWork : UnitOfWork
    {
         CustomerLoginRequestDto LoginRequest;
        private string dbName
        {
            get;
            set;
        }

        public CustomerLoginResponseDto Result
        {
            get;
            private set;
        }
        public ReturnigCustomerLoginAuthUnitOfWork(CustomerLoginRequestDto LoginRequest)
        {
            this.LoginRequest = LoginRequest;
            this.Result = new CustomerLoginResponseDto();
        }

        public override bool PreExecute()
        {

            try
            {
                if (LoginRequest.UserName.Length > 0 && LoginRequest.Password.Length > 0 && LoginRequest.tpaID.Length > 0)
                {
                    TASEntitySessionManager.OpenSession();
                    List<TASTPA> tasTpa = new List<TASTPA>();
                    tasTpa = TASTPAEntityManager.GetTPADetailById(Guid.Parse(LoginRequest.tpaID));
                    TASEntitySessionManager.CloseSession();
                    if (tasTpa.FirstOrDefault() != null)
                    {
                        if (tasTpa.First().DBConnectionString != null && tasTpa.First().DBName != null && tasTpa.First().DBConnectionString != "" && tasTpa.First().DBName != "")
                        {
                            dbConnectionString = tasTpa.First().DBConnectionString;
                            dbName = tasTpa.First().DBName;
                            this.Result.IsValid = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                else
                {
                    Result.IsValid = false;
                    this.Result.JsonWebToken = null;
                    return false;
                }
            }
            catch (Exception e)
            {
                Result.IsValid = false;
                this.Result.JsonWebToken = null;
                return false;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
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


                SystemUserEntityManager systemUserEntityManager = new SystemUserEntityManager();
                CustomerLoginResponseDto result = systemUserEntityManager.CustomerLoginAuth(LoginRequest, dbName);
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
