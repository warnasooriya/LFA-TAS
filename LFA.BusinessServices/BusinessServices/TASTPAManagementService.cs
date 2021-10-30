using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    public class TASTPAManagementService : ITASTPAManagementService
    {
        public TASTPAsResponseDto GetAllTPAs(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            TASTPAsResponseDto result = null;
            TASTPARetrievalUnitOfWork uow = new TASTPARetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public TASTPAsResponseDto GetTPADetailsByTPAId(
          SecurityContext securityContext,
          AuditContext auditContext, Guid tpaId)
        {
            TASTPAsResponseDto result = null;
            TASTPARetrievalUnitOfWork uow = new TASTPARetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }


        public string GetTPANameById(SecurityContext securityContext, AuditContext auditContext, Guid tpaId)
        {
            string result = string.Empty;
            TPADisplayRetrievalUnitOfWork uow = new TPADisplayRetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                result = uow.tpaName;
            }
           
            return result;
        }

        public TPAsResponseDto GetProductDisplayTPADetailsByTPAId(
        SecurityContext securityContext,
        AuditContext auditContext, Guid tpaId)
        {
            TPAsResponseDto result = null;
            TPADisplayRetrievalUnitOfWork uow = new TPADisplayRetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public TPAsResponseDto GetProductDisplayTPADetailsByTPAName(
        SecurityContext securityContext,
        AuditContext auditContext, string tpaName)
        {
            TPAsResponseDto result = null;
            TPADisplayRetrievalByNameUnitOfWork uow = new TPADisplayRetrievalByNameUnitOfWork(tpaName);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public ImageResponseDto GetTPAImageById(Guid TPAId, Guid ImageId, SecurityContext securityContext, AuditContext auditContext)
        {
            //Guid ImageId = Guid.Empty;
            TPADisplayImageRetrievalUnitOfWork uow = new TPADisplayImageRetrievalUnitOfWork(TPAId);
            uow.imageId = ImageId;
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            ImageResponseDto iRd = uow.Result;
            //ImageId = uow.Result;
            return iRd;
        }


        public bool SaveTPA(TASTPARequestDto TPAData,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            bool result = false;
            TASTPAInsertionUnitOfWork uow = new TASTPAInsertionUnitOfWork(TPAData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool UpdateTPA(TASTPARequestDto TPAData,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            bool result = false;
            TASTPAUpdatingUnitOfWork uow = new TASTPAUpdatingUnitOfWork(TPAData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CommodityCategoriesRespondDto GetCommodityCategoriesByCommodityTypeId(SecurityContext securityContext,
    AuditContext auditContext, Guid commodityTypeID, Guid tpaId)
        {
            CommodityCategoriesRespondDto result = null;
            TASCommodityCategoriesRetrievalUnitOfWork uow = new TASCommodityCategoriesRetrievalUnitOfWork(commodityTypeID, tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductResponseDto GetProductByProdId(SecurityContext securityContext,
                                                                    AuditContext auditContext, Guid productId, Guid tpaId)
        {
            ProductResponseDto result = null;
            TASCommodityTypeByProdIdRetrievalUnitOfWork uow = new TASCommodityTypeByProdIdRetrievalUnitOfWork(productId, tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public MakesResponseDto GetAllMakes(
            SecurityContext securityContext,
            AuditContext auditContext, Guid CommodityTypeId, Guid tpaId)
        {
            MakesResponseDto result = null;
            TASMakesRetrievalUnitOfWork uow = new TASMakesRetrievalUnitOfWork(CommodityTypeId,tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;

            return result;
        }

        public ItemStatusesResponseDto GetItemStatuss(
           SecurityContext securityContext,
           AuditContext auditContext,Guid tpaId)
        {
            ItemStatusesResponseDto result = null;

            TASItemStatusesRetrievalUnitOfWork uow = new TASItemStatusesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ModelesResponseDto GetModelesByMakeId(
    SecurityContext securityContext,
    AuditContext auditContext,Guid makeId,Guid tpaId)
        {
            ModelesResponseDto result = null;

            TASModelesRetrievalUnitOfWork uow = new TASModelesRetrievalUnitOfWork(makeId,tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CylinderCountsResponseDto GetCylinderCounts(
            SecurityContext securityContext,
            AuditContext auditContext, Guid tpaId)
        {
            CylinderCountsResponseDto result = null;

            TASCylinderCountsRetrievalUnitOfWork uow = new TASCylinderCountsRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }


        public VehicleBodyTypesResponseDto GetVehicleBodyTypes(
           SecurityContext securityContext,
           AuditContext auditContext, Guid tpaId)
        {
            VehicleBodyTypesResponseDto result = null;

            TASVehicleBodyTypesRetrievalUnitOfWork uow = new TASVehicleBodyTypesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }



        public FuelTypesResponseDto GetFuelTypes(
           SecurityContext securityContext,
           AuditContext auditContext, Guid tpaId)
        {
            FuelTypesResponseDto result = null;

            TASFuelTypesRetrievalUnitOfWork uow = new TASFuelTypesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleAspirationTypesResponseDto GetVehicleAspirationTypes(
           SecurityContext securityContext,
           AuditContext auditContext, Guid tpaId)
        {
            VehicleAspirationTypesResponseDto result = null;

            TASVehicleAspirationTypesRetrievalUnitOfWork uow = new TASVehicleAspirationTypesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.ResultVehicleAspirationType;

            return result;
        }

        public TransmissionTypesResponseDto GetTransmissionTypes(
            SecurityContext securityContext,
            AuditContext auditContext, Guid tpaId)
        {
            TransmissionTypesResponseDto result = null;

            TASTransmissionTypesRetrievalUnitOfWork uow = new TASTransmissionTypesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }


        public EngineCapacitiesResponseDto GetEngineCapacities(
           SecurityContext securityContext,
           AuditContext auditContext, Guid tpaId)
        {
            EngineCapacitiesResponseDto result = null;

            TASEngineCapacitiesRetrievalUnitOfWork uow = new TASEngineCapacitiesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public DriveTypesResponseDto GetDriveTypes(
           SecurityContext securityContext,
           AuditContext auditContext, Guid tpaId)
        {
            DriveTypesResponseDto result = null;

            TASDriveTypesRetrievalUnitOfWork uow = new TASDriveTypesRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }


        public VariantsResponseDto GetVariants(
            SecurityContext securityContext,
            AuditContext auditContext, Guid tpaId)
        {
            VariantsResponseDto result = null;

            TASVariantsRetrievalUnitOfWork uow = new TASVariantsRetrievalUnitOfWork(tpaId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CountriesResponseDto GetAllCountries(
            SecurityContext securityContext,
            AuditContext auditContext, Guid tpaId)
        {
            CountriesResponseDto result = null;
            TASCountryRetrievalUnitOfWork uow = new TASCountryRetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public CountriesResponseDto GetAllCountriesThatHaveDealers(SecurityContext securityContext, AuditContext auditContext, Guid tpaId)
        {
            CountriesResponseDto result = null;
            TASDealerAssignedCountryRetrievalUnitOfWork uow = new TASDealerAssignedCountryRetrievalUnitOfWork(tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealersRespondDto GetAllDealersByCountryId(SecurityContext securityContext, AuditContext auditContext,Guid countryId, Guid tpaId)
        {
            DealersRespondDto result = null;
            TASDealersRetrievalUnitOfWork uow = new TASDealersRetrievalUnitOfWork(countryId,tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerLocationsRespondDto GetAllDealerLocationsByDealerId(SecurityContext securityContext, AuditContext auditContext, Guid dealerId, Guid tpaId)
        {
            DealerLocationsRespondDto result = null;
            TASDealerLocationsRetrievalUnitOfWork uow = new TASDealerLocationsRetrievalUnitOfWork(dealerId, tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ExtensionTypesResponseDto GetExtensionTypesByDealerId(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, Guid dealerId, Guid modelId)
        {
            ExtensionTypesResponseDto result = null;

            TASContractExtensionsRetrievalUnitOfWork uow = new TASContractExtensionsRetrievalUnitOfWork(modelId, dealerId, tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public ContractPricesResponseDto GetPrices(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, Guid dealerId, Guid modelId, decimal dealerPrice, decimal itemPrice) // , Guid extensionTypeId
        {
            ContractPricesResponseDto result = null;

            TASContractPricesRetrievalUnitOfWork uow = new TASContractPricesRetrievalUnitOfWork(modelId, dealerId, tpaId, dealerPrice, itemPrice); // , extensionTypeId
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public VariantsResponseDto GetVariantsByModelId(SecurityContext securityContext, AuditContext auditContext, Guid modelId, Guid tpaId)
        {
            VariantsResponseDto responseDto = new VariantsResponseDto();
            TASGetVariantsByModelIdUnitOfWork uow = new TASGetVariantsByModelIdUnitOfWork(modelId, tpaId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            responseDto = uow.Result;
            return responseDto;
        }
    }
}
