using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Caching;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class BordxPaymentInsertionUnitOfWork : UnitOfWork
    {
        public BordxPaymentRequestDto BordxPayment;
        private string UniqueDbName = string.Empty;

        public BordxPaymentInsertionUnitOfWork(BordxPaymentRequestDto BordxPayment)
        {

            this.BordxPayment = BordxPayment;
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
                BordxPaymentEntityManager BordxPaymentEntityManager = new BordxPaymentEntityManager();
                var ce = BordxPaymentEntityManager.GetBordxPaymentById(BordxPayment.Id);
                if (ce == null)
                {
                    bool result = BordxPaymentEntityManager.AddBordxPayment(BordxPayment);
                    this.BordxPayment.BordxPaymentInsertion = result;
                    if (BordxPayment.BordxPaymentInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_BordxPayment");
                    }
                }
                else
                {
                    this.BordxPayment.BordxPaymentInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
