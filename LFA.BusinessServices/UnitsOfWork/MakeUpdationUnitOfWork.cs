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
    internal sealed class MakeUpdationUnitOfWork : UnitOfWork
    {
        public MakeRequestDto Make;
        public MakeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public MakeUpdationUnitOfWork(MakeRequestDto Make)
        {

            this.Make = Make;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName =UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                            MakeEntityManager makeEntityManager = new MakeEntityManager();
                            var ce = makeEntityManager.GetMakeById(Make.Id);
                            if (ce.IsMakeExists == true)
                            {
                                Make.EntryDateTime = ce.EntryDateTime;
                                Make.EntryUser = ce.EntryUser;
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
        //        MakeEntityManager makeEntityManager = new MakeEntityManager();
        //        var ce = makeEntityManager.GetMakeById(Make.Id);
        //        if (ce.IsMakeExists == true)
        //        {
        //            Make.EntryDateTime = ce.EntryDateTime;
        //            Make.EntryUser = ce.EntryUser;
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
                MakeEntityManager makeEntityManager = new MakeEntityManager();
                //var ce = makeEntityManager.GetMakeById(Make.Id);
                //if (ce.IsMakeExists == true)
                //{
                bool result = makeEntityManager.UpdateMake(Make);
                this.Make.MakeInsertion = result;
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_Make");
                }
                //}
                //else
                //{
                //    this.Make.MakeInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
