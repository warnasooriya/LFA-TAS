using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CitiesRetrievalUnitOfWork : UnitOfWork
    {
        public Guid countryId = Guid.Empty;
        private string UniqueDbName = string.Empty;
        public CitiesResponseDto Result
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
            if (countryId != Guid.Empty)
            {
                // TODO: process for a specific country
            }
            else
            {
                // TODO: process for all countries
            }


            //  List<CityResponseDto> cityDtos = null;
            List<CityResponseDto> cityEntities = null;

            // TODO: retrieve from CacheData
            // if not in cache, go to Entity Manager -> then place in cache

            //if (countryId != Guid.Empty)
            //{
            //    cityDtos = CacheData.GetCitiesByCountryId(countryId);

            //}

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

                if (countryId != Guid.Empty)
                {
                    cityEntities = EntityCacheData.GetCities(countryId, UniqueDbName);
                }
                else
                {
                    cityEntities = EntityCacheData.GetCities(UniqueDbName);
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }

            this.Result = new CitiesResponseDto();
            this.Result.Cities = cityEntities;
        }
    }
}
