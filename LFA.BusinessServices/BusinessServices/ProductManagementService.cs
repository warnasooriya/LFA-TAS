using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ProductManagementService : IProductManagementService
    {

        public ProductsResponseDto GetProducts(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductsResponseDto result = null;
            ProductsRetrievalUnitOfWork uow = new ProductsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductsResponseDto GetProductWithBundleData(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductsResponseDto result = null;
            ProductsRetrievalUnitOfWork uow = new ProductsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductsResponseDto GetParentProducts(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductsResponseDto result = null;
            ProductsRetrievalUnitOfWork uow = new ProductsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.isParentOnly = true;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductsResponseDto GetChildProductsbyParentId(Guid ParentProductId,
        SecurityContext securityContext,
        AuditContext auditContext)
        {
            ProductsResponseDto result = null;
            ProductsRetrievalUnitOfWork uow = new ProductsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.isChildOnly = true;
            uow.ParentID = ParentProductId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductRequestDto AddProduct(
            ProductRequestDto Product,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductRequestDto result = new ProductRequestDto();
            ProductInsertionUnitOfWork uow = new ProductInsertionUnitOfWork(Product);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ProductInsertion = uow.Product.ProductInsertion;
            return result;
        }

        public ProductResponseDto GetProductById(
            Guid ProductId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductResponseDto result = new ProductResponseDto();
            ProductRetrievalUnitOfWork uow = new ProductRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.productId = ProductId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductRequestDto UpdateProduct(
            ProductRequestDto Product,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ProductRequestDto result = new ProductRequestDto();
            ProductUpdationUnitOfWork uow = new ProductUpdationUnitOfWork(Product);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ProductInsertion = uow.Product.ProductInsertion;
            return result;
        }

        public TPAProductsResponseDto GetProductsByTPA(
            Guid TPAId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            TPAProductsResponseDto result = null;
            TPAProductsRetrievalUnitOfWork uow = new TPAProductsRetrievalUnitOfWork(TPAId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public bool IsExsistingProductName(Guid id, string productName,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool Result = false;
            IsExsistingProductNameUnitOfWorks uow = new IsExsistingProductNameUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ProductName = productName;
            uow.Discription = "";
            uow.Id = id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }

        public object GetDocumentTypesByPageName(string pageName, Guid tpaId)
        {
            object result = new object();
            GetDocumentTypesByPageNameUnitOfWork uow = new GetDocumentTypesByPageNameUnitOfWork(pageName, tpaId);
            if (uow.PreExecute()) { uow.Execute(); }
            result = uow.Result;
            return result;
        }

        public object GetAllProductsByCommodityTypeId(Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            ProductsRetrivlByCommodityTypeIdUnitOfWork uow = new ProductsRetrivlByCommodityTypeIdUnitOfWork(CommodityTypeId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }
    }
}
