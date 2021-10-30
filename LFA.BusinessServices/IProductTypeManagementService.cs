using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IProductTypeManagementService
    {
        ProductTypesResponseDto GetProductTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        ProductTypeRequestDto AddProductType(ProductTypeRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);


        ProductTypeResponseDto GetProductTypeById(Guid ProductTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ProductTypeRequestDto UpdateProductType(ProductTypeRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);

    }
}
