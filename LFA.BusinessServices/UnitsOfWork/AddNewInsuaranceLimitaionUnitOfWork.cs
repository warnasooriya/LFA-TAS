using System;
using System.Collections.Generic;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Caching;
using TASTPAEntityManager = TAS.Services.Entities.Management.TASTPAEntityManager;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class AddNewInsuaranceLimitaionUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly InsuaranceLimitationRequestDto _insuaranceLimitation;
        private string UniqueDbName = string.Empty;


        public AddNewInsuaranceLimitaionUnitOfWork(InsuaranceLimitationRequestDto insuaranceLimitation)
        {
            _insuaranceLimitation = insuaranceLimitation;
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

                this.Result = ContractEntityManager.AddNewInsuaranceLimitation(_insuaranceLimitation);
                /* remove cache */
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_InsuaranceLimitation");
                string keyName = UniqueDbName + "_InsLimit_" + _insuaranceLimitation.commodityTypeId.ToString().ToLower() + "_" + _insuaranceLimitation.isRsa.ToString().ToLower();
                cache.Remove(keyName);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
