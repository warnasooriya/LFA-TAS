using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IRegionManagementService
    {
        RegionesResponseDto GetRegions(
            SecurityContext securityContext, 
            AuditContext auditContext);

        RegionRequestDto AddRegion(RegionRequestDto Region,
            SecurityContext securityContext,
            AuditContext auditContext);


        RegionResponseDto GetRegionById(Guid RegionId,
            SecurityContext securityContext,
            AuditContext auditContext);

        RegionRequestDto UpdateRegion(RegionRequestDto Region,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
