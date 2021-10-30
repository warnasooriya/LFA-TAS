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
    internal sealed class ItemStatusUpdationUnitOfWork : UnitOfWork
    {
        public ItemStatusRequestDto ItemStatus;
        public ItemStatusResponseDto ExPr;
        private string UniqueDbName = string.Empty;

        public ItemStatusUpdationUnitOfWork(ItemStatusRequestDto ItemStatus)
        {

            this.ItemStatus = ItemStatus;
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
                            ItemStatusEntityManager ItemStatusEntityManager = new ItemStatusEntityManager();
                            var ce = ItemStatusEntityManager.GetItemStatusById(ItemStatus.Id);
                            if (ce.IsItemStatusExists == true)
                            {
                                ItemStatus.EntryDateTime = ce.EntryDateTime;
                                ItemStatus.EntryUser = ce.EntryUser;
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
        //        ItemStatusEntityManager ItemStatusEntityManager = new ItemStatusEntityManager();
        //        var ce = ItemStatusEntityManager.GetItemStatusById(ItemStatus.Id);
        //        if (ce.IsItemStatusExists == true)
        //        {
        //            ItemStatus.EntryDateTime = ce.EntryDateTime;
        //            ItemStatus.EntryUser = ce.EntryUser;
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
                ItemStatusEntityManager ItemStatusEntityManager = new ItemStatusEntityManager();
                //var ce = ItemStatusEntityManager.GetItemStatusById(ItemStatus.Id);
                //if (ce.IsItemStatusExists == true)
                //{
                    bool result = ItemStatusEntityManager.UpdateItemStatus(ItemStatus);
                    if (result)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_ItemStatus");
                    }
                this.ItemStatus.ItemStatusInsertion = result;
                //}
                //else
                //{
                //    this.ItemStatus.ItemStatusInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
