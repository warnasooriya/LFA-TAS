using System;
using System.Collections.Generic;
using System.Linq;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ContractTaxesRetrievalByContractIdUnitOfWork : UnitOfWork
    {
        private Guid ContractId { get; set; }
        private decimal AmountForTaxCalc { get; set; }
        public object Result { get; set; }
        public ContractTaxesRetrievalByContractIdUnitOfWork(Guid _ContractId, decimal _AmountForTaxCalc)
        {
            ContractId = _ContractId;
            AmountForTaxCalc = _AmountForTaxCalc;
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
               // ContractEntityManager ContractEntityManager = new ContractEntityManager();
                this.Result = ContractEntityManager.GetContractTaxesByExtensionId(ContractId, AmountForTaxCalc);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }


    }
}
