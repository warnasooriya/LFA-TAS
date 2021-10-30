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
    internal sealed class ManufacturerWarrantiesRetrievalUnitOfWork : UnitOfWork
    {
       

        public ManufacturerWarrantiesResponseDto Result
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
                ManufacturerWarrantyEntityManager ManufacturerWarrantyEntityManager = new ManufacturerWarrantyEntityManager();
                List<ManufacturerWarranty> ManufacturerWarrantyEntities = ManufacturerWarrantyEntityManager.GetManufacturerWarranties();

				
                ManufacturerWarrantiesResponseDto result = new ManufacturerWarrantiesResponseDto();
                result.ManufacturerWarranties = new List<ManufacturerWarrantyResponseDto>();
                foreach (var ManufacturerWarranty in ManufacturerWarrantyEntities)
                {
                    ManufacturerWarrantyResponseDto pr = new ManufacturerWarrantyResponseDto();

                    pr.Id = ManufacturerWarranty.Id;
                    pr.MakeId = ManufacturerWarranty.MakeId;
                //    pr.ModelId = ManufacturerWarranty.ModelId;
                  //  pr.CountryId = ManufacturerWarranty.CountryId;
                    pr.ApplicableFrom = ManufacturerWarranty.ApplicableFrom;
                    pr.WarrantyKm = ManufacturerWarranty.IsUnlimited?"Unlimited":ManufacturerWarranty.WarrantyKm.ToString();
                    pr.WarrantyName = ManufacturerWarranty.WarrantyName;
                    pr.WarrantyMonths = ManufacturerWarranty.WarrantyMonths;
                    pr.EntryDateTime = ManufacturerWarranty.EntryDateTime;
                    pr.EntryUser = ManufacturerWarranty.EntryUser;
                    pr.IsUnlimited = ManufacturerWarranty.IsUnlimited;
                    //need to write other fields
                    result.ManufacturerWarranties.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
