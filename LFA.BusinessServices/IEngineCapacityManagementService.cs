using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IEngineCapacityManagementService
    {
        EngineCapacitiesResponseDto GetEngineCapacities(
            SecurityContext securityContext, 
            AuditContext auditContext);


        EngineCapacityRequestDto AddEngineCapacity(EngineCapacityRequestDto EngineCapacity,
            SecurityContext securityContext,
            AuditContext auditContext);


        EngineCapacityResponseDto GetEngineCapacityById(Guid EngineCapacityId,
            SecurityContext securityContext,
            AuditContext auditContext);

        EngineCapacityRequestDto UpdateEngineCapacity(EngineCapacityRequestDto EngineCapacity,
            SecurityContext securityContext,
            AuditContext auditContext);



        bool IsExsistingEngineCapacityByEngineCapacityNumber(Guid Id, decimal engineCapacityNumber, SecurityContext securityContext, AuditContext auditContext);
    }
}
