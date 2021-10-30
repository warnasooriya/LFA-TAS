using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ChangeForgotPasswordUnotOfWork:UnitOfWork
    {
        private readonly Guid requestId, tpaId, systemUserId = Guid.Empty;
        private readonly string password = string.Empty;
        public String Result = string.Empty;
        SystemUserEntityManager systemUserEntityManager;
        public ChangeForgotPasswordUnotOfWork(ChangePasswordForgotRequestDto changePasswordForgotRequestDto)
        {
            requestId = changePasswordForgotRequestDto.requestId;
            tpaId = changePasswordForgotRequestDto.tpaId;
            systemUserId = changePasswordForgotRequestDto.systemUserId;
            password = changePasswordForgotRequestDto.password;
            systemUserEntityManager = new Entities.Management.SystemUserEntityManager();
        }
        public override bool PreExecute()
        {

            bool response = false;
            try
            {
                if (tpaId != Guid.Empty && requestId != Guid.Empty
                    && systemUserId != Guid.Empty && !String.IsNullOrEmpty(password))
                {
                    TASEntitySessionManager.OpenSession();
                    string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                    if (!String.IsNullOrEmpty(dbName))
                    {
                        string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                        if (!String.IsNullOrEmpty(tasTpaConnString))
                        {
                            dbConnectionString = tasTpaConnString;
                            EntitySessionManager.OpenSession(dbConnectionString);
                            Result = systemUserEntityManager.ValidateChangePasswordRequestId(requestId);
                            if (!String.IsNullOrEmpty(Result))
                            {
                                response = true;
                            }
                        }
                    }
                }
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
            return response;
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
                }
                systemUserEntityManager = new SystemUserEntityManager();
                systemUserEntityManager.ChangeForgetPassword(requestId, systemUserId, password);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
