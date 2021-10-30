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
    internal sealed class CommodityCategoryInsertionUnitOfWork : UnitOfWork
    {
        public CommodityCategoryRequestDto CommodityCategory;
        private string UniqueDbName = string.Empty;

        public CommodityCategoryInsertionUnitOfWork(CommodityCategoryRequestDto CommodityCategory)
        {
            this.CommodityCategory = CommodityCategory;
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
                CommodityCategoryEntityManager CommodityCategoryEntityManager = new CommodityCategoryEntityManager();
                var ce = CommodityCategoryEntityManager.GetCommodityCategoryById(CommodityCategory.CommodityCategoryId,CommodityCategory.CommodityTypeId);
                if (ce == null)
                {
                    bool result = CommodityCategoryEntityManager.AddCommodityCategory(CommodityCategory);
                    this.CommodityCategory.CommodityCategoryInsertion = result;
                    if (CommodityCategory.CommodityCategoryInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_CommodityCategories_"+ CommodityCategory.CommodityTypeId.ToString().ToLower());
                    }
                }
                else
                {
                    this.CommodityCategory.CommodityCategoryInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
