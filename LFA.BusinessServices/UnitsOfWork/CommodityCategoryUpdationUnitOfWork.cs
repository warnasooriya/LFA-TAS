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
    internal sealed class CommodityCategoryUpdationUnitOfWork : UnitOfWork
    {
        public CommodityCategoryRequestDto CommodityCategory;
        public CommodityCategoryResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public CommodityCategoryUpdationUnitOfWork(CommodityCategoryRequestDto CommodityCategory)
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        CommodityCategoryEntityManager commodityCategoryEntityManager = new CommodityCategoryEntityManager();
        //        var ce = commodityCategoryEntityManager.GetCommodityCategoryById(CommodityCategory.CommodityCategoryId, CommodityCategory.CommodityTypeId);
        //        return true;
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
                CommodityCategoryEntityManager commodityCategoryEntityManager = new CommodityCategoryEntityManager();
                //var ce = commodityCategoryEntityManager.GetCommodityCategoryById(CommodityCategory.CommodityCategoryId, CommodityCategory.CommodityTypeId);
                //if (ce.IsCommodityCategoryExists == true)
                //{
                    bool result = commodityCategoryEntityManager.UpdateCommodityCategory(CommodityCategory);
                    this.CommodityCategory.CommodityCategoryInsertion = result;
                    if (result)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_CommodityCategories_"+CommodityCategory.CommodityTypeId.ToString().ToLower());
                    }
                //}
                //else
                //{
                //    this.CommodityCategory.CommodityCategoryInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
