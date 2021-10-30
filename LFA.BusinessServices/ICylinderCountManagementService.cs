using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ICylinderCountManagementService
    {
        CylinderCountsResponseDto GetCylinderCounts(
            SecurityContext securityContext, 
            AuditContext auditContext);


        CylinderCountsResponseDto GetParentCylinderCounts(
            SecurityContext securityContext, 
            AuditContext auditContext);

        CylinderCountRequestDto AddCylinderCount(CylinderCountRequestDto cylinderCount,
            SecurityContext securityContext,
            AuditContext auditContext);


        CylinderCountResponseDto GetCylinderCountById(Guid CylinderCountId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CylinderCountRequestDto UpdateCylinderCount(CylinderCountRequestDto cylinderCount,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
