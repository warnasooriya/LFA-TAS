using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IPremiumBasedOnManagementService
    {
        PremiumBasedOnesResponseDto GetPremiumBasedOns(
            SecurityContext securityContext, 
            AuditContext auditContext);

        PremiumBasedOnRequestDto AddPremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn,
            SecurityContext securityContext,
            AuditContext auditContext);


        PremiumBasedOnResponseDto GetPremiumBasedOnById(Guid PremiumBasedOnId,
            SecurityContext securityContext,
            AuditContext auditContext);

        PremiumBasedOnRequestDto UpdatePremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn,
            SecurityContext securityContext,
            AuditContext auditContext);

    }
}
