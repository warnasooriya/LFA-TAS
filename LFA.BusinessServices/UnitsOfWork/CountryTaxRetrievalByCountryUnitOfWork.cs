﻿using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CountryTaxRetrievalByCountryUnitOfWork: UnitOfWork
    {
        public CountryTaxessResponseDto Result
        {
            get;
            private set;
        }

        private string UniqueDbName = string.Empty;
        private Guid countryId;
        public CountryTaxRetrievalByCountryUnitOfWork(Guid countryId) {
            this.countryId = countryId;
        }
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



                List<CountryTaxesResponseDto> countryTaxes = EntityCacheData.GetCountryTaxesByCountryId(UniqueDbName, this.countryId);
                CountryTaxessResponseDto result = new CountryTaxessResponseDto();
                result.CountryTaxes = countryTaxes;
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
