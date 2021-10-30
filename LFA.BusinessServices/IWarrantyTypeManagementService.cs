using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IWarrantyTypeManagementService
    {
        WarrantyTypesResponseDto GetWarrantyTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        WarrantyTypeRequestDto AddWarrantyType(WarrantyTypeRequestDto WarrantyType,
            SecurityContext securityContext,
            AuditContext auditContext);


        WarrantyTypeResponseDto GetWarrantyTypeById(Guid WarrantyTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        WarrantyTypeRequestDto UpdateWarrantyType(WarrantyTypeRequestDto WarrantyType,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
