using TAS.DataTransfer.Responses;
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
    internal sealed class CountriesActiveRetrievalUnitOfWork : UnitOfWork
    {
        public CountriesResponseDto Result
        {
            get;
            private set;
        }
        private string UniqueDbName = string.Empty;
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


                //CountryEntityManager countryEntityManager = new CountryEntityManager();
                //List<CountryInfo> countryEntities = countryEntityManager.GetAllCountries();
                List<CountryInfo> countryEntities = EntityCacheData.GetAllCountries(UniqueDbName);
                CountriesResponseDto result = new CountriesResponseDto();
                result.Countries = new List<CountryResponseDto>();
                if (countryEntities != null)
                {
                    result.Countries = countryEntities
                                        //.Where(w => w.IsActive)
                                        .Select(s => new CountryResponseDto
                                        {
                                            CountryName = s.CountryName,
                                            IsActive = s.IsActive,
                                            CountryCode = s.CountryCode,
                                            CurrencyId = s.CurrencyId,
                                            PhoneCode = s.PhoneCode,
                                            Id = s.Id,
                                            Makes = s.Makes,
                                            Modeles = s.Models
                                        }).OrderBy(o => o.CountryName).ToList();
                }

                //result = new List<SystemUserResponseDto>();
                //foreach (CountryInfo country in countryEntities)
                //{
                //    CountryResponseDto countryRespondDto = new CountryResponseDto();
                //    countryRespondDto.CountryName = country.CountryName;
                //    countryRespondDto.IsActive = country.IsActive;
                //    countryRespondDto.CountryCode = country.CountryCode;
                //    countryRespondDto.CurrencyId = country.CurrencyId;
                //    countryRespondDto.PhoneCode = country.PhoneCode;
                //    countryRespondDto.Id = country.Id;
                //    countryRespondDto.Makes = country.Makes;
                //    countryRespondDto.Modeles = country.Models;

                //    result.Countries.Add(countryRespondDto);
                //}
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
