using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common.Notification;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class ForgotPasswordCustomerUnitOfWork : UnitOfWork
    {
        public bool Result = false;
        private string currentSystemUserId = String.Empty;
        private ForgotPasswordCustomerRequestDto _forgotPasswordRequestDto;
        private CustomerEntityManager customerEntityManager;

        public ForgotPasswordCustomerUnitOfWork(ForgotPasswordCustomerRequestDto forgotPasswordRequestDto)
        {
            _forgotPasswordRequestDto = forgotPasswordRequestDto;
            customerEntityManager = new Entities.Management.CustomerEntityManager();
        }

        public override bool PreExecute()
        {
            bool response = false;
            try
            {
                if (!String.IsNullOrEmpty(_forgotPasswordRequestDto.email) && _forgotPasswordRequestDto.tpaId != null)
                {
                    TASEntitySessionManager.OpenSession();
                    string dbName = TASTPAEntityManager.GetTPADetailById(_forgotPasswordRequestDto.tpaId).FirstOrDefault().DBName;
                    if (!String.IsNullOrEmpty(dbName))
                    {
                        string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(_forgotPasswordRequestDto.tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                        if (!String.IsNullOrEmpty(tasTpaConnString))
                        {
                            dbConnectionString = tasTpaConnString;
                            EntitySessionManager.OpenSession(dbConnectionString);
                            currentSystemUserId = customerEntityManager.ValidateEmail(_forgotPasswordRequestDto.email);
                            if (!String.IsNullOrEmpty(currentSystemUserId))
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
                EntitySessionManager.CloseSession();
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
                }     //**
                SystemUserEntityManager systemUserEntityManager = new SystemUserEntityManager();
                ForgotPasswordRequest forgotPasswordRequest = systemUserEntityManager.RequestChangePassword(currentSystemUserId);
                if (forgotPasswordRequest != null)
                {
                    EmailService.Instance.SendEmail(GetMyEmail.Instance.ForgotPasswordCustomer(forgotPasswordRequest, _forgotPasswordRequestDto.email, _forgotPasswordRequestDto.tpaId, currentSystemUserId));
                }
                else
                {
                    throw new Exception();
                }
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
        }

    }
}
