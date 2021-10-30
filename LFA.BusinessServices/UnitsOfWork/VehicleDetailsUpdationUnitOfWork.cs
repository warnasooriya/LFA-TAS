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
    internal sealed class VehicleDetailsUpdationUnitOfWork : UnitOfWork
    {
        public VehicleDetailsRequestDto VehicleDetails;
        public VehicleDetailsResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public VehicleDetailsUpdationUnitOfWork(VehicleDetailsRequestDto VehicleDetails)
        {

            this.VehicleDetails = VehicleDetails;
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
                            VehicleDetailsEntityManager VehicleDetailsEntityManager = new VehicleDetailsEntityManager();
                            var ce = VehicleDetailsEntityManager.GetVehicleDetailsById(VehicleDetails.Id);
                            if (ce.IsVehicleDetailsExists == true)
                            {
                                VehicleDetails.EntryDateTime = ce.EntryDateTime;
                                VehicleDetails.EntryUser = ce.EntryUser;
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
        //        VehicleDetailsEntityManager VehicleDetailsEntityManager = new VehicleDetailsEntityManager();
        //        var ce = VehicleDetailsEntityManager.GetVehicleDetailsById(VehicleDetails.Id);
        //        if (ce.IsVehicleDetailsExists == true)
        //        {
        //            VehicleDetails.EntryDateTime = ce.EntryDateTime;
        //            VehicleDetails.EntryUser = ce.EntryUser;
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
                VehicleDetailsEntityManager VehicleDetailsEntityManager = new VehicleDetailsEntityManager();
                //var ce = VehicleDetailsEntityManager.GetVehicleDetailsById(VehicleDetails.Id);
                //if (ce.IsVehicleDetailsExists == true)
                //{
                bool result = VehicleDetailsEntityManager.UpdateVehicleDetails(VehicleDetails);
                if (result) {

                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_VehicleDetails");
                }
                this.VehicleDetails.VehicleDetailsInsertion = result;
                //}
                //else
                //{
                //    this.VehicleDetails.VehicleDetailsInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
