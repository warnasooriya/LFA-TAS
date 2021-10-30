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
    internal sealed class VariantInsertionUnitOfWork : UnitOfWork
    {
        public VariantRequestDto Variant;
        private string UniqueDbName = string.Empty;
        public VariantInsertionUnitOfWork(VariantRequestDto Variant)
        {

            this.Variant = Variant;
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
                VariantEntityManager VariantEntityManager = new VariantEntityManager();
                var ce = VariantEntityManager.GetVariantById(Variant.Id);
                if (ce == null)
                {
                    bool result = VariantEntityManager.AddVariant(Variant);
                    this.Variant.VariantInsertion = result;
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_Variants");
                }
                else
                {
                    this.Variant.VariantInsertion = false;
                }


            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
