using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork : UnitOfWork
    {
        private readonly Guid GroupID;
        public ClaimsResponseDto Result
        {
            get;
            private set;
        }
        public ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork(Guid _GroupID)
        {
            GroupID = _GroupID;
        }

        public ClaimBatchGroupRetrievalForSearchGridV2UnitOfWork() {}

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
                if (GroupID == Guid.Empty)
                { 
                    List<Claim> ClaimEntities = ClaimBatchingEntityManager.GetAllClaimDetails();
                    ClaimsResponseDto result = new ClaimsResponseDto();
                    result.Claims = new List<ClaimResponseDto>();
                    foreach (Claim claim in ClaimEntities)
                    {
                        ClaimResponseDto claimResponseDto = new ClaimResponseDto();
                        claimResponseDto.Id = claim.Id;
                        claimResponseDto.ClaimCountryId = claim.ClaimCountryId;
                        claimResponseDto.ClaimNumber = claim.ClaimNumber;
                        claimResponseDto.TotalClaimAmount = claim.TotalClaimAmount;
                        result.Claims.Add(claimResponseDto);
                    }
                    this.Result = result;
                }
                else
                {  
                    List<Claim> ClaimEntities = ClaimBatchingEntityManager.GetAllClaimDetailsByGroupID(GroupID); 
                    ClaimsResponseDto result = new ClaimsResponseDto();
                    result.Claims = new List<ClaimResponseDto>(); 
                    foreach (Claim claim in ClaimEntities)
                    {
                        ClaimResponseDto claimResponseDto = new ClaimResponseDto();
                        claimResponseDto.Id = claim.Id;
                        claimResponseDto.ClaimCountryId = claim.ClaimCountryId;
                        claimResponseDto.ClaimNumber = claim.ClaimNumber;
                        claimResponseDto.TotalClaimAmount = claim.TotalClaimAmount; 
                        result.Claims.Add(claimResponseDto);
                    }
                    this.Result = result;

                }
                
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
