using TAS.DataTransfer.Requests;
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
    internal sealed class PremiumBasedOnInsertionUnitOfWork : UnitOfWork
    {
        public PremiumBasedOnRequestDto PremiumBasedOn;
        private string UniqueDbName = string.Empty;

        public PremiumBasedOnInsertionUnitOfWork(PremiumBasedOnRequestDto PremiumBasedOn)
        {

            this.PremiumBasedOn = PremiumBasedOn;
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
                PremiumBasedOnEntityManager PremiumBasedOnEntityManager = new PremiumBasedOnEntityManager();
                var ce = PremiumBasedOnEntityManager.GetPremiumBasedOnById(PremiumBasedOn.Id);
                if (ce == null)
                {
                    bool result = PremiumBasedOnEntityManager.AddPremiumBasedOn(PremiumBasedOn);
                    this.PremiumBasedOn.PremiumBasedOnInsertion = result;
                    if (PremiumBasedOn.PremiumBasedOnInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_PremiumBasedOns");
                    }
                }
                else
                {
                    this.PremiumBasedOn.PremiumBasedOnInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
