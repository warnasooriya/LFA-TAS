using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface INRPCommissionTypesManagementService
    {
        NRPCommissionTypessResponseDto GetNRPCommissionTypess(
            SecurityContext securityContext, 
            AuditContext auditContext);

        NRPCommissionTypesRequestDto AddNRPCommissionTypes(NRPCommissionTypesRequestDto NRPCommissionTypes,
            SecurityContext securityContext,
            AuditContext auditContext);

        NRPCommissionTypesResponseDto GetNRPCommissionTypesById(Guid NRPCommissionTypesId,
            SecurityContext securityContext,
            AuditContext auditContext);

        NRPCommissionTypesRequestDto UpdateNRPCommissionTypes(NRPCommissionTypesRequestDto NRPCommissionTypes,
            SecurityContext securityContext,
            AuditContext auditContext);

        NRPCommissionContractMappingsResponseDto GetNRPCommissionContractMappings(
          SecurityContext securityContext,
          AuditContext auditContext);

        NRPCommissionContractMappingRequestDto AddNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping,
            SecurityContext securityContext,
            AuditContext auditContext);


        NRPCommissionContractMappingResponseDto GetNRPCommissionContractMappingById(Guid NRPCommissionContractMappingId,
            SecurityContext securityContext,
            AuditContext auditContext);

        NRPCommissionContractMappingRequestDto UpdateNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
