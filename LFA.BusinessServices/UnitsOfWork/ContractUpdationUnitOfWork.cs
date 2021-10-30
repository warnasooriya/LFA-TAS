using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ContractUpdationUnitOfWork : UnitOfWork
    {
        public ContractRequestDto Contract;
        public ContractResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public ContractUpdationUnitOfWork(ContractRequestDto Contract)
        {

            this.Contract = Contract;
        }

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        ContractEntityManager ContractEntityManager = new ContractEntityManager();
        //        var ce = ContractEntityManager.GetContractById(Contract.Id);
        //        if (ce.IsContractExists == true)
        //        {
        //            Contract.EntryDateTime = ce.EntryDateTime;
        //            Contract.EntryUser = ce.EntryUser;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName= str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                            ContractEntityManager ContractEntityManager = new ContractEntityManager();
                            var ce = ContractEntityManager.GetContractById(Contract.Id);
                            if (ce.IsContractExists == true)
                            {
                                Contract.EntryDateTime = ce.EntryDateTime;
                                Contract.EntryUser = ce.EntryUser;
                                return true;
                            }
                            else
                            {
                                return false;
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
                ContractEntityManager ContractEntityManager = new ContractEntityManager();
                //var ce = ContractEntityManager.GetPremiumSetupById(PremiumSetup.Id);
                //if (ce.IsPremiumSetupExists == true)
                //{
                bool result = ContractEntityManager.UpdateContract(Contract);
                    this.Contract.ContractInsertion = result;
                //}
                //else
                //{
                //    this.PremiumSetup.ContractInsertion = false;
                //}
                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName + "_Contract_" + Contract.CommodityTypeId.ToString().ToLower();
                cache.Remove(uniqueCacheKey);

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
