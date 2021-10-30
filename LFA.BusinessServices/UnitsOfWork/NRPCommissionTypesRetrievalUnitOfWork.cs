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
    internal sealed class NRPCommissionTypesRetrievalUnitOfWork : UnitOfWork
    {

        public Guid NRPCommissionTypesId;
        public NRPCommissionTypesResponseDto Result
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
                NRPCommissionTypesEntityManager NRPCommissionTypesEntityManager = new NRPCommissionTypesEntityManager();
                NRPCommissionTypesResponseDto NRPCommissionTypesEntity = NRPCommissionTypesEntityManager.GetNRPCommissionTypesById(NRPCommissionTypesId);
                if (NRPCommissionTypesEntity.IsNRPCommissionTypesExists == null || NRPCommissionTypesEntity.IsNRPCommissionTypesExists== false)
                {
                    NRPCommissionTypesEntity.IsNRPCommissionTypesExists = false;
                }
                else
                {
                    NRPCommissionTypesEntity.IsNRPCommissionTypesExists = true;
                }

                this.Result = NRPCommissionTypesEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
