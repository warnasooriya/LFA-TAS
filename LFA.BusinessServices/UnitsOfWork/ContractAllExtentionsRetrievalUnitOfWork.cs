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
    internal sealed class ContractAllExtentionsRetrievalUnitOfWork : UnitOfWork
    {
       

        public ContractExtensionsResponseDto Result
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


                //ContractEntityManager contratEm = new ContractEntityManager();
                //contratEm.FullContractDetailsById()

                ContractExtensionsEntityManager ContractExtensionsEntityManager = new ContractExtensionsEntityManager();
                List<ContractExtensions> ContractExtensionsEntities = ContractExtensionsEntityManager.GetContractExtensions();

				
                ContractExtensionsResponseDto result = new ContractExtensionsResponseDto();
                result.ContractExtensions = new List<ContractExtensionResponseDto>();
                foreach (var ContractExtensions in ContractExtensionsEntities)
                {
                    ContractExtensionResponseDto pr = new ContractExtensionResponseDto();

                    //List<ContractExtensionsPremiumAddonResponseDto> addons = new List<ContractExtensionsPremiumAddonResponseDto>();
                    //foreach (var item in ContractExtensions.PremiumAddones)
                    //{
                    //    addons.Add(new ContractExtensionsPremiumAddonResponseDto()
                    //    {
                    //        Id = item.Id,
                    //        ContractExtensionId = item.ContractExtensionId,
                    //        PremiumAddonTypeId = item.PremiumAddonTypeId,
                    //        Value = item.Value
                    //    });
                    //}

                    pr.Id = ContractExtensions.Id;
                    pr.ContractInsuanceLimitationId = ContractExtensions.ContractInsuanceLimitationId;
                  //  pr.AttributeSpecification = ContractExtensions.AttributeSpecification;
                  //  pr.ContractId =ContractExtensions.ContractId;
                   // pr.ExtensionTypeId = ContractExtensions.ExtensionTypeId;           
                    //pr.EngineCapacities = ContractExtensions.EngineCapacities;                   
                    //pr.CylinderCounts = ContractExtensions.CylinderCounts; 
                    //pr.Makes = ContractExtensions.Makes;                   
                  //  //pr.Modeles = ContractExtensions.Modeles;
                  //  pr.ManufacturerWarrantyGross = ContractExtensions.ManufacturerWarrantyGross;
                  //  pr.ManufacturerWarrantyNett = ContractExtensions.ManufacturerWarrantyNett;

                  //  pr.WarrantyTypeId = ContractExtensions.WarrantyTypeId;
                  //  pr.IsCustAvailableGross = ContractExtensions.IsCustAvailableGross;
                  //  pr.IsCustAvailableNett = ContractExtensions.IsCustAvailableNett;

                  //  pr.MaxGross = ContractExtensions.MaxGross;
                  //  pr.MaxNett = ContractExtensions.MaxNett;
                  //  pr.MinGross = ContractExtensions.MinGross;
                  //  pr.MinNett = ContractExtensions.MinNett;

                  ////  pr.PremiumAddones = addons;
                  //  pr.PremiumBasedOnIdGross = ContractExtensions.PremiumBasedOnIdGross;
                  //  pr.PremiumBasedOnIdNett = ContractExtensions.PremiumBasedOnIdNett;

                  //  pr.PremiumTotal = ContractExtensions.PremiumTotal;
                  //  pr.GrossPremium = ContractExtensions.GrossPremium;
                  //  pr.PremiumCurrencyId = ContractExtensions.PremiumCurrencyId;
                    pr.EntryDateTime = ContractExtensions.EntryDateTime;
                    pr.EntryUser = ContractExtensions.EntryUser;
                    pr.RSAProviderId = ContractExtensions.RSAProviderId;
                    pr.Rate = ContractExtensions.Rate;
                    pr.RegionId = ContractExtensions.RegionId;
                    pr.AttributeSpecification = ContractExtensions.AttributeSpecification;
                    
                    
                    
					
                    //need to write other fields
                    result.ContractExtensions.Add(pr);
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
