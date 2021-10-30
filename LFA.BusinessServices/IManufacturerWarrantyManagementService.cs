using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IManufacturerWarrantyManagementService
    {
        ManufacturerWarrantiesResponseDto GetManufacturerWarranties(
            SecurityContext securityContext, 
            AuditContext auditContext);

        ManufacturerWarrantyRequestDto AddManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty,
            SecurityContext securityContext,
            AuditContext auditContext);


        ManufacturerWarrantyResponseDto GetManufacturerWarrantyById(Guid ManufacturerWarrantyId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ManufacturerWarrantyRequestDto UpdateManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty,
            SecurityContext securityContext,
            AuditContext auditContext);


        
       // object SearchManufacturerWarrantySchemes(ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequest, SecurityContext securityContext, AuditContext auditContext);

        //object SearchManufacturerWarrantySchemes(ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequest, SecurityContext securityContext, AuditContext auditContext);

        object SearchManufacturerWarrantySchemes(ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequest, SecurityContext securityContext, AuditContext auditContext);

        ManufacturerWarrantyResponseDto GetManufacturerDetailsByContractId(Guid ContractId, Guid ModelId, Guid MakeId, SecurityContext securityContext, AuditContext auditContext);
        object GetManufacturerDetailsByCountryId(Guid CountryId, Guid ModelId, Guid MakeId, SecurityContext securityContext, AuditContext auditContext);

    }
}
