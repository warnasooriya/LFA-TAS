using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IExtensionTypeManagementService
    {
        ExtensionTypesResponseDto GetExtensionTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        ExtensionTypeRequestDto AddExtensionType(ExtensionTypeRequestDto ExtensionType,
            SecurityContext securityContext,
            AuditContext auditContext);


        ExtensionTypeResponseDto GetExtensionTypeById(Guid ExtensionTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ExtensionTypeRequestDto UpdateExtensionType(ExtensionTypeRequestDto ExtensionType,
            SecurityContext securityContext,
            AuditContext auditContext);
    }
}
