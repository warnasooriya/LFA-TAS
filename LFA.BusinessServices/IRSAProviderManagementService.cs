using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IRSAProviderManagementService
    {
        #region Provider
        RSAProvideresResponseDto GetRSAProviders(
            SecurityContext securityContext, 
            AuditContext auditContext);

        RSAProviderRequestDto AddRSAProvider(RSAProviderRequestDto RSAProvider,
            SecurityContext securityContext,
            AuditContext auditContext);


        RSAProviderResponseDto GetRSAProviderById(Guid RSAProviderId,
            SecurityContext securityContext,
            AuditContext auditContext);

        RSAProviderRequestDto UpdateRSAProvider(RSAProviderRequestDto RSAProvider,
            SecurityContext securityContext,
            AuditContext auditContext);
        #endregion

        #region Anual Premium
        RSAAnualPremiumsResponseDto GetRSAAnualPremiums(
             SecurityContext securityContext,
             AuditContext auditContext);

        RSAAnualPremiumRequestDto AddRSAAnualPremium(RSAAnualPremiumRequestDto RSAProvider,
            SecurityContext securityContext,
            AuditContext auditContext);

        RSAAnualPremiumResponseDto GetRSAAnualPremiumById(Guid RSAProviderId,
            SecurityContext securityContext,
            AuditContext auditContext);

        RSAAnualPremiumRequestDto UpdateRSAAnualPremium(RSAAnualPremiumRequestDto RSAAnualPremium,
            SecurityContext securityContext,
           AuditContext auditContext);
        #endregion

    }
}
