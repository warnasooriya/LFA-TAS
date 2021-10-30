using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimBatchGroupInsertionUnitOfWork : UnitOfWork
    {
        public ClaimBatchGroupRequestDto ClaimBatchGroup;
        private string UniqueDbName = string.Empty;
        public ClaimBatchGroupInsertionUnitOfWork(ClaimBatchGroupRequestDto claimBatchGroup)
        {

            this.ClaimBatchGroup = claimBatchGroup;
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
                ClaimBatchingEntityManager ClaimBatchingEntityManager = new ClaimBatchingEntityManager();
                var ce = ClaimBatchingEntityManager.GetClaimBatchGroupById(ClaimBatchGroup.Id);
                if (ce == null)
                {
                    bool result = ClaimBatchingEntityManager.AddClaimBatchGroup(ClaimBatchGroup);
                    this.ClaimBatchGroup.ClaimBatchGroupEntryInsertion = result;
                }
                else
                {
                    bool result = ClaimBatchingEntityManager.UpdateClaimBachGroup(ClaimBatchGroup);
                    this.ClaimBatchGroup.ClaimBatchGroupEntryInsertion = result;
                }
                /* remove cache */
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_ClaimBatch");
                cache.Remove(UniqueDbName + "_Claim");
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
