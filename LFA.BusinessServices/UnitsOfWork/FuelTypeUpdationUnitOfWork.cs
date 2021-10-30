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
    internal sealed class FuelTypeUpdationUnitOfWork : UnitOfWork
    {
        public FuelTypeRequestDto FuelType;
        public FuelTypeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public FuelTypeUpdationUnitOfWork(FuelTypeRequestDto FuelType)
        {

            this.FuelType = FuelType;
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
                            FuelTypeEntityManager fuelTypeEntityManager = new FuelTypeEntityManager();
                            var ce = fuelTypeEntityManager.GetFuelTypeById(FuelType.FuelTypeId);
                            if (ce.IsFuelTypeExists == true)
                            {
                                FuelType.EntryDateTime = ce.EntryDateTime;
                                FuelType.EntryUser = ce.EntryUser;
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
        //        FuelTypeEntityManager fuelTypeEntityManager = new FuelTypeEntityManager();
        //        var ce = fuelTypeEntityManager.GetFuelTypeById(FuelType.FuelTypeId);
        //        if (ce.IsFuelTypeExists == true)
        //        {
        //            FuelType.EntryDateTime = ce.EntryDateTime;
        //            FuelType.EntryUser = ce.EntryUser;
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
                FuelTypeEntityManager fuelTypeEntityManager = new FuelTypeEntityManager();
                //var ce = fuelTypeEntityManager.GetFuelTypeById(FuelType.Id);
                //if (ce.IsFuelTypeExists == true)
               // {
                    bool result = fuelTypeEntityManager.UpdateFuelType(FuelType);
                    this.FuelType.FuelTypeInsertion = result;
                    if (FuelType.FuelTypeInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_FuelType");
                    }
                //}
                //else
                //{
                //     this.FuelType.FuelTypeInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
