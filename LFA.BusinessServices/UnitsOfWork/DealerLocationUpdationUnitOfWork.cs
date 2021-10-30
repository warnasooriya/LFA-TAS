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
    internal sealed class DealerLocationUpdationUnitOfWork : UnitOfWork
    {
        public DealerLocationRequestDto DealerLocation;
        public DealerLocationRespondDto ExPr;
        private string UniqueDbName = string.Empty;
        public DealerLocationUpdationUnitOfWork(DealerLocationRequestDto DealerLocation)
        {

            this.DealerLocation = DealerLocation;
        }
        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        DealerLocationEntityManager DealerLocationEntityManager = new DealerLocationEntityManager();
        //        var ce = DealerLocationEntityManager.GetDealerLocationById(DealerLocation.Id);
        //        if (ce.IsDealerLocationExists == true)
        //        {
        //            DealerLocation.EntryDateTime = ce.EntryDateTime;
        //            DealerLocation.EntryUser = ce.EntryUser;
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
                            DealerLocationEntityManager DealerLocationEntityManager = new DealerLocationEntityManager();
                            var ce = DealerLocationEntityManager.GetDealerLocationById(DealerLocation.Id);
                            if (ce.IsDealerLocationExists == true)
                            {
                                DealerLocation.EntryDateTime = ce.EntryDateTime;
                                DealerLocation.EntryUser = ce.EntryUser;
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
                DealerLocationEntityManager DealerLocationEntityManager = new DealerLocationEntityManager();
                //var ce = DealerLocationEntityManager.GetDealerLocationById(DealerLocation.Id);
                //if (ce.IsDealerLocationExists == true)
                //{
                    bool result = DealerLocationEntityManager.UpdateDealerLocation(DealerLocation);
                    this.DealerLocation.DealerLocationInsertion = result;
                    ICache cache = CacheFactory.GetCache();
                    string uniqueCacheKey = UniqueDbName + "_DealerLocations";
                    cache.Remove(uniqueCacheKey);
                //}
                //else
                //{
                //    this.DealerLocation.DealerLocationInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
