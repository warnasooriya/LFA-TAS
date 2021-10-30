using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CommodityCategoriesRetrievalUnitOfWork : UnitOfWork
    {


        public CommodityCategoriesRespondDto Result
        {
            get;
            private set;
        }
        public Guid commodityTypeId
        {
            get;
            set;
        }
        public CommodityCategoriesRetrievalUnitOfWork(Guid commodityTypeId)
        {
            this.commodityTypeId = commodityTypeId;
        }
        private string UniqueDbName = string.Empty;
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


                //CommodityCategoryEntityManager CommodityCategoryEntityManager = new CommodityCategoryEntityManager();
                //List<CommodityCategory> CommodityCategoryEntities = CommodityCategoryEntityManager.GetCommodityCategories(commodityTypeId);
                List<CommodityCategoryResponseDto> CommodityCategoryEntities = EntityCacheData.GetCommodityCategories(UniqueDbName, commodityTypeId);

                CommodityCategoriesRespondDto result = new CommodityCategoriesRespondDto();
                result.CommodityCategories = CommodityCategoryEntities;
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
