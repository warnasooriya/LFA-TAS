using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IProductManagementService
    {
        ProductsResponseDto GetProducts(
            SecurityContext securityContext,
            AuditContext auditContext);

        ProductsResponseDto GetProductWithBundleData(
           SecurityContext securityContext,
           AuditContext auditContext);


        ProductsResponseDto GetParentProducts(
            SecurityContext securityContext,
            AuditContext auditContext);

        ProductRequestDto AddProduct(ProductRequestDto product,
            SecurityContext securityContext,
            AuditContext auditContext);


        ProductResponseDto GetProductById(Guid ProductId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ProductRequestDto UpdateProduct(ProductRequestDto product,
            SecurityContext securityContext,
            AuditContext auditContext);

        TPAProductsResponseDto GetProductsByTPA(Guid TPAId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ProductsResponseDto GetChildProductsbyParentId(Guid ParentProductId,
            SecurityContext securityContext,
            AuditContext auditContext);



        bool IsExsistingProductName(Guid id, string productName, SecurityContext securityContext, AuditContext auditContext);
        object GetDocumentTypesByPageName(string pageName, Guid tpaId);
        object GetAllProductsByCommodityTypeId(Guid commodityTypeId, SecurityContext context1, AuditContext context2);
    }
}
