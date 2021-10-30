using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class ProductTypeManagementService : IProductTypeManagementService
    {

        public ProductTypesResponseDto GetProductTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            ProductTypesResponseDto result = null;

            ProductTypesRetrievalUnitOfWork uow = new ProductTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ProductTypeRequestDto AddProductType(ProductTypeRequestDto ProductType, SecurityContext securityContext,
            AuditContext auditContext) {
                ProductTypeRequestDto result = new ProductTypeRequestDto();
                ProductTypeInsertionUnitOfWork uow = new ProductTypeInsertionUnitOfWork(ProductType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ProductTypeInsertion = uow.ProductType.ProductTypeInsertion;
                return result;
        }


        public ProductTypeResponseDto GetProductTypeById(Guid ProductTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ProductTypeResponseDto result = new ProductTypeResponseDto();

            ProductTypeRetrievalUnitOfWork uow = new ProductTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ProductTypeId = ProductTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ProductTypeRequestDto UpdateProductType(ProductTypeRequestDto ProductType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            ProductTypeRequestDto result = new ProductTypeRequestDto();
            ProductTypeUpdationUnitOfWork uow = new ProductTypeUpdationUnitOfWork(ProductType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ProductTypeInsertion = uow.ProductType.ProductTypeInsertion;
            return result;
        }

       
       
    }
}
