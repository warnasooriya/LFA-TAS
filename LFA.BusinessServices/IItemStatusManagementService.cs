using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IItemStatusManagementService
    {
        ItemStatusesResponseDto GetItemStatuss(
            SecurityContext securityContext, 
            AuditContext auditContext);

        ItemStatusRequestDto AddItemStatus(ItemStatusRequestDto ItemStatus,
            SecurityContext securityContext,
            AuditContext auditContext);


        ItemStatusResponseDto GetItemStatusById(Guid ItemStatusId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ItemStatusRequestDto UpdateItemStatus(ItemStatusRequestDto ItemStatus,
            SecurityContext securityContext,
            AuditContext auditContext);


        bool IsExsistingItemStatusByStatus(Guid Id, string status, SecurityContext securityContext, AuditContext auditContext);
    }
}
