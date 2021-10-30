using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ModelRetrievalByMakeIdsAndCommodityCategoryIdUnitOfWork : UnitOfWork
    {
        private readonly List<Guid> MakeIds;
        private readonly Guid CommodityCategoryId;
        public ModelesResponseDto Results;
        public ModelRetrievalByMakeIdsAndCommodityCategoryIdUnitOfWork(List<Guid> MakeIdsList, Guid commodityCategoryId)
        {
            MakeIds = MakeIdsList;
            CommodityCategoryId = commodityCategoryId;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                ModelEntityManager ModelEntityManager = new ModelEntityManager();
                ModelesResponseDto ModelesResponseDto = ModelEntityManager.ModelsByMakeIdsAndCommodityCategoryId(MakeIds, CommodityCategoryId);

                this.Results = ModelesResponseDto;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
