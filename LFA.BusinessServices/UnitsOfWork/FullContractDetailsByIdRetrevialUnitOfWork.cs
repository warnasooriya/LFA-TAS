using System;
using System.Collections.Generic;
using System.Linq;
using TAS.Services.Common;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class FullContractDetailsByIdRetrevialUnitOfWork : UnitOfWork
    {
        private readonly Guid currentContractId, variantId, makeId;
        private readonly bool withTax;

        public ContractViewData Result;
        public FullContractDetailsByIdRetrevialUnitOfWork(Guid ContractId, Guid _variantId, Guid _makeId, bool _withTax)
        {
            currentContractId = ContractId;
            variantId = _variantId;
            withTax = _withTax;
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
                ContractEntityManager contractEm = new ContractEntityManager();
                Result = contractEm.FullContractDetailsById(currentContractId, variantId, makeId, withTax);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
