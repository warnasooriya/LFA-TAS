using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
	internal sealed class VehicleKiloWattsRetrievalUnitOfWork : UnitOfWork
	{


		public VehicleKiloWattsResponseDto ResultVehicleKiloWatt
		{
			get;
			private set;
		}
		public override bool PreExecute()
		{
			try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
				VehicleKiloWattEntityManager VehicleKiloWattEntityManager = new VehicleKiloWattEntityManager();
				List<VehicleKiloWatt> VehicleKiloWattEntities = VehicleKiloWattEntityManager.GetVehicleKiloWatts();

				
				VehicleKiloWattsResponseDto result = new VehicleKiloWattsResponseDto();
				result.VehicleKiloWatts = new List<VehicleKiloWattResponseDto>();
				foreach (var VehicleKiloWatt in VehicleKiloWattEntities)
				{
					VehicleKiloWattResponseDto pr = new VehicleKiloWattResponseDto();

					pr.Id = VehicleKiloWatt.Id;
					pr.KiloWatt = VehicleKiloWatt.KiloWatt;
					pr.EntryDateTime = VehicleKiloWatt.EntryDateTime;
					pr.EntryUser = VehicleKiloWatt.EntryUser;
					
					//need to write other fields
					result.VehicleKiloWatts.Add(pr);
				}
				this.ResultVehicleKiloWatt = result;
			}
			finally
			{
				EntitySessionManager.CloseSession();
			}
		}

	}
}
