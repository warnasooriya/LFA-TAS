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
    internal sealed class CommodityUsageTypeUpdationUnitOfWork : UnitOfWork
    {
        public CommodityUsageTypeRequestDto CommodityUsageType;
        public CommodityUsageTypeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public CommodityUsageTypeUpdationUnitOfWork(CommodityUsageTypeRequestDto CommodityUsageType)
        {

            this.CommodityUsageType = CommodityUsageType;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName =UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
        //        CommodityUsageTypeEntityManager CommodityUsageTypeEntityManager = new CommodityUsageTypeEntityManager();
        //        var ce = CommodityUsageTypeEntityManager.GetCommodityUsageTypeById(CommodityUsageType.Id);
        //        if (ce.IsCommodityUsageTypeExists == true)
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
                CommodityUsageTypeEntityManager CommodityUsageTypeEntityManager = new CommodityUsageTypeEntityManager();
                bool result = CommodityUsageTypeEntityManager.UpdateCommodityUsageType(CommodityUsageType);
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_CommodityUsageTypes");
                }

                this.CommodityUsageType.CommodityUsageTypeInsertion = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
