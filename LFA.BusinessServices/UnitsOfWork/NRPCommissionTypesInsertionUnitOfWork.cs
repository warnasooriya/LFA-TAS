using TAS.DataTransfer.Requests;
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
    internal sealed class NRPCommissionTypesInsertionUnitOfWork : UnitOfWork
    {
        public NRPCommissionTypesRequestDto NRPCommissionTypes;
        private string UniqueDbName = string.Empty;

        public NRPCommissionTypesInsertionUnitOfWork(NRPCommissionTypesRequestDto NRPCommissionTypes)
        {

            this.NRPCommissionTypes = NRPCommissionTypes;
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
                            NRPCommissionTypesEntityManager NRPCommissionTypesEntityManager = new NRPCommissionTypesEntityManager();
                            var ce = NRPCommissionTypesEntityManager.GetNRPCommissionTypesById(NRPCommissionTypes.Id);
                            if (ce == null)
                            {
                                bool result = NRPCommissionTypesEntityManager.AddNRPCommissionTypes(NRPCommissionTypes);
                                this.NRPCommissionTypes.NRPCommissionTypesInsertion = result;
                                if (NRPCommissionTypes.NRPCommissionTypesInsertion)
                                {
                                    /* remove cache */
                                    ICache cache = CacheFactory.GetCache();
                                    cache.Remove(UniqueDbName + "_NRPCommissionType");
                                }
                            }
                            else
                            {
                                this.NRPCommissionTypes.NRPCommissionTypesInsertion = false;
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
                NRPCommissionTypesEntityManager NRPCommissionTypesEntityManager = new NRPCommissionTypesEntityManager();
                var ce = NRPCommissionTypesEntityManager.GetNRPCommissionTypesById(NRPCommissionTypes.Id);
                if (ce == null)
                {
                    bool result = NRPCommissionTypesEntityManager.AddNRPCommissionTypes(NRPCommissionTypes);
                    this.NRPCommissionTypes.NRPCommissionTypesInsertion = result;
                    if (NRPCommissionTypes.NRPCommissionTypesInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_NRPCommissionType");
                    }
                }
                else
                {
                    this.NRPCommissionTypes.NRPCommissionTypesInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
