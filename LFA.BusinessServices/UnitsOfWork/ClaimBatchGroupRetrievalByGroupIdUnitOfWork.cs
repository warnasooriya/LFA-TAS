using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimBatchGroupRetrievalByGroupIdUnitOfWork : UnitOfWork
    {
        public ClaimBatchGroupsRespondDto Result
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
                ClaimBatchingEntityManager ClaimBatchingEntityManager = new ClaimBatchingEntityManager();
                List<ClaimBatchGroup> GroupEntities = ClaimBatchingEntityManager.GetClaimBatchingGroup();

                ClaimBatchGroupsRespondDto result = new ClaimBatchGroupsRespondDto();
                result.ClaimBatchGroups = new List<ClaimBatchGroupRespondDto>();
                if (GroupEntities != null && GroupEntities.Count > 0)
                {
                    foreach (var ClaimBatchGroup in GroupEntities)
                    {
                        ClaimBatchGroupRespondDto pr = new ClaimBatchGroupRespondDto();

                        pr.Id = ClaimBatchGroup.Id;
                        pr.ClaimBatchId = ClaimBatchGroup.ClaimBatchId;
                        pr.EntryBy = ClaimBatchGroup.EntryBy;
                        pr.EntryDate = ClaimBatchGroup.EntryDate;
                        pr.GroupName = ClaimBatchGroup.GroupName;
                        pr.IsAllocatedForCheque = ClaimBatchGroup.IsAllocatedForCheque;
                        
                        //need to write other fields
                        result.ClaimBatchGroups.Add(pr);
                    }
                    result.ClaimBatchGroups = result.ClaimBatchGroups.OrderBy(c => c.GroupName.Length).ThenBy(c => c.GroupName).ToList();
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
