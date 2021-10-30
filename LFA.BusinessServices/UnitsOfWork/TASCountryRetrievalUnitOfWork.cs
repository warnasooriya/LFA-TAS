using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASCountryRetrievalUnitOfWork : UnitOfWork
    {
        public CountriesResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public TASCountryRetrievalUnitOfWork(Guid tpaId)
        {
            this.tpaId = tpaId;
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
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
                CountryEntityManager countryEntityManager = new CountryEntityManager();
                List<CountryInfo> countryEntities = countryEntityManager.GetAllCountries();
                CountriesResponseDto result = new CountriesResponseDto();
                result.Countries = new List<CountryResponseDto>();
                //result = new List<SystemUserResponseDto>();
                foreach (CountryInfo country in countryEntities)
                {
                    CountryResponseDto countryRespondDto = new CountryResponseDto();
                    countryRespondDto.CountryName = country.CountryName;
                    countryRespondDto.IsActive = country.IsActive;
                    countryRespondDto.CountryCode = country.CountryCode;
                    countryRespondDto.Id = country.Id;
                    countryRespondDto.Makes = country.Makes;
                    countryRespondDto.Modeles = country.Models;                    
                    result.Countries.Add(countryRespondDto);
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
