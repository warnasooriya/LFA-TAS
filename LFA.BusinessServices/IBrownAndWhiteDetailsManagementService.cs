using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IBrownAndWhiteDetailsManagementService
    {
        BrownAndWhiteAllDetailsResponseDto GetBrownAndWhiteAllDetails(
            SecurityContext securityContext, 
            AuditContext auditContext);

        BrownAndWhiteDetailsRequestDto AddBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails,
            SecurityContext securityContext,
            AuditContext auditContext);


        BrownAndWhiteDetailsResponseDto GetBrownAndWhiteDetailsById(Guid BrownAndWhiteDetailsId,
            SecurityContext securityContext,
            AuditContext auditContext);

        BrownAndWhiteDetailsRequestDto UpdateBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails,
            SecurityContext securityContext,
            AuditContext auditContext);



        object GetAllItemsForSearchGrid(BnWSearchGridRequestDto BnWSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string ValidateDealerCurrency(Guid guid, SecurityContext securityContext, AuditContext auditContext);
    }
}
