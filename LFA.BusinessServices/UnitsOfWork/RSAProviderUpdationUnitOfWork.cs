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
    internal sealed class RSAProviderUpdationUnitOfWork : UnitOfWork
    {
        public RSAProviderRequestDto RSAProvider;
        public RSAProviderResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public RSAProviderUpdationUnitOfWork(RSAProviderRequestDto RSAProvider)
        {

            this.RSAProvider = RSAProvider;
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
                            RSAProviderEntityManager RSAProviderEntityManager = new RSAProviderEntityManager();
                            var ce = RSAProviderEntityManager.GetRSAProviderById(RSAProvider.Id);
                            if (ce.IsRSAProviderExists == true)
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

        //public override bool PreExecute()
        //{
        //    try
        //    {

        //        RSAProviderEntityManager RSAProviderEntityManager = new RSAProviderEntityManager();
        //        var ce = RSAProviderEntityManager.GetRSAProviderById(RSAProvider.Id);
        //        if (ce.IsRSAProviderExists == true)
        //        {
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
                RSAProviderEntityManager RSAProviderEntityManager = new RSAProviderEntityManager();
                //var ce = RSAProviderEntityManager.GetRSAProviderById(RSAProvider.Id);
                //if (ce.IsRSAProviderExists == true)
                //{
                    bool result = RSAProviderEntityManager.UpdateRSAProvider(RSAProvider);
                    this.RSAProvider.RSAProviderInsertion = result;
                //}
                //else
                //{
                //    this.RSAProvider.RSAProviderInsertion = false;
                //}

                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_RSAProviders");



            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
