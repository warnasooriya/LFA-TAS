using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports.PolicyStatement;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PloicyRetrievalForSearchGridReneval : UnitOfWork
    {
        private readonly PolicySearchGridRequestDto PolicySearchGridRequestDto;
        public object Result;
        public PloicyRetrievalForSearchGridReneval(PolicySearchGridRequestDto _PolicySearchGridRequestDto)
        {
            PolicySearchGridRequestDto = _PolicySearchGridRequestDto;
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
                            if (!String.IsNullOrEmpty(PolicySearchGridRequestDto.type))
                            {
                                return true;
                            }
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

                PolicyEntityManager policyEntityManager = new PolicyEntityManager();
                Result = policyEntityManager.GetPloiciesForSearchGridReneval(PolicySearchGridRequestDto);


                // this.Result = BordxEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
