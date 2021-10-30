using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IVariantManagementService
    {
        VariantsResponseDto GetVariants(
            SecurityContext securityContext,
            AuditContext auditContext);

        VariantRequestDto AddVariant(VariantRequestDto Variant,
            SecurityContext securityContext,
            AuditContext auditContext);

        VariantResponseDto GetVariantById(Guid VariantId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VariantRequestDto UpdateVariant(VariantRequestDto Variant,
            SecurityContext securityContext,
            AuditContext auditContext);



        VariantsResponseDto GetAllVariantsByModelIds(System.Collections.Generic.List<Guid> VariantIdList, SecurityContext securityContext, AuditContext auditContext);
    }
}
