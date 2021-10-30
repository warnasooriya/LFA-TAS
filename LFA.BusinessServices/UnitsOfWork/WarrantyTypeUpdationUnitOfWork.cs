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
    internal sealed class WarrantyTypeUpdationUnitOfWork : UnitOfWork
    {
        public WarrantyTypeRequestDto WarrantyType;
        public WarrantyTypeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public WarrantyTypeUpdationUnitOfWork(WarrantyTypeRequestDto WarrantyType)
        {

            this.WarrantyType = WarrantyType;
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
                            WarrantyTypeEntityManager WarrantyTypeEntityManager = new WarrantyTypeEntityManager();
                            var ce = WarrantyTypeEntityManager.GetWarrantyTypeById(WarrantyType.Id);
                            if (ce.IsWarrantyTypeExists == true)
                            {
                                WarrantyType.EntryDateTime = ce.EntryDateTime;
                                WarrantyType.EntryUser = ce.EntryUser;
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
        //        WarrantyTypeEntityManager WarrantyTypeEntityManager = new WarrantyTypeEntityManager();
        //        var ce = WarrantyTypeEntityManager.GetWarrantyTypeById(WarrantyType.Id);
        //        if (ce.IsWarrantyTypeExists == true)
        //        {
        //            WarrantyType.EntryDateTime = ce.EntryDateTime;
        //            WarrantyType.EntryUser = ce.EntryUser;
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
                WarrantyTypeEntityManager WarrantyTypeEntityManager = new WarrantyTypeEntityManager();
                //var ce = WarrantyTypeEntityManager.GetWarrantyTypeById(WarrantyType.Id);
                //if (ce.IsWarrantyTypeExists == true)
                //{
                    bool result = WarrantyTypeEntityManager.UpdateWarrantyType(WarrantyType);
                    this.WarrantyType.WarrantyTypeInsertion = result;
                    if (result)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_WarrantyType");
                    }
                //}
                //else
                //{
                //    this.WarrantyType.WarrantyTypeInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
