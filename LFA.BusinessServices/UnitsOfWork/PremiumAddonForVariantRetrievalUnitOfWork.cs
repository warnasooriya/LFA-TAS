using NHibernate;
using NHibernate.Linq;
using NLog;
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
    internal sealed class PremiumAddonForVariantRetrievalUnitOfWork : UnitOfWork
    {
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
            if (dbConnectionString != null)  
            {   
                EntitySessionManager.OpenSession(dbConnectionString);
            }    
            else    
            {    
                EntitySessionManager.OpenSession();     //**
            }

            PremiumAddonTypeEntityManager premiumAddonTypeEntityManager = new PremiumAddonTypeEntityManager();
            List<PremiumAddonType> PremiumAddonTypeEntities = premiumAddonTypeEntityManager.GetPremiumAddonTypesforVariant();


            PremiumAddonTypesResponseDto result = new PremiumAddonTypesResponseDto();
            result.PremiumAddonTypes = new List<PremiumAddonTypeResponseDto>();

            foreach (PremiumAddonType PremiumAddon in PremiumAddonTypeEntities)
            {
                PremiumAddonTypeResponseDto premiumAddonTypeResponseDto = new PremiumAddonTypeResponseDto();
                premiumAddonTypeResponseDto.Id = PremiumAddon.Id;
                premiumAddonTypeResponseDto.CommodityTypeId = PremiumAddon.CommodityTypeId;
                premiumAddonTypeResponseDto.Description = PremiumAddon.Description;
                premiumAddonTypeResponseDto.IndexNo = PremiumAddon.IndexNo;
                premiumAddonTypeResponseDto.IsApplicableforVariant = PremiumAddon.IsApplicableforVariant;


                result.PremiumAddonTypes.Add(premiumAddonTypeResponseDto);
            }
            this.Result = result;
            //}
            //finally
            //{
            //    EntitySessionManager.CloseSession();
            //}
        }
    }
}
