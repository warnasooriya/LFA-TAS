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
    internal sealed class RSAAnualPremiumsRetrievalUnitOfWork : UnitOfWork
    {
       

        public RSAAnualPremiumsResponseDto Result
        {
            get;
            private set;
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
                RSAAnualPremiumEntityManager RSAAnualPremiumEntityManager = new RSAAnualPremiumEntityManager();
                List<RSAAnualPremium> RSAAnualPremiumEntities = RSAAnualPremiumEntityManager.GetRSAAnualPremiums();

				
                RSAAnualPremiumsResponseDto result = new RSAAnualPremiumsResponseDto();
                result.RSAAnualPremiums = new List<RSAAnualPremiumResponseDto>();
                foreach (var RSAAnualPremium in RSAAnualPremiumEntities)
                {
                    RSAAnualPremiumResponseDto pr = new RSAAnualPremiumResponseDto();

                    pr.Id = RSAAnualPremium.Id;
                    pr.ContractExtensionId = RSAAnualPremium.ContractExtensionId;
                    pr.Year = RSAAnualPremium.Year;
                    pr.Value = RSAAnualPremium.Value;
					
                    //need to write other fields
                    result.RSAAnualPremiums.Add(pr);
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
