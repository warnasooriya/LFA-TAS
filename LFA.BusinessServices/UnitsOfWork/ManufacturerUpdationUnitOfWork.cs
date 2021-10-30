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
    internal sealed class ManufacturerUpdationUnitOfWork : UnitOfWork
    {
        public ManufacturerRequestDto Manufacturer;
        public ManufacturerResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public ManufacturerUpdationUnitOfWork(ManufacturerRequestDto Manufacturer)
        {

            this.Manufacturer = Manufacturer;
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
                            ManufacturerEntityManager ManufacturerEntityManager = new ManufacturerEntityManager();
                            var ce = ManufacturerEntityManager.GetManufacturerById(Manufacturer.Id);
                            if (ce.IsManufacturerExists == true)
                            {
                                Manufacturer.EntryDateTime = ce.EntryDateTime;
                                Manufacturer.EntryUser = ce.EntryUser;
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
        //        ManufacturerEntityManager ManufacturerEntityManager = new ManufacturerEntityManager();
        //        var ce = ManufacturerEntityManager.GetManufacturerById(Manufacturer.Id);
        //        if (ce.IsManufacturerExists == true)
        //        {
        //            Manufacturer.EntryDateTime = ce.EntryDateTime;
        //            Manufacturer.EntryUser = ce.EntryUser;
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
                ManufacturerEntityManager ManufacturerEntityManager = new ManufacturerEntityManager();
                //var ce = ManufacturerEntityManager.GetManufacturerById(Manufacturer.Id);
                //if (ce.IsManufacturerExists == true)
                //{
                    bool result = ManufacturerEntityManager.UpdateManufacturer(Manufacturer);
                    this.Manufacturer.ManufacturerInsertion = result;
                    if (result)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_ManufacturerWithCommodityTypes");
                    }
                //}
                //else
                //{
                //    this.Manufacturer.ManufacturerInsertion = false;
                //}
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
