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
    internal sealed class CurrencyConversionPeriodsRetrievalUnitOfWork : UnitOfWork
    {
       

        public CurrencyConversionPeriodsResponseDto Result
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
                List<CurrencyConversionPeriods> CurrencyConversionPeriodEntities = CurrencyEntityManager.GetCurrencyConversionPeriods();

				
                CurrencyConversionPeriodsResponseDto result = new CurrencyConversionPeriodsResponseDto();
                result.CurrencyConversionPeriods = new List<CurrencyConversionPeriodResponseDto>();
                foreach (var CurrencyConversionPeriod in CurrencyConversionPeriodEntities)
                {
                    CurrencyConversionPeriodResponseDto pr = new CurrencyConversionPeriodResponseDto();

                    pr.Id = CurrencyConversionPeriod.Id;
                    pr.FromDate = CurrencyConversionPeriod.FromDate;
                    pr.ToDate = CurrencyConversionPeriod.ToDate;
                    pr.Description = CurrencyConversionPeriod.Description;
					
                    //need to write other fields
                    result.CurrencyConversionPeriods.Add(pr);
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
