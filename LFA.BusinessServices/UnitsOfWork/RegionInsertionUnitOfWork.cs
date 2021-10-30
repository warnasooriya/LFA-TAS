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
    internal sealed class RegionInsertionUnitOfWork : UnitOfWork
    {
        public RegionRequestDto Region;
        private string UniqueDbName = string.Empty;
        public RegionInsertionUnitOfWork(RegionRequestDto Region)
        {

            this.Region = Region;
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
                RegionEntityManager RegionEntityManager = new RegionEntityManager();
                var ce = RegionEntityManager.GetRegionById(Region.Id);
                if (ce == null)
                {
                    bool result = RegionEntityManager.AddRegion(Region);
                    this.Region.RegionInsertion = result;
                }
                else
                {
                    this.Region.RegionInsertion = false;
                }

                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_Regions");

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
