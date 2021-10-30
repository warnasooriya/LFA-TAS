using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public  interface  IYellowGoodManagementService
    {

        object GetAllItemsForSearchGrid(YellowGoodSearchGridRequestDto YellowGoodSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);
        BrownAndWhiteDetailsResponseDto GetYellowGoodDetaisById(Guid PolicyBundleId, SecurityContext securityContext, AuditContext auditContext);
    }
}
