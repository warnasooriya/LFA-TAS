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
    internal sealed class ChangeForgotPasswordCustomerUnotOfWork : UnitOfWork
    {
        private readonly Guid requestId, tpaId,currentCustomerId, systemUserId = Guid.Empty;
        private readonly string password = string.Empty;
        public String Result = string.Empty;
        CustomerEntityManager customerEntityManager;

        public ChangeForgotPasswordCustomerUnotOfWork(ChangePasswordForgotCustomerRequestDto changePasswordForgotCustomerRequestDto)
        {
            requestId = changePasswordForgotCustomerRequestDto.requestId;
            tpaId = changePasswordForgotCustomerRequestDto.tpaId;
            systemUserId = changePasswordForgotCustomerRequestDto.systemUserId;
            password = changePasswordForgotCustomerRequestDto.password;
            customerEntityManager = new Entities.Management.CustomerEntityManager();
            currentCustomerId = changePasswordForgotCustomerRequestDto.currentCustomerId;
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
                            Result = customerEntityManager.ValidateChangePasswordRequestId(requestId);
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
                customerEntityManager = new CustomerEntityManager();
                customerEntityManager.ChangeForgetPasswordCustomer(requestId, systemUserId,currentCustomerId, password);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
