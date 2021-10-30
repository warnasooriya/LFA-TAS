using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IPremiumAddonTypeManagementService
    {
        PremiumAddonTypesResponseDto GetPremiumAddonTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        PremiumAddonTypesResponseDto GetPremiumAddonTypes(Guid CommodityTypeId,
          SecurityContext securityContext,
          AuditContext auditContext);

        PremiumAddonTypeRequestDto AddPremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType,
            SecurityContext securityContext,
            AuditContext auditContext);

        PremiumAddonTypeResponseDto GetPremiumAddonTypeById(Guid PremiumAddonTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        PremiumAddonTypeRequestDto UpdatePremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType,
            SecurityContext securityContext,
            AuditContext auditContext);

        PremiumAddonTypesResponseDto GetAllPremiumAddonType(SecurityContext securityContext, AuditContext auditContext);
    }
}
