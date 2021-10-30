using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class FaultCategoryUpdationUnitOfWork : UnitOfWork
    {
        public FaultCategoryRequestDto FaultCategory;
        private string UniqueDbName = string.Empty;
        public FaultCategoryUpdationUnitOfWork(FaultCategoryRequestDto FaultCategory)
        {

            this.FaultCategory = FaultCategory;
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
                            FaultEntityManager FaultEntityManager = new FaultEntityManager();
                            var ce = FaultEntityManager.GetFaultCategoryById(FaultCategory.Id);
                            if (ce.IsActive == true)
                            {
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
                FaultEntityManager FaultEntityManager = new FaultEntityManager();

                bool result = FaultEntityManager.UpdateFaultCategory(FaultCategory);
                this.FaultCategory.FaultCategoryInsertion = result;
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_FaultCategory");
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
