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


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASCommodityCategoriesRetrievalUnitOfWork : UnitOfWork
    {

        private string UniqueDbName = string.Empty;
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
        public Guid tpaId
        {
            get;
            set;
        }
        public TASCommodityCategoriesRetrievalUnitOfWork(Guid commodityTypeId,Guid tpaId)
        {
            this.commodityTypeId = commodityTypeId;
            this.tpaId = tpaId;
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = UniqueDbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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
