using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IOtherItemManagementService
    {

        object GetAllItemsForSearchGrid(OtherItemSearchGridRequestDto OtherItemSearchGridRequestDto, 
            DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);
        BrownAndWhiteDetailsResponseDto GetOtherItemDetailsById(Guid itemId, SecurityContext context1, AuditContext context2);

        OtherItemDetailsResponseDto GetOtherDetailsById(Guid OtherItemId, SecurityContext securityContext, AuditContext auditContext);
    }
}
