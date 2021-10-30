using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;
namespace TAS.Services.UnitsOfWork
{
    internal sealed class CountryUpdationUnitOfWork : UnitOfWork
    {
        public CountryRequestDto Country;
        public CountryResponseDto ExPr;
        private string UniqueDbName = string.Empty;

        public CountryUpdationUnitOfWork(CountryRequestDto Country)
        {

            this.Country = Country;
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
                            CountryEntityManager CountryEntityManager = new CountryEntityManager();
                            var ce = CountryEntityManager.GetCountryById(Country.Id);
                            if (ce.IsCountryExists == true)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        CountryEntityManager CountryEntityManager = new CountryEntityManager();
        //        var ce = CountryEntityManager.GetCountryById(Country.Id);
        //        if (ce.IsCountryExists == true)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

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
                CountryEntityManager CountryEntityManager = new CountryEntityManager();
                //var ce = CountryEntityManager.GetCountryById(Country.Id);
                //if (ce.IsCountryExists == true)
                //{
                    bool result = CountryEntityManager.UpdateCountry(Country);
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_Countries");
                    cache.Remove(UniqueDbName + "_ActiveCountries");
                }
                this.Country.CountryInsertion = result;
                //}
                //else
                //{
                //    this.Country.CountryInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
