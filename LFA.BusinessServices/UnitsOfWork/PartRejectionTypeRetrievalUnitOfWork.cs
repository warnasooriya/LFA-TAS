using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class PartRejectionTypeRetrievalUnitOfWork : UnitOfWork
    {
        public PartRejectionTypeResponseDto Result
        {
            get;
            private set;
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

            //try
            //{
            //temp section can remove  //**  lines once preexecute jwt and db name working fine
            if (dbConnectionString != null)   //**
            {     //**
                EntitySessionManager.OpenSession(dbConnectionString);
            }     //**
            else     //**
            {     //**
                EntitySessionManager.OpenSession();     //**
            }     //**

            // List<CommodityType> commodityEntities = commodityEntityManager.GetAllCommodities();
            List<PartRejectionTypesResponseDto> PartRejectionTypeEntities = EntityCacheData.GetAllPartRejectioDescription(UniqueDbName);

            PartRejectionTypeResponseDto result = new PartRejectionTypeResponseDto();
            result.PartRejectionType = PartRejectionTypeEntities;
            this.Result = result;
            //}
            //finally
            //{
            //    EntitySessionManager.CloseSession();
            //}
        }

    }
}
