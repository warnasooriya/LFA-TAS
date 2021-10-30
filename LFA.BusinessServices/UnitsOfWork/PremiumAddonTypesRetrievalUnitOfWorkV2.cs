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
    internal sealed class PremiumAddonTypesRetrievalUnitOfWorkV2 : UnitOfWork
    {
        Guid CommodityTypeId = new Guid();
        public PremiumAddonTypesRetrievalUnitOfWorkV2(Guid CommodityTypeId_)
        {

            this.CommodityTypeId = CommodityTypeId_;
        }

        public PremiumAddonTypesResponseDto Result
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
                //PremiumAddonTypeEntityManager PremiumAddonTypeEntityManager = new PremiumAddonTypeEntityManager();
                //List<PremiumAddonType> PremiumAddonTypeEntities = PremiumAddonTypeEntityManager.GetPremiumAddonTypes();

                //List<PremiumAddonType> PremiumAddonTypeEntities = EntityCacheData.GetPremiumAddonTypes(UniqueDbName);


                PremiumAddonTypeEntityManager PremiumAddonTypeEntityManager = new PremiumAddonTypeEntityManager();
                var PremiumAddonTypeEntities = PremiumAddonTypeEntityManager.GetPremiumAddonTypesBycommodityTypeId(CommodityTypeId);


                PremiumAddonTypesResponseDto result = new PremiumAddonTypesResponseDto();
                result.PremiumAddonTypes = new List<PremiumAddonTypeResponseDto>();
                foreach (var PremiumAddonType in PremiumAddonTypeEntities)
                {
                    PremiumAddonTypeResponseDto pr = new PremiumAddonTypeResponseDto();

                    pr.Id = PremiumAddonType.Id;
                    pr.CommodityTypeId = PremiumAddonType.CommodityTypeId;
                    pr.Description = PremiumAddonType.Description;
                    pr.EntryDateTime = PremiumAddonType.EntryDateTime;
                    pr.EntryUser = PremiumAddonType.EntryUser;
                    pr.IndexNo = PremiumAddonType.IndexNo;
					
                    //need to write other fields
                    result.PremiumAddonTypes.Add(pr);
                }
                this.Result = result;
               
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
