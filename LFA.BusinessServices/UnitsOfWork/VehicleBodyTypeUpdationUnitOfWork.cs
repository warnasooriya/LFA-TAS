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
    internal sealed class VehicleBodyTypeUpdationUnitOfWork : UnitOfWork
    {
        public VehicleBodyTypeRequestDto VehicleBodyType;
        public VehicleBodyTypeResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public VehicleBodyTypeUpdationUnitOfWork(VehicleBodyTypeRequestDto VehicleBodyType)
        {

            this.VehicleBodyType = VehicleBodyType;
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
                            VehicleBodyTypeEntityManager vehicleBodyTypeEntityManager = new VehicleBodyTypeEntityManager();
                            var ce = vehicleBodyTypeEntityManager.GetVehicleBodyTypeById(VehicleBodyType.Id);
                            if (ce.IsVehicleBodyTypeExists == true)
                            {
                                VehicleBodyType.EntryDateTime = ce.EntryDateTime;
                                VehicleBodyType.EntryUser = ce.EntryUser;
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
        //        VehicleBodyTypeEntityManager vehicleBodyTypeEntityManager = new VehicleBodyTypeEntityManager();
        //        var ce = vehicleBodyTypeEntityManager.GetVehicleBodyTypeById(VehicleBodyType.Id);
        //        if (ce.IsVehicleBodyTypeExists == true)
        //        {
        //            VehicleBodyType.EntryDateTime = ce.EntryDateTime;
        //            VehicleBodyType.EntryUser = ce.EntryUser;
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
                VehicleBodyTypeEntityManager vehicleBodyTypeEntityManager = new VehicleBodyTypeEntityManager();
                //var ce = vehicleBodyTypeEntityManager.GetVehicleBodyTypeById(VehicleBodyType.Id);
                //if (ce.IsVehicleBodyTypeExists == true)
               // {
                    bool result = vehicleBodyTypeEntityManager.UpdateVehicleBodyType(VehicleBodyType);
                    this.VehicleBodyType.VehicleBodyTypeInsertion = result;
                    if (VehicleBodyType.VehicleBodyTypeInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_VehicleBodyType");
                    }
                //}
                //else
                //{
                //     this.VehicleBodyType.VehicleBodyTypeInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
