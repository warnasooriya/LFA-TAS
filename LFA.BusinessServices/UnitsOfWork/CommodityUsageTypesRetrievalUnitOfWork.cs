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
    internal sealed class CommodityUsageTypesRetrievalUnitOfWork : UnitOfWork
    {


        public CommodityUsageTypesResponseDto Result
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

            //CommodityUsageTypeEntityManager CommodityUsageTypeEntityManager = new CommodityUsageTypeEntityManager();
            //List<CommodityUsageType> CommodityUsageTypeEntities = CommodityUsageTypeEntityManager.GetAllCommodityUsageTypes();
            List<CommodityUsageTypeResponseDto> CommodityUsageTypeEntities = EntityCacheData.GetAllCommodityUsageTypes(UniqueDbName);


            CommodityUsageTypesResponseDto result = new CommodityUsageTypesResponseDto();
            result.CommodityUsageTypes = new List<CommodityUsageTypeResponseDto>();

            if (CommodityUsageTypeEntities != null)
            {
                result.CommodityUsageTypes = CommodityUsageTypeEntities;
            }

            //foreach (var CommodityUsageType in CommodityUsageTypeEntities)
            //{
            //    CommodityUsageTypeResponseDto pr = new CommodityUsageTypeResponseDto();

            //    pr.Id = CommodityUsageType.Id;
            //    pr.Name = CommodityUsageType.Name;
            //    pr.Description = CommodityUsageType.Description;

            //    //need to write other fields
            //    result.CommodityUsageTypes.Add(pr);
            //}
            this.Result = result;
            //}
            //finally
            //{
            //    EntitySessionManager.CloseSession();
            //}
        }

    }
}
