using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IEligibilityManagementService
    {
        EligibilitiesResponseDto GetEligibilitys(
            SecurityContext securityContext, 
            AuditContext auditContext);

        EligibilityRequestDto AddEligibility(EligibilityRequestDto Eligibility,
            SecurityContext securityContext,
            AuditContext auditContext);


        EligibilityResponseDto GetEligibilityById(Guid EligibilityId,
            SecurityContext securityContext,
            AuditContext auditContext);

        EligibilityRequestDto UpdateEligibility(EligibilityRequestDto Eligibility,
            SecurityContext securityContext,
            AuditContext auditContext);

    }
}
