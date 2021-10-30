using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CurrencyEmailsRetrievalUnitOfWork : UnitOfWork
    {
       

        public CurrencyEmailsResponseDto Result
        {
            get;
            private set;
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
                CurrencyEntityManager CurrencyEntityManager = new CurrencyEntityManager();
                List<CurrencyEmail> CurrencyEmailEntities = CurrencyEntityManager.GetCurrencyEmails();

				
                CurrencyEmailsResponseDto result = new CurrencyEmailsResponseDto();
                result.CurrencyEmails = new List<CurrencyEmailResponseDto>();
                foreach (var CurrencyEmail in CurrencyEmailEntities)
                {
                    CurrencyEmailResponseDto pr = new CurrencyEmailResponseDto();

                    pr.Id = CurrencyEmail.Id;
                    pr.AdminEmail = CurrencyEmail.AdminEmail;
                    pr.FirstDurationType = CurrencyEmail.FirstDurationType;
                    pr.FirstEmailDuration = CurrencyEmail.FirstEmailDuration;
                    pr.FirstMailBody = CurrencyEmail.FirstMailBody;
                    pr.FirstMailSubject = CurrencyEmail.FirstMailSubject;
                    pr.LastDurationType = CurrencyEmail.LastDurationType;
                    pr.LastEmailDuration = CurrencyEmail.LastEmailDuration;
                    pr.LastMailBody = CurrencyEmail.LastMailBody;
                    pr.LastMailSubject = CurrencyEmail.LastMailSubject;
                    pr.SecoundDurationType = CurrencyEmail.SecoundDurationType;
                    pr.SecoundEmailDuration = CurrencyEmail.SecoundEmailDuration;
                    pr.SecoundMailBody = CurrencyEmail.SecoundMailBody;
                    pr.SecoundMailSubject = CurrencyEmail.SecoundMailSubject;
                    pr.TPAEmail = CurrencyEmail.TPAEmail;

					
                    //need to write other fields
                    result.CurrencyEmails.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
