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
    internal sealed class DealerInsertionUnitOfWork : UnitOfWork
    {
        private string UniqueDbName = string.Empty;

        public DealerRequestDto Dealer;

        public DealerInsertionUnitOfWork(DealerRequestDto Dealer)
        {

            this.Dealer = Dealer;
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
                DealerEntityManager DealerEntityManager = new DealerEntityManager();
                var ce = DealerEntityManager.GetDealerById(Dealer.Id);
                if (ce.IsDealerExists == false)
                {
                    Result = DealerEntityManager.AddDealer(Dealer, UniqueDbName);
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_Dealers");
                    string uniqueCacheKey = UniqueDbName + "_Dealers_" + Dealer.CountryId.ToString().ToLower();
                    cache.Remove(uniqueCacheKey);
                    cache.Remove(UniqueDbName + "_Dealers_" + Dealer.Id.ToString().ToLower());


                }
                else
                {
                    Result = "Dealer already exist.";
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

        public string Result { get; set; }


    }
}
