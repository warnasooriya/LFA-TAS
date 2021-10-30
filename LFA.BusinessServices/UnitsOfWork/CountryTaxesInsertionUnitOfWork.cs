using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CountryTaxesInsertionUnitOfWork : UnitOfWork
    {
        public CountryTaxesRequestDto CountryTaxes;
        private string UniqueDbName = string.Empty;
        public CountryTaxesInsertionUnitOfWork(CountryTaxesRequestDto CountryTaxess)
        {
            this.CountryTaxes = CountryTaxess;
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
                TaxEntityManager TaxEntityManager = new TaxEntityManager();
                var ce = TaxEntityManager.GetCountryTaxesById(CountryTaxes.Id);
                if (ce == null)
                {
                    bool result = TaxEntityManager.AddCountryTaxes(CountryTaxes);
                    this.CountryTaxes.CountryTaxesInsertion = result;
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_ContractTaxes");
                    cache.Remove(UniqueDbName + "_CountryTax_" + this.CountryTaxes.CountryId.ToString().ToLower());
                }
                else
                {
                    this.CountryTaxes.CountryTaxesInsertion = false;
                }



            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
