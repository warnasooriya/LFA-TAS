using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ITASTPAManagementService
    {
        TASTPAsResponseDto GetAllTPAs(SecurityContext securityContext, AuditContext auditContext);

        TASTPAsResponseDto GetTPADetailsByTPAId(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        bool SaveTPA(DataTransfer.Requests.TASTPARequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        bool UpdateTPA(DataTransfer.Requests.TASTPARequestDto TPAData, SecurityContext securityContext, AuditContext auditContext);

        TPAsResponseDto GetProductDisplayTPADetailsByTPAId(SecurityContext securityContext, AuditContext auditContext, Guid guid);

        ImageResponseDto GetTPAImageById(Guid TPAId, Guid ImageId, SecurityContext securityContext, AuditContext auditContext);


        CommodityCategoriesRespondDto GetCommodityCategoriesByCommodityTypeId(SecurityContext securityContext, AuditContext auditContext, Guid commodityTypeID, Guid tpaId);

        ProductResponseDto GetProductByProdId(SecurityContext securityContext, AuditContext auditContext, Guid productId, Guid tpaId);

        MakesResponseDto GetAllMakes(SecurityContext securityContext, AuditContext auditContext, Guid CommodityTypeId, Guid tpaId);

        ItemStatusesResponseDto GetItemStatuss(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        ModelesResponseDto GetModelesByMakeId(SecurityContext securityContext, AuditContext auditContext, Guid MakeId, Guid tpaId);

        CylinderCountsResponseDto GetCylinderCounts(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        VehicleBodyTypesResponseDto GetVehicleBodyTypes(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        FuelTypesResponseDto GetFuelTypes(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        VehicleAspirationTypesResponseDto GetVehicleAspirationTypes(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        TransmissionTypesResponseDto GetTransmissionTypes(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        EngineCapacitiesResponseDto GetEngineCapacities(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        DriveTypesResponseDto GetDriveTypes(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        VariantsResponseDto GetVariants(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        DealersRespondDto GetAllDealersByCountryId(SecurityContext securityContext, AuditContext auditContext, Guid countryId, Guid tpaId);

        DealerLocationsRespondDto GetAllDealerLocationsByDealerId(SecurityContext securityContext, AuditContext auditContext, Guid dealerId, Guid tpaId);

        CountriesResponseDto GetAllCountries(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);

        ExtensionTypesResponseDto GetExtensionTypesByDealerId(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, Guid dealerId, Guid modelId);

        ContractPricesResponseDto GetPrices(SecurityContext securityContext, AuditContext auditContext, Guid tpaId, Guid dealerId, Guid modelId, decimal dealerPrice, decimal itemPrice); //, Guid extensionTypeId

        TPAsResponseDto GetProductDisplayTPADetailsByTPAName(SecurityContext securityContext, AuditContext auditContext, string tpaName);


        string GetTPANameById(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);
        CountriesResponseDto GetAllCountriesThatHaveDealers(SecurityContext securityContext, AuditContext auditContext, Guid tpaId);
        VariantsResponseDto GetVariantsByModelId(SecurityContext context1, AuditContext context2, Guid modelId, Guid tpaId);
    }
}
