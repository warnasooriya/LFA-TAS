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
    internal sealed class VariantUpdationUnitOfWork : UnitOfWork
    {
        public VariantRequestDto Variant;
        public VariantResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public VariantUpdationUnitOfWork(VariantRequestDto Variant)
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
                            VariantEntityManager VariantEntityManager = new VariantEntityManager();
                            var ce = VariantEntityManager.GetVariantById(Variant.Id);
                            if (ce.IsVariantExists == true)
                            {
                                Variant.EntryDateTime = ce.EntryDateTime;
                                Variant.EntryUser = ce.EntryUser;
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
        //        EntitySessionManager.OpenSession();
        //        VariantEntityManager VariantEntityManager = new VariantEntityManager();
        //        var ce = VariantEntityManager.GetVariantById(Variant.Id);
        //        if (ce.IsVariantExists == true)
        //        {
        //            Variant.EntryDateTime = ce.EntryDateTime;
        //            Variant.EntryUser = ce.EntryUser;
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
                VariantEntityManager VariantEntityManager = new VariantEntityManager();
                //var ce = VariantEntityManager.GetVariantById(Variant.Id);
                //if (ce.IsVariantExists == true)
                //{
                bool result = VariantEntityManager.UpdateVariant(Variant);
                    this.Variant.VariantInsertion = result;
                //}
                //else
                //{
                //    this.Variant.VariantInsertion = false;
                //}

                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_Variants");


            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
