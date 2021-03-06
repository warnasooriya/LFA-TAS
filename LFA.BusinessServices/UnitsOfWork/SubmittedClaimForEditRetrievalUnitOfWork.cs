using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Persistence;
using TASTPAEntityManager = TAS.Services.Entities.Management.TASTPAEntityManager;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class SubmittedClaimForEditRetrievalUnitOfWork : UnitOfWork
    {
        private readonly ClaimListRequestDto claimListRequestDto;
        public object Result { get; set; }

        public SubmittedClaimForEditRetrievalUnitOfWork(ClaimListRequestDto _claimListRequestDto)
        {
            claimListRequestDto = _claimListRequestDto;
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
                Entities.Management.ClaimEntityManager ClaimEntityManager = new Entities.Management.ClaimEntityManager();
                this.Result = ClaimEntityManager.GetAllSubmittedClaimsForEditByUserId(claimListRequestDto);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
