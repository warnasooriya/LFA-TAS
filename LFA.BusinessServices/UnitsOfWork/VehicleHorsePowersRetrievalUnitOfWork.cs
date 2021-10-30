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
	internal sealed class VehicleHorsePowersRetrievalUnitOfWork : UnitOfWork
	{


		public VehicleHorsePowersResponseDto ResultVehicleHorsePower
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
				VehicleHorsePowerEntityManager VehicleHorsePowerEntityManager = new VehicleHorsePowerEntityManager();
				List<VehicleHorsePower> VehicleHorsePowerEntities = VehicleHorsePowerEntityManager.GetVehicleHorsePowers();

				
				VehicleHorsePowersResponseDto result = new VehicleHorsePowersResponseDto();
				result.VehicleHorsePowers = new List<VehicleHorsePowerResponseDto>();
				foreach (var VehicleHorsePower in VehicleHorsePowerEntities)
				{
					VehicleHorsePowerResponseDto pr = new VehicleHorsePowerResponseDto();

					pr.Id = VehicleHorsePower.Id;
					pr.HorsePower = VehicleHorsePower.HorsePower;
					pr.EntryDateTime = VehicleHorsePower.EntryDateTime;
					pr.EntryUser = VehicleHorsePower.EntryUser;
					
					//need to write other fields
					result.VehicleHorsePowers.Add(pr);
				}
				this.ResultVehicleHorsePower = result;
			}
			finally
			{
				EntitySessionManager.CloseSession();
			}
		}

	}
}
