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
    internal sealed class DealerUpdationUnitOfWork : UnitOfWork
    {
        public DealerRequestDto Dealer;
        public DealerRespondDto ExPr;
        private string UniqueDbName = string.Empty;

        public DealerUpdationUnitOfWork(DealerRequestDto Dealer)
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
                            DealerEntityManager dealerEntityManager = new DealerEntityManager();
                            var ce = dealerEntityManager.GetDealerById(Dealer.Id);
                            if (ce.IsDealerExists == true)
                            {
                                Dealer.EntryDateTime = ce.EntryDateTime;
                                Dealer.EntryUser = ce.EntryUser;
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
        //        DealerEntityManager dealerEntityManager = new DealerEntityManager();
        //        var ce = dealerEntityManager.GetDealerById(Dealer.Id);
        //        if (ce.IsDealerExists == true)
        //        {
        //            Dealer.EntryDateTime = ce.EntryDateTime;
        //            Dealer.EntryUser = ce.EntryUser;
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
                DealerEntityManager dealerEntityManager = new DealerEntityManager();
                //var ce = dealerEntityManager.GetDealerById(Dealer.Id);
                //if (ce.IsDealerExists == true)
                //{
                Result = dealerEntityManager.UpdateDealer(Dealer, UniqueDbName);
                /* remove cache */
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_Dealers");
                string uniqueCacheKey = UniqueDbName + "_Dealers_" + Dealer.CountryId.ToString().ToLower();
                cache.Remove(uniqueCacheKey);
                cache.Remove(UniqueDbName + "_Dealers_" + Dealer.Id.ToString().ToLower());
                //}
                //else
                //{
                //    this.Dealer.DealerInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

        public string Result { get; set; }
    }
}
