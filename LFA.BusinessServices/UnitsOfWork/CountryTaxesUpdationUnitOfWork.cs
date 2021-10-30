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
    internal sealed class CountryTaxesUpdationUnitOfWork : UnitOfWork
    {
        public CountryTaxesRequestDto CountryTaxes;
        public CountryTaxessResponseDto ExPr;
        public bool delete;
        private string UniqueDbName = string.Empty;

        public CountryTaxesUpdationUnitOfWork(CountryTaxesRequestDto CountryTaxes,bool delete)
        {
            this.delete = delete;
            this.CountryTaxes = CountryTaxes;
        }
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        TaxEntityManager TaxEntityManager = new TaxEntityManager();
        //        var ce = TaxEntityManager.GetCountryTaxesById(CountryTaxes.Id);
        //        if (ce.IsCountryTaxesExists == true)
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
                            TaxEntityManager TaxEntityManager = new TaxEntityManager();
                            var ce = TaxEntityManager.GetCountryTaxesById(CountryTaxes.Id);
                            if (ce.IsCountryTaxesExists == true)
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
                //var ce = TaxEntityManager.GetCountryTaxessById(CountryTaxess.Id);
                //if (ce.IsCountryTaxessExists == true)
                //{
                if (delete)
                {
                    bool result = TaxEntityManager.DeleteCountryTaxes(CountryTaxes);
                    this.CountryTaxes.CountryTaxesInsertion = result;
                }
                else
                {
                    bool result = TaxEntityManager.UpdateCountryTaxes(CountryTaxes);
                    this.CountryTaxes.CountryTaxesInsertion = result;
                }

                /* remove cache */
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_ContractTaxes");
                cache.Remove(UniqueDbName + "_CountryTax_" + CountryTaxes.CountryId.ToString().ToLower());
                //}
                //else
                //{
                //    this.CountryTaxess.CountryTaxessInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
