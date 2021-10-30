using System;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimListRetrievalForProcessUnitOfWork : UnitOfWork
    {
        private readonly ClaimRetirevalForProcessRequestDto claimRequestData;

        public ClaimListRetrievalForProcessUnitOfWork(ClaimRetirevalForProcessRequestDto _claimRequestData)
        {
            claimRequestData = _claimRequestData;
        }

        public object Result { get; set; }

        public override bool PreExecute()
        {
            try
            {
                var JWTHelper = new JWTHelper(SecurityContext);
                var str = JWTHelper.DecodeAuthenticationToken();
                var dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    var tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime)))
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
                if (dbConnectionString != null) //**
                {
                    //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                } //**
                else //**
                {
                    //**
                    EntitySessionManager.OpenSession(); //**
                } //**
                var claimEntityManager = new ClaimEntityManager();
                Result = claimEntityManager.GetClaimListForClaimProcess(claimRequestData);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}