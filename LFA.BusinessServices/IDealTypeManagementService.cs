using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IDealTypeManagementService
    {
        DealTypesResponseDto GetDealTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        DealTypeRequestDto AddDealType(DealTypeRequestDto DealType,
            SecurityContext securityContext,
            AuditContext auditContext);


        DealTypeResponseDto GetDealTypeById(Guid DealTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealTypeRequestDto UpdateDealType(DealTypeRequestDto DealType,
            SecurityContext securityContext,
            AuditContext auditContext);

    }
}
